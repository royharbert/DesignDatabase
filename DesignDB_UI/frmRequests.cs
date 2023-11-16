using DesignDB_Library.Models;
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

/*
 * Control Tag Uses
 * Tag property of most controls is used to identify several items. The items are separated by a | then separated into an array by a string.split
 * [0] - "L" if control is locked on open, "U" if unlocked
 * [1] - request model column for field search function
 * [2] - database table holding drop-down list items
 * [3] - database table column holding drop-down list items
 * [4] - Excel column heading in list collector
 */

namespace DesignDB_UI
{

    public partial class frmRequests : Form
    {
        RequestModel Rm = new RequestModel();

        //Class scope variables to hold button name strings

        static string Level1ButtonNames = "btnUndo,btnSave,btnSearch,btnAddAtt,btnRemoveAtt,btnDone";
        static string Level2ButtonNames = Level1ButtonNames + ",btnDelete,btnRestore,btnClone,btnNew,btnRev,btnLoadRpt";
        static string RequestNewButtons = "btnUndo,btnSave,btnAddAtt,btnRemoveAtt,btnDone,btnClone,btnNew,btnRev";
        static string RequestEditButtons = "btnUndo,btnSave,btnAddAtt,btnRemoveAtt,btnDone,btnClone,btnNew,btnRev";
        static string RequestCloneButtons = "btnUndo,btnSave,btnAddAtt,btnRemoveAtt,btnDone,btnClone,btnNew,btnRev";
        static string RequestRevisionButtons = "btnUndo,btnSave,btnAddAtt,btnRemoveAtt,btnDone,btnClone,btnNew,btnRev";
        static string RequestDeleteButtons = "btnDelete";
        static string RequestRestoreButtons = "btnRestore";
        static string RequestSearchButtons = "btnSearchFields,btnNewSearch";
        static string RequestSearchFieldsButtons = "btnSearchFields";

        bool formLoading;
        bool formDirty;
        bool _useDefaultLocation = true;
        string activeControlOriginalValue;

        Point _formLocation = new Point(-1, -1);
        frmAttType frm = null;
        DateTime failDate = new DateTime(1900, 1, 1);
        RequestModel initialRequest = null;
        private List<LogFieldModel> logFieldList = new List<LogFieldModel>();

        public bool UseDefaultLocation
        {
            get
            {
                return _useDefaultLocation;
            }
        }

        public Point FormLocation
        {
            get
            {
                return _formLocation;
            }
        }

        public RequestModel Request
        {
            get
            {
                return Rm;
            }
            set
            {
                formLoading = true;
                Application.DoEvents();
                Rm = value;
                prepFormForTask();
               
                insertData(Rm);
                if (Rm.ProjectID != null && GV.MODE != Mode.New && GV.MODE != Mode.Delete && GV.MODE != Mode.Restore)
                {
                    GV.MODE = Mode.Edit;
                    setButtonDisplay(RequestEditButtons); 
                }
                
                getAttachments(txtPID.Text);
                if (GV.MODE == Mode.New)
                {
                    cboCountry.SelectedIndex = 188;
                }
                
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
            _formLocation = this.Location;
            FC.SetFormPosition(this, _formLocation.X, _formLocation.Y, UseDefaultLocation);
            this.BringToFront();
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
                case Mode.DateRangeSearch:
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

                case Mode.SearchFields:
                    setButtonDisplay(RequestSearchFieldsButtons);
                    break;

                case Mode.Add_Attachment:
                    break;
                case Mode.Delete_Attachment:
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
        private void setButtonDisplay(string displayString = "")
        {
            //hide all buttons
            foreach (Control ctl in tlpBottom.Controls)
            {
                if (ctl is Button)
                {
                    ctl.Visible = false;
                }
            }

            if (GV.USERNAME.Priviledge == 1 & displayString == "")
            {
                displayString = Level1ButtonNames;
            }
            else if (displayString == "")
            {
                displayString = Level2ButtonNames;
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
            DialogResult result = MessageBox.Show("Save Changes?", caption: "Save", buttons: MessageBoxButtons.YesNo);
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
            txtID.Text = rm.ID.ToString();
            txtPID.Text = rm.ProjectID;
            cboMSO.Text = rm.MSO;
            txtCust.Text = rm.Cust;
            cboCities.Text = rm.City;
            cboState.Text = rm.ST;
            cboCountry.Text = rm.Country;
            cboRegion.Text = setCombo(cboRegion, rm.Region);
            cboRequestor.Text = rm.DesignRequestor;
            cboQuoteType.Text = setCombo(cboQuoteType, rm.QuoteType);
            cboPriority.Text = setCombo(cboPriority, rm.Pty);
            cboDesigner.Text = setCombo(cboDesigner, rm.Designer);
            cboAssisted.Text = setCombo(cboAssisted, rm.AssistedBy);
            txtProjName.Text = rm.ProjectName;
            cboOrigQuote.Text = rm.OriginalQuote;
            cboCategory.Text = setCombo(cboCategory, rm.Category);
            cboArchType.Text = setCombo(cboArchType, rm.ArchitectureType);

            dtpLoadFromModel(txtDateAssigned, rm.DateAssigned);

            dtpLoadFromModel(txtDateAllInfo, rm.DateAllInfoReceived);

            dtpLoadFromModel(txtDateDue, rm.DateDue);

            dtpLoadFromModel(txtDateCompleted, rm.DateCompleted);
            cboReviewedBy.Text = setCombo(cboReviewedBy, rm.ReviewedBy);
            txtBOM_Val.Text = rm.BOM_Value.ToString("C2");
            txtPctCovered.Text = rm.PercentageProjectCovered.ToString();
            cboAwardStatus.Text = setCombo(cboAwardStatus, rm.AwardStatus);
            txtTotalHours.Text = rm.TotalHours.ToString();

            dtpLoadFromModel(txtLastUpdate, rm.DateLastUpdate);
            rtbArchDetails.Text = rm.ArchitectureDetails;
            rtbComments.Text = rm.Comments;
            txtTotalVal.Text = calcTotVal().ToString("C2");

            //set modelMSO since it does not come from DB
            Rm.msoModel = (MSO_Model)cboMSO.SelectedItem;
            initialRequest = CommonOps.CloneRequestList(Rm);
        }

        private string setCombo(ComboBox cbo, string val)
        {
            string rtn = "";
            if (val == "")
            {
                cbo.SelectedIndex = -1;
            }
            else
            {
                rtn = val;
            }
            return rtn;
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
            //cboMSO.SelectedIndex = Rm.msoModel.ID;
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
            Rm.DateAssigned = dtpReset(txtDateAssigned);
            Rm.DateAllInfoReceived = dtpReset(txtDateAllInfo);
            Rm.DateCompleted = dtpReset(txtDateCompleted);
            Rm.DateDue = dtpReset(txtDateDue);
            Rm.DateLastUpdate = dtpReset(txtLastUpdate);

            Rm.ReviewedBy = cboReviewedBy.Text;

            decimal BOM = 0;
            decimal.TryParse(txtBOM_Val.Text, NumberStyles.Currency, CultureInfo.CurrentCulture.NumberFormat, out BOM);
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
                dtp.Value = new DateTime(1900, 1, 1);
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
                    searchReset();
                    break;
                case Mode.SearchFields:
                    searchReset();
                    btnSearchFields.Visible = true;
                    btnSearchFields.Text = "Search";
                    //formLoading = false;
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

        /// <summary>
        /// Was used to populate designer, assisted by and reviewers dropdowns with only active members
        /// Was changed on 10/30/2023 to include all members
        /// </summary>
        private void MakeActiveDependantLists()
        {
            if (GV.MODE != Mode.Search)
            {
                //List<DesignersReviewersModel> activeDesignerList = GlobalConfig.Connection.DesignersGetActive();
                //activeDesignerList.Insert(0, new DesignersReviewersModel());
                //cboDesigner.DataSource = activeDesignerList;
                List<DesignersReviewersModel> designerList = GlobalConfig.Connection.GenericGetAll<DesignersReviewersModel>("tblDesigners", "Designer");
                designerList.Insert(0, new DesignersReviewersModel());
                cboDesigner.DataSource = designerList;
                cboDesigner.DisplayMember = "Designer";
                cboDesigner.SelectedIndex = -1;


                List<DesignersReviewersModel> assistedList = cloneList(designerList);
                cboAssisted.DataSource = assistedList;
                cboAssisted.DisplayMember = "Designer";
                cboAssisted.SelectedIndex = -1;

                List<DesignersReviewersModel> reviewerList = GlobalConfig.Connection.GenericGetAll<DesignersReviewersModel>("tblReviewers", "Designer");
                reviewerList.Insert(0, new DesignersReviewersModel());
                cboReviewedBy.DataSource = reviewerList;
                cboReviewedBy.DisplayMember = "Designer";
                cboReviewedBy.SelectedIndex = -1;

                if (formLoading )
                {
                    //List<MSO_Model> msoList = GlobalConfig.Connection.GetAllActiveMSO();
                    List<MSO_Model> msoList = GlobalConfig.Connection.GenericGetAll<MSO_Model>("tblMSO", "MSO");
                    cboMSO.DataSource = msoList;
                    cboMSO.DisplayMember = "MSO";
                    cboMSO.SelectedIndex = -1; 
                }

                //List<SalespersonModel> salesList = GlobalConfig.Connection.SalesGetActive();
                List<SalespersonModel> salesList = GlobalConfig.Connection.GenericGetAll<SalespersonModel>("tblSalespersons", "SalesPerson");
                salesList.Insert(0, new SalespersonModel());
                cboRequestor.DataSource = salesList;
                cboRequestor.DisplayMember = "SalesPerson";
                cboRequestor.SelectedIndex = -1;
            }
            else
            {
                List<DesignersReviewersModel> activeDesignerList = GlobalConfig.Connection.GetAllDesigners();
                activeDesignerList.Insert(0, new DesignersReviewersModel());
                cboDesigner.DataSource = activeDesignerList;
                cboDesigner.DisplayMember = "Designer";
                cboDesigner.SelectedIndex = -1;


                List<DesignersReviewersModel> assistedList = cloneList(activeDesignerList);
                cboAssisted.DataSource = assistedList;
                cboAssisted.DisplayMember = "Designer";
                cboAssisted.SelectedIndex = -1;

                List<DesignersReviewersModel> reviewerList = GlobalConfig.Connection.Reviewers_GetAll();
                reviewerList.Insert(0, new DesignersReviewersModel());
                cboReviewedBy.DataSource = reviewerList;
                cboReviewedBy.DisplayMember = "Designer";
                cboReviewedBy.SelectedIndex = -1;

                List<MSO_Model> msoList = GlobalConfig.Connection.GetAllMSO();
                cboMSO.DataSource = msoList;
                cboMSO.DisplayMember = "MSO";
                cboMSO.SelectedIndex = -1;


                List<SalespersonModel> salesList = GlobalConfig.Connection.SalespersonsGetAll();
                salesList.Insert(0, new SalespersonModel());
                cboRequestor.DataSource = salesList;
                cboRequestor.DisplayMember = "SalesPerson";
                cboRequestor.SelectedIndex = -1;
            }
        }
        private void makeLists()
        {
            MakeActiveDependantLists();
            
            

            List<CityModel> cityList = GlobalConfig.Connection.GetAllCities();
            cboCities.DataSource = cityList;
            cboCities.DisplayMember = "City";
            cboCities.SelectedIndex = -1;

            List<StateModel> stateList = GlobalConfig.Connection.GetAllStates();
            stateList.Insert(0, new StateModel());
            cboState.DataSource = stateList;
            cboState.DisplayMember = "State";
            cboState.SelectedIndex = -1;

            List<CountriesModel> countryList = GlobalConfig.Connection.GetAllCountries();
            countryList.Insert(0, new CountriesModel());
            cboCountry.DataSource = countryList;
            cboCountry.DisplayMember = "Country";
            cboCountry.SelectedIndex = 188;

            List<RegionsModel> regionList = GlobalConfig.Connection.GetAllRegions();
            regionList.Insert(0, new RegionsModel());
            cboRegion.DataSource = regionList;
            cboRegion.DisplayMember = "Region";
            cboRegion.SelectedIndex = -1;

        }

        private void cboMSO_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!formLoading)
            {
                if (cboMSO.SelectedIndex > -1 && GV.MODE == Mode.New)
                {
                    MSO_Model mso = (MSO_Model)cboMSO.SelectedItem;
                    string PID = PID_Generator.MakePID(mso);
                    txtPID.Text = PID;
                    unlockTLP(true);
                    addToLogAffectedFields("MSO", cboMSO.Text);
                }
            }
        }


        private void btnDone_Click(object sender, EventArgs e)
        {
            if (formDirty)
            {
                checkForSave();
            }

            //GV.MODE = Mode.None;
            changeMode(Mode.None);
            resetForm();
            logFieldList.Clear();
            GV.REQFORM.Hide();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveChanges();

        }

        private void logSuccessfulSave(int saveSuccessful)
        {
            if (saveSuccessful > -1)
            {
                makeLogEntry(logFieldList);
            }
        }

        private void saveChanges()
        {
            int saved = -1;
            bool updated = false;
            if (Rm == null)
            {
                Rm = new RequestModel();
            }
            loadModel(); 

            Rm.DateLastUpdate = DateTime.Today;
            switch (GV.MODE)
            {
                case Mode.New:
                case Mode.Revision:
                    saved = GlobalConfig.Connection.RequestInsert(Rm);
                    if (saved > -1)
                    {
                        logSuccessfulSave(saved);
                        changeMode(Mode.Edit);
                        MessageBox.Show(Rm.ProjectID + " successfully saved.");
                        txtID.Text = saved.ToString();
                    }
                    else 
                    {
                        MessageBox.Show(Rm.ProjectID + " Not saved", "DB Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                case Mode.Edit:
                    updated = GlobalConfig.Connection.RequestUpdate(Rm);
                    if (updated)
                    {
                        logSuccessfulSave(saved);
                        MessageBox.Show(Rm.ProjectID + " successfully saved.");
                    }
                    else
                    {
                        MessageBox.Show(Rm.ProjectID + " Not saved", "DB Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                case Mode.Clone:
                    saved = RequestOps.InsertNewRequest(Rm);
                    if (saved > -1)
                    {
                        logSuccessfulSave(saved);
                        MessageBox.Show(Rm.ProjectID + " successfully saved.");
                        txtID.Text = saved.ToString();
                    }
                    else
                    {
                        MessageBox.Show(Rm.ProjectID + " Not saved", "DB Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    }
                    changeMode(Mode.Edit);
                    break;
                case Mode.Delete:
                    break;
                case Mode.None:
                    break;
                default:
                    break;
            }

            //if (saved == 1)
            //{
            //    MessageBox.Show("Record " + Rm.ProjectID + " Saved.");
            //}

            formDirty = false;
        }

        private void btnClone_Click(object sender, EventArgs e)
        {
            saveChanges();
            //GV.MODE = Mode.Clone;
            changeMode(Mode.Clone);
            //Rm.msoModel = cboMSO.SelectedItem as MSO_Model;
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
            //GV.MODE = Mode.New;
            changeMode(Mode.New);
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
            //GV.MODE = Mode.Revision;
            changeMode(Mode.Revision);
            if (formDirty)
            {
                checkForSave();
            }
            DialogResult result = confirmAction();

            if (result == DialogResult.Yes)
            {
                this.Request = RequestOps.CreateRevision(Rm);
            }
            changeMode(Mode.Revision);
            //set combo boxes to ""
            resetCombo(cboReviewedBy);
            resetCombo(cboPriority);
            resetCombo(cboArchType);
            resetCombo(cboCategory);
            resetCombo(cboAssisted);
            dtpResetForced(txtDateDue);
            dtpResetForced(txtDateAllInfo);
            dtpResetForced(txtLastUpdate);
            loadModel();
            unlockTLP(true);
        }

        private void resetCombo(ComboBox cbo)
        {
            cbo.SelectedIndex = -1;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //GV.MODE = Mode.Delete;
            changeMode(Mode.Delete);
            DialogResult result = confirmAction();
            if (result == DialogResult.Yes)
            {
                RequestOps.DeleteRequest(Rm);
                prepForButtonLogEntry(Rm.ProjectID);
            }
            resetForm();
            this.Visible = false;
        }

        private void btnAddAtt_Click(object sender, EventArgs e)
        {
            //GV.MODE = Mode.Add_Attachment;
            changeMode(Mode.Add_Attachment);
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
                FC.SetFormPosition(frm);
                this.BringToFront();
                frm.ShowDialog();
            }

            frm.TypeReadyEvent -= Frm_TypeReadyEvent;
            prepForButtonLogEntry(model.DisplayText);
        }

        private void prepForButtonLogEntry(string attachment)
        {
            List<LogFieldModel> logFields = new List<LogFieldModel>();
            LogFieldModel logModel = new LogFieldModel();
            logModel.FieldName = attachment;
            logFields.Add(logModel);
            makeLogEntry(logFields);
            //logFieldList = logFields;
            GV.MODE = GV.PreviousMode;
        }



        private void Frm_TypeReadyEvent(object sender, AttachmentModel e)
        {
            string fileName = GlobalConfig.AttachmentPath + "\\" + e.PID + "\\" + e.DisplayText;
            bool fileSaved = FileOps.SaveAttFile(e);
            if (fileSaved)
            {
                GlobalConfig.Connection.InsertInto_tblAttachments(e);
                List<AttachmentModel> aList = GlobalConfig.Connection.GetAttachments(e.PID);
                dgvAttachments.DataSource = null;
                dgvAttachments.DataSource = aList;
                formatAttGrid(); 
            }
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
            try
            {
                Process.Start(sinfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + fileName);
            }
        }

        private void btnRemoveAtt_Click(object sender, EventArgs e)
        {
            //use class to accomplish
            //make attachment model and pass to class
            //GV.MODE = Mode.Delete_Attachment;
            changeMode(Mode.Delete_Attachment);
            if (dgvAttachments.CurrentRow != null)
            {
                int sel = dgvAttachments.CurrentRow.Index;
                List<AttachmentModel> aList = (List<AttachmentModel>)dgvAttachments.DataSource;
                AttachmentModel model = aList[sel];
                List<AttachmentModel> newList = new List<AttachmentModel>();
                


                try
                {
                    newList = AttachmentOps.DeleteAttachment(model);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("network path was not found"))
                    {
                        MessageBox.Show("Cannot connect to server. Check VPN and network settings");
                    }
                }
                dgvAttachments.DataSource = null;
                dgvAttachments.DataSource = newList;
                formatAttGrid();
                prepForButtonLogEntry(model.DisplayText);  
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
            //GV.MODE = Mode.Undo;
            changeMode(Mode.Undo);
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
            if (!formLoading && GV.MODE != Mode.Undo)
            {
                dtp.Format = DateTimePickerFormat.Short;
                formDirty = true;
            }
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
            if (!GV.Exiting)
            {
                if (formDirty)
                {
                    checkForSave();
                }
                dropHandlers();
                //GV.REQFORM = null;
                e.Cancel = true;
                this.Hide();
                dgvAttachments.DataSource = null; 
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            //GV.MODE = Mode.Restore;
            changeMode(Mode.Restore);
            DialogResult result = confirmAction();
            if (result == DialogResult.Yes)
            {
                RequestOps.RestoreRequest(Rm);
                prepForButtonLogEntry(Rm.ProjectID);
            }

            resetForm();
            this.Visible = false;
        }

        private void btnSearchFields_Click(object sender, EventArgs e)
        {
            if (btnSearchFields.Text == "Search")
            {List<RequestModel> requests = new List<RequestModel>();
                changeMode(Mode.SearchFields);
                List<TableLayoutPanel> tlpList = new List<TableLayoutPanel>();
                tlpList.Add(tlpLeft);
                tlpList.Add(tlpCenterTop);
                tlpList.Add(tlpRight);
                List<FieldSearchModel> searchList = new List<FieldSearchModel>();
                collectSearchTerms(ref searchList, tlpList);
                if (!ckFilter.Checked)
                {
                    requests = SearchOps.FieldSearch(searchList, true); 
                }
                else
                {
                    requests = SearchOps.FieldSearch(searchList, true, dtpStart.Value, dtpEnd.Value);
                }
                //frmMultiResult frmMultiResult = new frmMultiResult(requests);
                GV.MultiResult.dataList = requests;
                GV.MultiResult.Show();
                formDirty = false;

                btnSearchFields.Text = "New Search"; 
            }
            else
            {
                //GV.MODE = Mode.Search;
                changeMode(Mode.SearchFields);
                btnSearchFields.Text = "Search";
                searchReset();
            }
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
            DialogResult result = MessageBox.Show(msg, "Confirm Action", MessageBoxButtons.YesNo);
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
                            if (dtp.Format == DateTimePickerFormat.Custom)
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

        private void clearValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ComboBox cbo = (ComboBox)ddContextMenu.SourceControl;
            cbo.SelectedIndex = -1;
        }

        private void miCopy_Click(object sender, EventArgs e)
        {
            // Try to cast the sender to a ToolStripItem
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    RichTextBox rtb = (RichTextBox)sourceControl;
                    rtb.Copy();
                }
            }
        }

        private void miCut_Click(object sender, EventArgs e)
        {
            // Try to cast the sender to a ToolStripItem
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    RichTextBox rtb = (RichTextBox)sourceControl;
                    rtb.Cut();
                }
            }
        }

        private void miPaste_Click(object sender, EventArgs e)
        {
            // Try to cast the sender to a ToolStripItem
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    Control sourceControl = owner.SourceControl;
                    RichTextBox rtb = (RichTextBox)sourceControl;
                    rtb.Paste();
                }
            }
        }

        private void clearDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTimePicker dtp = (DateTimePicker)dtpContextMenu.SourceControl;
            dtpResetForced(dtp);
        }

        private void frmRequests_Activated(object sender, EventArgs e)
        {
            FC.SetFormPosition(this, _formLocation.X, _formLocation.Y, _useDefaultLocation);
            if (GV.MODE == Mode.Edit)
            {
                btnSearchFields.Text = "Search";
            }

            //if (GV.MODE == Mode.SearchFields)
            //{
            //    gbDateRange.Visible = true;
            //    tlpSearch.Visible = false;
            //}
            //else
            //{
            //    gbDateRange.Visible = false;
            //    tlpSearch.Visible = true;
            //}
            this.Text = "Design Request - Mode: " + GV.MODE;
            
            //MakeActiveDependantLists();

        }

        private void txtDateCompleted_DropDown(object sender, EventArgs e)
        {
            dtpForceDate(txtDateCompleted);
        }

        private void dtpForceDate(DateTimePicker dtp)
        {
            dtp.Value = DateTime.Today;
        }

        private void txtDateAllInfo_DropDown(object sender, EventArgs e)
        {
            dtpForceDate(txtDateAllInfo);
        }

        private void frmRequests_Move(object sender, EventArgs e)
        {
            _formLocation = this.Location;
            if (!formLoading)
            {
                _useDefaultLocation = false;
            }
            switch (GV.MODE)
            {
                case Mode.Search:
                case Mode.SearchFields:
                    break;
                default:
                    break;
            }
            Application.DoEvents();
        }
    #region Log processes
        private void makeLogEntry(List<LogFieldModel> fields)
        {
            LogModel logModel = new LogModel();
            logModel.TimeStamp = DateTime.Now;
            logModel.User = GV.USERNAME.Designer.Trim();
            logModel.Action = GV.MODE.ToString();
            string xmlFields = Serialization.SerializeToXml<List<LogFieldModel>>(fields);
            logModel.AffectedFields = xmlFields;
            logModel.RequestID = txtPID.Text;
            Logger.WriteToLog(logModel);
            logModel = null;
        }
        private void addToLogAffectedFields(string fieldName, string fieldValue)
        {
            if (fieldValue != activeControlOriginalValue)
            {
                LogFieldModel fieldModel = new LogFieldModel();
                fieldModel.FieldName = fieldName;
                fieldModel.OriginalValue = activeControlOriginalValue;
                fieldModel.NewValue = fieldValue;
                logFieldList.Add(fieldModel);
            }
        }
        #region Make LogFieldData
        private void txtCust_Leave(object sender, EventArgs e)
        {
            addToLogAffectedFields("End Customer", txtCust.Text);
        }

        private void cboCities_Leave(object sender, EventArgs e)
        {
            addToLogAffectedFields("City", cboCities.Text);
        }

        private void cboState_Leave(object sender, EventArgs e)
        {
            addToLogAffectedFields("State", cboState.Text);
        }

        private void cboCountry_Leave(object sender, EventArgs e)
        {
            addToLogAffectedFields("Country", cboCountry.Text);
        }

        private void cboRegion_Leave(object sender, EventArgs e)
        {
            addToLogAffectedFields("Region", cboRegion.Text);
        }

        private void cboRequestor_Leave(object sender, EventArgs e)
        {
            addToLogAffectedFields("Design Requestor", cboRequestor.Text);
        }

        private void cboQuoteType_Leave(object sender, EventArgs e)
        {
            addToLogAffectedFields("Quote Type", cboQuoteType.Text);
        }

        private void cboPriority_Leave(object sender, EventArgs e)
        {
            addToLogAffectedFields("Priority", cboPriority.Text);
        }

        private void cboDesigner_Leave(object sender, EventArgs e)
        {
            addToLogAffectedFields("Designer", cboDesigner.Text);
        }

        private void cboAssisted_Leave(object sender, EventArgs e)
        {
            addToLogAffectedFields("Assisted By", cboAssisted.Text);
        }

        private void txtProjName_Leave(object sender, EventArgs e)
        {
            addToLogAffectedFields("Project Name", txtProjName.Text);
        }

        private void cboOrigQuote_Leave(object sender, EventArgs e)
        {
            addToLogAffectedFields("Original Quote", cboOrigQuote.Text);
        }

        private void cboCategory_Leave(object sender, EventArgs e)
        {
            addToLogAffectedFields("Category", cboCategory.Text);
        }

        private void cboArchType_Leave(object sender, EventArgs e)
        {
            addToLogAffectedFields("Architecture Type", cboArchType.Text);
        }

        private void txtDateAssigned_Leave(object sender, EventArgs e)
        {
            addToLogAffectedFields("Date Assigned", txtDateAssigned.Text);
        }

        private void txtDateAllInfo_Leave(object sender, EventArgs e)
        {
            addToLogAffectedFields("Date All Info Received", txtDateAllInfo.Text);
        }

        private void txtDateDue_Leave(object sender, EventArgs e)
        {
            addToLogAffectedFields("Date Due", txtDateDue.Text);
        }

        private void txtDateCompleted_Leave(object sender, EventArgs e)
        {
            addToLogAffectedFields("Date Completed", txtDateCompleted.Text);
        }

        private void cboReviewedBy_Leave(object sender, EventArgs e)
        {
            addToLogAffectedFields("Reviewed By", cboReviewedBy.Text);
        }

        private void txtBOM_Val_Leave(object sender, EventArgs e)
        {
            addToLogAffectedFields("BOM Value", txtBOM_Val.Text);
            txtTotalVal.Text = calcTotVal().ToString("C2");
        }

        private void txtPctCovered_Leave(object sender, EventArgs e)
        {
            addToLogAffectedFields("Percent Covered", txtPctCovered.Text);
            txtTotalVal.Text = calcTotVal().ToString("C2");
        }

        private void cboAwardStatus_Leave(object sender, EventArgs e)
        {
            addToLogAffectedFields("Award Status", cboAwardStatus.Text);
        }

        private void txtTotalHours_Leave(object sender, EventArgs e)
        {
            addToLogAffectedFields("Total Hours", txtTotalHours.Text);
        }

        private void txtLastUpdate_Leave(object sender, EventArgs e)
        {
            addToLogAffectedFields("Date Last Updated", txtLastUpdate.Text);
        }

        private void rtbArchDetails_Leave(object sender, EventArgs e)
        {
            addToLogAffectedFields("Architecture Details", rtbArchDetails.Text);
        }

        private void rtbComments_Leave(object sender, EventArgs e)
        {
            addToLogAffectedFields("Comments", rtbComments.Text);
        }
        #endregion
        #region Initial Values
        private void txtCust_Enter(object sender, EventArgs e)
        {
            activeControlOriginalValue = txtCust.Text;
        }

        private void cboCities_Enter(object sender, EventArgs e)
        {
            activeControlOriginalValue = cboCities.Text;
        }

        private void cboState_Enter(object sender, EventArgs e)
        {
            activeControlOriginalValue = cboState.Text;
        }

        private void cboCountry_Enter(object sender, EventArgs e)
        {
            activeControlOriginalValue = cboCountry.Text;
        }

        private void cboRegion_Enter(object sender, EventArgs e)
        {
            activeControlOriginalValue = cboRegion.Text;
        }

        private void cboRequestor_Enter(object sender, EventArgs e)
        {
            activeControlOriginalValue = cboRequestor.Text;
        }

        private void cboQuoteType_Enter(object sender, EventArgs e)
        {
            activeControlOriginalValue = cboQuoteType.Text;
        }

        private void cboPriority_Enter(object sender, EventArgs e)
        {
            activeControlOriginalValue = cboPriority.Text;
        }

        private void cboDesigner_Enter(object sender, EventArgs e)
        {
            activeControlOriginalValue = cboDesigner.Text;
        }

        private void cboAssisted_Enter(object sender, EventArgs e)
        {
            activeControlOriginalValue = cboAssisted.Text;
        }

        private void txtProjName_Enter(object sender, EventArgs e)
        {
            activeControlOriginalValue = txtProjName.Text;
        }

        private void cboOrigQuote_Enter(object sender, EventArgs e)
        {
            activeControlOriginalValue = cboOrigQuote.Text;
        }

        private void cboCategory_Enter(object sender, EventArgs e)
        {
            activeControlOriginalValue = cboCategory.Text;
        }

        private void cboArchType_Enter(object sender, EventArgs e)
        {
            activeControlOriginalValue = cboArchType.Text;
        }

        private void txtDateAssigned_Enter(object sender, EventArgs e)
        {
            activeControlOriginalValue = txtDateAssigned.Text;
        }

        private void txtDateAllInfo_Enter(object sender, EventArgs e)
        {
            activeControlOriginalValue = txtDateAllInfo.Text;
        }

        private void txtDateDue_Enter(object sender, EventArgs e)
        {
            activeControlOriginalValue = txtDateDue.Text;
        }

        private void txtDateCompleted_Enter(object sender, EventArgs e)
        {
            activeControlOriginalValue = txtDateCompleted.Text;
        }

        private void cboReviewedBy_Enter(object sender, EventArgs e)
        {
            activeControlOriginalValue = cboReviewedBy.Text;
        }

        private void txtBOM_Val_Enter(object sender, EventArgs e)
        {
            activeControlOriginalValue = txtBOM_Val.Text;
        }

        private void txtPctCovered_Enter(object sender, EventArgs e)
        {
            activeControlOriginalValue = txtPctCovered.Text;
        }

        private void cboAwardStatus_Enter(object sender, EventArgs e)
        {
            activeControlOriginalValue = cboAwardStatus.Text;
        }

        private void txtTotalHours_Enter(object sender, EventArgs e)
        {
            activeControlOriginalValue = txtTotalHours.Text;
        }

        private void txtLastUpdate_Enter(object sender, EventArgs e)
        {
            activeControlOriginalValue = txtLastUpdate.Text;
        }

        private void rtbArchDetails_Enter(object sender, EventArgs e)
        {
            activeControlOriginalValue = rtbArchDetails.Text;
        }

        private void rtbComments_Enter(object sender, EventArgs e)
        {
            activeControlOriginalValue = rtbComments.Text;
        }

        #endregion

        #endregion

      

        private void searchReset()
        {
            formLoading = true;
            Rm = new RequestModel();
            generalReset();
            formLoading = true;
            txtBOM_Val.Clear();
            txtPctCovered.Clear();
            txtTotalVal.Clear();
            txtTotalHours.Clear();
            cboCountry.SelectedIndex = -1;
            cboAwardStatus.SelectedIndex = -1;
            dtpResetForced(txtDateAssigned);
            unlockTLP(true);
            dgvAttachments.DataSource = "";
            formDirty = false;            
        }

        public void setDateRangeControls()
        {
            switch (GV.MODE)
            {
                case Mode.Search:
                case Mode.SearchFields:
                    //tlpSearch.Visible = false;
                    break;
                default:
                    //tlpSearch.Visible = true;
                    break;
            }
        }

        private void cboMSO_TextChanged(object sender, EventArgs e)
        {
            if (!formLoading && GV.MODE != Mode.New &&GV.MODE != Mode.Revision)
            {
                //GV.MODE = Mode.Edit;
                if (GV.MODE != Mode.Clone)
                {
                    changeMode(Mode.Edit); 
                }
                formDirty = true;
            }
        }

        private void changeMode(Mode mode) 
        {
            GV.MODE = mode;
            prepFormForTask();
        }

        private void dtpStart_ValueChanged(object sender, EventArgs e)
        {
            ckFilter.Checked = true;
        }

        private void dtpEnd_ValueChanged(object sender, EventArgs e)
        {
            ckFilter.Checked = true;
        }
    }   
}
