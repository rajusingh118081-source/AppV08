using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.DTOs.QuickBookOnlineDTO.Customer
{
    public class QuickBooksCustomerResponse
    {
        public QuickBooksCustomerQueryResponse? QueryResponse { get; set; }

        public DateTime? Time { get; set; }
    }

    public class QuickBooksCustomerQueryResponse
    {
        public List<QuickBooksCustomerDto> Customer { get; set; }= new();

        public int StartPosition { get; set; }

        public int MaxResults { get; set; }
    }

}
