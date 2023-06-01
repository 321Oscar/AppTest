
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.WhetherSendOrGet = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.messageIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.signalNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.strValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnStep = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnAdd = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ColumnReduce = new System.Windows.Forms.DataGridViewButtonColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dBCSignalBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.dBCSignalBindingSource = new System.Windows.Forms.BindingSource(this.components);
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
            this.nudTimerInterval = new System.Windows.Forms.NumericUpDown();
            this.btnGet = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dBCSignalBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dBCSignalBindingSource)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.gbRlcntControls.SuspendLayout();
            this.metroTabControl1.SuspendLayout();
            this.metroTabPage1.SuspendLayout();
            this.gbSetControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStep)).BeginInit();
            this.gbGetControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimerInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.tableLayoutPanel1);
            this.panelContent.Location = new System.Drawing.Point(17, 60);
            this.panelContent.Size = new System.Drawing.Size(475, 458);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.WhetherSendOrGet,
            this.messageIDDataGridViewTextBoxColumn,
            this.customNameDataGridViewTextBoxColumn,
            this.signalNameDataGridViewTextBoxColumn,
            this.strValueDataGridViewTextBoxColumn,
            this.columnStep,
            this.ColumnAdd,
            this.ColumnReduce});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.DataSource = this.dBCSignalBindingSource1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 25;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(461, 166);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            this.dataGridView1.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridView1_CurrentCellDirtyStateChanged);
            // 
            // WhetherSendOrGet
            // 
            this.WhetherSendOrGet.DataPropertyName = "WhetherSendOrGet";
            this.WhetherSendOrGet.HeaderText = "参与收发";
            this.WhetherSendOrGet.MinimumWidth = 8;
            this.WhetherSendOrGet.Name = "WhetherSendOrGet";
            this.WhetherSendOrGet.Visible = false;
            // 
            // messageIDDataGridViewTextBoxColumn
            // 
            this.messageIDDataGridViewTextBoxColumn.DataPropertyName = "MessageID";
            this.messageIDDataGridViewTextBoxColumn.HeaderText = "MessageID";
            this.messageIDDataGridViewTextBoxColumn.MinimumWidth = 8;
            this.messageIDDataGridViewTextBoxColumn.Name = "messageIDDataGridViewTextBoxColumn";
            this.messageIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.messageIDDataGridViewTextBoxColumn.Visible = false;
            // 
            // customNameDataGridViewTextBoxColumn
            // 
            this.customNameDataGridViewTextBoxColumn.DataPropertyName = "CustomName";
            this.customNameDataGridViewTextBoxColumn.HeaderText = "CustomName";
            this.customNameDataGridViewTextBoxColumn.MinimumWidth = 8;
            this.customNameDataGridViewTextBoxColumn.Name = "customNameDataGridViewTextBoxColumn";
            this.customNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // signalNameDataGridViewTextBoxColumn
            // 
            this.signalNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.signalNameDataGridViewTextBoxColumn.DataPropertyName = "SignalName";
            this.signalNameDataGridViewTextBoxColumn.HeaderText = "SignalName";
            this.signalNameDataGridViewTextBoxColumn.MinimumWidth = 8;
            this.signalNameDataGridViewTextBoxColumn.Name = "signalNameDataGridViewTextBoxColumn";
            this.signalNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.signalNameDataGridViewTextBoxColumn.Width = 103;
            // 
            // strValueDataGridViewTextBoxColumn
            // 
            this.strValueDataGridViewTextBoxColumn.DataPropertyName = "StrValue";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.strValueDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.strValueDataGridViewTextBoxColumn.HeaderText = "StrValue";
            this.strValueDataGridViewTextBoxColumn.MinimumWidth = 8;
            this.strValueDataGridViewTextBoxColumn.Name = "strValueDataGridViewTextBoxColumn";
            // 
            // columnStep
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = "1";
            this.columnStep.DefaultCellStyle = dataGridViewCellStyle2;
            this.columnStep.HeaderText = "Step";
            this.columnStep.Name = "columnStep";
            this.columnStep.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnStep.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.columnStep.ToolTipText = "步长";
            // 
            // ColumnAdd
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = "加";
            this.ColumnAdd.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColumnAdd.HeaderText = "Add";
            this.ColumnAdd.Name = "ColumnAdd";
            this.ColumnAdd.Text = "Add";
            this.ColumnAdd.ToolTipText = "增加";
            // 
            // ColumnReduce
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.NullValue = "减";
            this.ColumnReduce.DefaultCellStyle = dataGridViewCellStyle4;
            this.ColumnReduce.HeaderText = "Reduce";
            this.ColumnReduce.Name = "ColumnReduce";
            this.ColumnReduce.Text = "Reduce";
            this.ColumnReduce.ToolTipText = "减少";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // dBCSignalBindingSource1
            // 
            this.dBCSignalBindingSource1.DataSource = typeof(AppTest.Model.DBCSignal);
            // 
            // dBCSignalBindingSource
            // 
            this.dBCSignalBindingSource.DataSource = typeof(AppTest.Model.DBCSignal);
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(475, 458);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // gbRlcntControls
            // 
            this.gbRlcntControls.Controls.Add(this.lbTips);
            this.gbRlcntControls.Controls.Add(this.btnAutoSend);
            this.gbRlcntControls.Location = new System.Drawing.Point(3, 3);
            this.gbRlcntControls.Name = "gbRlcntControls";
            this.gbRlcntControls.Size = new System.Drawing.Size(469, 58);
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
            this.btnAutoSend.Location = new System.Drawing.Point(9, 21);
            this.btnAutoSend.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAutoSend.Name = "btnAutoSend";
            this.btnAutoSend.Size = new System.Drawing.Size(76, 33);
            this.btnAutoSend.TabIndex = 0;
            this.btnAutoSend.Text = "停止";
            this.btnAutoSend.UseVisualStyleBackColor = false;
            // 
            // metroTabControl1
            // 
            this.metroTabControl1.Controls.Add(this.metroTabPage1);
            this.metroTabControl1.Controls.Add(this.metroTabPage2);
            this.metroTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabControl1.Location = new System.Drawing.Point(3, 247);
            this.metroTabControl1.Name = "metroTabControl1";
            this.metroTabControl1.SelectedIndex = 0;
            this.metroTabControl1.Size = new System.Drawing.Size(469, 208);
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
            this.metroTabPage1.Size = new System.Drawing.Size(461, 166);
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
            this.metroTabPage2.Size = new System.Drawing.Size(696, 262);
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
            this.gbSetControls.Location = new System.Drawing.Point(3, 67);
            this.gbSetControls.Name = "gbSetControls";
            this.gbSetControls.Size = new System.Drawing.Size(469, 100);
            this.gbSetControls.TabIndex = 15;
            this.gbSetControls.TabStop = false;
            this.gbSetControls.Text = "SetControls";
            // 
            // metroComboBox_Signal
            // 
            this.metroComboBox_Signal.DropDownWidth = 156;
            this.metroComboBox_Signal.FormattingEnabled = true;
            this.metroComboBox_Signal.ItemHeight = 23;
            this.metroComboBox_Signal.Location = new System.Drawing.Point(41, 22);
            this.metroComboBox_Signal.Name = "metroComboBox_Signal";
            this.metroComboBox_Signal.Size = new System.Drawing.Size(156, 29);
            this.metroComboBox_Signal.TabIndex = 18;
            this.metroComboBox_Signal.UseSelectable = true;
            this.metroComboBox_Signal.DropDown += new System.EventHandler(this.metroComboBox_Signal_DropDown);
            this.metroComboBox_Signal.SelectedIndexChanged += new System.EventHandler(this.cbbSignals_SelectedIndexChanged);
            // 
            // btnSet
            // 
            this.btnSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(119)))), ((int)(((byte)(53)))));
            this.btnSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSet.ForeColor = System.Drawing.Color.White;
            this.btnSet.Location = new System.Drawing.Point(204, 58);
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
            this.label3.Location = new System.Drawing.Point(227, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 17);
            this.label3.TabIndex = 16;
            this.label3.Text = "当前值";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 24);
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
            this.btnDivision.Location = new System.Drawing.Point(317, 58);
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
            this.btnMultip.Location = new System.Drawing.Point(278, 58);
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
            this.btnReduce.Location = new System.Drawing.Point(166, 58);
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
            this.nudStep.Location = new System.Drawing.Point(42, 64);
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
            this.btnAdd.Location = new System.Drawing.Point(127, 58);
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
            this.label4.Location = new System.Drawing.Point(3, 66);
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
            this.gbGetControls.Location = new System.Drawing.Point(3, 173);
            this.gbGetControls.Name = "gbGetControls";
            this.gbGetControls.Size = new System.Drawing.Size(469, 68);
            this.gbGetControls.TabIndex = 16;
            this.gbGetControls.TabStop = false;
            this.gbGetControls.Text = "GetControls";
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
            // DataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(509, 541);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(1980128, 34696, 1980128, 34696);
            this.Name = "DataForm";
            this.Padding = new System.Windows.Forms.Padding(17, 60, 17, 0);
            this.Text = "DataForm";
            this.Load += new System.EventHandler(this.DataForm_Load);
            this.Controls.SetChildIndex(this.panelContent, 0);
            this.panelContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dBCSignalBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dBCSignalBindingSource)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.gbRlcntControls.ResumeLayout(false);
            this.gbRlcntControls.PerformLayout();
            this.metroTabControl1.ResumeLayout(false);
            this.metroTabPage1.ResumeLayout(false);
            this.gbSetControls.ResumeLayout(false);
            this.gbSetControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStep)).EndInit();
            this.gbGetControls.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudTimerInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.DataGridView dataGridView1;
        protected System.Windows.Forms.NumericUpDown nudTimerInterval;
        protected System.Windows.Forms.Button btnGet;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox gbGetControls;
        private System.Windows.Forms.GroupBox gbSetControls;
        private System.Windows.Forms.GroupBox gbRlcntControls;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSet;
        protected System.Windows.Forms.TextBox tbCurrent;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        protected System.Windows.Forms.Button btnDivision;
        protected System.Windows.Forms.Button btnMultip;
        protected System.Windows.Forms.Button btnReduce;
        protected System.Windows.Forms.NumericUpDown nudStep;
        protected System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnAutoSend;
        private System.Windows.Forms.Label lbTips;
        private MetroFramework.Controls.MetroTabControl metroTabControl1;
        protected MetroFramework.Controls.MetroTabPage metroTabPage1;
        protected MetroFramework.Controls.MetroTabPage metroTabPage2;
        protected MetroFramework.Controls.MetroComboBox metroComboBox_Signal;
        private System.Windows.Forms.BindingSource dBCSignalBindingSource;
        private System.Windows.Forms.BindingSource dBCSignalBindingSource1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn WhetherSendOrGet;
        private System.Windows.Forms.DataGridViewTextBoxColumn messageIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn signalNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn strValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnStep;
        private System.Windows.Forms.DataGridViewButtonColumn ColumnAdd;
        private System.Windows.Forms.DataGridViewButtonColumn ColumnReduce;
    }
}