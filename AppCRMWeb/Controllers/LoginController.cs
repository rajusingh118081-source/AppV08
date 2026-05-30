using AapRepository;
using App.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AppCRMWeb.Controllers
{
    public class LoginController : BaseController
    {
        private readonly ILogger<LoginController> _logger;
        public LoginController(ILogger<LoginController> logger, IUnitOfWork unitOfWork, IHttpContextAccessor httpContext) : base(unitOfWork, httpContext)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            //if (_configSettings.Value.IsRedirectShopping == true)
            //{
            //    return RedirectToRoute(new { action = "Shop", controller = "Home", area = "Shop" });
            //}
            return View();
        }
    }
}
