using DesignDB_Library;
using DesignDB_Library.Models;
using DesignDB_Library.Operations;
using Squirrel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignDB_UI
{
    public partial class frmMainMenu : Form
    {
        frmInput frmInput = new frmInput();
        frmDateMSO_Picker frmDateMSO_Picker = new frmDateMSO_Picker();

        private bool formLoading = false;
        private bool operationCanceled = false;     //flag to indicate cancel of operation

        public frmMainMenu()
        {
            GV.LogViewer = new frmLogView();
            GV.PickerForm = frmDateMSO_Picker;
            GV.InputForm = frmInput;
            GV.REQFORM = new frmRequests();
            GV.MultiResult = new frmMultiResult(null);
            frmDateMSO_Picker.Hide();
            frmDateMSO_Picker.PickerCanceled += FrmDateMSO_Picker_PickerCanceled;
            frmDateMSO_Picker.DataReadyEvent += FrmDateMSO_Picker_DataReadyEvent;
            formLoading = true;
            frmInput.InputDataReady += FrmInput_InputDataReady;
            InitializeComponent();
            FC.SetFormPosition(this);


#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            CheckForUpdates();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            if (GlobalConfig.DatabaseMode == DatabaseType.Live)
            {
                FC.setDBMode(this, true);
                setModeTextBox(true);
                rdoLive.Checked = true;
            }
            else
            {
                FC.setDBMode(this, false);
                setModeTextBox(false);
                rdoSandbox.Checked = true;
            }

            formLoading = false;
        }


        private void FrmDateMSO_Picker_PickerCanceled(object sender, frmDateMSO_Picker.CancelEventArgs e)
        {
            switch (GV.MODE)
            {
                case Mode.Report_CatMSO:
                    cancelOperation();
                    break;
                case Mode.Report_Snapshot:
                    cancelOperation();
                    break;
                case Mode.Report_AvgCompletion:
                    cancelOperation();
                    break;
                case Mode.Report_ByPriority:
                    cancelOperation();
                    break;
            }
        }

        private void cancelOperation()
        {
            frmDateMSO_Picker.Hide();
            operationCanceled = true;
        }

        private void setModeTextBox(bool live)
        {
            if (live)
            {
                txtMode.BackColor = txtMode.Parent.BackColor;
                txtMode.ForeColor = Color.Black;
                txtMode.Text = "Live";
            }
            else
            {
                txtMode.BackColor = Color.IndianRed;
                txtMode.ForeColor = Color.Black;
                txtMode.Text = "Sandbox";
            }
            AddVersionNumber();
        }

        private void AddVersionNumber()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            txtMode.Text += $"     V.{ versionInfo.FileVersion }";
        }

        private async Task CheckForUpdates()
        {
            if (GlobalConfig.DatabaseMode == DatabaseType.Live)
            {
                using (var manager = new UpdateManager(@"\\sccacve1\Databases\DesignDB\Beta"))
                {
                    await manager.UpdateApp();
                }
            }
            else
            {
                using (var manager = new UpdateManager(@"C:\Users\rharbert\OneDrive - CommScope\SQLDB\Squirrel"))
                {
                    await manager.UpdateApp();
                }
            }
        }

        private void SetButtonVisibility(DesignersReviewersModel designer)
        {
            int priviledge = designer.Priviledge;

            //turn on/off labels
            switch (priviledge)
            {
                case 1:
                    lblAdministrative.Visible = false;
                    lblAdministrator.Visible = false;
                    break;

                case 2:
                    lblAdministrative.Visible = true;
                    lblAdministrator.Visible = false;
                    break;

                default:
                    lblAdministrative.Visible = true;
                    lblAdministrator.Visible = true;
                    break;
            }

            //loop thru controls and compare button.tag to priviledge
            foreach (Control control in tlpMain.Controls)
            {
                if (control is Button || control is RadioButton)
                {
                    int tag = int.Parse((string)(control.Tag));
                    if (priviledge >= tag)
                    {
                        control.Visible = true;
                    }
                    else
                    {
                        control.Visible = false;
                    }
                }
            }
        }

        private void btnUserMaint_Click(object sender, EventArgs e)
        {
            Form frm = new frmUserMaintenance();
            frm.ShowDialog();
        }

        private void btnHoliday_Click(object sender, EventArgs e)
        {
            Form frm = new frmHolidays();
            frm.ShowDialog();
        }

        private void btnCountryMaint_Click(object sender, EventArgs e)
        {
            Form frm = new frmCountryMaint();
            frm.ShowDialog();
        }

        private void btnDesignerMaint_Click(object sender, EventArgs e)
        {
            Form frm = new frmReviewer();
            frm.ShowDialog();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Edit;
            frmInput = GV.InputForm;
            frmInput.ShowDialog();
            this.SendToBack();
        }

        private void FrmInput_InputDataReady(object sender, InputDataReadyEventArgs e)
        {
            // execute query
            //pass model to frmRequests
            List<RequestModel> rm = new List<RequestModel>();
            switch (GV.MODE)
            {
                case Mode.New:
                    break;
                case Mode.Edit:
                case Mode.Delete:
                    rm = GlobalConfig.Connection.GetRequestByPID(e.SearchString).ToList();
                    break;
                case Mode.Revision:
                    break;
                case Mode.Restore:
                    rm = GlobalConfig.Connection.GetDeletedRecordByPID(e.SearchString).ToList();
                    break;
                case Mode.Clone:
                    break;
                case Mode.DateRangeSearch:
                    break;
                case Mode.Forecast:
                    break;
                case Mode.None:
                    break;
                default:
                    break;
            }
            ManageSearchResults(rm);
        }

        public static void ManageSearchResults(List<RequestModel> rm)
        {
            if (rm.Count == 0)
            {
                MessageBox.Show("No matching records found.");
                return;
            }

            if (rm.Count == 1)
            {
                FC.DisplayRequestForm(rm[0]);
            }
            else
            {
                //Form frmMultiResult = new frmMultiResult(rm);
                GV.MultiResult.dataList = rm;
                FC.SetFormPosition(GV.MultiResult);
                GV.MultiResult.Show();
                GV.MultiResult.BringToFront();
            }
        }



        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmMainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                frmInput.InputDataReady -= FrmInput_InputDataReady;
                frmDateMSO_Picker.DataReadyEvent -= FrmDateMSO_Picker_DataReadyEvent;
                frmDateMSO_Picker.PickerCanceled -= FrmDateMSO_Picker_PickerCanceled;
            }
            catch (Exception)
            {

                throw;
            }
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (FC.isFormOpen("frmLogin"))
            {
                GV.LOGIN.Visible = true;
            }
            else
            {
                Form frmLogin = new frmLogin();
                frmLogin.Show();

                //put in global variable so it can be made visible later
                GV.LOGIN = frmLogin;
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            GV.USERNAME = null;

            this.Visible = false;
            GV.LOGIN.Visible = true;

        }

        private void btnMyUpdates_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Edit;
            List<RequestModel> requests = GlobalConfig.Connection.GetRequestsForDesignerUpdate(GV.USERNAME.Designer);
            //Form frmMultiResult = new frmMultiResult(requests);
            GV.MultiResult.dataList = requests;
            FC.SetFormPosition(GV.MultiResult);
            GV.MultiResult.BringToFront();
            GV.MultiResult.Show();
        }

        private void btnDateRange_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.DateRangeSearch;
            frmDateRangeSearch frm = new frmDateRangeSearch();
            frm.Show();
        }

        private void btnForecast_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Forecast;
            GV.MultiResult.DoForecast();
            //frmMultiResult frmMultiResult = new frmMultiResult(null);
        }

        private void btnLoadRpt_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Report_DesignerLoadReport;
            frmLoadReport frm = new frmLoadReport();
            frm.ShowDialog();
        }

        private void btnPassword_Click(object sender, EventArgs e)
        {
            frmPasswordChange frm = new frmPasswordChange();
            frm.ShowDialog();
        }

        private void btnNewReq_Click_1(object sender, EventArgs e)
        {
            GV.MODE = Mode.New;

            FC.DisplayRequestForm();
            GV.REQFORM.resetForm();
        }

        private void btnSnapshot_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Report_Snapshot;
            frmSnapahotReport frmSnapahot = new frmSnapahotReport();
            if (!operationCanceled)
            {
                frmSnapahot.Show();
            }
            else
            {
                operationCanceled = false;
                frmSnapahot = null;
            }
        }

        private void FrmDateMSO_Picker_DataReadyEvent(object sender, DataReadyEventArgs e)
        {
            switch (GV.MODE)
            {
                case Mode.New:
                    break;
                case Mode.Edit:
                    break;
                case Mode.Revision:
                    break;
                case Mode.Clone:
                    break;
                case Mode.Delete:
                    break;
                case Mode.Restore:
                    break;
                case Mode.DateRangeSearch:
                    break;
                case Mode.Forecast:
                    break;
                case Mode.Report_OpenRequests:
                    break;
                case Mode.Report_CatMSO:
                    List<ReportCategoryMSOModel> categoryReport = ReportOps.
                        reportCategoryMSOs(e.MSO_s, e.StartDate, e.EndDate);
                    frmReportCatMSO frmReportCatMSO = new frmReportCatMSO();
                    frmReportCatMSO.Report = categoryReport;
                    frmReportCatMSO.Show();
                    break;
                case Mode.Report_Snapshot:
                    List<SnapshotModel> report = ReportOps.GenerateSnapshotReport
                        (e.MSO_s, e.StartDate, e.EndDate);
                    break;
                case Mode.Report_AvgCompletion:
                    List<CompletionTimeModel> completionReport = ReportOps.GenerateCompletionTimeSummary
                        (e.StartDate, e.EndDate, e.MSO_s);
                    break;
                case Mode.Report_ByPriority:
                    DateTime startDate = e.StartDate;
                    DateTime endDate = e.EndDate;
                    List<ReportSalesPriorityModel> PriorityReport = ReportOps.GenerateSalesSummary(startDate, endDate);
                    frmReportSalesPriiority frmReportSalesPriiority = new frmReportSalesPriiority();
                    frmReportSalesPriiority.Report = PriorityReport;
                    frmReportSalesPriiority.Visible = true;
                    frmReportSalesPriiority.Show();
                    frmReportSalesPriiority.TopMost = true;
                    break;
                case Mode.Report_DesignerLoadReport:
                    break;
                case Mode.Report_Overdue:
                    break;
                case Mode.Search:
                    break;
                case Mode.None:
                    break;
                default:
                    break;
            }
        }

        private void frmMainMenu_Activated(object sender, EventArgs e)
        {
            SetButtonVisibility(GV.USERNAME);
            FC.SetFormPosition(this);
        }

        private void btnDeleteRecord_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Delete;
            frmInput.ShowDialog();
        }

        private void btnUndelete_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Restore;
            frmInput = GV.InputForm;
            //frmInput.InputDataReady += FrmInput_InputDataReady;
            frmInput.ShowDialog();
            this.SendToBack();
        }

        private void btnOpenReq_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Report_OpenRequests;
            List<RequestModel> openRequests = GlobalConfig.Connection.GetOpenRequests();
            //frmMultiResult frmMultiResult = new frmMultiResult(openRequests);
            GV.MultiResult.dataList = openRequests;
            GV.MultiResult.Show();
        }

        private void btnOverdue_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Report_Overdue;
            List<RequestModel> overdueRequests = GlobalConfig.Connection.GetOverdueRequests(DateTime.Now);
            //frmMultiResult frmMultiResult = new frmMultiResult(overdueRequests);
            GV.MultiResult.dataList = overdueRequests;
            GV.MultiResult.Show();
        }

        private void btnAvgComp_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Report_AvgCompletion;
            frmCompletionTimeReport frmCompletionTimeReport = new frmCompletionTimeReport();
            GV.PickerForm.ShowDialog();

            if (!operationCanceled)
            {
                try
                {
                    frmCompletionTimeReport.Show();
                }
                catch (Exception)
                {
                    MessageBox.Show("Report canceled");
                }
            }
            else
            {
                operationCanceled = false;      //reset flag
            }
        }

        private void btnCatMSO_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Report_CatMSO;
            ShowPicker();
        }

        private void ShowPicker()
        {
            frmDateMSO_Picker = GV.PickerForm;
            frmDateMSO_Picker.ShowDialog();
            if (operationCanceled)
            {
                operationCanceled = false;
            }
        }

        private void btnReqPriority_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Report_ByPriority;
            ShowPicker();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Search;
            FC.DisplayRequestForm();
            GV.REQFORM.resetForm();
        }




        private void rdoLive_CheckedChanged(object sender, EventArgs e)
        {
            if (!formLoading)
            {
                if (rdoLive.Checked)
                {
                    FC.setDBMode(this, true);
                    setModeTextBox(true);
                }
                else
                {
                    FC.setDBMode(this, false);
                    setModeTextBox(false);
                }
            }
        }

        private void btnUtility_Click(object sender, EventArgs e)
        {
            frmUtility frmUtility = new frmUtility();
            frmUtility.Show();
        }

        private void btnSalespersonMaint_Click(object sender, EventArgs e)
        {
            frmSalesMaint frmSalesMaint = new frmSalesMaint();
            frmSalesMaint.Show();
        }

        private void btnScreens_Click(object sender, EventArgs e)
        {
            Form ScreenForm = new frmScreens();
            ScreenForm.Show();
        }

        private void btnLogSearch_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Log_Search;
            FC.SetFormPosition(GV.LogViewer);
            GV.LogViewer.Show();
        }

        private void btnListCollector_Click(object sender, EventArgs e)
        {
            List<List<(string Field, bool Active)>> ddList = new List<List<(string Field, bool Active)>>();
            Form frmCollect = new frmRequests();
            foreach (Control control in frmCollect.Controls)
            {
                if (control is TableLayoutPanel)
                {
                    TableLayoutPanel tlp = (TableLayoutPanel)control;
                    ddList.AddRange(ReportOps.CollectDropDownLists(tlp));
                }
            }
            ExcelOps.PlaceDDListInExcel(ddList);
        }

        private void btnMSO_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.MSO_Maintenance;
            Form msoForm = new frmMSO();
            FC.SetFormPosition(msoForm);
            msoForm.Show();
        }

        private void btnFE_Maintenance_Click(object sender, EventArgs e)
        {
            frmFE FE_Form = new frmFE();
            FE_Form.Show();
        }

        private void btnReviewMaint_Click(object sender, EventArgs e)
        {
            frmReviewer reviewer = new frmReviewer();
            reviewer.Show();
        }
    }
}


