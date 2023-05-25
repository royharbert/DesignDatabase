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

        bool allSelected;
        public frmDateMSO_Picker(bool rollupVisible = false)
        {
            InitializeComponent();
            GV.PickerForm = this;
            //this.BringToFront();
            if (GV.MODE == Mode.Report_Rollup)
            {
                lbMSO.SelectionMode = SelectionMode.One;
            }
            else
            {
                lbMSO.SelectionMode = SelectionMode.MultiSimple;
            }
            if (GV.MODE == Mode.Report_ByPriority)
            {
                //this.Height = 236;
                lbMSO.Visible = false;
            }
            else
            {
                //this.Height = 822;
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
            if (GV.MODE!=Mode.Report_Rollup)
            {
                if (!allSelected)
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
            }
            

            if (GV.MODE == Mode.Report_Rollup)
            {
                if (lbMSO.SelectedItems.Count == 1)
                {
                    if (!allSelected)
                    {                        
                        msoList.Add(lbMSO.SelectedItem as MSO_Model); 
                    }
                }
                else
                {
                    if (allSelected)
                    {
                        msoList = null; 
                    }
                }
            }
            args.MSO_s = msoList;
            args.StartDate = dtpStart.Value;
            args.EndDate = dtpStop.Value;
            this.Hide();
            allSelected = false;
            DataReadyEvent?.Invoke(this, args);
        }

        private void ckAll_CheckedChanged(object sender, EventArgs e)
        {
            if (allSelected)
            {
                allSelected = true;
                for (int i = 0; i < lbMSO.Items.Count; i++)
                {
                    lbMSO.SetSelected(i, true);
                }
            }
            else
            {
                allSelected = false;
                lbMSO.Items.Clear();
            }
        }

        public class CancelEventArgs : EventArgs
        {
            public bool PickerCanceled { get; set; }
        }

        private void dtpStart_ValueChanged(object sender, EventArgs e)
        {
            if (GV.MODE != Mode.Report_AvgCompletion)
            {
                dtpStop.Value = dtpStart.Value.AddDays(7); 
            }
        }

        private void ckClear_CheckedChanged(object sender, EventArgs e)
        {
            lbMSO.SelectedItems.Clear();
        }

        private void frmDateMSO_Picker_Activated(object sender, EventArgs e)
        {
            if (GV.MODE == Mode.Report_Rollup)
            {
                lbMSO.SelectedItems.Clear();
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            allSelected = true;
            for (int i = 0; i < lbMSO.Items.Count; i++)
            {
                lbMSO.SetSelected(i, true);
            }
        }

        private void btnDeselect_Click(object sender, EventArgs e)
        {
            lbMSO.ClearSelected();
            allSelected  = false;
        }
    }

    public class DataReadyEventArgs : EventArgs
        {
            public List<MSO_Model> MSO_s { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }
}

