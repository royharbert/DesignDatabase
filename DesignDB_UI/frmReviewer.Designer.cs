﻿
namespace DesignDB_UI
{
    partial class frmReviewer
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
            this.txtDesigner = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lbxDesigner = new System.Windows.Forms.ListBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtPriviledge = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ckbActive = new System.Windows.Forms.CheckBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.ckbShowOnlyActive = new System.Windows.Forms.CheckBox();
            this.txtID = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtDesigner
            // 
            this.txtDesigner.Location = new System.Drawing.Point(428, 114);
            this.txtDesigner.Name = "txtDesigner";
            this.txtDesigner.Size = new System.Drawing.Size(247, 40);
            this.txtDesigner.TabIndex = 46;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(176, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 33);
            this.label1.TabIndex = 45;
            this.label1.Text = "Designers";
            // 
            // btnClose
            // 
            this.btnClose.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnClose.Location = new System.Drawing.Point(468, 768);
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
            this.btnDelete.Location = new System.Drawing.Point(468, 652);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(179, 42);
            this.btnDelete.TabIndex = 43;
            this.btnDelete.Text = "Delete Designer";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnUpdate.Location = new System.Drawing.Point(468, 604);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(179, 42);
            this.btnUpdate.TabIndex = 42;
            this.btnUpdate.Text = "Update  Designer";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAdd.Location = new System.Drawing.Point(468, 556);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(179, 42);
            this.btnAdd.TabIndex = 41;
            this.btnAdd.Text = "Add Designer";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lbxDesigner
            // 
            this.lbxDesigner.FormattingEnabled = true;
            this.lbxDesigner.ItemHeight = 33;
            this.lbxDesigner.Location = new System.Drawing.Point(66, 74);
            this.lbxDesigner.Name = "lbxDesigner";
            this.lbxDesigner.Size = new System.Drawing.Size(320, 763);
            this.lbxDesigner.TabIndex = 40;
            this.lbxDesigner.SelectedIndexChanged += new System.EventHandler(this.lbxDesigner_SelectedIndexChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(428, 202);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(247, 40);
            this.txtPassword.TabIndex = 47;
            // 
            // txtPriviledge
            // 
            this.txtPriviledge.Location = new System.Drawing.Point(428, 291);
            this.txtPriviledge.Name = "txtPriviledge";
            this.txtPriviledge.Size = new System.Drawing.Size(247, 40);
            this.txtPriviledge.TabIndex = 48;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(488, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 33);
            this.label2.TabIndex = 50;
            this.label2.Text = "Designer";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(488, 255);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 33);
            this.label4.TabIndex = 52;
            this.label4.Text = "Privilege";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(488, 157);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(143, 33);
            this.label5.TabIndex = 53;
            this.label5.Text = "Password";
            // 
            // ckbActive
            // 
            this.ckbActive.AutoSize = true;
            this.ckbActive.Location = new System.Drawing.Point(438, 337);
            this.ckbActive.Name = "ckbActive";
            this.ckbActive.Size = new System.Drawing.Size(246, 37);
            this.ckbActive.TabIndex = 54;
            this.ckbActive.Text = "Active Designer";
            this.ckbActive.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnClear.Location = new System.Drawing.Point(485, 425);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(137, 49);
            this.btnClear.TabIndex = 55;
            this.btnClear.Text = "Clear Boxes";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // ckbShowOnlyActive
            // 
            this.ckbShowOnlyActive.AutoSize = true;
            this.ckbShowOnlyActive.Checked = true;
            this.ckbShowOnlyActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbShowOnlyActive.Location = new System.Drawing.Point(49, 867);
            this.ckbShowOnlyActive.Name = "ckbShowOnlyActive";
            this.ckbShowOnlyActive.Size = new System.Drawing.Size(623, 56);
            this.ckbShowOnlyActive.TabIndex = 57;
            this.ckbShowOnlyActive.Text = "Show Only Active Reviewers";
            this.ckbShowOnlyActive.UseVisualStyleBackColor = true;
            this.ckbShowOnlyActive.CheckedChanged += new System.EventHandler(this.ckbShowOnlyActive_CheckedChanged);
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(419, 35);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(241, 40);
            this.txtID.TabIndex = 58;
            // 
            // frmReviewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 33F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(776, 953);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.ckbShowOnlyActive);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.ckbActive);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPriviledge);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtDesigner);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lbxDesigner);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmReviewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reviewer Maintenance";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDesigner;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListBox lbxDesigner;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtPriviledge;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox ckbActive;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.CheckBox ckbShowOnlyActive;
        private System.Windows.Forms.TextBox txtID;
    }
}