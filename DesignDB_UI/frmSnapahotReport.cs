using DesignDB_Library;
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
    public partial class frmSnapahotReport : Form
    {
        frmDateMSO_Picker frmDateMSO_Picker = null;
        List<SnapshotModel> report = new List<SnapshotModel>();
        public frmSnapahotReport()
        {
            InitializeComponent();
            frmDateMSO_Picker = ShowDateMSO_Picker();
            frmDateMSO_Picker.DataReadyEvent += FrmDateMSO_Picker_DataReadyEvent;
            frmDateMSO_Picker.ShowDialog();
        }

        private void FrmDateMSO_Picker_DataReadyEvent(object sender, DataReadyEventArgs e)
        {
            report = DesignDB_Library.Operations.ReportOps.GenerateSnapshotReport
                (e.MSO_s, e.StartDate, e.EndDate);
            dgvSnapshots.DataSource = report;
            formatDGV();
        }

        private frmDateMSO_Picker ShowDateMSO_Picker()
        {
            frmDateMSO_Picker returnForm = null;
            foreach (Form form in Application.OpenForms)
            {
                if (form.Name=="frmDateMSO_Picker")
                {
                    returnForm = (frmDateMSO_Picker)form;
                }
            }
            if (returnForm==null)
            {
                returnForm = new frmDateMSO_Picker();
            }
            return returnForm;
        }

        private void frmSnapahotReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                frmDateMSO_Picker.DataReadyEvent -= FrmDateMSO_Picker_DataReadyEvent;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void formatDGV()
        {
            ReportOps.FormatSnapshotDGV(dgvSnapshots);
          
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DesignDB_Library.Operations.ListLooper.ExcelExporter<SnapshotModel> exporter = 
                new DesignDB_Library.Operations.ListLooper.ExcelExporter<SnapshotModel>();
            exporter.List = report;
            Excel.Worksheet wks = exporter.Wksheet;
            ReportOps.FormatSnapshotExport(wks);
        }
    }
}
