using DesignDB_Library.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Models
{
    public class ReportDesignsInProcessModel
    {
        [ExcelExportPropertyAttribute]
        public string Salesperson { get; set; }
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
        
    }
}
