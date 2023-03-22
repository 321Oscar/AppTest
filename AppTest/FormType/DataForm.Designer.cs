
namespace AppTest.FormType
{
    partial class DataForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dBCSignalBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nudTimerInterval = new System.Windows.Forms.NumericUpDown();
            this.btnGet = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.gbRlcntControls = new System.Windows.Forms.GroupBox();
            this.lbTips = new System.Windows.Forms.Label();
            this.btnAutoSend = new System.Windows.Forms.Button();
            this.metroTabControl1 = new MetroFramework.Controls.MetroTabControl();
            this.metroTabPage1 = new MetroFramework.Controls.MetroTabPage();
            this.metroTabPage2 = new MetroFramework.Controls.MetroTabPage();
            this.gbSetControls = new System.Windows.Forms.GroupBox();
            this.metroComboBox_Signal = new MetroFramework.Controls.MetroComboBox();
            this.btnSet = new System.Windows.Forms.Button();
            this.tbCurrent = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDivision = new System.Windows.Forms.Button();
            this.btnMultip = new System.Windows.Forms.Button();
            this.btnReduce = new System.Windows.Forms.Button();
            this.nudStep = new System.Windows.Forms.NumericUpDown();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.gbGetControls = new System.Windows.Forms.GroupBox();
            this.cbbSignals = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.messageIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.signalNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.strValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dBCSignalBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimerInterval)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.gbRlcntControls.SuspendLayout();
            this.metroTabControl1.SuspendLayout();
            this.metroTabPage1.SuspendLayout();
            this.gbSetControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStep)).BeginInit();
            this.gbGetControls.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.messageIDDataGridViewTextBoxColumn,
            this.customNameDataGridViewTextBoxColumn,
            this.signalNameDataGridViewTextBoxColumn,
            this.strValueDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.dBCSignalBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(361, 154);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            // 
            // dBCSignalBindingSource
            // 
            this.dBCSignalBindingSource.DataSource = typeof(AppTest.ProjectClass.DBCSignal);
            // 
            // nudTimerInterval
            // 
            this.nudTimerInterval.Location = new System.Drawing.Point(89, 29);
            this.nudTimerInterval.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.nudTimerInterval.Name = "nudTimerInterval";
            this.nudTimerInterval.Size = new System.Drawing.Size(55, 23);
            this.nudTimerInterval.TabIndex = 9;
            this.nudTimerInterval.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudTimerInterval.Visible = false;
            // 
            // btnGet
            // 
            this.btnGet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            this.btnGet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGet.ForeColor = System.Drawing.Color.White;
            this.btnGet.Location = new System.Drawing.Point(7, 23);
            this.btnGet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(76, 33);
            this.btnGet.TabIndex = 10;
            this.btnGet.Text = "启动";
            this.btnGet.UseVisualStyleBackColor = false;
            this.btnGet.Click += new System.EventHandler(this.btnDataControl_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.gbRlcntControls, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.metroTabControl1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.gbSetControls, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.gbGetControls, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(375, 448);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // gbRlcntControls
            // 
            this.gbRlcntControls.Controls.Add(this.lbTips);
            this.gbRlcntControls.Controls.Add(this.btnAutoSend);
            this.gbRlcntControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbRlcntControls.Location = new System.Drawing.Point(3, 3);
            this.gbRlcntControls.Name = "gbRlcntControls";
            this.gbRlcntControls.Size = new System.Drawing.Size(369, 55);
            this.gbRlcntControls.TabIndex = 13;
            this.gbRlcntControls.TabStop = false;
            this.gbRlcntControls.Text = "RlcntControls";
            // 
            // lbTips
            // 
            this.lbTips.AutoSize = true;
            this.lbTips.Cursor = System.Windows.Forms.Cursors.Help;
            this.lbTips.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lbTips.Location = new System.Drawing.Point(325, 25);
            this.lbTips.Name = "lbTips";
            this.lbTips.Size = new System.Drawing.Size(14, 17);
            this.lbTips.TabIndex = 14;
            this.lbTips.Text = "?";
            // 
            // btnAutoSend
            // 
            this.btnAutoSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAutoSend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(177)))), ((int)(((byte)(89)))));
            this.btnAutoSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoSend.ForeColor = System.Drawing.Color.White;
            this.btnAutoSend.Location = new System.Drawing.Point(9, 18);
            this.btnAutoSend.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAutoSend.Name = "btnAutoSend";
            this.btnAutoSend.Size = new System.Drawing.Size(76, 33);
            this.btnAutoSend.TabIndex = 0;
            this.btnAutoSend.Text = "停止";
            this.btnAutoSend.UseVisualStyleBackColor = false;
            this.btnAutoSend.Click += new System.EventHandler(this.btnAutoSend_Click);
            // 
            // metroTabControl1
            // 
            this.metroTabControl1.Controls.Add(this.metroTabPage1);
            this.metroTabControl1.Controls.Add(this.metroTabPage2);
            this.metroTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabControl1.Location = new System.Drawing.Point(3, 245);
            this.metroTabControl1.Name = "metroTabControl1";
            this.metroTabControl1.SelectedIndex = 0;
            this.metroTabControl1.Size = new System.Drawing.Size(369, 200);
            this.metroTabControl1.TabIndex = 4;
            this.metroTabControl1.UseSelectable = true;
            // 
            // metroTabPage1
            // 
            this.metroTabPage1.Controls.Add(this.dataGridView1);
            this.metroTabPage1.HorizontalScrollbarBarColor = true;
            this.metroTabPage1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.HorizontalScrollbarSize = 7;
            this.metroTabPage1.Location = new System.Drawing.Point(4, 38);
            this.metroTabPage1.Name = "metroTabPage1";
            this.metroTabPage1.Size = new System.Drawing.Size(361, 158);
            this.metroTabPage1.TabIndex = 0;
            this.metroTabPage1.Text = "Signals";
            this.metroTabPage1.VerticalScrollbarBarColor = true;
            this.metroTabPage1.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.VerticalScrollbarSize = 9;
            // 
            // metroTabPage2
            // 
            this.metroTabPage2.HorizontalScrollbarBarColor = true;
            this.metroTabPage2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage2.HorizontalScrollbarSize = 7;
            this.metroTabPage2.Location = new System.Drawing.Point(4, 38);
            this.metroTabPage2.Name = "metroTabPage2";
            this.metroTabPage2.Size = new System.Drawing.Size(447, 158);
            this.metroTabPage2.TabIndex = 1;
            this.metroTabPage2.Text = "HistoryData";
            this.metroTabPage2.VerticalScrollbarBarColor = true;
            this.metroTabPage2.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage2.VerticalScrollbarSize = 9;
            // 
            // gbSetControls
            // 
            this.gbSetControls.Controls.Add(this.metroComboBox_Signal);
            this.gbSetControls.Controls.Add(this.btnSet);
            this.gbSetControls.Controls.Add(this.tbCurrent);
            this.gbSetControls.Controls.Add(this.label3);
            this.gbSetControls.Controls.Add(this.label2);
            this.gbSetControls.Controls.Add(this.btnDivision);
            this.gbSetControls.Controls.Add(this.btnMultip);
            this.gbSetControls.Controls.Add(this.btnReduce);
            this.gbSetControls.Controls.Add(this.nudStep);
            this.gbSetControls.Controls.Add(this.btnAdd);
            this.gbSetControls.Controls.Add(this.label4);
            this.gbSetControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSetControls.Location = new System.Drawing.Point(3, 64);
            this.gbSetControls.Name = "gbSetControls";
            this.gbSetControls.Size = new System.Drawing.Size(369, 101);
            this.gbSetControls.TabIndex = 15;
            this.gbSetControls.TabStop = false;
            this.gbSetControls.Text = "SetControls";
            // 
            // metroComboBox_Signal
            // 
            this.metroComboBox_Signal.FormattingEnabled = true;
            this.metroComboBox_Signal.ItemHeight = 23;
            this.metroComboBox_Signal.Location = new System.Drawing.Point(41, 22);
            this.metroComboBox_Signal.Name = "metroComboBox_Signal";
            this.metroComboBox_Signal.Size = new System.Drawing.Size(156, 29);
            this.metroComboBox_Signal.TabIndex = 18;
            this.metroComboBox_Signal.UseSelectable = true;
            this.metroComboBox_Signal.SelectedIndexChanged += new System.EventHandler(this.cbbSignals_SelectedIndexChanged);
            // 
            // btnSet
            // 
            this.btnSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(119)))), ((int)(((byte)(53)))));
            this.btnSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSet.ForeColor = System.Drawing.Color.White;
            this.btnSet.Location = new System.Drawing.Point(204, 59);
            this.btnSet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(56, 33);
            this.btnSet.TabIndex = 8;
            this.btnSet.Text = "Send";
            this.btnSet.UseVisualStyleBackColor = false;
            this.btnSet.Click += new System.EventHandler(this.btnSetOrSend_Click);
            // 
            // tbCurrent
            // 
            this.tbCurrent.Location = new System.Drawing.Point(278, 23);
            this.tbCurrent.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.tbCurrent.Name = "tbCurrent";
            this.tbCurrent.Size = new System.Drawing.Size(61, 23);
            this.tbCurrent.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(227, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 17);
            this.label3.TabIndex = 16;
            this.label3.Text = "当前值";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 17);
            this.label2.TabIndex = 15;
            this.label2.Text = "信号";
            // 
            // btnDivision
            // 
            this.btnDivision.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDivision.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(119)))), ((int)(((byte)(53)))));
            this.btnDivision.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDivision.ForeColor = System.Drawing.Color.White;
            this.btnDivision.Location = new System.Drawing.Point(317, 59);
            this.btnDivision.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDivision.Name = "btnDivision";
            this.btnDivision.Size = new System.Drawing.Size(31, 33);
            this.btnDivision.TabIndex = 7;
            this.btnDivision.Text = "除";
            this.btnDivision.UseVisualStyleBackColor = false;
            this.btnDivision.Visible = false;
            this.btnDivision.Click += new System.EventHandler(this.btnChangeValue_Click);
            // 
            // btnMultip
            // 
            this.btnMultip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMultip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(119)))), ((int)(((byte)(53)))));
            this.btnMultip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMultip.ForeColor = System.Drawing.Color.White;
            this.btnMultip.Location = new System.Drawing.Point(278, 59);
            this.btnMultip.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnMultip.Name = "btnMultip";
            this.btnMultip.Size = new System.Drawing.Size(31, 33);
            this.btnMultip.TabIndex = 6;
            this.btnMultip.Text = "乘";
            this.btnMultip.UseVisualStyleBackColor = false;
            this.btnMultip.Visible = false;
            this.btnMultip.Click += new System.EventHandler(this.btnChangeValue_Click);
            // 
            // btnReduce
            // 
            this.btnReduce.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReduce.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(119)))), ((int)(((byte)(53)))));
            this.btnReduce.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReduce.ForeColor = System.Drawing.Color.White;
            this.btnReduce.Location = new System.Drawing.Point(166, 59);
            this.btnReduce.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnReduce.Name = "btnReduce";
            this.btnReduce.Size = new System.Drawing.Size(31, 33);
            this.btnReduce.TabIndex = 5;
            this.btnReduce.Text = "减";
            this.btnReduce.UseVisualStyleBackColor = false;
            this.btnReduce.Click += new System.EventHandler(this.btnChangeValue_Click);
            // 
            // nudStep
            // 
            this.nudStep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudStep.Location = new System.Drawing.Point(42, 65);
            this.nudStep.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.nudStep.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudStep.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.nudStep.Name = "nudStep";
            this.nudStep.Size = new System.Drawing.Size(68, 23);
            this.nudStep.TabIndex = 3;
            this.nudStep.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(119)))), ((int)(((byte)(53)))));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(127, 59);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(31, 33);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "加";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnChangeValue_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 17);
            this.label4.TabIndex = 17;
            this.label4.Text = "步长";
            // 
            // gbGetControls
            // 
            this.gbGetControls.Controls.Add(this.nudTimerInterval);
            this.gbGetControls.Controls.Add(this.btnGet);
            this.gbGetControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbGetControls.Location = new System.Drawing.Point(3, 171);
            this.gbGetControls.Name = "gbGetControls";
            this.gbGetControls.Size = new System.Drawing.Size(369, 68);
            this.gbGetControls.TabIndex = 16;
            this.gbGetControls.TabStop = false;
            this.gbGetControls.Text = "GetControls";
            // 
            // cbbSignals
            // 
            this.cbbSignals.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbbSignals.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbbSignals.FormattingEnabled = true;
            this.cbbSignals.Location = new System.Drawing.Point(402, 278);
            this.cbbSignals.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbbSignals.Name = "cbbSignals";
            this.cbbSignals.Size = new System.Drawing.Size(118, 25);
            this.cbbSignals.TabIndex = 1;
            this.cbbSignals.Visible = false;
            this.cbbSignals.SelectedIndexChanged += new System.EventHandler(this.cbbSignals_SelectedIndexChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(400, 84);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(371, 115);
            this.tabControl1.TabIndex = 11;
            this.tabControl1.Visible = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(363, 85);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Signals";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(363, 85);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "HistoryData";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(102, 36);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "ms";
            // 
            // messageIDDataGridViewTextBoxColumn
            // 
            this.messageIDDataGridViewTextBoxColumn.DataPropertyName = "MessageID";
            this.messageIDDataGridViewTextBoxColumn.HeaderText = "MessageID";
            this.messageIDDataGridViewTextBoxColumn.Name = "messageIDDataGridViewTextBoxColumn";
            this.messageIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // customNameDataGridViewTextBoxColumn
            // 
            this.customNameDataGridViewTextBoxColumn.DataPropertyName = "CustomName";
            this.customNameDataGridViewTextBoxColumn.HeaderText = "CustomName";
            this.customNameDataGridViewTextBoxColumn.Name = "customNameDataGridViewTextBoxColumn";
            this.customNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // signalNameDataGridViewTextBoxColumn
            // 
            this.signalNameDataGridViewTextBoxColumn.DataPropertyName = "SignalName";
            this.signalNameDataGridViewTextBoxColumn.HeaderText = "SignalName";
            this.signalNameDataGridViewTextBoxColumn.Name = "signalNameDataGridViewTextBoxColumn";
            this.signalNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // strValueDataGridViewTextBoxColumn
            // 
            this.strValueDataGridViewTextBoxColumn.DataPropertyName = "StrValue";
            this.strValueDataGridViewTextBoxColumn.HeaderText = "StrValue";
            this.strValueDataGridViewTextBoxColumn.Name = "strValueDataGridViewTextBoxColumn";
            // 
            // DataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(415, 538);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cbbSignals);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(1980128, 34696, 1980128, 34696);
            this.Name = "DataForm";
            this.Padding = new System.Windows.Forms.Padding(17, 60, 17, 0);
            this.Text = "DataForm";
            this.Load += new System.EventHandler(this.DataForm_Load);
            this.Controls.SetChildIndex(this.cbbSignals, 0);
            this.Controls.SetChildIndex(this.tabControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dBCSignalBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimerInterval)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.gbRlcntControls.ResumeLayout(false);
            this.gbRlcntControls.PerformLayout();
            this.metroTabControl1.ResumeLayout(false);
            this.metroTabPage1.ResumeLayout(false);
            this.gbSetControls.ResumeLayout(false);
            this.gbSetControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStep)).EndInit();
            this.gbGetControls.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.NumericUpDown nudTimerInterval;
        private System.Windows.Forms.Button btnGet;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox gbGetControls;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox gbSetControls;
        private System.Windows.Forms.GroupBox gbRlcntControls;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.TextBox tbCurrent;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbbSignals;
        private System.Windows.Forms.Button btnDivision;
        private System.Windows.Forms.Button btnMultip;
        private System.Windows.Forms.Button btnReduce;
        private System.Windows.Forms.NumericUpDown nudStep;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnAutoSend;
        private System.Windows.Forms.Label lbTips;
        private MetroFramework.Controls.MetroTabControl metroTabControl1;
        private MetroFramework.Controls.MetroTabPage metroTabPage1;
        private MetroFramework.Controls.MetroTabPage metroTabPage2;
        private MetroFramework.Controls.MetroComboBox metroComboBox_Signal;
        private System.Windows.Forms.BindingSource dBCSignalBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn messageIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn signalNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn strValueDataGridViewTextBoxColumn;
    }
}