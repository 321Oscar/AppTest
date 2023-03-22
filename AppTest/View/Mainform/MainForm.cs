using AppTest.FormType;
using AppTest.FormType.Helper;
using AppTest.Helper;
using AppTest.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppTest
{
    public partial class MainForm : BaseForm
    {
        private readonly string ProjectPath = Application.StartupPath + "\\Config\\Project.json";
        /// <summary>
        /// 
        /// </summary>
        readonly TabControlDemo tabControlDemo;
        public Root root;

        public MainForm()
        {
            InitializeComponent();
            this.CenterToScreen();
            this.Icon = Icon.FromHandle(Global.IconHandle_ProjectCenter);

            tabControlDemo = new TabControlDemo();
            tabControlDemo.Dock = DockStyle.Fill;
            tabControlDemo.BackColor = Color.FromArgb(127, 125, 124);
            this.panel1.Controls.Add(tabControlDemo);

            InitRoot(ProjectPath);
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (treeViewRoot.SelectedNode.Tag is FormItem)
            {
                AddControl_FormItemToProject(treeViewRoot.SelectedNode);
            }
            else if (treeViewRoot.SelectedNode.Tag is ProjectItem)
            {
                openProjectToolStripMenuItem_Click(sender,e);
            }
        }

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (treeViewRoot.SelectedNode != null && treeViewRoot.SelectedNode.Tag is ProjectItem)
                {
                    treeViewRoot.ContextMenuStrip = contextMenuStrip_TreeView;
                    modifiedProjectToolStripMenuItem.Enabled = deleteToolStripMenuItem.Enabled = openProjectToolStripMenuItem.Enabled = true;
                }
                else if (treeViewRoot.SelectedNode != null && treeViewRoot.SelectedNode.Tag is FormItem)
                {
                    treeViewRoot.ContextMenuStrip = cmsFormItem;
                }
                else
                {
                    treeViewRoot.ContextMenuStrip = contextMenuStrip_TreeView;
                }
            }
        }

        /// <summary>
        /// Project 中Form添加事件的委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void TabPageProject_ProjectFormChanged(object sender, EventArgs args)
        {
            LoadTreeView();
        }

        /// <summary>
        /// Project中添加打开Form
        /// </summary>
        /// <param name="treeNode">FormItem 节点</param>
        private void AddControl_FormItemToProject(TreeNode treeNode)
        {
            FormItem formItem = treeNode.Tag as FormItem;
            TreeNode tnParent = FindProjectItemParentNode(treeNode);

            BaseDataForm userForm = FormCreateHelper.CreateForm(formItem, (ProjectItem)tnParent.Tag);
            if (userForm == null)
                return;
            
            //添加到父节点的tabcontrol中
            ProjectPanelUC tabPage = FindTabPageByName(tabControlDemo, tnParent.Text);
            if(tabPage == null)
            {
                MessageBox.Show(tnParent.Text + "未打开");
                return;
            }
            //焦点到该Form的tab上 
            tabControlDemo.ShowTabPage(tnParent.Text);

            tabPage.AddControl(userForm);

            userForm.ShowAndLoad();
        }

        private void dBCFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //throw new Exception("GG 挂了");
            DBCForm form = new DBCForm();
            form.Show();
        }

        #region Project
        private void importProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择导入文件";
            openFileDialog.Filter = "JSON文件|*.json";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                InitRoot(openFileDialog.FileName);
            }
        }

        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null != treeViewRoot.SelectedNode && treeViewRoot.SelectedNode.Tag is ProjectItem)
            {
                ProjectItem pi = treeViewRoot.SelectedNode.Tag as ProjectItem;

                if (this.tabControlDemo.TabPages.ContainsKey(pi.Name))
                {
                    //log4net.ILog log = log4net.LogManager.GetLogger("Log");
                    //LogHelper.Warn("Info Log");
                    //log.Error("Error Log");
                    LeapMessageBox.Instance.ShowInfo(pi.Name + "已经打开了");
                    tabControlDemo.ShowTabPage(pi.Name);
                }
                else
                {
                    ProjectPanelUC tabPageProject = new ProjectPanelUC();
                    tabPageProject.ProjectItem = pi;
                    tabPageProject.Text = pi.Name;
                    tabPageProject.Name = pi.Name;
                    tabPageProject.ProjectFormChanged += TabPageProject_ProjectFormChanged;

                    tabControlDemo.TabPages.Add(tabPageProject);
                    tabControlDemo.ShowTabPage(pi.Name);
                    foreach (TreeNode item in treeViewRoot.SelectedNode.Nodes)
                    {
                        if (item.Tag is FormItem)
                        {
                            AddControl_FormItemToProject(item);
                        }
                    }
                }
            }
        }

        private void exportProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (!Directory.Exists(Application.StartupPath + "\\Config"))
            //{
            //    Directory.CreateDirectory(Application.StartupPath + "\\Config");
            //}

            //if (!File.Exists(ProjectPath))
            //{
            //    using (File.Create(ProjectPath))
            //    {
            //    }
            //}
            string SavePath;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Json文件(*.json)|*.json";
            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                SavePath = (saveFileDialog.FileName);
                string prpjectJson = JsonConvert.SerializeObject(root);

                File.WriteAllText(SavePath, prpjectJson);

                LeapMessageBox.Instance.ShowInfo("保存成功。地址：" + SavePath);
            }
            
        }
        /// <summary>
        /// 增加Project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddProjectForm addProjectForm = new AddProjectForm();
            addProjectForm.StartPosition = FormStartPosition.CenterParent;
            if (addProjectForm.ShowDialog() == DialogResult.OK)
            {
                if (root == null)
                    root = new Root();

                root.project.Add(addProjectForm.ProjectItem);


                TreeNode projectTN = new TreeNode();
                projectTN.Text = addProjectForm.ProjectItem.Name;
                projectTN.Tag = addProjectForm.ProjectItem;
                treeViewRoot.Nodes.Add(projectTN);
                //LoadTreeView();

                LeapMessageBox.Instance.ShowInfo("Project添加成功。");
            }
        }

        private void btnProjectSave_Click(object sender, EventArgs e)
        {
            //throw new Exception("无效按钮");
            
            if (!Directory.Exists(Application.StartupPath + "\\Config"))
            {
                Directory.CreateDirectory(Application.StartupPath + "\\Config");
            }

            if (!File.Exists(ProjectPath))
            {
                using (File.Create(ProjectPath))
                {
                }
            }

            string prpjectJson = JsonConvert.SerializeObject(root);

            File.WriteAllText(ProjectPath, prpjectJson);

            LeapMessageBox.Instance.ShowInfo("保存成功。地址：" + ProjectPath);

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.treeViewRoot.SelectedNode != null && this.treeViewRoot.SelectedNode.Tag is ProjectItem)
            {
                if (MessageBox.Show(string.Format("确认删除【{0}】方案吗？", treeViewRoot.SelectedNode.Text), "删除不可恢复", MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    root.project.Remove(this.treeViewRoot.SelectedNode.Tag as ProjectItem);
                    treeViewRoot.Nodes.Remove(treeViewRoot.SelectedNode);
                    //LoadTreeView();

                    LeapMessageBox.Instance.ShowInfo("Project删除成功。");
                }

                
            }
        }

        private void modifiedProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddProjectForm addProjectForm = new AddProjectForm(this.treeViewRoot.SelectedNode.Tag as ProjectItem);
            if (addProjectForm.ShowDialog() == DialogResult.OK)
            {
                if (root == null)
                {
                    root = new Root();
                    root.project.Add(addProjectForm.ProjectItem);
                }
                else
                {
                    ProjectItem item = root.project.Find(x => x.Name == addProjectForm.ProjectItem.Name);
                    item.Copy(addProjectForm.ProjectItem);
                }
                LeapMessageBox.Instance.ShowInfo(String.Format("修改成功，请重新打开【{0}】", addProjectForm.ProjectItem.Name));
                LoadTreeView();
            }
        }
        #endregion

        #region Form
        private void openFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeViewRoot.SelectedNode.Tag is FormItem)
            {
                AddControl_FormItemToProject(treeViewRoot.SelectedNode);
            }
        }

        private void deleteFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.treeViewRoot.SelectedNode.Tag is FormItem)
            {
                ProjectItem project = FindProjectItemParentNode(treeViewRoot.SelectedNode).Tag as ProjectItem;
                if (MessageBox.Show(string.Format("确认删除【{0}】窗口吗？", treeViewRoot.SelectedNode.Text), "删除不可恢复", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    root.project.Find(x => x.Name == project.Name).Form.Remove((FormItem)treeViewRoot.SelectedNode.Tag);

                    treeViewRoot.Nodes.Remove(treeViewRoot.SelectedNode);
                    //LoadTreeView();

                    LeapMessageBox.Instance.ShowInfo("窗口删除成功。");
                }

            }
        }

        #endregion


        #region Priavte Method

        /// <summary>
        /// 记载Json文件--Project配置
        /// </summary>
        /// <param name="fileName">文件路径</param>
        private void InitRoot(string fileName)
        {
            string path = fileName;

            string jsonStr;

            try
            {
                FileStream fs = null;
                StreamReader sr = null;

                try
                {
                    fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    sr = new StreamReader(fs, System.Text.Encoding.UTF8);

                    jsonStr = sr.ReadToEnd();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (sr != null)
                    {
                        sr.Close();
                    }
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }

                if (root == null)
                {
                    root = JsonConvert.DeserializeObject<Root>(jsonStr);
                }
                else
                {
                    Root rImport = JsonConvert.DeserializeObject<Root>(jsonStr);
                    foreach (var project in rImport.project)
                    {
                        if (root.project.Find(x => x.Name == project.Name) == null)
                        {
                            root.project.Add(project);
                        }
                    }
                }

                LoadTreeView();
            }
            catch (Exception ex)
            {
                LeapMessageBox.Instance.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// 根据ProjectName查找TabPage
        /// </summary>
        /// <param name="tc"></param>
        /// <param name="name">ProjectName</param>
        /// <returns></returns>
        private ProjectPanelUC FindTabPageByName(TabControl tc, string name)
        {
            if (tc == null || tc.TabPages == null || tc.TabPages.Count == 0)
                return null;
            else
            {
                foreach (TabPage item in tc.TabPages)
                {
                    if (item.Name == name)
                        return item as ProjectPanelUC;
                }
            }
            return null;
        }

        /// <summary>
        /// 查找Project父节点
        /// </summary>
        /// <param name="tn"></param>
        /// <returns></returns>
        private TreeNode FindProjectItemParentNode(TreeNode tn)
        {
            if (tn.Parent == null)
            {
                return tn.Parent;
            }
            if (tn.Parent.Tag is ProjectItem)
            {
                return tn.Parent;
            }
            else
            {
                return FindProjectItemParentNode(tn.Parent);
            }
        }

        /// <summary>
        /// TreeView 加载
        /// </summary>
        private void LoadTreeView()
        {
            if (null == root)
                return;

            //if()
            treeViewRoot.Nodes.Clear();

            foreach (var project in root.project)
            {
                TreeNode projectTN = new TreeNode();
                projectTN.Text = project.Name;
                projectTN.Tag = project;
                treeViewRoot.Nodes.Add(projectTN);

                if (project.Form == null || project.Form.Count == 0)
                    continue;

                foreach (var form in project.Form)
                {
                    TreeNode tn = new TreeNode();
                    tn.Text = form.Name;
                    tn.Tag = form;
                    treeViewRoot.Nodes[treeViewRoot.Nodes.Count - 1].Nodes.Add(tn);
                }
            }

        }

        #endregion

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("保存配置文件吗", "提示", MessageBoxButtons.YesNoCancel,MessageBoxIcon.Information);
            if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }
            else if (result == DialogResult.Yes) {
                btnProjectSave_Click(null, null);
            }
            ///close can
            USBCanManager.Instance.Close();
            Dispose();
            Application.Exit();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm af = new AboutForm();
            af.StartPosition = FormStartPosition.CenterParent;
            af.ShowDialog();
        }

        private void editFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.treeViewRoot.SelectedNode.Tag is FormItem)
            {
                ProjectItem projectItem = FindProjectItemParentNode(treeViewRoot.SelectedNode).Tag as ProjectItem;
                FormItem form = treeViewRoot.SelectedNode.Tag as FormItem;
                AddNewForm editForm = new AddNewForm(projectItem, projectItem.Form.Find(x => x.Name == form.Name), true);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    //Root.project.Find(x => x.Name == projectItem.Name).Form.Remove(form);
                    //Root.project.Find(x => x.Name == projectItem.Name).Form.Add(editForm.FormItem);
                    LoadTreeView();
                    //treeViewRoot.Nodes.Remove(treeViewRoot.SelectedNode);
                    //reload Signals
                    //this.Signals = editForm.FormItem.Singals;
                    //ReLoadSignal();
                }
            }
        }

        private void historyDataCurveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HistoryCurveForm hcF = new HistoryCurveForm(true);
            hcF.Show();
        }

        private void treeViewRoot_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            e.DrawDefault = false;
            Graphics g = e.Graphics;
            SolidBrush txtBrush = new SolidBrush(Color.FromArgb(30, 123, 182));
            SolidBrush bacBrush = new SolidBrush(Color.FromArgb(243, 243, 243));

            SolidBrush txtBrush1 = new SolidBrush(Color.FromArgb(46, 45, 43));
            SolidBrush bacBrush1 = new SolidBrush(Color.FromArgb(255, 255, 255));

            int picSize = 25;
            int picX = e.Bounds.X + 20;
            int picY = e.Bounds.Y + treeViewRoot.ItemHeight / 5;

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;
            Font t = e.Node.Parent == null? treeViewRoot.Font : new Font(treeViewRoot.Font.FontFamily, treeViewRoot.Font.Size - 5);
            if (e.Node.IsSelected)
            {
                //节点背景
                g.FillRectangle(bacBrush, e.Bounds);

                //节点名称
                g.DrawString(e.Node.Text, t, txtBrush, e.Bounds, sf);

                //g.DrawLine(new Pen(txtBrush, 10), e.Bounds.X, e.Bounds.Y, e.Bounds.X, e.Bounds.Y + e.Bounds.Width);
            }
            else
            {
                g.FillRectangle(bacBrush1, e.Bounds);

                g.DrawString(e.Node.Text, t, txtBrush1, e.Bounds, sf);
            }

            var pen = new Pen(Brushes.LightGray, 1);
            g.DrawRectangle(pen, e.Bounds);

            //if (imageList1.Images.Count > 0)
            //{
            //    g.DrawImage(imageList1.Images[e.Node.ImageIndex], picX, picY, picSize, picSize);
            //}

            txtBrush.Dispose();
            bacBrush.Dispose();
            txtBrush1.Dispose();
            bacBrush1.Dispose();
            pen.Dispose();
            sf.Dispose();
            g.Dispose();
        }
    }
}
