using App.Application.DTOs.Main_DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.ExternalServices.QBDSync
{
    public interface IQuickBooks
    {
        string GetAuthorizationUrl();

        Task SaveTokensAsync(string authorizationCode,string realmId);

        Task<List<ContactDto>> GetCustomersAsync();

        Task<string> CreateCustomerAsync(ContactDto dto);
    }
}
