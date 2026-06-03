using App.Application.DTOs.Main_DTO;
using App.Application.DTOs.Sys_DTO;
using App.Domain.Entities.Main_Model;
using App.Domain.Entities.Sec_Model;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.Mapping.Sec_Mapping
{
    public class Sec_Profiles : Profile
    {
        public Sec_Profiles()
        {
            // Mapping for Sec_Users and Sec_UsersDTO
            CreateMap<Sec_Users, Sec_UsersDTO>();
            CreateMap<Sec_UsersDTO, Sec_Users>();
        }
    }
}
