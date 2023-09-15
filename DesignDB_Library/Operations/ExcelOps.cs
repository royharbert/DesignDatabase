using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Excel;
using System.Runtime.InteropServices;
using DesignDB_Library.Models;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices.ComTypes;
using static System.Collections.Specialized.BitVector32;

namespace DesignDB_Library.Operations
{
    public class ExcelOps
    {
        private static int PlaceMonthlyMSO_SummaryInExcelNoPct(Excel.Worksheet wks, DateTime startDate, DateTime endDate, 
            List<Report_SalesProjectValuesModel> msoSummary, int[,] sectionArray, int row, int section)
        {
            row = row + 2;
            sectionArray[section, 0] = row - 1;
            string weeklyHeader = "Current Week " + " " + startDate.ToShortDateString() + " " + endDate.ToShortDateString();
            string[] columnHeaders = new string[] {"MSO","Total $","Average $","Total Count","Jan","Feb","Mar","Apr","May",
                "Jun","Jul","Aug","Sep","Oct","Nov","Dec",weeklyHeader };
            row = makeTitle(wks, row, 17, "Requests by MSO/Month", columnHeaders);

            foreach (var model in msoSummary)
            {
                wks.Cells[row, 1] = model.SalesPerson;
                wks.Cells[row, 2] = model.CurrentYTD_Value;
                wks.Cells[row, 3] = model.AverageDollars;
                wks.Cells[row, 4] = model.CurrentYear_Count;
                wks.Cells[row, 5] = model.JanProjects;
                wks.Cells[row, 6] = model.FebProjects;
                wks.Cells[row, 7] = model.MarProjects;
                wks.Cells[row, 8] = model.AprProjects;
                wks.Cells[row, 9] = model.MayProjects;
                wks.Cells[row, 10] = model.JunProjects;
                wks.Cells[row, 11] = model.JulProjects;
                wks.Cells[row, 12] = model.AugProjects;
                wks.Cells[row, 13] = model.SepProjects;
                wks.Cells[row, 14] = model.OctProjects;
                wks.Cells[row, 15] = model.NovProjects;
                wks.Cells[row, 16] = model.DecProjects;
                wks.Cells[row, 17] = model.Weekly;
                row++;
            }
            sectionArray[section, 1] = row - 1;

            setDollarDecimalPlaces(wks, 0, sectionArray[section, 0] + 3, sectionArray[section, 1], 2, 3);
            Range summaryRange = wks.Range[wks.Cells[row - 1, 1], wks.Cells[row - 1, 17]];
            summaryRange.Font.Bold = true;

            return row;
        }
        private static int PlaceMonthlyMSO_SummaryInExcel(Excel.Worksheet wks, DateTime startDate, DateTime endDate, List<Report_SalesProjectValuesModel> msoSummary,
            int[,] sectionArray, int row)
        {           
            row = row + 2;
            sectionArray[1, 0] = row - 1;
            string weeklyHeader = "Current Week " + startDate + " " + endDate;
            string[] columnHeaders = new string[] {"MSO","Total $","Average $","Total Count","% of Total Value","Jan","Feb","Mar","Apr","May",
                "Jun","Jul","Aug","Sep","Oct","Nov","Dec",weeklyHeader };
            row = makeTitle(wks, row, 18, "Requests by MSO/Month", columnHeaders);

            foreach (var model in msoSummary)
            {
                wks.Cells[row, 1] = model.SalesPerson;
                wks.Cells[row, 2] = model.CurrentYTD_Value;
                wks.Cells[row, 3] = model.AverageDollars;
                wks.Cells[row, 4] = model.CurrentYear_Count;
                wks.Cells[row, 5] = model.PctTotalValue;
                wks.Cells[row, 6] = model.JanProjects;
                wks.Cells[row, 7] = model.FebProjects;
                wks.Cells[row, 8] = model.MarProjects;
                wks.Cells[row, 9] = model.AprProjects;
                wks.Cells[row, 10] = model.MayProjects;
                wks.Cells[row, 11] = model.JunProjects;
                wks.Cells[row, 12] = model.JulProjects;
                wks.Cells[row, 13] = model.AugProjects;
                wks.Cells[row, 14] = model.SepProjects;
                wks.Cells[row, 15] = model.OctProjects;
                wks.Cells[row, 16] = model.NovProjects;
                wks.Cells[row, 17] = model.DecProjects;
                wks.Cells[row, 18] = model.Weekly;
                row++;
            }
            sectionArray[1, 1] = row - 1;

            setDollarDecimalPlaces(wks, 0, sectionArray[1, 0] + 3, sectionArray[1, 1], 2, 3);
            setPercentDecimalPlaces(wks, 0, sectionArray[1, 0] + 3, sectionArray[1, 1], 5, 5);
            Range summaryRange = wks.Range[wks.Cells[row - 1, 1], wks.Cells[row - 1, 18]];
            summaryRange.Font.Bold = true;

            return row;
        }
        private static int PlaceRequestsBySalespersonInExcel(Excel.Worksheet wks, DateTime startDate, DateTime endDate, 
            List<Report_SalesProjectValuesModel> requests, int[,] sectionArray)
        {
            int row = 1;
            sectionArray[0, 0] = 1;

            //Place column headings
            string weeklyHeader = "Current Week" + startDate.ToShortDateString() + " " + endDate.ToShortDateString();
            string[] columnHeaders = {"Salesperson","MSO","Total $","Average $","Total Count","% of Total Value","Jan","Feb","Mar","Apr","May",
                "Jun","Jul","Aug","Sep","Oct","Nov","Dec",weeklyHeader};
            row = makeTitle(wks, row, 19, "Design Requests by Salesperson/Month", columnHeaders);

            wks.Columns[1].ColumnWidth = 28;
            wks.get_Range("B:C").ColumnWidth = 25;
            wks.get_Range("D:Z").ColumnWidth = 15;


            row = 3;
            foreach (var model in requests)
            {
                wks.Cells[row, 1] = model.SalesPerson;
                //wks.Cells[row, 2] = model.MSO;
                wks.Cells[row, 3] = model.CurrentYTD_Value;
                wks.Cells[row, 4] = model.AverageDollars;
                wks.Cells[row, 5] = model.CurrentYear_Count;
                wks.Cells[row, 6] = model.PctTotalValue;
                wks.Cells[row, 7] = model.JanProjects;
                wks.Cells[row, 8] = model.FebProjects;
                wks.Cells[row, 9] = model.MarProjects;
                wks.Cells[row, 10] = model.AprProjects;
                wks.Cells[row, 11] = model.MayProjects;
                wks.Cells[row, 12] = model.JunProjects;
                wks.Cells[row, 13] = model.JulProjects;
                wks.Cells[row, 14] = model.AugProjects;
                wks.Cells[row, 15] = model.SepProjects;
                wks.Cells[row, 16] = model.OctProjects;
                wks.Cells[row, 17] = model.NovProjects;
                wks.Cells[row, 18] = model.DecProjects;
                wks.Cells[row, 19] = model.Weekly;
                row++;
            }
            sectionArray[0, 1] = row - 1;
            int categoryStartRow = row;
            setDollarDecimalPlaces(wks, 0, sectionArray[0, 0] + 2, sectionArray[0, 1], 3, 4);
            setPercentDecimalPlaces(wks, 0, sectionArray[0, 0] + 2, sectionArray[0, 1], 6, 6);

            Excel.Range summaryRange = wks.Range[wks.Cells[row - 1, 1], wks.Cells[row - 1, 19]];
            summaryRange.Font.Bold = true;

            return row;
        }

        private static int PlaceRequestsBySalespersonInExcelNoPct(Excel.Worksheet wks, DateTime startDate, DateTime endDate,
            List<Report_SalesProjectValuesModel> requests, int[,] sectionArray, int row, int section)
        {
            row = row + 2;
            sectionArray[section, 0] = row;

            //Place column headings
            string weeklyHeader = "Current Week" + " " + startDate.ToShortDateString() + " " + endDate.ToShortDateString();
            string[] columnHeaders = {"Salesperson",/*"MSO",*/"Total $","Average $","Total Count", "Jan","Feb","Mar","Apr","May",
                "Jun","Jul","Aug","Sep","Oct","Nov","Dec",weeklyHeader};
            row = makeTitle(wks, row, 17, "Design Requests by Salesperson/Month", columnHeaders);

            foreach (var model in requests)
            {
                wks.Cells[row, 1] = model.SalesPerson;
                wks.Cells[row, 2] = model.CurrentYTD_Value;
                wks.Cells[row, 3] = model.AverageDollars;
                wks.Cells[row, 4] = model.CurrentYear_Count;
                wks.Cells[row, 5] = model.JanProjects;
                wks.Cells[row, 6] = model.FebProjects;
                wks.Cells[row, 7] = model.MarProjects;
                wks.Cells[row, 8] = model.AprProjects;
                wks.Cells[row, 9] = model.MayProjects;
                wks.Cells[row, 10] = model.JunProjects;
                wks.Cells[row, 11] = model.JulProjects;
                wks.Cells[row, 12] = model.AugProjects;
                wks.Cells[row, 13] = model.SepProjects;
                wks.Cells[row, 14] = model.OctProjects;
                wks.Cells[row, 15] = model.NovProjects;
                wks.Cells[row, 16] = model.DecProjects;
                wks.Cells[row, 17] = model.Weekly;
                row++;
            }
            sectionArray[section, 1] = row - 1;
            int categoryStartRow = row;
            setDollarDecimalPlaces(wks, 0, sectionArray[section, 0] + 2, sectionArray[section, 1], 2, 3);

            Excel.Range summaryRange = wks.Range[wks.Cells[row - 1, 1], wks.Cells[row - 1, 17]];
            summaryRange.Font.Bold = true;

            return row;
        }




        //***********************************************************************************************************************************************************************
        public static void PlaceRollupInExcel(DateTime startDate, DateTime endDate, List<OpenRequestsBySalesModel> openBySales,
            List<ReportCategoryMSOModel> categories, List<Report_SalesProjectValuesModel> requests,
            List<ReportSalesPriorityModel> priorityList, decimal bomTotal, List<Report_SalesProjectValuesModel> msoSummary, List<MSO_Model> msoModels,
            List<AwardStatusModel> awards, List<RollupCompletionTimeModel> completionReport, bool customFormat = false)
        {
            Excel.Application xlApp = makeExcelApp();
            xlApp.Workbooks.Add();
            Excel.Worksheet wks = xlApp.ActiveSheet;
            xlApp.ActiveWindow.Zoom = 80;
            xlApp.Visible = true;

            wks.Columns[1].ColumnWidth = 28;
            wks.get_Range("B:C").ColumnWidth = 25;
            wks.get_Range("D:Z").ColumnWidth = 15;

            int[,] sectionArray = new int[8, 2];
            int row = -1;
            int section;

            //Secton 1
            //*
            section = 0;
            row = PlaceMonthlyMSO_SummaryInExcelNoPct(wks, startDate, endDate, msoSummary, sectionArray, row, section);
            /*/
             * section = 0;
            row = PlaceMonthlyMSO_SummaryInExcel(wks, startDate, endDate, msoSummary, sectionArray, row, section);
            //*/

            //Section 2
            section = 1;
            row = PlaceRequestsByMSO_CategoryInExcel(wks, categories, sectionArray, row, section);

            //Section 3
            section = 2;
            row = PlaceCompletionTimeReportInExcel(wks, completionReport, sectionArray, row, section);

            //Section 4
            //*
            section = 3;
            if (!customFormat)
            {
                row = PlaceAwardStatusInExcel(wks, awards, msoModels, sectionArray, row, section);
            }

            //Section 5
            //*
            section = 4;
            row = PlaceOpenRequestsInExcelNoMSO(wks, openBySales, sectionArray, row, section);
            /*/
             * section = 4;
            row = PlaceOpenRequestsInExcel(wks, openBySales, sectionArray, row, section);
            //*/

            //Section 6
            //*
            section = 5;
            row = PlaceRequestsBySalesPersonPriorityNoMSO(wks, priorityList,sectionArray, row, section);
            /*/
             * section = 5;
            row = PlaceRequestsBySalesPersonPriority(wks, priorityList, sectionArray, row, section);
            //*/

            //Section 7
            //*
            section = 6;
            row = PlaceRequestsBySalespersonInExcelNoPct(wks, startDate, endDate, requests, sectionArray, row, section);
            /*/
             * section = 6;
            int row = PlaceRequestsBySalespersonInExcel(wks, startDate, endDate, requests, sectionArray);
            //*/



            releaseObject(xlApp);
        }

//***********************************************************************************************************************************************************************

        private static int PlaceRequestsBySalesPersonPriority(Excel.Worksheet wks, List<ReportSalesPriorityModel> list, int[,] sectionArray, int row, int section)
        {
            {
                row = row + 2; 
                
                string[] columnHeaaders = { "Sales Person", /*"MSO",*/ "Total", "P1", "P2", "P3" };
                row = makeTitle(wks, row, 6, "Design Requests by Salesperson/Priority", columnHeaaders);
                
                sectionArray[section, 0] = row - 1;

                foreach (var model in list)
                {
                    
                    wks.Cells[row, 1].Value = model.SalesPerson;
                    //wks.Cells[row, 2].Value = model.MSO;
                    wks.Cells[row, 3].Value = model.TotalCount;
                    wks.Cells[row, 4].Value = model.P1Pct;
                    wks.Cells[row, 5].Value = model.P2Pct;
                    wks.Cells[row, 6].Value = model.P3Pct;
                    row++;
                }
                sectionArray[section, 1] = row - 1;
                
                setPercentDecimalPlaces(wks, 0, sectionArray[5, 0] + 1, sectionArray[5, 1], 4, 6);
                Excel.Range boldRange = wks.Range[wks.Cells[row - 1, 1], wks.Cells[row, 5]];
                boldRange.Font.Bold = true;

                setDollarDecimalPlaces(wks, 0, sectionArray[section, 0] + 1, sectionArray[section, 1], 2, 3);

                return row;
            }
        }

        private static int PlaceRequestsBySalesPersonPriorityNoMSO(Excel.Worksheet wks, List<ReportSalesPriorityModel> list, int[,] sectionArray, 
            int row, int section)
        {
            {
                row = row + 2;
                string[] columnHeaaders = { "Sales Person", "Total", "P1", "P2", "P3" };
                row = makeTitle(wks, row, 5, "Design Requests by Salesperson/Priority", columnHeaaders);
                sectionArray[section, 0] = row - 1;

                foreach (var model in list)
                {
                    wks.Cells[row, 1].Value = model.SalesPerson;
                    wks.Cells[row, 2].Value = model.TotalCount;
                    wks.Cells[row, 3].Value = model.P1Pct;
                    wks.Cells[row, 4].Value = model.P2Pct;
                    wks.Cells[row, 5].Value = model.P3Pct;
                    row++;
                }
                sectionArray[section, 1] = row - 1;

                setPercentDecimalPlaces(wks, 0, sectionArray[section, 0] + 1, sectionArray[section, 1], 3, 5);
                Excel.Range boldRange = wks.Range[wks.Cells[row - 1, 1], wks.Cells[row, 5]];
                boldRange.Font.Bold = true;

                return row;
            }
        }


        private static int PlaceOpenRequestsInExcel(Excel.Worksheet wks, List<OpenRequestsBySalesModel> openBySales, int[,] sectionArray, int row)
        {
            row = row + 2;
            sectionArray[4, 0] = row - 1;
            string[] columnHeaders = { "Sales Person", "MSO", "Total", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            row = makeTitle(wks, row, 15, "Open Design Requests by Salesperson/Month", columnHeaders);
            foreach (var model in openBySales)
            {
                wks.Cells[row, 1] = model.Salesperson;
                wks.Cells[row, 2] = model.MSO;
                wks.Cells[row, 3] = model.Count;
                wks.Cells[row, 4] = model.Jan;
                wks.Cells[row, 5] = model.Feb;
                wks.Cells[row, 6] = model.Mar;
                wks.Cells[row, 7] = model.Apr;
                wks.Cells[row, 8] = model.May;
                wks.Cells[row, 9] = model.Jun;
                wks.Cells[row, 10] = model.Jul;
                wks.Cells[row, 11] = model.Aug;
                wks.Cells[row, 12] = model.Sep;
                wks.Cells[row, 13] = model.Oct;
                wks.Cells[row, 14] = model.Nov;
                wks.Cells[row, 15] = model.Dec;
                row++;
            }
            sectionArray[4, 1] = row - 1;
            Range summaryRange = wks.Range[wks.Cells[row - 1, 1], wks.Cells[row - 1, 15]];
            summaryRange.Font.Bold = true;

            return row;
        }

        private static int PlaceOpenRequestsInExcelNoMSO(Excel.Worksheet wks, List<OpenRequestsBySalesModel> openBySales, int[,] sectionArray, 
            int row, int section)
        {
            row = row + 2;
            sectionArray[section, 0] = row - 1;
            string[] columnHeaders = { "Sales Person", "Total", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            row = makeTitle(wks, row, 14, "Open Design Requests by Salesperson/Month", columnHeaders);
            foreach (var model in openBySales)
            {
                wks.Cells[row, 1] = model.Salesperson;
                wks.Cells[row, 2] = model.Count;
                wks.Cells[row, 3] = model.Jan;
                wks.Cells[row, 4] = model.Feb;
                wks.Cells[row, 5] = model.Mar;
                wks.Cells[row, 6] = model.Apr;
                wks.Cells[row, 7] = model.May;
                wks.Cells[row, 8] = model.Jun;
                wks.Cells[row, 9] = model.Jul;
                wks.Cells[row, 10] = model.Aug;
                wks.Cells[row, 11] = model.Sep;
                wks.Cells[row, 12] = model.Oct;
                wks.Cells[row, 13] = model.Nov;
                wks.Cells[row, 14] = model.Dec;
                row++;
            }
            sectionArray[section, 1] = row - 1;
            Range summaryRange = wks.Range[wks.Cells[row - 1, 1], wks.Cells[row - 1, 14]];
            summaryRange.Font.Bold = true;

            return row;
        }

        private static int PlaceRequestsByMSO_CategoryInExcel(Excel.Worksheet wks, List<ReportCategoryMSOModel> categories, int[,] sectionArray, 
            int row, int section)
        {
            row = row + 2;
            int categoryStartRow = row;
            sectionArray[section, 0] = row - 1;
            string[] columnHeaders = {"MSO","Total $","Average $","Total Count","% of Total Value","HFC","Node Split","RFoG","PON","RFoG-PON",
                "Fiber Deep","Data Transport","Other","Unassigned","HFC Dollars","Node Split Dollars","RFoG Dollars","PON Dollars",
                "RFoG-PON Dollars","Fiber Deep Dollars","Data Transport Dollars","Other Dollars","Unassigned Dollars"};
            row = makeTitle(wks, row, 23, "Design Requests by MSO/Category", columnHeaders);
            foreach (var model in categories)
            {
                wks.Cells[row, 1] = model.MSO;
                wks.Cells[row, 2] = model.TotalDollars;
                wks.Cells[row, 3] = model.AverageDollarsPerRequest;
                wks.Cells[row, 4] = model.TotalRequests;
                wks.Cells[row, 5] = model.PctOfTotal;
                wks.Cells[row, 6] = model.HFC;
                wks.Cells[row, 7] = model.NodeSplit;
                wks.Cells[row, 8] = model.RFoG;
                wks.Cells[row, 9] = model.PON;
                wks.Cells[row, 10] = model.RFoGPON;
                wks.Cells[row, 11] = model.FiberDeep;
                wks.Cells[row, 12] = model.DataTrans;
                wks.Cells[row, 13] = model.Other;
                wks.Cells[row, 14] = model.Unassigned;
                wks.Cells[row, 15] = model.HFCDollars;
                wks.Cells[row, 16] = model.NodeSplitDollars;
                wks.Cells[row, 17] = model.RFoGDollars;
                wks.Cells[row, 18] = model.PON_Dollars;
                wks.Cells[row, 19] = model.RFoGPON_Dollars;
                wks.Cells[row, 20] = model.FiberDeepDollars;
                wks.Cells[row, 21] = model.DataTransportDollars;
                wks.Cells[row, 22] = model.OtherDollars;
                wks.Cells[row, 23] = model.UnassignedDollars;
                row++;
            }
            sectionArray[section, 1] = row - 1;


            Excel.Range numRange = wks.Range[wks.Cells[categoryStartRow, 1], wks.Cells[row, 23]];
            Excel.Range summaryRange = wks.Range[wks.Cells[row - 1, 1], wks.Cells[row - 1, 23]];
            summaryRange.Font.Bold = true;

            setDollarDecimalPlaces(wks, 0, sectionArray[section, 0] + 3, sectionArray[section, 1], 2, 3);
            setDollarDecimalPlaces(wks, 0, sectionArray[section, 0] + 3, sectionArray[section, 1], 15, 23);

            setPercentDecimalPlaces(wks, 0, sectionArray[section, 0] + 3, sectionArray[section, 1], 5, 5);

            return row;
        }

        private static int PlaceAwardStatusInExcel(Excel.Worksheet wks, List<AwardStatusModel> awards, List<MSO_Model> msoList, int[,] sectionArray, 
            int row, int section)
        {
            row = row + 2;
            sectionArray[section,0] = row;

            string[] columnHeaders = new string[] {"MSO", "Pending Count","Pending $","Has Revision Count","Has Revision $","Canceled Count",
                "Canceled $","Inactive Count", "Inactive $","Yes Count","Yes $","No Count","No $"};
            row = makeTitle(wks, row, 13, "Award Status", columnHeaders);

            foreach(var award in awards)
            { 
                List<AwardStatusModel> msoAwards = new List<AwardStatusModel>();
                wks.Cells[row, 1].Value = award.MSO;
                wks.Cells[row, 2].Value = award.PendingCount;
                wks.Cells[row, 3].Value = award.PendingDollars;
                wks.Cells[row, 4].Value = award.HasRevisionCount;
                wks.Cells[row, 5].Value = award.HasRevisionDollars;
                wks.Cells[row, 6].Value = award.CanceledCount;
                wks.Cells[row, 7].Value = award.CanceledDollars;
                wks.Cells[row, 8].Value = award.InactiveCount;
                wks.Cells[row, 9].Value = award.InactiveDollars;
                wks.Cells[row, 10].Value = award.YesCount;
                wks.Cells[row, 11].Value = award.YesDollars;
                wks.Cells[row, 12].Value = award.NoCount;
                wks.Cells[row, 13].Value = award.NoDollars;

                row++;
                msoAwards.Add(award);    
            }
            Range summaryRange = wks.Range[wks.Cells[row - 1, 1], wks.Cells[row - 1, 13]];
            summaryRange.Font.Bold = true;
            sectionArray[section, 1] = row - 1;
            setDollarDecimalPlaces(wks, 0, sectionArray[section, 0] + 2, sectionArray[section, 1], 3, 3);
            setDollarDecimalPlaces(wks, 0, sectionArray[section, 0] + 2, sectionArray[section, 1], 5, 3);
            setDollarDecimalPlaces(wks, 0, sectionArray[section, 0] + 2, sectionArray[section, 1], 7, 7);
            setDollarDecimalPlaces(wks, 0, sectionArray[section, 0] + 2, sectionArray[section, 1], 9, 9);
            setDollarDecimalPlaces(wks, 0, sectionArray[section, 0] + 2, sectionArray[section, 1], 11, 11);
            setDollarDecimalPlaces(wks, 0, sectionArray[section, 0] + 2, sectionArray[section, 1], 13, 13);
                                                           
            return row;
        }

        private static int PlaceCompletionTimeReportInExcel(Excel.Worksheet wks, List<RollupCompletionTimeModel> report, int[,] sectionArray, int row, 
            int section)
        {
            row = row + 2;
            string[] columnHeaaders = { "MSO", "Completed Designs", "Average Days to Complete", "Average Days From All Info Received" };
            row = makeTitle(wks, row, 4, "Average Completion Time YTD", columnHeaaders);
            sectionArray[section, 0] = row - 1;

            foreach (RollupCompletionTimeModel mso in report)
            {
                wks.Cells[row, 1].Value = mso.MSO;
                wks.Cells[row, 2].Value = mso.CompletedDesigns;
                wks.Cells[row, 3].Value = mso.AvgDaysToComplete;
                wks.Cells[row, 4].Value = mso.AvgDaysFromAllInfo;
                row++;
            }
            sectionArray[section, 1] = row - 1;

            setDecimalPlaces(wks, 0, sectionArray[section, 0] + 1, sectionArray[section, 1], 2, 4);
            return row;
        }

        private static void setDollarDecimalPlaces(Excel.Worksheet wks, int decimals, int startRow, int stopRow, int startCol, int stopCol)
        {
            int[] bounds = { startRow, stopRow, startCol, stopCol };
            decimal val;
            for (int i = startRow; i <= stopRow; i++)
            {
                for (int j = startCol; j <= stopCol; j++)
                {
                    val = (decimal)wks.Cells[i, j].Value;
                    val = val * 100;
                    val = Math.Round(val, decimals);
                    val = val / 100;
                    wks.Cells[i, j].Value = val;
                }

                string formatString = "$#,###,###,##0";
                string decimalString = "";
                if (decimals > 0)
                {
                    decimalString = ".0" + new string('#', decimals) + decimalString;
                }
                Excel.Range range = wks.Range[wks.Cells[bounds[0], bounds[2]], wks.Cells[bounds[1], bounds[3]]];
                formatString = formatString + decimalString;
                range.NumberFormat = formatString;
            }
        }

        private static void setDecimalPlaces(Excel.Worksheet wks, int decimals, int startRow, int stopRow, int startCol, int stopCol)
        {
            int[] bounds = { startRow, stopRow, startCol, stopCol };
            decimal val;
            for (int i = startRow; i <= stopRow; i++)
            {
                for (int j = startCol; j <= stopCol; j++)
                {
                    val = (decimal)wks.Cells[i, j].Value;
                    val = val * 100;
                    val = Math.Round(val, decimals);
                    val = val / 100;
                    wks.Cells[i, j].Value = val;
                }

                string formatString = "#,###,###,##0";
                string decimalString = "";
                if (decimals > 0)
                {
                    decimalString = ".0" + new string('#', decimals) + decimalString;
                }
                Excel.Range range = wks.Range[wks.Cells[bounds[0], bounds[2]], wks.Cells[bounds[1], bounds[3]]];
                formatString = formatString + decimalString;
                range.NumberFormat = formatString;
            }
        }

        private static void setPercentDecimalPlaces(Excel.Worksheet wks, int decimals, int startRow, int stopRow, int startCol, int stopCol)
        {
            int[] bounds = { startRow, stopRow, startCol, stopCol };
            decimal val;
            for (int i = startRow; i <= stopRow; i++)
            {
                for (int j = startCol; j <= stopCol; j++)
                {
                    val = (decimal)wks.Cells[i, j].Value;
                    val = val * 100;
                    val = Math.Round(val, decimals);
                    val = val / 100;
                    wks.Cells[i, j].Value = val;
                }
            }
            //*
            Excel.Range range = wks.Range[wks.Cells[bounds[0], bounds[2]], wks.Cells[bounds[1], bounds[3]]];
            /*/
            Excel.Range range = wks.Range[wks.Cells[bounds[0], bounds[2]], wks.Cells[bounds[1], bounds[3]]];
            //*/
            string formatString = "##0";
            string decimalString = "%";
            if (decimals > 0)
            {
                decimalString = ".0" + new string('#', decimals) + decimalString;
            }

            formatString = formatString + decimalString;
            range.NumberFormat = formatString;
        }
        private static int makeTitle(Excel.Worksheet wks, int row, int rightmostCol, string title, string[] columnHeaders)
        {
            wks.Cells[row, 1].Value = title;
            wks.Cells[row, 1].Font.Size = 20;
            wks.Cells[row, 1].Font.Bold = true;
            var headerRow = wks.Cells[row + 1, 1];
            //var titleRow = wks.Range[wks.Cells[row, 1]], [wks.Cells[row, rightmostCol]];
            headerRow.RowHeight = 45;
            Range range = wks.Range[wks.Cells[row, 1], wks.Cells[row + 1, rightmostCol]];
            Range titleRow = wks.Range[wks.Cells[row, 1], wks.Cells[row, rightmostCol]];
            titleRow.Cells.Merge();
            range.Cells.HorizontalAlignment = HorizontalAlignment.Center;

            int col = 1;
            foreach (var column in columnHeaders)
            {
                wks.Cells[row + 1, col].Value = columnHeaders[col - 1];
                col = col + 1;
            }
            range.Font.Bold = true;
            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            range.Interior.Color = ColorTranslator.ToOle(System.Drawing.Color.LightSkyBlue);
            range.WrapText = true;
            row = row + 2;
            return row;
        }
        
        public static void PlaceDDListInExcel(List<List<(string Field, bool Active)>> ddList)
        {
            Excel.Application xlApp = makeExcelApp();
            xlApp.Workbooks.Add();
            Excel.Worksheet wks = xlApp.ActiveSheet;
            xlApp.Visible = true;

            int col = 0;
            for (int l = 0; l < ddList.Count; l++)
            {
                int row = 4;
                col = col + 1;
                List<(string Field, bool Active)> xList = ddList[l];
                for (int i = 0; i < xList.Count; i++)
                {
                    row = row + 1;
                    string entry = xList[i].Field.ToString();
                    wks.Cells[row, col].Value = entry;
                    if (! xList[i].Active)
                    {
                        wks.Cells[row,col].Font.Color = Color.Gray;
                    }
                }

            }

            //format sheet
            wks.Cells[5, 1].EntireRow.Font.Bold = true;
            wks.UsedRange.Columns.AutoFit();
            wks.Cells[5, 1].EntireRow.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

            releaseObject(xlApp);
        }
        public static Excel.Application OpenExcelWorkbook(Excel.Application xlApp, string WorkbookName)
        { 
            
            Excel.Workbook workbook = xlApp.Workbooks.Open(WorkbookName);
            return xlApp;
        }

        public static void releaseObject(object obj)
        {
            try
            {
                Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        public static Excel.Application makeExcelApp()
        {
            Excel.Application xlApp = new Excel.Application();

            return xlApp;
        }

        public static Excel.Application CreateForecastSheet()
        {
            Excel.Application xlApp = makeExcelApp();
            Excel.Workbook xlBook = xlApp.Workbooks.Add();

            Excel.Worksheet wks = xlBook.ActiveSheet;

            //Column width
            wks.Columns[1].ColumnWidth = 15;
            wks.Columns[1].HorizontalAlignment = 3;
            wks.get_Range("A:A").WrapText = true;
            wks.Columns[2].ColumnWidth = 25;
            wks.Columns[2].HorizontalAlignment = 3;
            wks.get_Range("B:B").WrapText = true;
            wks.Columns[3].ColumnWidth = 90;
            wks.Columns[3].HorizontalAlignment = 3;
            wks.get_Range("C:C").WrapText = true;
            wks.Columns[4].ColumnWidth = 26;
            wks.Columns[4].HorizontalAlignment = 3;
            wks.get_Range("D:D").WrapText = true;
            wks.Columns[7].ColumnWidth = 26;
            wks.Columns[7].HorizontalAlignment = 3;
            wks.get_Range("G:G").WrapText = true;


            //Header
            wks.Cells[1, 1] = "Design BOM Summary";        
            Excel.Range header = wks.get_Range("A1:D1");
            header.Merge(true);
            header.Font.Size = 20;
            header.Font.Bold = true;
            header.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightSkyBlue);

            //Details
            wks.Cells[2, 1] = "Start Date:";
            wks.Cells[3, 1] = "End Date:";
            wks.Cells[4, 1] = "MSO:";
            wks.get_Range("A2:A4").HorizontalAlignment = 4;

            //Data area
            wks.Cells[5, 1] = "Quantity";
            wks.Cells[5, 2] = "Model Number";
            wks.Cells[5, 3] = "Description";
            wks.Cells[5, 4] = "Quotes";
            wks.Cells[5, 7] = "Missing/Uncounted Quotes";
            wks.get_Range("A5:D5").Font.Bold = true;
            wks.get_Range("G5").Font.Bold = true;
            wks.get_Range("G5").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
            wks.get_Range("A5:D5").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

            return xlApp;
        }
    }
}
