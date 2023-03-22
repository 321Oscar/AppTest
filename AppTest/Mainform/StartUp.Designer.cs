
namespace AppTest.Mainform
{
    partial class StartUp
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
            this.rbMainForm = new System.Windows.Forms.RadioButton();
            this.rbMainFormV2 = new System.Windows.Forms.RadioButton();
            this.btnStart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rbMainForm
            // 
            this.rbMainForm.AutoSize = true;
            this.rbMainForm.Checked = true;
            this.rbMainForm.Location = new System.Drawing.Point(12, 12);
            this.rbMainForm.Name = "rbMainForm";
            this.rbMainForm.Size = new System.Drawing.Size(103, 21);
            this.rbMainForm.TabIndex = 0;
            this.rbMainForm.TabStop = true;
            this.rbMainForm.Text = "Root+Project";
            this.rbMainForm.UseVisualStyleBackColor = true;
            // 
            // rbMainFormV2
            // 
            this.rbMainFormV2.AutoSize = true;
            this.rbMainFormV2.Location = new System.Drawing.Point(12, 44);
            this.rbMainFormV2.Name = "rbMainFormV2";
            this.rbMainFormV2.Size = new System.Drawing.Size(99, 21);
            this.rbMainFormV2.TabIndex = 1;
            this.rbMainFormV2.Text = "Root-Project";
            this.rbMainFormV2.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(23, 75);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(88, 24);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // StartUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(128, 111);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.rbMainFormV2);
            this.Controls.Add(this.rbMainForm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "StartUp";
            this.Text = "StartUp";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbMainForm;
        private System.Windows.Forms.RadioButton rbMainFormV2;
        private System.Windows.Forms.Button btnStart;
    }
}