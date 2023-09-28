using DesignDB_Library.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    public class OpenRequestsBySalesModel
    {
        [ExcelExportPropertyAttribute]
        public string Salesperson { get; set; }
        [ExcelExportPropertyAttribute]
        public string MSO { get; set; }
        [ExcelExportPropertyAttribute]
        public int Count { get; set; }
        [ExcelExportPropertyAttribute]
        public int Jan { get; set; }
        [ExcelExportPropertyAttribute]
        public int Feb { get; set; }
        [ExcelExportPropertyAttribute]
        public int Mar { get; set; }
        [ExcelExportPropertyAttribute]
        public int Apr { get; set; }
        public int May { get; set; }
        [ExcelExportPropertyAttribute]
        public int Jun { get; set; }
        [ExcelExportPropertyAttribute]
        public int Jul { get; set; }
        [ExcelExportPropertyAttribute]
        public int Aug { get; set; }
        [ExcelExportPropertyAttribute]
        public int Sep { get; set; }
        public int Oct { get; set; }
        [ExcelExportPropertyAttribute]
        public int Nov { get; set; }
        [ExcelExportPropertyAttribute]
        public int Dec { get; set; }
    }
}


