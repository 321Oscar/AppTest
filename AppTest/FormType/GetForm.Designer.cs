
namespace AppTest.FormType
{
    partial class GetForm
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbLog = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nudTimerInterval = new System.Windows.Forms.NumericUpDown();
            this.btnGet = new System.Windows.Forms.Button();
            this.gbSignals = new System.Windows.Forms.GroupBox();
            this.pnSignals = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimerInterval)).BeginInit();
            this.gbSignals.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.gbSignals, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(260, 257);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbLog);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.nudTimerInterval);
            this.panel1.Controls.Add(this.btnGet);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(2, 224);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(256, 31);
            this.panel1.TabIndex = 0;
            // 
            // lbLog
            // 
            this.lbLog.AutoSize = true;
            this.lbLog.ForeColor = System.Drawing.Color.Red;
            this.lbLog.Location = new System.Drawing.Point(158, 9);
            this.lbLog.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(30, 17);
            this.lbLog.TabIndex = 3;
            this.lbLog.Text = "Log";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "ms";
            // 
            // nudTimerInterval
            // 
            this.nudTimerInterval.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudTimerInterval.Location = new System.Drawing.Point(2, 7);
            this.nudTimerInterval.Margin = new System.Windows.Forms.Padding(2);
            this.nudTimerInterval.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudTimerInterval.Name = "nudTimerInterval";
            this.nudTimerInterval.Size = new System.Drawing.Size(45, 23);
            this.nudTimerInterval.TabIndex = 1;
            this.nudTimerInterval.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // btnGet
            // 
            this.btnGet.Location = new System.Drawing.Point(78, 6);
            this.btnGet.Margin = new System.Windows.Forms.Padding(2);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(71, 25);
            this.btnGet.TabIndex = 0;
            this.btnGet.Text = "AutoGet";
            this.btnGet.UseVisualStyleBackColor = true;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // gbSignals
            // 
            this.gbSignals.Controls.Add(this.pnSignals);
            this.gbSignals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSignals.Location = new System.Drawing.Point(2, 2);
            this.gbSignals.Margin = new System.Windows.Forms.Padding(2);
            this.gbSignals.Name = "gbSignals";
            this.gbSignals.Padding = new System.Windows.Forms.Padding(2);
            this.gbSignals.Size = new System.Drawing.Size(256, 218);
            this.gbSignals.TabIndex = 1;
            this.gbSignals.TabStop = false;
            this.gbSignals.Text = "信号";
            // 
            // pnSignals
            // 
            this.pnSignals.AutoScroll = true;
            this.pnSignals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnSignals.Location = new System.Drawing.Point(2, 18);
            this.pnSignals.Margin = new System.Windows.Forms.Padding(2);
            this.pnSignals.Name = "pnSignals";
            this.pnSignals.Size = new System.Drawing.Size(252, 198);
            this.pnSignals.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(272, 291);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(264, 261);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "实时数据";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(264, 261);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "历史数据";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // GetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(272, 291);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "GetForm";
            this.Text = "GetForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GetForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GetForm_FormClosed);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimerInterval)).EndInit();
            this.gbSignals.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnGet;
        private System.Windows.Forms.GroupBox gbSignals;
        private System.Windows.Forms.Panel pnSignals;
        private System.Windows.Forms.NumericUpDown nudTimerInterval;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label lbLog;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}