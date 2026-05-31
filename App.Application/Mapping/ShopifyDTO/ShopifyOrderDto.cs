using App.Domain.Entities;
using App.Repository.DTOs.ShopifyDto;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.Mapping.ShopifyDTO
{
    public class ShopifyProfile : Profile
    {
        public ShopifyProfile()
        {
            CreateMap<ShopifyOrder, ShopifyOrderDto>();
            CreateMap<ShopifyOrderDto, ShopifyOrder>();
        }
    }
}
