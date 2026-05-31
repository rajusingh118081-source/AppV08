using App.Domain.Entities.Main_Model;
using App.Domain.Entities.Sec_Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Infrastructure.IRepository.Main_Rep
{
    public interface IContactRepository : IRepository<Contact> 
    {
        Task<Contact?> GetByEmailAsync(string email);
    }
}
