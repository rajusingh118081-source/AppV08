using App.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.BulkColumnMapping
{
    public interface IBulkUpsertService
    {
        Task<BulkUpsertResult> BulkUpsertAsync<T>(IEnumerable<T> data,string tableName,IReadOnlyCollection<BulkColumnMapping<T>> mappings,
            int batchSize = 5000,CancellationToken cancellationToken = default);
    }
}
