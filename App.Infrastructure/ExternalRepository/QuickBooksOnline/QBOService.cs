using App.Application.DTOs.Main_DTO;
using App.Application.IExternalRepository;
using App.Domain.Entities;
using Intuit.Ipp.OAuth2PlatformClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Infrastructure.ExternalRepository.QBO
{
    public class QBOService : IQuickBooksOnline
    {
        private readonly OAuth2Client _oauthClient;
        private readonly ILogger<QBOService> _logger;
        public QBOService(IConfiguration config, ILogger<QBOService> logger,
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
            return _oauthClient.GetAuthorizationURL(new List<OidcScopes>
            {
             OidcScopes.Accounting
            });
        }

        public async Task SaveTokensAsync(string code, string realmId)
        {
            var tokenResponse = await _oauthClient.GetBearerTokenAsync(code);

            // Save Access Token
            // Save Refresh Token
            // Save RealmId
        }

        public async Task<List<Main_ContactsDto>> GetCustomersAsync()
        {
            // Use DataService
            return new List<Main_ContactsDto>();
        }

        public async Task<string> CreateCustomerAsync(Main_ContactsDto dto)
        {
            return "";
        }
    }
}
