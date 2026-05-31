using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public class BaseEntity
    {
        [Key]
        public int ID { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        [StringLength(50)]
        [Display(Name = "Unique Number")]
        public required string UniqueNumber { get; set; }

        [Display(Name = "Inactive")]
        public bool IsInactive { get; set; }

        public int RefAddedByID { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime AddedDate { get; set; } = DateTime.Now;

        public int RefEditedByID { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime EditedDate { get; set; } = DateTime.Now;
        [NotMapped]
        public string? Category { get; set; }
        [NotMapped]
        public string? Description { get; set; }
    }
    public class ViewModelBaseEntity
    {
        [Key]
        public int ID { get; set; }
        public string? UniqueNumber { get; set; }

        [Display(Name = "Inactive")]
        public bool IsInactive { get; set; }

        public int RefAddedByID { get; set; }

        public string? AddedDate { get; set; }

        public int RefEditedByID { get; set; }
        public string? RefEditedBy { get; set; }
        public string? RefAddedBy { get; set; }

        public string? EditedDate { get; set; }
    }
}
