using App.Application.DTOs.Main_DTO;
using App.Application.DTOs.QuickBookOnlineDTO.Customer;
using App.Application.DTOs.QuickBookOnlineDTO.Invoice;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.IExternalRepository.QuickBookOnline
{
    public interface IQuickBooksInvoiceRep
    {
        /// <summary>
        /// This method retrieves a list of customers from QuickBooks Online asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of QuickBooksCustomerDto objects.</returns>
        Task<List<QuickBooksInvoiceDto>> GetInvoicesAsync(bool retryAfterRefresh = false);
    }

}
