
using App.Common.GenericResponse;

namespace App.Common.Pagination
{
    public class DataManager 
    {
        public DataManager()
        {
            this._ListData = new List<DatamanagerFields>();
        }
        public string ControllerName { get; set; }
        public int RefID { get; set; }
        public List<DatamanagerFields> _ListData { get; set; }
    }
}
