using App.Application.BulkColumnMapping;
using App.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace App.Infrastructure
{
    public sealed class BulkUpsertService : IBulkUpsertService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<BulkUpsertService> _logger;

        public BulkUpsertService(IConfiguration configuration,ILogger<BulkUpsertService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<BulkUpsertResult> BulkUpsertAsync<T>(IEnumerable<T> data,string tableName,IReadOnlyCollection<BulkColumnMapping<T>> mappings,
            int batchSize = 5000,
            CancellationToken cancellationToken = default)
        {
            var result = new BulkUpsertResult();
            try
            {
                if (data == null)
                    throw new ArgumentNullException(nameof(data));

                if (mappings == null || mappings.Count == 0)
                    throw new ArgumentException(
                        "At least one column mapping is required.",
                        nameof(mappings));

                if (batchSize <= 0)
                    throw new ArgumentOutOfRangeException(
                        nameof(batchSize));

                ValidateIdentifier(tableName);

                ValidateMappings(mappings);

                var keyMapping = mappings.Where(x => x.IsKey).ToList();

                if (keyMapping.Count == 0)
                {
                    throw new InvalidOperationException(
                        "At least one key column is required.");
                }

                var records = data.ToList();

                result = new BulkUpsertResult
                {
                    TotalProcessed = records.Count
                };

                if (records.Count == 0)
                {
                    _logger.LogInformation(
                        "Bulk upsert skipped. Table: {TableName}. No records.",
                        tableName);

                    return result;
                }

                var connectionString = _configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new InvalidOperationException(
                        "DefaultConnection connection string was not found.");
                }

                await using var connection =
                    new SqlConnection(connectionString);

                await connection.OpenAsync(cancellationToken);

                _logger.LogInformation(
                    "Bulk upsert started. Table: {TableName}, " +
                    "Records: {RecordCount}, BatchSize: {BatchSize}",
                    tableName,
                    records.Count,
                    batchSize);

                for (int offset = 0; offset < records.Count; offset += batchSize)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var batch = records.Skip(offset).Take(batchSize).ToList();
                    var batchNumber = (offset / batchSize) + 1;

                    var batchResult = await BulkUpsertBatchAsync(
                            connection,
                            tableName,
                            mappings,
                            keyMapping,
                            batch,
                            batchNumber,
                            cancellationToken);

                    result.TotalInserted += batchResult.Inserted;
                    result.TotalUpdated += batchResult.Updated;
                    result.BatchCount++;

                    _logger.LogInformation(
                        "Bulk batch completed. Table: {TableName}, " +
                        "Batch: {BatchNumber}, " +
                        "BatchSize: {BatchSize}, " +
                        "Inserted: {Inserted}, " +
                        "Updated: {Updated}, " +
                        "Processed: {Processed}/{Total}",
                        tableName,
                        batchNumber,
                        batch.Count,
                        batchResult.Inserted,
                        batchResult.Updated,
                        Math.Min(
                            offset + batch.Count,
                            records.Count),
                        records.Count);
                }

                _logger.LogInformation("Bulk upsert completed. Table: {TableName}, " +
                    "Total: {Total}, Inserted: {Inserted}, Updated: {Updated}",
                    tableName, result.TotalProcessed, result.TotalInserted, result.TotalUpdated);
            }catch(Exception ex)
            {
                _logger.LogError(ex, "Bulk upsert failed. Table: {TableName}", tableName);
                throw;
            }
            return result;
        }

        private async Task<BatchResult> BulkUpsertBatchAsync<T>(
            SqlConnection connection,
            string tableName,
            IReadOnlyCollection<BulkColumnMapping<T>> mappings,
            IReadOnlyCollection<BulkColumnMapping<T>> keyMappings,
            List<T> batch,
            int batchNumber,
            CancellationToken cancellationToken)
        {
            await using var transaction =
                (SqlTransaction)await connection.BeginTransactionAsync(
                    IsolationLevel.ReadCommitted,
                    cancellationToken);

            try
            {
                // -----------------------------------------
                // 1. Create temporary staging table
                // -----------------------------------------

                var stagingTableName =$"#BulkStage_{Guid.NewGuid():N}";
                var createSql =BuildCreateStagingSql(tableName,stagingTableName,mappings);
                await ExecuteNonQueryAsync(connection, transaction,createSql,cancellationToken);

                // -----------------------------------------
                // 2. Create DataTable
                // -----------------------------------------

                var dataTable =CreateDataTable(mappings);

                foreach (var item in batch)
                {
                    var row =dataTable.NewRow();

                    foreach (var mapping in mappings)
                    {
                        var value =mapping.ValueSelector(item);
                        row[mapping.ColumnName] = ConvertToDbValue(value, mapping.DataType);
                    }
                    dataTable.Rows.Add(row);
                }

                // -----------------------------------------
                // 3. Remove duplicate keys from batch
                // -----------------------------------------

                dataTable = RemoveDuplicateKeys(dataTable, keyMappings);

                if (dataTable.Rows.Count == 0)
                {
                    await transaction.CommitAsync(cancellationToken);
                    return new BatchResult();
                }

                // -----------------------------------------
                // 4. SqlBulkCopy
                // -----------------------------------------

                using var bulkCopy =
                    new SqlBulkCopy(
                        connection,
                        SqlBulkCopyOptions.KeepNulls |
                        SqlBulkCopyOptions.CheckConstraints,
                        transaction)
                    {
                        DestinationTableName =
                            stagingTableName,

                        BatchSize = 5000,

                        BulkCopyTimeout = 600,

                        EnableStreaming = true
                    };

                foreach (var mapping in mappings)
                {
                    bulkCopy.ColumnMappings.Add(
                        mapping.ColumnName,
                        mapping.ColumnName);
                }

                await bulkCopy.WriteToServerAsync(
                    dataTable,
                    cancellationToken);

                // -----------------------------------------
                // 5. UPDATE existing records
                // -----------------------------------------

                var updateSql =
                    BuildUpdateSql(
                        tableName,
                        stagingTableName,
                        mappings,
                        keyMappings);

                int updated =
                    await ExecuteNonQueryAsync(
                        connection,
                        transaction,
                        updateSql,
                        cancellationToken);

                // -----------------------------------------
                // 6. INSERT new records
                // -----------------------------------------

                var insertSql =
                    BuildInsertSql(
                        tableName,
                        stagingTableName,
                        mappings,
                        keyMappings);

                int inserted =await ExecuteNonQueryAsync(connection,transaction,insertSql,cancellationToken);

                // -----------------------------------------
                // 7. Commit
                // -----------------------------------------

                await transaction.CommitAsync(cancellationToken);

                return new BatchResult
                {
                    Inserted = inserted,
                    Updated = updated
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);

                _logger.LogError(
                    ex,
                    "Bulk batch failed. Table: {TableName}, " +
                    "BatchNumber: {BatchNumber}, " +
                    "BatchSize: {BatchSize}",
                    tableName,
                    batchNumber,
                    batch.Count);

                throw;
            }
        }

        private static DataTable CreateDataTable<T>(
            IReadOnlyCollection<BulkColumnMapping<T>> mappings)
        {
            var table = new DataTable();

            foreach (var mapping in mappings)
            {
                table.Columns.Add(mapping.ColumnName,Nullable.GetUnderlyingType(mapping.DataType)?? mapping.DataType);
            }

            return table;
        }

        private static object ConvertToDbValue(object? value,Type targetType)
        {
            if (value == null)
                return DBNull.Value;

            var underlyingType =
                Nullable.GetUnderlyingType(targetType)
                ?? targetType;

            if (value.GetType() == underlyingType)
                return value;

            if (underlyingType == typeof(Guid))
            {
                if (value is Guid guid)
                    return guid;

                return Guid.Parse(
                    value.ToString()!);
            }

            if (underlyingType == typeof(DateTime))
            {
                if (value is DateTime dateTime)
                    return dateTime;

                return Convert.ToDateTime(value);
            }

            if (underlyingType == typeof(decimal))
            {
                return Convert.ToDecimal(value);
            }

            if (underlyingType == typeof(int))
            {
                return Convert.ToInt32(value);
            }

            if (underlyingType == typeof(long))
            {
                return Convert.ToInt64(value);
            }

            if (underlyingType == typeof(bool))
            {
                return Convert.ToBoolean(value);
            }

            return Convert.ChangeType(
                value,
                underlyingType);
        }

        private static string BuildCreateStagingSql<T>(
            string tableName,
            string stagingTableName,
            IReadOnlyCollection<BulkColumnMapping<T>> mappings)
        {
            var columns =
                string.Join(
                    ", ",
                    mappings.Select(x =>
                        $"[{x.ColumnName}]"));

            return $@"
            SELECT TOP (0)
                {columns}
            INTO {stagingTableName}
            FROM [{tableName}];
        ";
        }

        private static string BuildUpdateSql<T>(
            string tableName,
            string stagingTableName,
            IReadOnlyCollection<BulkColumnMapping<T>> mappings,
            IReadOnlyCollection<BulkColumnMapping<T>> keyMappings)
        {
            var updateMappings =
                mappings
                    .Where(x =>
                        x.UpdateColumn &&
                        !x.IsKey)
                    .ToList();

            if (updateMappings.Count == 0)
                return "SELECT 0;";

            var setClause =
                string.Join(
                    ", ",
                    updateMappings.Select(x =>
                        $"Target.[{x.ColumnName}] = Source.[{x.ColumnName}]"));

            var joinClause =
                string.Join(
                    " AND ",
                    keyMappings.Select(x =>
                        $"Target.[{x.ColumnName}] = Source.[{x.ColumnName}]"));

            return $@"
            UPDATE Target
            SET
                {setClause}
            FROM [{tableName}] AS Target
            INNER JOIN {stagingTableName} AS Source
                ON {joinClause};
        ";
        }

        private static string BuildInsertSql<T>(string tableName,string stagingTableName,IReadOnlyCollection<BulkColumnMapping<T>> mappings,
            IReadOnlyCollection<BulkColumnMapping<T>> keyMappings)
        {
            var columns =string.Join(
                    ", ",mappings.Select(x =>$"[{x.ColumnName}]"));

            var sourceColumns =
                string.Join(
                    ", ",
                    mappings.Select(x =>
                        $"Source.[{x.ColumnName}]"));

            var keyMatch =
                string.Join(
                    " AND ",
                    keyMappings.Select(x =>
                        $"Target.[{x.ColumnName}] = Source.[{x.ColumnName}]"));

            return $@"
            INSERT INTO [{tableName}]
            (
                {columns}
            )
            SELECT
                {sourceColumns}
            FROM {stagingTableName} AS Source
            WHERE NOT EXISTS
            (
                SELECT 1
                FROM [{tableName}] AS Target
                WHERE {keyMatch}
            );
        ";
        }

        private static DataTable RemoveDuplicateKeys<T>(DataTable source,IReadOnlyCollection<BulkColumnMapping<T>> keyMappings)
        {
            if (keyMappings.Count == 0)
                return source;

            var seen =new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var rowsToRemove =
                new List<DataRow>();

            foreach (DataRow row in source.Rows)
            {
                var key =string.Join(
                        "|",
                        keyMappings.Select(x =>
                            row[x.ColumnName] == DBNull.Value
                                ? "<NULL>"
                                : row[x.ColumnName]?.ToString()
                                  ?? "<NULL>"));

                if (!seen.Add(key))
                {
                    rowsToRemove.Add(row);
                }
            }

            foreach (var row in rowsToRemove)
            {
                source.Rows.Remove(row);
            }

            return source;
        }

        private static void ValidateMappings<T>(IReadOnlyCollection<BulkColumnMapping<T>> mappings)
        {
            var duplicateColumns =
                mappings
                    .GroupBy(x =>
                        x.ColumnName,
                        StringComparer.OrdinalIgnoreCase)
                    .Where(x => x.Count() > 1)
                    .Select(x => x.Key)
                    .ToList();

            if (duplicateColumns.Count > 0)
            {
                throw new InvalidOperationException(
                    "Duplicate bulk columns: " +
                    string.Join(", ", duplicateColumns));
            }

            foreach (var mapping in mappings)
            {
                ValidateIdentifier(
                    mapping.ColumnName);

                if (mapping.ValueSelector == null)
                {
                    throw new InvalidOperationException(
                        $"Value selector missing for {mapping.ColumnName}");
                }
            }
        }

        private static void ValidateIdentifier(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                throw new ArgumentException(
                    "SQL identifier cannot be empty.");

            if (!Regex.IsMatch(
                    identifier,
                    @"^[A-Za-z0-9_]+$"))
            {
                throw new ArgumentException(
                    $"Invalid SQL identifier: {identifier}");
            }
        }

        private static async Task<int> ExecuteNonQueryAsync(
            SqlConnection connection,
            SqlTransaction transaction,
            string sql,
            CancellationToken cancellationToken)
        {
            await using var command =
                new SqlCommand(
                    sql,
                    connection,
                    transaction);

            command.CommandTimeout = 600;

            return await command.ExecuteNonQueryAsync(
                cancellationToken);
        }

        private sealed class BatchResult
        {
            public int Inserted { get; set; }
            public int Updated { get; set; }
        }
    }
}
