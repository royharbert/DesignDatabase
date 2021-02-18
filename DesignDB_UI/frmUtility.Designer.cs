
namespace DesignDB_UI
{
    partial class frmUtility
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
            this.btnCopyRequests = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCopyRequests
            // 
            this.btnCopyRequests.Location = new System.Drawing.Point(91, 74);
            this.btnCopyRequests.Name = "btnCopyRequests";
            this.btnCopyRequests.Size = new System.Drawing.Size(144, 57);
            this.btnCopyRequests.TabIndex = 0;
            this.btnCopyRequests.Text = "Copy to Live from Sandbox";
            this.btnCopyRequests.UseVisualStyleBackColor = true;
            this.btnCopyRequests.Click += new System.EventHandler(this.btnCopyRequests_Click);
            // 
            // frmUtility
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1200, 692);
            this.Controls.Add(this.btnCopyRequests);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmUtility";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmUtility";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCopyRequests;
    }
}