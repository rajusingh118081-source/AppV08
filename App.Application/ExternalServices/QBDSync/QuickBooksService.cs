using App.Application.DTOs.Main_DTO;
using App.Domain.Entities;
using Intuit.Ipp.OAuth2PlatformClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.ExternalServices.QBDSync
{
    public class QuickBooksService : IQuickBooks
    {
        private readonly OAuth2Client _oauthClient;
        private readonly ILogger<QuickBooksService> _logger;
        public QuickBooksService(IConfiguration config, ILogger<QuickBooksService> logger,
            IOptions<QBOSettings> options)
        {
            _logger = logger;

            var settings = options.Value;

            _oauthClient = new OAuth2Client(
                settings.ClientId,
                settings.ClientSecret,
                settings.RedirectUri,
                settings.Environment);
            _logger.LogInformation("Customer Sync Started. CorrelationId:{CorrelationId}", _oauthClient.ClientID);
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
