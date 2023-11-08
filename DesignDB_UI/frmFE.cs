using DesignDB_UI;
using DesignDB_Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DesignDB_Library.Operations;
using DesignDB_Library.TableOps;
using DesignDB_Library;
using Microsoft.Office.Interop.Excel;

namespace DesignDB_UI
{
    public partial class frmFE : Form
    {
        bool formloading = true;
        FE_Model model = null;
        List<FE_Model> feList = new List<FE_Model>();



        public frmFE()
        {
            model = new FE_Model();
            InitializeComponent();
            makeFEList();
            fillRegionCombo();
            formloading = false;
            bool active = true;
            bool inactive = false;

            string active1 = active.ToString();
            string inactive1 = inactive.ToString();
        }

        private void fillRegionCombo()
        {
            List<RegionsModel> regions = GlobalConfig.Connection.GenericGetAll<RegionsModel>("tblRegions", "Region");
            txtFE_Region.DataSource= regions;
            txtFE_Region.DisplayMember = "Region";
            txtFE_Region.SelectedIndex = -1;
        }

        private void makeFEList()
        {
            List<FE_Model> feList = GlobalConfig.Connection.GenericGetAll<FE_Model>("tblFE", "LastName");
            lst_FieldEngineer.DataSource = feList;
            lst_FieldEngineer.DisplayMember = "FullName";
            lst_FieldEngineer.SelectedIndex = -1;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //GlobalConfig.Connection.FE_GetAllActive();
            MessageBox.Show("Field Engineer Information Updated");
        }

        private void chk_viewActive_CheckedChanged(object sender, EventArgs e)
        {
            lstFE_Refresh();
        }

        private void lstFE_Refresh()
        {
            if (chk_viewActive.Checked == true)
            {
                feList = GlobalConfig.Connection.GetItemByColumn<FE_Model>("tblFE", "Active", "", 1);
            }
            else
            {
                feList = GlobalConfig.Connection.GenericGetAll<FE_Model>("tblFE", "LastName");
            }

            lst_FieldEngineer.DataSource = feList;
            lst_FieldEngineer.DisplayMember = "FullName";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lst_FieldEngineer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (! formloading)
            {
                model = (FE_Model)lst_FieldEngineer.SelectedItem;
                fillTextBoxes();
            }
           
        }

        private void fillTextBoxes()
        {
            txtID.Text = model.ID.ToString();
            txtFE_FirstName.Text = model.FirstName;
            txtFE_LastName.Text = model.LastName;
            txtFE_ManagerID.Text = model.ManagerID;
            txtFE_Phone.Text = model.Phone;
            txtFE_Region.Text = model.Region;
            txtFE_Email.Text = model.EMail;
            txtFE_Active.Text = model.Active.ToString();
        }
        private FE_Model loadModel()
        {
            model.FirstName = txtFE_FirstName.Text;
            model.LastName = txtFE_LastName.Text;
            model.ManagerID = txtFE_ManagerID.Text;
            model.Region = txtFE_Region.Text;
            model.Phone = txtFE_Phone.Text;
            model.EMail = txtFE_Email.Text;
            model.Active = bool.Parse(txtFE_Active.Text);

            return model;

        }
        private void btn_markActive_Click(object sender, EventArgs e)
        {
            model = CommonOps.ToggleFE_ActiveStatus(model);
            MessageBox.Show(model.FullName + " Active status has been set to " + model.Active);
            lstFE_Refresh();
            fillTextBoxes();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Are you sure you want to delete this FE?");
            //GlobalConfig.Connection.FE_CRUD(model, 'D');
            lstFE_Refresh();

        }

        private void btn_ClearFields_Click(object sender, EventArgs e)
        {
            foreach (Control ctl in this.Controls)
            {
                if (ctl is System.Windows.Forms.TextBox)
                {
                    ctl.Text = "";
                }
                txtFE_Region.SelectedIndex = -1;
                txtFE_Active.SelectedIndex = -1;
            }
        }

        private void btnFE_Save_Click(object sender, EventArgs e)
      {
            FE_Model model = new FE_Model();
            int id = 0;
            int.TryParse(txtID.Text, out id);
            model.ID = id;
            model.FirstName = txtFE_FirstName.Text;
            model.LastName = txtFE_LastName.Text;
            model.ManagerID = txtFE_ManagerID.Text;
            model.Region = txtFE_Region.Text;
            model.Phone = txtFE_Phone.Text;
            model.EMail = txtFE_Email.Text;
            model.Active = bool.Parse(txtFE_Active.Text);
            
            bool success = GlobalConfig.Connection.FE_Update(model);

            if (success)
            {
                MessageBox.Show("Field Engineer has been updated"); 
            }
            else
            {
                MessageBox.Show("Field Engineer not updated");
            }

        }

        private void btnFE_Add_Click(object sender, EventArgs e)
        {
            FE_Model model = new FE_Model();
            model.FirstName = txtFE_FirstName.Text;
            model.LastName = txtFE_LastName.Text;
            model.ManagerID = txtFE_ManagerID.Text;
            model.Region = txtFE_Region.Text;
            model.Phone = txtFE_Phone.Text;
            model.EMail = txtFE_Email.Text;
            model.Active = bool.Parse(txtFE_Active.Text);

            
            int id = GlobalConfig.Connection.FE_Add(model);
            txtID.Text = id.ToString();
            if (txtID.Text != "-1")
            {
                MessageBox.Show(model.FullName + " has been added.");
            }
            else
            {
                MessageBox.Show(model.FullName + " was not added", "DB Error", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
            lstFE_Refresh();
        }
    }
}
