using App.Application.DTOs.Main_DTO;
using App.Application.IExternalRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.Servicelayer
{
    public class CustomerService
    {
        private readonly IQuickBooksService _qbService;

        public CustomerService(IQuickBooksService qbService)
        {
            _qbService = qbService;
        }

        public async Task<bool> CreateCustomer(Main_ContactsDto dto)
        {
            // 1. Save in DB
            // 2. Sync to QBO
            return await _qbService.SyncCustomerAsync(dto);
        }
    }
}
