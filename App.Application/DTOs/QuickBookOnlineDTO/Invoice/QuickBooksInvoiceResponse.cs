using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace App.Application.DTOs.QuickBookOnlineDTO.Invoice
{
    public class QuickBooksInvoiceResponse
    {
        [JsonPropertyName("QueryResponse")]
        public QuickBooksInvoiceQueryResponse? QueryResponse { get; set; }
    }

    public class QuickBooksInvoiceQueryResponse
    {
        [JsonPropertyName("Invoice")]
        public List<QuickBooksInvoiceDto>? Invoice { get; set; }
    }
}
