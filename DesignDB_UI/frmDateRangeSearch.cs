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
using Microsoft.ReportingServices.RdlExpressions.ExpressionHostObjectModel;

namespace DesignDB_UI
{
    public partial class frmDateRangeSearch : Form
    {
        public string FileName { get; set; }

        public event EventHandler<CancelEventArgs> PickerCanceled;
        public event EventHandler<DateRangeEventArgs> DateRangeSet;
        CheckBox[] ckRegions;
        CheckBox[] ckTiers;
        List<MSO_Model> allMSO_S;
        List<MSO_Model> tier1Models;
        List<MSO_Model> tier2Models;
        List<MSO_Model> tier0Models;
        List<string> tierQuery = new List<string>();
        List<string> regionQuery = new List<string>();
        List<RequestModel> requestList = new List<RequestModel>();
        string term = "";
        DateRangeEventArgs drArgs = new DateRangeEventArgs();
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
                btnSelectAll_Click(sender, e);
            }

            //Remove this after BOM-Shipments is done------------------------------------------------------------------------------
            //dtpStartDate.Value = new DateTime(2023, 1, 1);
            //dtpEndDate.Value = new DateTime(2023, 3, 31);

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
            this.Hide();
            if (GV.MODE == Mode.BOM_Shipments)
            {
                List<MSO_Model> msoList = new List<MSO_Model>();
                foreach (var mso in lbMSO.SelectedItems)
                {
                    MSO_Model msoModel = (MSO_Model)mso;
                    msoList.Add(msoModel);
                }
                if (msoList.Count == 0)
                {
                    MessageBox.Show("No MSO selected. Please make selection.");
                    return;
                }
                drArgs.StartDate = dtpStartDate.Value;
                drArgs.EndDate = dtpEndDate.Value;
                drArgs.msoList = msoList;
                drArgs.Filename = FileName;
                DateRangeSet?.Invoke(this, drArgs);
            }
            else
            {
                GV.MODE = Mode.DateRangeSearch;
                List<MSO_Model> msoList = new List<MSO_Model>();
                foreach (var mso in lbMSO.SelectedItems)
                {
                    MSO_Model msoModel = (MSO_Model)mso;
                    msoList.Add(msoModel);
                }

                requestList = GlobalConfig.Connection.DateRangeSearch_Unfiltered
                    (dtpStartDate.Value, dtpEndDate.Value, term, false, "", cboDesigner.Text, cboRequestor.Text);
                int records = requestList.Count;

                List<RequestModel> filteredRequests = new List<RequestModel>();
                if (records > 0)
                {
                    foreach (var mso in msoList)
                    {
                        filteredRequests.AddRange(requestList.Where(x => x.MSO == mso.MSO).ToList());
                    }
                    records = filteredRequests.Count; 
                }

                switch (records)
                {
                    case 0:
                        MessageBox.Show("No records found");
                        break;

                    case 1:
                        GV.REQFORM.Request = filteredRequests.FirstOrDefault();
                        GV.REQFORM.Show();
                        break;

                    default:
                        //frmMultiResult frmMultiResult = new frmMultiResult(requestList);
                        GV.MultiResult.dataList = filteredRequests;
                        GV.MultiResult.Show();
                        break;
                }
                
            }
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
            public DateTime StartDate;
            public DateTime EndDate;
            public string MSO;
            public List<MSO_Model> msoList;
            public string Filename;
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
            GV.MODE = curMode;
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

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            LbSelectItems(true);
        }

        private void LbSelectItems(bool select)
        {
            for (int i = 0; i < lbMSO.Items.Count; i++)
                lbMSO.SetSelected(i, select);
        }

        private void btnDeselect_Click(object sender, EventArgs e)
        {
            LbSelectItems(false);
        }
    }   
}            


