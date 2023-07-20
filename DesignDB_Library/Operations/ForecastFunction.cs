using DesignDB_Library.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace DesignDB_Library.Operations
{
    public class ForecastFunction
    {
        //create lists for counted and uncounted quotes
        private static List<string> uncounted = new List<string>();
        private static Excel.Workbook book = null;
        private static Excel.Worksheet wks = null;
        private static Excel.Application xlApp = null;
        private static int startRow = 0;
        private static int startCol = 0;

        /// <summary>
        /// Creates worksheet, gets all requests, loops thru each one to find BOM.
        /// Sends filename to begin processing
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="mso"></param>
        /// <param name="searchTerm"></param>
        
        public static void DoForecast(List<RequestModel> requests, DateTime startDate, DateTime endDate, string mso, 
            TextBox progressBox, ProgressBar pb, DataGridView dgv)
        {
            CreateForecastWorksheet(startDate, endDate, mso);
            processRequests(requests, progressBox, pb, dgv);
        }
        private static void processRequests(List<RequestModel> requests, TextBox pBox, ProgressBar pb, DataGridView dgv) 
            
        {
            pb.Minimum = 0;
            pb.Maximum = requests.Count;
            pb.Step = 1;
            pb.Value = 0;
            int recordsProcessed = 0;            
            pBox.Text = recordsProcessed.ToString();            
            List<BOM_Model> products = new List<BOM_Model>();
            foreach (RequestModel request in requests)
            {
                int row = 0;
                string fileName = getBOM(request);
                if (fileName != null)
                {
                    products = processBOM(fileName, request.ProjectID, products);
                    pb.PerformStep();
                    dgv.Rows[row].Selected = true;
                    if (recordsProcessed > 0)
                    {
                        dgv.Rows[recordsProcessed - 1].Selected = false;
                    }                    
                    
                    recordsProcessed++;
                    pBox.Text = recordsProcessed.ToString();
                    Application.DoEvents();
                }
                else
                {
                    uncounted.Add(request.ProjectID);
                }
            }
            List<ForecastModel> forecast = processProducts(products);

            loadForecastToExcel(forecast);
            loadUncountedIntoCell();
            xlApp.WindowState = Excel.XlWindowState.xlMaximized;
            //xlApp.Visible = true;

            ExcelOps.releaseObject(xlApp);
        }

        public static void DoForecast(List<RequestModel> requests, DateTime startDate, DateTime endDate, string mso, 
            string searchTerm, TextBox progressBox, ProgressBar pb, DataGridView dgv)
        {
            CreateForecastWorksheet(startDate,endDate,mso);
            processRequests(requests, progressBox, pb, dgv);
        }

        private static Excel.Application CreateForecastWorksheet(DateTime startDate, DateTime endDate, string mso)
        {
            //xlApp = ExcelOps.makeExcelApp();
            xlApp = ExcelOps.CreateForecastSheet();
            //xlApp.Visible = true;
            book = xlApp.ActiveWorkbook;
            wks = xlApp.ActiveSheet;

            //populate dates and MSO
            wks.Cells[2, 2].Value = startDate;
            wks.Cells[3, 2].Value = endDate;
            wks.Cells[4, 2].Value = mso;            
          
            return xlApp;
        }

        public static List<RequestModel> GetForecastRequests(string mso, DateTime startDate, DateTime endDate, 
            string searchTerm)
        {
            List<RequestModel> requests = null;
            if (mso == "")
            {
                requests = GlobalConfig.Connection.DateRangeSearch_Unfiltered
                    (startDate, endDate, searchTerm, false, "");
            }
            else
            {
                requests = GlobalConfig.Connection.DateRangeSearch_MSOFiltered
                    (startDate, endDate, searchTerm, mso, false, "", "");
            }

            return requests;
        }

        private static string getBOM(RequestModel request)
        {
            string bom = "";
            List<AttachmentModel> attachments = GlobalConfig.Connection.GetAttachments(request.ProjectID);
            foreach (AttachmentModel attachment in attachments)
            {
                if (attachment.ItemType == "BOM")
                {
                    try
                    {
                        bom = GlobalConfig.AttachmentPath + "\\" + request.ProjectID + "\\" + attachment.DisplayText;
                        Application.DoEvents();
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show("Error processing BOM for " + request.ProjectID +
                            "\nFile name " + attachment.DisplayText + "\n" + ex.Message);
                    }
                }
            }

            return bom;
        }


        private static List<BOM_Model> processBOM(string fileName, string PID, List<BOM_Model> products)
        {         
            if (File.Exists(fileName))
            {
                //check existance of xlApp
                if (xlApp == null)
                {
                    xlApp = ExcelOps.makeExcelApp();
                }                
                //open BOM
                ExcelOps.OpenExcelWorkbook(xlApp, fileName);
                book = xlApp.ActiveWorkbook;
                wks = xlApp.ActiveSheet;

                //find header row in BOM
                Excel.Range header = findHeaderRow(xlApp);
                startCol = header.Column;
                startRow = header.Row + 1;

                //loop thru BOM rows and build BOM models
                int i = startRow;
                //bool ccNull = string.IsNullOrEmpty(wks.Cells[i, startCol].Value?.ToString());
                var qString = wks.Cells[i, startCol].Value.ToString();
                while (qString != null)
                {
                    BOM_Model model = new BOM_Model();
                    model.Quote = PID;
                    int quantity = 0;                    
                    int.TryParse(qString, out  quantity);
                    model.Quantity = quantity;
                    try
                    {
                        model.ModelNumber = wks.Cells[i, startCol + 1].Value.ToString();
                    }
                    catch (Exception)
                    {
                                               
                    }

                    try
                    {
                        model.Description= wks.Cells[i, startCol + 2].Value.ToString();
                    }
                    catch (Exception)
                    {
                        
                    }

                    //add to products list. combine duplicates at end
                    products.Add(model);

                    model = null;

                    //catch null error. If null, trigger end of while loop.
                    i++;
                    try
                    {
                        qString = wks.Cells[i, startCol].Value.ToString();
                    }
                    catch (Exception)
                    {
                        qString=null;
                    }
                }
                book.Close(false);              
                ExcelOps.releaseObject(book);
            }
            else
            {
                uncounted.Add(PID);
            }

            //lists are made. Sort thru products and combine
            //List<ForecastModel> forecast = processProducts();
            return products;
        }

        private static void loadForecastToExcel(List<ForecastModel> forecast)
        {
            wks = xlApp.ActiveSheet;
            int row = 6;
            foreach ( ForecastModel product in forecast)
            {
                wks.Cells[row, 1].Value = product.Quantity;
                wks.Cells[row, 2].Value = product.ModelNumber;
                wks.Cells[row, 3].Value = product.Description;           
                loadListIntoCell(product.Quotes, row, 4);
                row++;
            }
        }

        private static void loadUncountedIntoCell()
        {
            int i = 0;
            foreach (string item in uncounted)
            {       
                int row = startRow;
                wks.Cells[i + 6, 7].Value = item;
                i++;
            }
        }

        private static void loadListIntoCell(List<string> list, int row, int col)
        {
            foreach (string item in list)
            {
                if (wks.Cells[row, col].Value == null)
                {
                    wks.Cells[row, col].Value = wks.Cells[row, col].Value = item + " ";
                }
                else
                {
                    wks.Cells[row, col].Value = wks.Cells[row, col].Value.ToString() + item + " ";                
                }
            }
        }

  
        private static List<ForecastModel> processProducts(List<BOM_Model> products)
        {
            List<ForecastModel> forecast = new List<ForecastModel>();
            ForecastModel itemToAdd = null;
            //iterate thru products list
            foreach (BOM_Model product in products)
            {
                bool match = false;
                foreach (ForecastModel forecastItem  in forecast)
                {
                    if (product.ModelNumber == forecastItem.ModelNumber)
                    {
                        match = true;
                        itemToAdd = forecastItem;
                    }
                }
            
                if (match)
                {
                    itemToAdd.MergeLine(product);
                                       
                    
                }
                else
                {
                    addBOMtoForecast(forecast, product);
                }
                
            }

            return forecast;
        }

        private static void addBOMtoForecast(List<ForecastModel> forecast, BOM_Model product)
        {
            ForecastModel newLine = new ForecastModel();
            newLine.Quantity = product.Quantity;
            newLine.ModelNumber = product.ModelNumber;
            newLine.Description = product.Description;
            newLine.Quotes.Add(product.Quote);

            forecast.Add(newLine);
        }

        private static Excel.Range findHeaderRow(Excel.Application xlApp)
        {
            wks = xlApp.ActiveSheet;
            Excel.Range range =  wks.get_Range("A4:AZ7");
            Excel.Range result = range.Find("Quantity");
            
            return result;
        }

    }
}


