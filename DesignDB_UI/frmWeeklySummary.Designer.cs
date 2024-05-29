namespace DesignDB_UI
{
    partial class frmWeeklySummary
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
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtYTDtotal = new System.Windows.Forms.TextBox();
            this.txtBacklog = new System.Windows.Forms.TextBox();
            this.txtCompletedForPeriod = new System.Windows.Forms.TextBox();
            this.txtRequestsForPeriod = new System.Windows.Forms.TextBox();
            this.txtYTDvalue = new System.Windows.Forms.TextBox();
            this.btnCollectInfo = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnClipboard = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dtpStart
            // 
            this.dtpStart.Location = new System.Drawing.Point(290, 59);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(200, 24);
            this.dtpStart.TabIndex = 0;
            // 
            // dtpEnd
            // 
            this.dtpEnd.Location = new System.Drawing.Point(290, 99);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(200, 24);
            this.dtpEnd.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(129, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "Summary Start Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(134, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "Summary End Date";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(736, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(196, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "YTD Total Designs Assigned";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(760, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(172, 18);
            this.label4.TabIndex = 5;
            this.label4.Text = "YTD Total Designs Value";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(621, 129);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(311, 18);
            this.label5.TabIndex = 6;
            this.label5.Text = "Total Designs Requested for Reporting Period";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(620, 164);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(312, 18);
            this.label6.TabIndex = 7;
            this.label6.Text = "Total Designs Completed for Reporting Period";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(760, 199);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(172, 18);
            this.label7.TabIndex = 8;
            this.label7.Text = "Total Designs in Backlog";
            // 
            // txtYTDtotal
            // 
            this.txtYTDtotal.Location = new System.Drawing.Point(948, 59);
            this.txtYTDtotal.Name = "txtYTDtotal";
            this.txtYTDtotal.Size = new System.Drawing.Size(100, 24);
            this.txtYTDtotal.TabIndex = 9;
            // 
            // txtBacklog
            // 
            this.txtBacklog.Location = new System.Drawing.Point(948, 199);
            this.txtBacklog.Name = "txtBacklog";
            this.txtBacklog.Size = new System.Drawing.Size(100, 24);
            this.txtBacklog.TabIndex = 10;
            // 
            // txtCompletedForPeriod
            // 
            this.txtCompletedForPeriod.Location = new System.Drawing.Point(948, 164);
            this.txtCompletedForPeriod.Name = "txtCompletedForPeriod";
            this.txtCompletedForPeriod.Size = new System.Drawing.Size(100, 24);
            this.txtCompletedForPeriod.TabIndex = 11;
            // 
            // txtRequestsForPeriod
            // 
            this.txtRequestsForPeriod.Location = new System.Drawing.Point(948, 129);
            this.txtRequestsForPeriod.Name = "txtRequestsForPeriod";
            this.txtRequestsForPeriod.Size = new System.Drawing.Size(100, 24);
            this.txtRequestsForPeriod.TabIndex = 12;
            // 
            // txtYTDvalue
            // 
            this.txtYTDvalue.Location = new System.Drawing.Point(948, 94);
            this.txtYTDvalue.Name = "txtYTDvalue";
            this.txtYTDvalue.Size = new System.Drawing.Size(100, 24);
            this.txtYTDvalue.TabIndex = 13;
            // 
            // btnCollectInfo
            // 
            this.btnCollectInfo.Location = new System.Drawing.Point(304, 233);
            this.btnCollectInfo.Name = "btnCollectInfo";
            this.btnCollectInfo.Size = new System.Drawing.Size(113, 38);
            this.btnCollectInfo.TabIndex = 14;
            this.btnCollectInfo.Text = "Collect Info";
            this.btnCollectInfo.UseVisualStyleBackColor = true;
            this.btnCollectInfo.Click += new System.EventHandler(this.btnCollectInfo_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(651, 233);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(113, 38);
            this.btnClose.TabIndex = 15;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnClipboard
            // 
            this.btnClipboard.Location = new System.Drawing.Point(460, 233);
            this.btnClipboard.Name = "btnClipboard";
            this.btnClipboard.Size = new System.Drawing.Size(145, 38);
            this.btnClipboard.TabIndex = 16;
            this.btnClipboard.Text = "Copy to Clipboard";
            this.btnClipboard.UseVisualStyleBackColor = true;
            this.btnClipboard.Click += new System.EventHandler(this.btnClipboard_Click);
            // 
            // frmWeeklySummary
            // 
            this.AcceptButton = this.btnCollectInfo;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1200, 306);
            this.Controls.Add(this.btnClipboard);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCollectInfo);
            this.Controls.Add(this.txtYTDvalue);
            this.Controls.Add(this.txtRequestsForPeriod);
            this.Controls.Add(this.txtCompletedForPeriod);
            this.Controls.Add(this.txtBacklog);
            this.Controls.Add(this.txtYTDtotal);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpEnd);
            this.Controls.Add(this.dtpStart);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmWeeklySummary";
            this.Text = "Weekly Summary";
            this.Load += new System.EventHandler(this.frmWeeklySummary_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtYTDtotal;
        private System.Windows.Forms.TextBox txtBacklog;
        private System.Windows.Forms.TextBox txtCompletedForPeriod;
        private System.Windows.Forms.TextBox txtRequestsForPeriod;
        private System.Windows.Forms.TextBox txtYTDvalue;
        private System.Windows.Forms.Button btnCollectInfo;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnClipboard;
    }
}