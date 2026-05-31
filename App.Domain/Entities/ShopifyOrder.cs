using System;
using System.Collections.Generic;
using System.Text;

namespace App.Domain.Entities
{
    public class ShopifyOrder
    {
        public long Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
    }
}
