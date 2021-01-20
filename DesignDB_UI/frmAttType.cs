using DesignDB_Library.Models;
using GalaSoft.MvvmLight.Messaging;
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

    public partial class frmAttType : Form
    {
        string thisType = "";
        AttachmentModel model = null;
        public event EventHandler<AttachmentModel> TypeReadyEvent;
        public frmAttType(AttachmentModel aModel)
        {
            model = aModel;
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {           
            model.ItemType = thisType;
            
            TypeReadyEvent?.Invoke(this, model);
            this.Close();
        }

        private void rdoDesignReq_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb != null && rb.Checked)
            {
                thisType = rb.Tag.ToString();
            }
        }

        private void rdoBOM_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb != null && rb.Checked)
            {
                thisType = rb.Tag.ToString();
            }
        }

        private void rdoDrawing_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb != null && rb.Checked)
            {
                thisType = rb.Tag.ToString();
            }
        }

        private void rdoPerf_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb != null && rb.Checked)
            {
                thisType = rb.Tag.ToString();
            }
        }

        private void rdoOther_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb != null && rb.Checked)
            {
                thisType = rb.Tag.ToString();
            }
        }
    }
}
