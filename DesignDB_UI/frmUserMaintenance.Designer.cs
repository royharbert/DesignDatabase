namespace DesignDB_UI
{
    partial class frmUserMaintenance
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
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtPriviledge = new System.Windows.Forms.TextBox();
            this.cboUser = new System.Windows.Forms.ComboBox();
            this.cboActive = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPrivPriviledge = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.btnUpdateUser = new System.Windows.Forms.Button();
            this.btnDeleteUser = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtID = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(248, 148);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(277, 31);
            this.txtPassword.TabIndex = 2;
            // 
            // txtPriviledge
            // 
            this.txtPriviledge.Location = new System.Drawing.Point(248, 206);
            this.txtPriviledge.Name = "txtPriviledge";
            this.txtPriviledge.Size = new System.Drawing.Size(277, 31);
            this.txtPriviledge.TabIndex = 3;
            // 
            // cboUser
            // 
            this.cboUser.FormattingEnabled = true;
            this.cboUser.Location = new System.Drawing.Point(248, 89);
            this.cboUser.Name = "cboUser";
            this.cboUser.Size = new System.Drawing.Size(276, 32);
            this.cboUser.TabIndex = 1;
            this.cboUser.SelectedIndexChanged += new System.EventHandler(this.cboUser_SelectedIndexChanged);
            // 
            // cboActive
            // 
            this.cboActive.FormattingEnabled = true;
            this.cboActive.Items.AddRange(new object[] {
            "True",
            "False"});
            this.cboActive.Location = new System.Drawing.Point(248, 265);
            this.cboActive.Name = "cboActive";
            this.cboActive.Size = new System.Drawing.Size(276, 32);
            this.cboActive.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 268);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 24);
            this.label1.TabIndex = 4;
            this.label1.Text = "Active Designer:";
            // 
            // lblPrivPriviledge
            // 
            this.lblPrivPriviledge.AutoSize = true;
            this.lblPrivPriviledge.Location = new System.Drawing.Point(129, 209);
            this.lblPrivPriviledge.Name = "lblPrivPriviledge";
            this.lblPrivPriviledge.Size = new System.Drawing.Size(113, 24);
            this.lblPrivPriviledge.TabIndex = 5;
            this.lblPrivPriviledge.Text = "Priviledge:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(133, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 24);
            this.label3.TabIndex = 6;
            this.label3.Text = "Password:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(119, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 24);
            this.label4.TabIndex = 7;
            this.label4.Text = "User Name:";
            // 
            // btnAddUser
            // 
            this.btnAddUser.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.btnAddUser.Location = new System.Drawing.Point(55, 345);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(157, 40);
            this.btnAddUser.TabIndex = 8;
            this.btnAddUser.Text = "Add User";
            this.btnAddUser.UseVisualStyleBackColor = true;
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
            // 
            // btnUpdateUser
            // 
            this.btnUpdateUser.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.btnUpdateUser.Location = new System.Drawing.Point(287, 345);
            this.btnUpdateUser.Name = "btnUpdateUser";
            this.btnUpdateUser.Size = new System.Drawing.Size(157, 40);
            this.btnUpdateUser.TabIndex = 9;
            this.btnUpdateUser.Text = "Update User";
            this.btnUpdateUser.UseVisualStyleBackColor = true;
            this.btnUpdateUser.Click += new System.EventHandler(this.btnUpdateUser_Click);
            // 
            // btnDeleteUser
            // 
            this.btnDeleteUser.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.btnDeleteUser.Location = new System.Drawing.Point(514, 345);
            this.btnDeleteUser.Name = "btnDeleteUser";
            this.btnDeleteUser.Size = new System.Drawing.Size(157, 40);
            this.btnDeleteUser.TabIndex = 10;
            this.btnDeleteUser.Text = "Delete User";
            this.btnDeleteUser.UseVisualStyleBackColor = true;
            this.btnDeleteUser.Click += new System.EventHandler(this.btnDeleteUser_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(157, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 24);
            this.label2.TabIndex = 12;
            this.label2.Text = "UserID:";
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(248, 29);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(277, 31);
            this.txtID.TabIndex = 11;
            // 
            // btnClear
            // 
            this.btnClear.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnClear.Location = new System.Drawing.Point(579, 139);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(137, 41);
            this.btnClear.TabIndex = 13;
            this.btnClear.Text = "Clear Boxes";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // frmUserMaintenance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(750, 425);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.btnDeleteUser);
            this.Controls.Add(this.btnUpdateUser);
            this.Controls.Add(this.btnAddUser);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblPrivPriviledge);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboActive);
            this.Controls.Add(this.cboUser);
            this.Controls.Add(this.txtPriviledge);
            this.Controls.Add(this.txtPassword);
            this.Font = new System.Drawing.Font("MS Reference Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmUserMaintenance";
            this.Text = "User Maintenance";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtPriviledge;
        private System.Windows.Forms.ComboBox cboUser;
        private System.Windows.Forms.ComboBox cboActive;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPrivPriviledge;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.Button btnUpdateUser;
        private System.Windows.Forms.Button btnDeleteUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.Button btnClear;
    }
}

