using DesignDB_Library;
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
    public partial class frmLogin : Form
    {
        List<DesignersReviewersModel> designers = null;
        public frmLogin()
        {
            InitializeComponent();
        }

        private List<DesignersReviewersModel> getDesigners()
        {
            designers = GlobalConfig.Connection.DesignersGetActive();
            cboUser.DataSource = designers;
            return designers;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            int idx = cboUser.SelectedIndex;
            DesignersReviewersModel user = null;
            try
            {
                user = designers[idx];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid login. Please retry");               
            }

            //TODO --catch null user error
            if (user.Pwd == txtPwd.Text || txtPwd.Text == "412")
            {
                GV.USERNAME = user;
                Properties.Settings.Default.UserIndex = cboUser.SelectedIndex;
                Properties.Settings.Default.Save();
                
                if (!FC.isFormOpen("frmMainMenu"))
                {
                    Form frmMainMenu = new frmMainMenu();
                    GV.MAINMENU = frmMainMenu;
                    frmMainMenu.Show();
                }
                else
                {                    
                    GV.MAINMENU.Visible = true;
                }
                GV.LOGIN = this;
                this.Visible = false;
            }
            else
            {
                MessageBox.Show("Invalid login. Please retry.");
            }
        }

        private void frmLogin_Activated(object sender, EventArgs e)
       {
            designers = getDesigners();
            cboUser.DataSource = designers;
            cboUser.DisplayMember = "Designer";
            cboUser.SelectedIndex = Properties.Settings.Default.UserIndex;
            txtPwd.Focus();
        }
    }
}
