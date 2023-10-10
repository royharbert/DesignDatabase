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
using System.Drawing;
using System.Windows.Forms;

namespace DesignDB_Library.Operations
{
    public static class ShipmentOps
    {
        const string BOMFilePath = "\\\\usca5pdbatdgs01\\Databases\\AttachmentsDesign";
        static string quoteCity = "";
        static string quoteState = "";
        
        public static void ShipmentToBOMCompare(string fileName, DateTime startDate, DateTime endDate, List<MSO_Model> MSOs)
        {
            //Get shipment spreadsheet and locate data columns
            Excel.Application xlApp = ExcelOps.makeExcelApp();
            sendMessage("Opening Shipments File");
            Excel.Workbook wkb = xlApp.Workbooks.Open(fileName);
            Excel.Worksheet wks = xlApp.ActiveSheet;
            Range searchRange= wks.get_Range("A1:Z26");
            xlApp.Visible = true;
            int lastRow = FindLastSpreadsheetRow(wks);

            int QuanCol = 0;
            int SODateCol = 0;
            int SOCustCol = 0;
            int PartNumberCol = 0;
            int DescCol = 0;
            int CityCol = 0;
            int StateCol = 0;
            int SOCol = 0;
            QuanCol = GetColumn(wks, "Shipped Sales Quantity", searchRange);
            SODateCol = GetColumn(wks, "SO Item Create Date", searchRange);
            SOCustCol = GetColumn(wks, "Sold To Cust Name", searchRange);
            PartNumberCol = GetColumn(wks, "Material ID", searchRange);
            DescCol = GetColumn(wks, "Material Description", searchRange);
            CityCol = GetColumn(wks, "Ship To Cust City Name", searchRange);
            StateCol = GetColumn(wks, "Ship To Cust State Code", searchRange);
            SOCol = GetColumn(wks, "Sales Ord Id", searchRange);
            
            if(QuanCol == 0 || SODateCol == 0 || SOCustCol == 0 || PartNumberCol == 0 || DescCol == 0 ||
                CityCol == 0 || StateCol == 0 || SOCol == 0)
            {
                MessageBox.Show("File not in proper format.\nPlease close and retry.");
                return;
            }
            

            //Load SS into List and order by Part Number
            sendMessage("Loading " + lastRow.ToString() + " Spreadsheet Lines into List");
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
                double q = 0;
                string sQuan = wks.Cells[i, QuanCol].Value.ToString();
                double.TryParse(sQuan, out q);
                shipment.Quantity = q;
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

            //Iterate list by selected MOS's
            foreach (var mso in MSOs)
            {
                //Filter list for current MSO, create msoShipmentList for results
                //use .Contains for instances where mso may have different versions of name (i.e. Cable One & Cable One Inc.)
                foreach (var shipment in shipmentList)
                {
                    if (shipment.SOCust.Contains(mso.MSO));
                    {
                        msoShipmentList.Add(shipment);
                    }
                }
                
                //Filter request list by MSO into msoRequests
                msoRequests = requestList.Where(x => x.MSO == mso.MSO).ToList();

                //Create a comma separated string of all PIDs
                //Used to retrieve BOM file names
                string PIDs = GetPIDsFromRequests(msoRequests);
                List<BOMLineModel> bomFiles = GlobalConfig.Connection.getBOMList(PIDs);

                //Compare BOMs to shipment list
                //Place results into spreadsheet
                ProcessBOMs(xlApp, wkb, msoShipmentList, msoRequests, lastRow, bomFiles);
            }

            //release Excel instance
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
        private static void ProcessBOMs(Excel.Application xlApp, Excel.Workbook wkb, List<ShipmentLineModel> shipments, 
            List<RequestModel> msoRequests, int lastRow, List<BOMLineModel> BOMLineList, List<BOM_Model> BOMList= null)
        {
            //Scope wkbResults
            Excel.Workbook wkbResults = null;

            //Iterate through list of BOMs
            foreach (var BOM in BOMLineList) 
            {
                //Open BOM file and get active worksheet
                string bomFile = BOMFilePath + "\\" + BOM.PID + "\\" + BOM.DisplayText;
                sendMessage("Opening " + BOM.DisplayText);
                wkb = xlApp.Workbooks.Open(bomFile);
                Excel.Worksheet wks = xlApp.ActiveSheet;

                //Load this BOM's lines into List
                List<BOM_Model> BOMLines = LoadBOMtoList(wkb, lastRow, BOM.PID);                

                //Load BOMLineModel data into BOM_Model
                //Adds BOM file name to BOM_Model
                foreach (var line in BOMLines)
                {
                    BOMLineModel lineModel = BOMLineList.Where(x => x.PID == line.Quote).FirstOrDefault();
                    line.DisplayText = lineModel.DisplayText;
                }

                //Create new workbook in xlApp
                //Compare BOM to shipment
                if (wkbResults == null)
                {
                    wkbResults = xlApp.Workbooks.Add(); 
                }
                CompareBOMtoShipmentsl(xlApp, wkbResults, wkb, lastRow, shipments, msoRequests, BOM);
            }
        }

        /// <summary>
        /// Places BOM items into List<BOM_Model></BOM_Model>
        /// </summary>
        /// <param name="wks"></param>
        /// <param name="lastRow"></param>
        /// <param name="PID"></param>
        /// <returns></returns>
        private static List<BOM_Model> LoadBOMtoList(Excel.Workbook wkb, int lastRow, string PID)
        {
            Excel.Worksheet wks = wkb.ActiveSheet;
            List<BOM_Model> models = new List<BOM_Model>();

            //Locate necessary columns in BOM
            Range searchRange = wks.get_Range("A1:Z26");
            int lastBOMRow = FindLastSpreadsheetRow(wks);
            int headerRow = FindHeaderRow(searchRange, "Quantity");
            int quanCol = GetColumn(wks, "Quantity", searchRange);
            int descCol = GetColumn(wks, "Description", searchRange);
            int modelCol = GetColumn(wks, "Model Number", searchRange);
            int row = headerRow + 1;

            //Add BOM lines to list of BOM_Models
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

            //Initialize procedure wide variables
            int distinctMatches = 0;
            double pctMatch = 0;

            //Load BOM lines into list
            //Close BOM worksheet
            Excel.Worksheet wks = wkb.ActiveSheet;
            List<BOM_Model> bomItems = LoadBOMtoList(wkb, lastRow, BOM.PID);
            wkb.Close();
            
            //Initialize list of BOM-Shipment matches and list of non-matches
            string msg = "Analyzing Quote  " + BOM.PID;
            sendMessage(msg);
            List<ShipmentLineModel> bomMatches = new List<ShipmentLineModel>();
            List<BOM_Model> bomNonMatches = new List<BOM_Model>();
            string quoteID = "";

            //Iterate BOM lines and populate match and non-match lists
            foreach (var item in bomItems)
            {
                quoteID = item.Quote;
                sendMessage(msg + "     " + item.ModelNumber);

                //Initialize and populate list of matching part number BOM lines to shipment lines
                List<ShipmentLineModel> matches = shipments.Where(x => x.PartNumber == item.ModelNumber).ToList();
                
                //Filter request list for current PID
                RequestModel request = msoRequests.Where(x => x.ProjectID == item.Quote).FirstOrDefault();
                if (matches.Count > 0)
                {
                    //Increment match count (used for pct matched)
                    distinctMatches++;

                    //Populate shipment line fields with data from quote and BOM
                    foreach (var match in matches)
                    {
                        match.QuoteCity = request.City;
                        match.QuoteState = request.ST;
                        match.QuoteDateCompleted = request.DateCompleted.ToShortDateString();
                        BOM_Model bom = bomItems.Where(x => x.ModelNumber == item.ModelNumber).FirstOrDefault();
                        match.BOM_Quantity = bom.Quantity.ToString();
                        bomMatches.Add(match);
                    }
                }
                //If current part number not a match to shipments, add to non-matches
                else
                {
                    bomNonMatches.Add(item);
                }

            }

            //Sort bomMatches by part number
            bomMatches = bomMatches.OrderBy(x => x.PartNumber).ToList();

            //Add new sheet to workbook and place PID on sheet tab
            AddSheetToWorkbook(wkbResults);

            //Examine BOM for:
            //  Percent line items matching shipment (by part number)
            //  Quote city/state matching shipment city/state
            //  Sales order created after quote date completed
            //  Differences in ordered vs BOM quantities
            bomMatches = AnalyzeBOM(bomMatches);

            //Create new worksheet to display this BOM's results
            //Make header at row 3 - pct match will be on row 1
            int row = 3;
            (Excel.Workbook wkb, int row) rtn = MakeMatchXL(wkbResults, xlApp, row, quoteID);
            wkbResults = rtn.wkb;
            //Populate worksheet with results
            row = rtn.row;
            int startRow = row;
            Excel.Worksheet wksResults = wkbResults.ActiveSheet;

            //Calculate and display pct matches
            int pctRow = 1;
            pctMatch = distinctMatches * 100 / bomItems.Count;
            wksResults.Cells[pctRow, 2].Value = "Percent of BOM Lines Matching Shipments";
            wksResults.Cells[pctRow, 3].Value = Math.Round(pctMatch).ToString() + "%";

            //Format pct match area
            wksResults.Rows[pctRow].WrapText = true;
            wksResults.Cells[1, 1].EntireRow.Font.Bold = true;
            //wksResults.Rows[1].WrapText = true;

            //Add 5 to pctRow to allow blank line before header and 2 rows for header and one to advance beyond header
            row = pctRow + 5;
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

            //Find last row of match data
            int bottomRowMatches = FindLastSpreadsheetRow(wksResults); 

            //InsertText non matches;
            row = row + 3;
            Range range = (Excel.Range)wksResults.Range[wksResults.Cells[1, 1], wksResults.Cells[bottomRowMatches, 14]];
            CenterTextInRange(wkbResults, range);

            //Begin display of non-matches
            row = MakeHeader(wksResults, row, 14, "Non-matching BOM Line Items");

            //Insert column headers
            InsertText(wksResults, row, 2, "Part Number");
            InsertText(wksResults, row, 3, "Quantity");
            InsertText(wksResults, row, 4, "Description");
            range = (Excel.Range)wksResults.Range[wksResults.Cells[row, 2], wksResults.Cells[row, 3]];
            wksResults.Rows[row].EntireRow.Font.Bold = true;
            range.HorizontalAlignment = XlHAlign.xlHAlignCenter;

            row++;
            int startNonMatch = row;
            wksResults.Range[wksResults.Cells[startNonMatch - 1, 4], wksResults.Cells[startNonMatch - 1, 14]].Merge();
            wksResults.Range[wksResults.Cells[startNonMatch - 1, 4], wksResults.Cells[startNonMatch - 1, 14]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            foreach (var non_match in bomNonMatches)
            {
                InsertText(wksResults, row, 3, non_match.Quantity.ToString());
                InsertText(wksResults, row, 2, non_match.ModelNumber.ToString());
                InsertText(wksResults, row, 4, non_match.Description.ToString());

                row++;
            }
            for (int i = startNonMatch - 1; i < row; i++)
            {
                wksResults.Cells[i, 4].WrapText = true;
                wksResults.Rows[i].RowHeight = 28.5;
                wksResults.Range[wksResults.Cells[i + 1, 4], wksResults.Cells[i + 1, 14]].Merge();
            }
            wksResults.Range[wksResults.Cells[startNonMatch, 2], wksResults.Cells[row - 1, 3]].
                HorizontalAlignment = XlHAlign.xlHAlignCenter;

            //conditional formatting for analysis columns
            ConditionalFormatTrueFalse(startRow, bottomRowMatches + 1, 11, wkbResults);
            ConditionalFormatTrueFalse(startRow, bottomRowMatches + 1, 14, wkbResults);
            ConditionalFormatNumber(startRow, bottomRowMatches + 1, 5, wkbResults);
        }

        /// <summary>
        /// Conditional format for true/false columns. True is green, false is red
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="stopRow"></param>
        /// <param name="col"></param>
        /// <param name="wkb"></param>
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

        /// <summary>
        /// Conditional format for numeric results. Negative is red, positive is amber, 0 is green
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="stopRow"></param>
        /// <param name="col"></param>
        /// <param name="wkb"></param>
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

        /// <summary>
        /// Analyze results
        ///     Compare:
        ///         Date BOM completed to SO creation
        ///         Quantity ordered vs BOM Quantity
        ///         Request city/state vs shiped city/state
        ///         % part number matches
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static List<ShipmentLineModel> AnalyzeBOM(List<ShipmentLineModel> list)
        {
            foreach (var line in list)
            {
                //Date comparison
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

                //Shipment city/state vs request city/state
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

                //Compare quan ordered vs BOM quan
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

        private static int MakeHeader(Excel.Worksheet wks, int startRow, int stopCol, string title)
        {
            wks.Cells[startRow, 1].Value = title;
            wks.Cells[startRow, 1].Font.Size = 20;
            wks.Cells[startRow, 1].Font.Bold = true;
            var headerRow = wks.Cells[startRow + 1, 1];
            //var titleRow = wks.Range[wks.Cells[row, 1]], [wks.Cells[row, rightmostCol]];
            headerRow.RowHeight = 45;
            Range range = wks.Range[wks.Cells[startRow, 1], wks.Cells[startRow + 1, stopCol]];
            Range titleRow = wks.Range[wks.Cells[startRow, 1], wks.Cells[startRow, stopCol]];
            titleRow.Cells.Merge();
            range.Cells.HorizontalAlignment = HorizontalAlignment.Center;
            
            range.Font.Bold = true;
            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            range.Interior.Color = ColorTranslator.ToOle(System.Drawing.Color.LightSkyBlue);
            range.WrapText = true;

            return startRow + 2;
        }

        /// <summary>
        /// Create worksheet to hold match data
        /// </summary>
        /// <param name="xlApp"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static (Excel.Workbook wkb, int row) MakeMatchXL(Excel.Workbook wkbResults, Excel.Application xlApp, int startRow, string name)
        {
            (Excel.Workbook wkb, int row) rtn;
            Excel.Worksheet wks = wkbResults.ActiveSheet;
            int row = MakeHeader(wks, startRow, 14, "BOM Lines with Matches in Shipments");
            InsertText(wks, row, 1, "Shipment Row");
            InsertText(wks, row, 2, "Part Number");
            InsertText(wks, row, 3, "BOM Quantity");
            InsertText(wks, row, 4, "Shipment Quantity");
            InsertText(wks, row, 5, "Quan Shipped - Quan BOM");
            InsertText(wks, row, 6, "Sales Order ID");
            InsertText(wks, row, 7, "Shipment City");
            InsertText(wks, row, 8, "Shipment State");
            InsertText(wks, row, 9, "Quote City");
            InsertText(wks, row, 10, "Quote State");
            InsertText(wks, row, 11, "City/State Match");
            InsertText(wks, row, 12, "SO Date");
            InsertText(wks, row, 13, "Date Quote Completed");
            InsertText(wks, row, 14, "SO Newer Tham BOM");

            FormatXLSheet(wkbResults, name, row);
            rtn.wkb = wkbResults;
            rtn.row = row + 1;
            return rtn;
        }

        /// <summary>
        /// Sets column widths on spreadsheet and sets name
        /// </summary>
        /// <param name="wkbResults"></param>
        /// <param name="name"></param>
        private static void FormatXLSheet(Excel.Workbook wkbResults, string name, int row)
        {
            Excel.Worksheet wks = wkbResults.ActiveSheet;
            //               A   B   C   D   E   F   G   H   I   J   K   L   M   N
            int[] widths = { 10, 30, 12, 12, 12, 20, 25, 12, 25, 15, 12,  15, 15, 12};

            for (int i = 1; i <= widths.Length; i++)
            {
                wks.Columns[i].ColumnWidth = widths[i - 1];
            }
            Excel.Range range = (Excel.Range)wks.Range[wks.Cells[row, 1], wks.Cells[row, 14]];

            //Format column headers
            wks.Rows[row].WrapText = true;
            wks.Rows[row].EntireRow.Font.Bold = true;

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
            sendMessage(msoRequests.Count.ToString() + "BOMs to process");
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
            int col = 0;
            try
            {
                col = ExcelOps.GetColumn(wks, searchTerm, range);
            }
            catch (Exception)
            {

            }

            return col;
        }

    }
}
