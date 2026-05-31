using AapRepository;
using App.Domain.Entities.Sec_Model;
using App.Infrastructure;
using App.Infrastructure.IRepository;
using App.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Repository.Repository.Sec_Rep
{
    public class UserRep : Repository<Sec_Users>, IUserRep
    {
        public DB_Contexts _dbContext;

        public UserRep(DB_Contexts context) : base(context)
        {
            _dbContext = context;
        }
    }

    //#region add new user
    //public async Task<Response> AddorUpdate(Sec_Users users)
    //{
    //    Response _returnResponse = new Response();
    //    try
    //    {
    //        //if (users.ID.Equals(0))
    //        //{
    //        //    if (_auth.VerifyEmailAddress(users.EmailAddress).Equals(true))
    //        //    {
    //        //        if (!string.IsNullOrEmpty(users.PasswordHash))
    //        //            users.PasswordHash = new PasswordHasher().HashPassword(users.PasswordHash);

    //        //        this.Create(users);
    //        //        _returnResponse = new Response { Message = Model.Utility.Message.CreateUser, Status = true, ReturnResponse = users.ID };
    //        //    }
    //        //    else
    //        //    {
    //        //        _returnResponse = new Response { Message = Model.Utility.Message.EmailExist, Status = false, ReturnResponse = users.ID };
    //        //    }
    //        //}
    //        //else
    //        //{
    //        //    if (_auth.VerifyEmailAddress(users.EmailAddress).Equals(true))
    //        //    {
    //        //        this.Update(users);
    //        //        _returnResponse = new Response { Message = Model.Utility.Message.UpdateUser, Status = true, ReturnResponse = users.ID };
    //        //    }
    //        //    else
    //        //    {
    //        //        _returnResponse = new Response { Message = Model.Utility.Message.EmailExist, Status = false, ReturnResponse = users.ID };
    //        //    }
    //        //}
    //        //await _dbContext.SaveChangesAsync();
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //    return _returnResponse;
    //}
    //#endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="emailAddress"></param>
    /// <returns></returns>
    //public async Task<Sec_Users> FindAsync(string emailAddress)
    //{
    //    return await _dbContext.Sec_Users.Where(x => x.EmailAddress.ToLower() == emailAddress.ToLower()).FirstOrDefaultAsync();
    //}
}

