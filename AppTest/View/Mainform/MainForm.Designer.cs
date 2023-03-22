
namespace AppTest
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.contextMenuStrip_TreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modifiedProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dBCFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsFormItem = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openFormToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editFormToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteFormToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeViewRoot = new System.Windows.Forms.TreeView();
            this.btnProjectSave = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.工程ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.otherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.historyDataCurveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dbcFileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.VersionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip_TreeView.SuspendLayout();
            this.cmsFormItem.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip_TreeView
            // 
            this.contextMenuStrip_TreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.openProjectToolStripMenuItem,
            this.modifiedProjectToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.importProjectToolStripMenuItem,
            this.exportProjectToolStripMenuItem,
            this.dBCFileToolStripMenuItem});
            this.contextMenuStrip_TreeView.Name = "contextMenuStrip_TreeView";
            resources.ApplyResources(this.contextMenuStrip_TreeView, "contextMenuStrip_TreeView");
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0, 2, 0, 1);
            resources.ApplyResources(this.addToolStripMenuItem, "addToolStripMenuItem");
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // openProjectToolStripMenuItem
            // 
            resources.ApplyResources(this.openProjectToolStripMenuItem, "openProjectToolStripMenuItem");
            this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
            this.openProjectToolStripMenuItem.Click += new System.EventHandler(this.openProjectToolStripMenuItem_Click);
            // 
            // modifiedProjectToolStripMenuItem
            // 
            resources.ApplyResources(this.modifiedProjectToolStripMenuItem, "modifiedProjectToolStripMenuItem");
            this.modifiedProjectToolStripMenuItem.Name = "modifiedProjectToolStripMenuItem";
            this.modifiedProjectToolStripMenuItem.Click += new System.EventHandler(this.modifiedProjectToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // importProjectToolStripMenuItem
            // 
            this.importProjectToolStripMenuItem.Name = "importProjectToolStripMenuItem";
            resources.ApplyResources(this.importProjectToolStripMenuItem, "importProjectToolStripMenuItem");
            this.importProjectToolStripMenuItem.Click += new System.EventHandler(this.importProjectToolStripMenuItem_Click);
            // 
            // exportProjectToolStripMenuItem
            // 
            this.exportProjectToolStripMenuItem.Name = "exportProjectToolStripMenuItem";
            resources.ApplyResources(this.exportProjectToolStripMenuItem, "exportProjectToolStripMenuItem");
            this.exportProjectToolStripMenuItem.Click += new System.EventHandler(this.exportProjectToolStripMenuItem_Click);
            // 
            // dBCFileToolStripMenuItem
            // 
            this.dBCFileToolStripMenuItem.Name = "dBCFileToolStripMenuItem";
            resources.ApplyResources(this.dBCFileToolStripMenuItem, "dBCFileToolStripMenuItem");
            this.dBCFileToolStripMenuItem.Click += new System.EventHandler(this.dBCFileToolStripMenuItem_Click);
            // 
            // cmsFormItem
            // 
            this.cmsFormItem.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFormToolStripMenuItem,
            this.editFormToolStripMenuItem,
            this.deleteFormToolStripMenuItem});
            this.cmsFormItem.Name = "cmsFormItem";
            resources.ApplyResources(this.cmsFormItem, "cmsFormItem");
            // 
            // openFormToolStripMenuItem
            // 
            this.openFormToolStripMenuItem.Name = "openFormToolStripMenuItem";
            resources.ApplyResources(this.openFormToolStripMenuItem, "openFormToolStripMenuItem");
            this.openFormToolStripMenuItem.Click += new System.EventHandler(this.openFormToolStripMenuItem_Click);
            // 
            // editFormToolStripMenuItem
            // 
            this.editFormToolStripMenuItem.Name = "editFormToolStripMenuItem";
            resources.ApplyResources(this.editFormToolStripMenuItem, "editFormToolStripMenuItem");
            this.editFormToolStripMenuItem.Click += new System.EventHandler(this.editFormToolStripMenuItem_Click);
            // 
            // deleteFormToolStripMenuItem
            // 
            this.deleteFormToolStripMenuItem.Name = "deleteFormToolStripMenuItem";
            resources.ApplyResources(this.deleteFormToolStripMenuItem, "deleteFormToolStripMenuItem");
            this.deleteFormToolStripMenuItem.Click += new System.EventHandler(this.deleteFormToolStripMenuItem_Click);
            // 
            // tabPage1
            // 
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.menuStripMain, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeViewRoot);
            this.splitContainer1.Panel1.Controls.Add(this.btnProjectSave);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            // 
            // treeViewRoot
            // 
            this.treeViewRoot.ContextMenuStrip = this.contextMenuStrip_TreeView;
            resources.ApplyResources(this.treeViewRoot, "treeViewRoot");
            this.treeViewRoot.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.treeViewRoot.ImageList = this.imageList1;
            this.treeViewRoot.Name = "treeViewRoot";
            this.treeViewRoot.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeViewRoot_DrawNode);
            this.treeViewRoot.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            this.treeViewRoot.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseClick);
            // 
            // btnProjectSave
            // 
            resources.ApplyResources(this.btnProjectSave, "btnProjectSave");
            this.btnProjectSave.Name = "btnProjectSave";
            this.btnProjectSave.UseVisualStyleBackColor = true;
            this.btnProjectSave.Click += new System.EventHandler(this.btnProjectSave_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // menuStripMain
            // 
            this.menuStripMain.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.工程ToolStripMenuItem,
            this.otherToolStripMenuItem,
            this.AboutToolStripMenuItem});
            resources.ApplyResources(this.menuStripMain, "menuStripMain");
            this.menuStripMain.Name = "menuStripMain";
            // 
            // 工程ToolStripMenuItem
            // 
            this.工程ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProjectToolStripMenuItem,
            this.ImportToolStripMenuItem,
            this.ExportToolStripMenuItem});
            this.工程ToolStripMenuItem.Name = "工程ToolStripMenuItem";
            resources.ApplyResources(this.工程ToolStripMenuItem, "工程ToolStripMenuItem");
            // 
            // newProjectToolStripMenuItem
            // 
            this.newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
            resources.ApplyResources(this.newProjectToolStripMenuItem, "newProjectToolStripMenuItem");
            this.newProjectToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // ImportToolStripMenuItem
            // 
            this.ImportToolStripMenuItem.Name = "ImportToolStripMenuItem";
            resources.ApplyResources(this.ImportToolStripMenuItem, "ImportToolStripMenuItem");
            this.ImportToolStripMenuItem.Click += new System.EventHandler(this.importProjectToolStripMenuItem_Click);
            // 
            // ExportToolStripMenuItem
            // 
            this.ExportToolStripMenuItem.Name = "ExportToolStripMenuItem";
            resources.ApplyResources(this.ExportToolStripMenuItem, "ExportToolStripMenuItem");
            this.ExportToolStripMenuItem.Click += new System.EventHandler(this.exportProjectToolStripMenuItem_Click);
            // 
            // otherToolStripMenuItem
            // 
            this.otherToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.historyDataCurveToolStripMenuItem,
            this.dbcFileToolStripMenuItem1});
            this.otherToolStripMenuItem.Name = "otherToolStripMenuItem";
            resources.ApplyResources(this.otherToolStripMenuItem, "otherToolStripMenuItem");
            // 
            // historyDataCurveToolStripMenuItem
            // 
            this.historyDataCurveToolStripMenuItem.Name = "historyDataCurveToolStripMenuItem";
            resources.ApplyResources(this.historyDataCurveToolStripMenuItem, "historyDataCurveToolStripMenuItem");
            this.historyDataCurveToolStripMenuItem.Click += new System.EventHandler(this.historyDataCurveToolStripMenuItem_Click);
            // 
            // dbcFileToolStripMenuItem1
            // 
            this.dbcFileToolStripMenuItem1.Name = "dbcFileToolStripMenuItem1";
            resources.ApplyResources(this.dbcFileToolStripMenuItem1, "dbcFileToolStripMenuItem1");
            this.dbcFileToolStripMenuItem1.Click += new System.EventHandler(this.dBCFileToolStripMenuItem_Click);
            // 
            // AboutToolStripMenuItem
            // 
            this.AboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.VersionToolStripMenuItem});
            this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            resources.ApplyResources(this.AboutToolStripMenuItem, "AboutToolStripMenuItem");
            // 
            // VersionToolStripMenuItem
            // 
            this.VersionToolStripMenuItem.Name = "VersionToolStripMenuItem";
            resources.ApplyResources(this.VersionToolStripMenuItem, "VersionToolStripMenuItem");
            this.VersionToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "close.png");
            this.imageList1.Images.SetKeyName(1, "menu.png");
            this.imageList1.Images.SetKeyName(2, "notifier.png");
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.contextMenuStrip_TreeView.ResumeLayout(false);
            this.cmsFormItem.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeViewRoot;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_TreeView;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openProjectToolStripMenuItem;
        private System.Windows.Forms.Button btnProjectSave;
        private System.Windows.Forms.ContextMenuStrip cmsFormItem;
        private System.Windows.Forms.ToolStripMenuItem deleteFormToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ToolStripMenuItem modifiedProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dBCFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFormToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripMenuItem 工程ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ImportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem VersionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editFormToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem otherToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem historyDataCurveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dbcFileToolStripMenuItem1;
        private System.Windows.Forms.ImageList imageList1;
    }
}