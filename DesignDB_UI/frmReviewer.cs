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
    public partial class frmReviewer : Form
    {
        List<DesignersReviewersModel> aList;
        List<DesignersReviewersModel> cList;
        public frmReviewer()
        {
            InitializeComponent();

            if (GV.MODE == Mode.DesignerMaintenance)
            {
                cList = GlobalConfig.Connection.GetAllDesigners();
                this.Text = "Designer Maintenance";
                label1.Text = "Designers";
                btnAdd.Text = "Add Designer";
                btnUpdate.Text = "Update Designer";
                btnDelete.Text = "Delete Designer";
;            }
            else
            {
                cList = GlobalConfig.Connection.Reviewers_GetAll();
                this.Text = "Reviewer Maintenance";
                label1.Text = "Reviewers";
                btnAdd.Text = "Add Reviewer";
                btnUpdate.Text = "Update Reviewer";
                btnDelete.Text = "Delete Reviewer";
            }

            setlbxDatasource();
        }

        private void make_aList()
        {
            aList = new List<DesignersReviewersModel>();
            new List<DesignersReviewersModel>();
            cList = GlobalConfig.Connection.GenericGetAll<DesignersReviewersModel>("tblReviewers", "Designer");

            foreach (DesignersReviewersModel dm in cList)
            {
                if (GV.MODE == Mode.ReviewerMaintenance)
                {
                    if (dm.ActiveReviewer)
                    {
                        aList.Add(dm);
                    }
                }
                else
                {
                    if (dm.ActiveDesigner)
                    {
                        aList.Add(dm);                    
                    }
                }
            }      
        }
        private void refreshLBox()
        {
            lbxDesigner.DataSource = null;
            lbxDesigner.DataSource = aList;
            lbxDesigner.DisplayMember = "Reviewer";
        }

        private void lbxDesigner_SelectedIndexChanged(object sender, EventArgs e)
        {
            DesignersReviewersModel dm = (DesignersReviewersModel)lbxDesigner.SelectedItem;
            if (dm != null)
            {
                txtDesigner.Text = dm.Designer;
                txtPassword.Text = dm.Pwd;
                txtPriviledge.Text = dm.Priviledge.ToString();
                if (GV.MODE == Mode.DesignerMaintenance)
                {
                    ckbActive.Checked = dm.ActiveDesigner;
                }
                else
                {
                    ckbActive.Checked = dm.ActiveReviewer;
                }
                txtID.Text = dm.ID.ToString();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DesignersReviewersModel dm = new DesignersReviewersModel(txtDesigner.Text, txtPassword.Text, txtPriviledge.Text, ckbActive.Checked.ToString(),txtID.Text); ;
            dm.Designer = txtDesigner.Text;           
            GlobalConfig.Connection.AddDesigner(dm, "tblReviewers");
            MessageBox.Show(txtDesigner.Text + " Added");
            clearBoxes();
            setlbxDatasource();
        }

        private void clearBoxes()
        {
            txtDesigner.Clear();
            txtPassword.Clear();
            txtPriviledge.Clear();
            ckbActive.Checked = false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearBoxes();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DesignersReviewersModel dm = new DesignersReviewersModel
                (txtDesigner.Text, txtPassword.Text, txtPriviledge.Text, ckbActive.Checked.ToString(),txtID.Text);
            GlobalConfig.Connection.DeleteDesigner(dm);
            MessageBox.Show(txtDesigner.Text + " removed");
            setlbxDatasource();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DesignersReviewersModel dm = new DesignersReviewersModel
                (txtDesigner.Text, txtPassword.Text, txtPriviledge.Text, ckbActive.Checked.ToString(),txtID.Text);
            GlobalConfig.Connection.UpdateDesigner(dm, "tblReviewers");
            MessageBox.Show(txtDesigner.Text + " updated");
            setlbxDatasource();
        }

        private void ckbShowOnlyActive_CheckedChanged(object sender, EventArgs e)
        {
            setlbxDatasource();
        }

        private void setlbxDatasource()
        {
            make_aList(); 
            lbxDesigner.DataSource = null;
            lbxDesigner.DisplayMember = "Designer";
            lbxDesigner.Items.Clear();
            if (ckbShowOnlyActive.Checked)
            {
                lbxDesigner.DataSource = aList;
               
            }
            else
            {
                lbxDesigner.DataSource = cList;                
            }

            lbxDesigner.SelectedIndex = -1;           
        }
    }
}
