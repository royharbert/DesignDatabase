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

namespace DesignDB_Library.Operations
{
    public class ExcelOps
    {
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
