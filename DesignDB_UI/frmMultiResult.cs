using DesignDB_Library.Models;
using DesignDB_Library.Operations;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace DesignDB_UI
{
    public partial class frmMultiResult : Form
    {  
        List<RequestModel> Requests;
        public List<RequestModel> dataList
        {
            get 
            {
                return dataList;
            }
            set 
            {
                dgvResults.DataSource = value;
            }
        }
        DateTime startDate = new DateTime(1900, 1, 1);
        DateTime endDate = new DateTime(1900, 1, 1);
        string MSO = "";
        public frmMultiResult(List<RequestModel> requests)
        {            
            InitializeComponent();
            if (GV.MODE == Mode.Forecast)
            {
                this.Show();
                frmDateRangeSearch frmDateRangeSearch = new frmDateRangeSearch();
                frmDateRangeSearch.DateRangeSet += FrmDateRangeSearch_DateRangeSet;
                frmDateRangeSearch.ShowDialog();
            }
            else
            {
                Requests = requests;
                dgvResults.DataSource = requests;
                txtRecordsReturned.Text = Requests.Count.ToString();
            }
        }

        private void FrmDateRangeSearch_DateRangeSet(object sender, frmDateRangeSearch.DateRangeEventArgs e)
        {
            startDate = e.StartDate;
            endDate = e.EndDate;
            MSO = e.MSO;
            txtRecordsProcessed.Visible = true;
            lblRecordsProcessed.Visible = true;
            pbProgress.Visible = true;
            List<RequestModel> Requests = ForecastFunction.GetForecastRequests(MSO, startDate, endDate, "DateAssigned");
            dgvResults.DataSource = Requests;
            ReportOps.FormatMultiResultDGV(dgvResults);
            txtRecordsReturned.Text = Requests.Count.ToString();
            ForecastFunction.DoForecast(Requests, startDate, endDate, MSO, "DateAssigned",
                txtRecordsProcessed, pbProgress, dgvResults);
        }

        private void frmMultiResult_Load(object sender, EventArgs e)
        {
            txtRecordsProcessed.Visible = false;
            lblRecordsProcessed.Visible = false;
            pbProgress.Visible = false;
            switch (GV.MODE)
            {
                case Mode.Edit:
                case Mode.Delete:
                case Mode.Restore:
                    dgvResults.DataSource = Requests;
                    dgvResults.ClearSelection();                        
                    ReportOps.FormatMultiResultDGV(dgvResults);
                    break;                
                default:
                    break;
            }
        }

        private void dgvResults_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int selRow = dgvResults.CurrentRow.Index;
            List<RequestModel> myRequest = new List<RequestModel>();
            myRequest.Add(Requests[selRow]);
            FC.DisplayRequestForm(myRequest[0]);         
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {            
            ListLooper.ExcelExporter<RequestModel> exporter = new ListLooper.ExcelExporter<RequestModel>();
            exporter.List = (List<RequestModel>)dgvResults.DataSource;
            ReportOps.FormatMultiResultExport(exporter.Wksheet);
        }
    }
}

