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
    public class ContactService : IContactService
    {
        private readonly IContactRepository _repo;
        private readonly IMapper _mapper;
        public ContactService(IContactRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContactDto>> GetAllAsync()
        {
            var contacts = await _repo.GetAllAsync();
            // 🔥 AutoMapper instead of manual mapping
            return _mapper.Map<IEnumerable<ContactDto>>(contacts);
        }

        public async Task<ContactDto?> GetByIdAsync(int id)
        {
            var contact = await _repo.GetByIdAsync(id);
            if (contact == null) return null;


            return contact == null ? null: _mapper.Map<ContactDto>(contact);
        }

        public async Task<Guid> CreateAsync(ContactDto dto)
        {
            var entity = _mapper.Map<Contact>(dto);

            await _repo.AddAsync(entity);
            return entity.Id;
        }
    }
}
