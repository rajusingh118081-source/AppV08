using App.Application.DTOs.Main_DTO;
using App.Application.DTOs.QuickBookOnlineDTO.Customer;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.IExternalRepository.QuickBookOnline
{
    public interface IQuickBooksCustomerRep
    {
        Task<List<QuickBooksCustomerDto>> GetCustomersAsync();
    }

}
