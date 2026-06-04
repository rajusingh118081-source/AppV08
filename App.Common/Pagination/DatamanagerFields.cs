
using App.Common.GenericResponse;

namespace App.Common.Pagination
{
    public class DatamanagerFields : ViewBaseEntity
    {
        public string ControllerName { get; set; }
        public string DataManagerTitle { get; set; }
        public string FieldName { get; set; }
        public string ColumnHeading { get; set; }
        public int ShowOrderNumber { get; set; }
        
        public string DefaultWidth { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsScreenType { get; set; }
    }
}
