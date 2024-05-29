using DesignDB_Library.Models;
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
using DesignDB_Library.Operations;

namespace DesignDB_UI
{
    public partial class frmWeeklySummary : Form
    {
        SummaryModel summary = new SummaryModel();
        public frmWeeklySummary()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCollectInfo_Click(object sender, EventArgs e)
        {
            summary = ReportOps.DoWeeklySummary(dtpStart.Value, dtpEnd.Value);
            txtBacklog.Text = summary.Backlog.ToString();
            txtCompletedForPeriod.Text = summary.RequestsCompleted.ToString();
            txtRequestsForPeriod.Text = summary.RequestsInPeriod.ToString();
            txtYTDtotal.Text = summary.YTDassigned.ToString();
            txtYTDvalue.Text = summary.YTDvalue.ToString("###,###,###,###");
        }

        private void frmWeeklySummary_Load(object sender, EventArgs e)
        {
            FC.SetFormPosition(this);
            DateTime startDate = dtpEnd.Value.AddDays(-7);
            dtpStart.Value = startDate;
        }

        private void btnClipboard_Click(object sender, EventArgs e)
        {
            //SummaryModel model = new SummaryModel();
            //model.YTDassigned = 555;
            //model.YTDvalue = 100000000;
            //model.RequestsInPeriod = 45;
            //model.RequestsCompleted = 30;
            //model.Backlog = 20;

            ReportOps.CopyWeeklySummaryToClipboard(summary);
        }
    }
}
