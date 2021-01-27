using DesignDB_Library;
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
    public partial class frmDateMSO_Picker : Form
    {
        public event EventHandler<DataReadyEventArgs> DataReadyEvent;
        public event EventHandler<CancelEventArgs> PickerCanceled;
        public frmDateMSO_Picker(bool rollupVisible = false)
        {
            InitializeComponent();
            GV.PickerForm = this;
            //this.BringToFront();
            if (GV.MODE == Mode.Report_ByPriority)
            {
                this.Height = 236;
                lbMSO.Visible = false;
            }
            else
            {
                this.Height = 822;
                lbMSO.Visible = true;
            }
            List<MSO_Model> mso_s = GlobalConfig.Connection.GetAllMSO();
            List<string> msoList = GlobalConfig.Connection.GetSnapshotMSO_s();
            lbMSO.DataSource = mso_s;
            lbMSO.DisplayMember = "MSO";
            lbMSO.SelectedIndex = -1;

            for (int i = 0; i < mso_s.Count; i++)
            {
                MSO_Model msoModel = (MSO_Model)lbMSO.Items[i];
                string msoName = msoModel.MSO;

                if (msoList.Contains(msoName))
                {
                    lbMSO.SetSelected(i, true);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelEventArgs args = new CancelEventArgs();
            args.PickerCanceled = true;
            PickerCanceled?.Invoke(this, args);
            this.Hide();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            List<MSO_Model> msoList = new List<MSO_Model>();
            DataReadyEventArgs args = new DataReadyEventArgs();
            if (!ckAll.Checked)
            {
                GlobalConfig.Connection.ClearTable("tblSnapshotMSO_S");
                foreach (MSO_Model model in lbMSO.SelectedItems)
                {
                    msoList.Add(model);
                    GlobalConfig.Connection.UpdateSnapshotMSO_s(model.MSO);
                }
            }
            else
            {
                msoList = GlobalConfig.Connection.GetAllMSO();
            }
            args.MSO_s = msoList;
            args.StartDate = dtpStart.Value;
            args.EndDate = dtpStop.Value;
            DataReadyEvent?.Invoke(this, args);
            this.Close();
        }

        private void ckAll_CheckedChanged(object sender, EventArgs e)
        {
            if (ckAll.Checked)
            {
                for (int i = 0; i < lbMSO.Items.Count; i++)
                {
                    lbMSO.SetSelected(i, true);
                }
            }
            else
            {
                lbMSO.Items.Clear();
            }
        }

        public class CancelEventArgs : EventArgs
        {
            public bool PickerCanceled { get; set; }
        }

        private void dtpStart_ValueChanged(object sender, EventArgs e)
        {
            dtpStop.Value = dtpStart.Value.AddDays(7);
        }
    }

    public class DataReadyEventArgs : EventArgs
        {
            public List<MSO_Model> MSO_s { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }
}

