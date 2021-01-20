using DesignDB_Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DesignDB_Library.Models;
using Excel = Microsoft.Office.Interop.Excel;
using DesignDB_Library.Operations;

namespace DesignDB_UI
{
    public partial class frmReportCatMSO : Form
    {
        List<ReportCategoryMSOModel> report = null;
        frmDateMSO_Picker frmDateMSO_Picker = new frmDateMSO_Picker();
        public frmReportCatMSO()
        {
            InitializeComponent();
            frmDateMSO_Picker.DataReadyEvent += FrmDateMSO_Picker_DataReadyEvent;
            frmDateMSO_Picker.PickerCanceled += FrmDateMSO_Picker_PickerCanceled;
            frmDateMSO_Picker.ShowDialog();
            
        }

        private void FrmDateMSO_Picker_PickerCanceled(object sender, CancelEventArgs e)
        {
            this.Close();
        }

        private void FrmDateMSO_Picker_DataReadyEvent(object sender, DataReadyEventArgs e)
        {
            report = DesignDB_Library.Operations.ReportOps.reportCategoryMSOs(e.MSO_s, e.StartDate, e.EndDate);
            dgvReport.DataSource = report;
            ReportOps.FormatCatMSO_DGV(dgvReport);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmReportCatMSO_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmDateMSO_Picker.DataReadyEvent -= FrmDateMSO_Picker_DataReadyEvent;
            frmDateMSO_Picker.PickerCanceled -= FrmDateMSO_Picker_PickerCanceled;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DesignDB_Library.Operations.ListLooper.ExcelExporter<ReportCategoryMSOModel> exporter =
                new DesignDB_Library.Operations.ListLooper.ExcelExporter<ReportCategoryMSOModel>();
            exporter.List = report;
            Excel.Worksheet wks = exporter.Wksheet;
            ReportOps.FormatCatMSO_Export(wks);
        }
    }
}
