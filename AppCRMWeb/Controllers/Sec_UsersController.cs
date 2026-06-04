using AapRepository;
using App.Application.IRepository.Sec_Rep;
using App.Application.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AppCRMWeb.Controllers
{
    public class Sec_UsersController : BaseController
    {
        private readonly ILogger<Sec_UsersController> _logger;
        IUserRep _userRep;
        public Sec_UsersController(ILogger<Sec_UsersController> logger, IHttpContextAccessor httpContext, IUserRep userRep) : base(httpContext)
        {
            _logger = logger;
            _userRep = userRep;
        }
        public IActionResult Details()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable(int refID)
        {
            try
            {
                UserSearchRequest request = new UserSearchRequest();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                var sku = Request.Form["columns[4][search][value]"].FirstOrDefault();
                var name = Request.Form["columns[5][search][value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                //UserSearchRequest request = new UserSearchRequest();
                request.PageSize = pageSize;
                request.PageNumber = skip;
                var returnData = await _userRep.ReadAllAsync(request);
               

                //var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(returnData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
