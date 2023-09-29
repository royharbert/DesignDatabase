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
        const string BOMFilePath = "\\\\usca5pdbatdgs01\\Databases\\AttachmentsDesign";
        public static void ShipmentToBOMCompare(string fileName, DateTime startDate, DateTime endDate, List<MSO_Model> MSOs)
        {
            //Get shipment spreadsheet
            Application xlApp = ExcelOps.makeExcelApp();
            sendMessage("Opening Shipments File");
            Workbook wkb = xlApp.Workbooks.Open(fileName);
            Worksheet wks = xlApp.ActiveSheet;
            Range searchRange= wks.get_Range("A1:Z26");
            xlApp.Visible = true;
            
            int lastRow = FindLastSpreadsheetRow(wks);
            int QuanCol = GetColumn(wks, "Shipped Sales Quantity", searchRange);
            int SODateCol = GetColumn(wks, "SO Item Create Date", searchRange);
            int SOCustCol = GetColumn(wks, "Sold To Cust Name", searchRange);
            int PartNumberCol = GetColumn(wks, "Material ID", searchRange);
            int DescCol = GetColumn(wks, "Material Description", searchRange);
            int CityCol = GetColumn(wks, "Ship To Cust City Name", searchRange);
            int StateCol = GetColumn(wks, "Ship To Cust State Code", searchRange);
            int SOCol = GetColumn(wks, "Ship To Cust Name", searchRange);

            //Load SS into List
            sendMessage("Loading Spreadsheet into List");
            List<ShipmentLineModel> shipmentList = new List<ShipmentLineModel>();            
            int row = 17;
            for (int i = row; i < lastRow; i++)
            {
                ShipmentLineModel shipment = new ShipmentLineModel();
                shipment.Desc = wks.Cells[i, DescCol].Value;
                shipment.PartNumber = wks.Cells[i, PartNumberCol].Value;
                shipment.QuoteCity = wks.Cells[i, CityCol].Value;
                shipment.SOCust = wks.Cells[i, SOCustCol].Value;
                shipment.QuoteState = wks.Cells[i, StateCol].Value;
                shipment.Quantity = wks.Cells[i, QuanCol].Value;
                shipment.SODate = wks.Cells[i, SODateCol].Value;
                shipment.SONumber = wks.Cells[i, SOCol].Value;
                shipment.ExcelRow = i;
                shipmentList.Add(shipment);

                shipment = null;
            }
            wkb.Close();
            shipmentList = shipmentList.OrderBy(x => x.PartNumber).ToList();
            //Get Date Range
            sendMessage("Retrieving and Filtering Quotes in Date Range");
            List<RequestModel> requestList = GlobalConfig.Connection.DateRangeSearch_Unfiltered(startDate, endDate);
            List<RequestModel> msoRequests = new List<RequestModel>();
            List<ShipmentLineModel> msoShipmentList = new List<ShipmentLineModel>();
            foreach (var mso in MSOs)
            {
                foreach (var shipment in shipmentList)
                {
                    if (shipment.SOCust.Contains(mso.MSO));
                    {
                        msoShipmentList.Add(shipment);
                    }
                }
                //msoShipmentList = shipmentList.Where(x => x.SOCust.Contains(mso.MSO)).ToList();
                msoRequests = requestList.Where(x => x.MSO == mso.MSO).ToList();
                string PIDs = GetPIDsFromRequests(msoRequests);
                List<BOMLineModel> bomFiles = GlobalConfig.Connection.getBOMList(PIDs);
                ProcessBOM(xlApp, msoShipmentList, bomFiles, msoRequests, lastRow);
            }

            ExcelOps.releaseObject(xlApp);
        }

        private static void ProcessBOM(Excel.Application xlApp, List<ShipmentLineModel> shipments, List<BOMLineModel> BOMList, 
            List<RequestModel> msoRequests, int lastRow)
        {
            foreach (var BOM in BOMList) 
            {
                sendMessage("Opening " + BOM.DisplayText);
                string BOMName = BOMFilePath + "\\" + BOM.PID +"\\" + BOM.DisplayText;
                xlApp.Workbooks.Open(BOMName);
                Worksheet wks = xlApp.ActiveSheet;
                List<BOM_Model> BOMmodel = LoadBOMtoList(wks, lastRow, BOM.PID);
                foreach (var shipment in shipments) 
                {
                    RequestModel request = msoRequests.Where(x => x.ProjectID == BOM.PID).FirstOrDefault();
                    shipment.QuoteCity = request.City;
                    shipment.QuoteState = request.ST;
                    shipment.QuoteDateCompleted = request.DateCompleted.ToShortDateString();
                }
                //Compare BOM to shipment
                xlApp.Workbooks.Close();

            }

        }

        private static List<BOM_Model> LoadBOMtoList(Worksheet wks, int lastRow, string PID)
        {
            List<BOM_Model> models = new List<BOM_Model>();
            Range searchRange = wks.get_Range("A1:Z26");
            int lastBOMRow = FindLastSpreadsheetRow(wks);
            int headerRow = FindHeaderRow(searchRange, "Quantity");
            int quanCol = GetColumn(wks, "Quantity", searchRange);
            int descCol = GetColumn(wks, "Description", searchRange);
            int modelCol = GetColumn(wks, "Model Number", searchRange);
            int row = headerRow + 1;
            for (int i = row; i <= lastBOMRow - 1; i++)
            {
                BOM_Model model = new BOM_Model();
                model.Description = wks.Cells[i, descCol].Value;
                model.Quote = PID;
                model.ModelNumber = wks.Cells[i, modelCol].Value; 
                model.Quantity = wks.Cells[i, quanCol].Value;
                models.Add(model);
            }

            return models;
        }
        private static void CompareBOMtoShipmentsl(List<ShipmentLineModel> shipments,  RequestModel msoRequests)
        {

        }
        
        private static string GetPIDsFromRequests(List<RequestModel> msoRequests)
        {
            sendMessage("Getting BOM file names");
            StringBuilder sb = new StringBuilder();
            foreach (var request in msoRequests)
            {
                sb.Append(request.ProjectID + ',');
            }
            string PIDs = sb.ToString();
            PIDs = PIDs.Substring(0, PIDs.Length - 1);
            return PIDs;
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
