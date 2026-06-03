using App.Application.DTOs.Main_DTO;
using App.Domain.Entities;
using App.Domain.Entities.Main_Model;
using App.Repository.DTOs.ShopifyDto;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.Mapping.Main_Mapping
{
    public class ContactProfiles : Profile
    {
        public ContactProfiles()
        {
            // Mapping for Main_Contacts and Main_ContactsDto
            CreateMap<Main_Contacts, Main_ContactsDto>();
            CreateMap<Main_ContactsDto, Main_Contacts>();
        }
    }
}
