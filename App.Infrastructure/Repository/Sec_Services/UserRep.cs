using AapRepository;
using App.Application.DTOs.Sys_DTO;
using App.Application.IRepository.Sec_Rep;
using App.Application.Requests;
using App.Common.Pagination;
using App.Domain.Entities.Sec_Model;
using App.Infrastructure;
using App.Infrastructure.IRepository;
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
        public async Task<PageResult<Sec_UsersDTO>> ReadAllAsync(UserSearchRequest request)
        {
            var query = this.GetAllQueryable();   // 🔥 generic query

            // FILTER
            if (!string.IsNullOrEmpty(request.SearchText))
            {
                query = query.Where(x =>
                    x.Name.Contains(request.SearchText) ||
                    x.EmailAddress.Contains(request.SearchText));
            }

            // COUNT
            var totalRecords = await query.CountAsync();

            // PAGINATION + JOIN + DTO
            var data = await query
                .OrderBy(x => x.ID)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new Sec_UsersDTO
                {
                    ID = x.ID,
                    Name = x.Name,
                    EmailAddress = x.EmailAddress,
                    PasswordHash = x.PasswordHash,
                    //RoleName = x.Role.RoleName   // JOIN
                }).ToListAsync();

            return new PageResult<Sec_UsersDTO>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
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
}

