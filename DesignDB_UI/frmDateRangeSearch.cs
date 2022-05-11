using DesignDB_Library;
using DesignDB_Library.Operations;
using DesignDB_Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace DesignDB_UI
{
    public partial class frmDateRangeSearch : Form
    {
        public event EventHandler<DateRangeEventArgs> DateRangeSet;
        List<RequestModelReport> requestList = new List<RequestModelReport>();
        string term = "";
        public frmDateRangeSearch()
        {
            InitializeComponent();
            List<MSO_Model> msoList = GlobalConfig.Connection.GetAllMSO();
            cboMSO.DataSource = msoList;
            cboMSO.DisplayMember = "MSO";
            cboMSO.SelectedIndex = -1;
            term = "DateAssigned";
            if (GV.MODE == Mode.DateRangeSearch)
            {
                List<DesignersReviewersModel> designers = GlobalConfig.Connection.GetAllDesigners();
                cboDesigner.DataSource= designers;
                cboDesigner.DisplayMember = "Designer";
                cboDesigner.SelectedIndex = -1;

                List<SalespersonModel> requestors = GlobalConfig.Connection.GenericGetAll<SalespersonModel>("tblSalespersons", "SalesPerson");
                cboRequestor.DataSource = requestors;
                cboRequestor.DisplayMember = "Salesperson";
                cboRequestor.SelectedIndex = -1;

                btnForecast.Visible = false;
                btnSearch.Visible = true;
            }
            else
            {
                btnSearch.Visible = false;
                btnForecast.Visible = true;
            }

            FC.SetFormPosition(this);
            this.BringToFront();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdoDate_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb != null && rb.Checked)
            {
                term = "DateAssigned";
            }
        }

        private void rdoDateDue_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb != null && rb.Checked)
            {
                term = "DateDue";
            }
        }

        private void rdoDateCompleted_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb != null && rb.Checked)
            {
                term = "DateCompleted";
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int records = requestList.Count;
            
            requestList = GlobalConfig.Connection.ReportDateRangeSearch_MSOFiltered
                (dtpStartDate.Value, dtpEndDate.Value, term, cboMSO.Text, false,cboDesigner.Text,cboRequestor.Text);
            records = requestList.Count;

            switch (records)
            {
                case 0:
                    MessageBox.Show("No records found");
                    break;

                default:
                    //frmMultiResult frmMultiResult = new frmMultiResult(requestList);
                    GV.MultiResult.ReportDataList = requestList;
                    GV.MultiResult.Show();
                    break;
            }
            this.Close();
        }

        private void btnForecast_Click(object sender, EventArgs e)
        {
                  
            DateRangeEventArgs args = new DateRangeEventArgs();
            args.StartDate = dtpStartDate.Value;
            args.EndDate = dtpEndDate.Value;
            args.MSO = cboMSO.Text;
            DateRangeSet?.Invoke(this, args);
            this.Close();    
        }

        public class DateRangeEventArgs : EventArgs
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string MSO { get; set; }
        }



    }   
}            


