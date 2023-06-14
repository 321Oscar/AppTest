
namespace AppTest
{
    partial class MainFormV2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFormV2));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip_Project = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProjectToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.newFormToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip_Form = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openFormToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editFormToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteFormToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newFormToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.treeViewRoot = new System.Windows.Forms.TreeView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.方案ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.otherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dBCFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.historyMeasureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLogFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aSCFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eLFAndA2LFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.versionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuStrip_Project.SuspendLayout();
            this.contextMenuStrip_Form.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Measure.png");
            this.imageList1.Images.SetKeyName(1, "Get.png");
            this.imageList1.Images.SetKeyName(2, "Set.png");
            this.imageList1.Images.SetKeyName(3, "Rolling.png");
            this.imageList1.Images.SetKeyName(4, "解决方案.png");
            this.imageList1.Images.SetKeyName(5, "解决方案中心.png");
            // 
            // contextMenuStrip_Project
            // 
            this.contextMenuStrip_Project.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip_Project.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.refreshToolStripMenuItem,
            this.newProjectToolStripMenuItem1,
            this.newFormToolStripMenuItem});
            this.contextMenuStrip_Project.Name = "contextMenuStrip_Project";
            this.contextMenuStrip_Project.Size = new System.Drawing.Size(155, 184);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::AppTest.Properties.Resources.open;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(154, 30);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("editToolStripMenuItem.Image")));
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(154, 30);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.EditToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteToolStripMenuItem.Image")));
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(154, 30);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItem_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Image = global::AppTest.Properties.Resources.refresh;
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(154, 30);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.RefreshToolStripMenuItem_Click);
            // 
            // newProjectToolStripMenuItem1
            // 
            this.newProjectToolStripMenuItem1.Image = global::AppTest.Properties.Resources.add;
            this.newProjectToolStripMenuItem1.Name = "newProjectToolStripMenuItem1";
            this.newProjectToolStripMenuItem1.Size = new System.Drawing.Size(154, 30);
            this.newProjectToolStripMenuItem1.Text = "New Project";
            this.newProjectToolStripMenuItem1.Click += new System.EventHandler(this.NewProjectToolStripMenuItem_Click);
            // 
            // newFormToolStripMenuItem
            // 
            this.newFormToolStripMenuItem.Image = global::AppTest.Properties.Resources.add;
            this.newFormToolStripMenuItem.Name = "newFormToolStripMenuItem";
            this.newFormToolStripMenuItem.Size = new System.Drawing.Size(154, 30);
            this.newFormToolStripMenuItem.Text = "Add Form";
            this.newFormToolStripMenuItem.Click += new System.EventHandler(this.NewFormToolStripMenuItem_Click);
            // 
            // contextMenuStrip_Form
            // 
            this.contextMenuStrip_Form.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip_Form.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFormToolStripMenuItem,
            this.editFormToolStripMenuItem,
            this.deleteFormToolStripMenuItem,
            this.newFormToolStripMenuItem1});
            this.contextMenuStrip_Form.Name = "contextMenuStrip_Form";
            this.contextMenuStrip_Form.Size = new System.Drawing.Size(156, 124);
            // 
            // openFormToolStripMenuItem
            // 
            this.openFormToolStripMenuItem.Image = global::AppTest.Properties.Resources.open;
            this.openFormToolStripMenuItem.Name = "openFormToolStripMenuItem";
            this.openFormToolStripMenuItem.Size = new System.Drawing.Size(155, 30);
            this.openFormToolStripMenuItem.Text = "Open Form";
            this.openFormToolStripMenuItem.Visible = false;
            this.openFormToolStripMenuItem.Click += new System.EventHandler(this.OpenFormToolStripMenuItem_Click);
            // 
            // editFormToolStripMenuItem
            // 
            this.editFormToolStripMenuItem.Image = global::AppTest.Properties.Resources.Edit;
            this.editFormToolStripMenuItem.Name = "editFormToolStripMenuItem";
            this.editFormToolStripMenuItem.Size = new System.Drawing.Size(155, 30);
            this.editFormToolStripMenuItem.Text = "Edit Form";
            this.editFormToolStripMenuItem.Click += new System.EventHandler(this.EditFormToolStripMenuItem_Click);
            // 
            // deleteFormToolStripMenuItem
            // 
            this.deleteFormToolStripMenuItem.Image = global::AppTest.Properties.Resources.Delete;
            this.deleteFormToolStripMenuItem.Name = "deleteFormToolStripMenuItem";
            this.deleteFormToolStripMenuItem.Size = new System.Drawing.Size(155, 30);
            this.deleteFormToolStripMenuItem.Text = "Delete Form";
            this.deleteFormToolStripMenuItem.Click += new System.EventHandler(this.DeleteFormToolStripMenuItem_Click);
            // 
            // newFormToolStripMenuItem1
            // 
            this.newFormToolStripMenuItem1.Image = global::AppTest.Properties.Resources.add;
            this.newFormToolStripMenuItem1.Name = "newFormToolStripMenuItem1";
            this.newFormToolStripMenuItem1.Size = new System.Drawing.Size(155, 30);
            this.newFormToolStripMenuItem1.Text = "Add Form";
            this.newFormToolStripMenuItem1.Click += new System.EventHandler(this.NewFormToolStripMenuItem_Click);
            // 
            // treeViewRoot
            // 
            this.treeViewRoot.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeViewRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewRoot.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeViewRoot.FullRowSelect = true;
            this.treeViewRoot.ImageIndex = 0;
            this.treeViewRoot.ImageList = this.imageList1;
            this.treeViewRoot.Location = new System.Drawing.Point(0, 24);
            this.treeViewRoot.Name = "treeViewRoot";
            this.treeViewRoot.SelectedImageIndex = 0;
            this.treeViewRoot.Size = new System.Drawing.Size(292, 602);
            this.treeViewRoot.TabIndex = 1;
            this.treeViewRoot.NodeMouseHover += new System.Windows.Forms.TreeNodeMouseHoverEventHandler(this.treeViewRoot_NodeMouseHover);
            this.treeViewRoot.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewRoot_NodeMouseClick);
            this.treeViewRoot.DoubleClick += new System.EventHandler(this.treeViewRoot_DoubleClick);
            this.treeViewRoot.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TreeViewRoot_MouseClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.方案ToolStripMenuItem,
            this.otherToolStripMenuItem,
            this.关于ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(292, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(44, 22);
            this.saveToolStripMenuItem.Text = "保存";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // 方案ToolStripMenuItem
            // 
            this.方案ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProjectToolStripMenuItem,
            this.importProjectToolStripMenuItem,
            this.exportProjectToolStripMenuItem});
            this.方案ToolStripMenuItem.Name = "方案ToolStripMenuItem";
            this.方案ToolStripMenuItem.Size = new System.Drawing.Size(44, 22);
            this.方案ToolStripMenuItem.Text = "方案";
            // 
            // newProjectToolStripMenuItem
            // 
            this.newProjectToolStripMenuItem.Image = global::AppTest.Properties.Resources.add;
            this.newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
            this.newProjectToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.newProjectToolStripMenuItem.Text = "新建";
            this.newProjectToolStripMenuItem.Click += new System.EventHandler(this.NewProjectToolStripMenuItem_Click);
            // 
            // importProjectToolStripMenuItem
            // 
            this.importProjectToolStripMenuItem.Image = global::AppTest.Properties.Resources.Import;
            this.importProjectToolStripMenuItem.Name = "importProjectToolStripMenuItem";
            this.importProjectToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.importProjectToolStripMenuItem.Text = "导入";
            this.importProjectToolStripMenuItem.Click += new System.EventHandler(this.ImportProjectToolStripMenuItem_Click);
            // 
            // exportProjectToolStripMenuItem
            // 
            this.exportProjectToolStripMenuItem.Image = global::AppTest.Properties.Resources.export__1_;
            this.exportProjectToolStripMenuItem.Name = "exportProjectToolStripMenuItem";
            this.exportProjectToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.exportProjectToolStripMenuItem.Text = "导出";
            this.exportProjectToolStripMenuItem.Click += new System.EventHandler(this.ExportProjectToolStripMenuItem_Click);
            // 
            // otherToolStripMenuItem
            // 
            this.otherToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dBCFileToolStripMenuItem,
            this.eLFAndA2LFileToolStripMenuItem,
            this.aSCFileToolStripMenuItem,
            this.toolStripSeparator1,
            this.historyMeasureToolStripMenuItem,
            this.toolStripSeparator2,
            this.showLogFileToolStripMenuItem,
            this.showLogToolStripMenuItem,
            this.toolStripSeparator3,
            this.reStartToolStripMenuItem,
            this.fontToolStripMenuItem,
            this.testToolStripMenuItem});
            this.otherToolStripMenuItem.Name = "otherToolStripMenuItem";
            this.otherToolStripMenuItem.Size = new System.Drawing.Size(44, 22);
            this.otherToolStripMenuItem.Text = "其他";
            // 
            // dBCFileToolStripMenuItem
            // 
            this.dBCFileToolStripMenuItem.Name = "dBCFileToolStripMenuItem";
            this.dBCFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.dBCFileToolStripMenuItem.Text = "DBC解析";
            this.dBCFileToolStripMenuItem.Click += new System.EventHandler(this.dBCFileToolStripMenuItem_Click_1);
            // 
            // historyMeasureToolStripMenuItem
            // 
            this.historyMeasureToolStripMenuItem.Name = "historyMeasureToolStripMenuItem";
            this.historyMeasureToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.historyMeasureToolStripMenuItem.Text = "HistoryMeasure";
            this.historyMeasureToolStripMenuItem.Click += new System.EventHandler(this.historyMeasureToolStripMenuItem_Click);
            // 
            // fontToolStripMenuItem
            // 
            this.fontToolStripMenuItem.Name = "fontToolStripMenuItem";
            this.fontToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.fontToolStripMenuItem.Text = "Font";
            this.fontToolStripMenuItem.Click += new System.EventHandler(this.fontToolStripMenuItem_Click);
            // 
            // reStartToolStripMenuItem
            // 
            this.reStartToolStripMenuItem.Name = "reStartToolStripMenuItem";
            this.reStartToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.reStartToolStripMenuItem.Text = "ReStart";
            this.reStartToolStripMenuItem.Visible = false;
            this.reStartToolStripMenuItem.Click += new System.EventHandler(this.reStartToolStripMenuItem_Click);
            // 
            // showLogFileToolStripMenuItem
            // 
            this.showLogFileToolStripMenuItem.Name = "showLogFileToolStripMenuItem";
            this.showLogFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.showLogFileToolStripMenuItem.Text = "ShowLogFile";
            this.showLogFileToolStripMenuItem.Click += new System.EventHandler(this.showLogFileToolStripMenuItem_Click);
            // 
            // showLogToolStripMenuItem
            // 
            this.showLogToolStripMenuItem.Name = "showLogToolStripMenuItem";
            this.showLogToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.showLogToolStripMenuItem.Text = "ShowLog";
            this.showLogToolStripMenuItem.Visible = false;
            this.showLogToolStripMenuItem.Click += new System.EventHandler(this.showLogToolStripMenuItem_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.testToolStripMenuItem.Text = "Test";
            this.testToolStripMenuItem.Click += new System.EventHandler(this.testToolStripMenuItem_Click);
            // 
            // aSCFileToolStripMenuItem
            // 
            this.aSCFileToolStripMenuItem.Name = "aSCFileToolStripMenuItem";
            this.aSCFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.aSCFileToolStripMenuItem.Text = "ASCFile";
            this.aSCFileToolStripMenuItem.Click += new System.EventHandler(this.aSCFileToolStripMenuItem_Click);
            // 
            // eLFAndA2LFileToolStripMenuItem
            // 
            this.eLFAndA2LFileToolStripMenuItem.Name = "eLFAndA2LFileToolStripMenuItem";
            this.eLFAndA2LFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.eLFAndA2LFileToolStripMenuItem.Text = "ELFAndA2LFile";
            this.eLFAndA2LFileToolStripMenuItem.Click += new System.EventHandler(this.eLFAndA2LFileToolStripMenuItem_Click);
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.versionToolStripMenuItem});
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(44, 22);
            this.关于ToolStripMenuItem.Text = "版本";
            // 
            // versionToolStripMenuItem
            // 
            this.versionToolStripMenuItem.Image = global::AppTest.Properties.Resources.version;
            this.versionToolStripMenuItem.Name = "versionToolStripMenuItem";
            this.versionToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.versionToolStripMenuItem.Text = "版本";
            this.versionToolStripMenuItem.Click += new System.EventHandler(this.versionToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(177, 6);
            // 
            // MainFormV2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 626);
            this.Controls.Add(this.treeViewRoot);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainFormV2";
            this.Text = "MainFormV2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormV2_FormClosing);
            this.contextMenuStrip_Project.ResumeLayout(false);
            this.contextMenuStrip_Form.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 方案ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.TreeView treeViewRoot;
        private System.Windows.Forms.ToolStripMenuItem newProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportProjectToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Project;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Form;
        private System.Windows.Forms.ToolStripMenuItem openFormToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editFormToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteFormToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem versionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newProjectToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem newFormToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newFormToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem otherToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem historyMeasureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fontToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dBCFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reStartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLogFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aSCFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eLFAndA2LFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}