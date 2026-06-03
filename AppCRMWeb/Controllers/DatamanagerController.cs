using AapRepository;
using App.Application.ExternalServices.QBDSync;
using AppCRMWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppCRMWeb.Controllers
{
    public class DatamanagerController : BaseController
    {
        public DataManager _dataManagerFields;
        private readonly ILogger<DatamanagerController> _logger;
        private readonly IQuickBooks _quickBooks;
        public DatamanagerController(ILogger<DatamanagerController> logger, IUnitOfWork unitOfWork, IHttpContextAccessor httpContext,
            IQuickBooks quickBooks) : base(unitOfWork, httpContext)
        {
            _logger = logger;
            _quickBooks = quickBooks;
        }
        //public IActionResult Read(string view)
        //{
        //    //var dataManagerFields = unitOfWork._SysDataManagerFields.GetDataManagerFields(view).Result;
        //    _dataManagerFields._ListData = dataManagerFields.ToList();
        //    _dataManagerFields.ControllerName = view;
        //    return View(_dataManagerFields);
        //}
        //public IActionResult ReturnView(string view, int refID)
        //{
        //    var dataManagerFields = unitOfWork._SysDataManagerFields.GetDataManagerFields(view).Result;
        //    _dataManagerFields._ListData = dataManagerFields.ToList();
        //    _dataManagerFields.ControllerName = view;
        //    _dataManagerFields.RefID = refID;
        //    return PartialView("_Read", _dataManagerFields);
        //}
    }

}
