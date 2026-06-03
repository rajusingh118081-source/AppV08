using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Domain.Entities.Main_Model
{
    public class Main_Contacts:BaseEntity
    {
  
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        [StringLength(50)]

        public string AccountNumber { get; set; }
        public int? PrimaryContactID { get; set; }

        [NotMapped]
        public string PrimaryContact { get; set; }

        public byte[] ContactPhoto { get; set; }
        [Required]
        public int? RefContactTypeID { get; set; }
        public int? RefRegionID { get; set; }
        public int RefAssignedToID { get; set; }

        public int? RefCampaignID { get; set; }

        public Boolean IsBillWithParent { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        [StringLength(200)]
        public string ContactName { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        [StringLength(20)]
        public string Prefix { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [StringLength(50)]
        public string MiddleName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Column(TypeName = "nvarchar(120)")]
        [StringLength(120)]
        public string JobTitle { get; set; }

        [Column(TypeName = "nvarchar(120)")]
        [StringLength(120)]
        public string CompanyName { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(120)")]
        [StringLength(120)]
        public string DisplayName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string BillAddressLine1 { get; set; }

        [Display(Name = "Address2")]
        [Column(TypeName = "nvarchar(100)")]
        public string BillAddressLine2 { get; set; }

        [Display(Name = "Address3")]
        [Column(TypeName = "nvarchar(100)")]
        public string BillAddressLine3 { get; set; }

        [Display(Name = "City")]
        [Column(TypeName = "nvarchar(50)")]
        [StringLength(50)]
        public string BillCity { get; set; }

        [Display(Name = "State/Province")]
        [Column(TypeName = "nvarchar(50)")]
        [StringLength(50)]
        public string BillStateProvince { get; set; }

        [Display(Name = "Zip/Postal Code")]
        [Column(TypeName = "nvarchar(10)")]
        [StringLength(10)]
        public string BillZipPostalCode { get; set; }

        [Display(Name = "Country")]
        public int BillCountry { get; set; }


        [Display(Name = "Address")]
        [Column(TypeName = "nvarchar(100)")]
        public string ShipAddressLine1 { get; set; }

        [Display(Name = "Address2")]
        [Column(TypeName = "nvarchar(100)")]
        public string ShipAddressLine2 { get; set; }

        [Display(Name = "Address3")]
        [Column(TypeName = "nvarchar(100)")]
        public string ShipAdressLine3 { get; set; }

        [Display(Name = "City")]
        [Column(TypeName = "nvarchar(50)")]
        [StringLength(50)]
        public string ShipCity { get; set; }

        [Display(Name = "State/Province")]
        [Column(TypeName = "nvarchar(50)")]
        [StringLength(50)]
        public string ShipStateProvince { get; set; }

        [Display(Name = "Zip/Postal Code")]
        [Column(TypeName = "nvarchar(10)")]
        [StringLength(10)]
        public string ShipZipPostalCode { get; set; }

        [Display(Name = "Country")]
        public int ShipCountry { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }


        [Display(Name = "Total Billed")]
        [Column(TypeName = "DECIMAL(20, 4)")]
        public decimal TotalBilled { get; set; }

        [Display(Name = "Total Paid")]
        [Column(TypeName = "DECIMAL(20, 4)")]
        public decimal TotalPaid { get; set; }

        [Display(Name = "Total Balance Due")]
        [Column(TypeName = "DECIMAL(20, 4)")]
        public decimal TotalBalanceDue { get; set; }

        [Display(Name = "Sales Rep")]
        public int SalesRep { get; set; }

    }
}
