using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.BulkColumnMapping
{
    public sealed class BulkColumnMapping<T>
    {
        public string ColumnName { get; init; } = string.Empty;

        public Type DataType { get; init; } = typeof(string);

        public Func<T, object?> ValueSelector { get; init; } = default!;

        public bool IsKey { get; init; }

        public bool UpdateColumn { get; init; } = true;
    }
}
