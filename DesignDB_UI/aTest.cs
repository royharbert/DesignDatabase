using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using DesignDB_Library.Models;
using DesignDB_Library.Operations;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;

namespace DesignDB_UI
{
    public partial class aTest : Form
    {
        public aTest()
        {
            InitializeComponent();
            dtp.Text = DateTime.Parse("1/1/2010").ToShortDateString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //dtp.Value = new DateTime(1900, 1, 1);
            //Application.DoEvents();
            //dtp.CustomFormat = " ";
            //dtp.Format = DateTimePickerFormat.Custom;
            //txtBox.Text = dtp.Value.ToShortDateString();
            //ReportOps.NumberOfWorkDays(new DateTime(2023, 1, 1), new DateTime(2023, 6, 12));
            //Excel.Application xlApp = ExcelOps.makeExcelApp();
            //xlApp.Workbooks.Add();
            //xlApp.Visible = true;
            //Workbook wkb = xlApp.ActiveWorkbook;
            //Workbook wkb = xlApp.Workbooks.Open("C:\\Users\\rharbert\\OneDrive - CommScope\\Documents\\__xCopy of 091923-Backlog and Shipments - Copy.xlsx");
            //wkb = ExcelOps.SortSpreadsheetByColumn(wkb);
            object obj = new object();
            obj = "75%";
            txtBox.Text = obj.ToString();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            Excel.Application xlApp = ExcelOps.makeExcelApp();
            xlApp.Workbooks.Add();
            Excel.Worksheet wks = xlApp.ActiveSheet;
            xlApp.Visible = true;
            Excel.Worksheet wks = xlApp.ActiveSheet;
            string[] columnNames = new string[] { "Project ID", "File Name", "Quote Type", "Original Quote", "Priority",
                "Award Status", "Design Requestor", "BOM Value", "% Project Covered", "Project Value","MSO", "Region",
                "City", "Date Assigned", "Date All Info Received", "Date Due", "Date Completed", "Date Last Update",
                "Designer", "Assisted By", "Reviewed By", "Revision", "Category", "Architecture Details", "Comments",
                "Total Hours" };
            makeReportSheet(xlApp, columnNames);
            object obj = ExcelOps.GetCellValue(wks, 1, 5);
            dynamic dynamic = (dynamic)obj;
        }

        private Excel.Worksheet makeReportSheet(Excel.Application xlApp, string[] headers)
        {
            Excel.Worksheet wks = xlApp.ActiveSheet;
            for (int i = 0; i < headers.Length; i++)
            {
                wks.Cells[1, i + 1].Value = headers[i];
            }
            formatHeaderRow(wks);

            ExcelOps.releaseObject(wks);
            ExcelOps.releaseObject(xlApp);
            return wks;
        }

        private void formatHeaderRow(Excel.Worksheet wks)
        {
            Excel.Range header;
            header = wks.get_Range("1:1");
            header.Font.Bold = true;
            header.WrapText = true;
            header.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aqua);
        }

        private void dtp_ValueChanged(object sender, EventArgs e)
        {
            dtp.Format = DateTimePickerFormat.Short;
            txtBox.Text = dtp.Value.ToShortDateString();
        }
    }
}

          
