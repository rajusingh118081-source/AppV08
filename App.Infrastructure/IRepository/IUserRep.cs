using AapRepository;
using App.Domain.Entities.Sec_Model;
using App.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Infrastructure.IRepository
{
    public interface IUserRep : IRepository<Sec_Users>
    {
        /// <summary>
        /// This method is used to add or update user data.
        /// </summary>
        /// <param name="sec_Users"></param>
        /// <returns></returns>
        //Task<Response> AddorUpdate(Sec_Users sec_Users);
        //Task<Sec_Users> FindAsync(string emailAddress);
    }
}
