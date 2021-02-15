
namespace DesignDB_Utilities
{
    partial class frmUtilityMenu
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
            this.btnImportAtt = new System.Windows.Forms.Button();
            this.btnForecast = new System.Windows.Forms.Button();
            this.txtRow = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnImportAtt
            // 
            this.btnImportAtt.Location = new System.Drawing.Point(124, 91);
            this.btnImportAtt.Name = "btnImportAtt";
            this.btnImportAtt.Size = new System.Drawing.Size(170, 33);
            this.btnImportAtt.TabIndex = 0;
            this.btnImportAtt.Text = "Import Attachments";
            this.btnImportAtt.UseVisualStyleBackColor = true;
            this.btnImportAtt.Click += new System.EventHandler(this.btnImportAtt_Click);
            // 
            // btnForecast
            // 
            this.btnForecast.Location = new System.Drawing.Point(124, 130);
            this.btnForecast.Name = "btnForecast";
            this.btnForecast.Size = new System.Drawing.Size(170, 33);
            this.btnForecast.TabIndex = 1;
            this.btnForecast.Text = "Create Forecast";
            this.btnForecast.UseVisualStyleBackColor = true;
            this.btnForecast.Click += new System.EventHandler(this.btnForecast_Click);
            // 
            // txtRow
            // 
            this.txtRow.Location = new System.Drawing.Point(316, 91);
            this.txtRow.Name = "txtRow";
            this.txtRow.Size = new System.Drawing.Size(166, 26);
            this.txtRow.TabIndex = 2;
            // 
            // frmUtilityMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Tan;
            this.ClientSize = new System.Drawing.Size(1200, 692);
            this.Controls.Add(this.txtRow);
            this.Controls.Add(this.btnForecast);
            this.Controls.Add(this.btnImportAtt);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmUtilityMenu";
            this.Text = "Utilities";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnImportAtt;
        private System.Windows.Forms.Button btnForecast;
        private System.Windows.Forms.TextBox txtRow;
    }
}

