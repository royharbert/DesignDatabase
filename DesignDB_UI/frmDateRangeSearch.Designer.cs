
namespace DesignDB_UI
{
    partial class frmDateRangeSearch
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.dtpStart = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbxSearchTerm = new System.Windows.Forms.GroupBox();
            this.rdoDateCompleted = new System.Windows.Forms.RadioButton();
            this.rdoDateDue = new System.Windows.Forms.RadioButton();
            this.rdoDate = new System.Windows.Forms.RadioButton();
            this.btnForecast = new System.Windows.Forms.Button();
            this.lblDesigner = new System.Windows.Forms.Label();
            this.cboDesigner = new System.Windows.Forms.ComboBox();
            this.lblRequestor = new System.Windows.Forms.Label();
            this.cboRequestor = new System.Windows.Forms.ComboBox();
            this.btnClearTiers = new System.Windows.Forms.Button();
            this.ckUnclassified = new System.Windows.Forms.CheckBox();
            this.ckTier2 = new System.Windows.Forms.CheckBox();
            this.ckTier1 = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnDeselect = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.lbMSO = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnClearRegions = new System.Windows.Forms.Button();
            this.ckOther = new System.Windows.Forms.CheckBox();
            this.ckUSWest = new System.Windows.Forms.CheckBox();
            this.ckUSEast = new System.Windows.Forms.CheckBox();
            this.ckRussia = new System.Windows.Forms.CheckBox();
            this.ckMiddleEast = new System.Windows.Forms.CheckBox();
            this.ckLatinAmerica = new System.Windows.Forms.CheckBox();
            this.ckIndia = new System.Windows.Forms.CheckBox();
            this.ckEurope = new System.Windows.Forms.CheckBox();
            this.ckCaribbean = new System.Windows.Forms.CheckBox();
            this.ckCanada = new System.Windows.Forms.CheckBox();
            this.ckAustralia = new System.Windows.Forms.CheckBox();
            this.ckAsia = new System.Windows.Forms.CheckBox();
            this.ckAfrica = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.gbxSearchTerm.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Location = new System.Drawing.Point(324, 47);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(309, 40);
            this.dtpStartDate.TabIndex = 0;
            // 
            // dtpStart
            // 
            this.dtpStart.AutoSize = true;
            this.dtpStart.Location = new System.Drawing.Point(229, 51);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(145, 33);
            this.dtpStart.TabIndex = 1;
            this.dtpStart.Text = "Start Date";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(229, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 33);
            this.label1.TabIndex = 3;
            this.label1.Text = "End Date";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Location = new System.Drawing.Point(324, 92);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(309, 40);
            this.dtpEndDate.TabIndex = 2;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(351, 820);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(135, 43);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(629, 820);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(135, 43);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gbxSearchTerm
            // 
            this.gbxSearchTerm.Controls.Add(this.rdoDateCompleted);
            this.gbxSearchTerm.Controls.Add(this.rdoDateDue);
            this.gbxSearchTerm.Controls.Add(this.rdoDate);
            this.gbxSearchTerm.Location = new System.Drawing.Point(16, 21);
            this.gbxSearchTerm.Name = "gbxSearchTerm";
            this.gbxSearchTerm.Size = new System.Drawing.Size(192, 140);
            this.gbxSearchTerm.TabIndex = 8;
            this.gbxSearchTerm.TabStop = false;
            this.gbxSearchTerm.Text = "Search Term";
            // 
            // rdoDateCompleted
            // 
            this.rdoDateCompleted.AutoSize = true;
            this.rdoDateCompleted.Location = new System.Drawing.Point(16, 98);
            this.rdoDateCompleted.Name = "rdoDateCompleted";
            this.rdoDateCompleted.Size = new System.Drawing.Size(250, 37);
            this.rdoDateCompleted.TabIndex = 2;
            this.rdoDateCompleted.Text = "Date Completed";
            this.rdoDateCompleted.UseVisualStyleBackColor = true;
            this.rdoDateCompleted.CheckedChanged += new System.EventHandler(this.rdoDateCompleted_CheckedChanged);
            // 
            // rdoDateDue
            // 
            this.rdoDateDue.AutoSize = true;
            this.rdoDateDue.Location = new System.Drawing.Point(16, 64);
            this.rdoDateDue.Name = "rdoDateDue";
            this.rdoDateDue.Size = new System.Drawing.Size(162, 37);
            this.rdoDateDue.TabIndex = 1;
            this.rdoDateDue.Text = "Date Due";
            this.rdoDateDue.UseVisualStyleBackColor = true;
            this.rdoDateDue.CheckedChanged += new System.EventHandler(this.rdoDateDue_CheckedChanged);
            // 
            // rdoDate
            // 
            this.rdoDate.AutoSize = true;
            this.rdoDate.Checked = true;
            this.rdoDate.Location = new System.Drawing.Point(16, 30);
            this.rdoDate.Name = "rdoDate";
            this.rdoDate.Size = new System.Drawing.Size(229, 37);
            this.rdoDate.TabIndex = 0;
            this.rdoDate.TabStop = true;
            this.rdoDate.Text = "Date Assigned";
            this.rdoDate.UseVisualStyleBackColor = true;
            this.rdoDate.CheckedChanged += new System.EventHandler(this.rdoDate_CheckedChanged);
            // 
            // btnForecast
            // 
            this.btnForecast.Location = new System.Drawing.Point(492, 820);
            this.btnForecast.Name = "btnForecast";
            this.btnForecast.Size = new System.Drawing.Size(131, 43);
            this.btnForecast.TabIndex = 9;
            this.btnForecast.Text = "Forecast";
            this.btnForecast.UseVisualStyleBackColor = true;
            this.btnForecast.Click += new System.EventHandler(this.btnForecast_Click);
            // 
            // lblDesigner
            // 
            this.lblDesigner.AutoSize = true;
            this.lblDesigner.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDesigner.Location = new System.Drawing.Point(331, 217);
            this.lblDesigner.Name = "lblDesigner";
            this.lblDesigner.Size = new System.Drawing.Size(111, 29);
            this.lblDesigner.TabIndex = 11;
            this.lblDesigner.Text = "Designer";
            // 
            // cboDesigner
            // 
            this.cboDesigner.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboDesigner.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboDesigner.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboDesigner.FormattingEnabled = true;
            this.cboDesigner.Location = new System.Drawing.Point(410, 209);
            this.cboDesigner.Name = "cboDesigner";
            this.cboDesigner.Size = new System.Drawing.Size(279, 37);
            this.cboDesigner.TabIndex = 10;
            // 
            // lblRequestor
            // 
            this.lblRequestor.AutoSize = true;
            this.lblRequestor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRequestor.Location = new System.Drawing.Point(320, 246);
            this.lblRequestor.Name = "lblRequestor";
            this.lblRequestor.Size = new System.Drawing.Size(125, 29);
            this.lblRequestor.TabIndex = 13;
            this.lblRequestor.Text = "Requestor";
            // 
            // cboRequestor
            // 
            this.cboRequestor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboRequestor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboRequestor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboRequestor.FormattingEnabled = true;
            this.cboRequestor.Location = new System.Drawing.Point(410, 243);
            this.cboRequestor.Name = "cboRequestor";
            this.cboRequestor.Size = new System.Drawing.Size(279, 37);
            this.cboRequestor.TabIndex = 12;
            // 
            // btnClearTiers
            // 
            this.btnClearTiers.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearTiers.Location = new System.Drawing.Point(351, 413);
            this.btnClearTiers.Name = "btnClearTiers";
            this.btnClearTiers.Size = new System.Drawing.Size(125, 29);
            this.btnClearTiers.TabIndex = 66;
            this.btnClearTiers.Text = "Clear Tiers";
            this.btnClearTiers.UseVisualStyleBackColor = true;
            this.btnClearTiers.Click += new System.EventHandler(this.btnClearTiers_Click);
            // 
            // ckUnclassified
            // 
            this.ckUnclassified.AutoSize = true;
            this.ckUnclassified.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckUnclassified.Location = new System.Drawing.Point(351, 383);
            this.ckUnclassified.Name = "ckUnclassified";
            this.ckUnclassified.Size = new System.Drawing.Size(170, 33);
            this.ckUnclassified.TabIndex = 65;
            this.ckUnclassified.Tag = "0";
            this.ckUnclassified.Text = "Unclassified";
            this.ckUnclassified.UseVisualStyleBackColor = true;
            this.ckUnclassified.CheckedChanged += new System.EventHandler(this.ckTier1_CheckedChanged);
            // 
            // ckTier2
            // 
            this.ckTier2.AutoSize = true;
            this.ckTier2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckTier2.Location = new System.Drawing.Point(351, 351);
            this.ckTier2.Name = "ckTier2";
            this.ckTier2.Size = new System.Drawing.Size(134, 33);
            this.ckTier2.TabIndex = 64;
            this.ckTier2.Tag = "2";
            this.ckTier2.Text = "Tiers 2/3";
            this.ckTier2.UseVisualStyleBackColor = true;
            this.ckTier2.CheckedChanged += new System.EventHandler(this.ckTier1_CheckedChanged);
            // 
            // ckTier1
            // 
            this.ckTier1.AutoSize = true;
            this.ckTier1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckTier1.Location = new System.Drawing.Point(351, 321);
            this.ckTier1.Name = "ckTier1";
            this.ckTier1.Size = new System.Drawing.Size(102, 33);
            this.ckTier1.TabIndex = 63;
            this.ckTier1.Tag = "1";
            this.ckTier1.Text = "Tier 1";
            this.ckTier1.UseVisualStyleBackColor = true;
            this.ckTier1.CheckedChanged += new System.EventHandler(this.ckTier1_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(335, 298);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(178, 29);
            this.label4.TabIndex = 62;
            this.label4.Text = "Tier Selection";
            // 
            // btnDeselect
            // 
            this.btnDeselect.Location = new System.Drawing.Point(173, 804);
            this.btnDeselect.Name = "btnDeselect";
            this.btnDeselect.Size = new System.Drawing.Size(110, 30);
            this.btnDeselect.TabIndex = 69;
            this.btnDeselect.Text = "Clear";
            this.btnDeselect.UseVisualStyleBackColor = true;
            this.btnDeselect.Click += new System.EventHandler(this.btnDeselect_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(31, 804);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(110, 30);
            this.btnSelectAll.TabIndex = 68;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // lbMSO
            // 
            this.lbMSO.FormattingEnabled = true;
            this.lbMSO.ItemHeight = 33;
            this.lbMSO.Location = new System.Drawing.Point(16, 209);
            this.lbMSO.Name = "lbMSO";
            this.lbMSO.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbMSO.Size = new System.Drawing.Size(276, 565);
            this.lbMSO.TabIndex = 67;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 182);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(325, 29);
            this.label3.TabIndex = 70;
            this.label3.Text = "Optional MSO Selection(s)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(322, 179);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(264, 29);
            this.label5.TabIndex = 71;
            this.label5.Text = "Other Optional Fields";
            // 
            // btnClearRegions
            // 
            this.btnClearRegions.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearRegions.Location = new System.Drawing.Point(548, 711);
            this.btnClearRegions.Name = "btnClearRegions";
            this.btnClearRegions.Size = new System.Drawing.Size(125, 29);
            this.btnClearRegions.TabIndex = 86;
            this.btnClearRegions.Text = "Clear Regions";
            this.btnClearRegions.UseVisualStyleBackColor = true;
            this.btnClearRegions.Click += new System.EventHandler(this.btnClearRegions_Click);
            // 
            // ckOther
            // 
            this.ckOther.AutoSize = true;
            this.ckOther.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckOther.Location = new System.Drawing.Point(549, 681);
            this.ckOther.Name = "ckOther";
            this.ckOther.Size = new System.Drawing.Size(99, 33);
            this.ckOther.TabIndex = 85;
            this.ckOther.Tag = "Other";
            this.ckOther.Text = "Other";
            this.ckOther.UseVisualStyleBackColor = true;
            // 
            // ckUSWest
            // 
            this.ckUSWest.AutoSize = true;
            this.ckUSWest.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckUSWest.Location = new System.Drawing.Point(548, 651);
            this.ckUSWest.Name = "ckUSWest";
            this.ckUSWest.Size = new System.Drawing.Size(132, 33);
            this.ckUSWest.TabIndex = 84;
            this.ckUSWest.Tag = "US West";
            this.ckUSWest.Text = "US West";
            this.ckUSWest.UseVisualStyleBackColor = true;
            // 
            // ckUSEast
            // 
            this.ckUSEast.AutoSize = true;
            this.ckUSEast.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckUSEast.Location = new System.Drawing.Point(548, 621);
            this.ckUSEast.Name = "ckUSEast";
            this.ckUSEast.Size = new System.Drawing.Size(125, 33);
            this.ckUSEast.TabIndex = 83;
            this.ckUSEast.Tag = "US East";
            this.ckUSEast.Text = "US East";
            this.ckUSEast.UseVisualStyleBackColor = true;
            // 
            // ckRussia
            // 
            this.ckRussia.AutoSize = true;
            this.ckRussia.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckRussia.Location = new System.Drawing.Point(548, 591);
            this.ckRussia.Name = "ckRussia";
            this.ckRussia.Size = new System.Drawing.Size(112, 33);
            this.ckRussia.TabIndex = 82;
            this.ckRussia.Tag = "Russia";
            this.ckRussia.Text = "Russia";
            this.ckRussia.UseVisualStyleBackColor = true;
            // 
            // ckMiddleEast
            // 
            this.ckMiddleEast.AutoSize = true;
            this.ckMiddleEast.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckMiddleEast.Location = new System.Drawing.Point(548, 561);
            this.ckMiddleEast.Name = "ckMiddleEast";
            this.ckMiddleEast.Size = new System.Drawing.Size(166, 33);
            this.ckMiddleEast.TabIndex = 81;
            this.ckMiddleEast.Tag = "Middle East";
            this.ckMiddleEast.Text = "Middle East";
            this.ckMiddleEast.UseVisualStyleBackColor = true;
            // 
            // ckLatinAmerica
            // 
            this.ckLatinAmerica.AutoSize = true;
            this.ckLatinAmerica.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckLatinAmerica.Location = new System.Drawing.Point(548, 531);
            this.ckLatinAmerica.Name = "ckLatinAmerica";
            this.ckLatinAmerica.Size = new System.Drawing.Size(184, 33);
            this.ckLatinAmerica.TabIndex = 80;
            this.ckLatinAmerica.Tag = "Latin America";
            this.ckLatinAmerica.Text = "Latin America";
            this.ckLatinAmerica.UseVisualStyleBackColor = true;
            // 
            // ckIndia
            // 
            this.ckIndia.AutoSize = true;
            this.ckIndia.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckIndia.Location = new System.Drawing.Point(548, 501);
            this.ckIndia.Name = "ckIndia";
            this.ckIndia.Size = new System.Drawing.Size(91, 33);
            this.ckIndia.TabIndex = 79;
            this.ckIndia.Tag = "India";
            this.ckIndia.Text = "India";
            this.ckIndia.UseVisualStyleBackColor = true;
            // 
            // ckEurope
            // 
            this.ckEurope.AutoSize = true;
            this.ckEurope.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckEurope.Location = new System.Drawing.Point(548, 471);
            this.ckEurope.Name = "ckEurope";
            this.ckEurope.Size = new System.Drawing.Size(118, 33);
            this.ckEurope.TabIndex = 78;
            this.ckEurope.Tag = "Europe";
            this.ckEurope.Text = "Europe";
            this.ckEurope.UseVisualStyleBackColor = true;
            // 
            // ckCaribbean
            // 
            this.ckCaribbean.AutoSize = true;
            this.ckCaribbean.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckCaribbean.Location = new System.Drawing.Point(548, 441);
            this.ckCaribbean.Name = "ckCaribbean";
            this.ckCaribbean.Size = new System.Drawing.Size(151, 33);
            this.ckCaribbean.TabIndex = 77;
            this.ckCaribbean.Tag = "Caribbean";
            this.ckCaribbean.Text = "Caribbean";
            this.ckCaribbean.UseVisualStyleBackColor = true;
            // 
            // ckCanada
            // 
            this.ckCanada.AutoSize = true;
            this.ckCanada.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckCanada.Location = new System.Drawing.Point(548, 411);
            this.ckCanada.Name = "ckCanada";
            this.ckCanada.Size = new System.Drawing.Size(122, 33);
            this.ckCanada.TabIndex = 76;
            this.ckCanada.Tag = "Canada";
            this.ckCanada.Text = "Canada";
            this.ckCanada.UseVisualStyleBackColor = true;
            // 
            // ckAustralia
            // 
            this.ckAustralia.AutoSize = true;
            this.ckAustralia.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckAustralia.Location = new System.Drawing.Point(548, 381);
            this.ckAustralia.Name = "ckAustralia";
            this.ckAustralia.Size = new System.Drawing.Size(131, 33);
            this.ckAustralia.TabIndex = 75;
            this.ckAustralia.Tag = "Australia";
            this.ckAustralia.Text = "Australia";
            this.ckAustralia.UseVisualStyleBackColor = true;
            // 
            // ckAsia
            // 
            this.ckAsia.AutoSize = true;
            this.ckAsia.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckAsia.Location = new System.Drawing.Point(548, 351);
            this.ckAsia.Name = "ckAsia";
            this.ckAsia.Size = new System.Drawing.Size(85, 33);
            this.ckAsia.TabIndex = 74;
            this.ckAsia.Tag = "Asia";
            this.ckAsia.Text = "Asia";
            this.ckAsia.UseVisualStyleBackColor = true;
            // 
            // ckAfrica
            // 
            this.ckAfrica.AutoSize = true;
            this.ckAfrica.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckAfrica.Location = new System.Drawing.Point(548, 321);
            this.ckAfrica.Name = "ckAfrica";
            this.ckAfrica.Size = new System.Drawing.Size(99, 33);
            this.ckAfrica.TabIndex = 73;
            this.ckAfrica.Tag = "Africa";
            this.ckAfrica.Text = "Africa";
            this.ckAfrica.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(545, 298);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 29);
            this.label6.TabIndex = 72;
            this.label6.Text = "Regions";
            // 
            // frmDateRangeSearch
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 33F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(792, 898);
            this.Controls.Add(this.btnClearRegions);
            this.Controls.Add(this.ckOther);
            this.Controls.Add(this.ckUSWest);
            this.Controls.Add(this.ckUSEast);
            this.Controls.Add(this.ckRussia);
            this.Controls.Add(this.ckMiddleEast);
            this.Controls.Add(this.ckLatinAmerica);
            this.Controls.Add(this.ckIndia);
            this.Controls.Add(this.ckEurope);
            this.Controls.Add(this.ckCaribbean);
            this.Controls.Add(this.ckCanada);
            this.Controls.Add(this.ckAustralia);
            this.Controls.Add(this.ckAsia);
            this.Controls.Add(this.ckAfrica);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnDeselect);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.lbMSO);
            this.Controls.Add(this.btnClearTiers);
            this.Controls.Add(this.ckUnclassified);
            this.Controls.Add(this.ckTier2);
            this.Controls.Add(this.ckTier1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblRequestor);
            this.Controls.Add(this.cboRequestor);
            this.Controls.Add(this.lblDesigner);
            this.Controls.Add(this.cboDesigner);
            this.Controls.Add(this.btnForecast);
            this.Controls.Add(this.gbxSearchTerm);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpEndDate);
            this.Controls.Add(this.dtpStart);
            this.Controls.Add(this.dtpStartDate);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmDateRangeSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Date Range Search";
            this.Load += new System.EventHandler(this.frmDateRangeSearch_Load);
            this.gbxSearchTerm.ResumeLayout(false);
            this.gbxSearchTerm.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label dtpStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox gbxSearchTerm;
        private System.Windows.Forms.RadioButton rdoDateCompleted;
        private System.Windows.Forms.RadioButton rdoDateDue;
        private System.Windows.Forms.RadioButton rdoDate;
        private System.Windows.Forms.Button btnForecast;
        private System.Windows.Forms.Label lblDesigner;
        private System.Windows.Forms.ComboBox cboDesigner;
        private System.Windows.Forms.Label lblRequestor;
        private System.Windows.Forms.ComboBox cboRequestor;
        private System.Windows.Forms.Button btnClearTiers;
        private System.Windows.Forms.CheckBox ckUnclassified;
        private System.Windows.Forms.CheckBox ckTier2;
        private System.Windows.Forms.CheckBox ckTier1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnDeselect;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.ListBox lbMSO;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnClearRegions;
        private System.Windows.Forms.CheckBox ckOther;
        private System.Windows.Forms.CheckBox ckUSWest;
        private System.Windows.Forms.CheckBox ckUSEast;
        private System.Windows.Forms.CheckBox ckRussia;
        private System.Windows.Forms.CheckBox ckMiddleEast;
        private System.Windows.Forms.CheckBox ckLatinAmerica;
        private System.Windows.Forms.CheckBox ckIndia;
        private System.Windows.Forms.CheckBox ckEurope;
        private System.Windows.Forms.CheckBox ckCaribbean;
        private System.Windows.Forms.CheckBox ckCanada;
        private System.Windows.Forms.CheckBox ckAustralia;
        private System.Windows.Forms.CheckBox ckAsia;
        private System.Windows.Forms.CheckBox ckAfrica;
        private System.Windows.Forms.Label label6;
    }
}