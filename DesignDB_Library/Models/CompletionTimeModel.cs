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
        public int  CompletedDesigns { get; set; }
        [ExcelExportPropertyAttribute]
        public int TotalDaysToComplete { get; set; }
        [ExcelExportPropertyAttribute]
        public string AvgDaysToComplete { get; set; }
        [ExcelExportPropertyAttribute]
        public int OpenDesigns { get; set; }
        [ExcelExportPropertyAttribute]
        public int OpenDays { get; set; }
        [ExcelExportPropertyAttribute]
        public string AverageOpenDays { get; set; }
        [ExcelExportPropertyAttribute]
        public int CanceledDesigns { get; set; }

    }
}
