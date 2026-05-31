using System;
using System.Collections.Generic;
using System.Text;

namespace App.Domain.Entities.Main_Model
{
    public class Contact:BaseEntity
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
    }
}
