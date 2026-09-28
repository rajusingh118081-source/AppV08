using System;
using System.Collections.Generic;
using System.Text;

namespace App.Common
{
    public sealed class BulkUpsertResult
    {
        public int TotalProcessed { get; set; }
        public int TotalInserted { get; set; }
        public int TotalUpdated { get; set; }
        public int BatchCount { get; set; }
    }
}
