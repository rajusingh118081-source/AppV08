using System;
using System.Collections.Generic;
using System.Text;

namespace App.Domain.Entities.Main_Model
{
    public class Main_Invoices
    {
        public int Id { get; set; }

        public string QboInvoiceId { get; set; } = null!;

        public string? CustomerQboId { get; set; }

        public DateTime? TxnDate { get; set; }

        public decimal? TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Main_InvoiceLineItems> LineItems { get; set; } = new List<Main_InvoiceLineItems>();
    }
}
