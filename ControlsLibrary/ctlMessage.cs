using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlsLibrary
{
    public partial class ctlMessage : UserControl
    {
        private string _msg = "";

        public string Msg
        {
            get
            {
                return _msg;
            }
            set
            {
                _msg = value;
                lblMsg.Text = _msg;
            }
        }

        public ctlMessage()
        {
            InitializeComponent();
        }
    }
}
