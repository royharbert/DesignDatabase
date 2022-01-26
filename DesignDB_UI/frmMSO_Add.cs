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
            if (txtName.Text != "" & txtTLA.Text != "")
            {
                GlobalConfig.Connection.MSO_Add(txtName.Text, txtTLA.Text, true);
                MessageBox.Show(txtName.Text + " Added");
            }
            else
            {
                MessageBox.Show("Please enter information in both boxes.");
            }
        }
    }
}
