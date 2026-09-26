using App.Application.DTOs.Main_DTO;
using System;
using System.Collections.Generic;
using System.Text;
using Intuit.Ipp.OAuth2PlatformClient;
namespace App.Application.IExternalRepository.QuickBookOnline
{
    public interface IQuickBooksOnline
    {
        string GetAuthorizationUrl();

        Task<TokenResponse> GetBearerTokenAsync(string authorizationCode, string realmId);

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
