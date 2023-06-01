
namespace AppTest.FormType
{
    partial class MeasureForm
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.nudPointCount = new System.Windows.Forms.NumericUpDown();
            this.btnGetData = new System.Windows.Forms.Button();
            this.nudTime = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.metroCheckBox_Y = new MetroFramework.Controls.MetroCheckBox();
            this.metroCheckBox_X = new MetroFramework.Controls.MetroCheckBox();
            this.btnFocus = new System.Windows.Forms.Button();
            this.plotView1 = new OxyPlot.WindowsForms.PlotView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.metroCheckBox_checkAll = new MetroFramework.Controls.MetroCheckBox();
            this.btnCheckOther = new System.Windows.Forms.Button();
            this.cbCheckAll = new System.Windows.Forms.CheckBox();
            this.panelLegend = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.AddValueToPlottimer = new System.Windows.Forms.Timer(this.components);
            this.metroTabControl1 = new MetroFramework.Controls.MetroTabControl();
            this.metroTabPage1 = new MetroFramework.Controls.MetroTabPage();
            this.metroTabPage2 = new MetroFramework.Controls.MetroTabPage();
            this.panelContent.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPointCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTime)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.metroTabControl1.SuspendLayout();
            this.metroTabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.metroTabControl1);
            this.panelContent.Location = new System.Drawing.Point(17, 60);
            this.panelContent.Size = new System.Drawing.Size(618, 499);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(384, 457);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.nudPointCount);
            this.panel3.Controls.Add(this.btnGetData);
            this.panel3.Controls.Add(this.nudTime);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(378, 44);
            this.panel3.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(170)))), ((int)(((byte)(173)))));
            this.label2.Location = new System.Drawing.Point(239, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "曲线点数";
            // 
            // nudPointCount
            // 
            this.nudPointCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(170)))), ((int)(((byte)(173)))));
            this.nudPointCount.Location = new System.Drawing.Point(301, 12);
            this.nudPointCount.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudPointCount.Name = "nudPointCount";
            this.nudPointCount.Size = new System.Drawing.Size(44, 23);
            this.nudPointCount.TabIndex = 9;
            this.nudPointCount.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // btnGetData
            // 
            this.btnGetData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(170)))), ((int)(((byte)(173)))));
            this.btnGetData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetData.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGetData.ForeColor = System.Drawing.Color.White;
            this.btnGetData.Location = new System.Drawing.Point(100, 6);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(100, 33);
            this.btnGetData.TabIndex = 0;
            this.btnGetData.Text = "开始获取数据";
            this.btnGetData.UseVisualStyleBackColor = false;
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // nudTime
            // 
            this.nudTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(170)))), ((int)(((byte)(173)))));
            this.nudTime.Location = new System.Drawing.Point(13, 11);
            this.nudTime.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudTime.Name = "nudTime";
            this.nudTime.Size = new System.Drawing.Size(44, 23);
            this.nudTime.TabIndex = 7;
            this.nudTime.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(170)))), ((int)(((byte)(173)))));
            this.label1.Location = new System.Drawing.Point(63, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "ms";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.metroCheckBox_Y);
            this.panel1.Controls.Add(this.metroCheckBox_X);
            this.panel1.Controls.Add(this.btnFocus);
            this.panel1.Controls.Add(this.plotView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 53);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(378, 401);
            this.panel1.TabIndex = 0;
            // 
            // metroCheckBox_Y
            // 
            this.metroCheckBox_Y.AutoSize = true;
            this.metroCheckBox_Y.Checked = true;
            this.metroCheckBox_Y.CheckState = System.Windows.Forms.CheckState.Checked;
            this.metroCheckBox_Y.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(170)))), ((int)(((byte)(173)))));
            this.metroCheckBox_Y.Location = new System.Drawing.Point(99, 9);
            this.metroCheckBox_Y.Name = "metroCheckBox_Y";
            this.metroCheckBox_Y.Size = new System.Drawing.Size(69, 15);
            this.metroCheckBox_Y.Style = MetroFramework.MetroColorStyle.Teal;
            this.metroCheckBox_Y.TabIndex = 5;
            this.metroCheckBox_Y.Text = "Y轴缩放";
            this.metroCheckBox_Y.UseSelectable = true;
            this.metroCheckBox_Y.CheckedChanged += new System.EventHandler(this.cbYZoom_CheckedChanged);
            // 
            // metroCheckBox_X
            // 
            this.metroCheckBox_X.AutoSize = true;
            this.metroCheckBox_X.Checked = true;
            this.metroCheckBox_X.CheckState = System.Windows.Forms.CheckState.Checked;
            this.metroCheckBox_X.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(170)))), ((int)(((byte)(173)))));
            this.metroCheckBox_X.Location = new System.Drawing.Point(2, 9);
            this.metroCheckBox_X.Name = "metroCheckBox_X";
            this.metroCheckBox_X.Size = new System.Drawing.Size(69, 15);
            this.metroCheckBox_X.Style = MetroFramework.MetroColorStyle.Teal;
            this.metroCheckBox_X.TabIndex = 4;
            this.metroCheckBox_X.Text = "X轴缩放";
            this.metroCheckBox_X.UseSelectable = true;
            this.metroCheckBox_X.CheckedChanged += new System.EventHandler(this.cbXZoom_CheckedChanged);
            // 
            // btnFocus
            // 
            this.btnFocus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFocus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(170)))), ((int)(((byte)(173)))));
            this.btnFocus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFocus.ForeColor = System.Drawing.Color.White;
            this.btnFocus.Location = new System.Drawing.Point(325, 2);
            this.btnFocus.Name = "btnFocus";
            this.btnFocus.Size = new System.Drawing.Size(48, 33);
            this.btnFocus.TabIndex = 3;
            this.btnFocus.Text = "聚焦";
            this.btnFocus.UseVisualStyleBackColor = false;
            this.btnFocus.Click += new System.EventHandler(this.btnFocus_Click);
            // 
            // plotView1
            // 
            this.plotView1.BackColor = System.Drawing.Color.White;
            this.plotView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plotView1.Location = new System.Drawing.Point(0, 0);
            this.plotView1.Name = "plotView1";
            this.plotView1.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.plotView1.Size = new System.Drawing.Size(376, 399);
            this.plotView1.TabIndex = 0;
            this.plotView1.Text = "plotView1";
            this.plotView1.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.plotView1.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.plotView1.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.metroCheckBox_checkAll);
            this.panel2.Controls.Add(this.btnCheckOther);
            this.panel2.Controls.Add(this.cbCheckAll);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(217, 44);
            this.panel2.TabIndex = 1;
            // 
            // metroCheckBox_checkAll
            // 
            this.metroCheckBox_checkAll.AutoSize = true;
            this.metroCheckBox_checkAll.Checked = true;
            this.metroCheckBox_checkAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.metroCheckBox_checkAll.Location = new System.Drawing.Point(3, 16);
            this.metroCheckBox_checkAll.Name = "metroCheckBox_checkAll";
            this.metroCheckBox_checkAll.Size = new System.Drawing.Size(80, 15);
            this.metroCheckBox_checkAll.Style = MetroFramework.MetroColorStyle.Teal;
            this.metroCheckBox_checkAll.TabIndex = 2;
            this.metroCheckBox_checkAll.Text = "全选/不选";
            this.metroCheckBox_checkAll.UseSelectable = true;
            this.metroCheckBox_checkAll.CheckedChanged += new System.EventHandler(this.cbCheckAll_CheckedChanged);
            // 
            // btnCheckOther
            // 
            this.btnCheckOther.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(170)))), ((int)(((byte)(173)))));
            this.btnCheckOther.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckOther.ForeColor = System.Drawing.Color.White;
            this.btnCheckOther.Location = new System.Drawing.Point(86, 3);
            this.btnCheckOther.Name = "btnCheckOther";
            this.btnCheckOther.Size = new System.Drawing.Size(57, 33);
            this.btnCheckOther.TabIndex = 1;
            this.btnCheckOther.Text = "反选";
            this.btnCheckOther.UseVisualStyleBackColor = false;
            this.btnCheckOther.Click += new System.EventHandler(this.btnCheckOther_Click);
            // 
            // cbCheckAll
            // 
            this.cbCheckAll.AutoSize = true;
            this.cbCheckAll.BackColor = System.Drawing.Color.Transparent;
            this.cbCheckAll.Checked = true;
            this.cbCheckAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCheckAll.Location = new System.Drawing.Point(153, 12);
            this.cbCheckAll.Name = "cbCheckAll";
            this.cbCheckAll.Size = new System.Drawing.Size(80, 21);
            this.cbCheckAll.TabIndex = 0;
            this.cbCheckAll.Text = "全选/不选";
            this.cbCheckAll.UseVisualStyleBackColor = false;
            this.cbCheckAll.Visible = false;
            this.cbCheckAll.CheckedChanged += new System.EventHandler(this.cbCheckAll_CheckedChanged);
            // 
            // panelLegend
            // 
            this.panelLegend.AutoScroll = true;
            this.panelLegend.BackColor = System.Drawing.Color.White;
            this.panelLegend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLegend.Location = new System.Drawing.Point(3, 53);
            this.panelLegend.Name = "panelLegend";
            this.panelLegend.Size = new System.Drawing.Size(217, 401);
            this.panelLegend.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel2);
            this.splitContainer1.Size = new System.Drawing.Size(610, 457);
            this.splitContainer1.SplitterDistance = 384;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panelLegend, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(223, 457);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // AddValueToPlottimer
            // 
            this.AddValueToPlottimer.Interval = 5;
            this.AddValueToPlottimer.Tick += new System.EventHandler(this.AddValueToPlottimer_Tick);
            // 
            // metroTabControl1
            // 
            this.metroTabControl1.Controls.Add(this.metroTabPage1);
            this.metroTabControl1.Controls.Add(this.metroTabPage2);
            this.metroTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabControl1.Location = new System.Drawing.Point(0, 0);
            this.metroTabControl1.Name = "metroTabControl1";
            this.metroTabControl1.SelectedIndex = 0;
            this.metroTabControl1.Size = new System.Drawing.Size(618, 499);
            this.metroTabControl1.TabIndex = 2;
            this.metroTabControl1.UseSelectable = true;
            // 
            // metroTabPage1
            // 
            this.metroTabPage1.BackColor = System.Drawing.Color.White;
            this.metroTabPage1.Controls.Add(this.splitContainer1);
            this.metroTabPage1.HorizontalScrollbarBarColor = true;
            this.metroTabPage1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.HorizontalScrollbarSize = 10;
            this.metroTabPage1.Location = new System.Drawing.Point(4, 38);
            this.metroTabPage1.Name = "metroTabPage1";
            this.metroTabPage1.Size = new System.Drawing.Size(610, 457);
            this.metroTabPage1.TabIndex = 0;
            this.metroTabPage1.Text = "实时数据";
            this.metroTabPage1.VerticalScrollbarBarColor = true;
            this.metroTabPage1.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.VerticalScrollbarSize = 10;
            // 
            // metroTabPage2
            // 
            this.metroTabPage2.HorizontalScrollbarBarColor = true;
            this.metroTabPage2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage2.HorizontalScrollbarSize = 10;
            this.metroTabPage2.Location = new System.Drawing.Point(4, 38);
            this.metroTabPage2.Name = "metroTabPage2";
            this.metroTabPage2.Size = new System.Drawing.Size(702, 402);
            this.metroTabPage2.TabIndex = 1;
            this.metroTabPage2.Text = "历史数据";
            this.metroTabPage2.VerticalScrollbarBarColor = true;
            this.metroTabPage2.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage2.VerticalScrollbarSize = 10;
            // 
            // MeasureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 582);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(500, 195);
            this.Name = "MeasureForm";
            this.Padding = new System.Windows.Forms.Padding(17, 60, 17, 0);
            this.Text = "MeasureForm";
            this.Load += new System.EventHandler(this.MeasureForm_Load);
            this.panelContent.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPointCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTime)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.metroTabControl1.ResumeLayout(false);
            this.metroTabPage1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelLegend;
        private System.Windows.Forms.Panel panel1;
        protected OxyPlot.WindowsForms.PlotView plotView1;
        private System.Windows.Forms.Button btnGetData;
        private System.Windows.Forms.Label label1;
        protected System.Windows.Forms.NumericUpDown nudTime;
        private System.Windows.Forms.CheckBox cbCheckAll;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCheckOther;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnFocus;
        private System.Windows.Forms.Timer AddValueToPlottimer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudPointCount;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private MetroFramework.Controls.MetroTabControl metroTabControl1;
        private MetroFramework.Controls.MetroTabPage metroTabPage1;
        protected MetroFramework.Controls.MetroTabPage metroTabPage2;
        private MetroFramework.Controls.MetroCheckBox metroCheckBox_checkAll;
        private MetroFramework.Controls.MetroCheckBox metroCheckBox_Y;
        private MetroFramework.Controls.MetroCheckBox metroCheckBox_X;
    }
}