
namespace DesignDB_UI
{
    partial class frmLogView
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
            this.txtTimeStamp = new System.Windows.Forms.TextBox();
            this.txtPID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnDone = new System.Windows.Forms.Button();
            this.txtActivity = new System.Windows.Forms.ComboBox();
            this.txtUser = new System.Windows.Forms.ComboBox();
            this.dgvLog = new System.Windows.Forms.DataGridView();
            this.btnViewAll = new System.Windows.Forms.Button();
            this.btnGoTORecord = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).BeginInit();
            this.SuspendLayout();
            // 
            // txtTimeStamp
            // 
            this.txtTimeStamp.Location = new System.Drawing.Point(224, 163);
            this.txtTimeStamp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTimeStamp.Name = "txtTimeStamp";
            this.txtTimeStamp.Size = new System.Drawing.Size(306, 26);
            this.txtTimeStamp.TabIndex = 1;
            this.txtTimeStamp.Tag = "TimeStamp";
            // 
            // txtPID
            // 
            this.txtPID.Location = new System.Drawing.Point(224, 91);
            this.txtPID.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPID.Name = "txtPID";
            this.txtPID.Size = new System.Drawing.Size(306, 26);
            this.txtPID.TabIndex = 0;
            this.txtPID.Tag = "RequestID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(134, 95);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Project ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(171, 246);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "User";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(159, 326);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Action";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(120, 168);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Time Stamp";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(262, 401);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(112, 35);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(132, 401);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(112, 35);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "Clear Form";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(251, 523);
            this.btnDone.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(136, 35);
            this.btnDone.TabIndex = 6;
            this.btnDone.Text = "Exit";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // txtActivity
            // 
            this.txtActivity.FormattingEnabled = true;
            this.txtActivity.Items.AddRange(new object[] {
            "New",
            "Edit",
            "Delete",
            "Restore",
            "Revision",
            "Clone",
            "Add_Attachment",
            "Delete_Attachment"});
            this.txtActivity.Location = new System.Drawing.Point(224, 326);
            this.txtActivity.Name = "txtActivity";
            this.txtActivity.Size = new System.Drawing.Size(306, 28);
            this.txtActivity.TabIndex = 9;
            this.txtActivity.Tag = "Action";
            // 
            // txtUser
            // 
            this.txtUser.FormattingEnabled = true;
            this.txtUser.Items.AddRange(new object[] {
            "New",
            "Edit",
            "Delete",
            "Restore",
            "Revision",
            "Clone",
            "Add Attachment",
            "Delete Attachment"});
            this.txtUser.Location = new System.Drawing.Point(224, 243);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(306, 28);
            this.txtUser.TabIndex = 10;
            this.txtUser.Tag = "User";
            // 
            // dgvLog
            // 
            this.dgvLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLog.Location = new System.Drawing.Point(571, 11);
            this.dgvLog.Name = "dgvLog";
            this.dgvLog.Size = new System.Drawing.Size(621, 631);
            this.dgvLog.TabIndex = 11;
            // 
            // btnViewAll
            // 
            this.btnViewAll.Location = new System.Drawing.Point(391, 401);
            this.btnViewAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnViewAll.Name = "btnViewAll";
            this.btnViewAll.Size = new System.Drawing.Size(112, 35);
            this.btnViewAll.TabIndex = 12;
            this.btnViewAll.Text = "View All";
            this.btnViewAll.UseVisualStyleBackColor = true;
            this.btnViewAll.Click += new System.EventHandler(this.btnViewAll_Click);
            // 
            // btnGoTORecord
            // 
            this.btnGoTORecord.Location = new System.Drawing.Point(251, 461);
            this.btnGoTORecord.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnGoTORecord.Name = "btnGoTORecord";
            this.btnGoTORecord.Size = new System.Drawing.Size(136, 35);
            this.btnGoTORecord.TabIndex = 13;
            this.btnGoTORecord.Text = "Go to Record";
            this.btnGoTORecord.UseVisualStyleBackColor = true;
            this.btnGoTORecord.Click += new System.EventHandler(this.btnGoTORecord_Click);
            // 
            // frmLogView
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 692);
            this.ControlBox = false;
            this.Controls.Add(this.btnGoTORecord);
            this.Controls.Add(this.btnViewAll);
            this.Controls.Add(this.dgvLog);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.txtActivity);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPID);
            this.Controls.Add(this.txtTimeStamp);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmLogView";
            this.Text = "Log Viewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLogView_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTimeStamp;
        private System.Windows.Forms.TextBox txtPID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.ComboBox txtActivity;
        private System.Windows.Forms.ComboBox txtUser;
        private System.Windows.Forms.DataGridView dgvLog;
        private System.Windows.Forms.Button btnViewAll;
        private System.Windows.Forms.Button btnGoTORecord;
    }
}