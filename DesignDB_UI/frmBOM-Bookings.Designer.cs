namespace DesignDB_UI
{
    partial class frmBOM_Bookings
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
            this.btnImportBookings = new System.Windows.Forms.Button();
            this.ofdImport = new System.Windows.Forms.OpenFileDialog();
            this.pbBookings = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // btnImportBookings
            // 
            this.btnImportBookings.Location = new System.Drawing.Point(79, 514);
            this.btnImportBookings.Name = "btnImportBookings";
            this.btnImportBookings.Size = new System.Drawing.Size(127, 57);
            this.btnImportBookings.TabIndex = 0;
            this.btnImportBookings.Text = "Import Bookings";
            this.btnImportBookings.UseVisualStyleBackColor = true;
            this.btnImportBookings.Click += new System.EventHandler(this.btnImportBookings_Click);
            // 
            // ofdImport
            // 
            this.ofdImport.FileName = "openFileDialog1";
            // 
            // pbBookings
            // 
            this.pbBookings.Location = new System.Drawing.Point(31, 612);
            this.pbBookings.Name = "pbBookings";
            this.pbBookings.Size = new System.Drawing.Size(214, 22);
            this.pbBookings.TabIndex = 1;
            // 
            // frmBOM_Bookings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 692);
            this.Controls.Add(this.pbBookings);
            this.Controls.Add(this.btnImportBookings);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmBOM_Bookings";
            this.Text = "Bookingd to BOM";
            this.Load += new System.EventHandler(this.frmBOM_Bookings_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnImportBookings;
        private System.Windows.Forms.OpenFileDialog ofdImport;
        private System.Windows.Forms.ProgressBar pbBookings;
    }
}