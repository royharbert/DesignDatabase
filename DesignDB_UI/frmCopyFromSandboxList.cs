using DesignDB_Library;
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
    public partial class frmCopyFromSandboxList : Form
    {
        List<string> pidList = new List<string>();
        public frmCopyFromSandboxList()
        {
            InitializeComponent();
            lstPIDs.DataSource = pidList;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtPID.Text != "")
            {
                pidList.Add(txtPID.Text);
                lstPIDs.DataSource = null;
                lstPIDs.DataSource = pidList;
                txtPID.Clear();
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            List<RequestModel> requests = new List<RequestModel>();

            //get requests from Sandbox
            FC.setDBMode(this, false);

            foreach (var pid in pidList)
            {
                List<RequestModel> request = GlobalConfig.Connection.GetRequestByPID(pid);
                if(request.Count > 0)
                {
                    requests.Add(request[0]);
                    lstRequests.DataSource = null;
                    lstRequests.DataSource = requests;
                    lstRequests.DisplayMember = "ProjectID";
                }
            }

            FC.setDBMode(this, true);
            foreach (var item in requests)
            {
                RequestOps.InsertNewRequest(item);
            }
        }
    }
}
