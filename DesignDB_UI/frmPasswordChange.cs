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
    public partial class frmPasswordChange : Form
    {
        public frmPasswordChange()
        {
            InitializeComponent();
            txtDesigner.Text = GV.USERNAME.Designer;            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnChange_Click(object sender, EventArgs e)
                {
            if (txtConfirm.Text == txtPW.Text)
            {
                DesignDB_Library.Operations.DesignerUpdate.ChangePassword(txtDesigner.Text, txtCurrentPW.Text, txtPW.Text);
                this.Close();
            }
        }

        private void frmPasswordChange_Activated(object sender, EventArgs e)
        {
            txtCurrentPW.Clear();
            txtCurrentPW.Focus();
        }
    }
}
