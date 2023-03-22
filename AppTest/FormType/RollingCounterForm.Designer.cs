
namespace AppTest.FormType
{
    partial class RollingCounterForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.plSend = new System.Windows.Forms.Panel();
            this.metroTextBox_CurVal = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroComboBox_Signals = new MetroFramework.Controls.MetroComboBox();
            this.btnAutoSend = new System.Windows.Forms.Button();
            this.lbCoe = new System.Windows.Forms.Label();
            this.btnSetValue = new System.Windows.Forms.Button();
            this.btnDivision = new System.Windows.Forms.Button();
            this.btnMultip = new System.Windows.Forms.Button();
            this.btnReduce = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.nudCoe = new System.Windows.Forms.NumericUpDown();
            this.tbCurrent = new System.Windows.Forms.TextBox();
            this.cbbSignals = new System.Windows.Forms.ComboBox();
            this.pnSignals = new System.Windows.Forms.Panel();
            this.tsslbLog = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.plSend.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCoe)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pnSignals, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(350, 366);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // plSend
            // 
            this.plSend.Controls.Add(this.metroTextBox_CurVal);
            this.plSend.Controls.Add(this.metroLabel1);
            this.plSend.Controls.Add(this.metroComboBox_Signals);
            this.plSend.Controls.Add(this.btnAutoSend);
            this.plSend.Controls.Add(this.lbCoe);
            this.plSend.Controls.Add(this.btnSetValue);
            this.plSend.Controls.Add(this.btnDivision);
            this.plSend.Controls.Add(this.btnMultip);
            this.plSend.Controls.Add(this.btnReduce);
            this.plSend.Controls.Add(this.btnAdd);
            this.plSend.Controls.Add(this.nudCoe);
            this.plSend.Controls.Add(this.tbCurrent);
            this.plSend.Controls.Add(this.cbbSignals);
            this.plSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plSend.Location = new System.Drawing.Point(3, 19);
            this.plSend.Name = "plSend";
            this.plSend.Size = new System.Drawing.Size(338, 122);
            this.plSend.TabIndex = 2;
            // 
            // metroTextBox_CurVal
            // 
            // 
            // 
            // 
            this.metroTextBox_CurVal.CustomButton.Image = null;
            this.metroTextBox_CurVal.CustomButton.Location = new System.Drawing.Point(47, 1);
            this.metroTextBox_CurVal.CustomButton.Name = "";
            this.metroTextBox_CurVal.CustomButton.Size = new System.Drawing.Size(27, 27);
            this.metroTextBox_CurVal.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox_CurVal.CustomButton.TabIndex = 1;
            this.metroTextBox_CurVal.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox_CurVal.CustomButton.UseSelectable = true;
            this.metroTextBox_CurVal.CustomButton.Visible = false;
            this.metroTextBox_CurVal.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.metroTextBox_CurVal.Lines = new string[] {
        "metroTextBox1"};
            this.metroTextBox_CurVal.Location = new System.Drawing.Point(251, 48);
            this.metroTextBox_CurVal.MaxLength = 32767;
            this.metroTextBox_CurVal.Name = "metroTextBox_CurVal";
            this.metroTextBox_CurVal.PasswordChar = '\0';
            this.metroTextBox_CurVal.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox_CurVal.SelectedText = "";
            this.metroTextBox_CurVal.SelectionLength = 0;
            this.metroTextBox_CurVal.SelectionStart = 0;
            this.metroTextBox_CurVal.ShortcutsEnabled = true;
            this.metroTextBox_CurVal.Size = new System.Drawing.Size(75, 29);
            this.metroTextBox_CurVal.Style = MetroFramework.MetroColorStyle.Green;
            this.metroTextBox_CurVal.TabIndex = 11;
            this.metroTextBox_CurVal.Text = "metroTextBox1";
            this.metroTextBox_CurVal.UseSelectable = true;
            this.metroTextBox_CurVal.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox_CurVal.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.metroTextBox_CurVal.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbCurrent_KeyUp);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(194, 53);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(51, 19);
            this.metroLabel1.Style = MetroFramework.MetroColorStyle.Green;
            this.metroLabel1.TabIndex = 10;
            this.metroLabel1.Text = "当前值";
            // 
            // metroComboBox_Signals
            // 
            this.metroComboBox_Signals.FormattingEnabled = true;
            this.metroComboBox_Signals.ItemHeight = 23;
            this.metroComboBox_Signals.Location = new System.Drawing.Point(9, 48);
            this.metroComboBox_Signals.Name = "metroComboBox_Signals";
            this.metroComboBox_Signals.Size = new System.Drawing.Size(179, 29);
            this.metroComboBox_Signals.Style = MetroFramework.MetroColorStyle.Green;
            this.metroComboBox_Signals.TabIndex = 9;
            this.metroComboBox_Signals.UseSelectable = true;
            this.metroComboBox_Signals.SelectedIndexChanged += new System.EventHandler(this.cbbSignals_SelectedIndexChanged);
            // 
            // btnAutoSend
            // 
            this.btnAutoSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAutoSend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(177)))), ((int)(((byte)(89)))));
            this.btnAutoSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoSend.ForeColor = System.Drawing.Color.White;
            this.btnAutoSend.Location = new System.Drawing.Point(9, 6);
            this.btnAutoSend.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAutoSend.Name = "btnAutoSend";
            this.btnAutoSend.Size = new System.Drawing.Size(76, 33);
            this.btnAutoSend.TabIndex = 0;
            this.btnAutoSend.Text = "停止";
            this.btnAutoSend.UseVisualStyleBackColor = false;
            this.btnAutoSend.Click += new System.EventHandler(this.btnAutoSend_Click);
            // 
            // lbCoe
            // 
            this.lbCoe.AutoSize = true;
            this.lbCoe.Location = new System.Drawing.Point(9, 91);
            this.lbCoe.Name = "lbCoe";
            this.lbCoe.Size = new System.Drawing.Size(32, 17);
            this.lbCoe.TabIndex = 8;
            this.lbCoe.Text = "步长";
            // 
            // btnSetValue
            // 
            this.btnSetValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSetValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(177)))), ((int)(((byte)(89)))));
            this.btnSetValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetValue.ForeColor = System.Drawing.Color.White;
            this.btnSetValue.Location = new System.Drawing.Point(194, 86);
            this.btnSetValue.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSetValue.Name = "btnSetValue";
            this.btnSetValue.Size = new System.Drawing.Size(76, 33);
            this.btnSetValue.TabIndex = 7;
            this.btnSetValue.Text = "设定";
            this.btnSetValue.UseVisualStyleBackColor = false;
            this.btnSetValue.Click += new System.EventHandler(this.btnSetValue_Click);
            // 
            // btnDivision
            // 
            this.btnDivision.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDivision.Location = new System.Drawing.Point(372, 86);
            this.btnDivision.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDivision.Name = "btnDivision";
            this.btnDivision.Size = new System.Drawing.Size(37, 27);
            this.btnDivision.TabIndex = 6;
            this.btnDivision.Text = "除";
            this.btnDivision.UseVisualStyleBackColor = true;
            this.btnDivision.Visible = false;
            this.btnDivision.Click += new System.EventHandler(this.btnChangeValue_Click);
            // 
            // btnMultip
            // 
            this.btnMultip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMultip.Location = new System.Drawing.Point(329, 86);
            this.btnMultip.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnMultip.Name = "btnMultip";
            this.btnMultip.Size = new System.Drawing.Size(37, 27);
            this.btnMultip.TabIndex = 5;
            this.btnMultip.Text = "乘";
            this.btnMultip.UseVisualStyleBackColor = true;
            this.btnMultip.Visible = false;
            this.btnMultip.Click += new System.EventHandler(this.btnChangeValue_Click);
            // 
            // btnReduce
            // 
            this.btnReduce.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReduce.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(177)))), ((int)(((byte)(89)))));
            this.btnReduce.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReduce.ForeColor = System.Drawing.Color.White;
            this.btnReduce.Location = new System.Drawing.Point(151, 86);
            this.btnReduce.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnReduce.Name = "btnReduce";
            this.btnReduce.Size = new System.Drawing.Size(37, 33);
            this.btnReduce.TabIndex = 4;
            this.btnReduce.Text = "减";
            this.btnReduce.UseVisualStyleBackColor = false;
            this.btnReduce.Click += new System.EventHandler(this.btnChangeValue_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(177)))), ((int)(((byte)(89)))));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(108, 86);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(37, 33);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "加";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnChangeValue_Click);
            // 
            // nudCoe
            // 
            this.nudCoe.Location = new System.Drawing.Point(47, 89);
            this.nudCoe.Name = "nudCoe";
            this.nudCoe.Size = new System.Drawing.Size(55, 23);
            this.nudCoe.TabIndex = 2;
            // 
            // tbCurrent
            // 
            this.tbCurrent.Location = new System.Drawing.Point(257, 11);
            this.tbCurrent.Name = "tbCurrent";
            this.tbCurrent.Size = new System.Drawing.Size(69, 23);
            this.tbCurrent.TabIndex = 1;
            this.tbCurrent.Visible = false;
            this.tbCurrent.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbCurrent_KeyUp);
            // 
            // cbbSignals
            // 
            this.cbbSignals.FormattingEnabled = true;
            this.cbbSignals.Location = new System.Drawing.Point(91, 9);
            this.cbbSignals.Name = "cbbSignals";
            this.cbbSignals.Size = new System.Drawing.Size(136, 25);
            this.cbbSignals.TabIndex = 0;
            this.cbbSignals.Visible = false;
            this.cbbSignals.SelectedIndexChanged += new System.EventHandler(this.cbbSignals_SelectedIndexChanged);
            // 
            // pnSignals
            // 
            this.pnSignals.AutoScroll = true;
            this.pnSignals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnSignals.Location = new System.Drawing.Point(3, 154);
            this.pnSignals.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnSignals.Name = "pnSignals";
            this.pnSignals.Size = new System.Drawing.Size(344, 208);
            this.pnSignals.TabIndex = 1;
            // 
            // tsslbLog
            // 
            this.tsslbLog.ForeColor = System.Drawing.Color.Red;
            this.tsslbLog.Name = "tsslbLog";
            this.tsslbLog.Size = new System.Drawing.Size(131, 17);
            this.tsslbLog.Text = "toolStripStatusLabel1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.plSend);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(344, 144);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Controls";
            // 
            // RollingCounterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 457);
            this.Margin = new System.Windows.Forms.Padding(56, 34, 56, 34);
            this.Name = "RollingCounterForm";
            this.Style = MetroFramework.MetroColorStyle.Green;
            this.Text = "RollingCounterForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RollingCounterForm_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.plSend.ResumeLayout(false);
            this.plSend.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCoe)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnSignals;
        private System.Windows.Forms.Button btnAutoSend;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel plSend;
        private System.Windows.Forms.Button btnSetValue;
        private System.Windows.Forms.Button btnDivision;
        private System.Windows.Forms.Button btnMultip;
        private System.Windows.Forms.Button btnReduce;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.NumericUpDown nudCoe;
        private System.Windows.Forms.TextBox tbCurrent;
        private System.Windows.Forms.ComboBox cbbSignals;
        private System.Windows.Forms.Label lbCoe;
        private System.Windows.Forms.ToolStripStatusLabel tsslbLog;
        private MetroFramework.Controls.MetroComboBox metroComboBox_Signals;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroTextBox metroTextBox_CurVal;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}