namespace AppTest.FormType
{
    partial class AddProjectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddProjectForm));
            this.label1 = new System.Windows.Forms.Label();
            this.tbProjectName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbbProtocolType = new System.Windows.Forms.ComboBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cbbProtocolFiles = new System.Windows.Forms.ComboBox();
            this.btnImportProtocolFile = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbBaudValue1 = new System.Windows.Forms.Label();
            this.lbBaudValue0 = new System.Windows.Forms.Label();
            this.lbBaud = new System.Windows.Forms.Label();
            this.tbBaudValue1 = new System.Windows.Forms.TextBox();
            this.tbBaudValue0 = new System.Windows.Forms.TextBox();
            this.cbbBaud = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.cbChannelUsed = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.grbCanInfo = new System.Windows.Forms.GroupBox();
            this.btnDel = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.cbbCanIndex = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbbDeviceIndex = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbbDeviceType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tBoxProtocols = new System.Windows.Forms.TextBox();
            this.btnSelectProtocol = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.grbCanInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbProjectName
            // 
            resources.ApplyResources(this.tbProjectName, "tbProjectName");
            this.tbProjectName.Name = "tbProjectName";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cbbProtocolType
            // 
            this.cbbProtocolType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbProtocolType.FormattingEnabled = true;
            resources.ApplyResources(this.cbbProtocolType, "cbbProtocolType");
            this.cbbProtocolType.Name = "cbbProtocolType";
            this.cbbProtocolType.SelectedIndexChanged += new System.EventHandler(this.cbbProtocolType_SelectedIndexChanged);
            // 
            // btnConfirm
            // 
            resources.ApplyResources(this.btnConfirm, "btnConfirm");
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cbbProtocolFiles
            // 
            this.cbbProtocolFiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbProtocolFiles.FormattingEnabled = true;
            resources.ApplyResources(this.cbbProtocolFiles, "cbbProtocolFiles");
            this.cbbProtocolFiles.Name = "cbbProtocolFiles";
            // 
            // btnImportProtocolFile
            // 
            resources.ApplyResources(this.btnImportProtocolFile, "btnImportProtocolFile");
            this.btnImportProtocolFile.Name = "btnImportProtocolFile";
            this.btnImportProtocolFile.UseVisualStyleBackColor = true;
            this.btnImportProtocolFile.Click += new System.EventHandler(this.btnImportProtocolFile_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSelectProtocol);
            this.groupBox1.Controls.Add(this.tBoxProtocols);
            this.groupBox1.Controls.Add(this.lbBaudValue1);
            this.groupBox1.Controls.Add(this.lbBaudValue0);
            this.groupBox1.Controls.Add(this.lbBaud);
            this.groupBox1.Controls.Add(this.tbBaudValue1);
            this.groupBox1.Controls.Add(this.tbBaudValue0);
            this.groupBox1.Controls.Add(this.cbbBaud);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.cbChannelUsed);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnImportProtocolFile);
            this.groupBox1.Controls.Add(this.cbbProtocolType);
            this.groupBox1.Controls.Add(this.cbbProtocolFiles);
            this.groupBox1.Controls.Add(this.label3);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // lbBaudValue1
            // 
            resources.ApplyResources(this.lbBaudValue1, "lbBaudValue1");
            this.lbBaudValue1.Name = "lbBaudValue1";
            // 
            // lbBaudValue0
            // 
            resources.ApplyResources(this.lbBaudValue0, "lbBaudValue0");
            this.lbBaudValue0.Name = "lbBaudValue0";
            // 
            // lbBaud
            // 
            resources.ApplyResources(this.lbBaud, "lbBaud");
            this.lbBaud.Name = "lbBaud";
            // 
            // tbBaudValue1
            // 
            resources.ApplyResources(this.tbBaudValue1, "tbBaudValue1");
            this.tbBaudValue1.Name = "tbBaudValue1";
            this.tbBaudValue1.ReadOnly = true;
            // 
            // tbBaudValue0
            // 
            resources.ApplyResources(this.tbBaudValue0, "tbBaudValue0");
            this.tbBaudValue0.Name = "tbBaudValue0";
            this.tbBaudValue0.ReadOnly = true;
            // 
            // cbbBaud
            // 
            this.cbbBaud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbBaud.FormattingEnabled = true;
            resources.ApplyResources(this.cbbBaud, "cbbBaud");
            this.cbbBaud.Name = "cbbBaud";
            this.cbbBaud.SelectedIndexChanged += new System.EventHandler(this.cbbBaud_SelectedIndexChanged);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.BtnSaveChannel_Click);
            // 
            // cbChannelUsed
            // 
            resources.ApplyResources(this.cbChannelUsed, "cbChannelUsed");
            this.cbChannelUsed.Name = "cbChannelUsed";
            this.cbChannelUsed.UseVisualStyleBackColor = true;
            this.cbChannelUsed.CheckedChanged += new System.EventHandler(this.cbChannelUsed_CheckedChanged);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // grbCanInfo
            // 
            this.grbCanInfo.Controls.Add(this.btnDel);
            this.grbCanInfo.Controls.Add(this.button1);
            this.grbCanInfo.Controls.Add(this.groupBox1);
            this.grbCanInfo.Controls.Add(this.cbbCanIndex);
            this.grbCanInfo.Controls.Add(this.label6);
            this.grbCanInfo.Controls.Add(this.cbbDeviceIndex);
            this.grbCanInfo.Controls.Add(this.label5);
            this.grbCanInfo.Controls.Add(this.cbbDeviceType);
            this.grbCanInfo.Controls.Add(this.label4);
            resources.ApplyResources(this.grbCanInfo, "grbCanInfo");
            this.grbCanInfo.Name = "grbCanInfo";
            this.grbCanInfo.TabStop = false;
            // 
            // btnDel
            // 
            resources.ApplyResources(this.btnDel, "btnDel");
            this.btnDel.Name = "btnDel";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.BtnAddChannel_Click);
            // 
            // cbbCanIndex
            // 
            this.cbbCanIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbCanIndex.FormattingEnabled = true;
            resources.ApplyResources(this.cbbCanIndex, "cbbCanIndex");
            this.cbbCanIndex.Name = "cbbCanIndex";
            this.cbbCanIndex.SelectedIndexChanged += new System.EventHandler(this.cbbCanChannel_SelectedIndexChanged);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // cbbDeviceIndex
            // 
            this.cbbDeviceIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbDeviceIndex.FormattingEnabled = true;
            this.cbbDeviceIndex.Items.AddRange(new object[] {
            resources.GetString("cbbDeviceIndex.Items"),
            resources.GetString("cbbDeviceIndex.Items1"),
            resources.GetString("cbbDeviceIndex.Items2"),
            resources.GetString("cbbDeviceIndex.Items3"),
            resources.GetString("cbbDeviceIndex.Items4"),
            resources.GetString("cbbDeviceIndex.Items5"),
            resources.GetString("cbbDeviceIndex.Items6"),
            resources.GetString("cbbDeviceIndex.Items7")});
            resources.ApplyResources(this.cbbDeviceIndex, "cbbDeviceIndex");
            this.cbbDeviceIndex.Name = "cbbDeviceIndex";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cbbDeviceType
            // 
            this.cbbDeviceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbDeviceType.FormattingEnabled = true;
            resources.ApplyResources(this.cbbDeviceType, "cbbDeviceType");
            this.cbbDeviceType.Name = "cbbDeviceType";
            this.cbbDeviceType.SelectedIndexChanged += new System.EventHandler(this.cbbDeviceType_SelectedIndexChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panel1.Name = "panel1";
            // 
            // tBoxProtocols
            // 
            resources.ApplyResources(this.tBoxProtocols, "tBoxProtocols");
            this.tBoxProtocols.Name = "tBoxProtocols";
            this.tBoxProtocols.ReadOnly = true;
            // 
            // btnSelectProtocol
            // 
            resources.ApplyResources(this.btnSelectProtocol, "btnSelectProtocol");
            this.btnSelectProtocol.Name = "btnSelectProtocol";
            this.btnSelectProtocol.UseVisualStyleBackColor = true;
            this.btnSelectProtocol.Click += new System.EventHandler(this.btnSelectProtocol_Click);
            // 
            // AddProjectForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.grbCanInfo);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.tbProjectName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "AddProjectForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grbCanInfo.ResumeLayout(false);
            this.grbCanInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbProjectName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbbProtocolType;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbbProtocolFiles;
        private System.Windows.Forms.Button btnImportProtocolFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox grbCanInfo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbbCanIndex;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbbDeviceIndex;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbbDeviceType;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox cbChannelUsed;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox tbBaudValue1;
        private System.Windows.Forms.TextBox tbBaudValue0;
        private System.Windows.Forms.ComboBox cbbBaud;
        private System.Windows.Forms.Label lbBaud;
        private System.Windows.Forms.Label lbBaudValue1;
        private System.Windows.Forms.Label lbBaudValue0;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnSelectProtocol;
        private System.Windows.Forms.TextBox tBoxProtocols;
    }
}