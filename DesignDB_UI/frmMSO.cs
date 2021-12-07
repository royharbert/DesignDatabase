using DesignDB_Library.Models;
using DesignDB_Library.Operations;
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
    public partial class frmMSO : Form
    {
        public frmMSO()
        {
            InitializeComponent();
        }

        private void frmMSO_Load(object sender, EventArgs e)
        {
            List<MSO_Model> msoList = DesignDB_Library.GlobalConfig.Connection.GenericGetAll<MSO_Model>("tblMSO");
            CommonOps.MakeMSO_StatusList(msoList, dgvMSO);
            //lbMSO.DataSource = tupList;
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            CommonOps.ToggleMSO_ActiveStatus(dgvMSO);
        }
    }
}
