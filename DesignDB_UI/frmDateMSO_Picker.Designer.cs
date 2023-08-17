
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
            this.SuspendLayout();
            // 
            // lbMSO
            // 
            this.lbMSO.FormattingEnabled = true;
            this.lbMSO.ItemHeight = 20;
            this.lbMSO.Location = new System.Drawing.Point(196, 138);
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
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(141, 781);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(93, 26);
            this.btnSelectAll.TabIndex = 9;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnDeselect
            // 
            this.btnDeselect.Location = new System.Drawing.Point(327, 781);
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
            this.rdo_Normal.Location = new System.Drawing.Point(32, 161);
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
            this.rdo_Custom.Location = new System.Drawing.Point(32, 191);
            this.rdo_Custom.Name = "rdo_Custom";
            this.rdo_Custom.Size = new System.Drawing.Size(137, 24);
            this.rdo_Custom.TabIndex = 12;
            this.rdo_Custom.TabStop = true;
            this.rdo_Custom.Text = "Custom Format";
            this.rdo_Custom.UseVisualStyleBackColor = true;
            this.rdo_Custom.CheckedChanged += new System.EventHandler(this.rdo_Custom_CheckedChanged);
            // 
            // frmDateMSO_Picker
            // 
            this.AcceptButton = this.btnGo;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(573, 867);
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
    }
}