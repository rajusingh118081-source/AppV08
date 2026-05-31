using System;
using System.Collections.Generic;
using System.Text;

namespace App.Repository.DTOs.ShopifyDto
{
    public class ShopifyOrderDto
    {
        public long Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
    }
}
