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

namespace DesignDB_Library.Operations
{
    public class ExcelOps
    {
        public static void PlaceRollupInExcel(List<ReportCategoryMSOModel> categories, List<Report_SalesProjectValuesModel> requests, decimal bomTotal)
        {
            int row = 1;
            int col = 1;

            Excel.Application xlApp = makeExcelApp();
            xlApp.Workbooks.Add();
            Excel.Worksheet wks = xlApp.ActiveSheet;
            xlApp.Visible = true;

            //Place column headings
            wks.Cells[1, 1].Value = "Salesperson";
            wks.Cells[1, 2].Value = "Total $";
            wks.Cells[1, 3].Value = "Average $";
            wks.Cells[1, 4].Value = "Total Count";
            wks.Cells[1, 5].Value = "% of Total Value";
            wks.Cells[1, 6].Value = "Jan";
            wks.Cells[1, 7].Value = "Feb";
            wks.Cells[1, 8].Value = "Mar";
            wks.Cells[1, 9].Value = "Apr";
            wks.Cells[1, 10].Value = "May";
            wks.Cells[1, 11].Value = "Jun";
            wks.Cells[1, 12].Value = "Jul";
            wks.Cells[1, 13].Value = "Aug";
            wks.Cells[1, 14].Value = "Sep";
            wks.Cells[1, 15].Value = "Oct";
            wks.Cells[1, 16].Value = "Nov";
            wks.Cells[1, 17].Value = "Dec";
            wks.Cells[1, 18].Value = "Current Week";

            wks.Columns[1].ColumnWidth = 28;
            wks.get_Range("B:C").ColumnWidth = 20;
            wks.get_Range("D:Y").ColumnWidth=12;

            Excel.Range header = wks.get_Range("A1:R1");
            header.Font.Bold = true;
            header.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            header.Interior.Color = ColorTranslator.ToOle(System.Drawing.Color.LightSkyBlue);
            header.WrapText = true;

            row = 2;
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
            wks.Cells[row, 2].Value = bomTotal;

            Excel.Range currencyRange = wks.Range[wks.Cells[2, 2], wks.Cells[row, 3]];
            FormatExcelRangeAsCurrency(wks, currencyRange);
            row = row + 3;
            wks.Cells[row, 1].Value = "MSO";
            wks.Cells[row, 2].Value = "Total $";
            wks.Cells[row, 3].Value = "Average $";
            wks.Cells[row, 4].Value = "Total Count";
            wks.Cells[row, 5].Value = "% of Total Value";
            wks.Cells[row, 6].Value = "HFC";
            wks.Cells[row, 7].Value = "Node Split";
            wks.Cells[row, 8].Value = "RFoG";
            wks.Cells[row, 9].Value = "PON";
            wks.Cells[row, 10].Value = "Fiber Deep";
            wks.Cells[row, 11].Value = "Data Transport";
            wks.Cells[row, 12].Value = "Other";
            wks.Cells[row, 13].Value = "PEG";
            wks.Cells[row, 14].Value = "Commercial";
            wks.Cells[row, 15].Value = "Unassigned";
            wks.Cells[row, 16].Value = "HFC Dollars";
            wks.Cells[row, 17].Value = "Node Split Dollars";
            wks.Cells[row, 18].Value = "RFoG Dollars";
            wks.Cells[row, 19].Value = "PON Dollars";
            wks.Cells[row, 20].Value = "Fiber Deep Dollars";
            wks.Cells[row, 21].Value = "Data Transport Dollars";
            wks.Cells[row, 22].Value = "Other Dollars";
            wks.Cells[row, 23].Value = "PEG Dollars";
            wks.Cells[row, 24].Value = "Commercial Dollars";
            wks.Cells[row, 25].Value = "Unassigned Dollars";

            int categoryStartRow = row;
            Excel.Range header2 = wks.Range[wks.Cells[categoryStartRow, 1], wks.Cells[row, 25]];
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
                wks.Cells[row, 10] = model.FiberDeep;
                wks.Cells[row, 11] = model.DataTrans;
                wks.Cells[row, 12] = model.Other;
                wks.Cells[row, 13] = model.PEG;
                wks.Cells[row, 14] = model.Commercial;
                wks.Cells[row, 15] = model.Unassigned;
                wks.Cells[row, 16] = model.HFCDollars;
                wks.Cells[row, 17] = model.NodeSplitDollars;
                wks.Cells[row, 18] = model.RFoGDollars;
                wks.Cells[row, 19] = model.PON_Dollars;
                wks.Cells[row, 20] = model.FiberDeepDollars;
                wks.Cells[row, 21] = model.DataTransportDollars;
                wks.Cells[row, 22] = model.OtherDollars;
                wks.Cells[row, 23] = model.PEG_Dollars;
                wks.Cells[row, 24] = model.CommercialDollars;
                wks.Cells[row, 25] = model.UnassignedDollars;
                row++;
            }

            currencyRange = wks.Range[wks.Cells[categoryStartRow, 2], wks.Cells[row, 3]];
            FormatExcelRangeAsCurrency(wks, currencyRange);

            currencyRange = wks.Range[wks.Cells[categoryStartRow, 16], wks.Cells[row, 25]];
            FormatExcelRangeAsCurrency(wks, currencyRange);
            Excel.Range numRange = wks.Range[wks.Cells[categoryStartRow, 1], wks.Cells[row, 25]];
            Excel.Range summaryRange = wks.Range[wks.Cells[row-1, 1], wks.Cells[row-1, 25]];
            summaryRange.Font.Bold = true;
            numRange.NumberFormat = "0.00";


            releaseObject(xlApp);
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
