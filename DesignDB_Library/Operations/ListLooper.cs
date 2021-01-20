using DesignDB_Library.Attributes;
using DesignDB_Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace DesignDB_Library.Operations
{
    public static class ListLooper
    {
        private static Excel.Worksheet wks = null;

        public static void excelRequestExport(List<RequestModel> list)
        {
            Excel.Application xlApp = makeRequestSheet();
            loadData(list);
            ExcelOps.releaseObject(xlApp);
        }

        private static void loadData(List<RequestModel> list)
        {
            int row = 2;

            foreach (RequestModel item in list)
            {
                wks.Cells[row, 1].Value = item.ProjectID;
                wks.Cells[row, 2].Value = item.MSO;
                wks.Cells[row, 3].Value = item.Cust;
                wks.Cells[row, 4].Value = item.City;
                wks.Cells[row, 5].Value = item.ST;
                wks.Cells[row, 6].Value = item.Country;
                wks.Cells[row, 7].Value = item.Region;
                wks.Cells[row, 8].Value = item.DesignRequestor;
                wks.Cells[row, 9].Value = item.QuoteType;
                wks.Cells[row, 10].Value = item.Pty;
                wks.Cells[row, 11].Value = item.Designer;
                wks.Cells[row, 12].Value = item.ProjectName;
                wks.Cells[row, 13].Value = item.OriginalQuote;
                wks.Cells[row, 14].Value = item.AssistedBy;
                wks.Cells[row, 15].Value = item.Category;
                wks.Cells[row, 16].Value = item.ArchitectureType;
                wks.Cells[row, 17].Value = item.DateAssigned;
                wks.Cells[row, 18].Value = item.DateAllInfoReceived;
                wks.Cells[row, 19].Value = item.DateDue;
                wks.Cells[row, 20].Value = item.AwardStatus;
                wks.Cells[row, 21].Value = item.DateLastUpdate;
                wks.Cells[row, 22].Value = item.ReviewedBy;
                wks.Cells[row, 23].Value = item.DateCompleted;
                wks.Cells[row, 24].Value = item.TotalHours;
                wks.Cells[row, 25].Value = item.BOM_Value;
                wks.Cells[row, 26].Value = item.PercentageProjectCovered;
                wks.Cells[row, 27].Value = item.ArchitectureDetails;
                //wks.Cells[row, 27].Value = item.Comments;
                row++;
            }
        }

        private static Excel.Application makeRequestSheet()
        {
            //get application, set worksheet
            Excel.Application xlApp = ExcelOps.makeExcelApp();
            xlApp.Workbooks.Add();
            wks = xlApp.ActiveSheet;
            xlApp.Visible = true;


            //make header row            
            int col = 1;

            string[] headers = new string[]
            {
                "Project ID", "MSO", "End Customer", "City", "State","Country", "Region", "Design Requestor", "Quote Type",
                "Priority", "Designer", "Project Name", "Original Quote", "Assisted By", "Category", "Architecture Type",
                "Date Assigned", "Date All Info Received", "Date Due", "Award Status", "Date Last Update", "Reviewed By",
                "Date Completed", "Total Hours", "BOM Value", "% Project Covered", "Architecture Details", "Comments"
            };

            int[] widths = new int[]
            {
                26, 26, 26, 26, 20,26, 26,20,15, 10,15,26,26,15,10,20,15,15,15,15,15,15,15,15,15,15,50,26
            };

            foreach (string header in headers)
            {
                wks.Cells[1, col].Value = headers[col - 1];
                wks.Columns[col].ColumnWidth = widths[col - 1];
                wks.Columns[col].HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;
                col++;
            }

            return xlApp;
        }

        public static void useReflection(object obj, Type T)
        {

            FieldInfo[] fields = T.GetFields();
            foreach (FieldInfo field in fields)
            {
                string fName = field.Name;
                string fType = field.FieldType.Name;
            }
        }

        /// <summary>
        /// Instantiate class and set List property to a generic list
        /// Wksheet property will hold unformatted worksheet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class ExcelExporter<T>
        {
            List<T> _list = null;
            public List<T> List
            {
                get
                {
                    return _list;
                }
                set
                {
                    List<T> _list = value;
                    if (_list.Count > 0)
                    {
                        Type type = _list[0].GetType();
                        Excel.Worksheet wks = excelExport(type, _list);
                        Wksheet = wks;
                    }
                }
            }

            public Excel.Worksheet Wksheet { get; set; }

            /// <summary>
            /// Reflects into list and makes header row and data rows
            /// </summary>
            /// <param name="type"></param>
            /// <param name="list"></param>
            private Excel.Worksheet excelExport(Type type, List<T> list)
            {
                //make xlApp
                Excel.Application xlApp = ExcelOps.makeExcelApp();
                xlApp.Workbooks.Add();
                xlApp.Visible = true;

                //set wks
                Excel.Worksheet wks = xlApp.ActiveSheet;
                wks.get_Range("1:1").WrapText = true;
                wks.get_Range("1:1").Font.Bold = true;

                //get property names, place in row 1

                PropertyInfo[] properties = type.GetProperties();
                Collection<PropertyInfo> props = new Collection<PropertyInfo>();
                for (int i = 0; i < properties.Length; i++)
                {

                    Object[] att = properties[i].GetCustomAttributes(true);
                    for (int j = 0; j < att.Length; j++)
                    {
                        Type aType = att[j].GetType();
                        if (aType.Name == "ExcelExportPropertyAttribute")
                        {
                            props.Add((PropertyInfo)properties[i]);
                        }

                    }
                }


                int col = 1;
                foreach (var prop in props)
                {
                    wks.Cells[1, col].Value = prop.Name;
                    col++;
                }

                int row = 2;
                col = 1;
                foreach (var request in list)
                {
                    foreach (PropertyInfo prop in props)
                    {
                        wks.Cells[row, col].Value = prop.GetValue(request);
                        col++;
                    }
                    row++;
                    col = 1;
                }
                ExcelOps.releaseObject(xlApp);
                return wks;

            }


        }
    }
}

