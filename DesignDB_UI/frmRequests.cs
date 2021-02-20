﻿using DesignDB_Library.Models;
using DesignDB_Library.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GalaSoft.MvvmLight.Messaging;
using System.IO;
using System.Diagnostics;
using DesignDB_Library;
using System.Collections.ObjectModel;
using System.Globalization;

namespace DesignDB_UI
{

    public partial class frmRequests : Form
    {        
        RequestModel Rm = new RequestModel();

        //Class scope variables to hold button name strings
        
        static string Level1ButtonNames = "btnUndo,btnSave,btnSearch,btnAddAtt,btnRemoveAtt,btnDone,btnLoadRpt";
        static string Level2ButtonNames = Level1ButtonNames + ",btnDelete,btnRestore,btnClone,btnNew,btnRev";
        static string RequestNewButtons = "btnUndo,btnSave,btnAddAtt,btnRemoveAtt,btnDone,btnClone,btnNew,btnRev";
        static string RequestEditButtons = "btnUndo,btnSave,btnAddAtt,btnRemoveAtt,btnDone,btnClone,btnNew,btnRev";
        static string RequestCloneButtons = "btnUndo,btnSave,btnAddAtt,btnRemoveAtt,btnDone,btnClone,btnNew,btnRev";
        static string RequestRevisionButtons = "btnUndo,btnSave,btnAddAtt,btnRemoveAtt,btnDone,btnClone,btnNew,btnRev";
        static string RequestDeleteButtons = "btnDelete";
        static string RequestRestoreButtons = "btnRestore";
        static string RequestSearchButtons = "btnSearchFields";

        bool formLoading;
        bool formDirty;        

        frmAttType frm = null;       
        DateTime failDate = new DateTime(1900, 1, 1);
        //DateTime minDate = new DateTime(2000, 1, 1);

        RequestModel initialRequest = null;

        public RequestModel Request
        {
            get
            {
                return Rm;
            }
            set
            {
                formLoading = true;
                Rm = value;
                prepFormForTask();
                if (Rm != null)
                {
                    insertData(Rm);
                }
                getAttachments(txtPID.Text);
                formLoading = false;
                formDirty = false;
                addHandlers();
            }
}
        public frmRequests(List<RequestModel> rm = null)
        {
            formLoading = true;
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();
            InitializeComponent();
            makeLists();    
        }

        private void prepFormForTask()
        {
            switch (GV.MODE)
            { 
                case Mode.New:                   
                    setButtonDisplay(RequestNewButtons);
                    break;

                case Mode.Restore:
                    setButtonDisplay(RequestRestoreButtons);
                    break;

                case Mode.Delete:
                    setButtonDisplay(RequestDeleteButtons);
                    break;

                case Mode.Edit:
                case Mode.Report_Overdue:
                    setButtonDisplay(RequestEditButtons);
                    break;

                case Mode.Revision:
                    setButtonDisplay(RequestRevisionButtons);
                    break;

                case Mode.Clone:
                    setButtonDisplay(RequestCloneButtons);
                    break;

                case Mode.Search:
                    setButtonDisplay(RequestSearchButtons);
                    break;

                case Mode.None:
                    Rm = null;
                    break;

                case Mode.Forecast:
                    unlockTLP(true);
                    break;

                default:
                    Rm = null;
                    break;
            }            
        }
  
        /// <summary>
        /// sets visibility of buttons on form
        /// </summary>
        /// <param name="displayString"></param>
        private void setButtonDisplay(string displayString)
        {
            //hide all buttons
            foreach (Control ctl in tlpBottom.Controls)
            {
                if (ctl is Button)
                {
                    ctl.Visible = false;
                }
            }

            string[] visibleButtons = displayString.Split(',');
            foreach (Control ctl in tlpBottom.Controls)
            {
                string name = ctl.Name;
                int idx = Array.IndexOf(visibleButtons, name);
                if (idx != -1)
                {
                    ctl.Visible = true;                            
                }
            }
            btnDone.Visible = true;
            
        }

        private void checkForSave()
        {
            DialogResult result = MessageBox.Show("Save Changes?", caption: "Save", buttons:  MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                saveChanges();
            }
        }

        private void SetButtonVisibility(DesignersReviewersModel designer)
        {
            string[] p1Array = Level1ButtonNames.Split(',');
            string[] p2Array = Level2ButtonNames.Split(',');

            int priviledge = designer.Priviledge;

            //loop thru all controls and set visibility
            foreach (Control control in tlpBottom.Controls)
            {
                if (control is Button)
                {
                    int idx = -1;
                    switch (priviledge)
                    {
                        case 1:
                            idx = Array.IndexOf(p1Array, control.Name);
                            //if idx >-1 then control name is in the visible list
                            if (idx > -1)
                            {
                                control.Visible = true;
                            }
                            else
                            {
                                control.Visible = false;
                            }
                            break;
                        case 2:
                        case 3:
                        case 4:
                            idx = Array.IndexOf(p2Array, control.Name);
                            //if idx >-1 then control name is in the visible list
                            if (idx > -1)
                            {
                                control.Visible = true;
                            }
                            else
                            {
                                control.Visible = false;
                            }
                            break;

                        default:
                            break;
                    }

                }
            }
        }

        private void formatAttGrid()
        {
            
            dgvAttachments.Columns[0].Visible = false;
            dgvAttachments.Columns[1].Visible = false;
            dgvAttachments.Columns[4].Visible = false;

            dgvAttachments.Columns[2].HeaderText = "File Name";
            dgvAttachments.Columns[2].Width = 490;
            dgvAttachments.Columns[2].DefaultCellStyle.ForeColor = Color.Blue;

            dgvAttachments.Columns[3].HeaderText = "Item Type";
            dgvAttachments.Columns[3].DefaultCellStyle.ForeColor = Color.Black;
        }

        private List<AttachmentModel> getAttachments(string pid)
        {
            List<AttachmentModel> att = GlobalConfig.Connection.GetAttachments(pid);
            dgvAttachments.DataSource = att;
            formatAttGrid();
            return att;
        }

        /// <summary>
        /// /inserts data from model into boxes then sets formLoading to false
        /// </summary>
        /// <param name="rm"></param>
        private void insertData(RequestModel rm)
        {
            txtPID.Text = rm.ProjectID;
            cboMSO.Text = rm.MSO;
            txtCust.Text = rm.Cust;
            cboCities.Text = rm.City;
            cboState.Text = rm.ST;
            cboCountry.Text = rm.Country;
            cboRegion.Text = rm.Region;
            cboRequestor.Text = rm.DesignRequestor;
            cboQuoteType.Text = rm.QuoteType;
            cboPriority.Text = rm.Pty;
            cboDesigner.Text = rm.Designer; 
            cboAssisted.Text = rm.AssistedBy;
            txtProjName.Text = rm.ProjectName;
            cboOrigQuote.Text = rm.OriginalQuote;
            cboCategory.Text = rm.Category;
            cboArchType.Text = rm.ArchitectureType;
            
            dtpLoadFromModel(txtDateAssigned, rm.DateAssigned);
            
            dtpLoadFromModel(txtDateAllInfo, rm.DateAllInfoReceived);
            
            dtpLoadFromModel(txtDateDue, rm.DateDue);
            
            dtpLoadFromModel(txtDateCompleted, rm.DateCompleted);
            cboReviewedBy.Text = rm.ReviewedBy;
            txtBOM_Val.Text = rm.BOM_Value.ToString("C2");
            txtPctCovered.Text = rm.PercentageProjectCovered.ToString();
            cboAwardStatus.Text = rm.AwardStatus;
            txtTotalHours.Text = rm.TotalHours.ToString();
           
            dtpLoadFromModel(txtLastUpdate, rm.DateLastUpdate);
            rtbArchDetails.Text = rm.ArchitectureDetails;
            rtbComments.Text = rm.Comments;
            txtTotalVal.Text = calcTotVal().ToString("C2");

            //set modelMSO since it does not come from DB
            Rm.msoModel = (MSO_Model)cboMSO.SelectedItem;
            initialRequest = CommonOps.CloneRequestList(Rm);            
        }

        private void dtpLoadFromModel(DateTimePicker dtp, DateTime modelDate)
        {
            if (modelDate <= failDate)
            {
                dtpResetForced(dtp);
            }
            else
            {
                dtp.Format = DateTimePickerFormat.Short;
                dtp.Value = modelDate;
            }
        }

        private void setDTP_CustomFormat(DateTimePicker picker)
        {
            dtpResetForced(picker);       
        }

        private void loadModel()
        {
            Rm.ProjectID = txtPID.Text;
            Rm.msoModel = (MSO_Model)cboMSO.SelectedItem;
            Rm.MSO = Rm.msoModel.MSO;
            Rm.Cust = txtCust.Text;
            Rm.City = cboCities.Text;
            Rm.ST = cboState.Text;
            Rm.Country = cboCountry.Text;
            Rm.Region = cboRegion.Text;
            Rm.DesignRequestor = cboRequestor.Text;
            Rm.QuoteType = cboQuoteType.Text;
            Rm.Pty = cboPriority.Text;
            Rm.Designer = cboDesigner.Text;
            Rm.AssistedBy = cboAssisted.Text;
            Rm.ProjectName = txtProjName.Text;
            Rm.OriginalQuote = cboOrigQuote.Text;
            Rm.Category = cboCategory.Text;
            Rm.ArchitectureType = cboArchType.Text;

            if (txtDateAssigned == null)
            {
                MessageBox.Show("Date Assigned is a required parameter. Please enter a valid date.");
                return;
            }

            Rm.DateAllInfoReceived = dtpReset(txtDateAllInfo);
            Rm.DateCompleted = dtpReset(txtDateCompleted);
            Rm.DateDue = dtpReset(txtDateDue);
            Rm.DateLastUpdate = dtpReset(txtLastUpdate);            

            Rm.ReviewedBy = cboReviewedBy.Text;

            decimal BOM = 0;
            decimal.TryParse(txtBOM_Val.Text, out BOM);
            Rm.BOM_Value = BOM;

            int pct = 0;
            int.TryParse(txtPctCovered.Text, out pct);
            Rm.PercentageProjectCovered = pct;

            Rm.AwardStatus = cboAwardStatus.Text;

            int hrs = 0;
            int.TryParse(txtTotalHours.Text, out hrs);
            Rm.TotalHours = hrs;
            Rm.ArchitectureDetails = rtbArchDetails.Text;
            Rm.Comments = rtbComments.Text;

        }

        private void dtpResetForced(DateTimePicker dtp)
        {
            dtp.Value = new DateTime(1900, 1, 1);
            Application.DoEvents();
            dtp.CustomFormat = " ";
            dtp.Format = DateTimePickerFormat.Custom;
        }

        private DateTime dtpReset(DateTimePicker dtp)
        {
            if (dtp.Value <= failDate)
            {
                dtp.Value = new DateTime(1900,1,1);
                Application.DoEvents();
                dtp.CustomFormat = " ";
                dtp.Format = DateTimePickerFormat.Custom;
            }
            return dtp.Value;
        }

        private decimal calcTotVal()
        {
            decimal BOMVal = 0;
            decimal pct = 0;

            if (txtBOM_Val.Text == "")
            {
                txtBOM_Val.Text = "0";
            }

            if (txtPctCovered.Text == "")
            {
                txtPctCovered.Text = "0";
            }
            //string unformattedVal = txtBOM_Val.Text.ToString();
            bool BOMSuccess = decimal.TryParse(txtBOM_Val.Text, NumberStyles.Currency, CultureInfo.CurrentCulture.NumberFormat, out BOMVal);
            bool pctSuccess = decimal.TryParse(txtPctCovered.Text, out pct);

            //TODO -- Routine to catch parse errors

            if (pct > 0)
            {
                return BOMVal / (pct / 100);
            }
            else
            {
                return 0;
            }
        }

        private void generalReset()
        {
            formLoading = true;
            unlockTLP(false);
            Rm = null;
            Rm = new RequestModel();
            //clear combos and texts
            FC.clearTextinControl(this.Controls, typeof(TextBox));
            FC.clearTextinControl(this.Controls, typeof(ComboBox));
            FC.clearTextinControl(this.Controls, typeof(RichTextBox));
            Rm.DateAssigned = DateTime.Today;
            Rm.AwardStatus = "Pending";
            Rm.PercentageProjectCovered = 100;
            resetDTPs(false);            
            unlockTLP(false);
            cboMSO.Enabled = true;
            cboMSO.SelectedIndex = -1;
            cboMSO.Focus();
            dgvAttachments.DataSource = "";
            this.Request = Rm;
            formLoading = false;
            formDirty = false;
        }

 
        /// <summary>
        /// Resets form for new input by clearing boxes, formatting DTP's and making new RequestModel
        /// </summary>
        public void resetForm()
        {
            switch (GV.MODE)
            {
                case Mode.New:                          
                    generalReset();
                    break;
                case Mode.Search:
                    Rm = new RequestModel();
                    insertData(Rm);
                    txtBOM_Val.Clear();
                    txtPctCovered.Clear();
                    txtTotalVal.Clear();
                    txtTotalHours.Clear();
                    break;
                case Mode.Edit:
                    resetDTPs(false);
                    break;
                case Mode.Revision:
                    resetDTPs(false);
                    txtPID.Text = Rm.ProjectID;
                    getAttachments(Rm.ProjectID);
                    formDirty = false;
                    break;          
                case Mode.Delete:
                    break;
                case Mode.Restore:
                    break;
                case Mode.DateRangeSearch:
                    break;
                case Mode.Forecast:
                    break;
                case Mode.None:
                    GV.MODE = GV.PreviousMode;
                    break;
                default:
                    break;
            }
      

        }

        private void unlockTLP(bool unLock)
        {
            List<TableLayoutPanel> panels = new List<TableLayoutPanel>();
            
            panels.Add(tlpLeft);
            panels.Add(tlpCenterTop);
            panels.Add(tlpRight);

            foreach (TableLayoutPanel panel in panels)
            {
                unlockControls(unLock, panel);
            }
        }

        private static void unlockControls(bool Unlock, TableLayoutPanel tlp)
        {
            foreach (Control ctl in tlp.Controls)
            {
                if (ctl.Tag != null)
                {
                    string tag = ctl.Tag.ToString();
                    string[] tags = tag.Split('|');
                    if (tags[0] == "L")
                    {
                        ctl.Enabled = Unlock;
                    }
                }
            }
        }

        private List<DesignersReviewersModel> cloneList(List<DesignersReviewersModel> oldList)
        {
            List<DesignersReviewersModel> newList = new List<DesignersReviewersModel>();
            foreach (DesignersReviewersModel model in oldList)
            {
                newList.Add(model);
            }
            return newList;
        }       
        
        private void makeLists()
        {
            List<DesignersReviewersModel> activeDesignerList = GlobalConfig.Connection.DesignersGetActive();
            cboDesigner.DataSource = activeDesignerList;
            cboDesigner.DisplayMember = "Designer";
            cboDesigner.SelectedIndex = -1;


            List<DesignersReviewersModel> assistedList = cloneList(activeDesignerList);
            cboAssisted.DataSource = assistedList;
            cboAssisted.DisplayMember = "Designer";
            cboAssisted.SelectedIndex = -1;

            List<DesignersReviewersModel> reviewerList = GlobalConfig.Connection.Reviewers_GetActive();
            cboReviewedBy.DataSource = reviewerList;
            cboReviewedBy.DisplayMember = "Designer";
            cboReviewedBy.SelectedIndex = -1;

            List<MSO_Model> msoList = GlobalConfig.Connection.GetAllMSO();
            cboMSO.DataSource = msoList;
            cboMSO.DisplayMember = "MSO";
            cboMSO.SelectedIndex = -1;

            List<CityModel> cityList = GlobalConfig.Connection.GetAllCities();
            cboCities.DataSource = cityList;
            //cboCities.DataSource = GV.Cities;
            cboCities.DisplayMember = "City";
            cboCities.SelectedIndex = -1;

            List<StateModel> stateList = GlobalConfig.Connection.GetAllStates();
            cboState.DataSource = stateList;
            cboState.DisplayMember = "State";
            cboState.SelectedIndex = -1;

            List<CountriesModel> countryList = GlobalConfig.Connection.GetAllCountries();
            cboCountry.DataSource = countryList;
            cboCountry.DisplayMember = "Country";
            cboCountry.SelectedIndex = -1;

            List<RegionsModel> regionList = GlobalConfig.Connection.GetAllRegions();
            cboRegion.DataSource = regionList;
            cboRegion.DisplayMember = "Region";
            cboRegion.SelectedIndex = -1;

            List<SalespersonModel> salesList = GlobalConfig.Connection.SalesGetActive();
            cboRequestor.DataSource = salesList;
            cboRequestor.DisplayMember = "SalesPerson";
            cboRequestor.SelectedIndex = -1;
        }

        private void cboMSO_SelectedIndexChanged(object sender, EventArgs e)
        {  
            //Rm.msoModel = (MSO_Model)cboMSO.SelectedItem;        
            if (!formLoading)
            {
                if (cboMSO.SelectedIndex > -1 && GV.MODE == Mode.New)
                {
                    MSO_Model mso = (MSO_Model)cboMSO.SelectedItem;
                    string PID = PID_Generator.MakePID(mso);
                    txtPID.Text = PID;
                    unlockTLP(true);                      
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Edit;
            string search = '%' + txtSearch.Text + '%';
            List<RequestModel> rm = GlobalConfig.Connection.GetRequestByPID(search).ToList();
            frmMainMenu.ManageSearchResults(rm);
            insertData(rm[0]);
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            if (formDirty)
            {
                checkForSave();
            }

            GV.MODE = Mode.None;
            resetForm();            
            GV.REQFORM.Hide();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveChanges();
        }

        private void saveChanges()
        {
            loadModel();
            switch (GV.MODE)
            {
                case Mode.New:
                    GlobalConfig.Connection.RequestInsert(Rm);
                    break;
                case Mode.Edit:
                    GlobalConfig.Connection.RequestUpdate(Rm);
                    break;
                case Mode.Revision:                    
                    break;
                case Mode.Clone:
                    RequestOps.InsertNewRequest(Rm);
                    GV.MODE = Mode.Edit;
                    break;
                case Mode.Delete:
                    break;
                case Mode.None:
                    break;
                default:
                    break;
            }

            MessageBox.Show("Record " + Rm.ProjectID + " Saved.");

            formDirty = false;
        }

        private void btnClone_Click(object sender, EventArgs e)
        {
            saveChanges();
            GV.MODE = Mode.Clone;
            Rm = RequestOps.Clone(Rm);

            //Load boxes
            insertData(Rm);
            resetDTPs(false);
            formLoading = false;
            saveChanges();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (formDirty)
            {
                checkForSave();
            }
            GV.MODE = Mode.New;
            cboMSO.Enabled = true;
            Rm = new RequestModel();             
            resetForm();
            unlockTLP(false);
        }

        private void resetDTPs(bool resetDateAssigned)
        {
            setDTP_CustomFormat(txtDateAllInfo);
            setDTP_CustomFormat(txtDateCompleted); 
            setDTP_CustomFormat(txtDateDue);
            setDTP_CustomFormat(txtLastUpdate);
            if (resetDateAssigned)
            {
                setDTP_CustomFormat(txtDateAssigned);  
            }
        }

        private void btnRev_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Revision;
            if(formDirty)
            {
                checkForSave();
            }
            DialogResult result = confirmAction();

            if (result == DialogResult.Yes)
            {
                this.Request  = RequestOps.CreateRevision(Rm);         
            }

            //set combo boxes to ""
            resetCombo(cboReviewedBy);
            resetCombo(cboPriority);
            resetCombo(cboArchType);
            resetCombo(cboCategory);
            resetCombo(cboAssisted);
            resetCombo(cboDesigner);
            dtpResetForced(txtDateDue);
            dtpResetForced(txtDateAllInfo);            
            dtpResetForced(txtLastUpdate);
        }

        private void resetCombo(ComboBox cbo)
        {
            cbo.SelectedIndex = -1;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Delete;
            DialogResult result = confirmAction();
            if (result == DialogResult.Yes)
            {
                RequestOps.DeleteRequest(Rm);
            }
            resetForm();
            this.Visible = false;
        }

        private void btnAddAtt_Click(object sender, EventArgs e)
        {
            AttachmentModel model = new AttachmentModel();
            frm = new frmAttType(model);
            frm.TypeReadyEvent += Frm_TypeReadyEvent;

            OpenFileDialog openFD = new OpenFileDialog();
            openFD.Title = "Save Attachment";
            string myDox = Environment.SpecialFolder.MyDocuments.ToString();
            openFD.InitialDirectory = myDox;
            openFD.RestoreDirectory = true;
            openFD.Multiselect = false;

            model.ID = 0;
            model.PID = txtPID.Text;
                        
            if (openFD.ShowDialog() == DialogResult.OK)
            {
                string fullFileName = openFD.FileName;
                string fileName = Path.GetFileName(fullFileName);
                model.FileToSave = fullFileName;
                model.DisplayText = fileName;
                frm.ShowDialog();                
            }

            frm.TypeReadyEvent -= Frm_TypeReadyEvent;

        }

        private void Frm_TypeReadyEvent(object sender, AttachmentModel e)
        {
            string fileName = GlobalConfig.AttachmentPath + "\\" + e.PID + "\\" + e.DisplayText;
            FileOps.SaveAttFile(e);
            GlobalConfig.Connection.InsertInto_tblAttachments(e);
            List<AttachmentModel> aList =  GlobalConfig.Connection.GetAttachments(e.PID);
            dgvAttachments.DataSource = null;
            dgvAttachments.DataSource = aList;
            formatAttGrid();
        }

        private void txtPID_TextChanged(object sender, EventArgs e)
        {
            if (txtPID.Text != "")
            {
                getAttachments(txtPID.Text);
            }
            
        }

        private void dgvAttachments_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            List<AttachmentModel> aList = (List<AttachmentModel>)dgvAttachments.DataSource;
            int selRow = dgvAttachments.CurrentRow.Index;
            AttachmentModel model = aList[selRow];
            
            string fileName = dgvAttachments.CurrentRow.Cells[2].Value.ToString();
            fileName = GlobalConfig.AttachmentPath + "\\" + model.PID + "\\" + fileName;
            ProcessStartInfo sinfo = new ProcessStartInfo(fileName);
            Process.Start(sinfo);
        }

        private void btnRemoveAtt_Click(object sender, EventArgs e)
        {
            //use class to accomplish
            //make attachment model and pass to class
            if (dgvAttachments.CurrentRow != null)
            {
                int sel = dgvAttachments.CurrentRow.Index;
                List<AttachmentModel> aList = (List<AttachmentModel>)dgvAttachments.DataSource;
                AttachmentModel model = aList[sel];
                List<AttachmentModel> newList = DesignDB_Library.Operations.AttachmentOps.DeleteAttachment(model);
                dgvAttachments.DataSource = null;
                dgvAttachments.DataSource = newList;
                formatAttGrid();
            }
            else
            {
                MessageBox.Show("No row selected for deletion. \nPlease click left margin of desired row");
            }
        }

        private void txtPctCovered_TextChanged(object sender, EventArgs e)
        {
            if (GV.MODE != Mode.Search)
            {
                txtTotalVal.Text = calcTotVal().ToString("C2");
            }
        }

        private void txtBOM_Val_TextChanged(object sender, EventArgs e)
        {
            if (GV.MODE != Mode.Search)
            {
                txtTotalVal.Text = calcTotVal().ToString("C2");
            }
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Undo;
            Rm = initialRequest;
            insertData(Rm);
            GV.MODE = GV.PreviousMode;
            txtDateAssigned.Value = DateTime.Now;
        }

        private void txtDateCompleted_ValueChanged(object sender, EventArgs e)
        {
            setdtpFormat(sender as DateTimePicker);
        }

        private void txtDateDue_ValueChanged(object sender, EventArgs e)
        {
            setdtpFormat(sender as DateTimePicker);
        }

        private void txtDateAllInfo_ValueChanged(object sender, EventArgs e)
        {
            setdtpFormat(sender as DateTimePicker);
        }

        private void txtDateAssigned_ValueChanged(object sender, EventArgs e)
        {
            setdtpFormat(sender as DateTimePicker);
            if (cboPriority.Text != "" && !formLoading && GV.MODE != Mode.Search)
            {
                txtDateDue.Value = CommonOps.CalculateDateDue(txtDateAssigned.Value, cboPriority.Text);
            }
        }

        private void txtLastUpdate_ValueChanged(object sender, EventArgs e)
        {
            setdtpFormat(sender as DateTimePicker);
        }

        private void setdtpFormat(DateTimePicker dtp)
        {
            if (!formLoading  && GV.MODE != Mode.Undo)
            {
                dtp.Format = DateTimePickerFormat.Short;
            }
            formDirty = true;
        }

        private void cboPriority_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtDateAssigned.Text != null && !formLoading && GV.MODE != Mode.Search)
            {
                setdtpFormat(txtDateDue);
                txtDateDue.Value = CommonOps.CalculateDateDue(txtDateAssigned.Value, cboPriority.Text);                
            }
            formDirty = true;
        }

        private void btnLoadRpt_Click(object sender, EventArgs e)
        {
            Form frmLoadReport = new frmLoadReport();
            frmLoadReport.ShowDialog();
        }
        /// <summary>
        /// Event handlers to prompt save dialog
        /// </summary>
        private void addHandlers()
        {
            foreach (Control control in tlpLeft.Controls.OfType<TextBox>())
            {
                control.TextChanged += new EventHandler(OnContentChanged);
            }

            foreach (Control control in tlpCenterTop.Controls.OfType<TextBox>())
            {
                control.TextChanged += new EventHandler(OnContentChanged);
            }

            foreach (Control control in tlpRight.Controls.OfType<RichTextBox>())
            {
                control.TextChanged += new EventHandler(OnContentChanged);
            }

            foreach (ComboBox control in tlpLeft.Controls.OfType<ComboBox>())
            {
                control.SelectedIndexChanged += new EventHandler(OnContentChanged);
            }

            foreach (ComboBox control in tlpCenterTop.Controls.OfType<ComboBox>())
            {
                control.SelectedIndexChanged += new EventHandler(OnContentChanged);
            }

            foreach (ComboBox control in tlpRight.Controls.OfType<ComboBox>())
            {
                control.SelectedIndexChanged += new EventHandler(OnContentChanged);
            }

        }

        private void dropHandlers()
        {
            foreach (Control control in tlpLeft.Controls.OfType<TextBox>())
            {
                control.TextChanged -= new EventHandler(OnContentChanged);
            }

            foreach (Control control in tlpCenterTop.Controls.OfType<TextBox>())
            {
                control.TextChanged -= new EventHandler(OnContentChanged);
            }

            foreach (Control control in tlpRight.Controls.OfType<RichTextBox>())
            {
                control.TextChanged -= new EventHandler(OnContentChanged);
            }

            foreach (ComboBox control in tlpLeft.Controls.OfType<ComboBox>())
            {
                control.SelectedIndexChanged -= new EventHandler(OnContentChanged);
            }

            foreach (ComboBox control in tlpCenterTop.Controls.OfType<ComboBox>())
            {
                control.SelectedIndexChanged -= new EventHandler(OnContentChanged);
            }

            foreach (ComboBox control in tlpRight.Controls.OfType<ComboBox>())
            {
                control.SelectedIndexChanged -= new EventHandler(OnContentChanged);
            }

        }

        protected void OnContentChanged(object sender, EventArgs e)
        {
            formDirty = true;
            
        }

        private void frmRequests_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formDirty)
            {
                checkForSave();
            }
            dropHandlers();
            GV.REQFORM = null;
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Restore;
            DialogResult result = confirmAction();
            if (result == DialogResult.Yes)
                {
                    RequestOps.RestoreRequest(Rm);
                }
            
            resetForm();
            this.Visible = false;
        }

        private void btnSearchFields_Click(object sender, EventArgs e)
        {
            GV.MODE = Mode.Search;
            List<TableLayoutPanel> tlpList = new List<TableLayoutPanel>();
            tlpList.Add(tlpLeft);
            tlpList.Add(tlpCenterTop);
            tlpList.Add(tlpRight);
            List<FieldSearchModel> searchList = new List<FieldSearchModel>();
            collectSearchTerms(ref searchList, tlpList);
            List<RequestModel> requests = DesignDB_Library.Operations.SearchOps.FieldSearch(searchList, true);
            frmMultiResult frmMultiResult = new frmMultiResult(requests);
            frmMultiResult.Show();formDirty = false;
        }

        private DialogResult confirmAction()
        {
            string msg = "";
            switch (GV.MODE)
            {
                case Mode.New:
                    break;
                case Mode.Edit:
                    break;
                case Mode.Revision:
                    msg = "Create revision of " + txtPID.Text + "?";
                    break;
                case Mode.Clone:
                    break;
                case Mode.Delete:
                    msg = "Delete " + txtPID.Text + "?";
                    break;
                case Mode.Export:
                    break;
                case Mode.Restore:
                    msg = "Restore " + txtPID.Text + "?";
                    break;
                case Mode.DateRangeSearch:
                    break;
                case Mode.Forecast:
                    break;
                case Mode.Report_OpenRequests:
                    break;
                case Mode.Report_CatMSO:
                    break;
                case Mode.Report_Snapshot:
                    break;
                case Mode.Report_AvgCompletion:
                    break;
                case Mode.Report_ByPriority:
                    break;
                case Mode.Report_DesignerLoadReport:
                    break;
                case Mode.Report_Overdue:
                    break;
                case Mode.Search:
                    break;
                case Mode.None:
                    break;
                default:
                    break;

            }
            DialogResult result = MessageBox.Show(msg, "Confirm Action",MessageBoxButtons.YesNo);
            return result;
        }

        private void collectSearchTerms(ref List<FieldSearchModel> fieldList, List<TableLayoutPanel> panelList)
        {
            foreach (TableLayoutPanel panel in panelList)
            {
                foreach (Control control in panel.Controls)
                {
                    if ((control is TextBox || control is ComboBox || control is RichTextBox || control is DateTimePicker)
                        && control.Text != "")
                    {
                        FieldSearchModel fieldSearch = new FieldSearchModel();
                        bool addDTP = true;
                        string tag = control.Tag.ToString();
                        string[] tags = tag.Split('|');
                        fieldSearch.FieldName = tags[1];

                        if (control is DateTimePicker)
                        {
                            DateTimePicker dtp = (DateTimePicker)control;
                            if (dtp.Format==DateTimePickerFormat.Custom)
                            {
                                addDTP = false;
                            }
                        }
                        fieldSearch.FieldValue = control.Text;
                        if (addDTP)
                        {
                            fieldList.Add(fieldSearch);
                        }
                    }
                }
            }
        }

        private void tlpCenterTop_Paint(object sender, PaintEventArgs e)
        {

        }

        private void clearValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ComboBox cbo = (ComboBox)ddContextMenu.SourceControl;
            cbo.SelectedIndex = -1;
        }

        private void clearDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTimePicker dtp = (DateTimePicker)dtpContextMenu.SourceControl;
            dtpResetForced(dtp);
        } 
    } 
}
