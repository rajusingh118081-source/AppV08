using App.Application.BulkColumnMapping;
using App.Application.DTOs.QuickBookOnlineDTO.Invoice;
using App.Application.IExternalRepository.QuickBookOnline;
using App.Common.GenericResponse;
using App.Common.Utilities;
using Intuit.Ipp.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.Services.QuickBooks
{
    public class SyncImportInvoice
    {
        private readonly IQuickBooksInvoiceRep _quickBooks;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SyncImportInvoice> _logger;
        private readonly IBulkUpsertService _bulkUpsertService;
        public SyncImportInvoice(IQuickBooksInvoiceRep quickBooks,IUnitOfWork unitOfWork,ILogger<SyncImportInvoice> logger
            ,IBulkUpsertService bulkUpsertService)
        {
            _quickBooks = quickBooks?? throw new ArgumentNullException(nameof(quickBooks));
            _unitOfWork = unitOfWork?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _bulkUpsertService = bulkUpsertService ?? throw new ArgumentNullException(nameof(bulkUpsertService));
        }

        #region This method is responsible for synchronizing Invoice from QuickBooks to the local database.
        public async Task<Response> GetInvoicesAsync()
        {
            _logger.LogInformation("QuickBooks invoice sync started.");
            try
            {
                // 1. Get invoices from QuickBooks
                _logger.LogInformation("Fetching invoices from QuickBooks.");
                var invoices = await _quickBooks.GetInvoicesAsync();
                if (invoices == null || !invoices.Any())
                {
                    _logger.LogInformation("No invoices found in QuickBooks.");
                    return new Response
                    {
                        Status = true,
                        Message = "No invoices found in QuickBooks."
                    };
                }
                _logger.LogInformation("Retrieved {InvoiceCount} invoices from QuickBooks.", invoices.Count);

                // ==========================
                // 1. Invoice
                // ==========================

                var invoiceResult =await _bulkUpsertService.BulkUpsertAsync(invoices,"Invoices",GetInvoiceMappings(),batchSize: 5000);

                _logger.LogInformation("Invoices imported. Processed: {Processed}, " +"Inserted: {Inserted}, Updated: {Updated}",
                    invoiceResult.TotalProcessed,invoiceResult.TotalInserted,invoiceResult.TotalUpdated);

                // ==========================
                // 2. Invoice Line Items
                // ==========================

                var lineItems =invoices.Where(x => x.Line != null).SelectMany(invoice =>invoice.Line!.Where(line =>
                                !string.IsNullOrWhiteSpace(line.Id))
                                .Select(line =>
                                {
                                    line.InvoiceQuickBooksId =invoice.Id;
                                    return line;
                                })).ToList();

                if (lineItems.Count == 0)
                    return new Response
                    {
                        Status = true,
                        Message = "No invoices Line Items found in QuickBooks."
                    };

                var lineResult =await _bulkUpsertService.BulkUpsertAsync(lineItems,"InvoiceLineItems",GetInvoiceLineMappings(),batchSize: 5000);

                _logger.LogInformation("Invoice line items imported. " +"Processed: {Processed}, " +
                    "Inserted: {Inserted}, " +"Updated: {Updated}",
                    lineResult.TotalProcessed,lineResult.TotalInserted,lineResult.TotalUpdated);

                // 3. Commit all changes
                _logger.LogInformation("Saving invoice sync changes to database.");
                return new Response
                {
                    Status = true,
                    Message = "Customer sync completed successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "QuickBooks invoice sync failed.");

                return new Response
                {
                    Status = false,
                    Message = $"Invoice sync failed: {ex.Message}"
                };
            }
        }
        #endregion

        private static IReadOnlyCollection<BulkColumnMapping<QuickBooksInvoiceDto>> GetInvoiceMappings()
        {
            return new[]
            {
                BulkMapping.Column<QuickBooksInvoiceDto>(
                    "QuickBooksId",
                    typeof(string),
                    x => x.Id,
                    isKey: true,
                    update: false),

                BulkMapping.Column<QuickBooksInvoiceDto>(
                    "SyncToken",
                    typeof(string),
                    x => x.SyncToken),

                BulkMapping.Column<QuickBooksInvoiceDto>(
                    "InvoiceNumber",
                    typeof(string),
                    x => x.DocNumber),

                BulkMapping.Column<QuickBooksInvoiceDto>(
                    "InvoiceDate",
                    typeof(DateTime),
                    x => DecimalRounding.ParseDate(x.TxnDate)),

                BulkMapping.Column<QuickBooksInvoiceDto>(
                    "DueDate",
                    typeof(DateTime),
                    x => DecimalRounding.ParseDate(x.DueDate)),

                BulkMapping.Column<QuickBooksInvoiceDto>(
                   "Subtotal",
                    typeof(decimal),
                    x => x.TotalAmt - (x.TxnTaxDetail?.TotalTax ?? 0)),

                  BulkMapping.Column<QuickBooksInvoiceDto>(
                    "TotalAmount",
                    typeof(decimal),
                    x => x.TotalAmt),

                    BulkMapping.Column<QuickBooksInvoiceDto>(
                    "TotalTax",
                    typeof(decimal),
                    x => x.TxnTaxDetail?.TotalTax),

                BulkMapping.Column<QuickBooksInvoiceDto>(
                    "Balance",
                    typeof(decimal),
                    x => x.Balance),

                BulkMapping.Column<QuickBooksInvoiceDto>(
                "TaxName",
                typeof(string),
                x => x.TxnTaxDetail?.TaxLine?
                    .FirstOrDefault()?
                    .TaxLineDetail?
                    .TaxRateRef?
                    .Name),

                BulkMapping.Column<QuickBooksInvoiceDto>(
                    "CustomerQuickBooksId",
                    typeof(string),
                    x => x.CustomerRef?.Value),

                BulkMapping.Column<QuickBooksInvoiceDto>(
                    "CustomerName",
                    typeof(string),
                    x => x.CustomerRef?.Name),

                BulkMapping.Column<QuickBooksInvoiceDto>(
                    "CurrencyCode",
                    typeof(string),
                    x => x.CurrencyRef?.Value),

                BulkMapping.Column<QuickBooksInvoiceDto>(
                    "PrivateNote",
                    typeof(string),
                    x => x.PrivateNote),

                BulkMapping.Column<QuickBooksInvoiceDto>(
                    "TxnStatus",
                    typeof(string),
                    x => x.TxnStatus),

                BulkMapping.Column<QuickBooksInvoiceDto>(
                    "CreatedTime",
                    typeof(DateTime),
                    x => x.MetaData?.CreateTime),

                BulkMapping.Column<QuickBooksInvoiceDto>(
                    "LastUpdatedTime",
                    typeof(DateTime),
                    x => x.MetaData?.LastUpdatedTime)
            };
        }

        private static IReadOnlyCollection<BulkColumnMapping<QuickBooksInvoiceLineDto>>GetInvoiceLineMappings()
        {
            return new[]
            {
                // Composite key - Invoice
                BulkMapping.Column<QuickBooksInvoiceLineDto>(
                    "InvoiceQuickBooksId",
                    typeof(string),
                    x => x.InvoiceQuickBooksId,
                    isKey: true,
                    update: false),

                // Composite key - Line
                BulkMapping.Column<QuickBooksInvoiceLineDto>(
                    "QuickBooksLineId",
                    typeof(string),
                    x => x.Id,
                    isKey: true,
                    update: false),

                BulkMapping.Column<QuickBooksInvoiceLineDto>(
                    "Description",
                    typeof(string),
                    x => x.Description),

                BulkMapping.Column<QuickBooksInvoiceLineDto>(
                    "DetailType",
                    typeof(string),
                    x => x.DetailType),

                BulkMapping.Column<QuickBooksInvoiceLineDto>(
                    "Amount",
                    typeof(decimal),
                    x => x.Amount),

                BulkMapping.Column<QuickBooksInvoiceLineDto>(
                    "ItemQuickBooksId",
                    typeof(string),
                    x => x.SalesItemLineDetail?
                        .ItemRef?.Value),

                BulkMapping.Column<QuickBooksInvoiceLineDto>(
                    "ItemName",
                    typeof(string),
                    x => x.SalesItemLineDetail?
                        .ItemRef?.Name),

                BulkMapping.Column<QuickBooksInvoiceLineDto>(
                    "Quantity",
                    typeof(decimal),
                    x => x.SalesItemLineDetail?.Qty),

                BulkMapping.Column<QuickBooksInvoiceLineDto>(
                    "UnitPrice",
                    typeof(decimal),
                    x => x.SalesItemLineDetail?.UnitPrice),

                BulkMapping.Column<QuickBooksInvoiceLineDto>(
                    "TaxCodeQuickBooksId",
                    typeof(string),
                    x => x.SalesItemLineDetail?
                        .TaxCodeRef?.Value),

                BulkMapping.Column<QuickBooksInvoiceLineDto>(
                    "TaxCodeName",
                    typeof(string),
                    x => x.SalesItemLineDetail?
                        .TaxCodeRef?.Name)
            };
        }
    }
}
