using DesignDB_Library.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    public class ReportSalesPriorityModel
    {
        [ExcelExportPropertyAttribute]
        public string SalesPerson { get; set; }
        [ExcelExportPropertyAttribute]
        public int P1Count { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal  P1Dollars { get; set; }
        [ExcelExportPropertyAttribute]
        public int P2Count { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal P2Dollars { get; set; }
        [ExcelExportPropertyAttribute]
        public int P3Count { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal P3Dollars { get; set; }
        [ExcelExportPropertyAttribute]
        public int TotalCount { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal TotalDollars { get; set; }
    }
}
