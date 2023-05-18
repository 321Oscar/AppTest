
namespace AppTest.FormType
{
    partial class HistoryCurveForm
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
            this.plotView1 = new OxyPlot.WindowsForms.PlotView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAutoSize = new System.Windows.Forms.Button();
            this.cbYZoom = new System.Windows.Forms.CheckBox();
            this.cbXZoom = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.btnCheckOther = new System.Windows.Forms.Button();
            this.cbCheckAll = new System.Windows.Forms.CheckBox();
            this.panelLegend = new System.Windows.Forms.Panel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // plotView1
            // 
            this.plotView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plotView1.Location = new System.Drawing.Point(0, 0);
            this.plotView1.Name = "plotView1";
            this.plotView1.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.plotView1.Size = new System.Drawing.Size(589, 449);
            this.plotView1.TabIndex = 0;
            this.plotView1.Text = "plotView1";
            this.plotView1.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.plotView1.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.plotView1.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelLegend, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(803, 457);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnAutoSize);
            this.panel1.Controls.Add(this.cbYZoom);
            this.panel1.Controls.Add(this.cbXZoom);
            this.panel1.Controls.Add(this.plotView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.tableLayoutPanel1.SetRowSpan(this.panel1, 2);
            this.panel1.Size = new System.Drawing.Size(591, 451);
            this.panel1.TabIndex = 3;
            // 
            // btnAutoSize
            // 
            this.btnAutoSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAutoSize.Location = new System.Drawing.Point(508, 4);
            this.btnAutoSize.Name = "btnAutoSize";
            this.btnAutoSize.Size = new System.Drawing.Size(75, 23);
            this.btnAutoSize.TabIndex = 3;
            this.btnAutoSize.Text = "默认大小";
            this.btnAutoSize.UseVisualStyleBackColor = true;
            this.btnAutoSize.Click += new System.EventHandler(this.btnAutoSize_Click);
            // 
            // cbYZoom
            // 
            this.cbYZoom.AutoSize = true;
            this.cbYZoom.Checked = true;
            this.cbYZoom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbYZoom.Location = new System.Drawing.Point(88, 4);
            this.cbYZoom.Name = "cbYZoom";
            this.cbYZoom.Size = new System.Drawing.Size(70, 21);
            this.cbYZoom.TabIndex = 2;
            this.cbYZoom.Text = "Y轴缩放";
            this.cbYZoom.UseVisualStyleBackColor = true;
            this.cbYZoom.CheckedChanged += new System.EventHandler(this.cbYZoom_CheckedChanged);
            // 
            // cbXZoom
            // 
            this.cbXZoom.AutoSize = true;
            this.cbXZoom.Checked = true;
            this.cbXZoom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbXZoom.Location = new System.Drawing.Point(4, 4);
            this.cbXZoom.Name = "cbXZoom";
            this.cbXZoom.Size = new System.Drawing.Size(71, 21);
            this.cbXZoom.TabIndex = 1;
            this.cbXZoom.Text = "X轴缩放";
            this.cbXZoom.UseVisualStyleBackColor = true;
            this.cbXZoom.CheckedChanged += new System.EventHandler(this.cbXZoom_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.checkBox2);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.btnCheckOther);
            this.groupBox1.Controls.Add(this.cbCheckAll);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(600, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "按钮";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(74, 73);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(90, 23);
            this.numericUpDown1.TabIndex = 7;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "线条粗细";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.BackColor = System.Drawing.Color.Transparent;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(104, 54);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(63, 21);
            this.checkBox2.TabIndex = 5;
            this.checkBox2.Text = "点显示";
            this.checkBox2.UseVisualStyleBackColor = false;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.BackColor = System.Drawing.Color.Transparent;
            this.checkBox1.Location = new System.Drawing.Point(6, 54);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(80, 21);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "曲线/折线";
            this.checkBox1.UseVisualStyleBackColor = false;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // btnCheckOther
            // 
            this.btnCheckOther.Location = new System.Drawing.Point(104, 20);
            this.btnCheckOther.Name = "btnCheckOther";
            this.btnCheckOther.Size = new System.Drawing.Size(75, 23);
            this.btnCheckOther.TabIndex = 3;
            this.btnCheckOther.Text = "反选";
            this.btnCheckOther.UseVisualStyleBackColor = true;
            this.btnCheckOther.Click += new System.EventHandler(this.btnCheckOther_Click);
            // 
            // cbCheckAll
            // 
            this.cbCheckAll.AutoSize = true;
            this.cbCheckAll.BackColor = System.Drawing.Color.Transparent;
            this.cbCheckAll.Checked = true;
            this.cbCheckAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCheckAll.Location = new System.Drawing.Point(6, 24);
            this.cbCheckAll.Name = "cbCheckAll";
            this.cbCheckAll.Size = new System.Drawing.Size(80, 21);
            this.cbCheckAll.TabIndex = 2;
            this.cbCheckAll.Text = "全选/不选";
            this.cbCheckAll.UseVisualStyleBackColor = false;
            this.cbCheckAll.CheckedChanged += new System.EventHandler(this.cbCheckAll_CheckedChanged);
            // 
            // panelLegend
            // 
            this.panelLegend.AutoScroll = true;
            this.panelLegend.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLegend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLegend.Location = new System.Drawing.Point(600, 109);
            this.panelLegend.Name = "panelLegend";
            this.panelLegend.Size = new System.Drawing.Size(200, 345);
            this.panelLegend.TabIndex = 2;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem,
            this.changeXToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(803, 25);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Enabled = false;
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(60, 21);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // changeXToolStripMenuItem
            // 
            this.changeXToolStripMenuItem.Name = "changeXToolStripMenuItem";
            this.changeXToolStripMenuItem.Size = new System.Drawing.Size(72, 21);
            this.changeXToolStripMenuItem.Text = "ChangeX";
            // 
            // HistoryCurveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 482);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "HistoryCurveForm";
            this.Text = "HistoryCurveForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OxyPlot.WindowsForms.PlotView plotView1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCheckOther;
        private System.Windows.Forms.CheckBox cbCheckAll;
        private System.Windows.Forms.Panel panelLegend;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnAutoSize;
        private System.Windows.Forms.CheckBox cbYZoom;
        private System.Windows.Forms.CheckBox cbXZoom;
        private System.Windows.Forms.ToolStripMenuItem changeXToolStripMenuItem;
    }
}