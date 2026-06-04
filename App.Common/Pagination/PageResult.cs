using System;
using System.Collections.Generic;
using System.Text;

namespace App.Common.Pagination
{
    public class PageResult<T>
    {
        public List<T> Data { get; set; } = new();

        public long TotalRecords { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
    }
}
