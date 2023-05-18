
namespace AppTest.FormType
{
    partial class CheckBoxColorUC
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelColor = new System.Windows.Forms.Panel();
            this.tbSignalData = new System.Windows.Forms.TextBox();
            this.metroCheckBox1 = new MetroFramework.Controls.MetroCheckBox();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tableLayoutPanel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(242, 51);
            this.panel2.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.panelColor, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbSignalData, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.metroCheckBox1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(242, 51);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // panelColor
            // 
            this.panelColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelColor.Location = new System.Drawing.Point(3, 3);
            this.panelColor.Name = "panelColor";
            this.tableLayoutPanel1.SetRowSpan(this.panelColor, 2);
            this.panelColor.Size = new System.Drawing.Size(44, 45);
            this.panelColor.TabIndex = 2;
            this.panelColor.Click += new System.EventHandler(this.panel1_Click);
            // 
            // tbSignalData
            // 
            this.tbSignalData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSignalData.Location = new System.Drawing.Point(53, 28);
            this.tbSignalData.Name = "tbSignalData";
            this.tbSignalData.ReadOnly = true;
            this.tbSignalData.Size = new System.Drawing.Size(186, 21);
            this.tbSignalData.TabIndex = 3;
            // 
            // metroCheckBox1
            // 
            this.metroCheckBox1.AutoSize = true;
            this.metroCheckBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroCheckBox1.Location = new System.Drawing.Point(53, 3);
            this.metroCheckBox1.Name = "metroCheckBox1";
            this.metroCheckBox1.Size = new System.Drawing.Size(186, 19);
            this.metroCheckBox1.Style = MetroFramework.MetroColorStyle.Teal;
            this.metroCheckBox1.TabIndex = 4;
            this.metroCheckBox1.Text = "metroCheckBox1";
            this.metroCheckBox1.UseSelectable = true;
            this.metroCheckBox1.CheckedChanged += new System.EventHandler(this.btn_Click);
            // 
            // CheckBoxColorUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Name = "CheckBoxColorUC";
            this.Size = new System.Drawing.Size(242, 51);
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox tbSignalData;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelColor;
        private MetroFramework.Controls.MetroCheckBox metroCheckBox1;
    }
}
