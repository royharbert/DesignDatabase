using DesignDB_Library.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    public class Report_SalesProjectValuesModel
    {
        [ExcelExportPropertyAttribute]
        public string SalesPerson { get; set; }
        //[ExcelExportPropertyAttribute]
        //public string MSO { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal CurrentYTD_Value { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal AverageDollars {  get; set; }
        [ExcelExportPropertyAttribute]
        public int CurrentYear_Count { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal PctTotalValue { get; set; }
        [ExcelExportPropertyAttribute]
        public int JanProjects { get; set; }
        [ExcelExportPropertyAttribute]
        public int FebProjects { get; set; }
        [ExcelExportPropertyAttribute]
        public int MarProjects { get; set; }
        [ExcelExportPropertyAttribute]
        public int AprProjects { get; set; }
        [ExcelExportPropertyAttribute]
        public int MayProjects { get; set; }
        [ExcelExportPropertyAttribute]
        public int JunProjects { get; set; }
        [ExcelExportPropertyAttribute]
        public int JulProjects { get; set; }
        [ExcelExportPropertyAttribute]
        public int AugProjects { get; set; }
        [ExcelExportPropertyAttribute]
        public int SepProjects { get; set; }
        [ExcelExportPropertyAttribute]
        public int OctProjects { get; set; }
        [ExcelExportPropertyAttribute]
        public int NovProjects { get; set; }
        [ExcelExportPropertyAttribute]
        public int DecProjects { get; set; }
        [ExcelExportPropertyAttribute]
        public int Weekly { get; set; }
        [ExcelExportPropertyAttribute]
        public int Total { get; set; }

        public void Clear()
        {
            this.SalesPerson = "";
            //this.MSO = "";
            this.CurrentYTD_Value = 0;
            this.AverageDollars = 0;
            this.CurrentYear_Count = 0;
            this.PctTotalValue = 0;
            this.JanProjects = 0;
            this.FebProjects = 0;
            this.MarProjects = 0;
            this.AprProjects = 0;
            this.MayProjects = 0;
            this.JunProjects = 0;
            this.JulProjects = 0;
            this.AugProjects = 0;
            this.SepProjects = 0;
            this.OctProjects = 0;
            this.NovProjects = 0;
            this.DecProjects = 0;
        }
    }
}
