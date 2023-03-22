
namespace AppTest.FormType
{
    partial class ZedGraphForm
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
            this.timer_AddPoint = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.nudPointCount = new System.Windows.Forms.NumericUpDown();
            this.btnGetData = new System.Windows.Forms.Button();
            this.nudTime = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCheckOther = new System.Windows.Forms.Button();
            this.cbCheckAll = new System.Windows.Forms.CheckBox();
            this.panelLegend = new System.Windows.Forms.Panel();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.statusStrip_LogInfo = new System.Windows.Forms.StatusStrip();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPointCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTime)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer_AddPoint
            // 
            this.timer_AddPoint.Tick += new System.EventHandler(this.timer_AddPoint_Tick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelLegend, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.zedGraphControl1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip_LogInfo, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(628, 518);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.nudPointCount);
            this.panel3.Controls.Add(this.btnGetData);
            this.panel3.Controls.Add(this.nudTime);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(207, 4);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(418, 42);
            this.panel3.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(247, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "曲线点数";
            // 
            // nudPointCount
            // 
            this.nudPointCount.Location = new System.Drawing.Point(309, 8);
            this.nudPointCount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.nudPointCount.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudPointCount.Name = "nudPointCount";
            this.nudPointCount.Size = new System.Drawing.Size(51, 23);
            this.nudPointCount.TabIndex = 9;
            this.nudPointCount.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // btnGetData
            // 
            this.btnGetData.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGetData.Location = new System.Drawing.Point(98, 4);
            this.btnGetData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(101, 30);
            this.btnGetData.TabIndex = 0;
            this.btnGetData.Text = "开始获取数据";
            this.btnGetData.UseVisualStyleBackColor = true;
            this.btnGetData.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // nudTime
            // 
            this.nudTime.Location = new System.Drawing.Point(10, 8);
            this.nudTime.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.nudTime.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudTime.Name = "nudTime";
            this.nudTime.Size = new System.Drawing.Size(51, 23);
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
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(67, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "ms";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnCheckOther);
            this.panel2.Controls.Add(this.cbCheckAll);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 4);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(198, 42);
            this.panel2.TabIndex = 13;
            // 
            // btnCheckOther
            // 
            this.btnCheckOther.Location = new System.Drawing.Point(89, 4);
            this.btnCheckOther.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCheckOther.Name = "btnCheckOther";
            this.btnCheckOther.Size = new System.Drawing.Size(66, 30);
            this.btnCheckOther.TabIndex = 1;
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
            this.cbCheckAll.Location = new System.Drawing.Point(3, 9);
            this.cbCheckAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbCheckAll.Name = "cbCheckAll";
            this.cbCheckAll.Size = new System.Drawing.Size(80, 21);
            this.cbCheckAll.TabIndex = 0;
            this.cbCheckAll.Text = "全选/不选";
            this.cbCheckAll.UseVisualStyleBackColor = false;
            this.cbCheckAll.CheckedChanged += new System.EventHandler(this.cbCheckAll_CheckedChanged);
            // 
            // panelLegend
            // 
            this.panelLegend.AutoScroll = true;
            this.panelLegend.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLegend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLegend.Location = new System.Drawing.Point(3, 54);
            this.panelLegend.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelLegend.Name = "panelLegend";
            this.panelLegend.Size = new System.Drawing.Size(198, 440);
            this.panelLegend.TabIndex = 12;
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphControl1.Location = new System.Drawing.Point(207, 54);
            this.zedGraphControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(418, 440);
            this.zedGraphControl1.TabIndex = 0;
            // 
            // statusStrip_LogInfo
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.statusStrip_LogInfo, 2);
            this.statusStrip_LogInfo.Location = new System.Drawing.Point(0, 498);
            this.statusStrip_LogInfo.Name = "statusStrip_LogInfo";
            this.statusStrip_LogInfo.Size = new System.Drawing.Size(628, 20);
            this.statusStrip_LogInfo.TabIndex = 0;
            this.statusStrip_LogInfo.Text = "statusStrip1";
            // 
            // ZedGraphForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 518);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0, 132829462, 0, 132829462);
            this.Name = "ZedGraphForm";
            this.Text = "ZedGraphForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ZedGraphForm_FormClosing);
            this.Load += new System.EventHandler(this.ZedGraphForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPointCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTime)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl zedGraphControl1;
        private System.Windows.Forms.Timer timer_AddPoint;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelLegend;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCheckOther;
        private System.Windows.Forms.CheckBox cbCheckAll;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudPointCount;
        private System.Windows.Forms.Button btnGetData;
        private System.Windows.Forms.NumericUpDown nudTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip_LogInfo;
    }
}