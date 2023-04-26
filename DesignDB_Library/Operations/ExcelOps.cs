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

namespace DesignDB_Library.Operations
{
    public class ExcelOps
    {
        public static void PlaceRollupInExcel(DateTime startDate, DateTime endDate, List<OpenRequestsBySalesModel> openBySales, 
            List<ReportCategoryMSOModel> categories, List<Report_SalesProjectValuesModel> requests, 
            List<ReportSalesPriorityModel> priorityList, decimal bomTotal, List<Report_SalesProjectValuesModel> msoSummary, List<MSO_Model> msoModels,
            List<List<RequestModel>> awards)
        {
            int row = 1;

            Excel.Application xlApp = makeExcelApp();
            xlApp.Workbooks.Add();
            Excel.Worksheet wks = xlApp.ActiveSheet;
            xlApp.Visible = true;

            //Place column headings
            wks.Cells[2, 1].Value = "Salesperson";
            wks.Cells[2, 2].Value = "Total $ *";
            wks.Cells[2, 3].Value = "Average $";
            wks.Cells[2, 4].Value = "Total Count";
            wks.Cells[2, 5].Value = "% of Total Value";
            wks.Cells[2, 6].Value = "Jan";
            wks.Cells[2, 7].Value = "Feb";
            wks.Cells[2, 8].Value = "Mar";
            wks.Cells[2, 9].Value = "Apr";
            wks.Cells[2, 10].Value = "May";
            wks.Cells[2, 11].Value = "Jun";
            wks.Cells[2, 12].Value = "Jul";
            wks.Cells[2, 13].Value = "Aug";
            wks.Cells[2, 14].Value = "Sep";
            wks.Cells[2, 15].Value = "Oct";
            wks.Cells[2, 16].Value = "Nov";
            wks.Cells[2, 17].Value = "Dec";
            wks.Cells[2, 18].Value = "Current Week " + startDate.ToShortDateString() + " " + endDate.ToShortDateString();
                
            wks.Columns[1].ColumnWidth = 28;
            wks.get_Range("B:C").ColumnWidth = 20;
            wks.get_Range("D:Z").ColumnWidth=15;

            makeTitle(wks, 1, 18, "Design Requests by Salesperson/Month");
            
            Excel.Range header = wks.get_Range("A1:R2");
            header.Font.Bold = true;
            header.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            header.Interior.Color = ColorTranslator.ToOle(System.Drawing.Color.LightSkyBlue);
            header.WrapText = true;

            row = 3;
            foreach (var model in requests)
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
            int categoryStartRow = row;
            Excel.Range decRange = wks.Range[wks.Cells[2, 5], wks.Cells[row, 5]];
            decRange.NumberFormat = "###.0%";

            Excel.Range currencyRange = wks.Range[wks.Cells[2, 2], wks.Cells[row, 3]];
            FormatExcelRangeAsCurrency(wks, currencyRange);
            Excel.Range summaryRange = wks.Range[wks.Cells[row - 1, 1], wks.Cells[row - 1, 17]];
            summaryRange.Font.Bold = true;

            //Monthly MSO Summary
            row = row + 3;
            makeTitle(wks, row, 18, "Requests by MSO/Month");
            row++;
            wks.Cells[row, 1].Value = "MSO";
            wks.Cells[row, 2].Value = "Total $";
            wks.Cells[row, 3].Value = "Average $";
            wks.Cells[row, 4].Value = "Total Count";
            wks.Cells[row, 5].Value = "% of Total Value";
            wks.Cells[row, 6].Value = "Jan";
            wks.Cells[row, 7].Value = "Feb";
            wks.Cells[row, 8].Value = "Mar";
            wks.Cells[row, 9].Value = "Apr";
            wks.Cells[row, 10].Value = "May";
            wks.Cells[row, 11].Value = "Jun";
            wks.Cells[row, 12].Value = "Jul";
            wks.Cells[row, 13].Value = "Aug";
            wks.Cells[row, 14].Value = "Sep";
            wks.Cells[row, 15].Value = "Oct";
            wks.Cells[row, 16].Value = "Nov";
            wks.Cells[row, 17].Value = "Dec";
            wks.Cells[row, 18].Value = "Current Week " + startDate.ToShortDateString() + " " + endDate.ToShortDateString();
            //wks.Cells[row, 19].Value = "TOTAL";
            wks.Columns[1].ColumnWidth = 28;

            header = wks.Range[wks.Cells[categoryStartRow + 2, 1], wks.Cells[row, 18]];
            header.Font.Bold = true;
            header.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            header.Interior.Color = ColorTranslator.ToOle(System.Drawing.Color.LightSkyBlue);
            header.WrapText = true;
            row++;
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
                //wks.Cells[row, 19] = model.Total;
                row++; 
            }
            decRange = wks.Range[wks.Cells[categoryStartRow, 5], wks.Cells[row, 5]];
            decRange.NumberFormat = "###.0%";

            currencyRange = wks.Range[wks.Cells[categoryStartRow, 2], wks.Cells[row, 3]];
            FormatExcelRangeAsCurrency(wks, currencyRange);
            summaryRange = wks.Range[wks.Cells[row - 1, 1], wks.Cells[row - 1, 18]];
            summaryRange.Font.Bold = true;


            row = row + 3;
            makeTitle(wks, row,12,"Award Status Summary");
            categoryStartRow = row;
            header = wks.Range[wks.Cells[categoryStartRow - 1, 1], wks.Cells[row, 12]];
            header.Font.Bold = true;
            header.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            header.Interior.Color = ColorTranslator.ToOle(System.Drawing.Color.LightSkyBlue);
            header.WrapText = true;
            row = row + 2;
            wks.Cells[row, 1].Value = "Pending Count";
            wks.Cells[row, 2].Value = "Pending $";
            wks.Cells[row, 3].Value = "Has Revision Count";
            wks.Cells[row, 4].Value = "Has Revision $";
            wks.Cells[row, 5].Value = "Canceled Count";
            wks.Cells[row, 6].Value = "Canceled $";
            wks.Cells[row, 7].Value = "Inactive Count";
            wks.Cells[row, 8].Value = "Inactive $";
            wks.Cells[row, 9].Value = "Yes Count";
            wks.Cells[row, 10].Value = "Yes $";
            wks.Cells[row, 11].Value = "No Count";
            wks.Cells[row, 12].Value = "No $";
            header = wks.Range[wks.Cells[row - 2, 1], wks.Cells[row, 12]];
            header.Font.Bold = true;
            header.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            header.Interior.Color = ColorTranslator.ToOle(System.Drawing.Color.LightSkyBlue);
            header.WrapText = true;
            row++;

            int col = 1;
            row = row++;
            for (int i = 0; i < awards.Count; i++)
            {
                List<RequestModel> status = awards[i];
                col = placeAwardStatusData(status, wks, row, col); 
            }

            row = row + 5;
            categoryStartRow = row;
            makeTitle(wks, row, 21, "Design Requests by MSO/Category");
            row++;
            wks.Cells[row, 1].Value = "MSO";
            wks.Cells[row, 2].Value = "Total $";
            wks.Cells[row, 3].Value = "Average $";
            wks.Cells[row, 4].Value = "Total Count";
            wks.Cells[row, 5].Value = "% of Total Value";
            wks.Cells[row, 6].Value = "HFC";
            wks.Cells[row, 7].Value = "Node Split";
            wks.Cells[row, 8].Value = "RFoG";
            wks.Cells[row, 9].Value = "PON";
            wks.Cells[row, 10].Value = "RFoG-PON";
            wks.Cells[row, 11].Value = "Fiber Deep";
            wks.Cells[row, 12].Value = "Data Transport";
            wks.Cells[row, 13].Value = "Other";
            wks.Cells[row, 14].Value = "Unassigned";
            wks.Cells[row, 15].Value = "HFC Dollars";
            wks.Cells[row, 16].Value = "Node Split Dollars";
            wks.Cells[row, 17].Value = "RFoG Dollars";
            wks.Cells[row, 18].Value = "PON Dollars";
            wks.Cells[row, 19].Value = "RFoG-PON Dollars";
            wks.Cells[row, 20].Value = "Fiber Deep Dollars";
            wks.Cells[row, 21].Value = "Data Transport Dollars";
            wks.Cells[row, 22].Value = "Other Dollars";
            //wks.Cells[row, 21].Value = "PEG Dollars";
            //wks.Cells[row, 22].Value = "Commercial Dollars";
            wks.Cells[row, 23].Value = "Unassigned Dollars";

           
            Excel.Range header2 = wks.Range[wks.Cells[categoryStartRow - 1, 1], wks.Cells[row, 23]];
            header2.Font.Bold = true;
            header2.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            header2.Interior.Color = ColorTranslator.ToOle(System.Drawing.Color.LightSkyBlue);
            header2.WrapText = true;
            row++;
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
                //wks.Cells[row, 13] = model.PEG;
                //wks.Cells[row, 14] = model.Commercial;
                wks.Cells[row, 14] = model.Unassigned;
                wks.Cells[row, 15] = model.HFCDollars;
                wks.Cells[row, 16] = model.NodeSplitDollars;
                wks.Cells[row, 17] = model.RFoGDollars;
                wks.Cells[row, 18] = model.PON_Dollars;
                wks.Cells[row, 19] = model.RFoGPON_Dollars;
                wks.Cells[row, 20] = model.FiberDeepDollars;
                wks.Cells[row, 21] = model.DataTransportDollars;
                wks.Cells[row, 22] = model.OtherDollars;
                //wks.Cells[row, 21] = model.PEG_Dollars;
                //wks.Cells[row, 22] = model.CommercialDollars;
                wks.Cells[row, 23] = model.UnassignedDollars;
                row++;
            }


            Excel.Range numRange = wks.Range[wks.Cells[categoryStartRow, 1], wks.Cells[row, 23]];
            summaryRange = wks.Range[wks.Cells[row-1, 1], wks.Cells[row-1, 23]];
            summaryRange.Font.Bold = true;
            //numRange.NumberFormat = "0";

            Excel.Range pctRange = wks.Range[wks.Cells[categoryStartRow + 1, 5], wks.Cells[row, 5]];
            pctRange.NumberFormat = "###.0%";

            currencyRange = wks.Range[wks.Cells[categoryStartRow, 2], wks.Cells[row, 3]];
            FormatExcelRangeAsCurrency(wks, currencyRange);
            currencyRange = wks.Range[wks.Cells[categoryStartRow, 15], wks.Cells[row, 23]];
            FormatExcelRangeAsCurrency(wks, currencyRange);

            //openBySales
            row = row + 3;
            makeTitle(wks, row, 14, "Open Design Requests by Salesperson/Month");
            row++;
            int openStartRow = row - 1;
            Excel.Range header3 = wks.Range[wks.Cells[openStartRow, 1], wks.Cells[row, 14]];
            header3.Font.Bold = true;
            header3.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            header3.Interior.Color = ColorTranslator.ToOle(System.Drawing.Color.LightSkyBlue);
            header3.WrapText = true;
            //row++;
            wks.Cells[row, 1].Value = "Sales Person";
            wks.Cells[row, 2].Value = "Total";
            wks.Cells[row, 3].Value = "Jan";
            wks.Cells[row, 4].Value = "Feb";
            wks.Cells[row, 5].Value = "Mar";
            wks.Cells[row, 6].Value = "Apr";
            wks.Cells[row, 7].Value = "May";
            wks.Cells[row, 8].Value = "Jun";
            wks.Cells[row, 9].Value = "Jul";
            wks.Cells[row, 10].Value = "Aug";
            wks.Cells[row, 11].Value = "Sep";
            wks.Cells[row, 12].Value = "Oct";
            wks.Cells[row, 13].Value = "Nov";
            wks.Cells[row, 14].Value = "Dec";
            row++;
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
            summaryRange = wks.Range[wks.Cells[row - 1, 1], wks.Cells[row - 1, 14]];
            summaryRange.Font.Bold = true;
            InsertPriorityDataIntoWorksheet(wks, row + 2, priorityList, msoModels);

            releaseObject(xlApp);
        }

        private static int  placeAwardStatusData(List<RequestModel> status, Excel.Worksheet wks, int row, int col)
        {
            wks.Cells[row, col].value = status.Count;
            wks.Cells[row, col + 1].value = status.Sum(x => x.BOM_Value);
            wks.Cells[row, col + 1].NumberFormat = "$###,###,###.00";
            col = col + 2;
            return col;
        }

        private static void makeTitle(Excel.Worksheet wks, int row, int rightmostCol, string title)
        {
            wks.Cells[row, 1].Value = title;
            wks.Cells[row, 1].Font.Size = 20;
            wks.Cells[row, 1].Font.Bold = true;
            Excel.Range range = wks.Range[wks.Cells[row, 1], wks.Cells[row, rightmostCol]];
            range.Cells.HorizontalAlignment = HorizontalAlignment.Center;
            range.Cells.Merge();
        }
        private static void InsertPriorityDataIntoWorksheet(Excel.Worksheet wks, int startRow, List<ReportSalesPriorityModel> list, List<MSO_Model> MSO_model)
        {
            int row = startRow;
            makeTitle(wks, row, 5, "Design Requests by Salesperson/Priority");
            row++;
            Excel.Range header3 = wks.Range[wks.Cells[row - 1, 1], wks.Cells[row, 5]];
            header3.Font.Bold = true;
            header3.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            header3.Interior.Color = ColorTranslator.ToOle(System.Drawing.Color.LightSkyBlue);
            header3.WrapText = true;

            wks.Cells[row, 1].Value = "Sales Person";
            wks.Cells[row, 2].Value = "Total";
            wks.Cells[row, 3].Value = "P1";
            wks.Cells[row, 4].Value = "P2";
            wks.Cells[row, 5].Value = "P3";
            row++;
            foreach (var model in list)
            {
                wks.Cells[row, 1].Value = model.SalesPerson;
                wks.Cells[row, 2].Value = model.TotalCount;
                wks.Cells[row, 3].Value = model.P1Pct;
                wks.Cells[row, 4].Value = model.P2Pct;
                wks.Cells[row, 5].Value = model.P3Pct;
                row++;
            }
            Excel.Range pctRange = wks.Range[wks.Cells[startRow + 2, 3], wks.Cells[row - 1, 5]];
            pctRange.NumberFormat = "##.00%";
            wks.Cells[row - 1, 2].NumberFormat = "##%";
            Excel.Range boldRange = wks.Range[wks.Cells[row - 1, 1], wks.Cells[row, 5]];
            boldRange.Font.Bold = true;
        }

            private static void FormatExcelRangeAsCurrency(Excel.Worksheet wks, Excel.Range range)
        {   
            range.NumberFormat = "$###,###,###.00";            
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
