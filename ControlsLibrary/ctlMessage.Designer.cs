namespace ControlsLibrary
{
    partial class ctlMessage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblMsg = new Label();
            SuspendLayout();
            // 
            // lblMsg
            // 
            lblMsg.AutoSize = true;
            lblMsg.Dock = DockStyle.Fill;
            lblMsg.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            lblMsg.Location = new Point(0, 0);
            lblMsg.Name = "lblMsg";
            lblMsg.Size = new Size(50, 20);
            lblMsg.TabIndex = 0;
            lblMsg.Text = "label1";
            // 
            // ctlMessage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            Controls.Add(lblMsg);
            Name = "ctlMessage";
            Size = new Size(50, 20);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblMsg;
    }
}
