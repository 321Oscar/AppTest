
namespace AppTest.View.UC
{
    partial class LogListview
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogListview));
            this.lvLog = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnClearLog = new System.Windows.Forms.Button();
            this.cbbLogLevel = new System.Windows.Forms.ComboBox();
            this.lbLogLevel = new System.Windows.Forms.Label();
            this.btnQueryLog = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.querytb = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvLog
            // 
            this.lvLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvLog.FullRowSelect = true;
            this.lvLog.GridLines = true;
            this.lvLog.HideSelection = false;
            this.lvLog.Location = new System.Drawing.Point(0, 30);
            this.lvLog.Margin = new System.Windows.Forms.Padding(4);
            this.lvLog.Name = "lvLog";
            this.lvLog.Size = new System.Drawing.Size(651, 118);
            this.lvLog.SmallImageList = this.imageList1;
            this.lvLog.TabIndex = 0;
            this.lvLog.UseCompatibleStateImageBehavior = false;
            this.lvLog.View = System.Windows.Forms.View.Details;
            this.lvLog.VirtualListSize = 50;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Datetime";
            this.columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "LogLevel";
            this.columnHeader2.Width = 73;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "LogStr";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 300;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "debug.png");
            this.imageList1.Images.SetKeyName(1, "info.png");
            this.imageList1.Images.SetKeyName(2, "warn.png");
            this.imageList1.Images.SetKeyName(3, "error.png");
            // 
            // btnClearLog
            // 
            this.btnClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearLog.Location = new System.Drawing.Point(200, 0);
            this.btnClearLog.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(88, 27);
            this.btnClearLog.TabIndex = 1;
            this.btnClearLog.Text = "Clear";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // cbbLogLevel
            // 
            this.cbbLogLevel.FormattingEnabled = true;
            this.cbbLogLevel.Location = new System.Drawing.Point(52, 2);
            this.cbbLogLevel.Margin = new System.Windows.Forms.Padding(4);
            this.cbbLogLevel.Name = "cbbLogLevel";
            this.cbbLogLevel.Size = new System.Drawing.Size(140, 25);
            this.cbbLogLevel.TabIndex = 2;
            this.cbbLogLevel.SelectedIndexChanged += new System.EventHandler(this.cbbLogLevel_SelectedIndexChanged);
            // 
            // lbLogLevel
            // 
            this.lbLogLevel.AutoSize = true;
            this.lbLogLevel.Location = new System.Drawing.Point(4, 4);
            this.lbLogLevel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbLogLevel.Name = "lbLogLevel";
            this.lbLogLevel.Size = new System.Drawing.Size(40, 17);
            this.lbLogLevel.TabIndex = 3;
            this.lbLogLevel.Text = "Level:";
            // 
            // btnQueryLog
            // 
            this.btnQueryLog.Location = new System.Drawing.Point(487, 2);
            this.btnQueryLog.Margin = new System.Windows.Forms.Padding(4);
            this.btnQueryLog.Name = "btnQueryLog";
            this.btnQueryLog.Size = new System.Drawing.Size(88, 23);
            this.btnQueryLog.TabIndex = 4;
            this.btnQueryLog.Text = "Test";
            this.btnQueryLog.UseVisualStyleBackColor = true;
            this.btnQueryLog.Visible = false;
            this.btnQueryLog.Click += new System.EventHandler(this.btnAddLog_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.querytb);
            this.panel1.Controls.Add(this.btnClearLog);
            this.panel1.Controls.Add(this.cbbLogLevel);
            this.panel1.Controls.Add(this.lbLogLevel);
            this.panel1.Controls.Add(this.btnQueryLog);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(651, 30);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(309, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "关键字";
            this.label1.Visible = false;
            // 
            // querytb
            // 
            this.querytb.Location = new System.Drawing.Point(364, 3);
            this.querytb.Margin = new System.Windows.Forms.Padding(4);
            this.querytb.Name = "querytb";
            this.querytb.Size = new System.Drawing.Size(116, 23);
            this.querytb.TabIndex = 5;
            this.querytb.Visible = false;
            // 
            // LogListview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvLog);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LogListview";
            this.Size = new System.Drawing.Size(651, 148);
            this.SizeChanged += new System.EventHandler(this.LogListview_SizeChanged);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvLog;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.ComboBox cbbLogLevel;
        private System.Windows.Forms.Label lbLogLevel;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button btnQueryLog;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox querytb;
    }
}
