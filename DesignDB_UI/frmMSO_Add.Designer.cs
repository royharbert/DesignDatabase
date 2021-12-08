namespace DesignDB_UI
{
    partial class frmMSO_Add
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
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTLA = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(345, 126);
            this.txtName.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(826, 35);
            this.txtName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(158, 132);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = "MSO Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 186);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(285, 29);
            this.label3.TabIndex = 5;
            this.label3.Text = "Three Letter Abbreviation";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTLA
            // 
            this.txtTLA.Location = new System.Drawing.Point(345, 180);
            this.txtTLA.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtTLA.Name = "txtTLA";
            this.txtTLA.Size = new System.Drawing.Size(826, 35);
            this.txtTLA.TabIndex = 4;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(360, 329);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(184, 63);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(747, 329);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(184, 63);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel/Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmMSO_Add
            // 
            this.AcceptButton = this.btnAdd;
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1251, 440);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtTLA);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtName);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "frmMSO_Add";
            this.Text = "Add MSO";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTLA;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnCancel;
    }
}