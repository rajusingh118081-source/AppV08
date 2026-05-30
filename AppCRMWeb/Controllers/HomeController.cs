using AapRepository;
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

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork,IHttpContextAccessor httpContext): base(unitOfWork, httpContext)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var dadd=_unitOfWork._UserRep.GetByIdAsync(18).Result;
            return View();
        }

        public IActionResult Privacy(string q)
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
