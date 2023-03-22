using AppTest.FormType;
using AppTest.FormType.Helper;
using AppTest.Helper;
using AppTest.ProjectClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppTest
{
    public partial class ProjectForm : BaseForm
    {
        #region 打开动画


        /// <summary>
        /// 激活窗口，在使用AW_HIDE标志后不要使用这个标志
        /// </summary>
        private const int AW_ACTIVE = 0x20000;

        /// <summary>
        /// 使用淡入淡出效果
        /// </summary>
        private const int AW_BLEND = 0x80000;

        /// <summary>
        /// 若使用了SW_HIDE标志，则窗口向内重叠，否则向外拓展
        /// </summary>
        private const int AW_CNETER = 0x0010;

        /// <summary>
        /// 隐藏窗口
        /// </summary>
        private const int AW_HIDE = 0x10000;

        /// <summary>
        /// 自右向左显示窗口，该表示可以再滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志
        /// </summary>
        private const int AW_HOR_NEGATIVE = 0x002;

        /// <summary>
        /// 自左向右显示窗口，该表示可以再滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志
        /// </summary>
        private const int AW_HOR_POSITIVE = 0x001;

        //隐藏窗口
        //激活窗口，在使用AW_HIDE标志后不要使用这个标志
        /// <summary>
        /// 使用滑动类型动画效果，默认为滚动动画效果，当使用AW_CENTER标志时，这个标志就被忽略
        /// </summary>
        private const int AW_SLIDE = 0x40000;

        /// <summary>
        /// 自下向上显示窗口，该表示可以再滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志
        /// </summary>
        private const int AW_VER_NEGATIVE = 0x008;

        /// <summary>
        /// 自顶向下显示窗口，该表示可以再滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志
        /// </summary>
        private const int AW_VER_POSITIVE = 0x004;

        /// <summary>
        /// 函数功能：该函数能在显示与隐藏窗口时能产生特殊的效果。有两种类型的动画效果：滚动动画和滑动动画。
        /// <para>函数原型：BOOL AnimateWindow(HWND hWnd,DWORD dwTime,DWORD dwFlags)</para>
        /// <para>hWnd:指定产生动画的窗口的句柄</para>
        /// <para>dwTime:指明动画持续的时间（以微秒计），完成一个动画的标准时间为200微秒。</para>
        /// <para>dwFlags:指明动画类型。这个参数可以是一个或多个下列标志的组合</para>
        /// </summary>
        /// <param name="hwnd">指定产生动画的窗口的句柄</param>
        /// <param name="dwtime">指明动画持续的时间（以微秒计），完成一个动画的标准时间为200微秒。</param>
        /// <param name="flags">指明动画类型。这个参数可以是一个或多个下列标志的组合。</param>
        /// <returns>如果函数成功，返回值为非零；如果函数失败，返回值为零。</returns>
        /// <remarks>
        /// 在下列情况下函数将失败：窗口使用了窗口边界；窗口已经可见仍要显示窗口；窗口已经隐藏仍要隐藏窗口。若想活得更多错误信息，请调用GetLastError函数。
        /// 备注：可以将AW_HOR_POSITIVE或AW_HOR_NEGATIVE与AW_VER_POSITIVE或AW_VER_NEGATIVE组合来激活一个窗口。
        /// 可能需要在该窗口的窗口过程和它的子窗口的窗口过程中处理WM_PRINT或WM_PRINTCLIENT消息。对话框，控制，及公用控制已处理WM_PRINTCLIENT消息，缺省窗口过程也已处理WM_PRINTCLIENT消息。
        /// 速查：WIDDOWS NT：5.0以上版本；Windows：98以上版本；Windows CE：不支持；头文件：Winuser.h；库文件:user32.lib
        /// </remarks>
        [DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwtime, int flags);
        


        #endregion
        public bool CanIsOpen = false;

        public int StartX = 0;

        public int StartY = 0;
        public List<FormItem> openedForm;

        private ProjectItem projectItem;

        public ProjectForm()
        {
            InitializeComponent();
            this.Font = Global.CurrentFont;
            this.IsMdiContainer = true;

            //this.menuStrip1.BackColor = Color.FromArgb(45, 45, 48);
            //this.menuStrip1.ForeColor = Color.FromArgb(241, 241, 241);

            DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            //this.FormBorderStyle = FormBorderStyle.None;

            openedForm = new List<FormItem>();

            
        }
        public ProjectForm(ProjectItem project) : this()
        {
            projectItem = project;
            this.Text = this.Name = project.Name;

            InitStatusBar();

            this.tslbCanStatus.Text = $"{(DeviceType)projectItem.DeviceType}未打开";
        }

        /// <summary>
        /// 关闭Project时，执行的委托
        /// </summary>
        public delegate void CloseProject(ProjectForm projectForm);

        /// <summary>
        /// 关闭Project时，执行的委托
        /// </summary>
        public event CloseProject ProjectClosing;

        public ProjectItem Project
        {
            get { return this.projectItem; }
            set { this.projectItem = value; }
        }

        #region Private Method
        private ToolStripMenuItem AddContextMenu(string text, ToolStripItemCollection cms, EventHandler callback)
        {
            if (text == "-")
            {
                ToolStripSeparator tsp = new ToolStripSeparator();
                cms.Add(tsp);
                return null;

            }
            else if (!string.IsNullOrEmpty(text))
            {
                ToolStripMenuItem tsmi = new ToolStripMenuItem(text);
                tsmi.Tag = text + "TAG";
                if (callback != null)
                    tsmi.Click += callback;
                cms.Add(tsmi);

                return tsmi;
            }

            return null;
        }

        private ToolStripMenuItem AddContextMenu(FormItem form, ToolStripItemCollection cms, EventHandler callback)
        {
            if (form != null)
            {
                ToolStripMenuItem tsmi = new ToolStripMenuItem(form.Name);
                tsmi.Image = imageList1.Images[form.FormType];
                tsmi.Tag = form;
                if (callback != null)
                    tsmi.Click += callback;
                cms.Add(tsmi);

                return tsmi;

            }

            return null;
        }

        /// <summary>
        /// 加载/更新 menu。打开上次打开的form
        /// </summary>
        /// <param name="openForm"></param>
        /// <returns></returns>
        private Task InitForms(bool openForm = false)
        {
            var t = Task.Run(new Action(() =>
            {
                this.Invoke(new Action(() =>
                {
                    projectItem.Form.Sort();
                    OpenFormToolStripMenuItem.DropDownItems.Clear();
                    foreach (FormItem form in projectItem.Form)
                    {
                        if (OpenFormToolStripMenuItem.DropDownItems.Count > 0)
                        {
                            if (form.FormType != (OpenFormToolStripMenuItem.DropDownItems[OpenFormToolStripMenuItem.DropDownItems.Count - 1].Tag as FormItem).FormType)
                            {
                                AddContextMenu("-", OpenFormToolStripMenuItem.DropDownItems, null);
                            }
                        }

                        AddContextMenu(form, OpenFormToolStripMenuItem.DropDownItems, MenuFormOpenClick);

                        //open Form 
                        if (form.IsOpen && openForm)
                        {
                            BaseDataForm userForm = FormCreateHelper.CreateForm(form, this.projectItem);
                            userForm.MdiContainer = this;
                            userForm.MdiParent = this;
                           // userForm.Parent = this;
                            //userForm.Delete += UserForm_Delete;
                            userForm.ShowAndLoad();
                            //openedForm.Add(form);
                        }
                    }

                }));

            }));
            return t;
            
        }

        private void UserForm_Delete(string formName)
        {
            var form = openedForm.Find(x => x.Name == formName);
            if (form != null)
                openedForm.Remove(form);
        }

        #endregion

        #region Event

        private void MenuFormOpenClick(object sender,EventArgs e)
        {
            if(sender is ToolStripMenuItem item)
            {
                
                FormItem formItem = (item.Tag as FormItem);
                BaseDataForm userForm = FormCreateHelper.CreateForm(formItem, this.projectItem);
                //userForm.Delete += UserForm_Delete;
                if (userForm == null)
                    return;
                //BaseFormEqualityComparer signalEqualityComarer = new BaseFormEqualityComparer();
                //string[] s = new string[] { "123", "1" };
                //s.ToList().Contains()

                bool open;
                foreach (var baseform in MdiChildren)
                {
                    if (baseform.Name == userForm.Name)
                    {
                        open = true;
                        baseform.Location = new Point(0, 0);
                        //baseform.Show();
                        baseform.Activate();
                        baseform.WindowState = FormWindowState.Normal;
                        break;
                    }
                }

                open = CheckFormExsit(userForm.Name);
                if (!open)
                {
                    //userForm.Parent = this;
                    //userForm.TopLevel = true;
                    userForm.MdiContainer = this;
                    userForm.MdiParent = this;
                    userForm.ShowAndLoad();
                    openedForm.Add(formItem);
                }
                else
                {
                    LeapMessageBox.Instance.ShowInfo($"【{userForm.Name}】已经打开");
                }

            }
        }

        private void newFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewForm addNewForm = new AddNewForm(this.projectItem);
            addNewForm.StartPosition = FormStartPosition.CenterParent;
            if (addNewForm.ShowDialog() == DialogResult.OK)
            {
                //
                if (this.projectItem.Form.Find(x => x.Name == addNewForm.FormItem.Name) != null)
                {
                    MessageBox.Show("名称重复");
                    return;
                }
                this.projectItem.Form.Add(addNewForm.FormItem);
                //IsAdded = true;
                BaseDataForm userForm = FormCreateHelper.CreateForm(addNewForm.FormItem, projectItem);
                if (userForm != null)
                {
                    userForm.MdiContainer = this;
                    userForm.MdiParent = this;
                    //this.panel2.Controls.Add(userForm);
                    userForm.ShowAndLoad();
                    InitForms();
                }
                else
                {
                    MessageBox.Show("打开【" + addNewForm.FormItem.Name + "】窗口失败");
                }
            }
        }

       
        private void toolStripButton_Connect_Click(object sender, EventArgs e)
        {
            if (!USBCanManager.Instance.Exist(projectItem))
            {
                UsbCan usbCan = new UsbCan(projectItem.DeviceType, projectItem.DeviceIndex, 0);
                USBCanManager.Instance.AddUsbCan(usbCan, projectItem);
            }

            if (CanIsOpen)
            {
                LeapMessageBox.Instance.ShowInfo(this.Name + "，Can口已经打开成功。");
                //if (USBCanManager.Instance.Close(projectItem))
                //{
                //    //btnStartCan.Text = "Start";
                //    CanIsOpen = false;
                //}
            }
            else
            {
                ///can.open 
                if (USBCanManager.Instance.Open(projectItem))
                {
                    List<int> caninds = new List<int>();
                    foreach (var item in projectItem.CanIndex)
                    {
                        try
                        {
                            caninds.Add(item.CanChannel);
                            switch ((DeviceType)projectItem.DeviceType)
                            {
                                case DeviceType.VCI_USBCAN_2E_U:
                                    ///Can.SetReference
                                    USBCanManager.Instance.SetRefenrece(projectItem, item.CanChannel);
                                    break;
                            }
                            ///can.init
                            USBCanManager.Instance.InitCan(projectItem, item.CanChannel);
                            ///can.start
                            USBCanManager.Instance.StartCanIndex(projectItem, item.CanChannel);
                        }
                        catch (Exception ex)
                        {
                            LeapMessageBox.Instance.ShowError("Init Can Error：" + ex.Message);
                            continue;
                        }

                    }
                    CanIsOpen = true;
                    toolStripButton_DisConnect.Enabled = true;
                    //btnStartCan.Text = "Close";
                    LeapMessageBox.Instance.ShowInfo("CAN 打开成功");

                    //启动接收线程
                    this.tslbCanStatus.Text = $"{(DeviceType)projectItem.DeviceType} [{projectItem.CanIndex.Count}] 已打开 ";
                    USBCanManager.Instance.StartRecv(projectItem, caninds.ToArray());//caninds
                    LeapMessageBox.Instance.ShowInfo("CAN 数据接收已启动");
                }
                else
                {
                    this.tslbCanStatus.Text = $"{(DeviceType)projectItem.DeviceType} 打开失败 ";
                    LeapMessageBox.Instance.ShowWarn(tslbCanStatus.Text);
                    USBCanManager.Instance.RemoveUsbCan(projectItem);
                }

            }
        }

        private void toolStripButton_DisConnect_Click(object sender, EventArgs e)
        {
            if (USBCanManager.Instance.Close(projectItem))
            {
                //btnStartCan.Text = "Start";
                CanIsOpen = false;
                toolStripButton_DisConnect.Enabled = false;
                LeapMessageBox.Instance.ShowInfo(this.Name + "，关闭Can成功。");
            }
        }

        private void VerticaltoolStripButton_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(System.Windows.Forms.MdiLayout.TileVertical);
        }

        #endregion

        #region 底部状态栏

        ToolStripComboBox tscbb;
        ToolStripLabel tslbCanStatus;
        ToolStripLabel tslbXcpStatus;
        ToolStripLabel tslbMasterID;
        ToolStripLabel tslbSlaveID;
        ToolStripLabel tslbXCPCmdStatus;

        private void InitStatusBar()
        {
            //CAN通道
            ToolStripLabel tsl = new ToolStripLabel
            {
                Text = "Can通道"
            };
            statusStrip1.Items.Add(tsl);

            tscbb = new ToolStripComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 20
            };
            for (int i = 0; i < projectItem.CanIndex.Count; i++)
            {
                tscbb.Items.Add(projectItem.CanIndex[i].CanChannel);
            }
            tscbb.SelectedIndexChanged += Tscbb_SelectedIndexChanged;
            statusStrip1.Items.Add(tscbb);
            //分隔符
            ToolStripSeparator tss = new ToolStripSeparator();
            statusStrip1.Items.Add(tss);

            //CAN打开状态
            tslbCanStatus = new ToolStripLabel();
            //tslCanStatus.Text = "";
            statusStrip1.Items.Add(tslbCanStatus);
            //分隔符
            ToolStripSeparator tss1 = new ToolStripSeparator();
            statusStrip1.Items.Add(tss1);

            //XCP
            tslbXcpStatus = new ToolStripLabel();
            statusStrip1.Items.Add(tslbXcpStatus);

            tslbMasterID = new ToolStripLabel { Dock = DockStyle.Right };
            statusStrip1.Items.Add(tslbMasterID);

            tslbSlaveID = new ToolStripLabel { Dock = DockStyle.Right };
            statusStrip1.Items.Add(tslbSlaveID);

            tslbXCPCmdStatus = new ToolStripLabel();
            statusStrip1.Items.Add(tslbXCPCmdStatus);

            tscbb.SelectedIndex = 0;
        }

        private void XcpModule_OnConnectStatusChanged(object sender, EventArgs args)
        {
            string status = sender.ToString();
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => { tslbXcpStatus.Text = $"XCP:{status}"; }));
            }
            else
            {
                tslbXcpStatus.Text = $"XCP:{status}";
            }
        }

        private void XcpModule_OnCMDStatusChanged(object sender, EventArgs args)
        {
            string status = sender.ToString();
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => { tslbXCPCmdStatus.Text = $"XCP CMD:{status}"; }));
            }
            else
            {
                tslbXCPCmdStatus.Text = $"XCP CMD:{status}";
            }
        }


        private void Tscbb_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ToolStripComboBox toolStripComboBox = sender as ToolStripComboBox;
            //int index = toolStripComboBox.SelectedIndex;
            //if (projectItem.CanIndex[index].ProtocolType == (int)ProtocolType.XCP)
            //{
            //    tslbXcpStatus.Visible = tslbMasterID.Visible = tslbSlaveID.Visible = true;
            //    tslbMasterID.Text = $"MasterID:0x{projectItem.CanIndex[index].MasterID:X}";
            //    tslbSlaveID.Text = $"SlaveID:0x{projectItem.CanIndex[index].SlaveID:X}";
            //}
            //else
            //{
            //    tslbXcpStatus.Visible = tslbMasterID.Visible = tslbSlaveID.Visible = false;
            //}
        }
        #endregion

        private void ProjectForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (CanIsOpen)
            {
                USBCanManager.Instance.Close(projectItem);
                CanIsOpen = false;
            }
            USBCanManager.Instance.RemoveUsbCan(projectItem);
            if (this.WindowState == FormWindowState.Maximized)
            {
                AnimateWindow(this.Handle, 500, AW_VER_POSITIVE | AW_HIDE | AW_BLEND);
            }
            else
            {
                AnimateWindow(this.Handle, 200, AW_HOR_NEGATIVE | AW_HIDE | AW_SLIDE);
            }
            if (ProjectClosing != null)
            {
                ProjectClosing(this);
            }
        }

        private async void ProjectForm_Load(object sender, EventArgs e)
        {
            //int x = Screen.PrimaryScreen.WorkingArea.Right - this.Width;
            //int y = Screen.PrimaryScreen.WorkingArea.Bottom - this.Height;
            this.Location = new Point(StartX, StartY);
            AnimateWindow(this.Handle, 200, AW_HOR_POSITIVE | AW_SLIDE);
            await InitForms(true);

            //for (int i = 0; i < this.Controls.Count; i++)
            //{
            //    var mdiClientForm = this.Controls[i] as MdiClient;
            //    if (mdiClientForm == null) continue;
            //    // 找到了mdi客户区    
            //    // 取得客户区的边框 
            //    int style = Win32.GetWindowLong(mdiClientForm.Handle, Win32.GWL_STYLE);
            //    int exStyle = Win32.GetWindowLong(mdiClientForm.Handle, Win32.GWL_EXSTYLE);
            //    style &= ~Win32.WS_BORDER;
            //    exStyle &= ~Win32.WS_EX_CLIENTEDGE;

            //    // 调用win32设定样式    
            //    Win32.SetWindowLong(mdiClientForm.Handle, Win32.GWL_STYLE, style);
            //    Win32.SetWindowLong(mdiClientForm.Handle, Win32.GWL_EXSTYLE, exStyle);

            //    // 更新客户区    
            //    Win32.SetWindowPos(mdiClientForm.Handle, IntPtr.Zero, 0, 0, 0, 0,
            //        Win32.SWP_NOACTIVATE | Win32.SWP_NOMOVE | Win32.SWP_NOSIZE | Win32.SWP_NOZORDER |
            //        Win32.SWP_NOOWNERZORDER | Win32.SWP_FRAMECHANGED);


            //    UpdateStyles();
            //    break;
            //}
        }

        private void ProjectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var item in projectItem.Form)
            {
                item.IsOpen = CheckFormExsit(item.Name,true);
            }

            //close form
            var forms = openedForm.Copy();

            foreach (var item in forms)
            {
                var form = Application.OpenForms[item.Name];
                if (form != null)
                    form.Close();
            }
        }

        /// <summary>
        /// 检查Form是否打开
        /// </summary>
        /// <param name="name"></param>
        /// <param name="miniBox">是否包括最小化的窗口，默认包括</param>
        /// <returns></returns>
        private bool CheckFormExsit(string name,bool miniBox = false)
        {
            if (this.MdiChildren.Any(x => x.Name == name))
            {
                if (miniBox && this.MdiChildren.First(x => x.Name == name).WindowState == FormWindowState.Minimized)
                    return false;
                return true;
            }
            return false;

            //if (openedForm.Count > 0)
            //{
            //    if (openedForm.Any(x => x.Name == item.Name))
            //    {
            //        item.IsOpen = true;
            //    }
            //}
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                //cp.ExStyle |= 0x02000000;
                //if (this.isSized)
                //{
                //    cp.ExStyle |= 0x02000000;//导致拖动有残影
                //}
                //else
                //{
                //    cp.ExStyle &= ~0x02000000;
                //}
                return cp;
            }
        }

        //[System.Security.SuppressUnmanagedCodeSecurity]
        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        //public const int WM_MDINEXT = 0x224;

        //public new void ActivateMdiChild(Form childToActivate)
        //{
        //    if (this.ActiveMdiChild != childToActivate)
        //    {
        //        CreateParams cp = base.CreateParams;

        //        //cp.ExStyle |= 0x02000000;
        //        //if (this.isSized)
        //        //{
        //        //    cp.ExStyle |= 0x02000000;//导致拖动有残影
        //        //}
        //        //else
        //        //{
        //        //    cp.ExStyle &= ~0x02000000;
        //        //}
        //        return cp;
        //    }
        //}
        public MdiClient GetCurrentMdiControl()
        {
            foreach (var c in this.Controls)
            {
                if (c is MdiClient)
                {
                    return c as MdiClient;
                }
            }
            return null;
        }

    }
}
