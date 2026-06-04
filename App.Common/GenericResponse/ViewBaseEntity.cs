using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace App.Common.GenericResponse
{
    public class ViewBaseEntity
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
