using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DesignDB_Library.Operations;
using Microsoft.Office.Interop.Excel;
using System.Dynamic;
using DesignDB_Library;
using DesignDB_Library.Models;

namespace DesignDB_UI
{
    class CityState
    {
        public string City { get; set; }
        public string State { get; set; }
    }
    public partial class frmBOM_Bookings : Form
    {
        const int FIRSTROW = 17;
        const int STATECOL = 20;
        const int STCOL = 18;

        Workbook wkb = null;
        Worksheet wks = null;
        Microsoft.Office.Interop.Excel.Application xlApp = null;


        public frmBOM_Bookings()
        {
            InitializeComponent();             
        }

        private void btnImportBookings_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            xlApp = ExcelOps.makeExcelApp();
            OpenFileDialog ofd = ofdImport;
            ofd.DefaultExt = "xlsm";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            ofd.Title = "Select Bookings Spreadsheet";
            //ofd.ShowDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                filePath = ofd.FileName;
            }

            //xlApp.Workbooks.Add();
            //xlApp.ActiveWindow.Zoom = 80;
            xlApp.Visible = true;
            wkb = xlApp.Workbooks.Open(filePath);
            wks = xlApp.ActiveSheet;

            createStateColumn();
        }

        private void createStateColumn()
        {
            CityState cs = new CityState();
            List<CityState> csList = new List<CityState>();

            string st = "";
            int lastUsedRow = wks.UsedRange.Rows.Count - 1;
            pbBookings.Maximum = lastUsedRow;
            pbBookings.Minimum = 0;
            pbBookings.Step = 1;
            List<StateModel> states = GlobalConfig.Connection.GenericGetAll<StateModel>("tblStates", "State");
            wks.Cells[1, STATECOL].Value = "State";
            for (int i = FIRSTROW; i < lastUsedRow; i++) 
            {
                string val = wks.Cells[i, STCOL].Value;
                if (val != null)
                {
                    st = wks.Cells[i, STCOL].Value.ToString(); 
                }
                //string state = GlobalConfig.Connection.getFullStateFromAbbreviation(st);
                StateModel state = states.Find(x => x.Abbreviation == st);
                if (state != null)
                {
                    wks.Cells[i, STATECOL].Value = state.State; 
                    cs.City = wks.Cells[i, STCOL - 1].Value.ToUpper();
                    cs.State = state.State;
                    int idx = csList.IndexOf(cs);
                }
                pbBookings.PerformStep();
                
                csList.Add(cs);
                
            }
            List<CityState> csNoDup = csList.Distinct().ToList();

            MessageBox.Show("Complete");
        }

        private void frmBOM_Bookings_Load(object sender, EventArgs e)
        {
           
        }
    }
    
}
