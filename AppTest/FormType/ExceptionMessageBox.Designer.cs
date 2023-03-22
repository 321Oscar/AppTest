
namespace AppTest.FormType
{
    partial class ExceptionMessageBox
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
            this.tabControlError = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.rtbMessage = new System.Windows.Forms.RichTextBox();
            this.rtbStackTrace = new System.Windows.Forms.RichTextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControlError.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlError
            // 
            this.tabControlError.Controls.Add(this.tabPage1);
            this.tabControlError.Controls.Add(this.tabPage2);
            this.tabControlError.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControlError.Location = new System.Drawing.Point(0, 0);
            this.tabControlError.Name = "tabControlError";
            this.tabControlError.SelectedIndex = 0;
            this.tabControlError.Size = new System.Drawing.Size(376, 388);
            this.tabControlError.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.rtbMessage);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(368, 362);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Message";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.rtbStackTrace);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(368, 362);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "StackTrace";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // rtbMessage
            // 
            this.rtbMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbMessage.Location = new System.Drawing.Point(3, 3);
            this.rtbMessage.Name = "rtbMessage";
            this.rtbMessage.Size = new System.Drawing.Size(362, 356);
            this.rtbMessage.TabIndex = 0;
            this.rtbMessage.Text = "";
            // 
            // rtbStackTrace
            // 
            this.rtbStackTrace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbStackTrace.Location = new System.Drawing.Point(3, 3);
            this.rtbStackTrace.Name = "rtbStackTrace";
            this.rtbStackTrace.Size = new System.Drawing.Size(362, 356);
            this.rtbStackTrace.TabIndex = 0;
            this.rtbStackTrace.Text = "";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(294, 394);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "确定";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ExceptionMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 425);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControlError);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ExceptionMessageBox";
            this.Text = "ExceptionMessageBox";
            this.tabControlError.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlError;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox rtbMessage;
        private System.Windows.Forms.RichTextBox rtbStackTrace;
        private System.Windows.Forms.Button btnClose;
    }
}