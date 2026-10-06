using App.Application.BulkColumnMapping;
using App.Application.DTOs.QuickBookOnlineDTO.Customer;
using App.Application.IExternalRepository.QuickBookOnline;
using App.Common.GenericResponse;
using Intuit.Ipp.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.Services.QuickBooks
{
    public class SyncImportCustomer
    {
        private readonly IQuickBooksCustomerRep _quickBooks;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SyncImportCustomer> _logger;
        private readonly IBulkUpsertService _bulkUpsertService;
        public SyncImportCustomer(IQuickBooksCustomerRep quickBooks,IUnitOfWork unitOfWork,ILogger<SyncImportCustomer> logger
            ,IBulkUpsertService bulkUpsertService)
        {
            _quickBooks = quickBooks?? throw new ArgumentNullException(nameof(quickBooks));
            _unitOfWork = unitOfWork?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _bulkUpsertService = bulkUpsertService ?? throw new ArgumentNullException(nameof(bulkUpsertService));
        }

        #region This method is responsible for synchronizing customers from QuickBooks to the local database.
        public async Task<Response> GetCustomersAsync()
        {
            _logger.LogInformation("QuickBooks customer sync started.");
            try
            {
                // 1. Get customers from QuickBooks
                _logger.LogInformation("Fetching customers from QuickBooks.");
                var customers = await _quickBooks.GetCustomersAsync();
                if (customers == null || !customers.Any())
                {
                    _logger.LogInformation("No customers found in QuickBooks.");
                    return new Response
                    {
                        Status = true,
                        Message = "No customers found in QuickBooks."
                    };
                }

                _logger.LogInformation("Retrieved {CustomerCount} customers from QuickBooks.", customers.Count);

                var mappings = GetCustomerMappings();
                var result = await _bulkUpsertService.BulkUpsertAsync(customers, "Main_Customers", mappings,batchSize: 5000);

                _logger.LogInformation("Customer import completed. " + "Processed: {Processed}, " +
                 "Inserted: {Inserted}, " + "Updated: {Updated}, " +"Batches: {Batches}",
                 result.TotalProcessed,result.TotalInserted,result.TotalUpdated,result.BatchCount);
                // 3. Commit all changes
                _logger.LogInformation("Saving customer sync changes to database.");

                return new Response
                {
                    Status = true,
                    Message = "Customer sync completed successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "QuickBooks customer sync failed.");

                return new Response
                {
                    Status = false,
                    Message = $"Customer sync failed: {ex.Message}"
                };
            }
            
        }
        #endregion

        private static IReadOnlyCollection<BulkColumnMapping<QuickBooksCustomerDto>>GetCustomerMappings()
        {
            return new[]
            {
                BulkMapping.Column<QuickBooksCustomerDto>(
                    "QuickBooksId",
                    typeof(string),
                    x => x.Id,
                    isKey: true,
                    update: false),

                BulkMapping.Column<QuickBooksCustomerDto>(
                    "SyncToken",
                    typeof(string),
                    x => x.SyncToken),

                BulkMapping.Column<QuickBooksCustomerDto>(
                    "DisplayName",
                    typeof(string),
                    x => x.DisplayName),

                BulkMapping.Column<QuickBooksCustomerDto>(
                    "GivenName",
                    typeof(string),
                    x => x.GivenName),

                BulkMapping.Column<QuickBooksCustomerDto>(
                    "FamilyName",
                    typeof(string),
                    x => x.FamilyName),

                BulkMapping.Column<QuickBooksCustomerDto>(
                    "CompanyName",
                    typeof(string),
                    x => x.CompanyName),

                BulkMapping.Column<QuickBooksCustomerDto>(
                    "Active",
                    typeof(bool),
                    x => x.Active),

                BulkMapping.Column<QuickBooksCustomerDto>(
                    "Balance",
                    typeof(decimal),
                    x => x.Balance),

                BulkMapping.Column<QuickBooksCustomerDto>(
                    "Email",
                    typeof(string),
                    x => x.PrimaryEmailAddr?.Address),

                BulkMapping.Column<QuickBooksCustomerDto>(
                    "Phone",
                    typeof(string),
                    x => x.PrimaryPhone?.FreeFormNumber),

                BulkMapping.Column<QuickBooksCustomerDto>(
                    "BillAddress",
                    typeof(string),
                    x => x.BillAddr?.Line1),

                BulkMapping.Column<QuickBooksCustomerDto>(
                    "BillCity",
                    typeof(string),
                    x => x.BillAddr?.City),

                BulkMapping.Column<QuickBooksCustomerDto>(
                    "BillPostalCode",
                    typeof(string),
                    x => x.BillAddr?.PostalCode),

                BulkMapping.Column<QuickBooksCustomerDto>(
                    "ShipAddress",
                    typeof(string),
                    x => x.ShipAddr?.Line1),

                BulkMapping.Column<QuickBooksCustomerDto>(
                    "ShipCity",
                    typeof(string),
                    x => x.ShipAddr?.City),

                BulkMapping.Column<QuickBooksCustomerDto>(
                    "ShipPostalCode",
                    typeof(string),
                    x => x.ShipAddr?.PostalCode),

                BulkMapping.Column<QuickBooksCustomerDto>(
                    "CreatedTime",
                    typeof(DateTime),
                    x => x.MetaData?.CreateTime),

                BulkMapping.Column<QuickBooksCustomerDto>(
                    "LastUpdatedTime",
                    typeof(DateTime),
                    x => x.MetaData?.LastUpdatedTime)
            };
        }
    }
}
