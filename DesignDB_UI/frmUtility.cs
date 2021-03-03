using DesignDB_Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesignDB_UI
{
    public partial class frmUtility : Form
    {
        public frmUtility()
        {
      
      InitializeComponent();
        }

        private void btnCopyRequests_Click(object sender, EventArgs e)
        {
            frmCopyFromSandboxList frmCopyFromSandboxList = new frmCopyFromSandboxList();
            frmCopyFromSandboxList.Show();
        }

        private void btnAttachmentFix_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(GlobalConfig.ConnString("Live"));
        }
    }
}
