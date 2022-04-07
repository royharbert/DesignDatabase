
namespace DesignDB_UI
{
    partial class frmSalesMaint
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
            this.txtSalesperson = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lbxSalesperson = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ckbActive = new System.Windows.Forms.CheckBox();
            this.ckbShowOnlyActive = new System.Windows.Forms.CheckBox();
            this.txtID = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtSalesperson
            // 
            this.txtSalesperson.Location = new System.Drawing.Point(409, 219);
            this.txtSalesperson.Name = "txtSalesperson";
            this.txtSalesperson.Size = new System.Drawing.Size(247, 29);
            this.txtSalesperson.TabIndex = 46;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(176, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 24);
            this.label1.TabIndex = 45;
            this.label1.Text = "Salespersons";
            // 
            // btnClose
            // 
            this.btnClose.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnClose.Location = new System.Drawing.Point(450, 611);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(179, 42);
            this.btnClose.TabIndex = 44;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnDelete.Location = new System.Drawing.Point(432, 495);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(210, 42);
            this.btnDelete.TabIndex = 43;
            this.btnDelete.Text = "Delete Salesperson";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Visible = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnUpdate.Location = new System.Drawing.Point(432, 447);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(210, 42);
            this.btnUpdate.TabIndex = 42;
            this.btnUpdate.Text = "Update   Salesperson";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAdd.Location = new System.Drawing.Point(432, 399);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(210, 42);
            this.btnAdd.TabIndex = 41;
            this.btnAdd.Text = "Add Salesperson";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lbxSalesperson
            // 
            this.lbxSalesperson.FormattingEnabled = true;
            this.lbxSalesperson.ItemHeight = 24;
            this.lbxSalesperson.Location = new System.Drawing.Point(66, 74);
            this.lbxSalesperson.Name = "lbxSalesperson";
            this.lbxSalesperson.Size = new System.Drawing.Size(320, 580);
            this.lbxSalesperson.TabIndex = 40;
            this.lbxSalesperson.SelectedIndexChanged += new System.EventHandler(this.lbxSalesperson_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(483, 189);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 24);
            this.label2.TabIndex = 50;
            this.label2.Text = "Salesperson";
            // 
            // ckbActive
            // 
            this.ckbActive.AutoSize = true;
            this.ckbActive.Location = new System.Drawing.Point(445, 282);
            this.ckbActive.Name = "ckbActive";
            this.ckbActive.Size = new System.Drawing.Size(190, 28);
            this.ckbActive.TabIndex = 54;
            this.ckbActive.Text = "Active Salesperson";
            this.ckbActive.UseVisualStyleBackColor = true;
            // 
            // ckbShowOnlyActive
            // 
            this.ckbShowOnlyActive.AutoSize = true;
            this.ckbShowOnlyActive.Checked = true;
            this.ckbShowOnlyActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbShowOnlyActive.Location = new System.Drawing.Point(94, 680);
            this.ckbShowOnlyActive.Name = "ckbShowOnlyActive";
            this.ckbShowOnlyActive.Size = new System.Drawing.Size(267, 28);
            this.ckbShowOnlyActive.TabIndex = 57;
            this.ckbShowOnlyActive.Text = "Show Only Active Designers";
            this.ckbShowOnlyActive.UseVisualStyleBackColor = true;
            this.ckbShowOnlyActive.CheckedChanged += new System.EventHandler(this.ckbShowOnlyActive_CheckedChanged);
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(409, 133);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(247, 29);
            this.txtID.TabIndex = 58;
            this.txtID.Visible = false;
            // 
            // frmSalesMaint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(722, 831);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.ckbShowOnlyActive);
            this.Controls.Add(this.ckbActive);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSalesperson);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lbxSalesperson);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmSalesMaint";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Salesperson Maintenance";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSalesperson;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListBox lbxSalesperson;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox ckbActive;
        private System.Windows.Forms.CheckBox ckbShowOnlyActive;
        private System.Windows.Forms.TextBox txtID;
    }
}