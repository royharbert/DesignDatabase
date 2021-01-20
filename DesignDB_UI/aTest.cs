using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using DesignDB_Library.Models;
using DesignDB_Library;

namespace DesignDB_UI
{
    public partial class aTest : Form
    {
        public aTest()
        {
            InitializeComponent();
            dtp.Text = DateTime.Parse("1/1/2010").ToShortDateString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<RequestModel> requests = GlobalConfig.Connection.GetOpenRequests();
            DateTime compDate = requests[0].DateCompleted;
            if (compDate == DateTime.MinValue)
            {
                MessageBox.Show("MinDate");
            }
        }
    }
}

          
