using System;
using System.Collections.Generic;
using System.Text;

namespace App.Domain.Entities.Main_Model
{
    public class Main_InvoiceLineItems
    {
        public int Id { get; set; }

        public int LinkedInvoiceID { get; set; }

        public string? QboLineId { get; set; }

        public string? QboItemId { get; set; }

        public string? Description { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? UnitPrice { get; set; }

        public decimal? Amount { get; set; }

        // Navigation property
        public Main_Invoices Invoice { get; set; } = null!;
    }

}
