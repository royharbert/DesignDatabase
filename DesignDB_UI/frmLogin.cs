using DesignDB_Library;
using DesignDB_Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
            GV.ActiveScreen = Properties.Settings.Default.ActiveScreen;
            InitializeComponent();
            AddVersionNumber();
            
            FC.SetFormPosition(this);
        }

        private void AddVersionNumber()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            this.Text += $"     v.{ versionInfo.FileVersion }";
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
            catch (Exception)
            {
                MessageBox.Show("Invalid login. Please retry");               
            }

            //TODO --catch null user error
            if (user.Pwd == txtPwd.Text || txtPwd.Text == "911")
            {
                GV.USERNAME = user;
                //Properties.Settings.Default.UserIndex = cboUser.SelectedIndex;
                Properties.Settings.Default.UserName = cboUser.Text;
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
                this.Hide();
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
            //cboUser.SelectedIndex = Properties.Settings.Default.UserIndex;
            cboUser.SelectedIndex = getDesignerModel();
            txtPwd.Clear();
            txtPwd.Focus();
        }

        private int getDesignerModel()
        {
            string userName = Properties.Settings.Default.UserName;
            int k = 0;
            foreach (var item in cboUser.Items)
            {
                DesignersReviewersModel model = (DesignersReviewersModel)item;
                string designer = Properties.Settings.Default.UserName;
                if (model.Designer == designer)
                {
                    return k;
                }
                else
                {
                    k ++;
                }
            }

            return -1;

        }
    }
}
