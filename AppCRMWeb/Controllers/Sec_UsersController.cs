using AapRepository;
using Microsoft.AspNetCore.Mvc;

namespace AppCRMWeb.Controllers
{
    public class Sec_UsersController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        public Sec_UsersController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IHttpContextAccessor httpContext) : base(unitOfWork, httpContext)
        {
            _logger = logger;
        }
        public IActionResult Details()
        {
            return View();
        }
    }
}
