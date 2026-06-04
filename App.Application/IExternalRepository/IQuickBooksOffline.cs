using App.Application.DTOs.Main_DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.IExternalRepository
{
    public interface IQuickBooksOffline
    {
        string GetAuthorizationUrl();

        Task SaveTokensAsync(string authorizationCode, string realmId);

        Task<List<Main_ContactsDto>> GetCustomersAsync();

        Task<string> CreateCustomerAsync(Main_ContactsDto dto);
    }
}
