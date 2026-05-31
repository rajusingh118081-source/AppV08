using AapRepository;
using App.Application.ExternalServices.QBDSync;
using Microsoft.AspNetCore.Mvc;

namespace AppCRMWeb.Controllers
{
    public class Sec_UsersController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IQuickBooks _quickBooks;
        public Sec_UsersController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IHttpContextAccessor httpContext,
            IQuickBooks quickBooks) : base(unitOfWork, httpContext)
        {
            _logger = logger;
            _quickBooks = quickBooks;
        }
        public IActionResult Details()
        {
            return View();
        }
    }
}
