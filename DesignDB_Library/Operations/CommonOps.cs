using DesignDB_Library;
using DesignDB_Library.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignDB_Library.Operations
{
    public static class CommonOps
    {
        public static RequestModel CloneRequestList(RequestModel oldModel)
        {
            RequestModel newModel = new RequestModel();

            newModel.ArchitectureDetails = oldModel.ArchitectureDetails;
            newModel.ArchitectureType = oldModel.ArchitectureType;
            newModel.AssistedBy = oldModel.AssistedBy;
            newModel.AwardStatus = oldModel.AwardStatus;
            newModel.BOM_Value = oldModel.BOM_Value;
            newModel.Category = oldModel.Category;
            newModel.City = oldModel.City;
            newModel.Comments = oldModel.Comments;
            newModel.Country = oldModel.Country;
            newModel.Cust = oldModel.Cust;
            newModel.DateAllInfoReceived = oldModel.DateAllInfoReceived;
            newModel.DateAssigned = oldModel.DateAssigned;
            newModel.DateCompleted = oldModel.DateCompleted;
            newModel.DateDue = oldModel.DateDue;
            newModel.DateLastUpdate = oldModel.DateLastUpdate;
            newModel.Designer = oldModel.Designer;
            newModel.DesignRequestor = oldModel.DesignRequestor;
            newModel.ID = oldModel.ID;
            newModel.MSO = oldModel.MSO;
            newModel.msoModel = oldModel.msoModel;
            newModel.OriginalQuote = oldModel.OriginalQuote;
            newModel.PercentageProjectCovered = oldModel.PercentageProjectCovered;
            newModel.ProjectID = oldModel.ProjectID;
            newModel.ProjectName = oldModel.ProjectName;
            newModel.Pty = oldModel.Pty;
            newModel.QuoteType = oldModel.QuoteType;
            newModel.Region = oldModel.Region;
            newModel.ReviewedBy = oldModel.ReviewedBy;
            newModel.ST = oldModel.ST;
            newModel.TotalHours = oldModel.TotalHours;            

            return newModel;
        }

        public static DateTime CalculateDateDue(DateTime StartDate, string priority)
        {
            //get list of holidays
            List<CompanyHolidaysModel> holidayList = GlobalConfig.Connection.GetAllHolidays();
            //create list of holiday dates
            List<DateTime> holidayDates = new List<DateTime>();
            foreach (var holiday in holidayList)
            {
                holidayDates.Add(holiday.HolidayDate.Date);
            }
            //use Pty to determine work days
            int workDays = 0;
            switch (priority)
            {
                case "P1":
                    workDays = (int)prty.P1;
                    break;

                case "P2":
                    workDays = (int)prty.P2;
                    break;

                case "P3":
                    workDays = (int)prty.P3;
                    break;

                default:
                    break;
            }
            //loop thru for number of days
            DateTime currentDay = StartDate;
            int wkDay = 1;
            while (wkDay <= workDays)
            {
                //check day of week
                if (currentDay.DayOfWeek != DayOfWeek.Saturday && currentDay.DayOfWeek != DayOfWeek.Sunday)
                {
                    //check to see if current date is holiday
                    if (!holidayDates.Contains(currentDay.Date))
                    {
                        //if not holiday increment counter
                        wkDay ++;
                    }
                }
                currentDay = currentDay.AddDays(1);

            }

            //make sure not returning holiday or weekend
            while (currentDay.DayOfWeek == DayOfWeek.Saturday || currentDay.DayOfWeek ==DayOfWeek.Sunday
                || holidayDates.Contains(currentDay.Date))
            {
                currentDay = currentDay.AddDays(1);
            }
            //at end of loop return date
            return currentDay.Date;
        }
    }
}
