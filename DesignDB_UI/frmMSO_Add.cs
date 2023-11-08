using DesignDB_Library;
using DesignDB_Library.Models;
using Microsoft.Office.Interop.Excel;
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
    public partial class frmMSO_Add : Form
    {
        public frmMSO_Add()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.AddMSO;
            btnSave.Enabled = true;
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            cboMSO.Enabled = true;
            cboMSO.DataSource = null;
            cboMSO.Items.Clear();
            cboMSO.SelectedIndex = -1;
            txtTLA.Clear();
            cboTier.SelectedIndex = -1;
            ckActive.Checked = false;
            cboMSO.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.EditMSO;
            btnSave.Enabled = true;
            btnAdd.Enabled = false;
            btnEdit.Enabled = false;
            cboMSO.Enabled = true;
            lblAction.Text = "Edit MSO";
            List<MSO_Model> msoList = GlobalConfig.Connection.GenericGetAll<MSO_Model>("tblMSO", "MSO");
            cboMSO.DataSource = msoList;
            cboMSO.DisplayMember = "MSO";
            cboMSO.SelectedIndex = -1;
            cboMSO.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int dbID = -1;
            if (GV.MODE == Mode.AddMSO)
            {
                if (cboMSO.Text != "" & txtTLA.Text != "")
                {
                    dbID = GlobalConfig.Connection.MSO_Add(cboMSO.Text, txtTLA.Text, true, Convert.ToInt16(cboTier.Text));
                }
                else
                {
                    MessageBox.Show("Please enter information in both boxes.");
                }
                if (dbID != -1)
                {
                    txtID.Text = dbID.ToString();
                    MessageBox.Show(cboMSO.Text + " has been added.");
                }
                else
                {
                    MessageBox.Show(cboMSO.Text + " has not been added.", "DB Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            bool updated = false;
            if (GV.MODE == Mode.EditMSO)
            {
                MSO_Model deltaMSO = new MSO_Model();
                deltaMSO.Active = ckActive.Checked;
                deltaMSO.Abbreviation = txtTLA.Text;
                deltaMSO.Tier = cboTier.SelectedIndex + 1;
                deltaMSO.MSO = cboMSO.Text;
                deltaMSO.ID = Convert.ToInt16(txtID.Text);
                updated = GlobalConfig.Connection.MSO_Update(deltaMSO);

                if (updated)
                {
                    MessageBox.Show(cboMSO.Text + " Updated");
                }
                else
                {
                    MessageBox.Show(cboMSO.Text + " update failed");
                }
            }
            btnAdd.Enabled = true;
            btnEdit.Enabled = true;
            btnSave.Enabled = false;
            cboMSO.Enabled = false;
            cboMSO.SelectedIndex = -1;

            txtTLA.Clear();
            txtID.Clear();
            cboMSO.Text = "";
            cboTier.SelectedIndex = -1;
            ckActive.Checked = false;
        }

        private void cboMSO_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMSO.SelectedIndex > 0)
            {
                MSO_Model mso = (MSO_Model)cboMSO.SelectedItem;
                txtID.Text = mso.ID.ToString();
                txtTLA.Text = mso.Abbreviation;
                cboTier.SelectedIndex = mso.Tier - 1;
                ckActive.Checked = mso.Active;
            }
        }

        private void frmMSO_Add_Load(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            cboMSO.Enabled = false;
        }
    }
}