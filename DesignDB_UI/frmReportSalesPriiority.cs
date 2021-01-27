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
        public frmDateMSO_Picker frmDateMSO_Picker = null;

        public object operations { get; private set; }

        public frmReportSalesPriiority()
        {
            InitializeComponent();
           
            frmDateMSO_Picker = FC.SetDTP_MSO_Picker();           
            frmDateMSO_Picker.DataReadyEvent += FrmDateMSO_Picker_DataReadyEvent;
            frmDateMSO_Picker.PickerCanceled += FrmDateMSO_Picker_PickerCanceled;
            GV.PickerForm.Visible = true;
            //GV.PickerForm.ShowDialog();
        }

        private void FrmDateMSO_Picker_PickerCanceled(object sender, frmDateMSO_Picker.CancelEventArgs e)
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
