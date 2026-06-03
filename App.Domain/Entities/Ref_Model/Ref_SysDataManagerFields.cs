using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Domain.Entities.Ref_Model
{
    public class Ref_SysDataManagerFields:BaseEntity
    {
        [Column(TypeName = "nvarchar(200)")]
        [StringLength(200)]
        [Display(Name = "Table Name")]
        [Required]
        public required string TableName { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        [StringLength(200)]
        [Display(Name = "Title Name")]
        public string DataManagerTitle { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [StringLength(100)]
        [Display(Name = "Field Name")]
        [Required]
        public required string FieldName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [StringLength(100)]
        [Display(Name = "Column Heading")]
        [Required]
        public required string ColumnHeading { get; set; }

        [Display(Name = "Order Number")]
        [Required]
        public required int ShowOrderNumber { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [StringLength(100)]
        [Display(Name = "Default Width")]
        public string DefaultWidth { get; set; }

        [Display(Name = "IsReadOnly")]
        public bool IsReadOnly { get; set; }
        [Display(Name = "IsScreenType")]
        public bool IsScreenType { get; set; }
    }
}
