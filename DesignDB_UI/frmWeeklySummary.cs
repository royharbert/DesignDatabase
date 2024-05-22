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
            SummaryModel summary = ReportOps.DoWeeklySummary(dtpStart.Value, dtpEnd.Value);
            txtBacklog.Text = summary.Backlog.ToString();
            txtCompletedForPeriod.Text = summary.RequestsCompleted.ToString();
            txtRequestsForPeriod.Text = summary.RequestsInPeriod.ToString();
            txtYTDtotal.Text = summary.YTDassigned.ToString();
            txtYTDvalue.Text = summary.YTDvalue.ToString("###,###,###,###");
        }
    }
}
