using DesignDB_Library.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    public class SnapshotModel
    {
        [ExcelExportPropertyAttribute]
        public string MSO { get; set; }
        [ExcelExportPropertyAttribute]
        public int RequestsThisYear { get; set; }
        [ExcelExportPropertyAttribute]
        public int RequestsThisMonth { get; set; }
        [ExcelExportPropertyAttribute]
        public int RequestsThisWeek { get; set; }
        [ExcelExportPropertyAttribute]
        public Int64 TotalDaysToComplete { get; set; }
        [ExcelExportPropertyAttribute]
        public int TotalCompletedDesigns { get; set; }
        [ExcelExportPropertyAttribute]
        public int TotalOpenRequests { get; set; }
        [ExcelExportPropertyAttribute]
        public int TotalCanceledDesigns { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal TotalValue { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal AverageCompletionTime { get; set; }
    }
}
