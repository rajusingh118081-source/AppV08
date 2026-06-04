using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.Requests
{
    public class UserSearchRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 100;

        public string? SearchText { get; set; }

        public bool? IsActive { get; set; }

        public string? SortBy { get; set; }

        public bool SortDescending { get; set; }
    }
}
