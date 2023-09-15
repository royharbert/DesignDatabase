using DesignDB_Library.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    public class RollupCompletionTimeModel
    {
        [ExcelExportPropertyAttribute]
        public string MSO { get; set; }
        [ExcelExportPropertyAttribute]
        public int CompletedDesigns { get; set; }
        [ExcelExportPropertyAttribute]
        public int DaysToComplete { get; set; }
        [ExcelExportPropertyAttribute]
        public float AvgDaysToComplete { get; set; }
        [ExcelExportPropertyAttribute]
        public int DaysFromAllInfo { get; set; }
        [ExcelExportPropertyAttribute]
        public float AvgDaysFromAllInfo { get; set; }
        [ExcelExportPropertyAttribute]
        public float P1Count { get; set; }
        [ExcelExportPropertyAttribute]
        public float P2Count { get; set; }
        [ExcelExportPropertyAttribute]
        public float P3Count { get; set; }
        [ExcelExportPropertyAttribute]
        public int CanceledDesigns { get; set; }
    }
}
