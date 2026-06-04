using AapRepository;
using App.Application.IRepository.Ref_Rep;
using App.Common.Pagination;
using App.Common.Utilities;
using App.Domain.Entities.Ref_Model;
using App.Domain.Entities.Sec_Model;
using App.Infrastructure.IRepository;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<DatamanagerFields>> GetDataManagerFields(string controllerName)
        {
            List<DatamanagerFields> dataManagerFields = new List<DatamanagerFields>();
            try
            {
                var data = await (from tempData in this.GetAllQueryable()
                        .Where(x => x.TableName == controllerName)
                        .OrderBy(x => x.ShowOrderNumber)
                    join addUser in _dbContext.Sec_Users
                    on tempData.RefAddedByID equals addUser.ID into addUsers
                    from addUsersResult in addUsers.DefaultIfEmpty()
                    join editUser in _dbContext.Sec_Users
                    on tempData.RefEditedByID equals editUser.ID into editUsers
                    from editUsersResult in editUsers.DefaultIfEmpty()
                    select new
                    {
                        tempData,
                        AddUserName = addUsersResult != null ? addUsersResult.Name : "",
                        EditUserName = editUsersResult != null ? editUsersResult.Name : ""
                    }
                ).ToListAsync();
                dataManagerFields = data.Select(x => new DatamanagerFields
                {
                    ID = x.tempData.ID,
                    ControllerName = x.tempData.TableName,
                    DataManagerTitle = x.tempData.DataManagerTitle,
                    ShowOrderNumber = x.tempData.ShowOrderNumber,
                    FieldName = x.tempData.FieldName,
                    ColumnHeading = x.tempData.ColumnHeading,
                    DefaultWidth = x.tempData.DefaultWidth,
                    IsReadOnly = x.tempData.IsReadOnly,
                    IsScreenType = x.tempData.IsScreenType,
                    UniqueNumber = EncryptOrDecrypt.Encrypt(x.tempData.ID.ToString()),
                    RefAddedByID = x.tempData.RefAddedByID,
                    RefEditedByID = x.tempData.RefEditedByID,
                    RefAddedBy = x.AddUserName,
                    RefEditedBy = x.EditUserName,
                    AddedDate = x.tempData.AddedDate.ToString("MM/dd/yyyy"),
                    EditedDate = x.tempData.EditedDate.ToString("MM/dd/yyyy")
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dataManagerFields;
        }
    }
}
