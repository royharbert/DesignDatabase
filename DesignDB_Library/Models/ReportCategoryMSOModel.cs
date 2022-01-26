using DesignDB_Library.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    public class ReportCategoryMSOModel
    {
        [ExcelExportPropertyAttribute]
        public string MSO { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal TotalDollars { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal AverageDollarsPerRequest { get; set; }
        [ExcelExportPropertyAttribute]
        public int TotalRequests { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal PctOfTotal { get; set; }
        [ExcelExportPropertyAttribute]
        public int HFC { get; set; }
        [ExcelExportPropertyAttribute]
        public int NodeSplit { get; set; }
        [ExcelExportPropertyAttribute]
        public int RFoG { get; set; }
        [ExcelExportPropertyAttribute]
        public int PON { get; set; }
        [ExcelExportPropertyAttribute]
        public int FiberDeep { get; set; }
        [ExcelExportPropertyAttribute]
        public int DataTrans { get; set; }
        [ExcelExportPropertyAttribute]
        public int Other { get; set; }
        [ExcelExportPropertyAttribute]
        public int PEG { get; set; }
        [ExcelExportPropertyAttribute]
        public int Commercial { get; set; }
        [ExcelExportPropertyAttribute]
        public int Unassigned { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal HFCDollars { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal NodeSplitDollars { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal RFoGDollars { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal PON_Dollars { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal FiberDeepDollars { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal DataTransportDollars { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal OtherDollars { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal PEG_Dollars { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal CommercialDollars { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal UnassignedDollars { get; set; }
    }
}
