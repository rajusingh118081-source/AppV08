using AapRepository;
using App.Application.IExternalRepository;
using App.Application.IRepository.Ref_Rep;
using App.Common.Pagination;
using App.Infrastructure.Repository.Ref_Services;
using Microsoft.AspNetCore.Mvc;

namespace AppCRMWeb.Controllers
{
    public class DatamanagerController : BaseController
    {
        public DataManager _dataManager;
        private readonly ILogger<DatamanagerController> _logger;
        private readonly IRefSysDataManagerRep _dataManagerRep;
        public DatamanagerController(ILogger<DatamanagerController> logger, IHttpContextAccessor httpContext,
            IRefSysDataManagerRep dataManagerRep) : base(httpContext)
        {
            _logger = logger;
            _dataManagerRep = dataManagerRep;
            _dataManager = new DataManager();
        }
        public IActionResult Read(string view)
        {
            var dataManagerFields = _dataManagerRep.GetDataManagerFields(view).Result;
            _dataManager._ListData = dataManagerFields.ToList();
            _dataManager.ControllerName = view;

            return View(_dataManager);
        }
        public IActionResult ReturnView(string view, int refID)
        {
            var dataManagerFields = _dataManagerRep.GetDataManagerFields(view).Result;
            _dataManager._ListData = dataManagerFields.ToList();
            _dataManager.ControllerName = view;
            _dataManager.RefID = refID;
            return PartialView("_Read", _dataManager);
        }
    }

}
