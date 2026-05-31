using App.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.DTOs.Main_DTO
{
    public class ContactDto:BaseEntity
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;

    }
}
