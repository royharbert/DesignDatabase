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

namespace DesignDB_UI
{
    public partial class frmSalesMaint : Form
    {
        List<SalespersonModel> activeList = new List<SalespersonModel>();
        List<SalespersonModel> completeList = new List<SalespersonModel>();
        public frmSalesMaint()
        {
            InitializeComponent();            
            setlbxDatasource();
        }


        private void refreshLBox()
        {            
            lbxSalesperson.DataSource = null;
            lbxSalesperson.DisplayMember = "Salesperson";
        }



        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text == "Add Salesperson")
            {
                clearBoxes();
                btnDelete.Enabled = false;
                btnUpdate.Enabled = false;
                btnAdd.Text = "Proceed";
                txtSalesperson.Focus();
            }
            else
            {
                addSalesperson();
            }
        }

        private void addSalesperson()
        {
            btnAdd.Text = "Add Salesperson";
            int ID = 0;            
            SalespersonModel sm = new SalespersonModel(ID, txtSalesperson.Text, ckbActive.Checked); ;
            sm.SalesPerson = txtSalesperson.Text;
            GlobalConfig.Connection.SalespersonAdd(sm);
            MessageBox.Show(txtSalesperson.Text + " Added");
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
            clearBoxes();
            setlbxDatasource();
        }

        private void clearBoxes()
        {
            txtSalesperson.Clear();
            txtID.Clear();
            ckbActive.Checked = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SalespersonModel sm = new SalespersonModel(int.Parse(txtID.Text), txtSalesperson.Text, ckbActive.Checked);
            GlobalConfig.Connection.SalespersonDelete(sm);
            MessageBox.Show(txtSalesperson.Text + " removed");
            setlbxDatasource();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SalespersonModel sm = new SalespersonModel(int.Parse(txtID.Text), txtSalesperson.Text,  ckbActive.Checked);
            GlobalConfig.Connection.SalespersonUpdate(sm);
            MessageBox.Show(txtSalesperson.Text + " updated");
            setlbxDatasource();
        }

        private void ckbShowOnlyActive_CheckedChanged(object sender, EventArgs e)
        {
            setlbxDatasource();
        }

        private void setlbxDatasource()
        {
            completeList = GlobalConfig.Connection.SalespersonsGetAll();
            activeList = GlobalConfig.Connection.SalesGetActive();

            lbxSalesperson.DataSource = null;
            lbxSalesperson.DisplayMember = "Salesperson";
            lbxSalesperson.Items.Clear();
            if (ckbShowOnlyActive.Checked)
            {
                lbxSalesperson.DataSource = activeList;
               
            }
            else
            {
                lbxSalesperson.DataSource = completeList;                
            }

            lbxSalesperson.SelectedIndex = -1;           
        }

        private void lbxSalesperson_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxSalesperson.SelectedIndex >= 0)
            {
                int idx = lbxSalesperson.SelectedIndex;
                SalespersonModel model = (SalespersonModel)lbxSalesperson.SelectedItem;
                txtID.Text = model.Id.ToString();
                txtSalesperson.Text = model.SalesPerson;
                ckbActive.Checked = model.Active;
            }
        }
    }
}
