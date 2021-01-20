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
        public frmDateMSO_Picker()
        {
            InitializeComponent();
            this.TopMost = true;
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
            this.Close();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            DataReadyEventArgs args = new DataReadyEventArgs();
            GlobalConfig.Connection.ClearTable("tblSnapshotMSO_S");
            List<MSO_Model> msoList = new List<MSO_Model>();
            foreach (MSO_Model model in lbMSO.SelectedItems)
            {
                msoList.Add(model);
                GlobalConfig.Connection.UpdateSnapshotMSO_s(model.MSO);
            }
            args.MSO_s = msoList;
            args.StartDate = dtpStart.Value;
            args.EndDate = dtpStop.Value;
            DataReadyEvent?.Invoke(this, args);
            this.Close();
        }
    }

    public class CancelEventArgs : EventArgs
    {
        public bool PickerCanceled { get; set; }
    }

    public class DataReadyEventArgs : EventArgs
    {
        public List<MSO_Model> MSO_s { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
