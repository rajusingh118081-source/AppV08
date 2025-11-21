using App.Model;
using App.Model.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AapRepository.IRepository
{
    public interface IUserRep : IRepository<Sec_Users>
    {
        /// <summary>
        /// This method is used to add or update user data.
        /// </summary>
        /// <param name="sec_Users"></param>
        /// <returns></returns>
        //Task<Response> AddorUpdate(Sec_Users sec_Users);
        Task<Sec_Users> FindAsync(string emailAddress);
    }
}
