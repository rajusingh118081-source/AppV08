using AapRepository;
using App.Application.IExternalRepository.QuickBookOnline;
using App.Application.Services.QuickBooks;
using App.Infrastructure.ExternalRepository.QBO;
using AppCRMWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AppCRMWeb.Controllers
{
    public class HomeController : BaseController
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IQuickBooksOnline _quickBooks;
        private readonly IQuickBooksTokenRep _quickBooksToken;
        private readonly SyncDataQuickBooksToken _syncDataToken;
        private readonly SyncDataQuickBooksCustomer _syncDataCustomer;
        public HomeController(ILogger<HomeController> logger,IHttpContextAccessor httpContext,
            IQuickBooksOnline quickBooks, SyncDataQuickBooksToken syncDataToken, SyncDataQuickBooksCustomer syncDataCustomer) : base(httpContext)
        {
            _logger = logger;
            _quickBooks = quickBooks; 
            _syncDataToken = syncDataToken;
            _syncDataCustomer = syncDataCustomer;
        }

        public async Task<IActionResult> Index()
        {
            //[FromQuery] UserSearchRequest request
            return View();
        }
        public async Task<IActionResult> QuickBookOnlineConnect()
        {
            //[FromQuery] UserSearchRequest request
            var qboOnle = _quickBooks.GetAuthorizationUrl();
            return Json(qboOnle);
        }

        public async Task<IActionResult> QuickBookOnlineCustomer()
        {
            //[FromQuery] UserSearchRequest request
            var tokenResponse = await _syncDataCustomer.GetCustomersAsync();
            return Json(tokenResponse);
        }
        [HttpGet]
        public async Task<ActionResult> Callback(string code,string state,string realmId)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Content("Authorization code is missing.");
            }

            if (string.IsNullOrEmpty(realmId))
            {
                return Content("RealmId is missing.");
            }

            try
            {
                var tokenResponse = await _syncDataToken.GetBearerTokenAsync(code, realmId);

                // Save these in your database.
                // DO NOT display the actual tokens in production.

                return Content("QuickBooks connected successfully.");
            }
            catch (Exception ex)
            {
                return Content($"QuickBooks authorization failed: {ex.Message}");
            }
        }

        public IActionResult Privacy(string code, string state, string realmId)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
