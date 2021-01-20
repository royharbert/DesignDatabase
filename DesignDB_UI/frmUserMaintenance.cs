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
    public partial class frmUserMaintenance : Form
    {
        public frmUserMaintenance()
        {
            InitializeComponent();

            refreshUserList();
         

            UserModel user = GlobalConfig.Connection.GetUser(cboUser.Text);
            if ((cboUser.Text != null) && (cboUser.Text != "DesignDB_Library.Models.UserModel"))
            {
                populateBoxes(user);
            }                
        }

        private void cboUser_SelectedIndexChanged(object sender, EventArgs e)
        {

            if ((cboUser.Text != null) && (cboUser.Text != "DesignDB_Library.Models.UserModel"))
            {
                UserModel thisUser = createUserModel();
                UserModel user = GlobalConfig.Connection.GetUser(cboUser.Text);
                populateBoxes(user);
            }
        }            

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            UserModel newUser = new UserModel(cboUser.Text, txtPassword.Text, txtPriviledge.Text, cboActive.Text,txtID.Text);

            GlobalConfig.Connection.AddUser(newUser);

            refreshUserList();
            clearForm();
        }

        private void btnUpdateUser_Click(object sender, EventArgs e)
        {
            UserModel User = new UserModel(cboUser.Text, txtPassword.Text, txtPriviledge.Text, cboActive.Text, txtID.Text);

            GlobalConfig.Connection.UpdateUser(User);
            refreshUserList();
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)           
        {
            int Ival = 0;
            int.TryParse(txtID.Text, out Ival);
            GlobalConfig.Connection.DeleteUser(Ival);
            clearForm();
            refreshUserList();
        }

        private void clearForm()
        {
            cboUser.Text = "";
            txtPassword.Text = "";
            txtPriviledge.Text = "";
            cboActive.Text = "";
            txtID.Text = "";
        }

        private void refreshUserList()
        {
            List<UserModel> Users = new List<UserModel>();
            Users = GlobalConfig.Connection.GetUsers_All();
            cboUser.DataSource = Users;
            cboUser.DisplayMember = "UserName";
        }
        private void populateBoxes(UserModel User)
        {
            if (cboUser.Text != "")
            {
                cboUser.Text = User.UserName;
                txtPassword.Text = User.PW;
                txtPriviledge.Text = User.Priviledge.ToString();
                cboActive.Text = User.ActiveDesigner.ToString();
                txtID.Text = User.Id.ToString();
            }

        }
        private UserModel createUserModel()
        {
            UserModel User = new UserModel(cboUser.Text, txtPassword.Text, txtPriviledge.Text, cboActive.Text, txtID.Text);
            return User;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearForm();
        }
    
    }
}
