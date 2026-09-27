using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace App.Application.DTOs.QuickBookOnlineDTO.Invoice
{
    public class QuickBooksInvoiceDto
    {
        [JsonPropertyName("Id")]
        public string? Id { get; set; }

        [JsonPropertyName("DocNumber")]
        public string? DocNumber { get; set; }

        [JsonPropertyName("TxnDate")]
        public string? TxnDate { get; set; }

        [JsonPropertyName("TotalAmt")]
        public decimal? TotalAmt { get; set; }

        [JsonPropertyName("CustomerRef")]
        public QuickBooksReferenceDto? CustomerRef { get; set; }

        [JsonPropertyName("Line")]
        public List<QuickBooksInvoiceLineDto>? Line { get; set; }

        [JsonPropertyName("MetaData")]
        public QuickBooksMetaDataDto? MetaData { get; set; }
    }

    public class QuickBooksReferenceDto
    {
        [JsonPropertyName("value")]
        public string? Value { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    public class QuickBooksMetaDataDto
    {
        [JsonPropertyName("CreateTime")]
        public DateTime? CreateTime { get; set; }

        [JsonPropertyName("LastUpdatedTime")]
        public DateTime? LastUpdatedTime { get; set; }
    }

    public class QuickBooksInvoiceLineDto
    {
        [JsonPropertyName("Id")]
        public string? Id { get; set; }

        [JsonPropertyName("Description")]
        public string? Description { get; set; }

        [JsonPropertyName("Amount")]
        public decimal? Amount { get; set; }

        [JsonPropertyName("DetailType")]
        public string? DetailType { get; set; }

        [JsonPropertyName("SalesItemLineDetail")]
        public QuickBooksSalesItemLineDetailDto? SalesItemLineDetail { get; set; }
    }
    public class QuickBooksSalesItemLineDetailDto
    {
        [JsonPropertyName("ItemRef")]
        public QuickBooksReferenceDto? ItemRef { get; set; }

        [JsonPropertyName("Qty")]
        public decimal? Qty { get; set; }

        [JsonPropertyName("UnitPrice")]
        public decimal? UnitPrice { get; set; }

        [JsonPropertyName("TaxCodeRef")]
        public QuickBooksReferenceDto? TaxCodeRef { get; set; }
    }
}
