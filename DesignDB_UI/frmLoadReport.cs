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

namespace DesignDB_UI
{
    public partial class frmLoadReport : Form
    {
        List<DesignerLoadModel> load =new List<DesignerLoadModel>();
        public frmLoadReport()
        {
            load = GlobalConfig.Connection.DoLoadReport();
            InitializeComponent();
            dgvLoad.DataSource = load;
            ReportOps.FormatDesignerLoadDGV(dgvLoad);
        }

        private void btnClosr_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ListLooper.ExcelExporter<DesignerLoadModel> exporter = new ListLooper.ExcelExporter<DesignerLoadModel>();
            exporter.List = (List<DesignerLoadModel>)dgvLoad.DataSource;
            ReportOps.FormatDesignerLoadExport(exporter.Wksheet);
        }
    }
}
