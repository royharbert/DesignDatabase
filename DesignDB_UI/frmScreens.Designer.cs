
namespace DesignDB_UI
{
    partial class frmScreens
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
            this.rdoScreen1 = new System.Windows.Forms.RadioButton();
            this.rdoScreen2 = new System.Windows.Forms.RadioButton();
            this.rdoScreen3 = new System.Windows.Forms.RadioButton();
            this.rdoScreen4 = new System.Windows.Forms.RadioButton();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rdoScreen1
            // 
            this.rdoScreen1.AutoSize = true;
            this.rdoScreen1.Location = new System.Drawing.Point(116, 55);
            this.rdoScreen1.Name = "rdoScreen1";
            this.rdoScreen1.Size = new System.Drawing.Size(91, 24);
            this.rdoScreen1.TabIndex = 0;
            this.rdoScreen1.TabStop = true;
            this.rdoScreen1.Tag = "1";
            this.rdoScreen1.Text = "Screen 1";
            this.rdoScreen1.UseVisualStyleBackColor = true;
            this.rdoScreen1.Visible = false;
            this.rdoScreen1.CheckedChanged += new System.EventHandler(this.rdoScreen1_CheckedChanged);
            // 
            // rdoScreen2
            // 
            this.rdoScreen2.AutoSize = true;
            this.rdoScreen2.Location = new System.Drawing.Point(116, 85);
            this.rdoScreen2.Name = "rdoScreen2";
            this.rdoScreen2.Size = new System.Drawing.Size(91, 24);
            this.rdoScreen2.TabIndex = 1;
            this.rdoScreen2.TabStop = true;
            this.rdoScreen2.Tag = "2";
            this.rdoScreen2.Text = "Screen 2";
            this.rdoScreen2.UseVisualStyleBackColor = true;
            this.rdoScreen2.Visible = false;
            this.rdoScreen2.CheckedChanged += new System.EventHandler(this.rdoScreen2_CheckedChanged);
            // 
            // rdoScreen3
            // 
            this.rdoScreen3.AutoSize = true;
            this.rdoScreen3.Location = new System.Drawing.Point(116, 115);
            this.rdoScreen3.Name = "rdoScreen3";
            this.rdoScreen3.Size = new System.Drawing.Size(91, 24);
            this.rdoScreen3.TabIndex = 2;
            this.rdoScreen3.TabStop = true;
            this.rdoScreen3.Tag = "3";
            this.rdoScreen3.Text = "Screen 3";
            this.rdoScreen3.UseVisualStyleBackColor = true;
            this.rdoScreen3.Visible = false;
            this.rdoScreen3.CheckedChanged += new System.EventHandler(this.rdoScreen3_CheckedChanged);
            // 
            // rdoScreen4
            // 
            this.rdoScreen4.AutoSize = true;
            this.rdoScreen4.Location = new System.Drawing.Point(116, 145);
            this.rdoScreen4.Name = "rdoScreen4";
            this.rdoScreen4.Size = new System.Drawing.Size(91, 24);
            this.rdoScreen4.TabIndex = 3;
            this.rdoScreen4.TabStop = true;
            this.rdoScreen4.Tag = "4";
            this.rdoScreen4.Text = "Screen 4";
            this.rdoScreen4.UseVisualStyleBackColor = true;
            this.rdoScreen4.Visible = false;
            this.rdoScreen4.CheckedChanged += new System.EventHandler(this.rdoScreen4_CheckedChanged);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(102, 214);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(119, 37);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmScreens
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 324);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.rdoScreen4);
            this.Controls.Add(this.rdoScreen3);
            this.Controls.Add(this.rdoScreen2);
            this.Controls.Add(this.rdoScreen1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmScreens";
            this.Text = "Screen Selection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoScreen1;
        private System.Windows.Forms.RadioButton rdoScreen2;
        private System.Windows.Forms.RadioButton rdoScreen3;
        private System.Windows.Forms.RadioButton rdoScreen4;
        private System.Windows.Forms.Button btnClose;
    }
}