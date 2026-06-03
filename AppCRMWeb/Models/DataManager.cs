using App.Domain;

namespace AppCRMWeb.Models
{
    public class DataManager : ViewBaseEntity
    {
        public DataManager()
        {
            this._ListData = new List<DataManager>();
        }

        public string ControllerName { get; set; }
        public string DataManagerTitle { get; set; }
        public string FieldName { get; set; }
        public string ColumnHeading { get; set; }
        public int ShowOrderNumber { get; set; }
        public int RefID { get; set; }
        public string DefaultWidth { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsScreenType { get; set; }
        public List<DataManager> _ListData { get; set; }
    }
}
