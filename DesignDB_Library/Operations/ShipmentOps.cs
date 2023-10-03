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
using System.Text.RegularExpressions;

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
            Workbook wkbResults = xlApp.Workbooks.Add();
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
            int SOCol = GetColumn(wks, "IC Invoice ID", searchRange);

            //Load SS into List
            sendMessage("Loading Spreadsheet into List");
            List<ShipmentLineModel> shipmentList = new List<ShipmentLineModel>();            
            int row = 17;
            for (int i = row; i < lastRow; i++)
            {
                ShipmentLineModel shipment = new ShipmentLineModel();
                shipment.Desc = wks.Cells[i, DescCol].Value;
                shipment.PartNumber = wks.Cells[i, PartNumberCol].Value;
                shipment.City = wks.Cells[i, CityCol].Value;
                shipment.SOCust = wks.Cells[i, SOCustCol].Value;
                shipment.State = wks.Cells[i, StateCol].Value;
                shipment.Quantity = wks.Cells[i, QuanCol].Value;
                shipment.SODate = wks.Cells[i, SODateCol].Value;
                if (wks.Cells[i, SOCol].Value != null)
                {
                    shipment.SONumber = wks.Cells[i, SOCol].Value.ToString(); 
                }
                else
                {
                    shipment.SONumber = "None";
                }
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
                ProcessBOM(xlApp, wks, wkbResults, msoShipmentList, msoRequests, lastRow, bomFiles);
            }

            ExcelOps.releaseObject(xlApp);
        }
        /// <summary>
        /// Opens BOM, loads items into List BOM_Model, populates BOM related items into ShipmentLineModel
        /// </summary>
        /// <param name="xlApp"></param>
        /// <param name="shipments"></param>
        /// <param name="BOMList"></param>
        /// <param name="msoRequests"></param>
        /// <param name="lastRow"></param>
        private static void ProcessBOM(Excel.Application xlApp, Worksheet wks, Workbook wkbResults, List<ShipmentLineModel> shipments, 
            List<RequestModel> msoRequests, int lastRow, List<BOMLineModel> BOMLineList, List<BOM_Model> BOMList= null)
        {
            foreach (var BOM in BOMLineList) 
            {
                //Workbook wkb = xlApp.Workbooks.Add();
                //Worksheet wks = wkb.Sheets.Add();
                //Open BOM attachment from server
                wks = OpenBOMFile(BOM, xlApp);

                //Load this BOM into List
                List<BOM_Model> BOMLines = LoadBOMtoList(wks, lastRow, BOM.PID);

                //Load BOMLineModel data into BOM_Model
                foreach (var line in BOMLines)
                {
                    BOMLineModel lineModel = BOMLineList.Where(x => x.PID == line.Quote).FirstOrDefault();
                    line.DisplayText = lineModel.DisplayText;
                }

                foreach (var shipment in shipments) 
                {
                    RequestModel request = msoRequests.Where(x => x.ProjectID == BOM.PID).FirstOrDefault();
                    shipment.QuoteCity = request.City;
                    shipment.QuoteState = request.ST;
                    shipment.QuoteDateCompleted = request.DateCompleted.ToShortDateString();
                }
                //Compare BOM to shipment
                CompareBOMtoShipmentsl(xlApp, wkbResults, wks, lastRow, shipments, msoRequests, BOM);
            }
        }

        private static Worksheet OpenBOMFile(BOMLineModel BOM, Excel.Application xlApp)
        {
            sendMessage("Opening " + BOM.DisplayText);
            string BOMName = BOMFilePath + "\\" +BOM.PID + "\\" + BOM.DisplayText;
            xlApp.Workbooks.Open(BOMName);
            Worksheet wks = xlApp.ActiveSheet;
            return wks;
        }


        /// <summary>
        /// Places BIM items into List<BOM_Model></BOM_Model>
        /// </summary>
        /// <param name="wks"></param>
        /// <param name="lastRow"></param>
        /// <param name="PID"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Creates a list of Part Number matches from BOM to Quote
        /// </summary>
        /// <param name="xlApp"></param>
        /// <param name="wks"></param>
        /// <param name="lastRow"></param>
        /// <param name="shipments"></param>
        /// <param name="msoRequests"></param>
        /// <param name="BOM"></param>
        private static void CompareBOMtoShipmentsl(Excel.Application xlApp, Workbook wkbResults, Worksheet wks, int lastRow, List<ShipmentLineModel> shipments,  
            List<RequestModel> msoRequests, BOMLineModel BOM)
        {
            //ProcessBOM(xlApp, shipments, msoRequests, lastRow, bomLineList, BOM);
            List<BOM_Model> bomItems = LoadBOMtoList(wks, lastRow, BOM.PID);
            string msg = "Analyzing Quote" + BOM.PID;
            sendMessage(msg);

            Dictionary<int, (string soNumber, string partNumber) > pnMatch = new Dictionary<int, (string soNumber, string partNumber)>();
            List<ShipmentLineModel> bomMatches = new List<ShipmentLineModel>();
            string quoteID = "";
            foreach (var item in bomItems)
            {
                quoteID = item.Quote;
                sendMessage(msg + "     " + item.ModelNumber);
                List<ShipmentLineModel> matches = shipments.Where(x => x.PartNumber == item.ModelNumber).ToList();
                (string soNumber, string partNumber) so_part;
                if (matches.Count > 0)
                {
                    foreach (var match in matches)
                    {
                        bomMatches.Add(match);
                        so_part.soNumber = match.SONumber;
                        so_part.partNumber = match.PartNumber;
                        pnMatch.Add(match.ExcelRow, so_part);
                        item.pnMatchList = pnMatch;
                    }
                }
            }
            wks = MakeMatchXL(wkbResults, quoteID);
            int row = 2;
            foreach (var match in pnMatch)
            {
                string keyString = match.Key.ToString();
                InsertText(wks, row, 1, keyString);
                (string soNumber, string partNumber) values = match.Value;
                InsertText(wks, row, 3, values.soNumber);
                InsertText(wks, row, 2, values.partNumber);

                row++;
            }
        }
        /// <summary>
        /// Uses ExcelOps to place text in cell
        /// </summary>
        /// <param name="wks"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="text"></param>
        private static void InsertText(Worksheet wks, int row, int col, string text)
        {
            ExcelOps.PlaceTextInWorksheet(wks, row, col, text);
        }
        /// <summary>
        /// Create worksheet to hold match data
        /// </summary>
        /// <param name="xlApp"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static Worksheet MakeMatchXL(Workbook wkbResults, string name)
        {
            //Workbook wkb = xlApp.Workbooks.Add();
            Worksheet wks = wkbResults.ActiveSheet;
            InsertText(wks, 1, 1, "Shipment Row");
            InsertText(wks, 1, 2, "Part Number");
            InsertText(wks, 1, 3, "IC Invoice");
            wks.Columns[1].ColumnWidth = 20;
            wks.Columns[2].ColumnWidth = 20;
            wks.Columns[3].ColumnWidth = 20;
            wks.Name = name;
            return wks;
        }
        
        /// <summary>
        /// Creates a comma sepatated string list of PID's
        /// </summary>
        /// <param name="msoRequests"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Calls ExcelOps sendMessage to place string into message bar
        /// </summary>
        /// <param name="message"></param>
        public static void sendMessage(string message)
        {
            ReportOps.sendMessage(message);
        }

        /// <summary>
        /// Shortcut to ExcelOps code to return last usde row in spreadsheet
        /// </summary>
        /// <param name="wks"></param>
        /// <returns></returns>
        private static int FindLastSpreadsheetRow(Worksheet  wks)
        {
            return ExcelOps.FindLastSpreadsheetRow(wks);
        }

        /// <summary>
        /// Shortcut to ExcelOps findHeaderRow, returning row number of first occurence of searchTerm
        /// </summary>
        /// <param name="range"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        private static int FindHeaderRow(Range range, string searchTerm)
        {
            return ExcelOps.FindHeaderRow(range, searchTerm);
        }

        /// <summary>
        /// Returns column number of first occurence of searchTerm in range
        /// </summary>
        /// <param name="wks"></param>
        /// <param name="searchTerm"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        private static int GetColumn(Worksheet wks, string searchTerm, Range range)
        {
            return ExcelOps.GetColumn(wks, searchTerm, range);
        }

    }
}
