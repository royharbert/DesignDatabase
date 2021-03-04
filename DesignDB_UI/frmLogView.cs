﻿using DesignDB_Library;
using DesignDB_Library.Models;
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
    public partial class frmLogView : Form
    {
        private LogModel logRecord;

        public LogModel LogRecord
        {
            get { return logRecord; }
            set 
            {
                clearForm();
                logRecord = value;
                txtActivity.Text = logRecord.Action;
                txtPID.Text = logRecord.RequestID;
                txtTimeStamp.Text = logRecord.TimeStamp.ToString();
                txtUser.Text = logRecord.User;
                List<string> fields = DesignDB_Library.Operations.
                    Serialization.DeserializeToList<List<string>>(logRecord.AffectedFields);
                lstFields.DataSource = fields;
            }
        }

        public frmLogView()
        {
            InitializeComponent();
            List<DesignersReviewersModel> designers =  GlobalConfig.Connection.DesignersGetActive();
            txtUser.DataSource = designers;
            txtUser.DisplayMember = "Designer";
            txtUser.SelectedIndex = -1;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchValue = "";
            string searchTerm = "";
            foreach (Control control in this.Controls)
            {
                if ((control is TextBox | control is ComboBox) & control.Text != "")
                {
                    searchTerm = control.Tag.ToString();
                    searchValue = control.Text;
                }
            }

            if (searchTerm == "" | searchValue == "")
            {
                MessageBox.Show("Please specify what to search for.");
            }
            else
            {
                List<LogModel> logList = GlobalConfig.Connection.LogList(searchTerm, searchValue);
                frmLogMultiResult Results = new frmLogMultiResult();
                Results.LogResults = logList;
                Results.Show();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearForm();
        }

        private void clearForm()
        {
            txtActivity.Text = "";
            txtActivity.SelectedIndex = -1;

            txtPID.Clear();
            txtTimeStamp.Clear();
            txtUser.SelectedIndex = -1;
            lstFields.DataSource = null;
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            clearForm();
            this.Hide();
        }

        private void frmLogView_FormClosing(object sender, FormClosingEventArgs e)
        {
            GV.MAINMENU.BringToFront();
        }
    }
}
