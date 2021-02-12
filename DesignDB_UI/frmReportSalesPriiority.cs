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
        private List<ReportSalesPriorityModel> _report = null;
        public List<ReportSalesPriorityModel> Report 
        {
            set
            {
                _report = value;
                dgvReport.DataSource = _report;
                ReportOps.formatPriorityDGV(dgvReport);
            } 
        }

        public object operations { get; private set; }

        public frmReportSalesPriiority()
        {
            InitializeComponent();       
        }

        private void FrmDateMSO_Picker_PickerCanceled(object sender, frmDateMSO_Picker.CancelEventArgs e)
        {
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ListLooper.ExcelExporter<ReportSalesPriorityModel> exporter = new ListLooper.ExcelExporter<ReportSalesPriorityModel>();
            exporter.List = (List<ReportSalesPriorityModel>)dgvReport.DataSource;
            ReportOps.FormatRequestPriorityExport(exporter.Wksheet);
        }
    }
}
