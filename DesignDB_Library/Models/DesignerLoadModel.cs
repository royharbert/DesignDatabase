using DesignDB_Library.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    public class DesignerLoadModel
    {
        String pID = null;
        [ExcelExportPropertyAttribute]
        public string Designer { get; set; }
        [ExcelExportPropertyAttribute]
        public string Pty { get; set; }
        [ExcelExportPropertyAttribute]
        public DateTime DateAssigned { get; set; }
        [ExcelExportPropertyAttribute]
        public DateTime DateDue { get; set; }
        [ExcelExportPropertyAttribute]
        public string ProjID { get; set; }
        [ExcelExportPropertyAttribute]
        public string ProjectID
        {
            get
            {
                return pID;
            }
            set
            {
                pID = value;
                this.ProjID = value.Substring(0, value.Length - 6); 
            }

        }
        [ExcelExportPropertyAttribute]
        public string AwardStatus { get; set; }
    }
}
