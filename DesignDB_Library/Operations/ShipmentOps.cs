using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using DesignDB_Library.Operations;
using DesignDB_Library.Models;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
namespace DesignDB_Library.Operations
{
    public static class ShipmentOps
    {
        public static void ShipmentToBOMCompare(string fileName, DateTime startDate, DateTime endDate, List<MSO_Model> MSO)
        {
            //Get shipment spreadsheet
            Application xlApp = ExcelOps.makeExcelApp();
            sendMessage("Opening Shipments File");
            Workbook wkb = xlApp.Workbooks.Open(fileName);  
            xlApp.Visible = true;
            //Get Date Range
            List<RequestModel> requestList = GlobalConfig.Connection.DateRangeSearch_Unfiltered(startDate, endDate);
            //Identify SS Columns
            //Make a data table for shipments
            //Insert table in database
            //Get Project ID's for date range + MSO
            //Get BOM File names
        }

        public static void sendMessage(string message)
        {
            ReportOps.sendMessage(message);
        }
    }
}
