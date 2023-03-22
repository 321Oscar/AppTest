
namespace AppTest
{
    partial class DBCForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.listView1 = new System.Windows.Forms.ListView();
            this.textBox_path = new System.Windows.Forms.TextBox();
            this.button_load = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tbOffset = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbFactor = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbValue = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.tbDataByte7 = new System.Windows.Forms.TextBox();
            this.tbDataByte6 = new System.Windows.Forms.TextBox();
            this.tbDataByte5 = new System.Windows.Forms.TextBox();
            this.tbDataByte4 = new System.Windows.Forms.TextBox();
            this.tbDataByte3 = new System.Windows.Forms.TextBox();
            this.tbDataByte2 = new System.Windows.Forms.TextBox();
            this.tbDataByte1 = new System.Windows.Forms.TextBox();
            this.tbLength = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbStartBit = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDataByte0 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnAddLinear = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.btnCancle = new System.Windows.Forms.Button();
            this.btnOxyPlot = new System.Windows.Forms.Button();
            this.plotView1 = new OxyPlot.WindowsForms.PlotView();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(3, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(298, 274);
            this.treeView1.TabIndex = 6;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(307, 3);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(704, 274);
            this.listView1.TabIndex = 7;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // textBox_path
            // 
            this.textBox_path.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_path.Location = new System.Drawing.Point(2, 6);
            this.textBox_path.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_path.Name = "textBox_path";
            this.textBox_path.ReadOnly = true;
            this.textBox_path.Size = new System.Drawing.Size(362, 23);
            this.textBox_path.TabIndex = 8;
            // 
            // button_load
            // 
            this.button_load.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_load.Location = new System.Drawing.Point(368, 6);
            this.button_load.Margin = new System.Windows.Forms.Padding(2);
            this.button_load.Name = "button_load";
            this.button_load.Size = new System.Drawing.Size(62, 26);
            this.button_load.TabIndex = 9;
            this.button_load.Text = "浏览";
            this.button_load.UseVisualStyleBackColor = true;
            this.button_load.Click += new System.EventHandler(this.button_load_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.Controls.Add(this.treeView1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.listView1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1014, 320);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // panel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.textBox_path);
            this.panel1.Controls.Add(this.button_load);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 283);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 34);
            this.panel1.TabIndex = 8;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1028, 614);
            this.tabControl1.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.tbOffset);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.tbFactor);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.tbValue);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.tbDataByte7);
            this.tabPage1.Controls.Add(this.tbDataByte6);
            this.tabPage1.Controls.Add(this.tbDataByte5);
            this.tabPage1.Controls.Add(this.tbDataByte4);
            this.tabPage1.Controls.Add(this.tbDataByte3);
            this.tabPage1.Controls.Add(this.tbDataByte2);
            this.tabPage1.Controls.Add(this.tbDataByte1);
            this.tabPage1.Controls.Add(this.tbLength);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.tbStartBit);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.tbDataByte0);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.tableLayoutPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1020, 584);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "DBCDetails";
            // 
            // tbOffset
            // 
            this.tbOffset.Location = new System.Drawing.Point(250, 434);
            this.tbOffset.Name = "tbOffset";
            this.tbOffset.Size = new System.Drawing.Size(100, 23);
            this.tbOffset.TabIndex = 23;
            this.tbOffset.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(182, 437);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 17);
            this.label5.TabIndex = 29;
            this.label5.Text = "偏移量";
            // 
            // tbFactor
            // 
            this.tbFactor.BackColor = System.Drawing.Color.White;
            this.tbFactor.Location = new System.Drawing.Point(250, 398);
            this.tbFactor.Name = "tbFactor";
            this.tbFactor.Size = new System.Drawing.Size(100, 23);
            this.tbFactor.TabIndex = 22;
            this.tbFactor.Text = "1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(182, 401);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 17);
            this.label6.TabIndex = 27;
            this.label6.Text = "分辨率";
            // 
            // tbValue
            // 
            this.tbValue.Location = new System.Drawing.Point(76, 506);
            this.tbValue.Name = "tbValue";
            this.tbValue.Size = new System.Drawing.Size(100, 23);
            this.tbValue.TabIndex = 25;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 512);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 17);
            this.label4.TabIndex = 25;
            this.label4.Text = "值";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(76, 466);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(77, 34);
            this.button2.TabIndex = 24;
            this.button2.Text = "解析";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.BtnParse_Click);
            // 
            // tbDataByte7
            // 
            this.tbDataByte7.Location = new System.Drawing.Point(307, 364);
            this.tbDataByte7.Name = "tbDataByte7";
            this.tbDataByte7.Size = new System.Drawing.Size(27, 23);
            this.tbDataByte7.TabIndex = 19;
            this.tbDataByte7.Text = "0";
            // 
            // tbDataByte6
            // 
            this.tbDataByte6.Location = new System.Drawing.Point(274, 364);
            this.tbDataByte6.Name = "tbDataByte6";
            this.tbDataByte6.Size = new System.Drawing.Size(27, 23);
            this.tbDataByte6.TabIndex = 18;
            this.tbDataByte6.Text = "0";
            // 
            // tbDataByte5
            // 
            this.tbDataByte5.Location = new System.Drawing.Point(241, 364);
            this.tbDataByte5.Name = "tbDataByte5";
            this.tbDataByte5.Size = new System.Drawing.Size(27, 23);
            this.tbDataByte5.TabIndex = 17;
            this.tbDataByte5.Text = "0";
            // 
            // tbDataByte4
            // 
            this.tbDataByte4.Location = new System.Drawing.Point(208, 364);
            this.tbDataByte4.Name = "tbDataByte4";
            this.tbDataByte4.Size = new System.Drawing.Size(27, 23);
            this.tbDataByte4.TabIndex = 16;
            this.tbDataByte4.Text = "0";
            // 
            // tbDataByte3
            // 
            this.tbDataByte3.Location = new System.Drawing.Point(175, 364);
            this.tbDataByte3.Name = "tbDataByte3";
            this.tbDataByte3.Size = new System.Drawing.Size(27, 23);
            this.tbDataByte3.TabIndex = 15;
            this.tbDataByte3.Text = "0";
            // 
            // tbDataByte2
            // 
            this.tbDataByte2.Location = new System.Drawing.Point(142, 364);
            this.tbDataByte2.Name = "tbDataByte2";
            this.tbDataByte2.Size = new System.Drawing.Size(27, 23);
            this.tbDataByte2.TabIndex = 14;
            this.tbDataByte2.Text = "0";
            // 
            // tbDataByte1
            // 
            this.tbDataByte1.Location = new System.Drawing.Point(109, 364);
            this.tbDataByte1.Name = "tbDataByte1";
            this.tbDataByte1.Size = new System.Drawing.Size(27, 23);
            this.tbDataByte1.TabIndex = 13;
            this.tbDataByte1.Text = "0";
            // 
            // tbLength
            // 
            this.tbLength.Location = new System.Drawing.Point(76, 437);
            this.tbLength.Name = "tbLength";
            this.tbLength.Size = new System.Drawing.Size(100, 23);
            this.tbLength.TabIndex = 21;
            this.tbLength.Text = "8";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 440);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 17);
            this.label3.TabIndex = 15;
            this.label3.Text = "信号长度";
            // 
            // tbStartBit
            // 
            this.tbStartBit.Location = new System.Drawing.Point(76, 401);
            this.tbStartBit.Name = "tbStartBit";
            this.tbStartBit.Size = new System.Drawing.Size(100, 23);
            this.tbStartBit.TabIndex = 20;
            this.tbStartBit.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 404);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 17);
            this.label2.TabIndex = 13;
            this.label2.Text = "信号起始位";
            // 
            // tbDataByte0
            // 
            this.tbDataByte0.Location = new System.Drawing.Point(76, 364);
            this.tbDataByte0.Name = "tbDataByte0";
            this.tbDataByte0.Size = new System.Drawing.Size(27, 23);
            this.tbDataByte0.TabIndex = 12;
            this.tbDataByte0.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 370);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 17);
            this.label1.TabIndex = 11;
            this.label1.Text = "数据帧";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnAddLinear);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.checkBox2);
            this.tabPage2.Controls.Add(this.checkBox1);
            this.tabPage2.Controls.Add(this.btnCancle);
            this.tabPage2.Controls.Add(this.btnOxyPlot);
            this.tabPage2.Controls.Add(this.plotView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1020, 584);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "CurveDemo";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnAddLinear
            // 
            this.btnAddLinear.Location = new System.Drawing.Point(349, 22);
            this.btnAddLinear.Name = "btnAddLinear";
            this.btnAddLinear.Size = new System.Drawing.Size(111, 28);
            this.btnAddLinear.TabIndex = 20;
            this.btnAddLinear.Text = "Add LinearAixs";
            this.btnAddLinear.UseVisualStyleBackColor = true;
            this.btnAddLinear.Click += new System.EventHandler(this.btnAddLinear_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(255, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 28);
            this.button1.TabIndex = 19;
            this.button1.Text = "Auto";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(219, 29);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(34, 21);
            this.checkBox2.TabIndex = 18;
            this.checkBox2.Text = "Y";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.ZoomChange_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(183, 29);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(35, 21);
            this.checkBox1.TabIndex = 17;
            this.checkBox1.Text = "X";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.ZoomChange_CheckedChanged);
            // 
            // btnCancle
            // 
            this.btnCancle.Location = new System.Drawing.Point(89, 22);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(75, 28);
            this.btnCancle.TabIndex = 16;
            this.btnCancle.Text = "Cancle";
            this.btnCancle.UseVisualStyleBackColor = true;
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // btnOxyPlot
            // 
            this.btnOxyPlot.Location = new System.Drawing.Point(8, 22);
            this.btnOxyPlot.Name = "btnOxyPlot";
            this.btnOxyPlot.Size = new System.Drawing.Size(75, 28);
            this.btnOxyPlot.TabIndex = 15;
            this.btnOxyPlot.Text = "InitOxy";
            this.btnOxyPlot.UseVisualStyleBackColor = true;
            this.btnOxyPlot.Click += new System.EventHandler(this.btnOxyPlot_Click);
            // 
            // plotView1
            // 
            this.plotView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.plotView1.Location = new System.Drawing.Point(3, 92);
            this.plotView1.Name = "plotView1";
            this.plotView1.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.plotView1.Size = new System.Drawing.Size(1014, 489);
            this.plotView1.TabIndex = 14;
            this.plotView1.Text = "plotView1";
            this.plotView1.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.plotView1.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.plotView1.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // DBCForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 614);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DBCForm";
            this.Text = "DBC文件解析";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TextBox textBox_path;
        private System.Windows.Forms.Button button_load;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button btnCancle;
        private System.Windows.Forms.Button btnOxyPlot;
        private OxyPlot.WindowsForms.PlotView plotView1;
        private System.Windows.Forms.TextBox tbDataByte0;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbLength;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbStartBit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDataByte7;
        private System.Windows.Forms.TextBox tbDataByte6;
        private System.Windows.Forms.TextBox tbDataByte5;
        private System.Windows.Forms.TextBox tbDataByte4;
        private System.Windows.Forms.TextBox tbDataByte3;
        private System.Windows.Forms.TextBox tbDataByte2;
        private System.Windows.Forms.TextBox tbDataByte1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox tbValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbOffset;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbFactor;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnAddLinear;
    }
}

