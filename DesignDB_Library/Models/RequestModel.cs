using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignDB_Library.Attributes;

namespace DesignDB_Library.Models
{
    /// <summary>
    /// Model of design request
    /// </summary>
    public class RequestModel        
    {
        public int ID { get; set; }
        [ExcelExportPropertyAttribute]
        public string ProjectID { get; set; }
        [ExcelExportPropertyAttribute]
        public string MSO { get; set; }
        [ExcelExportPropertyAttribute]
        public string Cust { get; set; }
        [ExcelExportPropertyAttribute]
        public string City { get; set; }
        [ExcelExportPropertyAttribute]
        public string ST { get; set; }
        [ExcelExportPropertyAttribute]
        public string Country { get; set; }
        [ExcelExportPropertyAttribute]
        public string Region { get; set; }
        [ExcelExportPropertyAttribute]
        public string DesignRequestor { get; set; }
        [ExcelExportPropertyAttribute]
        public string QuoteType { get; set; }
        [ExcelExportPropertyAttribute]
        public string Pty { get; set; }
        [ExcelExportPropertyAttribute]
        public string Designer { get; set; }
        [ExcelExportPropertyAttribute]
        public string ProjectName { get; set; }
        [ExcelExportPropertyAttribute]
        public string OriginalQuote { get; set; }
        [ExcelExportPropertyAttribute]
        public string AssistedBy { get; set; }
        [ExcelExportPropertyAttribute]
        public string Category { get; set; }
        [ExcelExportPropertyAttribute]
        public string ArchitectureType { get; set; }
        [ExcelExportPropertyAttribute]
        public DateTime DateAssigned { get; set; }
        [ExcelExportPropertyAttribute]
        public DateTime DateAllInfoReceived  { get; set; }
        [ExcelExportPropertyAttribute]
        public DateTime DateDue { get; set; }
        [ExcelExportPropertyAttribute]
        public string AwardStatus { get; set; }
        [ExcelExportPropertyAttribute]
        public DateTime DateLastUpdate { get; set; }
        [ExcelExportPropertyAttribute]
        public string ReviewedBy { get; set; }
        [ExcelExportPropertyAttribute]
        public DateTime DateCompleted { get; set; }
        [ExcelExportPropertyAttribute]
        public int TotalHours { get; set; }
        [ExcelExportPropertyAttribute]
        public decimal BOM_Value { get; set; }
        [ExcelExportPropertyAttribute]
        public int PercentageProjectCovered { get; set; }
        [ExcelExportPropertyAttribute]
        public string ArchitectureDetails { get; set; }
        //[ExcelExportPropertyAttribute]
        public string Comments { get; set; }
        public MSO_Model msoModel { get; set; }

        public RequestModelReport RequestModelToRequestModelReport()
        {
            RequestModelReport rptModel = new RequestModelReport();
            rptModel.ID = this.ID;
            rptModel.ProjectID = this.ProjectID;
            rptModel.QuoteType = this.QuoteType;
            rptModel.OriginalQuote = this.OriginalQuote;
            rptModel.Pty = this.Pty;
            rptModel.AwardStatus = this.AwardStatus;
            rptModel.DesignRequestor = this.DesignRequestor;
            rptModel.BOM_Value = this.BOM_Value;
            rptModel.PercentageProjectCovered = this.PercentageProjectCovered;
            rptModel.MSO = this.MSO;
            rptModel.Region = this.Region;
            rptModel.City = this.City;
            rptModel.DateAssigned = this.DateAssigned;
            rptModel.DateAllInfoReceived = this.DateAllInfoReceived;
            rptModel.DateDue = this.DateDue;
            rptModel.DateCompleted = this.DateCompleted;
            rptModel.DateLastUpdate = this.DateLastUpdate;
            rptModel.Designer = this.Designer;
            rptModel.AssistedBy = this.AssistedBy;
            rptModel.ReviewedBy = this.ReviewedBy;
            rptModel.Category = this.Category;
            rptModel.ArchitectureDetails = this.ArchitectureDetails;
            rptModel.Comments = this.Comments;
            rptModel.TotalHours = this.TotalHours;
            rptModel.ArchitectureType = this.ArchitectureType;
            rptModel.Cust = this.Cust;
            rptModel.ST = this.ST;
            rptModel.Country = this.Country;
            rptModel.ProjectName = this.ProjectName;
            rptModel.msoModel = this.msoModel;

            return rptModel;
        }

        public RequestModel()
        {
            //initialize all date fields to valid date string that will not be
            //displayed in frmRequest
            this.DateCompleted = new DateTime(1900, 1, 1);
            this.DateAllInfoReceived = new DateTime(1900, 1, 1);
            this.DateDue = new DateTime(1900, 1, 1);
            this.DateLastUpdate = new DateTime(1900, 1, 1);
        }

        public RequestModel(string PID, string mso, string EndCust, string city, string state, 
            string country, string region, string requestor, string quotetype, string priority, string designer, string projName,
            string originalQuote, string assistedBy, string category, string architectureType, DateTime dateAssigned, 
            DateTime dateAllInfo, DateTime dateDue, string awardStatus, DateTime lastUpdata, string reviewer,DateTime dateCompleted,
            int totalHours, decimal bomValue, int percentCovered, string archDetails, string comments, int custLocID, MSO_Model modelMSO)
        {
            ProjectID = PID;
            MSO = mso;                        
            Cust = EndCust;
            City = city;
            ST = state;
            Country = country;
            Region = region;
            DesignRequestor = requestor;
            QuoteType = quotetype;
            Pty = priority;
            Designer = designer;
            ProjectName = projName;
            OriginalQuote = originalQuote;
            AssistedBy = assistedBy;
            Category = category;
            ArchitectureType = architectureType;
            DateAssigned = dateAssigned;
            DateAllInfoReceived = dateAllInfo;
            DateDue = dateDue;
            AwardStatus = awardStatus;
            DateLastUpdate = DateLastUpdate;
            ReviewedBy = reviewer;
            DateCompleted = dateCompleted;
            TotalHours = totalHours;
            BOM_Value = bomValue;
            PercentageProjectCovered = percentCovered;
            ArchitectureDetails = archDetails;
            Comments = comments;
            
            msoModel = modelMSO;
        }
    }
}
