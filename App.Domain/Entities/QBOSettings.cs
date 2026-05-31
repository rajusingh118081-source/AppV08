using System;
using System.Collections.Generic;
using System.Text;

namespace App.Domain.Entities
{
    public class QBOSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }
        public string Environment { get; set; }
    }
}
