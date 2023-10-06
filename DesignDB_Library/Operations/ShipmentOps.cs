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
using System.Xml.Linq;
using System.Windows.Forms.VisualStyles;
using Microsoft.Office.Tools.Excel;
using System.Configuration;

namespace DesignDB_Library.Operations
{
    public static class ShipmentOps
    {
        const string BOMFilePath = "\\\\usca5pdbatdgs01\\Databases\\AttachmentsDesign";
        static string quoteCity = "";
        static string quoteState = "";
        
        public static void ShipmentToBOMCompare(string fileName, DateTime startDate, DateTime endDate, List<MSO_Model> MSOs)
        {
            //Get shipment spreadsheet
            Application xlApp = ExcelOps.makeExcelApp();
            sendMessage("Opening Shipments File");
            Excel.Workbook wkb = xlApp.Workbooks.Open(fileName);
            Excel.Worksheet wks = xlApp.ActiveSheet;
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
            int row = 2;
            for (int i = row; i < lastRow; i++)
            {
                ShipmentLineModel shipment = new ShipmentLineModel();
                shipment.Desc = wks.Cells[i, DescCol].Value;
                shipment.PartNumber = wks.Cells[i, PartNumberCol].Value.ToString();
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
                ProcessBOM(xlApp, wkb, msoShipmentList, msoRequests, lastRow, bomFiles);
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
        private static void ProcessBOM(Excel.Application xlApp, Excel.Workbook wkb, List<ShipmentLineModel> shipments, 
            List<RequestModel> msoRequests, int lastRow, List<BOMLineModel> BOMLineList, List<BOM_Model> BOMList= null)
        {
            Excel.Workbook wkbResults = null;
            foreach (var BOM in BOMLineList) 
            {
                string bomFile = BOMFilePath + "\\" + BOM.PID + "\\" + BOM.DisplayText;
                sendMessage("Opening " + BOM.DisplayText);
                wkb = xlApp.Workbooks.Open(bomFile);
                Excel.Worksheet wks = xlApp.ActiveSheet;

                //Load this BOM into List
                List<BOM_Model> BOMLines = LoadBOMtoList(wkb, lastRow, BOM.PID);                

                //Load BOMLineModel data into BOM_Model
                foreach (var line in BOMLines)
                {
                    BOMLineModel lineModel = BOMLineList.Where(x => x.PID == line.Quote).FirstOrDefault();
                    line.DisplayText = lineModel.DisplayText;
                }

                //Compare BOM to shipment
                if (wkbResults == null)
                {
                    wkbResults = xlApp.Workbooks.Add(); 
                }
                CompareBOMtoShipmentsl(xlApp, wkbResults, wkb, lastRow, shipments, msoRequests, BOM);
            }
        }

        /// <summary>
        /// Places BIM items into List<BOM_Model></BOM_Model>
        /// </summary>
        /// <param name="wks"></param>
        /// <param name="lastRow"></param>
        /// <param name="PID"></param>
        /// <returns></returns>
        private static List<BOM_Model> LoadBOMtoList(Excel.Workbook wkb, int lastRow, string PID)
        {
            Excel.Worksheet wks = wkb.ActiveSheet;
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
                if (wks.Cells[i, quanCol].Value != null)
                {
                    model.Quantity = wks.Cells[i, quanCol].Value; 
                }
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
        private static void CompareBOMtoShipmentsl(Excel.Application xlApp, Excel.Workbook wkbResults, Excel.Workbook wkb, int lastRow, 
            List<ShipmentLineModel> shipments, List<RequestModel> msoRequests, BOMLineModel BOM)
        {
            int distinctMatches = 0;
            double pctMatch = 0;
            Excel.Worksheet wks = wkb.ActiveSheet;
            List<BOM_Model> bomItems = LoadBOMtoList(wkb, lastRow, BOM.PID);
            wkb.Close();
           
            string msg = "Analyzing Quote  " + BOM.PID;
            sendMessage(msg);
            List<ShipmentLineModel> bomMatches = new List<ShipmentLineModel>();
            string quoteID = "";
            foreach (var item in bomItems)
            {
                quoteID = item.Quote;
                sendMessage(msg + "     " + item.ModelNumber);
                List<ShipmentLineModel> matches = shipments.Where(x => x.PartNumber == item.ModelNumber).ToList();
                if (matches.Count > 0)
                {
                    distinctMatches++;
                    foreach (var match in matches)
                    {
                        RequestModel request = msoRequests.Where(x => x.ProjectID == item.Quote).FirstOrDefault();
                        match.QuoteCity = request.City;
                        match.QuoteState = request.ST;
                        match.QuoteDateCompleted = request.DateCompleted.ToShortDateString();
                        BOM_Model bom = bomItems.Where(x => x.ModelNumber == item.ModelNumber).FirstOrDefault();

                        match.BOM_Quantity = bom.Quantity.ToString();
                        bomMatches.Add(match);
                    }
                    bomMatches = bomMatches.OrderBy(x => x.PartNumber).ToList();
                }
            }
           
            AddSheetToWorkbook(wkbResults);
            bomMatches = AnalyzeBOM(bomMatches);
            wkbResults = MakeMatchXL(wkbResults, xlApp, quoteID);
            int row = 2;
            Excel.Worksheet wksResults = wkbResults.ActiveSheet;
            foreach (var match in bomMatches)
            { 
                InsertText(wksResults, row, 1, match.ExcelRow.ToString());
                InsertText(wksResults, row, 2, match.PartNumber);
                InsertText(wksResults, row, 3, match.BOM_Quantity);
                InsertText(wksResults, row, 4, match.Quantity.ToString());
                InsertText(wksResults, row, 5, match.QShippedMinusQBOM.ToString());
                InsertText(wksResults, row, 6, match.SONumber);
                InsertText(wksResults, row, 7, match.City);
                InsertText(wksResults, row, 8, match.State);
                InsertText(wksResults, row, 9, match.QuoteCity);
                InsertText(wksResults, row, 10, match.QuoteState);
                InsertText(wksResults, row, 11, match.CityStateMatch.ToString());
                InsertText(wksResults, row, 12, match.SODate.ToShortDateString());
                InsertText(wksResults, row, 13, match.QuoteDateCompleted);
                InsertText(wksResults, row, 14, match.SONewerThanBOM.ToString());

                row++;
            }
            int bottomRow = FindLastSpreadsheetRow(wksResults); 
            pctMatch = distinctMatches * 100/bomItems.Count;
            wksResults.Cells[bottomRow + 2, 2].Value = "Percent of BOM Lines Matching Shipments";
            wksResults.Cells[bottomRow + 2, 3].Value = Math.Round(pctMatch);
            wksResults.Rows[bottomRow + 2].WrapText = true;
            Range range = (Excel.Range)wksResults.Range[wksResults.Cells[1, 1], wksResults.Cells[bottomRow, 14]];
            CenterTextInRange(wkbResults, range);
            wksResults.Cells[1,1].EntireRow.Font.Bold = true;
            wksResults.Rows[1].WrapText = true;

            //conditional formatting
            ConditionalFormatTrueFalse(2, bottomRow + 1, 11, wkbResults);
            ConditionalFormatTrueFalse(2, bottomRow + 1, 14, wkbResults);
            ConditionalFormatNumber(2, bottomRow + 1, 5, wkbResults);
        }

        private static void ConditionalFormatTrueFalse(int startRow, int stopRow, int col, Excel.Workbook wkb)
        {
            Excel.Worksheet wks = wkb.ActiveSheet;
            for (int i = startRow; i < stopRow; i++)
            {
                if (wks.Cells[i, col].Value.ToString().ToUpper() == "TRUE")
                {
                    wks.Cells[i, col].Font.Color = XlRgbColor.rgbGreen;
                }
                else
                {
                    wks.Cells[i, col].Font.Color = XlRgbColor.rgbRed;
                }
            }
            
        }

        private static void ConditionalFormatNumber(int startRow, int stopRow, int col, Excel.Workbook wkb)
        {
            Excel.Worksheet wks = wkb.ActiveSheet;
            for (int i = startRow; i < stopRow; i++)
            {
                double number = (wks.Cells[i, col].Value);
                if (number < 0)
                {
                    wks.Cells[i, col].Font.Color = XlRgbColor.rgbRed;
                }
                else if (number > 1)
                {
                    wks.Cells[i, col].Font.Color = XlRgbColor.rgbDarkGoldenrod;
                }
                else
                {
                    wks.Cells[i, col].Font.Color = XlRgbColor.rgbGreen;
                }                
            }

        }

        /// <summary>
        /// Adds a sheet to workbook and returns workbook with new sheet as activesheet
        /// </summary>
        /// <param name="wkb"></param>
        /// <returns></returns>
        private static Excel.Workbook AddSheetToWorkbook(Excel.Workbook wkb)
        {
            int count = wkb.Sheets.Count;            
            wkb.Sheets.Add();             
            count = wkb.Sheets.Count;
            return wkb;
        }

        private static List<ShipmentLineModel> AnalyzeBOM(List<ShipmentLineModel> list)
        {
            foreach (var line in list)
            {
                DateTime bomDate = DateTime.Parse(line.QuoteDateCompleted.ToString());
                DateTime soDate = DateTime.Parse(line.SODate.ToString());
                int dateCompre = DateTime.Compare(bomDate, soDate);
                if (dateCompre < 0)
                {
                    line.SONewerThanBOM = true;
                }
                else
                {
                    line.SONewerThanBOM = false;
                }
                string stateAbbreviation = GlobalConfig.Connection.GetStateAbbreviation(line.QuoteState);
                string soCityState = line.City + line.State.ToString();
                string quoteCityState = line.QuoteCity + stateAbbreviation.ToString();
                if (quoteCityState == soCityState)
                {
                    line.CityStateMatch = true;                        
                }
                else 
                {
                    line.CityStateMatch = false;
                }
                double diff = 0;
                double.TryParse(line.BOM_Quantity, out diff );
                line.QShippedMinusQBOM = line.Quantity - diff;
            }
            return list;
        }

        /// <summary>
        /// Uses ExcelOps to place text in cell
        /// </summary>
        /// <param name="wks"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="text"></param>
        private static void InsertText(Excel.Worksheet wks, int row, int col, string text)
        {
            ExcelOps.PlaceTextInWorksheet(wks, row, col, text);
        }

        /// <summary>
        /// Create worksheet to hold match data
        /// </summary>
        /// <param name="xlApp"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static Excel.Workbook MakeMatchXL(Excel.Workbook wkbResults, Excel.Application xlApp, string name)
        {
            Excel.Worksheet wks = wkbResults.ActiveSheet;
            InsertText(wks, 1, 1, "Shipment Row");
            InsertText(wks, 1, 2, "Part Number");
            InsertText(wks, 1, 3, "BOM Quantity");
            InsertText(wks, 1, 4, "Shipment Quantity");
            InsertText(wks, 1, 5, "Quan Shipped - Quan BOM");
            InsertText(wks, 1, 6, "IC Invoice");
            InsertText(wks, 1, 7, "Shipment City");
            InsertText(wks, 1, 8, "Shipment State");
            InsertText(wks, 1, 9, "Quote City");
            InsertText(wks, 1, 10, "Quote State");
            InsertText(wks, 1, 11, "City/State Match");
            InsertText(wks, 1, 12, "SO Date");
            InsertText(wks, 1, 13, "Date Quote Completed");
            InsertText(wks, 1, 14, "SO Newer Tham BOM");

            FormatXLSheet(wkbResults, name);
            return wkbResults;
        }

        /// <summary>
        /// Sets column widths on spreadsheet and sets name
        /// </summary>
        /// <param name="wkbResults"></param>
        /// <param name="name"></param>
        private static void FormatXLSheet(Excel.Workbook wkbResults, string name)
        {
            Excel.Worksheet wks = wkbResults.ActiveSheet;
            //               A   B   C   D   E   F   G   H   I   J   K   L   M   N
            int[] widths = { 8, 30, 12, 12, 12, 20, 25, 12, 25, 15, 12,  15, 15, 12};

            for (int i = 1; i <= widths.Length; i++)
            {
                wks.Columns[i].ColumnWidth = widths[i - 1];
            }

            wks.Name = name;
        }

        private static void CenterTextInRange(Excel.Workbook wkbResults, Range range)
        {
            range.Cells.HorizontalAlignment = XlHAlign.xlHAlignCenter;
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
        private static int FindLastSpreadsheetRow(Excel.Worksheet  wks)
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
        private static int GetColumn(Excel.Worksheet wks, string searchTerm, Range range)
        {
            return ExcelOps.GetColumn(wks, searchTerm, range);
        }

    }
}
