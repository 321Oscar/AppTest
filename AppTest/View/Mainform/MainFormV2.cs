using AppTest.FormType;
using AppTest.FormType.Helper;
using AppTest.Helper;
using AppTest.Model;
using AppTest.View.FormType;
using ELFTest;
using LPCanControl.UDS;
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
    public partial class MainFormV2 : BaseForm
    {
        private Root Root;

        private List<ProjectForm> openedProject;

        public MainFormV2():base()
        {
            InitializeComponent();
            //this.AutoScaleMode = AutoScaleMode.Dpi;
            //this.Font = Global.CurrentFont;
            InitMain();
            this.Icon = Icon.FromHandle(Global.IconHandle_ProjectCenter);/*global::AppTest.Properties.Resources.导入*/;

            treeViewRoot.FullRowSelect = false;

            UDSHelper.lp_logInit();
        }


        #region Event

       
        private void TreeViewRoot_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (treeViewRoot.SelectedNode != null && treeViewRoot.SelectedNode.Tag is ProjectItem)
                {
                    treeViewRoot.ContextMenuStrip = contextMenuStrip_Project;
                    //modifiedProjectToolStripMenuItem.Enabled = deleteToolStripMenuItem.Enabled = openProjectToolStripMenuItem.Enabled = true;
                }
                else if (treeViewRoot.SelectedNode != null && treeViewRoot.SelectedNode.Tag is FormItem)
                {
                    treeViewRoot.ContextMenuStrip = contextMenuStrip_Form;
                }
                else
                {
                    treeViewRoot.ContextMenuStrip = contextMenuStrip_Project;
                }
            }
        }
       

        private void MainFormV2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(openedProject.Count > 0)
            {
                DialogResult close = MessageBox.Show("还有未关闭的Project，确认关闭软件吗", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (close == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    int count = openedProject.Count;
                    while (openedProject.Count  > 0)
                    {
                        openedProject[0].Close();
                        if(count == openedProject.Count)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            }

            DialogResult result = MessageBox.Show("保存配置文件吗", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }
            else if (result == DialogResult.Yes)
            {
                saveToolStripMenuItem_Click(null, null);
            }
            //关闭数据库连接
            DBHelper.DisposeConnect();

            DialogResult resultLog = MessageBox.Show("保留本次Log文件吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (resultLog == DialogResult.No)
            {
                //删除日志
                string logPath = Application.StartupPath + "\\Log";

                if (Directory.Exists(logPath))
                {
                    Directory.GetFiles(logPath).ToList().ForEach(x =>
                    {
                        try
                        {
                            File.Delete(x);
                        }
                        catch{}
                    });

                    Directory.GetDirectories(logPath).ToList().ForEach(x =>
                    {
                        try
                        {
                            Directory.Delete(x, true);
                        }
                        catch { }

                    });
                }

                //删除db数据
                string dbFile = $"{Environment.CurrentDirectory}{Global.DBPATH}";

                if (File.Exists(dbFile))
                {
                    try
                    {
                        File.Delete(dbFile);
                    }
                    catch(Exception ex) { }
                }
            }

            ///close can
            USBCanManager.Instance.Close();
            this.Dispose();
            Application.Exit();
            System.Environment.Exit(0);
        }

        private void DBCFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DBCForm dBCForm = new DBCForm();
            dBCForm.ShowDialog();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshToolStripMenuItem_Click(null, null);
            //判断目录
            if (!Directory.Exists(Application.StartupPath + "\\Config"))
            {
                Directory.CreateDirectory(Application.StartupPath + "\\Config");
            }

            //创建文件
            if (!File.Exists(Global.ProjectPath))
            {
                using (File.Create(Global.ProjectPath))
                {
                }
            }

            string prpjectJson = JsonConvert.SerializeObject(Root);
            //写入数据
            File.WriteAllText(Global.ProjectPath, prpjectJson);

            LeapMessageBox.Instance.ShowInfo("保存成功。地址：" + Global.ProjectPath);
        }

        private void ImportProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择导入文件";
            openFileDialog.Filter = "JSON文件|*.json";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Root = RootHelper.InitRootByJson(path: openFileDialog.FileName, oldRoot: Root);
                LoadTreeView();
                //InitRoot();
            }
        }

        private void ExportProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string SavePath;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Json文件(*.json)|*.json";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                SavePath = (saveFileDialog.FileName);
                string prpjectJson = JsonConvert.SerializeObject(Root);

                File.WriteAllText(SavePath, prpjectJson);

                LeapMessageBox.Instance.ShowInfo("保存成功。地址：" + SavePath);
            }
        }

        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadTreeView();
        }
        #endregion

        #region Project

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null != treeViewRoot.SelectedNode && treeViewRoot.SelectedNode.Tag is ProjectItem)
            {
                ProjectItem pi = treeViewRoot.SelectedNode.Tag as ProjectItem;

                ProjectForm projectForm = new ProjectForm(pi);
                int x = this.Location.X + this.Width - 20;
                int y = this.Location.Y;
                projectForm.StartX = x;
                projectForm.StartY = y;
                projectForm.Icon = Icon.FromHandle(Global.IconHandle_Project);

                projectForm.ProjectClosing += ProjectForm_ProjectClosing;

                projectForm.Show();
                openedProject.Add(projectForm);
            }

        }

        private void ProjectForm_ProjectClosing(ProjectForm projectForm)
        {
            projectForm.Dispose();
            openedProject.Remove(projectForm);
        }

        private void NewProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddProjectForm addProjectForm = new AddProjectForm();
            addProjectForm.StartPosition = FormStartPosition.CenterParent;
            if (addProjectForm.ShowDialog() == DialogResult.OK)
            {
                if (Root == null)
                    Root = new Root();

                Root.project.Add(addProjectForm.ProjectItem);


                TreeNode projectTN = new TreeNode();
                projectTN.Text = addProjectForm.ProjectItem.Name;
                projectTN.Tag = addProjectForm.ProjectItem;
                treeViewRoot.Nodes.Add(projectTN);
                //LoadTreeView();

                LeapMessageBox.Instance.ShowInfo("Project添加成功。");
            }
        }

        private void EditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddProjectForm addProjectForm = new AddProjectForm(this.treeViewRoot.SelectedNode.Tag as ProjectItem);
            if (addProjectForm.ShowDialog() == DialogResult.OK)
            {
                //引用类型-已经直接修改了Root中的值，重新加载一边root即可
                //if (Root == null)
                //{
                //    Root = new Root();
                //    Root.project.Add(addProjectForm.ProjectItem);
                //}
                //else
                //{
                //    ProjectItem item = Root.project.Find(x => x.Name == addProjectForm.ProjectItem.Name);
                //    item.Copy(addProjectForm.ProjectItem);
                //}
                LeapMessageBox.Instance.ShowInfo(String.Format("修改成功，请重新打开【{0}】", addProjectForm.ProjectItem.Name));
                LoadTreeView();
            }
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.treeViewRoot.SelectedNode != null && this.treeViewRoot.SelectedNode.Tag is ProjectItem)
            {
                if (MessageBox.Show(string.Format("确认删除【{0}】方案吗？", treeViewRoot.SelectedNode.Text), "删除不可恢复", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    Root.project.Remove(this.treeViewRoot.SelectedNode.Tag as ProjectItem);
                    treeViewRoot.Nodes.Remove(treeViewRoot.SelectedNode);
                    //LoadTreeView();

                    LeapMessageBox.Instance.ShowInfo("Project删除成功。");
                }
            }
        }

        #endregion

        #region Form 

        private void OpenFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeViewRoot.SelectedNode.Tag is FormItem)
            {
                //AddControl_FormItemToProject(treeViewRoot.SelectedNode);
                ///find project
                ///if project does not open
                ///open project and this form
                ///else 
                ///open form
            }
        }

        private void DeleteFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.treeViewRoot.SelectedNode.Tag is FormItem)
            {
                ProjectItem project = FindProjectItemParentNode(treeViewRoot.SelectedNode).Tag as ProjectItem;
                if (MessageBox.Show(string.Format("确认删除【{0}】窗口吗？", treeViewRoot.SelectedNode.Text), "删除不可恢复", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    Root.project.Find(x => x.Name == project.Name).Form.Remove((FormItem)treeViewRoot.SelectedNode.Tag);

                    treeViewRoot.Nodes.Remove(treeViewRoot.SelectedNode);
                    //LoadTreeView();

                    LeapMessageBox.Instance.ShowInfo("窗口删除成功。");
                }

            }
        }

        private void EditFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.treeViewRoot.SelectedNode.Tag is FormItem )
            {
                ProjectItem projectItem = FindProjectItemParentNode(treeViewRoot.SelectedNode).Tag as ProjectItem;
                FormItem form = treeViewRoot.SelectedNode.Tag as FormItem;
                AddNewForm editForm;// = new AddNewForm(projectItem, projectItem.Form.Find(x => x.Name == form.Name), true);
                if (projectItem.CanIndex[0].ProtocolType == (int)ProtocolType.DBC)
                    editForm = new AddNewForm(projectItem, form);
                else if (projectItem.CanIndex[0].ProtocolType == (int)ProtocolType.XCP)
                    editForm = new AddNewXcpForm(projectItem, form);
                else return;
                
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

        private void NewFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProjectItem projectItem;
            if (this.treeViewRoot.SelectedNode.Tag is FormItem)
            {
                projectItem = FindProjectItemParentNode(treeViewRoot.SelectedNode).Tag as ProjectItem;

            }
            else if(this.treeViewRoot.SelectedNode.Tag is ProjectItem)
            {
                projectItem = this.treeViewRoot.SelectedNode.Tag as ProjectItem;
            }
            else
            {
                return;
            }

            AddNewForm addNewForm;// = new AddNewForm(projectItem, projectItem.Form.Find(x => x.Name == form.Name), true);
            if (projectItem.CanIndex[0].ProtocolType == (int)ProtocolType.DBC)
                addNewForm = new AddNewForm(projectItem);
            else if (projectItem.CanIndex[0].ProtocolType == (int)ProtocolType.XCP)
                addNewForm = new AddNewXcpForm(projectItem);
            else return;
            addNewForm.StartPosition = FormStartPosition.CenterParent;
            if (addNewForm.ShowDialog() == DialogResult.OK)
            {
                //
                if (projectItem.Form.Find(x => x.Name == addNewForm.FormItem.Name) != null)
                {
                    MessageBox.Show("名称重复");
                    return;
                }
                projectItem.Form.Add(addNewForm.FormItem);
                LoadTreeView();
            }
        }

        #endregion


        private void InitMain()
        {
            this.MaximizeBox = false;
            Root = RootHelper.InitRootByJson(path: Global.ProjectPath);
            LoadTreeView();
            openedProject = new List<ProjectForm>();
        }

        /// <summary>
        /// TreeView 加载
        /// </summary>
        private void LoadTreeView()
        {
            if (null == Root)
                return;

            //if()
            treeViewRoot.Nodes.Clear();

            foreach (var project in Root.project)
            {
                TreeNode projectTN = new TreeNode();
                projectTN.Text = project.Name;
                projectTN.Tag = project;
                projectTN.ImageIndex = 4;
                projectTN.SelectedImageIndex = 4;
                treeViewRoot.Nodes.Add(projectTN);

                if (project.Form == null || project.Form.Count == 0)
                    continue;

                project.Form.Sort();
                foreach (var form in project.Form)
                {
                    TreeNode tn = new TreeNode();
                    tn.Text = form.Name;
                    tn.Tag = form;
                    tn.ImageIndex = form.FormType;
                    tn.SelectedImageIndex = form.FormType;
                    treeViewRoot.Nodes[treeViewRoot.Nodes.Count - 1].Nodes.Add(tn);
                }
            }

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

        private void historyMeasureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HistoryCurveForm hcF = new HistoryCurveForm(true);
            hcF.Show();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.Font = Global.CurrentFont;
            if(fd.ShowDialog() == DialogResult.OK)
            {
                Global.CurrentFont = fd.Font;
            }
        }

        private void versionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm af = new AboutForm();
            af.StartPosition = FormStartPosition.CenterParent;
            af.ShowDialog();
        }

        private void dBCFileToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            DBCForm form = new DBCForm();
            form.Show();
        }

        private void reStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogHelper.Info("Info Txt");
            LogHelper.WriteToOutput("outputTest","Debug txt");
            //Application.Restart();
        }

        private void showLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string logPath = Path.Combine(Application.StartupPath, "Log");
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
            psi.Arguments = "/e,/select," + logPath;
            System.Diagnostics.Process.Start("Explorer.exe", logPath);
        }

        private void treeViewRoot_DoubleClick(object sender, EventArgs e)
        {
            if (treeViewRoot.SelectedNode.Tag is FormItem)
            {
                //AddControl_FormItemToProject(treeViewRoot.SelectedNode);
            }
            else if (treeViewRoot.SelectedNode.Tag is ProjectItem)
            {
                //openProjectToolStripMenuItem_Click(sender, e);
                OpenToolStripMenuItem_Click(sender, e); 
            }
        }

        private void showLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogHelper.LogForm.Show();
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var configAsync = DBHelper.GetAuthenticationDb();
            //configAsync.Result.AuthenticationEntities.CountAsync();

            LicenseForm licenseForm = new LicenseForm();
            licenseForm.Show();
        }

        private void treeViewRoot_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            return;
            e.DrawDefault = false;
            Graphics g = e.Graphics;
            SolidBrush txtBrush = new SolidBrush(Color.FromArgb(255, 255, 255));//选中的文本颜色
            SolidBrush bacBrush = new SolidBrush(Color.FromArgb(224, 224, 224));//选中的背景颜色

            SolidBrush txtBrush1 = new SolidBrush(Color.FromArgb(46, 45, 43));//未选中的文本颜色
            SolidBrush bacBrush1 = new SolidBrush(Color.FromArgb(245, 245, 245));//未选中的背景颜色

            treeViewRoot.ItemHeight = 50;

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            Font t = treeViewRoot.Font;

            int picSize = 15;
            int picX = e.Bounds.X + 20;
            int picY = e.Bounds.Y + treeViewRoot.ItemHeight / 3;
            if (e.Node.Parent != null) 
            {
                t = new Font(treeViewRoot.Font.FontFamily, treeViewRoot.Font.Size - 2);
                picX += 10;
                //treeViewRoot.Indent = 30;
                bacBrush1 = new SolidBrush(Color.FromArgb(250, 250, 250));
            }
            

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

            var pen = new Pen(Brushes.White, 2);
            g.DrawRectangle(pen, e.Bounds);

            
            if (imageList1.Images.Count > 0)
            {
                g.DrawImage(imageList1.Images[e.Node.ImageIndex], picX, picY, picSize, picSize);
            }

            txtBrush.Dispose();
            bacBrush.Dispose();
            txtBrush1.Dispose();
            bacBrush1.Dispose();
            pen.Dispose();
            sf.Dispose();
            g.Dispose();
        }

        private void treeViewRoot_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            return;
            if(e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (e.Node.IsExpanded)
                {
                    e.Node.Collapse();
                }
                else
                {
                    e.Node.Expand();
                }
                //if (e.Node.Bounds.Contains(e.Location))
                //{

                //}
            }
            //this.SelectedNode = e.Node;
        }

        private void treeViewRoot_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void aSCFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ASCForm asc = new ASCForm();
            asc.Show();
        }

        private void eLFAndA2LFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ELFParseForm eLFParseForm = new ELFParseForm();
            eLFParseForm.Show();
        }
    }
}
