using App.Application.DTOs.QuickBookOnlineDTO.Invoice;
using App.Application.IExternalRepository;
using App.Application.IExternalRepository.QuickBookOnline;
using App.Domain.Entities.QuickBooksOnline;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace App.Infrastructure.ExternalRepository.QuickBooksOnline
{
    public class QuickBooksInvoiceRep : IQuickBooksInvoiceRep
    {
        private readonly IHttpService _httpService;
        private readonly IQuickBooksTokenRep _tokenRep;
        private readonly ILogger<QuickBooksInvoiceRep> _logger;
        private readonly IQuickBooksOnline _quickBooksOnlineRep;
        public QuickBooksInvoiceRep(IHttpService httpService, IQuickBooksTokenRep tokenRep, IQuickBooksOnline quickBooksOnline)
        {
            _httpService = httpService;
            _tokenRep = tokenRep;
            _logger = new LoggerFactory().CreateLogger<QuickBooksInvoiceRep>();
            _quickBooksOnlineRep = quickBooksOnline;
        }

        #region This method retrieves invoices from QuickBooks Online API with pagination and error handling.
        public async Task<List<QuickBooksInvoiceDto>> GetInvoicesAsync(bool retryAfterRefresh = false)
        {
            QuickBooksToken quickBooksToken = new QuickBooksToken();
            var allInvoices = new List<QuickBooksInvoiceDto>();
            DateTime fromDate = DateTime.UtcNow.AddDays(-50);
            const int pageSize = 10;
            int startPosition = 1;
            int pageNumber = 1;
            try
            {
                _logger.LogInformation("Starting QuickBooks invoice retrieval. " + "FromDate: {FromDate}, PageSize: {PageSize}",fromDate,pageSize);
                // Get QuickBooks token
                quickBooksToken = await _tokenRep.GetByRealmIdAsync(string.Empty);
                if (quickBooksToken == null)
                {
                    _logger.LogWarning("QuickBooks token not found.");
                    throw new Exception("QuickBooks token not found.");
                }
                _logger.LogInformation("QuickBooks token found. RealmId: {RealmId}",quickBooksToken.RealmId);

                while (true)
                {
                    _logger.LogInformation("Fetching QuickBooks invoice page. " +"PageNumber: {PageNumber}, " +
                        "StartPosition: {StartPosition}, " +"PageSize: {PageSize}",pageNumber,startPosition,pageSize);

                    string date =fromDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss");

                    string query =
                        "SELECT * FROM Invoice " +
                        $"WHERE MetaData.LastUpdatedTime > '{date}' " +
                        "ORDER BY MetaData.LastUpdatedTime " +
                        $"STARTPOSITION {startPosition} " +
                        $"MAXRESULTS {pageSize}";

                    _logger.LogDebug("QuickBooks invoice query created. " +"PageNumber: {PageNumber}, " +
                        "StartPosition: {StartPosition}, " +"FromDate: {FromDate}",
                        pageNumber,startPosition,fromDate);

                    var url =$"v3/company/{quickBooksToken.RealmId}/query" +$"?query={Uri.EscapeDataString(query)}";
                    var headers = new Dictionary<string, string>
                    {
                        ["Authorization"] =$"Bearer {quickBooksToken.AccessToken}",
                        ["Accept"] = "application/json"
                    };

                    _logger.LogInformation("Calling QuickBooks Invoice API. " +
                        "PageNumber: {PageNumber}, " +"StartPosition: {StartPosition}",
                        pageNumber,startPosition);

                    var result =await _httpService.GetAsync<QuickBooksInvoiceResponse>(url,headers);

                    if (result == null)
                    {
                        _logger.LogWarning("QuickBooks returned a null invoice response. " +
                            "PageNumber: {PageNumber}, " + "StartPosition: {StartPosition}",pageNumber,startPosition);

                        break;
                    }
                    var invoices =result.QueryResponse?.Invoice?? new List<QuickBooksInvoiceDto>();
                    int currentPageCount = invoices.Count;

                    _logger.LogInformation("QuickBooks invoice page received. " +"PageNumber: {PageNumber}, " +
                        "StartPosition: {StartPosition}, " +"RecordsInPage: {RecordsInPage}, " + "PageSize: {PageSize}",
                        pageNumber,startPosition,currentPageCount,pageSize);

                    if (currentPageCount == 0)
                    {
                        _logger.LogInformation("No invoices returned. Pagination completed. " + "PageNumber: {PageNumber}, " +
                            "TotalInvoices: {TotalInvoices}",pageNumber,allInvoices.Count);
                        break;
                    }

                    allInvoices.AddRange(invoices);

                    _logger.LogInformation("Invoice page added to result. " +"PageNumber: {PageNumber}, " +
                        "RecordsAdded: {RecordsAdded}, " +
                        "TotalInvoices: {TotalInvoices}",pageNumber,currentPageCount, allInvoices.Count);

                    // Last page
                    if (currentPageCount < pageSize)
                    {
                        _logger.LogInformation(
                            "Last QuickBooks invoice page detected. " +
                            "PageNumber: {PageNumber}, " + "RecordsInPage: {RecordsInPage}, " + "TotalInvoices: {TotalInvoices}",
                            pageNumber,currentPageCount,allInvoices.Count);
                        break;
                    }

                    // Next page
                    startPosition += pageSize;
                    pageNumber++;

                    _logger.LogInformation("Moving to next QuickBooks invoice page. " +
                        "NextPageNumber: {NextPageNumber}, " +
                        "NextStartPosition: {NextStartPosition}",
                        pageNumber,
                        startPosition);
                }

                _logger.LogInformation("QuickBooks invoice retrieval completed successfully. " +
                    "TotalInvoices: {TotalInvoices}, " +
                    "TotalPages: {TotalPages}", allInvoices.Count,pageNumber);
            }
            catch (HandleHttpException ex)when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (retryAfterRefresh)
                {
                    _logger.LogError(ex,"QuickBooks returned 401 again after token refresh. " +"Stopping invoice retrieval.");
                    throw;
                }

                _logger.LogWarning(
                    "QuickBooks access token expired during invoice retrieval. " +
                    "PageNumber: {PageNumber}, " + "StartPosition: {StartPosition}, " +
                    "RealmId: {RealmId}. Refreshing token.",pageNumber,startPosition,quickBooksToken?.RealmId);

                await _quickBooksOnlineRep.RefreshQuickBooksTokenAsync(quickBooksToken);
                _logger.LogInformation("QuickBooks token refreshed successfully. " +"Retrying invoice retrieval once.");
                return await GetInvoicesAsync(true);
            }
            catch (HandleHttpException ex)
            {
                _logger.LogError(ex,"QuickBooks invoice API failed. " + "PageNumber: {PageNumber}, " + "StartPosition: {StartPosition}, " +"StatusCode: {StatusCode}, " +
                    "Response: {Response}",pageNumber,startPosition,(int)ex.StatusCode,ex.ResponseBody);

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unexpected error while retrieving QuickBooks invoices. " +
                    "PageNumber: {PageNumber}, " +
                    "StartPosition: {StartPosition}, " +
                    "TotalInvoicesRetrieved: {TotalInvoicesRetrieved}",pageNumber,startPosition,allInvoices.Count);
                throw;
            }

            return allInvoices;
        }
        #endregion
    }
}
