
namespace AppTest.FormType
{
    partial class BaseDataForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseDataForm));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addSignalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.isSaveDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mDIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panelContent = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addSignalToolStripMenuItem,
            this.autoSizeToolStripMenuItem,
            this.isSaveDataToolStripMenuItem,
            this.mDIToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // addSignalToolStripMenuItem
            // 
            this.addSignalToolStripMenuItem.Name = "addSignalToolStripMenuItem";
            resources.ApplyResources(this.addSignalToolStripMenuItem, "addSignalToolStripMenuItem");
            this.addSignalToolStripMenuItem.Click += new System.EventHandler(this.addSignalToolStripMenuItem_Click);
            // 
            // autoSizeToolStripMenuItem
            // 
            this.autoSizeToolStripMenuItem.Name = "autoSizeToolStripMenuItem";
            resources.ApplyResources(this.autoSizeToolStripMenuItem, "autoSizeToolStripMenuItem");
            this.autoSizeToolStripMenuItem.Click += new System.EventHandler(this.autoSizeToolStripMenuItem_Click);
            // 
            // isSaveDataToolStripMenuItem
            // 
            this.isSaveDataToolStripMenuItem.Name = "isSaveDataToolStripMenuItem";
            resources.ApplyResources(this.isSaveDataToolStripMenuItem, "isSaveDataToolStripMenuItem");
            this.isSaveDataToolStripMenuItem.Click += new System.EventHandler(this.isSaveDataToolStripMenuItem_Click);
            // 
            // mDIToolStripMenuItem
            // 
            this.mDIToolStripMenuItem.Checked = true;
            this.mDIToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mDIToolStripMenuItem.Name = "mDIToolStripMenuItem";
            resources.ApplyResources(this.mDIToolStripMenuItem, "mDIToolStripMenuItem");
            this.mDIToolStripMenuItem.Click += new System.EventHandler(this.mDIToolStripMenuItem_Click);
            // 
            // panelContent
            // 
            resources.ApplyResources(this.panelContent, "panelContent");
            this.panelContent.Name = "panelContent";
            // 
            // statusStrip1
            // 
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // BaseDataForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.statusStrip1);
            this.Name = "BaseDataForm";
            this.ShowIcon = false;
            this.Style = MetroFramework.MetroColorStyle.Teal;
            this.Theme = MetroFramework.MetroThemeStyle.Default;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BaseDataForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BaseDataForm_FormClosed);
            this.ResizeEnd += new System.EventHandler(this.BaseForm_ResizeEnd);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.BaseDataForm_MouseClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BaseDataForm_MouseMove);
            this.Resize += new System.EventHandler(this.BaseForm_Resize);
            this.StyleChanged += new System.EventHandler(this.BaseForm_SizeChanged);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addSignalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem isSaveDataToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem mDIToolStripMenuItem;
        public System.Windows.Forms.Panel panelContent;
        public System.Windows.Forms.StatusStrip statusStrip1;
    }
}