using App.Application.DTOs.Main_DTO;
using App.Application.IInterface;
using App.Domain.Entities.Main_Model;
using App.Infrastructure.IRepository.Main_Rep;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.Services.Main_Services
{
    public class ContactService : IContacts
    {
        private readonly IContactRep _repo;
        private readonly IMapper _mapper;
        public ContactService(IContactRep repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Main_ContactsDto>> GetAllAsync()
        {
            var contacts = await _repo.GetAllAsync();
            // 🔥 AutoMapper instead of manual mapping
            return _mapper.Map<IEnumerable<Main_ContactsDto>>(contacts);
        }

        public async Task<Main_ContactsDto?> GetByIdAsync(int id)
        {
            var contact = await _repo.GetByIdAsync(id);
            if (contact == null) return null;


            return contact == null ? null: _mapper.Map<Main_ContactsDto>(contact);
        }

        public async Task<int> CreateAsync(Main_ContactsDto dto)
        {
            var entity = _mapper.Map<Main_Contacts>(dto);

            await _repo.AddAsync(entity);
            return entity.ID;
        }
    }
}
