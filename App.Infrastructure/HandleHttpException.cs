using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace App.Infrastructure
{
    public class HandleHttpException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string ResponseBody { get; }

        public HandleHttpException(HttpStatusCode statusCode,string responseBody): base($"HTTP {(int)statusCode} ({statusCode})")
        {
            StatusCode = statusCode;
            ResponseBody = responseBody;
        }
    }
}
