using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.GenericResponse
{
    public class Response
    {
        public string? Message { get; set; }
        public bool Status { get; set; }
        public object? ReturnResponse { get; set; }
    }
}
