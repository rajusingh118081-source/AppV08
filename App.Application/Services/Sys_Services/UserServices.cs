using App.Application.DTOs.Main_DTO;
using App.Application.DTOs.Sys_DTO;
using App.Application.IInterface;
using App.Domain.Entities.Main_Model;
using App.Domain.Entities.Sec_Model;
using App.Infrastructure.IRepository;
using App.Infrastructure.IRepository.Main_Rep;
using AutoMapper;
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
}
