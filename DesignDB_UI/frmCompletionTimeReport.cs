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
using Excel = Microsoft.Office.Interop.Excel;

namespace DesignDB_UI
{
    public partial class frmCompletionTimeReport : Form
    {
        frmDateMSO_Picker frmDateMSO_Picker = GV.PickerForm;

        public frmCompletionTimeReport()
        {
            InitializeComponent();
            
            frmDateMSO_Picker.DataReadyEvent += FrmDateMSO_Picker_DataReadyEvent;        
        }

 

        private void FrmDateMSO_Picker_DataReadyEvent(object sender, DataReadyEventArgs e)
        {
            List<CompletionTimeModel> report = ReportOps.GenerateCompletionTimeSummary(e.StartDate, e.EndDate, e.MSO_s);
            dgvCompletion.DataSource = report;
            ReportOps.FormatCompletionTimeDGV(dgvCompletion);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Export;
            ListLooper.ExcelExporter<CompletionTimeModel> exporter = new ListLooper.ExcelExporter<CompletionTimeModel>();
            exporter.List = (List<CompletionTimeModel>)dgvCompletion.DataSource;
            ReportOps.formatCompletionTimeExport(exporter.Wksheet);
        }

        private void frmCompletionTimeReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            //frmDateMSO_Picker.DataReadyEvent -= FrmDateMSO_Picker_DataReadyEvent;
            //frmDateMSO_Picker.PickerCanceled -= FrmDateMSO_Picker_PickerCanceled;
        }
    }
}
