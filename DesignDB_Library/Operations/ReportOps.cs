using DesignDB_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace DesignDB_Library.Operations
{
    public static class ReportOps
    {
        public static List<CompletionTimeModel> GenerateCompletionTimeSummary
            (DateTime startDate, DateTime endDate, List<MSO_Model> msoList)
        {
            List<CompletionTimeModel> report = new List<CompletionTimeModel>();
            //get completed requests from the time frame
            foreach (MSO_Model mso in msoList)
            {
                List<RequestModel> requestList =
                    GlobalConfig.Connection.DateRangeSearch_MSOFiltered(startDate, endDate, "DateAssigned", 
                    mso.MSO, false);
                if (requestList.Count > 0)
                {
                    CompletionTimeModel model = new CompletionTimeModel();
                    model.MSO = mso.MSO;
                    List<RequestModel> completedList = requestList.Where
                        (x => x.DateCompleted != DateTime.MinValue).ToList();
                    List<RequestModel> openList = requestList.Where
                        (x => x.DateCompleted == DateTime.MinValue).ToList();
                    int totalOpenDays = 0;
                    float averageOpenDays = 0;
                    float averageDaysToComplete = 0;
                    int invalidCompletedRequests = 0;

                    //Accumulate days open
                    int totalDaysToComplete = 0;

                    //sort completed designs               
                    foreach (RequestModel completedRequest in completedList)
                    {
                        //do completed designs
                        int designDays = (completedRequest.DateCompleted - completedRequest.DateAssigned).Days;
                        if (designDays < 0)
                        {
                            invalidCompletedRequests++;
                        }
                        else
                        {
                            totalDaysToComplete = totalDaysToComplete + designDays;
                        }
                    }
                    if (completedList.Count > 0)
                    {
                        averageDaysToComplete = totalDaysToComplete / completedList.Count;
                    }
                    model.CompletedDesigns = completedList.Count;
                    model.TotalDaysToComplete = totalDaysToComplete;
                    model.AvgDaysToComplete = (averageDaysToComplete - invalidCompletedRequests).ToString("0.00");

                    //sort open designs
                    foreach (RequestModel openRequest in openList)
                    {
                        int openDays = (DateTime.Now - openRequest.DateAssigned).Days;
                        totalOpenDays = totalOpenDays + openDays;
                    }
                    if (openList.Count > 0)
                    {
                        averageOpenDays = totalOpenDays / openList.Count;
                    }

                    model.CompletedDesigns = completedList.Count;
                    model.TotalDaysToComplete = totalDaysToComplete;
                    model.AvgDaysToComplete = averageDaysToComplete.ToString("0.00");

                    model.OpenDesigns = openList.Count;
                    model.OpenDays = totalOpenDays;
                    model.AverageOpenDays = averageOpenDays.ToString("#0.00");

                    model.OpenDesigns = openList.Count;
                    model.OpenDays = totalOpenDays;
                    model.AverageOpenDays = averageOpenDays.ToString("#0.00");
                    List<RequestModel> canceledDesigns = requestList.Where(x => x.AwardStatus == "Canceled").ToList();
                    model.CanceledDesigns = canceledDesigns.Count;

                    report.Add(model);
                    model = null;
                }

            }
            return report;
        }
        public static List<ReportSalesPriorityModel> GenerateSalesSummary(DateTime startDate, DateTime endDate)
        {
            List<SalespersonModel> sales = GlobalConfig.Connection.SalesGetActive();
            List<ReportSalesPriorityModel> report = new List<ReportSalesPriorityModel>();

            //get list of requests in timeframe
            List<RequestModel> requests = GlobalConfig.Connection.DateRangeSearch_Unfiltered(startDate, endDate, 
                "DateCompleted", false, "");

            //loop thru sales team
            List<RequestModel> requestsP1 = null;
            List<RequestModel> requestsP2 = null;
            List<RequestModel> requestsP3 = null;
            List<RequestModel> sRequests = null;
            foreach (SalespersonModel salesperson in sales)
            {
                ReportSalesPriorityModel model = new ReportSalesPriorityModel();
                model.SalesPerson = salesperson.SalesPerson;

                //use linq to extract data
                sRequests = requests.Where(x => x.DesignRequestor == salesperson.SalesPerson).ToList();
                model.TotalCount = sRequests.Count;
                requestsP1 = sRequests.Where(x => x.Pty == "P1").ToList();
                requestsP2 = sRequests.Where(x => x.Pty == "P2").ToList();
                requestsP3 = sRequests.Where(x => x.Pty == "P3").ToList();

                model.P1Count = requestsP1.Count;
                model.P2Count = requestsP2.Count;
                model.P3Count = requestsP3.Count;

                model.P1Dollars = AccumulateDollars(requestsP1);
                model.P2Dollars = AccumulateDollars(requestsP2);
                model.P3Dollars = AccumulateDollars(requestsP3);

                model.TotalDollars = model.P1Dollars + model.P2Dollars + model.P3Dollars;
                //add line to report
                report.Add(model);
                model = null;
                sRequests = null;
            }

            return report;
        }

        public static void formatPriorityDGV(DataGridView dgv)
        {
            string[] headers = { "Salesperson", "P1 Count", "P2 Count", "P3 Count", "P1 Dollars",
                "P2 Dollars", "P3 Dollars", "Total Count","Total Dollars" };
            setDGV_HeaderText(dgv, headers);

            int[] widths = { 120, 80, 80, 80, 80, 80, 80, 80 };
            SetDGV_ColumnWidths(dgv, widths);
        }

        private static decimal AccumulateDollars(List<RequestModel> requests)
        {
            decimal sum = 0;
            foreach (RequestModel request in requests)
            {
                sum = sum + request.BOM_Value;
            }
            return sum;
        }

        public static List<SnapshotModel> GenerateSnapshotReport(List<MSO_Model> msoList, DateTime startDate, DateTime endDate)
        {
            List<SnapshotModel> snapshots = new List<SnapshotModel>();
            foreach (MSO_Model mso in msoList)
            {
                SnapshotModel snapshotModel = ReportOps.SnapshotLine
                    (mso.MSO, startDate, endDate);
                if (snapshotModel != null)
                {
                    snapshots.Add(snapshotModel);
                }
            }
            return snapshots;
        }

        private static SnapshotModel SnapshotLine(string mso, DateTime startDate, DateTime endDate)
        {
            SnapshotModel snap = new SnapshotModel();
            int year = startDate.Year;
            int month = startDate.Month;
            DateTime yearStart = DateTime.Parse("1/1/" + year.ToString());
            DateTime yearEnd = DateTime.Parse("12/31/" + year.ToString());
            List<RequestModelReport> snapshot = GlobalConfig.Connection.GetSnapshotData(mso, yearStart, endDate);
            snap.MSO = mso;

            snap.RequestsThisYear = snapshot.Count;
            if (snap.RequestsThisYear > 0)
            {
                List<RequestModelReport> CompletedDesignList = snapshot.Where(x => x.DateCompleted != DateTime.Parse("1/1/0001")).ToList();
                snap.TotalCompletedDesigns = CompletedDesignList.Count;

                //loop thru completed design list and sum the days to complete
                int totalDaysToComplete = 0;
                int requestDaysToComplete = 0;
                foreach (RequestModelReport request in CompletedDesignList)
                {
                    if (request.DateCompleted > DateTime.Parse("1/1/2000") && request.AwardStatus == "Pending")
                    {
                        TimeSpan timeSpan = request.DateCompleted - request.DateAssigned;
                        requestDaysToComplete = timeSpan.Days;
                        totalDaysToComplete += requestDaysToComplete;
                    }
                }
                snap.TotalDaysToComplete = totalDaysToComplete;

                if (snap.TotalCompletedDesigns > 0)
                {
                    snap.AverageCompletionTime = (totalDaysToComplete / snap.TotalCompletedDesigns).ToString("#.#");
                }
                else
                {
                    snap.AverageCompletionTime = "";
                }
                List<RequestModelReport> snapList = snapshot.Where(x => x.DateCompleted <= DateTime.Parse("1/1/2000")).ToList();
                snap.TotalOpenRequests = snapList.Where(x => x.AwardStatus == "Pending").ToList().Count;
                snap.TotalCanceledDesigns = snapshot.Where(x => x.AwardStatus == "Canceled").ToList().Count;

                snap.RequestsThisMonth = snapshot.Where(x => month == x.DateAssigned.Month).ToList().Count;
                snap.RequestsThisWeek = snapshot.Where(x => x.DateAssigned >= startDate && x.DateAssigned <= startDate.AddDays(6)).ToList().Count;
                snap.TotalValue = snapshot.Sum(x => x.BOM_Value);
            }
            else
            {
                snap = null;
            }
            return snap;
        }

        public static void FormatSnapshotDGV(DataGridView dgv)
        {
            string[] headers = { "MSO", "Requests This Year", "Requests This Month", "Requests This Week", "Total Days to Complete",
            "Total Completed Designs", "Total Open Requests", "Total Canceled Designs", "Total Value", "Avg. Days to Complete"};
            setDGV_HeaderText(dgv, headers);
            

            int[] widths = { 140, 140, 140, 140, 140, 140, 140, 140, 140, 140 };
            SetDGV_ColumnWidths(dgv, widths);
        }

        public static void FormatLogViewDGV(DataGridView dgv)
        {
            string[] headers = { "Time Stamp", "Request ID", "User", "Action", "Affected Fields" };
            setDGV_HeaderText(dgv, headers);


            int[] widths = { 140, 200, 140, 140, 140 };
            SetDGV_ColumnWidths(dgv, widths);

            dgv.Columns[3].Visible = true;
            dgv.Columns[4].Visible = false;
        }

        public static void FormatDesignerLoadDGV(DataGridView dgv)
        {
            string[] headers = { "Designer", "Priority", "Date Due", "Project ID", "Award Status" };
            setDGV_HeaderText(dgv, headers);

            int[] widths = { 160, 60, 110, 180, 70 };
            SetDGV_ColumnWidths(dgv, widths);            
        }
        public static void FormatDesignerLoadExport(Excel.Worksheet wks)
        {
            string[] headers = { "Designer", "Priority", "Date Due", "Project ID", "Award Status" };
            placeHeaderTextInExport(wks, headers);
            formatExcelHeaderRow(wks);            

            int[] widths = { 25, 10, 15, 25,15 };
            setExcelExportColumnWidths(wks, widths);

            CenterAllExcelColumns(wks, widths);
        }

        public static void FormatSnapshotExport(Excel.Worksheet wks)
        {
            string[] headers = { "MSO", "Requests This Year", "Requests This Month", "Requests This Week", "Total Days to Complete",
            "Total Completed Designs", "Total Open Requests", "Total Canceled Designs", "Total Value", "Avg. Days to Complete"};
            placeHeaderTextInExport(wks, headers);
            formatExcelHeaderRow(wks);

            int[] widths = { 20, 12, 12, 12, 12, 12, 12, 12, 15, 12 };
            setExcelExportColumnWidths(wks, widths);

            string[] currencyCols = { "I" };
            FormatExcelColumnsAsCurrency(wks, currencyCols);

            int[] cols = { 2, 3, 4, 5, 6, 7, 8,  10 };
            CenterSpecificExcelColumns(wks, cols);
        }

        public static List<ReportCategoryMSOModel> reportCategoryMSOs(List<MSO_Model> msoList, DateTime startDate, DateTime endDate)
        {
            List<ReportCategoryMSOModel> reportList = new List<ReportCategoryMSOModel>();
            ReportCategoryMSOModel reportLine = null;
            foreach (MSO_Model mso in msoList)
            {
                List<RequestModel> requests = GlobalConfig.Connection.DateRangeSearch_MSOFiltered(startDate, endDate,
                    "DateAssigned", mso.MSO,false).ToList();

                if (requests.Count > 0)
                {
                    reportLine = new ReportCategoryMSOModel();
                    reportLine.TotalRequests = requests.Count;
                    foreach (RequestModel request in requests)
                    {
                        reportLine.MSO = request.MSO;
                        switch (request.Category)
                        {
                            case "HFC":
                                reportLine.HFC++;
                                reportLine.HFCDollars = reportLine.HFCDollars + request.BOM_Value;
                                break;
                            case "RFOG":
                                reportLine.RFoG++;
                                reportLine.RFoGDollars = reportLine.RFoGDollars + request.BOM_Value;
                                break;
                            case "RFOG PON":
                                reportLine.RFoGPON++;
                                reportLine.RFoGDollars += request.BOM_Value;
                                break;
                            case "PON":
                                reportLine.PON++;
                                reportLine.PON_Dollars = reportLine.PON_Dollars + request.BOM_Value;
                                break;
                            case "Fiber Deep":
                                reportLine.FiberDeep++;
                                reportLine.FiberDeepDollars = reportLine.FiberDeepDollars + request.BOM_Value;
                                break;
                            case "Data Transport":
                                reportLine.DataTrans++;
                                reportLine.DataTransportDollars = reportLine.DataTransportDollars + request.BOM_Value;
                                break;
                            case "PEG":
                                reportLine.PEG++;
                                reportLine.PEG_Dollars = reportLine.PEG_Dollars + request.BOM_Value;
                                break;
                            case "Commercial":
                                reportLine.Commercial++;
                                reportLine.CommercialDollars = reportLine.CommercialDollars + request.BOM_Value;
                                break;
                            case "Other":
                                reportLine.Other++;
                                reportLine.OtherDollars = reportLine.OtherDollars + request.BOM_Value;
                                break;
                            default:
                                reportLine.Unassigned++;
                                reportLine.UnassignedDollars = reportLine.UnassignedDollars + request.BOM_Value;
                                break;
                        }
                        reportLine.TotalDollars = reportLine.TotalDollars + request.BOM_Value;
                    }
                    if (reportLine.TotalRequests > 0)
                    {
                        reportLine.AverageDollarsPerRequest = reportLine.TotalDollars / reportLine.TotalRequests;
                    }
                    reportList.Add(reportLine);
                }
                else
                {
                    reportLine = null;
                }
            }

            return reportList;
        }

        public static void formatCompletionTimeExport(Excel.Worksheet wks)
        {
            //column widths
            int[] widths = { 20, 12, 12, 12, 12, 12, 12, 12 };
            setExcelExportColumnWidths(wks, widths);

            //Header Texts
            string[] headers = { "MSO", "Completed Designs", "Total Days to Complete", "Average Days to Complete",
                "Open Designs", "Total Days Open", "Average Days Open", "Canceled Designs" };
            placeHeaderTextInExport(wks, headers);

            formatExcelHeaderRow(wks);
        }
        public static void FormatCompletionTimeDGV(DataGridView dgv)
        {
            string[] headers = { "MSO", "Completed Designs","Total Days to Complete", "Average Days to Complete", "Open Designs",
            "Open Days", "Average Open Days", "Canceled Designs"};
            setDGV_HeaderText(dgv, headers);
        }
        public static void FormatRequestPriorityExport(Excel.Worksheet wks)
        {
            string[] headers = { "Sales Person", "P1 Count", "P1 Dollars", "P2 Count", "P2 Dollars", "P3 Count", "P3 Dollars",
                "Total Count", "Total Dollars" };
            placeHeaderTextInExport(wks, headers);
            formatExcelHeaderRow(wks);

            int[] widths = { 20, 12, 12, 12, 12, 12, 12, 12 };
            setExcelExportColumnWidths(wks, widths);

            string[] currencyRange = { "C", "E", "G", "I" };
            FormatExcelColumnsAsCurrency(wks, currencyRange);

            int[] cols = { 2, 4, 6, 8 };
            CenterSpecificExcelColumns(wks, cols);

        }

        private static void FormatExcelColumnsAsCurrency(Excel.Worksheet wks, string[] cols)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                string rangeString = cols[i] + ":" + cols[i];
                Excel.Range formatRange;
                formatRange = wks.get_Range(rangeString);
                formatRange.NumberFormat = "$###,###,###.00";
            }
        }

        private static void setExcelExportColumnWidths(Excel.Worksheet wks, int[] widths)
        {
            for (int i = 0; i < widths.Length; i++)
            {
                wks.Columns[i + 1].ColumnWidth = widths[i];
            }
        }

        public static void ReportFormatMultiResultExport(Excel.Worksheet wks)
        {
            string[] headers = new string[]
            {
                 "Project ID", "Quote Type", "Original Quote", "Priority", "Award Status", "Design Requestor",
                "BOM Value", "% Project Covered", "MSO", "Region", "City", "Date Assigned", "Date All Info Received", "Date Due",
                "Date Completed", "Date Last Update", "Designer", "Assisted By", "Reviewed By", "Category",
                "Architecture Details", "Total Hours", "Architecture Type", "End Customer", "State","Country", "Project Name"
            };
        {
                //"Project ID", "MSO", "End Customer", "City", "State","Country", "Region", "Design Requestor", "Quote Type",
                //"Priority", "Designer", "Project Name", "Original Quote", "Assisted By", "Category", "Architecture Type",
                //"Date Assigned", "Date All Info Received", "Date Due", "Award Status", "Date Last Update", "Reviewed By",
                //"Date Completed", "Total Hours", "BOM Value", "% Project Covered", "Architecture Details", "Comments"
        };
            placeHeaderTextInExport(wks, headers);
            formatExcelHeaderRow(wks);

            int[] widths = new int[]
            {
            //  A  B  C  D  E  F  G  H  I  J  K  L  M  N  O  P  Q  R  S  T  U  V  W  X  Y  Z  AA AB  
                26,15,26,15,17,26,17,12,22,15,25,15,15,15,15,15,22,22,22,12,22,15,22,15,20,20,50,26
            };

            string[] currencyCols = { "G" };
            FormatExcelColumnsAsCurrency(wks,currencyCols);
            setExcelExportColumnWidths(wks, widths);
            wks.get_Range("R:R").WrapText = true;
            wks.get_Range("Z:Z").WrapText = true;
            wks.get_Range("AA:AA").WrapText = false;
            wks.get_Range("L:L").WrapText = true;

            int[] cols = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 };
            CenterSpecificExcelColumns(wks, cols);
        }

        public static void FormatMultiResultExport(Excel.Worksheet wks)
        {
            string[] headers = new string[]
            {
                // "Project ID", "Quote Type", "Original Quote", "Priority", "Award Status", "Design Requestor",
                //"BOM Value", "% Project Covered", "MSO", "Region", "City", "Date Assigned", "Date All Info Received", "Date Due",
                //"Date Completed", "Date Last Update", "Designer", "Assisted By", "Reviewed By", "Category",
                //"Architecture Details", "Total Hours", "Architecture Type", "End Customer", "State","Country", "Project Name"
            
                "Project ID", "MSO", "End Customer", "City", "State","Country", "Region", "Design Requestor", "Quote Type",
                "Priority", "Designer", "Project Name", "Original Quote", "Assisted By", "Category", "Architecture Type",
                "Date Assigned", "Date All Info Received", "Date Due", "Award Status", "Date Last Update", "Reviewed By",
                "Date Completed", "Total Hours", "BOM Value", "% Project Covered", "Architecture Details", "Comments"
            };
            placeHeaderTextInExport(wks, headers);
            formatExcelHeaderRow(wks);

            int[] widths = new int[]
            {
            //  A  B  C  D  E  F  G  H  I  J  K  L  M  N  O  P  Q  R  S  T  U  V  W  X  Y  Z  AA AB  
                26,26,26,22,17,26,17,22,18,15,25,22,26,22,15,22,18,18,18,18,18,22,18,12,12,12,26,26
            };

            string[] currencyCols = { "Y" };
            FormatExcelColumnsAsCurrency(wks, currencyCols);
            setExcelExportColumnWidths(wks, widths);
            wks.get_Range("R:R").WrapText = true;
            wks.get_Range("Z:Z").WrapText = true;
            wks.get_Range("AA:AA").WrapText = false;
            wks.get_Range("L:L").WrapText = true;

            int[] cols = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 };
            CenterSpecificExcelColumns(wks, cols);
        }

        private static void placeHeaderTextInExport(Excel.Worksheet wks, string[] headers)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                wks.Cells[1, i + 1].Value = headers[i];
            }
        }

        private static void formatExcelHeaderRow(Excel.Worksheet wks)
        {
            //Center text in header/Turn word wrap on/Bold Font
            wks.Range["1:1"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            wks.Range["1:1"].WrapText = true;
            wks.Range["1:1"].Font.Bold = true;
        }

        private static void CenterAllExcelColumns(Excel.Worksheet wks, int[] widths)
        {
            for (int i = 1; i < widths.Length +1; i++)
            {
                wks.Columns[i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            }
        }

        private static void CenterSpecificExcelColumns(Excel.Worksheet wks, int[] cols)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                wks.Columns[cols[i]].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            }
        }

        //format datagridview controls
        private static void SetDGV_ColumnWidths(DataGridView dgv, int[] widths)
        {
            for (int i = 0; i < widths.Length; i++)
            {
                dgv.Columns[i].Width = widths[i];
            }
        }
        public static void FormatMultiResultDGV(DataGridView dgv)
        {
            string[] headers = new string[]
            {
                "ID","Project ID", "MSO", "End Customer", "City", "State","Country", "Region", "Design Requestor", "Quote Type",
                "Priority", "Designer", "Project Name", "Original Quote", "Assisted By", "Category", "Architecture Type",
                "Date Assigned", "Date All Info Received", "Date Due", "Award Status", "Date Last Update", "Reviewed By",
                "Date Completed", "Total Hours", "BOM Value", "% Project Covered", "Architecture Details", "Comments"            
            };

            setDGV_HeaderText(dgv, headers);

            int[] widths = { 10,220,140,140,130,130,130,100,150,90,70,140,180,200,
                140,140,100,120,100,100,120,100,100,140,100,100,150,100 };
            SetDGV_ColumnWidths(dgv, widths);
            dgv.Columns[0].Visible = false;
            dgv.Columns[29].Visible = false;

            string[] currencyCols = { "BOM_Value" };
            setDGV_CellFormatToCurrency(dgv, currencyCols);
        }

        public static void ReportFormatMultiResultDGV(DataGridView dgv)
        {
            string[] headers = new string[]

            {
                "ID", "Project ID", "Quote Type", "Original Quote", "Priority", "Award Status", "Design Requestor",
                "BOM Value", "% Project Covered", "MSO", "Region", "City", "Date Assigned", "Date All Info Received", "Date Due",
                "Date Completed", "Date Last Update", "Designer", "Assisted By", "Reviewed By", "Category", 
                "Architecture Details", "Comments", "Total Hours", "Architecture Type", "End Customer", "State","Country", "Project Name"
            };
            
            setDGV_HeaderText(dgv, headers);

            int[] widths = { 10,220,140,140,130,130,130,100,150,90,70,140,180,200,
                140,140,100,120,100,100,120,100,100,140,100,100,150,100 };
            SetDGV_ColumnWidths(dgv, widths);
            dgv.Columns[0].Visible = false;
            dgv.Columns[29].Visible = false;

            string[] currencyCols = { "BOM_Value" };
            setDGV_CellFormatToCurrency(dgv, currencyCols);
        }

        private static void setDGV_CellFormatToCurrency(DataGridView dgv, string[] cols)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                dgv.Columns[i].DefaultCellStyle.Format = "c";
            }
        }
   
        private static void setDGV_HeaderText(DataGridView dgv, string[] headers)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                dgv.Columns[i].HeaderText = headers[i];
                DataGridViewCellStyle style = dgv.ColumnHeadersDefaultCellStyle;
                style.Font = new System.Drawing.Font(dgv.Font, System.Drawing.FontStyle.Bold);
                dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                
            }
      
        }
        public static void FormatCatMSO_DGV(DataGridView dgv)
        {
            string[] headers = { "MSO", "Total Dollars", "Average $/ Request","Total Requests", "HFC","RFoG", "PON", "RFoG/ PON",
            "Fiber Deep", "Data Trans.", "Other", "PEG", "Commercial","Unassigned", "HFC Dollars","RFoG Dollars", "PON Dollars",
            "RFoG/ Pon Dollars","Fiber Deep Dollars", "Data Trans. Dollars","Other Dollars", "PEG Dollars","Commercial Dollars",
            "Unassigned Dollars" };

            int[] widths = { 150, 100, 100, 60, 60, 60, 60, 60, 60, 60, 60, 60, 75, 75, 160,
             160,160,160,160,160,160,160,160,160};

            SetDGV_ColumnWidths(dgv, widths);
            setDGV_HeaderText(dgv, headers);
        }
        public static void FormatCatMSO_Export(Excel.Worksheet wks)
        {
            string[] headers = { "MSO", "Total Dollars", "Average $/ Request","Total Requests", "HFC","RFoG", "PON", "RFoG/ PON",
            "Fiber Deep", "Data Trans.", "Other", "PEG", "Commercial","Unassigned", "HFC Dollars","RFoG Dollars", "PON Dollars",
            "RFoG/ Pon Dollars","Fiber Deep Dollars", "Data Trans. Dollars","Other Dollars", "PEG Dollars","Commercial Dollars",
            "Unassigned Dollars" };
            placeHeaderTextInExport(wks, headers);
            formatExcelHeaderRow(wks);

            int[] widths = { 25, 15, 20, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 20,
             20,20,20,20,20,20,20,20,20};
            setExcelExportColumnWidths(wks, widths);

            string[] currencyCols = { "B", "C", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X" };
            FormatExcelColumnsAsCurrency(wks, currencyCols);

            int[] cols = { 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            CenterSpecificExcelColumns(wks, cols);
        }
    }
}
