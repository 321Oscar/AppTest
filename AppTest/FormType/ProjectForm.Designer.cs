
namespace AppTest
{
    partial class ProjectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_Connect = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_DisConnect = new System.Windows.Forms.ToolStripButton();
            this.AddtoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.VerticaltoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.tsb_ConnectXCP = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.OpenFormToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newFormToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_Connect,
            this.toolStripButton_DisConnect,
            this.AddtoolStripButton,
            this.VerticaltoolStripButton,
            this.tsb_ConnectXCP});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 25);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(1222, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_Connect
            // 
            this.toolStripButton_Connect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Connect.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Connect.Image")));
            this.toolStripButton_Connect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Connect.Name = "toolStripButton_Connect";
            this.toolStripButton_Connect.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_Connect.Text = "ConnectCan";
            this.toolStripButton_Connect.Click += new System.EventHandler(this.toolStripButton_Connect_Click);
            // 
            // toolStripButton_DisConnect
            // 
            this.toolStripButton_DisConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_DisConnect.Enabled = false;
            this.toolStripButton_DisConnect.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_DisConnect.Image")));
            this.toolStripButton_DisConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_DisConnect.Name = "toolStripButton_DisConnect";
            this.toolStripButton_DisConnect.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_DisConnect.Text = "DisConnect";
            this.toolStripButton_DisConnect.Click += new System.EventHandler(this.toolStripButton_DisConnect_Click);
            // 
            // AddtoolStripButton
            // 
            this.AddtoolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddtoolStripButton.Image = global::AppTest.Properties.Resources.add;
            this.AddtoolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddtoolStripButton.Name = "AddtoolStripButton";
            this.AddtoolStripButton.Size = new System.Drawing.Size(23, 22);
            this.AddtoolStripButton.Text = "toolStripButton1";
            this.AddtoolStripButton.ToolTipText = "新建窗口";
            this.AddtoolStripButton.Click += new System.EventHandler(this.newFormToolStripMenuItem_Click);
            // 
            // VerticaltoolStripButton
            // 
            this.VerticaltoolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.VerticaltoolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("VerticaltoolStripButton.Image")));
            this.VerticaltoolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.VerticaltoolStripButton.Name = "VerticaltoolStripButton";
            this.VerticaltoolStripButton.Size = new System.Drawing.Size(23, 22);
            this.VerticaltoolStripButton.Text = "铺满";
            this.VerticaltoolStripButton.Click += new System.EventHandler(this.VerticaltoolStripButton_Click);
            // 
            // tsb_ConnectXCP
            // 
            this.tsb_ConnectXCP.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_ConnectXCP.Image = ((System.Drawing.Image)(resources.GetObject("tsb_ConnectXCP.Image")));
            this.tsb_ConnectXCP.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_ConnectXCP.Name = "tsb_ConnectXCP";
            this.tsb_ConnectXCP.Size = new System.Drawing.Size(23, 22);
            this.tsb_ConnectXCP.Text = "toolStripButton1";
            this.tsb_ConnectXCP.Visible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenFormToolStripMenuItem,
            this.OpenedToolStripMenuItem,
            this.newFormToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.MdiWindowListItem = this.OpenedToolStripMenuItem;
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1222, 25);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // OpenFormToolStripMenuItem
            // 
            this.OpenFormToolStripMenuItem.Name = "OpenFormToolStripMenuItem";
            this.OpenFormToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.OpenFormToolStripMenuItem.Text = "打开窗口";
            // 
            // OpenedToolStripMenuItem
            // 
            this.OpenedToolStripMenuItem.Checked = true;
            this.OpenedToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.OpenedToolStripMenuItem.Name = "OpenedToolStripMenuItem";
            this.OpenedToolStripMenuItem.Size = new System.Drawing.Size(80, 21);
            this.OpenedToolStripMenuItem.Text = "已打开窗口";
            this.OpenedToolStripMenuItem.Visible = false;
            // 
            // newFormToolStripMenuItem
            // 
            this.newFormToolStripMenuItem.Name = "newFormToolStripMenuItem";
            this.newFormToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.newFormToolStripMenuItem.Text = "新建窗口";
            this.newFormToolStripMenuItem.Click += new System.EventHandler(this.newFormToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Measure.png");
            this.imageList1.Images.SetKeyName(1, "Get.png");
            this.imageList1.Images.SetKeyName(2, "Set.png");
            this.imageList1.Images.SetKeyName(3, "RL.png");
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 720);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1222, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ProjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1222, 742);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ProjectForm";
            this.Text = "ProjectForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProjectForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProjectForm_FormClosed);
            this.Load += new System.EventHandler(this.ProjectForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_Connect;
        private System.Windows.Forms.ToolStripButton toolStripButton_DisConnect;
        private System.Windows.Forms.ToolStripMenuItem OpenFormToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newFormToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem OpenedToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton AddtoolStripButton;
        private System.Windows.Forms.ToolStripButton VerticaltoolStripButton;
        private System.Windows.Forms.ToolStripButton tsb_ConnectXCP;
        private System.Windows.Forms.StatusStrip statusStrip1;
    }
}