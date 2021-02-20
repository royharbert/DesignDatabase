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
    public partial class frmInput : Form
    {
        public event EventHandler<InputDataReadyEventArgs> InputDataReady;       

        public frmInput()
        {
            InitializeComponent();
            this.AcceptButton = btnGo;
            this.CancelButton = btnCancel;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGo_Click(object sender, EventArgs e)        
        {
            InputDataReadyEventArgs args = new InputDataReadyEventArgs();
            args.SearchString = "%" + txtInput.Text + "%";
            InputDataReady?.Invoke(this, args);
            this.Close();
        }

        private void frmInput_Activated(object sender, EventArgs e)
        {
            txtInput.Clear();
        }
    }

    public class InputDataReadyEventArgs : EventArgs
    {
        public string SearchString { get; set; }
    }
}
