using AapRepository;
using App.Domain.Entities.Ref_Model;
using App.Domain.Entities.Sec_Model;
using App.Infrastructure.IRepository;
using App.Infrastructure.IRepository.Ref_Rep;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Infrastructure.Repository.Ref_Services
{
    public class RefSysDataManagerRep : Repository<Ref_SysDataManagerFields>, IRefSysDataManagerRep
    {
        public DB_Contexts _dbContext;

        public RefSysDataManagerRep(DB_Contexts context) : base(context)
        {
            _dbContext = context;
        }
    }
}
