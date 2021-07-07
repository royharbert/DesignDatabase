
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
            this.ckAll = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lbMSO
            // 
            this.lbMSO.FormattingEnabled = true;
            this.lbMSO.ItemHeight = 20;
            this.lbMSO.Location = new System.Drawing.Point(141, 148);
            this.lbMSO.Name = "lbMSO";
            this.lbMSO.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbMSO.Size = new System.Drawing.Size(279, 604);
            this.lbMSO.TabIndex = 0;
            // 
            // dtpStart
            // 
            this.dtpStart.Location = new System.Drawing.Point(131, 21);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(279, 26);
            this.dtpStart.TabIndex = 1;
            this.dtpStart.ValueChanged += new System.EventHandler(this.dtpStart_ValueChanged);
            // 
            // dtpStop
            // 
            this.dtpStop.Location = new System.Drawing.Point(131, 66);
            this.dtpStop.Name = "dtpStop";
            this.dtpStop.Size = new System.Drawing.Size(279, 26);
            this.dtpStop.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Start Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "End Date";
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(428, 19);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(119, 36);
            this.btnGo.TabIndex = 5;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(428, 61);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(119, 36);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ckAll
            // 
            this.ckAll.AutoSize = true;
            this.ckAll.Location = new System.Drawing.Point(252, 758);
            this.ckAll.Name = "ckAll";
            this.ckAll.Size = new System.Drawing.Size(94, 24);
            this.ckAll.TabIndex = 7;
            this.ckAll.Text = "Select All";
            this.ckAll.UseVisualStyleBackColor = true;
            this.ckAll.CheckedChanged += new System.EventHandler(this.ckAll_CheckedChanged);
            // 
            // frmDateMSO_Picker
            // 
            this.AcceptButton = this.btnGo;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(573, 812);
            this.Controls.Add(this.ckAll);
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
            this.Text = "frmDateMSO_Picker";
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
        private System.Windows.Forms.CheckBox ckAll;
    }
}