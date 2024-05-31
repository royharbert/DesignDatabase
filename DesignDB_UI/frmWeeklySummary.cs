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
            summary = ReportOps.DoWeeklySummary(dtpStart.Value.Date, dtpEnd.Value.Date);
            txtBacklog.Text = summary.Backlog.ToString();
            txtCompletedForPeriod.Text = summary.RequestsCompleted.ToString();
            txtRequestsForPeriod.Text = summary.RequestsInPeriod.ToString();
            txtYTDtotal.Text = summary.YTDassigned.ToString();
            txtYTDvalue.Text = summary.YTDvalue.ToString("###,###,###,###");
            btnClipboard.Enabled = true;
            this.AcceptButton = btnClipboard;
        }

        private void frmWeeklySummary_Load(object sender, EventArgs e)
        {
            FC.SetFormPosition(this);
            DateTime startDate = dtpEnd.Value.AddDays(-7);
            dtpStart.Value = startDate;
            btnClipboard.Enabled = false;
        }

        private void btnClipboard_Click(object sender, EventArgs e)
        {
            bool complete = ReportOps.CopyWeeklySummaryToClipboard(summary, dtpEnd.Value.Date.ToString());
            if (complete)
            {
                MessageBox.Show("Data copied to clipboard.");
            }
            this.AcceptButton = btnCollectInfo;
        }

        private void dtpEnd_ValueChanged(object sender, EventArgs e)
        {
            DateTime start = dtpEnd.Value.AddDays(-7).Date;
            dtpStart.Value = start;
        }
    }
}
