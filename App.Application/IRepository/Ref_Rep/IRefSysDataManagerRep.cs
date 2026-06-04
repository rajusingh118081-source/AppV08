using App.Application;
using App.Common.Pagination;
using App.Domain.Entities.Main_Model;
using App.Domain.Entities.Ref_Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.IRepository.Ref_Rep
{
    public interface IRefSysDataManagerRep : IRepository<Ref_SysDataManagerFields>
    {
        Task<List<DatamanagerFields>> GetDataManagerFields(string controllerName);
    }
}
