using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.DTOs.QuickBookOnlineDTO.Customer
{
    public class QuickBooksCustomerDto
    {
        public bool? Taxable { get; set; }

        public QuickBooksAddressDto? BillAddr { get; set; }

        public QuickBooksAddressDto? ShipAddr { get; set; }

        public bool? Job { get; set; }

        public bool? BillWithParent { get; set; }

        public QuickBooksReferenceDto? ParentRef { get; set; }

        public int? Level { get; set; }

        public decimal? Balance { get; set; }

        public decimal? BalanceWithJobs { get; set; }

        public QuickBooksReferenceDto? CurrencyRef { get; set; }

        public string? PreferredDeliveryMethod { get; set; }

        public bool? IsProject { get; set; }

        public string? Domain { get; set; }

        public bool? Sparse { get; set; }

        public string? Id { get; set; }

        public string? SyncToken { get; set; }

        public QuickBooksMetaDataDto? MetaData { get; set; }

        public string? GivenName { get; set; }

        public string? MiddleName { get; set; }

        public string? FamilyName { get; set; }

        public string? FullyQualifiedName { get; set; }

        public string? CompanyName { get; set; }

        public string? DisplayName { get; set; }

        public string? PrintOnCheckName { get; set; }

        public bool? Active { get; set; }

        public string? V4IDPseudonym { get; set; }

        public QuickBooksPhoneDto? PrimaryPhone { get; set; }

        public QuickBooksPhoneDto? Mobile { get; set; }

        public QuickBooksPhoneDto? Fax { get; set; }

        public QuickBooksEmailDto? PrimaryEmailAddr { get; set; }

        public QuickBooksWebAddressDto? WebAddr { get; set; }

        public QuickBooksReferenceDto? DefaultTaxCodeRef { get; set; }
    }

}
