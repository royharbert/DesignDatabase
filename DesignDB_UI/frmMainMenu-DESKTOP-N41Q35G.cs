using DesignDB_Library;
using DesignDB_Library.Models;
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
        frmDateMSO_Picker frmDateMSO_Picker = GV.PickerForm;

        public frmMainMenu()
        {
            //frmDateMSO_Picker = FC.DisplayPicker();
            frmInput.InputDataReady += FrmInput_InputDataReady;
            InitializeComponent();
            AddVersionNumber();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            CheckForUpdates();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private void AddVersionNumber()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            this.Text += $" v.{ versionInfo.FileVersion }";
        }

        private async Task CheckForUpdates()
        {
            using (var manager = new UpdateManager(@"\\sccacve1\Databases\DesignDB\Beta"))
            {                
                await manager.UpdateApp();
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
                if (control is Button)
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
            Form frm = new frmDesigner();
            frm.ShowDialog();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Edit;
            frmInput = DisplayInputForm();
            frmInput.InputDataReady += FrmInput_InputDataReady;
            frmInput.Show();
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
                Form frmMultiResult = new frmMultiResult(rm);
                frmMultiResult.Show();
            }
        }



        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                frmInput.InputDataReady -= FrmInput_InputDataReady;
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
                GV.LOGIN = this;
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
            Form frmMultiResult = new frmMultiResult(requests);
            frmMultiResult.Show();
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
            frmMultiResult frmMultiResult = new frmMultiResult(null);
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

        private frmDateMSO_Picker ShowDateMSO_Picker()
        {
            //frmDateMSO_Picker returnForm = null;
            //foreach (Form form in Application.OpenForms)
            //{
            //    if (form.Name == "frmDateMSO_Picker")
            //    {
            //        returnForm = (frmDateMSO_Picker)form;
            //    }
            //}
            //if (returnForm == null)
            //{
            //    returnForm = new frmDateMSO_Picker();
            //}
            //return returnForm;
            return FC.DisplayPicker();
        }

        private void btnSnapshot_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Report_Snapshot;
            frmSnapahotReport frmSnapahot = new frmSnapahotReport();
            frmSnapahot.Show();
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
                    break;
                case Mode.Report_Snapshot:
                    List<SnapshotModel> report = DesignDB_Library.Operations.ReportOps.GenerateSnapshotReport
                        (e.MSO_s, e.StartDate, e.EndDate);
                    break;
                case Mode.Report_AvgCompletion:
                    break;
                case Mode.Report_ByPriority:
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
        }

        private void btnDeleteRecord_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Delete;
            frmInput = DisplayInputForm();
            frmInput.InputDataReady += FrmInput_InputDataReady;

            frmInput.ShowDialog();
        }
        private frmInput DisplayInputForm()
        {
            frmInput returnForm = null;
            foreach (Form form in Application.OpenForms)
            {
                if (form.Name == "frmInput")
                {
                    returnForm = (frmInput)form;
                }
            }
            if (returnForm == null)
            {
                returnForm = new frmInput();
            }
            return returnForm;
        }

        private void btnUndelete_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Restore;
            frmInput = DisplayInputForm();
            frmInput.InputDataReady += FrmInput_InputDataReady;
            frmInput.ShowDialog();
        }

        private void btnOpenReq_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Report_OpenRequests;
            List<RequestModel> openRequests = GlobalConfig.Connection.GetOpenRequests();
            frmMultiResult frmMultiResult = new frmMultiResult(openRequests);
            frmMultiResult.Show();
        }

        private void btnOverdue_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Report_Overdue;
            List<RequestModel> overdueRequests = GlobalConfig.Connection.GetOverdueRequests(DateTime.Now);
            frmMultiResult frmMultiResult = new frmMultiResult(overdueRequests);
            frmMultiResult.Show();
        }

        private void btnAvgComp_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Report_AvgCompletion;
            frmCompletionTimeReport frmCompletionTimeReport = new frmCompletionTimeReport();
            try
            {
                frmCompletionTimeReport.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("Report canceled");
            }
        }

        private void btnCatMSO_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Report_CatMSO;
            frmReportCatMSO frmReportCatMSO = new frmReportCatMSO();
            try
            {
                frmReportCatMSO.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("Report canceled.");
            }

        }

        private void btnReqPriority_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Report_ByPriority;
            frmReportSalesPriiority frmReportSalesPriiority = new frmReportSalesPriiority();
            frmReportSalesPriiority.Show();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Search;
            FC.DisplayRequestForm();
            GV.REQFORM.resetForm();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            frmSalesMaint frmSalesMaint = new frmSalesMaint();
            frmSalesMaint.Show();
        }
    }
}

