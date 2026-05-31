using App.Application.DTOs.Main_DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.IInterface
{
    public interface IContactService
    {
        Task<IEnumerable<ContactDto>> GetAllAsync();
        Task<ContactDto?> GetByIdAsync(int id);
        Task<Guid> CreateAsync(ContactDto dto);
    }
}
