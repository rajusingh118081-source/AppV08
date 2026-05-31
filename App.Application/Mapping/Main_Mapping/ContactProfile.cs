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
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            CreateMap<Contact, ContactDto>();
            CreateMap<ContactDto, Contact>();
        }
    }
}
