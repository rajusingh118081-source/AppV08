using App.Application.DTOs.Main_DTO;
using App.Application.DTOs.QuickBookOnlineDTO.Customer;
using App.Application.IExternalRepository;
using App.Application.IExternalRepository.QuickBookOnline;
using App.Domain.Entities.QuickBooksOnline;
using App.Infrastructure.ExternalRepository.QBO;
using Microsoft.Extensions.Configuration;
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
        public QuickBooksCustomerRep(IHttpService httpService, IQuickBooksTokenRep tokenRep, IQuickBooksOnline quickBooksOnline)
        {
            _httpService = httpService;
            _tokenRep = tokenRep;
            _logger = new LoggerFactory().CreateLogger<QuickBooksCustomerRep>();
            _quickBooksOnlineRep = quickBooksOnline;
        }

        #region This is a method to get customers from QuickBooks Online API
        public async Task<List<QuickBooksCustomerDto>> GetCustomersAsync()
        {
            QuickBooksToken quickBooksToken = new QuickBooksToken();
            var allCustomers = new List<QuickBooksCustomerDto>();
            DateTime fromDate = DateTime.UtcNow.AddDays(-50);
            const int pageSize = 10;
            int startPosition = 1;
            int pageNumber = 1;
            try
            {
                _logger.LogInformation("Starting QuickBooks customer retrieval. FromDate: {FromDate}, PageSize: {PageSize}",fromDate,pageSize);
                quickBooksToken =await _tokenRep.GetByRealmIdAsync(string.Empty);
                if (quickBooksToken == null)
                {
                    _logger.LogWarning("QuickBooks token not found.");
                    throw new Exception("QuickBooks token not found.");
                }
                _logger.LogInformation("QuickBooks token found. RealmId: {RealmId}",quickBooksToken.RealmId);
                while (true)
                {
                    _logger.LogInformation("Fetching QuickBooks customer page. " + "PageNumber: {PageNumber}, StartPosition: {StartPosition}, PageSize: {PageSize}",
                        pageNumber,
                        startPosition,
                        pageSize);

                    string date =fromDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss");

                    string query =
                        "SELECT * FROM Customer " +
                        $"WHERE MetaData.LastUpdatedTime > '{date}' " +
                        "ORDER BY MetaData.LastUpdatedTime " +
                        $"STARTPOSITION {startPosition} " +
                        $"MAXRESULTS {pageSize}";

                    _logger.LogDebug("QuickBooks customer query created. " + "PageNumber: {PageNumber}, StartPosition: {StartPosition}, FromDate: {FromDate}",
                        pageNumber,
                        startPosition,
                        fromDate);

                    var url =$"v3/company/{quickBooksToken.RealmId}/query" +$"?query={Uri.EscapeDataString(query)}";
                    var headers = new Dictionary<string, string>
                    {
                        ["Authorization"] = $"Bearer {quickBooksToken.AccessToken}",
                        ["Accept"] = "application/json"
                    };

                    _logger.LogInformation("Calling QuickBooks Customer API. " + "PageNumber: {PageNumber}, StartPosition: {StartPosition}",
                        pageNumber,
                        startPosition);

                    var result =await _httpService.GetAsync<QuickBooksCustomerResponse>(url,headers);
                    if (result == null)
                    {
                        _logger.LogWarning("QuickBooks returned a null response. " + "PageNumber: {PageNumber}, StartPosition: {StartPosition}",pageNumber, startPosition);
                        break;
                    }

                    var customers =result.QueryResponse?.Customer?? new List<QuickBooksCustomerDto>();
                    int currentPageCount = customers.Count;
                    _logger.LogInformation("QuickBooks customer page received. " + "PageNumber: {PageNumber}, StartPosition: {StartPosition}, " +
                        "RecordsInPage: {RecordsInPage}, PageSize: {PageSize}",
                        pageNumber,
                        startPosition,
                        currentPageCount,
                        pageSize);

                    if (currentPageCount == 0)
                    {
                        _logger.LogInformation("No customers returned. Pagination completed. " +"PageNumber: {PageNumber}, TotalCustomers: {TotalCustomers}", pageNumber, allCustomers.Count);
                        break;
                    }
                    allCustomers.AddRange(customers);
                    _logger.LogInformation(
                        "Customer page added to result. " +
                        "PageNumber: {PageNumber}, RecordsAdded: {RecordsAdded}, " +
                        "TotalCustomers: {TotalCustomers}",
                        pageNumber,
                        currentPageCount,
                        allCustomers.Count);

                    // If less than pageSize is returned,
                    // this is the last page.
                    if (currentPageCount < pageSize)
                    {
                        _logger.LogInformation("Last QuickBooks customer page detected. " + "PageNumber: {PageNumber}, RecordsInPage: {RecordsInPage}, " +
                            "TotalCustomers: {TotalCustomers}",
                            pageNumber,
                            currentPageCount,
                            allCustomers.Count);

                        break;
                    }

                    // Move to next page
                    startPosition += pageSize;
                    pageNumber++;

                    _logger.LogInformation(
                        "Moving to next QuickBooks customer page. " +
                        "NextPageNumber: {NextPageNumber}, NextStartPosition: {NextStartPosition}",
                        pageNumber,
                        startPosition);
                }

                _logger.LogInformation(
                    "QuickBooks customer retrieval completed successfully. " +
                    "TotalCustomers: {TotalCustomers}, TotalPages: {TotalPages}",
                    allCustomers.Count,
                    pageNumber);
            }
            catch (HandleHttpException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                _logger.LogWarning("QuickBooks access token expired during customer retrieval. " +
                    "PageNumber: {PageNumber}, StartPosition: {StartPosition}, RealmId: {RealmId}. " +
                    "Refreshing token.",
                    pageNumber,
                    startPosition,
                    quickBooksToken?.RealmId);

                await _quickBooksOnlineRep.RefreshQuickBooksTokenAsync(quickBooksToken);

                _logger.LogInformation("QuickBooks token refreshed successfully. " +"Retrying customer retrieval from the beginning.");
                return await GetCustomersAsync();
            }
            catch (HandleHttpException ex)
            {
                _logger.LogError(ex,"QuickBooks customer API failed. " + "PageNumber: {PageNumber}, StartPosition: {StartPosition}, " +
                    "StatusCode: {StatusCode}, Response: {Response}",
                    pageNumber,
                    startPosition,
                    (int)ex.StatusCode,
                    ex.ResponseBody);

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Unexpected error while retrieving QuickBooks customers. " +
                    "PageNumber: {PageNumber}, StartPosition: {StartPosition}, " +
                    "TotalCustomersRetrieved: {TotalCustomersRetrieved}",
                    pageNumber,
                    startPosition,
                    allCustomers.Count);

                throw;
            }
            return allCustomers;
        }
        #endregion
    }
}
