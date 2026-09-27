using App.Application.DTOs.QuickBookOnlineDTO.Invoice;
using App.Application.IExternalRepository.QuickBookOnline;
using App.Common.GenericResponse;
using Intuit.Ipp.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.Services.QuickBooks
{
    public class SyncDataQuickBooksInvoice
    {
        private readonly IQuickBooksInvoiceRep _quickBooks;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SyncDataQuickBooksInvoice> _logger;

        public SyncDataQuickBooksInvoice(IQuickBooksInvoiceRep quickBooks,IUnitOfWork unitOfWork,ILogger<SyncDataQuickBooksInvoice> logger)
        {
            _quickBooks = quickBooks?? throw new ArgumentNullException(nameof(quickBooks));
            _unitOfWork = unitOfWork?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

                int added = 0;
                int updated = 0;
                // 2. Insert / Update local database
                foreach (var invoice in invoices)
                {

                    _logger.LogInformation("Invoice received. InvoiceId: {InvoiceId}, DocNumber: {DocNumber}",invoice.Id, invoice.DocNumber);

                    var items = invoice.Line?.Where(x =>x.DetailType == "SalesItemLineDetail" && x.SalesItemLineDetail != null).ToList() ?? new List<QuickBooksInvoiceLineDto>();
                   
                    _logger.LogInformation("Invoice items received. InvoiceId: {InvoiceId}, ItemCount: {ItemCount}",invoice.Id,items.Count);

                    foreach (var item in items)
                    {
                        _logger.LogInformation(
                            "Invoice item. InvoiceId: {InvoiceId}, " +
                            "ItemId: {ItemId}, ItemName: {ItemName}, " +
                            "Qty: {Qty}, UnitPrice: {UnitPrice}, Amount: {Amount}",
                            invoice.Id,
                            item.SalesItemLineDetail?.ItemRef?.Value,
                            item.SalesItemLineDetail?.ItemRef?.Name,
                            item.SalesItemLineDetail?.Qty,
                            item.SalesItemLineDetail?.UnitPrice,
                            item.Amount);
                    }
                }
                _logger.LogInformation("Invoice processing completed. Added: {Added}, Updated: {Updated}.", added, updated);

                // 3. Commit all changes
                _logger.LogInformation("Saving invoice sync changes to database.");

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("QuickBooks invoice sync completed successfully. " +
                    "Added: {Added}, Updated: {Updated}.",
                    added,
                    updated);

                return new Response
                {
                    Status = true,
                    Message = $"Invoice sync completed. " + $"Added: {added}, Updated: {updated}."
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
    }
}
