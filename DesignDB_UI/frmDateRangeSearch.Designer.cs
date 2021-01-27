
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
            this.cboMSO = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbxSearchTerm = new System.Windows.Forms.GroupBox();
            this.rdoDateCompleted = new System.Windows.Forms.RadioButton();
            this.rdoDateDue = new System.Windows.Forms.RadioButton();
            this.rdoDate = new System.Windows.Forms.RadioButton();
            this.btnForecast = new System.Windows.Forms.Button();
            this.gbxSearchTerm.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Location = new System.Drawing.Point(435, 61);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(309, 29);
            this.dtpStartDate.TabIndex = 0;
            // 
            // dtpStart
            // 
            this.dtpStart.AutoSize = true;
            this.dtpStart.Location = new System.Drawing.Point(275, 67);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(89, 24);
            this.dtpStart.TabIndex = 1;
            this.dtpStart.Text = "Start Date";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(275, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "End Date";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Location = new System.Drawing.Point(435, 106);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(309, 29);
            this.dtpEndDate.TabIndex = 2;
            // 
            // cboMSO
            // 
            this.cboMSO.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboMSO.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboMSO.FormattingEnabled = true;
            this.cboMSO.Location = new System.Drawing.Point(435, 151);
            this.cboMSO.Name = "cboMSO";
            this.cboMSO.Size = new System.Drawing.Size(309, 32);
            this.cboMSO.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(275, 155);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 24);
            this.label2.TabIndex = 5;
            this.label2.Text = "MSO  (optional)";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(435, 222);
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
            this.btnCancel.Location = new System.Drawing.Point(609, 222);
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
            this.gbxSearchTerm.Location = new System.Drawing.Point(31, 79);
            this.gbxSearchTerm.Name = "gbxSearchTerm";
            this.gbxSearchTerm.Size = new System.Drawing.Size(221, 164);
            this.gbxSearchTerm.TabIndex = 8;
            this.gbxSearchTerm.TabStop = false;
            this.gbxSearchTerm.Text = "Search Term";
            // 
            // rdoDateCompleted
            // 
            this.rdoDateCompleted.AutoSize = true;
            this.rdoDateCompleted.Location = new System.Drawing.Point(46, 113);
            this.rdoDateCompleted.Name = "rdoDateCompleted";
            this.rdoDateCompleted.Size = new System.Drawing.Size(163, 28);
            this.rdoDateCompleted.TabIndex = 2;
            this.rdoDateCompleted.Text = "Date Completed";
            this.rdoDateCompleted.UseVisualStyleBackColor = true;
            this.rdoDateCompleted.CheckedChanged += new System.EventHandler(this.rdoDateCompleted_CheckedChanged);
            // 
            // rdoDateDue
            // 
            this.rdoDateDue.AutoSize = true;
            this.rdoDateDue.Location = new System.Drawing.Point(46, 79);
            this.rdoDateDue.Name = "rdoDateDue";
            this.rdoDateDue.Size = new System.Drawing.Size(106, 28);
            this.rdoDateDue.TabIndex = 1;
            this.rdoDateDue.Text = "Date Due";
            this.rdoDateDue.UseVisualStyleBackColor = true;
            this.rdoDateDue.CheckedChanged += new System.EventHandler(this.rdoDateDue_CheckedChanged);
            // 
            // rdoDate
            // 
            this.rdoDate.AutoSize = true;
            this.rdoDate.Checked = true;
            this.rdoDate.Location = new System.Drawing.Point(46, 45);
            this.rdoDate.Name = "rdoDate";
            this.rdoDate.Size = new System.Drawing.Size(150, 28);
            this.rdoDate.TabIndex = 0;
            this.rdoDate.TabStop = true;
            this.rdoDate.Text = "Date Assigned";
            this.rdoDate.UseVisualStyleBackColor = true;
            this.rdoDate.CheckedChanged += new System.EventHandler(this.rdoDate_CheckedChanged);
            // 
            // btnForecast
            // 
            this.btnForecast.Location = new System.Drawing.Point(435, 281);
            this.btnForecast.Name = "btnForecast";
            this.btnForecast.Size = new System.Drawing.Size(309, 38);
            this.btnForecast.TabIndex = 9;
            this.btnForecast.Text = "Forecast";
            this.btnForecast.UseVisualStyleBackColor = true;
            this.btnForecast.Click += new System.EventHandler(this.btnForecast_Click);
            // 
            // frmDateRangeSearch
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(801, 377);
            this.Controls.Add(this.btnForecast);
            this.Controls.Add(this.gbxSearchTerm);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboMSO);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpEndDate);
            this.Controls.Add(this.dtpStart);
            this.Controls.Add(this.dtpStartDate);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmDateRangeSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Date Range Search";
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
        private System.Windows.Forms.ComboBox cboMSO;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox gbxSearchTerm;
        private System.Windows.Forms.RadioButton rdoDateCompleted;
        private System.Windows.Forms.RadioButton rdoDateDue;
        private System.Windows.Forms.RadioButton rdoDate;
        private System.Windows.Forms.Button btnForecast;
    }
}