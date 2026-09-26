using System;
using System.Collections.Generic;
using System.Text;

namespace App.Domain.Entities.Main_Model
{
    public class Main_Invoices: BaseEntity
    {
        public string QboInvoiceID { get; set; } = null!;

        public string CustomerQboID { get; set; }

        public DateTime TxnDate { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Main_InvoiceLineItems> LineItems { get; set; } = new List<Main_InvoiceLineItems>();
    }
}
