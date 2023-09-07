
namespace DesignDB_UI
{
    partial class frmDateMSO_Picker
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
            this.lbMSO = new System.Windows.Forms.ListBox();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.dtpStop = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnDeselect = new System.Windows.Forms.Button();
            this.rdo_Normal = new System.Windows.Forms.RadioButton();
            this.rdo_Custom = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
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
            this.ckTier2 = new System.Windows.Forms.CheckBox();
            this.ckTier1 = new System.Windows.Forms.CheckBox();
            this.ckUnclassified = new System.Windows.Forms.CheckBox();
            this.btnClearRegions = new System.Windows.Forms.Button();
            this.btnClearTiers = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbMSO
            // 
            this.lbMSO.FormattingEnabled = true;
            this.lbMSO.ItemHeight = 20;
            this.lbMSO.Location = new System.Drawing.Point(68, 149);
            this.lbMSO.Name = "lbMSO";
            this.lbMSO.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbMSO.Size = new System.Drawing.Size(279, 604);
            this.lbMSO.TabIndex = 0;
            // 
            // dtpStart
            // 
            this.dtpStart.Location = new System.Drawing.Point(218, 25);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(279, 26);
            this.dtpStart.TabIndex = 1;
            this.dtpStart.ValueChanged += new System.EventHandler(this.dtpStart_ValueChanged);
            // 
            // dtpStop
            // 
            this.dtpStop.Location = new System.Drawing.Point(218, 70);
            this.dtpStop.Name = "dtpStop";
            this.dtpStop.Size = new System.Drawing.Size(279, 26);
            this.dtpStop.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(129, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Start Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(135, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "End Date";
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(500, 748);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(119, 48);
            this.btnGo.TabIndex = 5;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(656, 748);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(119, 48);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(68, 770);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(93, 26);
            this.btnSelectAll.TabIndex = 9;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnDeselect
            // 
            this.btnDeselect.Location = new System.Drawing.Point(254, 770);
            this.btnDeselect.Name = "btnDeselect";
            this.btnDeselect.Size = new System.Drawing.Size(93, 26);
            this.btnDeselect.TabIndex = 10;
            this.btnDeselect.Text = "Clear";
            this.btnDeselect.UseVisualStyleBackColor = true;
            this.btnDeselect.Click += new System.EventHandler(this.btnDeselect_Click);
            // 
            // rdo_Normal
            // 
            this.rdo_Normal.AutoSize = true;
            this.rdo_Normal.Checked = true;
            this.rdo_Normal.Location = new System.Drawing.Point(617, 31);
            this.rdo_Normal.Name = "rdo_Normal";
            this.rdo_Normal.Size = new System.Drawing.Size(132, 24);
            this.rdo_Normal.TabIndex = 11;
            this.rdo_Normal.TabStop = true;
            this.rdo_Normal.Text = "Normal Format";
            this.rdo_Normal.UseVisualStyleBackColor = true;
            this.rdo_Normal.CheckedChanged += new System.EventHandler(this.rdo_Normal_CheckedChanged);
            // 
            // rdo_Custom
            // 
            this.rdo_Custom.AutoSize = true;
            this.rdo_Custom.Location = new System.Drawing.Point(617, 61);
            this.rdo_Custom.Name = "rdo_Custom";
            this.rdo_Custom.Size = new System.Drawing.Size(137, 24);
            this.rdo_Custom.TabIndex = 12;
            this.rdo_Custom.Text = "Custom Format";
            this.rdo_Custom.UseVisualStyleBackColor = true;
            this.rdo_Custom.CheckedChanged += new System.EventHandler(this.rdo_Custom_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(424, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 24);
            this.label3.TabIndex = 28;
            this.label3.Text = "Regions";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(562, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 24);
            this.label4.TabIndex = 29;
            this.label4.Text = "Tiers";
            // 
            // ckOther
            // 
            this.ckOther.AutoSize = true;
            this.ckOther.Location = new System.Drawing.Point(429, 609);
            this.ckOther.Name = "ckOther";
            this.ckOther.Size = new System.Drawing.Size(68, 24);
            this.ckOther.TabIndex = 53;
            this.ckOther.Tag = "Other";
            this.ckOther.Text = "Other";
            this.ckOther.UseVisualStyleBackColor = true;
            // 
            // ckUSWest
            // 
            this.ckUSWest.AutoSize = true;
            this.ckUSWest.Location = new System.Drawing.Point(428, 579);
            this.ckUSWest.Name = "ckUSWest";
            this.ckUSWest.Size = new System.Drawing.Size(92, 24);
            this.ckUSWest.TabIndex = 52;
            this.ckUSWest.Tag = "US West";
            this.ckUSWest.Text = "US West";
            this.ckUSWest.UseVisualStyleBackColor = true;
            // 
            // ckUSEast
            // 
            this.ckUSEast.AutoSize = true;
            this.ckUSEast.Location = new System.Drawing.Point(428, 549);
            this.ckUSEast.Name = "ckUSEast";
            this.ckUSEast.Size = new System.Drawing.Size(88, 24);
            this.ckUSEast.TabIndex = 51;
            this.ckUSEast.Tag = "US East";
            this.ckUSEast.Text = "US East";
            this.ckUSEast.UseVisualStyleBackColor = true;
            // 
            // ckRussia
            // 
            this.ckRussia.AutoSize = true;
            this.ckRussia.Location = new System.Drawing.Point(428, 519);
            this.ckRussia.Name = "ckRussia";
            this.ckRussia.Size = new System.Drawing.Size(77, 24);
            this.ckRussia.TabIndex = 50;
            this.ckRussia.Tag = "Russia";
            this.ckRussia.Text = "Russia";
            this.ckRussia.UseVisualStyleBackColor = true;
            // 
            // ckMiddleEast
            // 
            this.ckMiddleEast.AutoSize = true;
            this.ckMiddleEast.Location = new System.Drawing.Point(428, 489);
            this.ckMiddleEast.Name = "ckMiddleEast";
            this.ckMiddleEast.Size = new System.Drawing.Size(111, 24);
            this.ckMiddleEast.TabIndex = 49;
            this.ckMiddleEast.Tag = "Middle East";
            this.ckMiddleEast.Text = "Middle East";
            this.ckMiddleEast.UseVisualStyleBackColor = true;
            // 
            // ckLatinAmerica
            // 
            this.ckLatinAmerica.AutoSize = true;
            this.ckLatinAmerica.Location = new System.Drawing.Point(428, 459);
            this.ckLatinAmerica.Name = "ckLatinAmerica";
            this.ckLatinAmerica.Size = new System.Drawing.Size(125, 24);
            this.ckLatinAmerica.TabIndex = 48;
            this.ckLatinAmerica.Tag = "Latin America";
            this.ckLatinAmerica.Text = "Latin America";
            this.ckLatinAmerica.UseVisualStyleBackColor = true;
            // 
            // ckIndia
            // 
            this.ckIndia.AutoSize = true;
            this.ckIndia.Location = new System.Drawing.Point(428, 429);
            this.ckIndia.Name = "ckIndia";
            this.ckIndia.Size = new System.Drawing.Size(63, 24);
            this.ckIndia.TabIndex = 47;
            this.ckIndia.Tag = "India";
            this.ckIndia.Text = "India";
            this.ckIndia.UseVisualStyleBackColor = true;
            // 
            // ckEurope
            // 
            this.ckEurope.AutoSize = true;
            this.ckEurope.Location = new System.Drawing.Point(428, 399);
            this.ckEurope.Name = "ckEurope";
            this.ckEurope.Size = new System.Drawing.Size(80, 24);
            this.ckEurope.TabIndex = 46;
            this.ckEurope.Tag = "Europe";
            this.ckEurope.Text = "Europe";
            this.ckEurope.UseVisualStyleBackColor = true;
            // 
            // ckCaribbean
            // 
            this.ckCaribbean.AutoSize = true;
            this.ckCaribbean.Location = new System.Drawing.Point(428, 369);
            this.ckCaribbean.Name = "ckCaribbean";
            this.ckCaribbean.Size = new System.Drawing.Size(101, 24);
            this.ckCaribbean.TabIndex = 45;
            this.ckCaribbean.Tag = "Caribbean";
            this.ckCaribbean.Text = "Caribbean";
            this.ckCaribbean.UseVisualStyleBackColor = true;
            // 
            // ckCanada
            // 
            this.ckCanada.AutoSize = true;
            this.ckCanada.Location = new System.Drawing.Point(428, 339);
            this.ckCanada.Name = "ckCanada";
            this.ckCanada.Size = new System.Drawing.Size(84, 24);
            this.ckCanada.TabIndex = 44;
            this.ckCanada.Tag = "Canada";
            this.ckCanada.Text = "Canada";
            this.ckCanada.UseVisualStyleBackColor = true;
            // 
            // ckAustralia
            // 
            this.ckAustralia.AutoSize = true;
            this.ckAustralia.Location = new System.Drawing.Point(428, 309);
            this.ckAustralia.Name = "ckAustralia";
            this.ckAustralia.Size = new System.Drawing.Size(90, 24);
            this.ckAustralia.TabIndex = 43;
            this.ckAustralia.Tag = "Australia";
            this.ckAustralia.Text = "Australia";
            this.ckAustralia.UseVisualStyleBackColor = true;
            // 
            // ckAsia
            // 
            this.ckAsia.AutoSize = true;
            this.ckAsia.Location = new System.Drawing.Point(428, 279);
            this.ckAsia.Name = "ckAsia";
            this.ckAsia.Size = new System.Drawing.Size(59, 24);
            this.ckAsia.TabIndex = 42;
            this.ckAsia.Tag = "Asia";
            this.ckAsia.Text = "Asia";
            this.ckAsia.UseVisualStyleBackColor = true;
            // 
            // ckAfrica
            // 
            this.ckAfrica.AutoSize = true;
            this.ckAfrica.Location = new System.Drawing.Point(428, 249);
            this.ckAfrica.Name = "ckAfrica";
            this.ckAfrica.Size = new System.Drawing.Size(69, 24);
            this.ckAfrica.TabIndex = 41;
            this.ckAfrica.Tag = "Africa";
            this.ckAfrica.Text = "Africa";
            this.ckAfrica.UseVisualStyleBackColor = true;
            // 
            // ckTier2
            // 
            this.ckTier2.AutoSize = true;
            this.ckTier2.Location = new System.Drawing.Point(566, 279);
            this.ckTier2.Name = "ckTier2";
            this.ckTier2.Size = new System.Drawing.Size(67, 24);
            this.ckTier2.TabIndex = 56;
            this.ckTier2.Tag = "2";
            this.ckTier2.Text = "Tier 2";
            this.ckTier2.UseVisualStyleBackColor = true;
            this.ckTier2.CheckedChanged += new System.EventHandler(this.ckTier1_CheckedChanged);
            // 
            // ckTier1
            // 
            this.ckTier1.AutoSize = true;
            this.ckTier1.Location = new System.Drawing.Point(566, 249);
            this.ckTier1.Name = "ckTier1";
            this.ckTier1.Size = new System.Drawing.Size(63, 24);
            this.ckTier1.TabIndex = 55;
            this.ckTier1.Tag = "1";
            this.ckTier1.Text = "Tier1";
            this.ckTier1.UseVisualStyleBackColor = true;
            this.ckTier1.CheckedChanged += new System.EventHandler(this.ckTier1_CheckedChanged);
            // 
            // ckUnclassified
            // 
            this.ckUnclassified.AutoSize = true;
            this.ckUnclassified.Location = new System.Drawing.Point(566, 311);
            this.ckUnclassified.Name = "ckUnclassified";
            this.ckUnclassified.Size = new System.Drawing.Size(114, 24);
            this.ckUnclassified.TabIndex = 59;
            this.ckUnclassified.Tag = "0";
            this.ckUnclassified.Text = "Unclassified";
            this.ckUnclassified.UseVisualStyleBackColor = true;
            // 
            // btnClearRegions
            // 
            this.btnClearRegions.Location = new System.Drawing.Point(423, 176);
            this.btnClearRegions.Name = "btnClearRegions";
            this.btnClearRegions.Size = new System.Drawing.Size(93, 52);
            this.btnClearRegions.TabIndex = 60;
            this.btnClearRegions.Text = "Clear Regions";
            this.btnClearRegions.UseVisualStyleBackColor = true;
            this.btnClearRegions.Click += new System.EventHandler(this.btnClearRegions_Click);
            // 
            // btnClearTiers
            // 
            this.btnClearTiers.Location = new System.Drawing.Point(550, 176);
            this.btnClearTiers.Name = "btnClearTiers";
            this.btnClearTiers.Size = new System.Drawing.Size(93, 52);
            this.btnClearTiers.TabIndex = 61;
            this.btnClearTiers.Text = "Clear Tiers";
            this.btnClearTiers.UseVisualStyleBackColor = true;
            this.btnClearTiers.Click += new System.EventHandler(this.btnClearTiers_Click);
            // 
            // frmDateMSO_Picker
            // 
            this.AcceptButton = this.btnGo;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(823, 867);
            this.Controls.Add(this.btnClearTiers);
            this.Controls.Add(this.btnClearRegions);
            this.Controls.Add(this.ckUnclassified);
            this.Controls.Add(this.ckTier2);
            this.Controls.Add(this.ckTier1);
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
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rdo_Custom);
            this.Controls.Add(this.rdo_Normal);
            this.Controls.Add(this.btnDeselect);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpStop);
            this.Controls.Add(this.dtpStart);
            this.Controls.Add(this.lbMSO);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmDateMSO_Picker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Date Range Selection";
            this.Activated += new System.EventHandler(this.frmDateMSO_Picker_Activated);
            this.Load += new System.EventHandler(this.frmDateMSO_Picker_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbMSO;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.DateTimePicker dtpStop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnDeselect;
        private System.Windows.Forms.RadioButton rdo_Normal;
        private System.Windows.Forms.RadioButton rdo_Custom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
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
        private System.Windows.Forms.CheckBox ckTier2;
        private System.Windows.Forms.CheckBox ckTier1;
        private System.Windows.Forms.CheckBox ckUnclassified;
        private System.Windows.Forms.Button btnClearRegions;
        private System.Windows.Forms.Button btnClearTiers;
    }
}