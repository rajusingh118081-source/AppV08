using App.Application.DTOs.Main_DTO;
using App.Application.IExternalRepository;
using App.Domain.Entities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Infrastructure.ExternalRepository.QuickBooksOnline
{
    public class QuickBooksService : IQuickBooksService
    {
        private readonly HttpClient _httpClient;
        private readonly QBOSettings _settings;

        public QuickBooksService(HttpClient httpClient, IOptions<QBOSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            // OAuth 2.0 token call
            var request = new HttpRequestMessage(HttpMethod.Post,
                "https://oauth.platform.intuit.com/oauth2/v1/tokens/bearer");

            // Add headers + body

            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<bool> SyncCustomerAsync(Main_ContactsDto customer)
        {
            var payload = new
            {
                DisplayName = customer.DisplayName,
                PrimaryEmailAddr = new { Address = customer.EmailAddress }
            };

            var json = JsonConvert.SerializeObject(payload);

            var request = new HttpRequestMessage(HttpMethod.Post,
                "https://quickbooks.api.intuit.com/v3/company/{realmId}/customer");

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public Task<bool> SyncInvoiceAsync(Main_ContactsDto invoice)
        {
            throw new NotImplementedException();
        }
    }
}
