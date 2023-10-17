using DeltaCompressionDotNet.MsDelta;
using DesignDB_Library;
using DesignDB_Library.Models;

using DesignDB_Library.Operations;
using Microsoft.ReportingServices.Interfaces;
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
        CheckBox[] ckRegions;
        CheckBox[] ckTiers;
        List<MSO_Model> allMSO_S;
        List<MSO_Model> tier1Models;
        List<MSO_Model> tier2Models;
        List<MSO_Model> tier0Models;
        List<string> tierQuery = new List<string>();
        List<string> regionQuery = new List<string>();

        bool allSelected;
        bool CustomFormat = false;
        NewMessageEventArgs msgArgs = new NewMessageEventArgs();

        public static event EventHandler<NewMessageEventArgs> NewMessageEvent;
        public frmDateMSO_Picker(bool rollupVisible = false)
        {
            InitializeComponent();
            
            GV.PickerForm = this;
            rdo_Normal.Checked = true;
            

            ckRegions = new CheckBox[] { ckAfrica, ckAsia, ckAustralia, ckCanada, ckCaribbean, ckEurope, ckIndia, ckLatinAmerica, ckMiddleEast,
                ckRussia, ckUSEast, ckUSWest, ckOther  };

            ckTiers = new CheckBox[]{ ckTier1, ckTier2, ckUnclassified };
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
            this.Hide();
            tierQuery = createTiersString(ckTiers);
            regionQuery = createRegionsString(ckRegions);
            List<MSO_Model> msoList = new List<MSO_Model>();
            DataReadyEventArgs args = new DataReadyEventArgs();
            if (tierQuery.Count == 0)
            {
                ReportOps.sendMessage("Saving MSO Selections");
                //If no checkboxes are checked
                GlobalConfig.Connection.ClearTable("tblSnapshotMSO_S");
                if (regionQuery.Count == 0)
                {
                    foreach (MSO_Model model in lbMSO.SelectedItems)
                    {
                        msoList.Add(model);
                        GlobalConfig.Connection.UpdateSnapshotMSO_s(model.MSO);
                    } 
                }
                else
                {
                    ReportOps.sendMessage("Retrieving All MSO's");
                    msoList = allMSO_S;   
                }
                args.MSO_s = msoList;
                args.StartDate = dtpStart.Value;
                args.EndDate = dtpStop.Value;
                args.regionQuery = regionQuery;
            }
            else
            {
                msoList = new List<MSO_Model>();
                foreach (var tier in tierQuery)
                {
                    ReportOps.sendMessage("Creating Tier Lists");
                    int tierInt =0;
                    int.TryParse(tier, out tierInt);
                    msoList.AddRange(allMSO_S.Where(x => x.Tier == tierInt).ToList());

                    //Highlight MSOs from tier query
                    for (int i = 0; i < allMSO_S.Count; i++)
                    {
                        MSO_Model msoModel = (MSO_Model)lbMSO.Items[i];
                        string msoName = msoModel.MSO;

                        if (msoList.Contains(msoModel))
                        {
                            lbMSO.SetSelected(i, true);
                        }
                    }
                    msoList = new List<MSO_Model>();
                    MSO_Model msoItem;
                    foreach (var item in lbMSO.SelectedItems)
                    {
                        msoItem = (MSO_Model)item;
                        msoList.Add(msoItem);
                    }
                }
                clearCheckBoxes(ckTiers);
                clearCheckBoxes(ckRegions);

                args.MSO_s = msoList;
                args.StartDate = dtpStart.Value;
                args.EndDate = dtpStop.Value;
                args.regionQuery = regionQuery;
                GV.MODE = GV.PreviousMode;
            }
            if (CustomFormat)
            {
                args.CustomFormat = true;
            }
            else
            {
                args.CustomFormat = false;
            } 
            allSelected = false;
            DataReadyEvent?.Invoke(this, args);
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


        private void frmDateMSO_Picker_Activated(object sender, EventArgs e)
        {
            if (GV.MODE == Mode.Report_Rollup || GV.MODE == Mode.Report_AvgCompletion && GV.MODE != Mode.Loading_lbMSOFormCheckBox)
            {
                lbMSO.SelectedItems.Clear();
            }

            if (GV.MODE == Mode.Report_ByPriority)
            {
                this.Height = 180;
            }
            else
            {
                this.Height = 890;
            }

            //If loading lbMSO from CheckBox switch back to ReportRollup
            if (GV.MODE == Mode.Loading_lbMSOFormCheckBox)
            {
                GV.MODE = GV.PreviousMode;
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

        private void rdo_Normal_CheckedChanged(object sender, EventArgs e)
        {
            if (rdo_Normal.Checked)
            {
                CustomFormat = false;
            }
        }

        private void rdo_Custom_CheckedChanged(object sender, EventArgs e)
        {
            if (rdo_Custom.Checked)
            {
                CustomFormat = true;
            }
        }

        private List<string> createRegionsString(CheckBox[] ckRegions)
        {
            //Create query list for regions
            List<string> regions = new List<string>();
            foreach (var checkBox in ckRegions)
            {
                    if (checkBox.Checked)
                    {
                        regions.Add(checkBox.Tag.ToString());
                    }
            }
            
            return regions;
        }

        private List<string> createTiersString(CheckBox[] ckTiers)
        {
            //Create query list tiers
            List<string> tiers = new List<string>();
            foreach (var checkBox in ckTiers)
            {
                if (checkBox.Checked)
                {
                    tiers.Add(checkBox.Tag.ToString());
                }                
            }

            return tiers;
        }

        private void clearCheckBoxes(CheckBox[] ckArray)
        {
            foreach (CheckBox ckBox in ckArray)
            {
                ckBox.Checked = false;
            }
        }

        private void btnClearRegions_Click(object sender, EventArgs e)
        {
            clearCheckBoxes(ckRegions);
        }

        private void btnClearTiers_Click(object sender, EventArgs e)
        {
            clearCheckBoxes(ckTiers);
        }

        private void highlightListBoxItemsFromTierSelection(List<MSO_Model> modelList, bool isChecked)
        {
            foreach (var mso in modelList)
            {
                if (isChecked)
                {
                    for (int i = 0; i < lbMSO.Items.Count - 1; i++)
                    //foreach (MSO_Model model in lbMSO.Items)
                    {
                        MSO_Model currentMSO = (MSO_Model)lbMSO.Items[i];
                        string msoModel = currentMSO.MSO;
                        if (msoModel == mso.MSO && isChecked)
                        {
                            lbMSO.SetSelected(i, true);
                            break;
                        }
                    }
                }
                if (!isChecked)                    
                {
                    for (int i = 0; i < lbMSO.Items.Count; i++)
                    {
                        MSO_Model currentMSO = (MSO_Model)lbMSO.Items[i];
                        string msoModel = currentMSO.MSO;
                        if (msoModel == mso.MSO && !isChecked)
                        {
                            lbMSO.SetSelected(i, false);
                            break;
                        } 
                    }
                } 
            }
        }

        private void ckTier1_CheckedChanged(object sender, EventArgs e)
        {
            Mode curMode = GV.MODE;
            if (GV.MODE != Mode.Loading_lbMSOFormCheckBox)
            {
                GV.MODE = Mode.Loading_lbMSOFormCheckBox; 
            }
            CheckBox ckBox = (CheckBox)sender;
            switch (ckBox.Name)
            {
                case "ckTier1":
                    highlightListBoxItemsFromTierSelection(tier1Models, ckBox.Checked);
                    break;

                case "ckTier2":
                    highlightListBoxItemsFromTierSelection(tier2Models, ckBox.Checked);
                    break;

                case "ckTier0":
                    highlightListBoxItemsFromTierSelection(tier0Models, ckBox.Checked);
                    break;

                default:
                    break;
            } 
        }


        private void frmDateMSO_Picker_Load(object sender, EventArgs e)
        {
            //Gather list of MSO's and create lists of tiers
            allMSO_S = GlobalConfig.Connection.GenericGetAll<MSO_Model>("tblMSO", "MSO");
            tier1Models = allMSO_S.Where(x => x.Tier == 1).ToList();
            tier2Models = allMSO_S.Where(x => x.Tier == 2).ToList();
            tier0Models = allMSO_S.Where(x => x.Tier == 0).ToList();
            List<string> msoList = GlobalConfig.Connection.GetSnapshotMSO_s();
            lbMSO.DataSource = allMSO_S;
            lbMSO.DisplayMember = "MSO";
            lbMSO.SelectedIndex = -1;
        }

        private void ckUnclassified_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }




    public class DataReadyEventArgs : EventArgs
    {
        public List<MSO_Model> MSO_s { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool CustomFormat { get; set; }
        public List<string> tierQuery { get; set; }
        public List<string> regionQuery { get; set;}
    }
}

