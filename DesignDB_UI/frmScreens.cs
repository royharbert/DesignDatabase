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
    public partial class frmScreens : Form
    {
        public frmScreens()
        {
            InitializeComponent();
            RadioButton[] rdo = new RadioButton[] { rdoScreen1, rdoScreen2, rdoScreen3, rdoScreen4 };
            for (int i = 0; i < GV.ScreenList.Count; i++)
            {
                rdo[i].Visible = true;
            }
            rdo[GV.ActiveScreen - 1].Checked = true;
            FC.SetFormPosition(this);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void setGV_Screen(RadioButton radiobutton)
        {
            if (radiobutton.Checked)
            {
                int tag = int.Parse(radiobutton.Tag.ToString());
                GV.ActiveScreen = tag;
                Properties.Settings.Default.ActiveScreen = GV.ActiveScreen;
                Properties.Settings.Default.Save();
            }
        }

        private void rdoScreen1_CheckedChanged(object sender, EventArgs e)
        {
            setGV_Screen((RadioButton)sender);
        }

        private void rdoScreen2_CheckedChanged(object sender, EventArgs e)
        {
            setGV_Screen((RadioButton)sender);
        }

        private void rdoScreen3_CheckedChanged(object sender, EventArgs e)
        {
            setGV_Screen((RadioButton)sender);
        }

        private void rdoScreen4_CheckedChanged(object sender, EventArgs e)
        {
            setGV_Screen((RadioButton)sender);
        }
    }
}
