using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DesignDB_Library;
using DesignDB_Library.Operations; 
using Excel = Microsoft.Office.Interop.Excel;

namespace DesignDB_Utilities
{
    public partial class frmUtilityMenu : Form
    {
        public frmUtilityMenu()
        {
            InitializeComponent();
        }

        private void btnImportAtt_Click(object sender, EventArgs e)
        {
            {
                FilePicker fp = new FilePicker();
                string fileName = fp.GetFile(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;

                xlApp = ExcelOps.makeExcelApp();
                xlApp = ExcelOps.OpenExcelWorkbook(xlApp, fileName);
                xlApp.Visible = true;
                xlWorkBook = xlApp.Workbooks[1];
                xlWorkSheet = (Excel.Worksheet)xlApp.ActiveSheet;

                //Move items to table
                AttachmentOps.ImportAttachmentsFromExcel(xlApp, txtRow);
                  
                ExcelOps.releaseObject(xlWorkSheet);
                ExcelOps.releaseObject(xlWorkBook);
                ExcelOps.releaseObject(xlApp);                
            }

            System.Windows.Forms.MessageBox.Show("Complete");
        }

        private void btnForecast_Click(object sender, EventArgs e)
        {
            //ForecastFunction.DoForecast(DateTime.Parse("Nov 1, 2020"), DateTime.Parse("Nov 3, 2020"), "Comcast", "DateAssigned");
        }
    }
}
