
namespace AppTest.FormType
{
    partial class XCPDAQGetForm
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
            this.metroTabControl1 = new MetroFramework.Controls.MetroTabControl();
            this.metroTabPage1 = new MetroFramework.Controls.MetroTabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.customNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.signalNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.strValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.messageIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startBitDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.byteOrderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.factorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.offsetDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cycleTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unitDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.commentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.minimumDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maximumDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lengthDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.byteOrderintDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.whetherSendOrGetDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dBCSignalBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.metroTabPage2 = new MetroFramework.Controls.MetroTabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.metroButtonStart = new MetroFramework.Controls.MetroButton();
            this.panelContent.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.metroTabControl1.SuspendLayout();
            this.metroTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dBCSignalBindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.tableLayoutPanel1);
            this.panelContent.Size = new System.Drawing.Size(608, 442);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.metroTabControl1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(608, 442);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // metroTabControl1
            // 
            this.metroTabControl1.Controls.Add(this.metroTabPage1);
            this.metroTabControl1.Controls.Add(this.metroTabPage2);
            this.metroTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabControl1.Location = new System.Drawing.Point(3, 103);
            this.metroTabControl1.Name = "metroTabControl1";
            this.metroTabControl1.SelectedIndex = 0;
            this.metroTabControl1.Size = new System.Drawing.Size(602, 336);
            this.metroTabControl1.TabIndex = 2;
            this.metroTabControl1.UseSelectable = true;
            // 
            // metroTabPage1
            // 
            this.metroTabPage1.Controls.Add(this.dataGridView1);
            this.metroTabPage1.HorizontalScrollbarBarColor = true;
            this.metroTabPage1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.HorizontalScrollbarSize = 10;
            this.metroTabPage1.Location = new System.Drawing.Point(4, 38);
            this.metroTabPage1.Name = "metroTabPage1";
            this.metroTabPage1.Size = new System.Drawing.Size(594, 294);
            this.metroTabPage1.TabIndex = 0;
            this.metroTabPage1.Text = "实时数据";
            this.metroTabPage1.VerticalScrollbarBarColor = true;
            this.metroTabPage1.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.VerticalScrollbarSize = 10;
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
            this.customNameDataGridViewTextBoxColumn,
            this.signalNameDataGridViewTextBoxColumn,
            this.strValueDataGridViewTextBoxColumn,
            this.messageIDDataGridViewTextBoxColumn,
            this.startBitDataGridViewTextBoxColumn,
            this.byteOrderDataGridViewTextBoxColumn,
            this.valueTypeDataGridViewTextBoxColumn,
            this.factorDataGridViewTextBoxColumn,
            this.offsetDataGridViewTextBoxColumn,
            this.cycleTimeDataGridViewTextBoxColumn,
            this.unitDataGridViewTextBoxColumn,
            this.commentDataGridViewTextBoxColumn,
            this.minimumDataGridViewTextBoxColumn,
            this.maximumDataGridViewTextBoxColumn,
            this.lengthDataGridViewTextBoxColumn,
            this.byteOrderintDataGridViewTextBoxColumn,
            this.whetherSendOrGetDataGridViewCheckBoxColumn});
            this.dataGridView1.DataSource = this.dBCSignalBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(-4, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(598, 317);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
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
            this.strValueDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // messageIDDataGridViewTextBoxColumn
            // 
            this.messageIDDataGridViewTextBoxColumn.DataPropertyName = "MessageID";
            this.messageIDDataGridViewTextBoxColumn.HeaderText = "MessageID";
            this.messageIDDataGridViewTextBoxColumn.Name = "messageIDDataGridViewTextBoxColumn";
            this.messageIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.messageIDDataGridViewTextBoxColumn.Visible = false;
            // 
            // startBitDataGridViewTextBoxColumn
            // 
            this.startBitDataGridViewTextBoxColumn.DataPropertyName = "StartBit";
            this.startBitDataGridViewTextBoxColumn.HeaderText = "StartBit";
            this.startBitDataGridViewTextBoxColumn.Name = "startBitDataGridViewTextBoxColumn";
            this.startBitDataGridViewTextBoxColumn.ReadOnly = true;
            this.startBitDataGridViewTextBoxColumn.Visible = false;
            // 
            // byteOrderDataGridViewTextBoxColumn
            // 
            this.byteOrderDataGridViewTextBoxColumn.DataPropertyName = "ByteOrder";
            this.byteOrderDataGridViewTextBoxColumn.HeaderText = "ByteOrder";
            this.byteOrderDataGridViewTextBoxColumn.Name = "byteOrderDataGridViewTextBoxColumn";
            this.byteOrderDataGridViewTextBoxColumn.ReadOnly = true;
            this.byteOrderDataGridViewTextBoxColumn.Visible = false;
            // 
            // valueTypeDataGridViewTextBoxColumn
            // 
            this.valueTypeDataGridViewTextBoxColumn.DataPropertyName = "ValueType";
            this.valueTypeDataGridViewTextBoxColumn.HeaderText = "ValueType";
            this.valueTypeDataGridViewTextBoxColumn.Name = "valueTypeDataGridViewTextBoxColumn";
            this.valueTypeDataGridViewTextBoxColumn.ReadOnly = true;
            this.valueTypeDataGridViewTextBoxColumn.Visible = false;
            // 
            // factorDataGridViewTextBoxColumn
            // 
            this.factorDataGridViewTextBoxColumn.DataPropertyName = "Factor";
            this.factorDataGridViewTextBoxColumn.HeaderText = "Factor";
            this.factorDataGridViewTextBoxColumn.Name = "factorDataGridViewTextBoxColumn";
            this.factorDataGridViewTextBoxColumn.ReadOnly = true;
            this.factorDataGridViewTextBoxColumn.Visible = false;
            // 
            // offsetDataGridViewTextBoxColumn
            // 
            this.offsetDataGridViewTextBoxColumn.DataPropertyName = "Offset";
            this.offsetDataGridViewTextBoxColumn.HeaderText = "Offset";
            this.offsetDataGridViewTextBoxColumn.Name = "offsetDataGridViewTextBoxColumn";
            this.offsetDataGridViewTextBoxColumn.ReadOnly = true;
            this.offsetDataGridViewTextBoxColumn.Visible = false;
            // 
            // cycleTimeDataGridViewTextBoxColumn
            // 
            this.cycleTimeDataGridViewTextBoxColumn.DataPropertyName = "CycleTime";
            this.cycleTimeDataGridViewTextBoxColumn.HeaderText = "CycleTime";
            this.cycleTimeDataGridViewTextBoxColumn.Name = "cycleTimeDataGridViewTextBoxColumn";
            this.cycleTimeDataGridViewTextBoxColumn.ReadOnly = true;
            this.cycleTimeDataGridViewTextBoxColumn.Visible = false;
            // 
            // unitDataGridViewTextBoxColumn
            // 
            this.unitDataGridViewTextBoxColumn.DataPropertyName = "Unit";
            this.unitDataGridViewTextBoxColumn.HeaderText = "Unit";
            this.unitDataGridViewTextBoxColumn.Name = "unitDataGridViewTextBoxColumn";
            this.unitDataGridViewTextBoxColumn.ReadOnly = true;
            this.unitDataGridViewTextBoxColumn.Visible = false;
            // 
            // commentDataGridViewTextBoxColumn
            // 
            this.commentDataGridViewTextBoxColumn.DataPropertyName = "Comment";
            this.commentDataGridViewTextBoxColumn.HeaderText = "Comment";
            this.commentDataGridViewTextBoxColumn.Name = "commentDataGridViewTextBoxColumn";
            this.commentDataGridViewTextBoxColumn.ReadOnly = true;
            this.commentDataGridViewTextBoxColumn.Visible = false;
            // 
            // minimumDataGridViewTextBoxColumn
            // 
            this.minimumDataGridViewTextBoxColumn.DataPropertyName = "Minimum";
            this.minimumDataGridViewTextBoxColumn.HeaderText = "Minimum";
            this.minimumDataGridViewTextBoxColumn.Name = "minimumDataGridViewTextBoxColumn";
            this.minimumDataGridViewTextBoxColumn.ReadOnly = true;
            this.minimumDataGridViewTextBoxColumn.Visible = false;
            // 
            // maximumDataGridViewTextBoxColumn
            // 
            this.maximumDataGridViewTextBoxColumn.DataPropertyName = "Maximum";
            this.maximumDataGridViewTextBoxColumn.HeaderText = "Maximum";
            this.maximumDataGridViewTextBoxColumn.Name = "maximumDataGridViewTextBoxColumn";
            this.maximumDataGridViewTextBoxColumn.ReadOnly = true;
            this.maximumDataGridViewTextBoxColumn.Visible = false;
            // 
            // lengthDataGridViewTextBoxColumn
            // 
            this.lengthDataGridViewTextBoxColumn.DataPropertyName = "Length";
            this.lengthDataGridViewTextBoxColumn.HeaderText = "Length";
            this.lengthDataGridViewTextBoxColumn.Name = "lengthDataGridViewTextBoxColumn";
            this.lengthDataGridViewTextBoxColumn.ReadOnly = true;
            this.lengthDataGridViewTextBoxColumn.Visible = false;
            // 
            // byteOrderintDataGridViewTextBoxColumn
            // 
            this.byteOrderintDataGridViewTextBoxColumn.DataPropertyName = "ByteOrder_int";
            this.byteOrderintDataGridViewTextBoxColumn.HeaderText = "ByteOrder_int";
            this.byteOrderintDataGridViewTextBoxColumn.Name = "byteOrderintDataGridViewTextBoxColumn";
            this.byteOrderintDataGridViewTextBoxColumn.ReadOnly = true;
            this.byteOrderintDataGridViewTextBoxColumn.Visible = false;
            // 
            // whetherSendOrGetDataGridViewCheckBoxColumn
            // 
            this.whetherSendOrGetDataGridViewCheckBoxColumn.DataPropertyName = "WhetherSendOrGet";
            this.whetherSendOrGetDataGridViewCheckBoxColumn.HeaderText = "WhetherSendOrGet";
            this.whetherSendOrGetDataGridViewCheckBoxColumn.Name = "whetherSendOrGetDataGridViewCheckBoxColumn";
            this.whetherSendOrGetDataGridViewCheckBoxColumn.ReadOnly = true;
            this.whetherSendOrGetDataGridViewCheckBoxColumn.Visible = false;
            // 
            // dBCSignalBindingSource
            // 
            this.dBCSignalBindingSource.DataSource = typeof(AppTest.Model.DBCSignal);
            // 
            // metroTabPage2
            // 
            this.metroTabPage2.HorizontalScrollbarBarColor = true;
            this.metroTabPage2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage2.HorizontalScrollbarSize = 10;
            this.metroTabPage2.Location = new System.Drawing.Point(4, 38);
            this.metroTabPage2.Name = "metroTabPage2";
            this.metroTabPage2.Size = new System.Drawing.Size(696, 296);
            this.metroTabPage2.TabIndex = 1;
            this.metroTabPage2.Text = "历史数据";
            this.metroTabPage2.VerticalScrollbarBarColor = true;
            this.metroTabPage2.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage2.VerticalScrollbarSize = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.metroButtonStart);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(602, 94);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DAQ Control";
            // 
            // metroButtonStart
            // 
            this.metroButtonStart.Location = new System.Drawing.Point(6, 32);
            this.metroButtonStart.Name = "metroButtonStart";
            this.metroButtonStart.Size = new System.Drawing.Size(101, 39);
            this.metroButtonStart.TabIndex = 1;
            this.metroButtonStart.Text = "启动";
            this.metroButtonStart.UseSelectable = true;
            this.metroButtonStart.Click += new System.EventHandler(this.metroButtonStart_Click);
            // 
            // XCPDAQGetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 526);
            this.Name = "XCPDAQGetForm";
            this.Text = "XCPDAQForm";
            this.panelContent.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.metroTabControl1.ResumeLayout(false);
            this.metroTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dBCSignalBindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource dBCSignalBindingSource;
        private MetroFramework.Controls.MetroButton metroButtonStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn customNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn signalNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn strValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn messageIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn startBitDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn byteOrderDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn factorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn offsetDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cycleTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn unitDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn commentDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn minimumDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maximumDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lengthDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn byteOrderintDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn whetherSendOrGetDataGridViewCheckBoxColumn;
        private MetroFramework.Controls.MetroTabControl metroTabControl1;
        private MetroFramework.Controls.MetroTabPage metroTabPage1;
        private MetroFramework.Controls.MetroTabPage metroTabPage2;
    }
}