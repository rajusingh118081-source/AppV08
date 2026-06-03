using App.Application.DTOs.Main_DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.IInterface
{
    public interface IContacts
    {
        /// <summary>
        /// Get all contacts
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Main_ContactsDto>> GetAllAsync();

        /// <summary>
        /// Get contact by id
        /// </summary>
        /// <param name="id">The unique identifier of the contact to retrieve.</param>
        /// <returns>A Main_ContactsDto object representing the contact, or null if not found.</returns>
        Task<Main_ContactsDto?> GetByIdAsync(int id);
        /// <summary>
        /// This method creates a new contact in the system. It takes a Main_ContactsDto object as input, which contains all the necessary information about the contact to be created. The method returns an integer representing the ID of the newly created contact. This allows the caller to easily reference the new contact for further operations, such as updating or deleting it in the future.
        /// </summary>
        /// <param name="dto">The Main_ContactsDto object representing the contact to create.</param>
        /// <returns>The unique identifier (ID) of the newly created contact.</returns>
        Task<int> CreateAsync(Main_ContactsDto dto);
    }
}
