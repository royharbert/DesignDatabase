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
using NuGet;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DesignDB_Library.Operations
{
    /// <summary>
    /// Event args to update progress bar on main menu
    /// </summary>
    public class ProgressStripEventArgs :   EventArgs
    {
        public int MaxCount;
        public int CurrentCount;
        public bool IsVisible;
    }

    /// <summary>
    /// Event args to update BOM processing progress
    /// </summary>
    public class BOMProgressEventArgs    :   EventArgs
    {
        public int bomCount;
        public int currentBOMCount;
        public bool IsVisible;
    }

    public static class ShipmentOps
    {
        //Class wide scope variables
        public static event EventHandler<ProgressStripEventArgs> UpdateProgressStrip;
        public static event EventHandler<BOMProgressEventArgs> BOMProgressEvent;
        const string BOMFilePath = "\\\\usca5pdbatdgs01\\Databases\\AttachmentsDesign";
        static string quoteCity = "";
        static string quoteState = "";

        /*
         * Program flow
         *  Create instance of Excel
         *  Open SHIPMENTS file
         *  Establish search range to look for specific column headers      
         *  Find last row in spreadsheet FindLastSpreadsheetRow(wks); 
         *  Create instance of BOMProgressEventArgs for updating main menu progress bar
         *  Get column numbers for necessary columns
         *  Load xlsm file into list
         *  Establish 5% update points for progress bar
         *  If update threshold reached, update progress bar then increment threshold
         *      Loop from to to lastRow of SHIPMENTS
         *      Create new ShipmentLineModel
         *      Populate model with data from SHIPMENTS
         *      Add model to list
         *      Keep progress bar updeate on each threshold
         *      After finishing loop, make progress bar invisible, close SHIPMENTS
         *      Sort shipments list by part number
         *  Use start/stop dates from date/time picker to retrieve all requests in date range
         *  Use MSO list from date/time picker to filter shipments by MSO
         *  Use same list to filter requests by selected MSO's
         *  Set BOMArgs.Count to msoRequests.Count
         *  Create comma separated string of all PIDs
         *  Use stored procedure to get list of all BOM file names
         *  Begin processing all BOM files (ProcessBOMs)
         *      Initialize BOMArgs.currentBOMcount to 0
         *      Start processing individual BOMs
         *          Increment bomArgs.currentCount
         *          Insert BOM file name into path string
         *          Retrieve BOM
         *          Load BOM line items into list (LoadBOMtoList)
         *              Get last used row of BOM
         *              Find header row of BOM
         *              Find column numbers of items
         *              Loop from header row + 1 to lastRow adding each row to list of BOM model
         *              Return list
         *          Compare BOM items to shipment list (CompareBOMtoShipments)
         *              Initialize distinctMatches and pctMatch to 0
         *              Load BOM items to list ********* redundant?
         *              Close BOM file
         *              Initialize list of ShipmentLineModel and BOM_Model (for non-matched items)
         *              Loop through each item in BOM
         *                  Initialize ShipmentLineModel list for matches and filter for match between part numbers in shipment and BOM
         *                  Filter list of requests for matches between BOM quote ID and PID
         *                  If match count > 0 increment distinct matches
         *                      Populate ShipmentLineModel quote related fields with quote data
         *                      Add populated ShipmentLineModel to bomMatches list
         *                  else
         *                      add item to non-matches list
         *                  Sort matches by part number
         *                  Make new worksheet
         *                  Create tuple to return city and state match count and ShipmentLineModel
         *                  Analyze BOM stats (AnalyzeBOM)
         *                      Initialize city and state match counts to 0
         *                      Do date comparison with SO and BOM creation
         *                      Place result in SLM
         *                      Get abbreviation for state from request
         *                      Compare uppercase city and state in quote and shipment
         *                      Place result in SLM
         *                      Compare BOM and Quote quantities
         *                      Place difference in SLM
         *                      Place city and state matches in rtn tuple
         *                      Add SLM list to tuple and return
         *                  Make new worksheet to display results (MakeMatchXL)
         *                      Make section header for match results
         *                      Insert column names
         *                      Format column names row (FormatXLSheet)
         *                  Insert pct match data
         *                      Place match data in sheet
         *                      Place non-match data in sheet
         *                      
         *                      
         *              
         *              
         *          
         */
        /// <summary>
        /// Primary program flow
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="MSOs"></param>
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
            BOMProgressEventArgs BOMArgs = new BOMProgressEventArgs();

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
            ProgressStripEventArgs psArgs = new ProgressStripEventArgs();
            psArgs.MaxCount = lastRow;
            int updateThreshold = Convert.ToInt32(lastRow * 0.05);
            int updateIncrement = updateThreshold;
            
            int row = 2;
            for (int i = row; i < lastRow; i++)
            {
                if(i == updateThreshold)
                {
                    psArgs.CurrentCount = i;
                    psArgs.IsVisible = true;
                    UpdateProgressStrip?.Invoke("ShipmentOps", psArgs);
                    updateThreshold = updateThreshold + updateIncrement;
                }
                ShipmentLineModel shipment = new ShipmentLineModel();
                shipment.Desc = wks.Cells[i, DescCol].Value.ToString();
                shipment.PartNumber = wks.Cells[i, PartNumberCol].Value.ToString();
                shipment.City = wks.Cells[i, CityCol].Value.ToString();
                shipment.SOCust = wks.Cells[i, SOCustCol].Value.ToString();
                shipment.State = wks.Cells[i, StateCol].Value.ToString();
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
            psArgs.IsVisible = false;
            UpdateProgressStrip?.Invoke("ShipmentOps", psArgs);
            BOMArgs.IsVisible = false;
            BOMProgressEvent?.Invoke("ShipmentOp", BOMArgs);
            wkb.Close();
            shipmentList = shipmentList.OrderBy(x => x.PartNumber).ToList();

            //Get Date Range
            sendMessage("Retrieving and Filtering Quotes in Date Range");
            List<RequestModel> requestList = GlobalConfig.Connection.DateRangeSearch_Unfiltered(startDate, endDate);
            List<RequestModel> msoRequests = new List<RequestModel>();
            List<ShipmentLineModel> msoShipmentList = new List<ShipmentLineModel>();

            //Iterate list by selected MSO's
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
                sendMessage(bomFiles.Count.ToString() + "BOMs to process");
                BOMArgs.bomCount = bomFiles.Count;
                //Compare BOMs to shipment list
                //Place results into spreadsheet
                ProcessBOMs(xlApp, wkb, msoShipmentList, msoRequests, lastRow, bomFiles, BOMArgs);
                BOMProgressEventArgs bomArgs = new BOMProgressEventArgs();
                bomArgs.IsVisible = false;
                BOMProgressEvent?.Invoke("ShipmentOps", bomArgs);
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
            List<RequestModel> msoRequests, int lastRow, List<BOMLineModel> BOMLineList, BOMProgressEventArgs 
                bomArgs, List<BOM_Model> BOMList= null)
        {
            //Scope wkbResults
            Excel.Workbook wkbResults = null;
            
            bomArgs.currentBOMCount = 0;
            //Iterate through list of BOMs
            List<BOMSummaryModel> summaryList = new List<BOMSummaryModel>();
            foreach (var BOM in BOMLineList) 
            {
                bomArgs.currentBOMCount++;
                bomArgs.IsVisible = true;
                BOMProgressEvent?.Invoke("ShipmentOps", bomArgs);
                //Open BOM file and get active worksheet
                string bomFile = BOMFilePath + "\\" + BOM.PID + "\\" + BOM.DisplayText;
                sendMessage("Opening " + BOM.DisplayText);
                //TODO try catch
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
                BOMSummaryModel summary = CompareBOMtoShipmentsl(xlApp, wkbResults, wkb, lastRow, shipments, msoRequests, BOM);
                summaryList.Add(summary);
                ApplyFilters(wkbResults);
            }
            sendMessage("");
            CreateSummarySheet(wkbResults, summaryList);
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
                if (wks.Cells[i, descCol].Value != null && wks.Cells[i, modelCol].Value != null 
                    && wks.Cells[i, quanCol].Value != null)
                {
                    BOM_Model model = new BOM_Model();
                    model.Description = wks.Cells[i, descCol].Value.ToString();
                    model.Quote = PID;
                    model.ModelNumber = wks.Cells[i, modelCol].Value.ToString();
                    model.Quantity = wks.Cells[i, quanCol].Value;
                    
                    models.Add(model); 
                }
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
        private static BOMSummaryModel CompareBOMtoShipmentsl(Excel.Application xlApp, Excel.Workbook wkbResults, Excel.Workbook wkb, int lastRow, 
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
            string msg = "Analyzing Quote  " + BOM.DisplayText;
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
                if (item.ModelNumber != null)
                {
                    List<ShipmentLineModel> matches = shipments.Where(x => x.PartNumber.Contains(item.ModelNumber)).ToList();

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
            List<ShipmentLineModel> shipmentList = new List<ShipmentLineModel>();
            var rtnTuple = new Tuple<int, int, int, List<ShipmentLineModel>>
                    (0, 0, 0, shipmentList);
            rtnTuple = AnalyzeBOM(bomMatches);
            BOMSummaryModel summary = new BOMSummaryModel();
            bomMatches = rtnTuple.Item4;

            //Create new worksheet to display this BOM's results
            //Make header at row 3 - pct match will be on row 1
            int row = 3;
            (Excel.Workbook wkb, int row, string wksName) rtn = MakeMatchXL(wkbResults, xlApp, row, quoteID);
            wkbResults = rtn.wkb;
            //Populate worksheet with results
            row = rtn.row;
            int startRow = row;
            Excel.Worksheet wksResults = wkbResults.ActiveSheet;

            //Calculate and display pct matches
            int pctRow = 1;
            summary.PID = rtn.wksName;
            summary.BOMLines = bomItems.Count;
            summary.LineMatches = distinctMatches;
            pctMatch = distinctMatches * 100 / bomItems.Count;
            wksResults.Cells[pctRow, 2].Value = "Percent of BOM Lines Matching Shipments";
            wksResults.Cells[pctRow, 3].Value = Math.Round(pctMatch).ToString() + "%";

            double pctCityMatch = rtnTuple.Item1 * 100 / bomItems.Count;
            summary.CityMatches = rtnTuple.Item1;
            wksResults.Cells[pctRow, 9].Value = "Percent City Matches";
            wksResults.Cells[pctRow, 10].Value = Math.Round(pctCityMatch).ToString() + "%";

            double pctStateMatch = rtnTuple.Item2 * 100 / bomItems.Count;
            summary.StateMatches = rtnTuple.Item2;
            wksResults.Cells[pctRow, 11].Value = "Percent State Matches";
            wksResults.Cells[pctRow, 12].Value = Math.Round(pctStateMatch).ToString() + "%";

            int dateAndStateTrues = rtnTuple.Item3;
            summary.ValidSO_DateMatches = rtnTuple.Item3;
            wksResults.Cells[pctRow, 14].Value = "Number of Date and State Matches";
            wksResults.Cells[pctRow, 15].Value = summary.ValidSO_DateMatches;

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
                InsertText(wksResults, row, 10, match.CityMatch.ToString());
                InsertText(wksResults, row, 11, match.QuoteState);
                InsertText(wksResults, row, 12, match.StateMatch.ToString());
                InsertText(wksResults, row, 13, match.SODate.ToShortDateString());
                InsertText(wksResults, row, 14, match.QuoteDateCompleted);
                InsertText(wksResults, row, 15, match.SONewerThanBOM.ToString());

                row++;
            }

            //Find last row of match data
            int bottomRowMatches = FindLastSpreadsheetRow(wksResults); 

            //InsertText non matches;
            row = row + 3;
            Range range = (Excel.Range)wksResults.Range[wksResults.Cells[1, 1], wksResults.Cells[bottomRowMatches, 14]];
            CenterTextInRange(wkbResults, range);

            //Begin display of non-matches
            row = MakeHeader(wksResults, row, 15, "Non-matching BOM Line Items");

            //Insert column headers
            InsertText(wksResults, row, 2, "Part Number");
            InsertText(wksResults, row, 3, "Quantity");
            InsertText(wksResults, row, 4, "Description");
            range = (Excel.Range)wksResults.Range[wksResults.Cells[row, 2], wksResults.Cells[row, 3]];
            wksResults.Rows[row].EntireRow.Font.Bold = true;
            range.HorizontalAlignment = XlHAlign.xlHAlignCenter;

            row++;
            int startNonMatch = row;
            wksResults.Range[wksResults.Cells[startNonMatch - 1, 4], wksResults.Cells[startNonMatch - 1, 15]].Merge();
            wksResults.Range[wksResults.Cells[startNonMatch - 1, 4], wksResults.Cells[startNonMatch - 1, 15]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            foreach (var non_match in bomNonMatches)
            {
                InsertText(wksResults, row, 3, non_match.Quantity.ToString());
                //string modelNumber = wksResults.Cells[row, 2].ToString();
                string modelNumber = non_match.ModelNumber;
                if (modelNumber != null)
                {
                    InsertText(wksResults, row, 2, modelNumber);
                }
                string desc = non_match.Description;
                if (desc != null)
                {
                    InsertText(wksResults, row, 4, desc);
                }

                row++;
            }
            for (int i = startNonMatch - 1; i < row; i++)
            {
                wksResults.Cells[i, 4].WrapText = true;
                wksResults.Rows[i].RowHeight = 28.5;
                wksResults.Range[wksResults.Cells[i + 1, 4], wksResults.Cells[i + 1, 15]].Merge();
            }
            wksResults.Range[wksResults.Cells[startNonMatch, 2], wksResults.Cells[row - 1, 3]].
                HorizontalAlignment = XlHAlign.xlHAlignCenter;

            //conditional formatting for analysis columns
            ConditionalFormatTrueFalse(startRow, bottomRowMatches + 1, 10, wkbResults);
            ConditionalFormatTrueFalse(startRow, bottomRowMatches + 1, 12, wkbResults);
            ConditionalFormatTrueFalse(startRow, bottomRowMatches + 1, 15, wkbResults);
            ConditionalFormatNumber(startRow, bottomRowMatches + 1, 5, wkbResults);

            return summary;
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
            wkb.Sheets.Add();   
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
        //private static List<ShipmentLineModel>  AnalyzeBOM(List<ShipmentLineModel> list)
        private static Tuple<int, int, int, List<ShipmentLineModel>> AnalyzeBOM(List<ShipmentLineModel> list)
        {
            int cityMatches = 0;
            int stateMatches = 0;
            int dateAndStateTrue = 0;

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
                string stateAbbreviation = "";
                if (line.QuoteState != "")
                {
                     stateAbbreviation = GlobalConfig.Connection.GetStateAbbreviation(line.QuoteState); 
                }
                string soCity = line.City;
                string soState = line.State;
                string quoteCity = line.QuoteCity.ToUpper().ToString();
                if (quoteCity == soCity)
                {
                    line.CityMatch = true;
                    cityMatches++;
                }
                else 
                {
                    line.CityMatch = false;
                }
                if (stateAbbreviation == soState)
                {
                    line.StateMatch = true;
                    stateMatches++;
                }
                else
                {
                    line.StateMatch = false;
                }

                if (line.StateMatch == true && line.SONewerThanBOM == true)
                {
                    dateAndStateTrue++;
                }

                //Compare quan ordered vs BOM quan
                double diff = 0;
                double.TryParse(line.BOM_Quantity, out diff );
                line.QShippedMinusQBOM = line.Quantity - diff;
            }
            var rtnVal = new Tuple<int, int, int, List<ShipmentLineModel>>
                    (cityMatches, stateMatches, dateAndStateTrue, list);

            return rtnVal;
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
        private static (Excel.Workbook wkb, int row, string wksName) MakeMatchXL(Excel.Workbook wkbResults, Excel.Application xlApp, int startRow, string name)
        {
            (Excel.Workbook wkb, int row, string wksName) rtn;
            Excel.Worksheet wks = wkbResults.ActiveSheet;
            wks.Hyperlinks.Add(wks.Cells[1, 1], "", "Summary!A1", "Summary Page","Go to Summary Page");
            int row = MakeHeader(wks, startRow, 15, "BOM Lines with Matches in Shipments");
            InsertText(wks, row, 1, "Shipment Row");
            InsertText(wks, row, 2, "Part Number");
            InsertText(wks, row, 3, "BOM Quantity");
            InsertText(wks, row, 4, "Shipment Quantity");
            InsertText(wks, row, 5, "Quan Shipped - Quan BOM");
            InsertText(wks, row, 6, "Sales Order ID");
            InsertText(wks, row, 7, "Shipment City");
            InsertText(wks, row, 8, "Shipment State");
            InsertText(wks, row, 9, "Quote City");
            InsertText(wks, row, 10, "City Match");
            InsertText(wks, row, 11, "Quote State");
            InsertText(wks, row, 12, "State Match");
            InsertText(wks, row, 13, "SO Date");
            InsertText(wks, row, 14, "Date Quote Completed");
            InsertText(wks, row, 15, "SO Newer Tham BOM");

            rtn.wksName = FormatXLSheet(wkbResults, name, row);
            rtn.wkb = wkbResults;
            rtn.row = row + 1;
            return rtn;
        }

        /// <summary>
        /// Sets column widths on spreadsheet and sets name
        /// </summary>
        /// <param name="wkbResults"></param>
        /// <param name="name"></param>
        private static string FormatXLSheet(Excel.Workbook wkbResults, string name, int row)
        {
            Excel.Worksheet wks = wkbResults.ActiveSheet;
            //               A   B   C   D   E   F   G   H   I   J   K   L   M   N     O
            int[] widths = { 10, 30, 12, 12, 12, 20, 25, 12, 25, 12, 25, 12,  15, 15, 12};

            for (int i = 1; i <= widths.Length; i++)
            {
                wks.Columns[i].ColumnWidth = widths[i - 1];
            }
            Excel.Range range = (Excel.Range)wks.Range[wks.Cells[row, 1], wks.Cells[row, 15]];

            //Format column headers
            wks.Rows[row].WrapText = true;
            wks.Rows[row].EntireRow.Font.Bold = true;
            string suffix = "";
            try
            {
                wks.Name = name;
            }
            catch (Exception e)
            {
                if (e.Message == "That name is already taken. Try a different one.")
                {
                    string nameCheck = name.Substring(name.Length - 3, 1);
                    if (nameCheck == "(")
                    {
                        nameCheck = name.Substring(name.Length - 1, 1);
                        if (nameCheck == ")")
                        {
                            suffix = name.Substring(name.Length - 2, 1);
                            byte[] ascSuffix = Encoding.ASCII.GetBytes(suffix);
                            byte b = ascSuffix[0];
                            int inc = b + 1;
                            ascSuffix = BitConverter.GetBytes(inc);
                            suffix = Encoding.ASCII.GetString(ascSuffix)[0].ToString();
                            char[] nameArray = name.ToCharArray();
                            int pos = nameArray.Length - 2;
                            nameArray[pos] = char.Parse(suffix);
                            name = new string(nameArray);
                            wks.Name = name;
                        }
                    }
                    else 
                    {
                        wks.Name = name + "(A)";
                    }
                }
            }
            return wks.Name;
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

        /// <summary>
        /// Applies filters to worksheet columns
        /// </summary>
        /// <param name="wkb"></param>
        private static void ApplyFilters(Excel.Workbook wkb)
        {
            Excel.Worksheet wks = wkb.ActiveSheet;
            Range filterRange = wks.Range[wks.Cells[5, 1], wks.Cells[5, 15]];
            object filters = filterRange.AutoFilter(1);
            filterRange.AutoFilter(15, "TRUE");
        }

        /// <summary>
        /// Create summary sheet for analyzing results
        /// </summary>
        /// <param name="wkb"></param>
        /// <param name="summaryList"></param>
        private static void CreateSummarySheet(Excel.Workbook wkb, List<BOMSummaryModel> summaryList)
        { 
            Excel.Worksheet wks = wkb.Sheets["Sheet1"];
            Excel.Worksheet wksStudy = wkb.Sheets[1];
            MakeHeader(wks,  1, 7, "Summary");
            wks.Name = "Summary";

            FormatSummarySheet(wks, summaryList.Count);
            int row = 4;
            foreach (var summary in summaryList)
            {
                WriteSummaryLine(wks, row, summary);
                row++;
            }
        }

        /// <summary>
        /// Places row headers on summary sheet and formats text
        /// </summary>
        /// <param name="wks"></param>
        /// <param name="rows"></param>
        private static void FormatSummarySheet(Excel.Worksheet wks, int rows)
        {
            wks.Cells[3, 1].Value = "Request ID";
            wks.Cells[3, 2].Value = "Number of BOM Line Items";
            wks.Cells[3, 3].Value = "Number of Matched Items";
            wks.Cells[3, 4].Value = "Percent BOM Line Matches";
            wks.Cells[3, 5].Value = "Number of City Matches";
            wks.Cells[3, 6].Value = "Number of State Matches";
            wks.Cells[3, 7].Value = "Number of Valid Date and State Matches";
            wks.Rows[3].WrapText = true;
            Range range = wks.Range[wks.Cells[3, 1], wks.Cells[3 + rows, 7]];
            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            range.ColumnWidth = 15;
            wks.Columns[1].ColumnWidth = 65;
            wks.Rows[3].EntireRow.Font.Bold = true;
        }

        /// <summary>
        /// Writes line of summary data on summary sheet
        /// Adds hyperlink back to detail sheet
        /// </summary>
        /// <param name="wks"></param>
        /// <param name="row"></param>
        /// <param name="lineData">
        ///     Item1 - PID
        ///     Item2 - linr match %
        ///     Item3 - city match %
        ///     Item4 - state match %
        ///     Item5 - date and state matches
        /// </param>

        private static void WriteSummaryLine(Excel.Worksheet wks, int row, BOMSummaryModel summary)
        {
            wks.Cells[row, 1] = summary.PID;
            string subAddress = "'" + summary.PID + "'!A1";
            wks.Hyperlinks.Add(wks.Cells[row, 1], "", subAddress, "Details");
            wks.Cells[row, 2] = summary.BOMLines; 
            wks.Cells[row, 3] = summary.LineMatches;
            wks.Cells[row, 4] = summary.pctLineMatches;
            wks.Cells[row, 5] = summary.CityMatches;
            wks.Cells[row, 6] = summary.StateMatches;
            wks.Cells[row, 7] = summary.ValidSO_DateMatches;
        }
    }
}
