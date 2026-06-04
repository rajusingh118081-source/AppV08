using App.Application.DTOs.Main_DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.IExternalRepository
{
    public interface IQuickBooksOnline
    {
        string GetAuthorizationUrl();

        Task SaveTokensAsync(string authorizationCode, string realmId);

        Task<List<Main_ContactsDto>> GetCustomersAsync();

        Task<string> CreateCustomerAsync(Main_ContactsDto dto);
    }

    public interface IQuickBooksService
    {
        Task<string> GetAccessTokenAsync();
        Task<bool> SyncCustomerAsync(Main_ContactsDto customer);
        Task<bool> SyncInvoiceAsync(Main_ContactsDto invoice);
    }
}
