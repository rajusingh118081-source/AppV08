using App.Application.DTOs.Sys_DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.IInterface
{
    public interface IRefSysDataManager
    {
        Task<IEnumerable<Sec_UsersDTO>> GetAllAsync();
    }
}
