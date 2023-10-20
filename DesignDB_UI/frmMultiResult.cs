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
        //List<RequestModelReport> reportModel;
        List<RequestModel> reportModel;
        private Point _formLocation;
        private bool formLoading;
        List<RequestModel> Requests = new List<RequestModel>();
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
                ReportOps.FormatMultiResultDGV(dgvResults);
                //GV.MODE = Mode.Edit;
            }
        }


        //public List<RequestModel> ReportDataList
        //{
        //    get
        //    {
        //        return reportModel; ;
        //    }
        //    set
        //    {
        //        reportModel = value;
        //        txtRecordsReturned.Text = reportModel.Count.ToString();
        //        dgvResults.DataSource = null;
        //        dgvResults.DataSource = reportModel;
        //        ReportOps.ReportFormatMultiResultDGV(dgvResults);
        //    }
        //}

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
            DateTime emptyDate = new DateTime(1900,1,1);
            List<RequestModel> Requests = ForecastFunction.GetForecastRequests(MSO, startDate, endDate, "DateAssigned");
            Requests = Requests.Where(x => x.AwardStatus != "Canceled" && x.DateCompleted != emptyDate).ToList();
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
            btnExport.Visible = true;
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
                    //ReportOps.ReportFormatMultiResultDGV(dgvResults);
                    break;
                case Mode.Report_OpenRequests:
                    ReportOps.FormatMultiResultDGV(dgvResults);
                    break;
                case Mode.Forecast:
                    btnExport.Visible = false;
                    break;
                default:
                    break;
            }
        }

        private void dgvResults_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int selRow = dgvResults.CurrentRow.Index;
            List<RequestModel> myRequest = new List<RequestModel>();

            if (GV.MODE == Mode.DateRangeSearch)
            {
                //RequestModel request = /*RequestModelReportToRequestModel.ConvertReportModelToRequestModel*/(reportModel[selRow]);
                myRequest.Add(Requests[selRow]);
            }
            else
            {
                myRequest.Add(Requests[selRow]);
            }
            FC.DisplayRequestForm(myRequest[0]);         
            
            if (GV.MODE != Mode.Delete & GV.MODE != Mode.Restore)
            {
                GV.MODE = Mode.Edit;
            }
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            dgvResults.DataSource = null;
            pbProgress.Value = 0;
            txtRecordsProcessed.Text = "";
            txtRecordsReturned.Text = "";
            this.Hide();
            GV.MAINMENU.BringToFront();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            switch (GV.MODE)
            {               
                case Mode.DateRangeSearch:
                    ListLooper.ExcelExporter<RequestModelReport> reportExporter = new ListLooper.ExcelExporter<RequestModelReport>();
                    List<RequestModel> requests = (List<RequestModel>)dgvResults.DataSource;
                    List<RequestModelReport> listRpt = new List<RequestModelReport>();
                    foreach (RequestModel request in requests)
                    {
                        RequestModelReport report = new RequestModelReport();
                        report = request.RequestModelToRequestModelReport();
                        listRpt.Add(report);
                    }
                    //RequestModelReport rptRequest (List<RequestMode>)dgvResults.DataSource;
                    reportExporter.List = listRpt;
                    ReportOps.ReportFormatMultiResultExport(reportExporter.Wksheet);
                    break;
           
                case Mode.Report_AvgCompletion:
                    ListLooper.ExcelExporter<CompletionTimeModel> avgExporter = new ListLooper.ExcelExporter<CompletionTimeModel>();
                    avgExporter.List = (List<CompletionTimeModel>)dgvResults.DataSource;
                    ReportOps.FormatMultiResultExport(avgExporter.Wksheet);
                    break;
                
                default:
                    ListLooper.ExcelExporter<RequestModel> exporter = new ListLooper.ExcelExporter<RequestModel>();
                    exporter.List = (List<RequestModel>)dgvResults.DataSource;
                    ReportOps.FormatMultiResultExport(exporter.Wksheet);
                    break;
            }
            //if (GV.MODE == Mode.DateRangeSearch)
            //{
            //    ListLooper.ExcelExporter<RequestModelReport> exporter = new ListLooper.ExcelExporter<RequestModelReport>();
            //    exporter.List = (List<RequestModelReport>)dgvResults.DataSource;
            //    ReportOps.ReportFormatMultiResultExport(exporter.Wksheet);
            //}
            //else
            //{
           
            //}
        }

        private void frmMultiResult_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!GV.Exiting)
            {
                e.Cancel = true;
                this.Hide();
                GV.MAINMENU.BringToFront(); 
            }
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

