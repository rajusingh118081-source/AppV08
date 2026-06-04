using App.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.Application.DTOs.Sys_DTO
{
    public class Sec_UsersDTO: ViewBaseEntity
    {
        [Column(TypeName = "nvarchar(50)")]
        [StringLength(50)]
        public string? FirstName { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        [StringLength(50)]
        public string? LastName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [StringLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        [StringLength(50)]
        [Display(Name = "Email Address")]
        public  string EmailAddress { get; set; }
        [Display(Name = "User Type")]
        public int RefUserTypeID { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        [Display(Name = "Password")]
        public  string PasswordHash { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        [Display(Name = "RefreshToken")]
        public string? RefreshToken { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
