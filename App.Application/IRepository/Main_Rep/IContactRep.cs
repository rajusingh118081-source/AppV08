using App.Application;
using App.Domain.Entities.Main_Model;
using App.Domain.Entities.Sec_Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Infrastructure.IRepository.Main_Rep
{
    public interface IContactRep : IRepository<Main_Contacts> 
    {
        Task<Main_Contacts?> GetByEmailAsync(string email);
    }
}
