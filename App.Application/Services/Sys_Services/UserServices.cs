using App.Application.DTOs.Main_DTO;
using App.Application.DTOs.Sys_DTO;
using App.Application.IInterface;
using App.Application.Model;
using App.Domain.Entities.Main_Model;
using App.Domain.Entities.Sec_Model;
using App.Infrastructure.IRepository;
using App.Infrastructure.IRepository.Main_Rep;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.Services.Sys_Services
{
    public class UserServices : IUser
    {
        #region Constructor and Dependencies 
        private readonly IUserRep _repo;
        private readonly IMapper _mapper;
        public UserServices(IUserRep repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        #endregion

        #region GetAllAsync 
        public async Task<IEnumerable<Sec_UsersDTO>> GetAllAsync()
        {
            var users = await _repo.GetAllAsync();
            // 🔥 AutoMapper instead of manual mapping
            return _mapper.Map<IEnumerable<Sec_UsersDTO>>(users);


        }
        #endregion
    //    public async Task<PagedResult<Sec_UsersDTO>> GetPagedAsync(
    //UserSearchRequest request)
    //    {
    //        var query = _context.Sec_Users
    //            .AsNoTracking()
    //            .AsQueryable();

    //        // Global Search
    //        if (!string.IsNullOrWhiteSpace(request.SearchText))
    //        {
    //            query = query.Where(x =>
    //                x.UserName.Contains(request.SearchText) ||
    //                x.Email.Contains(request.SearchText));
    //        }

    //        // Filter
    //        if (request.IsActive.HasValue)
    //        {
    //            query = query.Where(x =>
    //                x.IsActive == request.IsActive.Value);
    //        }

    //        // Dynamic Sorting
    //        query = request.SortBy?.ToLower() switch
    //        {
    //            "username" => request.SortDescending
    //                ? query.OrderByDescending(x => x.UserName)
    //                : query.OrderBy(x => x.UserName),

    //            "email" => request.SortDescending
    //                ? query.OrderByDescending(x => x.Email)
    //                : query.OrderBy(x => x.Email),

    //            _ => query.OrderBy(x => x.UserId)
    //        };

    //        var totalRecords = await query.CountAsync();

    //        var data = await query
    //            .Skip((request.PageNumber - 1) * request.PageSize)
    //            .Take(request.PageSize)
    //            .Select(x => new Sec_UsersDTO
    //            {
    //                UserId = x.UserId,
    //                UserName = x.UserName,
    //                Email = x.Email
    //            })
    //            .ToListAsync();

    //        return new PagedResult<Sec_UsersDTO>
    //        {
    //            Data = data,
    //            TotalRecords = totalRecords,
    //            PageNumber = request.PageNumber,
    //            PageSize = request.PageSize
    //        };
    //    }
        public async Task<PagedResult<Sec_UsersDTO>> GetPagedAsync(UserSearchRequest request)
        {
            var users = await _repo.GetAllAsync();
            var data = _mapper.Map<IEnumerable<Sec_UsersDTO>>(users);

            // Convert to IQueryable first, then call AsNoTracking
            var query = data.AsQueryable().AsNoTracking();
            // Search by UserName
            //if (!string.IsNullOrWhiteSpace(request.UserName))
            //{
            //    query = query.Where(x =>
            //        x.UserName.Contains(request.UserName));
            //}

            //// Search by Email
            //if (!string.IsNullOrWhiteSpace(request.Email))
            //{
            //    query = query.Where(x =>
            //        x.Email.Contains(request.Email));
            //}
            // (optional) re-enable search filters against query if needed

            var totalRecords = await query.CountAsync();

            var pagedUsers = await query
                .OrderBy(x => x.ID)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new PagedResult<Sec_UsersDTO>
            {
                Data = pagedUsers,
                TotalRecords = totalRecords,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        #region GetByIdAsync with AutoMapper
        /// <summary>
        /// Get user by ID with AutoMapper for clean mapping
        /// </summary>
        /// <param name="id">The unique identifier of the user to retrieve.</param>
        /// <returns>A Sec_UsersDTO object representing the user, or null if not found.</returns>

        public async Task<Sec_UsersDTO?> GetByIdAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return null;


            return user == null ? null : _mapper.Map<Sec_UsersDTO>(user);
        }
        #endregion

        #region CreateAsync with AutoMapper
        public async Task<int> CreateAsync(Sec_UsersDTO dto)
        {
            var entity = _mapper.Map<Sec_Users>(dto);

            await _repo.AddAsync(entity);
            return entity.ID;
        }
        #endregion
    }

    public class UserSearchRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 100;

        public string? SearchText { get; set; }

        public bool? IsActive { get; set; }

        public string? SortBy { get; set; }

        public bool SortDescending { get; set; }
    }
}
