using AapRepository;
using App.Application.ExternalServices.QBDSync;
using App.Model;
using App.Model.Utility.Helper;
using AppCRMWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AppCRMWeb.Controllers
{
    public class HomeController : BaseController
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IQuickBooks _quickBooks;
        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork,IHttpContextAccessor httpContext,
            IQuickBooks quickBooks) : base(unitOfWork, httpContext)
        {
            _logger = logger;
            _quickBooks = quickBooks;
        }

        public async Task<IActionResult> Index()
        {
            //[FromQuery] UserSearchRequest request
            var qboOnle = _quickBooks.GetAuthorizationUrl();
            return View();
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
