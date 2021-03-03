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
        private bool _useDefaultLocation = true;
        private Point _formLocation;
        private bool formLoading;
        public bool UseDefaultLocation
        {
            get
            {
                return _useDefaultLocation;
            }
        }

        public Point FormLocation
        {
            get
            {
                return _formLocation;
            }
        }
        List<RequestModel> Requests;
        public List<RequestModel> dataList
        {
            get 
            {
                return dataList;
            }
            set 
            {
                Requests = value;
                txtRecordsReturned.Text = Requests.Count.ToString();
                dgvResults.DataSource = null;
                dgvResults.DataSource = Requests;
            }
        }

        public void DoForecast()
        {
            FC.SetFormPosition(this);
            this.BringToFront();
            this.Show();
            frmDateRangeSearch frmDateRangeSearch = new frmDateRangeSearch();
            frmDateRangeSearch.DateRangeSet += FrmDateRangeSearch_DateRangeSet;
            frmDateRangeSearch.ShowDialog();
        }

        DateTime startDate = new DateTime(1900, 1, 1);
        DateTime endDate = new DateTime(1900, 1, 1);
        string MSO = "";
        public frmMultiResult(List<RequestModel> requests)
        {
            formLoading = true;
            _useDefaultLocation = true;
            GV.MultiResult = this;
            InitializeComponent();
            if (GV.MODE == Mode.Forecast)
            {
                FC.SetFormPosition(this);
                this.BringToFront();
                this.Show();
                frmDateRangeSearch frmDateRangeSearch = new frmDateRangeSearch();
                frmDateRangeSearch.DateRangeSet += FrmDateRangeSearch_DateRangeSet;
                frmDateRangeSearch.ShowDialog();
            }
            else
            {
                if (requests != null)
                {
                    Requests = requests;
                    dgvResults.DataSource = requests;
                    txtRecordsReturned.Text = Requests?.Count.ToString();
                }
            }
            formLoading = false;
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
                case Mode.DateRangeSearch:
                case Mode.Report_OpenRequests:
                    ReportOps.FormatMultiResultDGV(dgvResults);
                    break;
                default:
                    break;
            }
        }

        private void dgvResults_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (GV.MODE != Mode.Delete & GV.MODE != Mode.Restore)
            {
                GV.MODE = Mode.Edit;
            }
            //GV.MODE = Mode.Edit;
            int selRow = dgvResults.CurrentRow.Index;
            List<RequestModel> myRequest = new List<RequestModel>();
            myRequest.Add(Requests[selRow]);
            FC.DisplayRequestForm(myRequest[0]);         
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
            GV.MAINMENU.BringToFront();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {            
            ListLooper.ExcelExporter<RequestModel> exporter = new ListLooper.ExcelExporter<RequestModel>();
            exporter.List = (List<RequestModel>)dgvResults.DataSource;
            ReportOps.FormatMultiResultExport(exporter.Wksheet);
        }

        private void frmMultiResult_FormClosing(object sender, FormClosingEventArgs e)
        {
            GV.MAINMENU.BringToFront();
        }

        private void frmMultiResult_Activated(object sender, EventArgs e)
        {
            FC.SetFormPosition(this, _formLocation.X, _formLocation.Y, false);
            this.BringToFront();
        }

        private void frmMultiResult_Move_1(object sender, EventArgs e)
        {
            _formLocation = this.Location;
            if (!formLoading)
            {
                _useDefaultLocation = false;
            }
        }
    }
}

