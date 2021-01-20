using DesignDB_Library.Models;
using DesignDB_Library.Operations;
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
    public partial class frmReportSalesPriiority : Form
    {
        frmDateMSO_Picker frmDateMSO_Picker = new frmDateMSO_Picker();

        public object ooperations { get; private set; }

        public frmReportSalesPriiority()
        {
            InitializeComponent();
            frmDateMSO_Picker.DataReadyEvent += FrmDateMSO_Picker_DataReadyEvent;
            frmDateMSO_Picker.PickerCanceled += FrmDateMSO_Picker_PickerCanceled;
            frmDateMSO_Picker.Show();
        }

        private void FrmDateMSO_Picker_PickerCanceled(object sender, CancelEventArgs e)
        {
            this.Close();
        }

        private void FrmDateMSO_Picker_DataReadyEvent(object sender, DataReadyEventArgs e)
        {
            DateTime startDate = e.StartDate;
            DateTime endDate = e.EndDate;
            List<ReportSalesPriorityModel> report = DesignDB_Library.Operations.ReportOps.GenerateSalesSummary(startDate, endDate);
            dgvReport.DataSource = report;
            DesignDB_Library.Operations.ReportOps.formatPriorityDGV(dgvReport);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmReportSalesPriiority_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                frmDateMSO_Picker.DataReadyEvent -= FrmDateMSO_Picker_DataReadyEvent;
            }
            catch (Exception)
            {

                
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ListLooper.ExcelExporter<ReportSalesPriorityModel> exporter = new ListLooper.ExcelExporter<ReportSalesPriorityModel>();
            exporter.List = (List<ReportSalesPriorityModel>)dgvReport.DataSource;
            ReportOps.FormatRequestPriorityExport(exporter.Wksheet);
        }
    }
}
