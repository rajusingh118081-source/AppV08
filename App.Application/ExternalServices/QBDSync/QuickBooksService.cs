using App.Application.DTOs.Main_DTO;
using Intuit.Ipp.OAuth2PlatformClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.ExternalServices.QBDSync
{
    public class QuickBooksService : IQuickBooks
    {
        private readonly OAuth2Client _oauthClient;

        public QuickBooksService(IConfiguration config)
        {
            _oauthClient = new OAuth2Client(
                config["QuickBooks:ClientId"],
                config["QuickBooks:ClientSecret"],
                config["QuickBooks:RedirectUri"],
                config["QuickBooks:Environment"]);
        }

        public string GetAuthorizationUrl()
        {
            return _oauthClient.GetAuthorizationURL( new List<OidcScopes>
            {
             OidcScopes.Accounting
            });
        }

        public async Task SaveTokensAsync(string code,string realmId)
        {
            var tokenResponse = await _oauthClient.GetBearerTokenAsync(code);

            // Save Access Token
            // Save Refresh Token
            // Save RealmId
        }

        public async Task<List<ContactDto>> GetCustomersAsync()
        {
            // Use DataService
            return new List<ContactDto>();
        }

        public async Task<string> CreateCustomerAsync(ContactDto dto)
        {
            return "";
        }
    }
}
