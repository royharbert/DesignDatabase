﻿
namespace DesignDB_UI
{
    partial class frmLoadReport
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
            this.dgvLoad = new System.Windows.Forms.DataGridView();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnClosr = new System.Windows.Forms.Button();
            this.txtRecords = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoad)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvLoad
            // 
            this.dgvLoad.AllowUserToAddRows = false;
            this.dgvLoad.AllowUserToDeleteRows = false;
            this.dgvLoad.AllowUserToOrderColumns = true;
            this.dgvLoad.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLoad.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLoad.Location = new System.Drawing.Point(0, 0);
            this.dgvLoad.Name = "dgvLoad";
            this.dgvLoad.ReadOnly = true;
            this.dgvLoad.Size = new System.Drawing.Size(696, 815);
            this.dgvLoad.TabIndex = 0;
            this.dgvLoad.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvLoad_RowHeaderMouseClick);
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(290, 744);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(171, 39);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "Export to Excel";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnClosr
            // 
            this.btnClosr.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClosr.Location = new System.Drawing.Point(484, 744);
            this.btnClosr.Name = "btnClosr";
            this.btnClosr.Size = new System.Drawing.Size(171, 39);
            this.btnClosr.TabIndex = 2;
            this.btnClosr.Text = "Close";
            this.btnClosr.UseVisualStyleBackColor = true;
            this.btnClosr.Click += new System.EventHandler(this.btnClosr_Click);
            // 
            // txtRecords
            // 
            this.txtRecords.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRecords.Location = new System.Drawing.Point(102, 735);
            this.txtRecords.Name = "txtRecords";
            this.txtRecords.Size = new System.Drawing.Size(52, 24);
            this.txtRecords.TabIndex = 3;
            this.txtRecords.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(65, 765);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 18);
            this.label1.TabIndex = 4;
            this.label1.Text = "Records Returned";
            // 
            // frmLoadReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 815);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRecords);
            this.Controls.Add(this.btnClosr);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.dgvLoad);
            this.Name = "frmLoadReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Designer Load Report";
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoad)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvLoad;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnClosr;
        private System.Windows.Forms.TextBox txtRecords;
        private System.Windows.Forms.Label label1;
    }
}