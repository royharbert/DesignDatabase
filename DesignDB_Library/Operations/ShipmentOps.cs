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
using System.Runtime.Remoting;

namespace DesignDB_Library.Operations
{
    public static class ShipmentOps
    {
        public static void ShipmentToBOMCompare(string fileName, DateTime startDate, DateTime endDate, List<MSO_Model> MSOs)
        {
            //Get shipment spreadsheet
            Application xlApp = ExcelOps.makeExcelApp();
            sendMessage("Opening Shipments File");
            Workbook wkb = xlApp.Workbooks.Open(fileName);
            Worksheet wks = xlApp.ActiveSheet;
            xlApp.Visible = true;

            //foreach (var mso in MSOs)
            //{
            //    List<ShipmentLineModel> shipmentsMSO = 
            //}
            // Find necessary rows/columns in SS
            int lastRow = FindLastSpreadsheetRow(wks);
            Range searchRange= wks.get_Range("A1:Z26");
            int QuanCol = GetColumn(wks, "Shipped Sales Quantity", searchRange);
            int SODateCol = GetColumn(wks, "SO Item Create Date", searchRange);
            int SOCustCol = GetColumn(wks, "Sold To Cust Name", searchRange);
            int PartNumberCol = GetColumn(wks, "Material ID", searchRange);
            int DescCol = GetColumn(wks, "Material Description", searchRange);
            int CityCol = GetColumn(wks, "Ship To Cust City Name", searchRange);
            int StateCol = GetColumn(wks, "Ship To Cust State Code", searchRange);
            int SOCol = GetColumn(wks, "Ship To Cust Name", searchRange);

            //Load SS into List
            List<ShipmentLineModel> shipmentList = new List<ShipmentLineModel>();            
            int row = 17;
            for (int i = row; i < lastRow; i++)
            {
                ShipmentLineModel shipment = new ShipmentLineModel();
                shipment.Desc = wks.Cells[i, DescCol].Value;
                shipment = null;
            }
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

        private static int FindLastSpreadsheetRow(Worksheet  wks)
        {
            int rowIndex = wks.Cells.Find("*", System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, 
                XlSearchOrder.xlByRows, Microsoft.Office.Interop.Excel.XlSearchDirection.xlPrevious, false, System.Reflection.Missing.Value, 
                System.Reflection.Missing.Value).Row;

            return rowIndex;
        }

        private static int FindHeaderRow(Range range, string searchTerm)
        {
            Excel.Range result = range.Find(searchTerm);
            return result.Row;
        }

        private static int GetColumn(Worksheet wks, string searchTerm, Range range)
        {
            Range result = range.Find(searchTerm);
            return result.Column;
        }

    }
}
