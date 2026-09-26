using App.Application.DTOs.Main_DTO;
using App.Application.DTOs.QuickBookOnlineDTO.Customer;
using App.Application.IExternalRepository;
using App.Application.IExternalRepository.QuickBookOnline;
using App.Domain.Entities.QuickBooksOnline;
using App.Infrastructure.ExternalRepository.QBO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace App.Infrastructure.ExternalRepository.QuickBooksOnline
{
    public class QuickBooksCustomerRep : IQuickBooksCustomerRep
    {
        private readonly IHttpService _httpService;
        private readonly IQuickBooksTokenRep _tokenRep;
        private readonly ILogger<QuickBooksCustomerRep> _logger;
        private readonly IQuickBooksOnline _quickBooksOnlineRep;
        public QuickBooksCustomerRep(IHttpService httpService,IQuickBooksTokenRep tokenRep, IQuickBooksOnline quickBooksOnline)
        {
            _httpService = httpService;
            _tokenRep = tokenRep;
            _logger = new LoggerFactory().CreateLogger<QuickBooksCustomerRep>();
            _quickBooksOnlineRep = quickBooksOnline;
        }

        #region This is a method to get customers from QuickBooks Online API
        public async Task<List<QuickBooksCustomerDto>> GetCustomersAsync()
        {
            QuickBooksToken quickBooksToken=new QuickBooksToken();
            try
            {
                _logger.LogInformation("Starting QuickBooks customer retrieval.");
                quickBooksToken = await _tokenRep.GetByRealmIdAsync(string.Empty);
                if (quickBooksToken == null)
                {
                    _logger.LogWarning("QuickBooks token not found.");
                    throw new Exception("QuickBooks token not found.");
                }
                _logger.LogInformation("QuickBooks token found. RealmId: {RealmId}", quickBooksToken.RealmId);

                var query = "select * from Customer";
                var url =$"v3/company/{quickBooksToken.RealmId}/query" + $"?query={Uri.EscapeDataString(query)}";

                _logger.LogInformation("Calling QuickBooks Customer API. URL: {Url}",url);

                var headers = new Dictionary<string, string>
                {
                    ["Authorization"] = $"Bearer {quickBooksToken.AccessToken}",
                    ["Accept"] = "application/json"
                };

                var result = await _httpService.GetAsync<QuickBooksCustomerResponse>(url, headers);

                if (result == null)
                {
                    _logger.LogWarning("QuickBooks API returned a null response. RealmId: {RealmId}",quickBooksToken.RealmId);
                    return new List<QuickBooksCustomerDto>();
                }
                var customers =result.QueryResponse?.Customer ?? new List<QuickBooksCustomerDto>();

                _logger.LogInformation("QuickBooks customer retrieval completed. Customer count: {Count}",customers.Count);
                return customers;
            }
            catch (HandleHttpException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                _logger.LogWarning("QuickBooks access token expired. RealmId: {RealmId}",quickBooksToken.RealmId);
                // Refresh token here
                await _quickBooksOnlineRep.RefreshQuickBooksTokenAsync(quickBooksToken);
                throw;
            }
            catch (HandleHttpException ex)
            {
                _logger.LogError(
                    ex,
                    "QuickBooks API failed. StatusCode: {StatusCode}, Response: {Response}",
                    (int)ex.StatusCode,
                    ex.ResponseBody);

                throw;
            }
        }
        #endregion
    }

}
