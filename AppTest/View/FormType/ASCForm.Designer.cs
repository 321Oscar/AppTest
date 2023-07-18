
namespace AppTest.FormType
{
    partial class ASCForm
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
            this.metroButton_Import = new MetroFramework.Controls.MetroButton();
            this.metroButton_DBCFile = new MetroFramework.Controls.MetroButton();
            this.tvAllNode = new System.Windows.Forms.TreeView();
            this.metroComboBoxDBCFiles = new MetroFramework.Controls.MetroComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.metroProgressBar1 = new MetroFramework.Controls.MetroProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.metroButton_Query = new MetroFramework.Controls.MetroButton();
            this.timestapDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reservedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sendOrRecDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reserved2DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cANMsgDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aSCSignalBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aSCSignalBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // metroButton_Import
            // 
            this.metroButton_Import.Location = new System.Drawing.Point(448, 12);
            this.metroButton_Import.Name = "metroButton_Import";
            this.metroButton_Import.Size = new System.Drawing.Size(94, 30);
            this.metroButton_Import.TabIndex = 0;
            this.metroButton_Import.Text = "ASC文件";
            this.metroButton_Import.UseSelectable = true;
            this.metroButton_Import.Click += new System.EventHandler(this.metroButton_Import_Click);
            // 
            // metroButton_DBCFile
            // 
            this.metroButton_DBCFile.Location = new System.Drawing.Point(245, 12);
            this.metroButton_DBCFile.Name = "metroButton_DBCFile";
            this.metroButton_DBCFile.Size = new System.Drawing.Size(94, 30);
            this.metroButton_DBCFile.TabIndex = 1;
            this.metroButton_DBCFile.Text = "DBC文件";
            this.metroButton_DBCFile.UseSelectable = true;
            this.metroButton_DBCFile.Click += new System.EventHandler(this.metroButton_DBCFile_Click);
            // 
            // tvAllNode
            // 
            this.tvAllNode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tvAllNode.CheckBoxes = true;
            this.tvAllNode.Location = new System.Drawing.Point(18, 84);
            this.tvAllNode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tvAllNode.Name = "tvAllNode";
            this.tvAllNode.Size = new System.Drawing.Size(220, 353);
            this.tvAllNode.TabIndex = 5;
            // 
            // metroComboBoxDBCFiles
            // 
            this.metroComboBoxDBCFiles.FormattingEnabled = true;
            this.metroComboBoxDBCFiles.ItemHeight = 23;
            this.metroComboBoxDBCFiles.Location = new System.Drawing.Point(19, 13);
            this.metroComboBoxDBCFiles.Name = "metroComboBoxDBCFiles";
            this.metroComboBoxDBCFiles.Size = new System.Drawing.Size(220, 29);
            this.metroComboBoxDBCFiles.TabIndex = 6;
            this.metroComboBoxDBCFiles.UseSelectable = true;
            this.metroComboBoxDBCFiles.SelectedIndexChanged += new System.EventHandler(this.metroComboBoxDBCFiles_SelectedIndexChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.timestapDataGridViewTextBoxColumn,
            this.reservedDataGridViewTextBoxColumn,
            this.sendOrRecDataGridViewTextBoxColumn,
            this.reserved2DataGridViewTextBoxColumn,
            this.cANMsgDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.aSCSignalBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(339, 84);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(665, 353);
            this.dataGridView1.TabIndex = 7;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // metroProgressBar1
            // 
            this.metroProgressBar1.Location = new System.Drawing.Point(549, 17);
            this.metroProgressBar1.Name = "metroProgressBar1";
            this.metroProgressBar1.Size = new System.Drawing.Size(195, 23);
            this.metroProgressBar1.TabIndex = 8;
            // 
            // metroButton_Query
            // 
            this.metroButton_Query.Location = new System.Drawing.Point(241, 208);
            this.metroButton_Query.Name = "metroButton_Query";
            this.metroButton_Query.Size = new System.Drawing.Size(94, 30);
            this.metroButton_Query.TabIndex = 9;
            this.metroButton_Query.Text = "选中ID";
            this.metroButton_Query.UseSelectable = true;
            this.metroButton_Query.Click += new System.EventHandler(this.metroButton_Query_Click);
            // 
            // timestapDataGridViewTextBoxColumn
            // 
            this.timestapDataGridViewTextBoxColumn.DataPropertyName = "Timestap";
            this.timestapDataGridViewTextBoxColumn.FillWeight = 85.25506F;
            this.timestapDataGridViewTextBoxColumn.HeaderText = "Timestap";
            this.timestapDataGridViewTextBoxColumn.Name = "timestapDataGridViewTextBoxColumn";
            this.timestapDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // reservedDataGridViewTextBoxColumn
            // 
            this.reservedDataGridViewTextBoxColumn.DataPropertyName = "Reserved";
            this.reservedDataGridViewTextBoxColumn.FillWeight = 17.30554F;
            this.reservedDataGridViewTextBoxColumn.HeaderText = "Reserved";
            this.reservedDataGridViewTextBoxColumn.Name = "reservedDataGridViewTextBoxColumn";
            this.reservedDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // sendOrRecDataGridViewTextBoxColumn
            // 
            this.sendOrRecDataGridViewTextBoxColumn.DataPropertyName = "SendOrRec";
            this.sendOrRecDataGridViewTextBoxColumn.FillWeight = 19.6895F;
            this.sendOrRecDataGridViewTextBoxColumn.HeaderText = "SendOrRec";
            this.sendOrRecDataGridViewTextBoxColumn.Name = "sendOrRecDataGridViewTextBoxColumn";
            this.sendOrRecDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // reserved2DataGridViewTextBoxColumn
            // 
            this.reserved2DataGridViewTextBoxColumn.DataPropertyName = "Reserved2";
            this.reserved2DataGridViewTextBoxColumn.FillWeight = 22.41995F;
            this.reserved2DataGridViewTextBoxColumn.HeaderText = "Reserved2";
            this.reserved2DataGridViewTextBoxColumn.Name = "reserved2DataGridViewTextBoxColumn";
            this.reserved2DataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cANMsgDataGridViewTextBoxColumn
            // 
            this.cANMsgDataGridViewTextBoxColumn.DataPropertyName = "CAN_Msg";
            this.cANMsgDataGridViewTextBoxColumn.FillWeight = 355.33F;
            this.cANMsgDataGridViewTextBoxColumn.HeaderText = "CAN_Msg";
            this.cANMsgDataGridViewTextBoxColumn.Name = "cANMsgDataGridViewTextBoxColumn";
            this.cANMsgDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // aSCSignalBindingSource
            // 
            this.aSCSignalBindingSource.DataSource = typeof(AppTest.FormType.ASCSignal);
            // 
            // ASCForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 450);
            this.Controls.Add(this.metroButton_Query);
            this.Controls.Add(this.metroProgressBar1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.metroComboBoxDBCFiles);
            this.Controls.Add(this.tvAllNode);
            this.Controls.Add(this.metroButton_DBCFile);
            this.Controls.Add(this.metroButton_Import);
            this.Name = "ASCForm";
            this.Text = "ASCForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aSCSignalBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroButton metroButton_Import;
        private MetroFramework.Controls.MetroButton metroButton_DBCFile;
        private System.Windows.Forms.TreeView tvAllNode;
        private MetroFramework.Controls.MetroComboBox metroComboBoxDBCFiles;
        private System.Windows.Forms.DataGridView dataGridView1;
#pragma warning disable CS0169 // 从不使用字段“ASCForm.msgidDataGridViewTextBoxColumn”
        private System.Windows.Forms.DataGridViewTextBoxColumn msgidDataGridViewTextBoxColumn;
#pragma warning restore CS0169 // 从不使用字段“ASCForm.msgidDataGridViewTextBoxColumn”
        private System.Windows.Forms.BindingSource aSCSignalBindingSource;
        private MetroFramework.Controls.MetroProgressBar metroProgressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.DataGridViewTextBoxColumn timestapDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn reservedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sendOrRecDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn reserved2DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cANMsgDataGridViewTextBoxColumn;
        private MetroFramework.Controls.MetroButton metroButton_Query;
    }
}