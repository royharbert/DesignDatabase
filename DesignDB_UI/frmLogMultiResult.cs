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
    public partial class frmLogMultiResult : Form
    {
        private List<LogModel> entries;
        bool formLoading;
        bool _useDefaultLocation = true;
        private Point _formLocation;
        public bool UseDefaultLocation
        {
            get
            {
                return _useDefaultLocation;
            }
        }

        public Point FormLocation
        {
            get
            {
                return _formLocation;
            }
        }

        public List<LogModel> LogResults
        {
            set 
            {
                entries = value;
                dgvLog.DataSource = entries;
                ReportOps.FormatLogViewDGV(dgvLog);
            } 
        }
        public frmLogMultiResult()
        {
            formLoading = true;
            InitializeComponent();
            FC.SetFormPosition(this);
            this.BringToFront();
        }

        private void frmLogMultiResult_Load(object sender, EventArgs e)
        {
            formLoading = false;
        }

        private void frmLogMultiResult_Move(object sender, EventArgs e)
        {
            {
                _formLocation = this.Location;
                if (!formLoading)
                {
                    _useDefaultLocation = false;
                }
            }
        }

        private void dgvLog_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int selRow = dgvLog.CurrentRow.Index;
            LogModel logEntry = new LogModel();
            logEntry = (LogModel)entries[selRow];
            GV.LogViewer.LogRecord = logEntry;
            GV.LogViewer.Show();
            GV.LogViewer.BringToFront();
        }

        private void frmLogMultiResult_FormClosing(object sender, FormClosingEventArgs e)
        {
            GV.LogViewer.BringToFront();
        }
    }
}
