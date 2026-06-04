using App.Application.DTOs.Main_DTO;
using App.Application.DTOs.Sys_DTO;
using App.Application.Requests;
using App.Common.Pagination;
using App.Domain.Entities.Sec_Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.IRepository.Sec_Rep
{
    public interface IUserRep : IRepository<Sec_Users>
    {
        // 🔥 Asynchronous methods for better performance
        // 🔥 Return DTOs instead of entities for better abstraction
        /// <summary>
        /// Asynchronously retrieves all users from the database and returns them as a collection of Sec_UsersDTO objects.
        /// </summary>
        /// <returns></returns>
        Task<PageResult<Sec_UsersDTO>> ReadAllAsync(UserSearchRequest request);

        /// <summary>
        /// Asynchronously retrieves a user by their unique identifier (ID) from the database and returns it as a Sec_UsersDTO object. If the user is not found, it returns null.
        /// </summary>
        /// <param name="id">The unique identifier of the user to retrieve.</param>
        /// <returns>A Sec_UsersDTO object representing the user, or null if not found.</returns>
        //Task<Sec_UsersDTO?> GetByIdAsync(int id);

        /// <summary>
        /// Asynchronously creates a new user in the database using the provided Sec_UsersDTO object. It returns the unique identifier (ID) of the newly created user.
        /// </summary>
        /// <param name="dto">The Sec_UsersDTO object representing the user to create.</param>
        /// <returns>The unique identifier (ID) of the newly created user.</returns>
        //Task<int> CreateAsync(Sec_UsersDTO dto);
    }
}
