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
        private List<ReportCategoryMSOModel> _report = null;

        public List<ReportCategoryMSOModel> Report 
        {
            set
            {
                _report = value;
                dgvReport.DataSource = _report;
                ReportOps.FormatCatMSO_DGV(dgvReport);
            }
        }
        public frmReportCatMSO()
        {
            InitializeComponent();
        }

        //private void FrmDateMSO_Picker_PickerCanceled(object sender, frmDateMSO_Picker.CancelEventArgs e)
        //{
        //    this.Close();
        //}

        //private void FrmDateMSO_Picker_DataReadyEvent(object sender, DataReadyEventArgs e)
        //{
        //    report = DesignDB_Library.Operations.ReportOps.reportCategoryMSOs(e.MSO_s, e.StartDate, e.EndDate);
        
        //}

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ListLooper.ExcelExporter<ReportCategoryMSOModel> exporter =
                new ListLooper.ExcelExporter<ReportCategoryMSOModel>();
            exporter.List = _report;
            Excel.Worksheet wks = exporter.Wksheet;
            ReportOps.FormatCatMSO_Export(wks);
        }
    }
}
