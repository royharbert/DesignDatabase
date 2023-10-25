
namespace DesignDB_UI
{
    partial class frmAttType
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
            this.gbxAttType = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.rdoOther = new System.Windows.Forms.RadioButton();
            this.rdoPerf = new System.Windows.Forms.RadioButton();
            this.rdoDrawing = new System.Windows.Forms.RadioButton();
            this.rdoBOM = new System.Windows.Forms.RadioButton();
            this.rdoDesignReq = new System.Windows.Forms.RadioButton();
            this.gbxAttType.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxAttType
            // 
            this.gbxAttType.Controls.Add(this.btnCancel);
            this.gbxAttType.Controls.Add(this.btnOK);
            this.gbxAttType.Controls.Add(this.rdoOther);
            this.gbxAttType.Controls.Add(this.rdoPerf);
            this.gbxAttType.Controls.Add(this.rdoDrawing);
            this.gbxAttType.Controls.Add(this.rdoBOM);
            this.gbxAttType.Controls.Add(this.rdoDesignReq);
            this.gbxAttType.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.gbxAttType.Location = new System.Drawing.Point(12, 12);
            this.gbxAttType.Name = "gbxAttType";
            this.gbxAttType.Size = new System.Drawing.Size(230, 365);
            this.gbxAttType.TabIndex = 1;
            this.gbxAttType.TabStop = false;
            this.gbxAttType.Text = "Attachment Type";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnCancel.Location = new System.Drawing.Point(6, 298);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(218, 45);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnOK.Location = new System.Drawing.Point(6, 247);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(218, 45);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // rdoOther
            // 
            this.rdoOther.AutoSize = true;
            this.rdoOther.Location = new System.Drawing.Point(35, 172);
            this.rdoOther.Name = "rdoOther";
            this.rdoOther.Size = new System.Drawing.Size(75, 28);
            this.rdoOther.TabIndex = 5;
            this.rdoOther.TabStop = true;
            this.rdoOther.Tag = "Other";
            this.rdoOther.Text = "Other";
            this.rdoOther.UseVisualStyleBackColor = true;
            this.rdoOther.CheckedChanged += new System.EventHandler(this.rdoOther_CheckedChanged);
            // 
            // rdoPerf
            // 
            this.rdoPerf.AutoSize = true;
            this.rdoPerf.Location = new System.Drawing.Point(34, 138);
            this.rdoPerf.Name = "rdoPerf";
            this.rdoPerf.Size = new System.Drawing.Size(139, 28);
            this.rdoPerf.TabIndex = 4;
            this.rdoPerf.TabStop = true;
            this.rdoPerf.Tag = "OMT-Review";
            this.rdoPerf.Text = "OMT-Review";
            this.rdoPerf.UseVisualStyleBackColor = true;
            this.rdoPerf.CheckedChanged += new System.EventHandler(this.rdoPerf_CheckedChanged);
            // 
            // rdoDrawing
            // 
            this.rdoDrawing.AutoSize = true;
            this.rdoDrawing.Location = new System.Drawing.Point(34, 104);
            this.rdoDrawing.Name = "rdoDrawing";
            this.rdoDrawing.Size = new System.Drawing.Size(161, 28);
            this.rdoDrawing.TabIndex = 3;
            this.rdoDrawing.TabStop = true;
            this.rdoDrawing.Tag = "Design Drawing";
            this.rdoDrawing.Text = "Design Drawing";
            this.rdoDrawing.UseVisualStyleBackColor = true;
            this.rdoDrawing.CheckedChanged += new System.EventHandler(this.rdoDrawing_CheckedChanged);
            // 
            // rdoBOM
            // 
            this.rdoBOM.AutoSize = true;
            this.rdoBOM.Location = new System.Drawing.Point(34, 70);
            this.rdoBOM.Name = "rdoBOM";
            this.rdoBOM.Size = new System.Drawing.Size(71, 28);
            this.rdoBOM.TabIndex = 2;
            this.rdoBOM.TabStop = true;
            this.rdoBOM.Tag = "BOM";
            this.rdoBOM.Text = "BOM";
            this.rdoBOM.UseVisualStyleBackColor = true;
            this.rdoBOM.CheckedChanged += new System.EventHandler(this.rdoBOM_CheckedChanged);
            // 
            // rdoDesignReq
            // 
            this.rdoDesignReq.AutoSize = true;
            this.rdoDesignReq.Location = new System.Drawing.Point(34, 36);
            this.rdoDesignReq.Name = "rdoDesignReq";
            this.rdoDesignReq.Size = new System.Drawing.Size(162, 28);
            this.rdoDesignReq.TabIndex = 1;
            this.rdoDesignReq.TabStop = true;
            this.rdoDesignReq.Tag = "Design Request";
            this.rdoDesignReq.Text = "Design Request";
            this.rdoDesignReq.UseVisualStyleBackColor = true;
            this.rdoDesignReq.CheckedChanged += new System.EventHandler(this.rdoDesignReq_CheckedChanged);
            // 
            // frmAttType
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(257, 394);
            this.Controls.Add(this.gbxAttType);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmAttType";
            this.Text = "Attachments";
            this.gbxAttType.ResumeLayout(false);
            this.gbxAttType.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxAttType;
        private System.Windows.Forms.RadioButton rdoOther;
        private System.Windows.Forms.RadioButton rdoPerf;
        private System.Windows.Forms.RadioButton rdoDrawing;
        private System.Windows.Forms.RadioButton rdoBOM;
        private System.Windows.Forms.RadioButton rdoDesignReq;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}