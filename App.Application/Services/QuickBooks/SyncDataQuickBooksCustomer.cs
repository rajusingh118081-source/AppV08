using App.Application.IExternalRepository.QuickBookOnline;
using App.Common.GenericResponse;
using Intuit.Ipp.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.Services.QuickBooks
{
    public class SyncDataQuickBooksCustomer
    {
        private readonly IQuickBooksCustomerRep _quickBooks;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SyncDataQuickBooksCustomer> _logger;
        public SyncDataQuickBooksCustomer(IQuickBooksCustomerRep quickBooks,IUnitOfWork unitOfWork,ILogger<SyncDataQuickBooksCustomer> logger)
        {
            _quickBooks = quickBooks?? throw new ArgumentNullException(nameof(quickBooks));
            _unitOfWork = unitOfWork?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
                _logger.LogInformation("Retrieved {CustomerCount} customers from QuickBooks.",customers.Count);

                int added = 0;
                int updated = 0;
                // 2. Insert / Update local database
                foreach (var customer in customers)
                {
                    try
                    {
                        //var existingCustomer = _unitOfWork.Customers.Find(x => x.QuickBooksId == customer.Id).FirstOrDefault();

                        //if (existingCustomer == null)
                        //{
                        //    // Customer does not exist → INSERT
                        //    //var newCustomer = new Customer
                        //    //{
                        //    //    QuickBooksId = customer.Id,
                        //    //    Name = customer.DisplayName,
                        //    //    // map other fields
                        //    //};

                        //    //_unitOfWork.Customers.Add(newCustomer);

                        //    added++;
                        //    _logger.LogInformation("QuickBooks customer added. QuickBooksId: {QuickBooksId}",customer.Id);
                        //}
                        //else
                        //{
                        //    // Customer already exists → UPDATE
                        //    existingCustomer.Name = customer.DisplayName;
                        //    // update other fields
                        //    updated++;
                        //    _logger.LogInformation("QuickBooks customer updated. QuickBooksId: {QuickBooksId}",customer.Id);
                        //}
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex,"Error processing QuickBooks customer. QuickBooksId: {QuickBooksId}",customer.Id);
                        throw;
                    }
                }
                _logger.LogInformation("Customer processing completed. Added: {Added}, Updated: {Updated}.",added,updated);

                // 3. Commit all changes
                _logger.LogInformation("Saving customer sync changes to database.");

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("QuickBooks customer sync completed successfully. " +
                    "Added: {Added}, Updated: {Updated}.",
                    added,
                    updated);

                return new Response
                {
                    Status = true,
                    Message =$"Customer sync completed. " +$"Added: {added}, Updated: {updated}."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"QuickBooks customer sync failed.");

                return new Response
                {
                    Status = false,
                    Message =$"Customer sync failed: {ex.Message}"
                };
            }
        #endregion
        }
    }
}
