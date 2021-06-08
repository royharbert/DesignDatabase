using DesignDB_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Operations
{
    public static class RequestModelReportToRequestModel
    {
        public static RequestModel ConvertReportModelToRequestModel(RequestModelReport rp)
        {
            RequestModel r = new RequestModel();
            r.ProjectID = rp.ProjectID;
            r.MSO = rp.MSO;
            r.Cust = rp.Cust;
            r.ArchitectureDetails = rp.ArchitectureDetails;
            r.ArchitectureType = rp.ArchitectureType;
            r.AssistedBy = rp.AssistedBy;
            r.AwardStatus = rp.AwardStatus;
            r.BOM_Value = rp.BOM_Value;
            r.Category = rp.Category;
            r.City = rp.City;
            r.Comments = rp.Comments;
            r.Country = rp.Country;
            r.Cust = rp.Cust;
            r.DateCompleted = rp.DateCompleted;
            r.DateAllInfoReceived = rp.DateAllInfoReceived;
            r.DateDue = rp.DateDue;
            r.DateAssigned = rp.DateAssigned;
            r.DateLastUpdate = rp.DateLastUpdate;
            r.Designer = rp.Designer;
            r.DesignRequestor = rp.DesignRequestor;
            r.ID = rp.ID;
            r.msoModel = rp.msoModel;
            r.OriginalQuote = rp.OriginalQuote;
            r.PercentageProjectCovered = rp.PercentageProjectCovered;
            r.ProjectName = rp.ProjectName;
            r.Pty = rp.Pty;
            r.QuoteType = rp.QuoteType;
            r.Region = rp.Region;
            r.ReviewedBy = rp.ReviewedBy;
            r.ST = rp.ST;
            r.TotalHours = rp.TotalHours;

            return r;
        }
    }
}
