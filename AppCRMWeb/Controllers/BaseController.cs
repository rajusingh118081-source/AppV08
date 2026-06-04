using AapRepository;
using App.Domain.Entities.Sec_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace AppCRMWeb.Controllers
{
    public class BaseController : Controller
    {
        protected Sec_Users CurrentUser;
        private readonly IHttpContextAccessor _httpContext;
        public BaseController(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            var userIdClaim = _httpContext?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                //CurrentUser = await _unitOfWork._UserRep.GetByIdAsync(userId);
            }
            base.OnActionExecuting(context);
        }
    }
}
