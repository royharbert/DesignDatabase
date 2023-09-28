using DesignDB_Library;
using DesignDB_Library.Operations;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace DesignDB_UI
{
    
    public partial class frmDateRangeSearch : Form
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
        public event EventHandler<DateRangeEventArgs> DateRangeSet;
        List<RequestModel> requestList = new List<RequestModel>();
        string term = "";
        DataReadyEventArgs drArgs = new DataReadyEventArgs();
        
        private void frmDateRangeSearch_Load(object sender, EventArgs e)
        {
            //Gather list of MSO's and create lists of tiers
            allMSO_S = GlobalConfig.Connection.GenericGetAll<MSO_Model>("tblMSO", "MSO");
            tier1Models = allMSO_S.Where(x => x.Tier == 1).ToList();
            tier2Models = allMSO_S.Where(x => x.Tier == 2).ToList();
            tier0Models = allMSO_S.Where(x => x.Tier == 0 || x.Tier == null).ToList();
            List<string> msoList = GlobalConfig.Connection.GetSnapshotMSO_s();
            lbMSO.DataSource = allMSO_S;
            lbMSO.DisplayMember = "MSO";
            lbMSO.SelectedIndex = -1;

            if (GV.MODE == Mode.BOM_Shipments)
            {
                cboDesigner.Visible = false;
                cboRequestor.Visible = false;
                btnForecast.Visible = false;
                btnSearch.Visible = true;
            }
            else
            {
                cboDesigner.Visible = true;
                cboRequestor.Visible = true;
                lbMSO.SelectedItem = "Cable One";
            }

            //Remove this after BOM-Shipments is done
            dtpStartDate.Value = new DateTime(2023, 6, 1);
           
        }
        public frmDateRangeSearch()
        {
            InitializeComponent();

            ckRegions = new CheckBox[] { ckAfrica, ckAsia, ckAustralia, ckCanada, ckCaribbean, ckEurope, ckIndia, ckLatinAmerica, ckMiddleEast,
                ckRussia, ckUSEast, ckUSWest, ckOther  };

            ckTiers = new CheckBox[] { ckTier1, ckTier2, ckUnclassified };

            term = "DateAssigned";
            if (GV.MODE == Mode.DateRangeSearch)
            {
                List<DesignersReviewersModel> designers = GlobalConfig.Connection.GetAllDesigners();
                cboDesigner.DataSource= designers;
                cboDesigner.DisplayMember = "Designer";
                cboDesigner.SelectedIndex = -1;

                List<SalespersonModel> requestors = GlobalConfig.Connection.GenericGetAll<SalespersonModel>("tblSalespersons", "SalesPerson");
                cboRequestor.DataSource = requestors;
                cboRequestor.DisplayMember = "Salesperson";
                cboRequestor.SelectedIndex = -1;

                lblDesigner.Visible = true;
                lblRequestor.Visible = true;
                cboDesigner.Visible = true;
                cboRequestor.Visible = true;

                btnForecast.Visible = false;
                btnSearch.Visible = true;
            }
            else
            {
                lblDesigner.Visible = false;
                lblRequestor.Visible = false;
                cboDesigner.Visible = false;
                cboRequestor.Visible = false;

                btnSearch.Visible = false;
                btnForecast.Visible = true;                
            } 

            FC.SetFormPosition(this);
            this.BringToFront();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdoDate_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb != null && rb.Checked)
            {
                term = "DateAssigned";
            }
        }

        private void rdoDateDue_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb != null && rb.Checked)
            {
                term = "DateDue";
            }
        }

        private void rdoDateCompleted_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb != null && rb.Checked)
            {
                term = "DateCompleted";
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (GV.MODE == Mode.BOM_Shipments)
            {
                List<MSO_Model>msoList = new List<MSO_Model>();
                MSO_Model mso = (MSO_Model)lbMSO.SelectedItem;
                msoList.Add(mso);
                drArgs.StartDate = dtpStartDate.Value;
                drArgs.EndDate = dtpEndDate.Value;
                drArgs.MSO_s = msoList;
                DataReadyEvent?.Invoke(this, drArgs);
            }
            else
            {
                GV.MODE = Mode.DateRangeSearch;
                int records = requestList.Count;

                requestList = GlobalConfig.Connection.ReportDateRangeSearch_MSOFiltered
                    (dtpStartDate.Value, dtpEndDate.Value, term, lbMSO.Text, false, cboDesigner.Text, cboRequestor.Text);
                records = requestList.Count;

                switch (records)
                {
                    case 0:
                        MessageBox.Show("No records found");
                        break;

                    default:
                        //frmMultiResult frmMultiResult = new frmMultiResult(requestList);
                        GV.MultiResult.dataList = requestList;
                        GV.MultiResult.Show();
                        break;
                }
            }
            this.Close();
        }

        private void btnForecast_Click(object sender, EventArgs e)
        {
                  
            DateRangeEventArgs args = new DateRangeEventArgs();
            args.StartDate = dtpStartDate.Value;
            args.EndDate = dtpEndDate.Value;
            args.MSO = lbMSO.Text;
            DateRangeSet?.Invoke(this, args);
            this.Close();    
        }

        public class DateRangeEventArgs : EventArgs
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string MSO { get; set; }
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

                case "ckUnclassified":
                    highlightListBoxItemsFromTierSelection(tier0Models, ckBox.Checked);
                    break;

                default:
                    break;
            }
        }
        private void clearCheckBoxes(CheckBox[] ckArray)
        {
            foreach (CheckBox ckBox in ckArray)
            {
                ckBox.Checked = false;
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

        private void btnClearTiers_Click(object sender, EventArgs e)
        {
            clearCheckBoxes(ckTiers);
        }

        private void btnClearRegions_Click(object sender, EventArgs e)
        {
            clearCheckBoxes(ckRegions);
        }

    }   
}            


