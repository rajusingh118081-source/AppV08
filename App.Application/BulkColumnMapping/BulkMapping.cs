using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.BulkColumnMapping
{
    public static class BulkMapping
    {
        public static BulkColumnMapping<T> Column<T>(
            string name,
            Type type,
            Func<T, object?> selector,
            bool isKey = false,
            bool update = true)
        {
            return new BulkColumnMapping<T>
            {
                ColumnName = name,
                DataType = type,
                ValueSelector = selector,
                IsKey = isKey,
                UpdateColumn = update
            };
        }
    }
}
