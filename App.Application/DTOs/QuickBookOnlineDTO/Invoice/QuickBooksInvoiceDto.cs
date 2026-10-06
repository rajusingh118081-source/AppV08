using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace App.Application.DTOs.QuickBookOnlineDTO.Invoice
{
    public class QuickBooksInvoiceDto
    {
        public string? Id { get; set; }

        public string? SyncToken { get; set; }

        public string? DocNumber { get; set; }

        public string? TxnDate { get; set; }

        public string? DueDate { get; set; }

        public decimal? TotalAmt { get; set; }

        public decimal? Balance { get; set; }
        public decimal? Subtotal { get; set; }

        public QuickBooksReferenceDto? CustomerRef { get; set; }

        public QuickBooksReferenceDto? CurrencyRef { get; set; }

        public string? PrivateNote { get; set; }

        public string? TxnStatus { get; set; }

        public List<QuickBooksInvoiceLineDto>? Line { get; set; }

        public QuickBooksMetaDataDto? MetaData { get; set; }
        public QuickBooksTxnTaxDetailDto? TxnTaxDetail { get; set; }
    }

    public class QuickBooksReferenceDto
    {
        public string? Value { get; set; }

        public string? Name { get; set; }
    }

    public class QuickBooksMetaDataDto
    {
        public DateTime? CreateTime { get; set; }

        public DateTime? LastUpdatedTime { get; set; }
    }

    public class QuickBooksInvoiceLineDto
    {
        public string? Id { get; set; }

        public string? Description { get; set; }

        public decimal? Amount { get; set; }

        public string? DetailType { get; set; }

        public QuickBooksSalesItemLineDetailDto?SalesItemLineDetail{ get; set; }

        // Set this from parent invoice
        public string? InvoiceQuickBooksId { get; set; }
    }

    public class QuickBooksSalesItemLineDetailDto
    {
        public QuickBooksReferenceDto? ItemRef { get; set; }

        public decimal? Qty { get; set; }

        public decimal? UnitPrice { get; set; }

        public QuickBooksReferenceDto? TaxCodeRef { get; set; }
    }

    public class QuickBooksTxnTaxDetailDto
    {
        public QuickBooksReferenceDto? TxnTaxCodeRef { get; set; }

        public decimal? TotalTax { get; set; }

        public List<QuickBooksTaxLineDto>? TaxLine { get; set; }
    }
    public class QuickBooksTaxLineDetailDto
    {
        public decimal? NetAmountTaxable { get; set; }

        public decimal? TaxPercent { get; set; }

        public QuickBooksReferenceDto? TaxRateRef { get; set; }

        public bool? PercentBased { get; set; }
    }
    public class QuickBooksTaxLineDto
    {
        public string? DetailType { get; set; }

        public decimal? Amount { get; set; }

        public QuickBooksTaxLineDetailDto? TaxLineDetail { get; set; }
    }
}
