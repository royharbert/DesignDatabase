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
        float _p1pct;
        float _p2pct;
        float _p3pct;

        [ExcelExportPropertyAttribute]
        public string SalesPerson { get; set; }
        [ExcelExportPropertyAttribute]
        public string MSO { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal P1Count { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal  P1Dollars { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal P2Count { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal P2Dollars { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal P3Count { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal P3Dollars { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal TotalCount { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal TotalDollars { get; set; }
        public float P1Pct
        {
            get 
            {
                _p1pct = (float)P1Count / (float)TotalCount;   
                return _p1pct; 
            }
        }
        public float P2Pct
        {
            get
            {
                _p2pct = (float)P2Count / (float)TotalCount;
                return _p2pct;
            }
        }

        public float P3Pct
        { 
            get
            {
                _p3pct = (float)P3Count / (float)TotalCount;
                return _p3pct;
        }
    }

    }
}
