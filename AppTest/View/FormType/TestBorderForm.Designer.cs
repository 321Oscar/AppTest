
namespace AppTest.FormType
{
    partial class TestBorderForm
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
            this.panelTitle = new System.Windows.Forms.Panel();
            this.buttonNor = new System.Windows.Forms.Button();
            this.buttonMin = new System.Windows.Forms.Button();
            this.buttonMax = new System.Windows.Forms.Button();
            this.lbTitle = new System.Windows.Forms.Label();
            this.panelControls = new System.Windows.Forms.Panel();
            this.statusStripLog = new System.Windows.Forms.StatusStrip();
            this.panelLog = new System.Windows.Forms.Panel();
            this.panelTitle.SuspendLayout();
            this.panelLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTitle
            // 
            this.panelTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTitle.BackColor = System.Drawing.Color.DodgerBlue;
            this.panelTitle.Controls.Add(this.buttonNor);
            this.panelTitle.Controls.Add(this.buttonMin);
            this.panelTitle.Controls.Add(this.buttonMax);
            this.panelTitle.Controls.Add(this.lbTitle);
            this.panelTitle.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.panelTitle.Location = new System.Drawing.Point(0, 0);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(334, 57);
            this.panelTitle.TabIndex = 1;
            this.panelTitle.Visible = false;
            this.panelTitle.DoubleClick += new System.EventHandler(this.panelTitle_DoubleClick);
            this.panelTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.panelTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.panelTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            // 
            // buttonNor
            // 
            this.buttonNor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNor.BackColor = System.Drawing.Color.White;
            this.buttonNor.Cursor = System.Windows.Forms.Cursors.Default;
            this.buttonNor.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonNor.ForeColor = System.Drawing.Color.DodgerBlue;
            this.buttonNor.Location = new System.Drawing.Point(248, 13);
            this.buttonNor.Name = "buttonNor";
            this.buttonNor.Size = new System.Drawing.Size(34, 30);
            this.buttonNor.TabIndex = 3;
            this.buttonNor.Text = "□";
            this.buttonNor.UseVisualStyleBackColor = false;
            this.buttonNor.Click += new System.EventHandler(this.buttonMax_Click);
            // 
            // buttonMin
            // 
            this.buttonMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMin.BackColor = System.Drawing.Color.White;
            this.buttonMin.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.buttonMin.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonMin.ForeColor = System.Drawing.Color.DodgerBlue;
            this.buttonMin.Location = new System.Drawing.Point(208, 13);
            this.buttonMin.Name = "buttonMin";
            this.buttonMin.Size = new System.Drawing.Size(34, 30);
            this.buttonMin.TabIndex = 2;
            this.buttonMin.Text = "——";
            this.buttonMin.UseVisualStyleBackColor = false;
            this.buttonMin.Visible = false;
            this.buttonMin.Click += new System.EventHandler(this.buttonMin_Click);
            // 
            // buttonMax
            // 
            this.buttonMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMax.BackColor = System.Drawing.Color.White;
            this.buttonMax.Cursor = System.Windows.Forms.Cursors.Default;
            this.buttonMax.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonMax.ForeColor = System.Drawing.Color.DodgerBlue;
            this.buttonMax.Location = new System.Drawing.Point(288, 13);
            this.buttonMax.Name = "buttonMax";
            this.buttonMax.Size = new System.Drawing.Size(34, 30);
            this.buttonMax.TabIndex = 1;
            this.buttonMax.Text = "×";
            this.buttonMax.UseVisualStyleBackColor = false;
            this.buttonMax.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Cursor = System.Windows.Forms.Cursors.Default;
            this.lbTitle.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbTitle.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lbTitle.Location = new System.Drawing.Point(12, 12);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(87, 31);
            this.lbTitle.TabIndex = 0;
            this.lbTitle.Text = "label1";
            // 
            // panelControls
            // 
            this.panelControls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControls.BackColor = System.Drawing.Color.White;
            this.panelControls.Location = new System.Drawing.Point(0, 67);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(334, 230);
            this.panelControls.TabIndex = 2;
            // 
            // statusStripLog
            // 
            this.statusStripLog.BackColor = System.Drawing.Color.Transparent;
            this.statusStripLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStripLog.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStripLog.Location = new System.Drawing.Point(0, 0);
            this.statusStripLog.Name = "statusStripLog";
            this.statusStripLog.Size = new System.Drawing.Size(334, 25);
            this.statusStripLog.TabIndex = 3;
            this.statusStripLog.Text = "statusStrip1";
            // 
            // panelLog
            // 
            this.panelLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelLog.Controls.Add(this.statusStripLog);
            this.panelLog.Location = new System.Drawing.Point(0, 315);
            this.panelLog.Name = "panelLog";
            this.panelLog.Size = new System.Drawing.Size(334, 25);
            this.panelLog.TabIndex = 4;
            // 
            // TestBorderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 344);
            this.Controls.Add(this.panelLog);
            this.Controls.Add(this.panelControls);
            this.Controls.Add(this.panelTitle);
            this.Name = "TestBorderForm";
            this.Text = "TestBorderForm";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TestBorderForm_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            this.panelTitle.ResumeLayout(false);
            this.panelTitle.PerformLayout();
            this.panelLog.ResumeLayout(false);
            this.panelLog.PerformLayout();
            this.ResumeLayout(false);

        }

        

        #endregion
        public System.Windows.Forms.Panel panelTitle;
        public System.Windows.Forms.Panel panelControls;
        public System.Windows.Forms.StatusStrip statusStripLog;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Button buttonMax;
        private System.Windows.Forms.Button buttonNor;
        private System.Windows.Forms.Button buttonMin;
        private System.Windows.Forms.Panel panelLog;
    }
}