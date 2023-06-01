using DesignDB_Library.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    public class CompletionTimeModel
    {
        [ExcelExportPropertyAttribute]
        public string MSO { get; set; }
        [ExcelExportPropertyAttribute]
        public float  CompletedDesigns { get; set; }
        [ExcelExportPropertyAttribute]
        public float TotalDaysToComplete { get; set; }
        [ExcelExportPropertyAttribute]
        public string AvgDaysToComplete { get; set; }
        [ExcelExportPropertyAttribute]
        public float TotalDaysFromAllInfo { get; set; }
        [ExcelExportPropertyAttribute]
        public string AvgDaysFromAllInfo { get; set; }
        [ExcelExportPropertyAttribute]
        public string P1Average  { get; set; }
        [ExcelExportPropertyAttribute]
        public string P2Average { get; set; }
        [ExcelExportPropertyAttribute]
        public string P3Average { get; set; }
        [ExcelExportPropertyAttribute]
        public int CanceledDesigns { get; set; }

    }
}
