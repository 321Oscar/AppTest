using AppTest.FormType;
using AppTest.FormType.Helper;
using AppTest.Helper;
using AppTest.Model;
using AppTest.ViewModel;
using ELFSharp.ELF;
using ELFSharp.ELF.Sections;
using ELFTest;
using LPCanControl.CANInfo;
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
        private XCPModule xcpModule;
        public XCPModule XcpModule { get => xcpModule; set => xcpModule = value; }
        /// <summary>
        /// XCP Event Name
        /// </summary>
        public List<string> XCPEventName { get; set; }

        public DependencyXCPDAQSignals dependencyXCPDAQSignals;

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

            RemoveMdiBackground();

            toolStripButton_Connect.ToolTipText = "连接CAN";
            toolStripButton_DisConnect.ToolTipText = "断开CAN";
            AddtoolStripButton.ToolTipText = "新增窗口";
            VerticaltoolStripButton.ToolTipText = "铺满";
            tsb_ConnectXCP.ToolTipText = "连接XCP";
            //tool
            //toolTip1.SetToolTip(toolStripButton_Connect,"连接CAN");
        }
        public ProjectForm(ProjectItem project) : this()
        {
            projectItem = project;
            this.Text = this.Name = project.Name;

            InitStatusBar();

            this.tslbCanStatus.Text = $"{(FormType.DeviceType)projectItem.DeviceType}未打开";
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
                            BaseDataForm userForm = FormCreateHelper.CreateForm(form, this.projectItem,this.dependencyXCPDAQSignals);
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

        private void RemoveMdiBackground()
        {
            foreach (Control control in this.Controls)
            {
                if (control is MdiClient)
                {
                    control.BackColor = Color.White;
                }
            }
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
                BaseDataForm userForm = FormCreateHelper.CreateForm(formItem, this.projectItem, this.dependencyXCPDAQSignals);
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
            AddNewForm addNewForm;// = new AddNewForm(this.projectItem);
            if (projectItem.CanIndex[0].ProtocolType == (int)ProtocolType.DBC)
                addNewForm = new AddNewForm(this.projectItem);
            else if (projectItem.CanIndex[0].ProtocolType == (int)ProtocolType.XCP)
                addNewForm = new AddNewXcpForm(projectItem);
            else return;
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
                BaseDataForm userForm = FormCreateHelper.CreateForm(addNewForm.FormItem, projectItem, this.dependencyXCPDAQSignals);
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
                if(projectItem.DeviceType == (int)FormType.DeviceType.VCI_USBCAN_2E_U || projectItem.DeviceType == (int)FormType.DeviceType.VCI_USBCAN1)
                {
                    USBCAN usbCan = new USBCAN(projectItem.DeviceType, projectItem.DeviceIndex, 0);
                    usbCan.SaveLog += LogHelper.InfoReceive;
                    USBCanManager.Instance.AddUsbCan(usbCan, projectItem);
                }
                else
                {
                    ZCANInstance zcan = new ZCANInstance((uint)projectItem.DeviceType, (uint)projectItem.DeviceIndex, new int[] { 0 });
                    zcan.SaveLog += LogHelper.InfoReceive;
                    USBCanManager.Instance.AddZCan( projectItem, zcan);
                }
                
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
                            switch ((FormType.DeviceType)projectItem.DeviceType)
                            {
                                case FormType.DeviceType.VCI_USBCAN_2E_U:
                                    ///Can.SetReference
                                    USBCanManager.Instance.SetRefenrece(projectItem, item.CanChannel);
                                    break;
                            }
                            ///can.init
                            if (tsb_ConnectXCP.Visible)
                            {
                                USBCanManager.Instance.InitCan(projectItem, item.CanChannel, (uint)projectItem.CanIndex[0].SlaveID - 1, (uint)projectItem.CanIndex[0].SlaveID +1 );
                            }
                            else
                            {
                                USBCanManager.Instance.InitCan(projectItem, item.CanChannel);
                            }
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
                    this.tslbCanStatus.Text = $"{(FormType.DeviceType)projectItem.DeviceType} [{projectItem.CanIndex.Count}] 已打开 ";
                    USBCanManager.Instance.StartRecv(projectItem, caninds.ToArray());//caninds
                    LeapMessageBox.Instance.ShowInfo("CAN 数据接收已启动");
                }
                else
                {
                    this.tslbCanStatus.Text = $"{(FormType.DeviceType)projectItem.DeviceType} 打开失败 ";
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
                this.tslbCanStatus.Text = $"{(FormType.DeviceType)projectItem.DeviceType} 通道数：[{projectItem.CanIndex.Count}] 已关闭 ";
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
                this.Invoke(new Action(() => {
                    tslbXcpStatus.Text = $"XCP:{status}";
                    if (status == XCPConnectStatus.Connected.ToString())
                        tsb_ConnectXCP.ToolTipText = "断开XCP";
                    else
                    {
                        tsb_ConnectXCP.ToolTipText = "连接XCP";
                    }
                }));
            }
            else
            {
                tslbXcpStatus.Text = $"XCP:{status}";
                if (status == XCPConnectStatus.Connected.ToString())
                    tsb_ConnectXCP.ToolTipText = "断开XCP";
                else
                {
                    tsb_ConnectXCP.ToolTipText = "连接XCP";
                }
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

        public uint CurrentCanValue { get; set; }

        private void Tscbb_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToolStripComboBox toolStripComboBox = sender as ToolStripComboBox;
            int index = toolStripComboBox.SelectedIndex;
            CurrentCanValue = uint.Parse(toolStripComboBox.SelectedItem.ToString());
            if (projectItem.CanIndex[index].ProtocolType == (int)ProtocolType.XCP)
            {
                tslbXcpStatus.Visible = tslbMasterID.Visible = tslbSlaveID.Visible = true;
                tslbMasterID.Text = $"MasterID:0x{projectItem.CanIndex[index].MasterID:X}";
                tslbSlaveID.Text = $"SlaveID:0x{projectItem.CanIndex[index].SlaveID:X}";
            }
            else
            {
                tslbXcpStatus.Visible = tslbMasterID.Visible = tslbSlaveID.Visible = false;
            }
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
            this.Location = new Point(StartX, StartY);
            AnimateWindow(this.Handle, 200, AW_HOR_POSITIVE | AW_SLIDE);

            if (projectItem.CanIndex[0].ProtocolType == (int)ProtocolType.XCP)
            {
                this.tslbXcpStatus.Visible = true;
                tsp_ImportElf.Visible = true;
                tslbXcpStatus.Text = $"XCP 未连接";
                this.tsb_ConnectXCP.Visible = tslbMasterID.Visible = tslbSlaveID.Visible = true;
                this.xcpModule = new XCPModule((uint)projectItem.CanIndex[0].MasterID, (uint)projectItem.CanIndex[0].SlaveID, this.projectItem);
                XCPModuleManager.AddXCPModule(xcpModule);
                this.XcpModule.OnCMDStatusChanged += XcpModule_OnCMDStatusChanged;
                this.xcpModule.OnConnectStatusChanged += XcpModule_OnConnectStatusChanged;
                this.xCPToolStripMenuItem.Visible = true;

                this.dependencyXCPDAQSignals = new DependencyXCPDAQSignals();
                dependencyXCPDAQSignals.XcpModule = this.xcpModule;
            }

            await InitForms(true);
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
            if (XcpModule != null && XcpModule.ConnectStatus == XCPConnectStatus.Connected)
                XcpModule.DisConnect(CurrentCanValue);
            XCPModuleManager.Remove(xcpModule);
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

        private void tsb_ConnectXCP_Click(object sender, EventArgs e)
        {
            if (this.tscbb.SelectedIndex != -1)
            {
                if (XcpModule.ConnectStatus == XCPConnectStatus.Connected)
                {
                    if (this.XcpModule.DisConnect(CurrentCanValue))
                    {
                        tsb_ConnectXCP.ToolTipText = "连接XCP";
                        LeapMessageBox.Instance.ShowInfo(this.Name + "，断开XCP成功");
                    }
                }
                    
                else if (this.XcpModule.Connect(CurrentCanValue))
                {
                    tsb_ConnectXCP.ToolTipText = "断开XCP";
                    LeapMessageBox.Instance.ShowInfo(this.Name + "，连接XCP成功");

                    XCPEventName = XcpModule.GetEventsName(CurrentCanValue);
                   
                }
            }
        }

        private void tsp_ImportElf_Click(object sender, EventArgs e)
        {
            /// 导入ELF，更新当前信号的地址，并修改a2l文件中的对应信号地址
            /// 选择ELF
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "请选择elf文件";
            openFileDialog.Filter = "elf文件|*.elf";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                /// 解析ELF文件，读取其中变量
                var elf = ELFReader.Load(openFileDialog.FileName);
                var s = (SymbolTable<uint>)elf.GetSection(".symtab");
                var parameters = s.Entries.Where(x => x.Type == SymbolType.Object);
                /// 遍历所有窗口中的信号
                foreach (var formItem in projectItem.Form)
                {
                    if (formItem.XCPSingals == null || formItem.XCPSingals.xCPSignalList.Count == 0)
                        continue;
                    foreach (var signal in formItem.XCPSingals.xCPSignalList)
                    {
                        var q = parameters.Where(x => x.Name == signal.SignalName).Select(x => x.Value);
                        if (q != null && q.Count() > 0)
                        {
                            signal.ECUAddress = q.First().ToString("X");
                            LogHelper.Info($"{formItem.Name}-{signal.SignalName} 更新地址。");
                        }
                           
                    }
                    LeapMessageBox.Instance.ShowInfo($"{formItem.Name}信号地址更新完成.");
                }

                /// 更新地址
                /// 解析a2l文件，并更新measurement 和characterics中的地址
                /// 1.import a2l
                var fileNames = this.projectItem.CanIndex[0].ProtocolFileName.Split(';');
                string filePath = AppDomain.CurrentDomain.BaseDirectory  + ProtocolType.XCP.ToString() + "\\" + fileNames[0];
                var jarPath = ".\\Config\\a2lparser.jar";
                var a2lJson = ELFParseForm.ParseA2lFile(jarPath, filePath);
                if(a2lJson == null)
                {
                    LeapMessageBox.Instance.ShowError($"{filePath}信号地址更新失败.");
                    return;
                }
                /// 2.modified a2l
                foreach (var module in a2lJson.Project.Modules)
                {
                    foreach (var measurement in module.Measurements)
                    {
                        var q = parameters.Where(x => x.Name == measurement.Name).Select(x => x.Value);
                        if (q != null && q.Count() > 0)
                        {
                            measurement.EcuAddress = (int)q.First();
                        }
                    }

                    foreach (var charac in module.Characteristics)
                    {
                        var q = parameters.Where(x => x.Name == charac.Name).Select(x => x.Value);
                        if (q != null && q.Count() > 0)
                        {
                            charac.Address = (int)q.First();
                        }
                    }
                }
                /// 3.export a2l
                ELFParseForm.Json2Asap(a2lJson, filePath, jarPath);
            }

        }

        private void xCPInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(this.XcpModule.ConnectStatus != XCPConnectStatus.Connected)
            {
                LeapMessageBox.Instance.ShowInfo("XCP未连接");
                return;
            }
            //展示XCP基础信息
            //if(this.XcpModule.)
            string da = string.Empty;
            /// byteorder
            /// calPageSupport
            /// daqsupport
            /// stim
            /// pgm
            /// max cto
            /// max dto
            /// protocol version
            /// transport version
            da += $"byteorder:{XcpModule.ByteOrder}\n\r";
            //da += $"calPage Support:{XcpModule.CalPageSupport}\n\r";
            //da += $"DAQ Support:{XcpModule.DAQSupport}\n\r";
            //da += $"STIM Support:{XcpModule.STIMPageSupport}\n\r";
            //da += $"PGM Support:{XcpModule.PGMSupport}\n\r";
            da += $"MAX CTO:{XcpModule.MAX_CTO}\n\r";
            da += $"MAX DTO:{XcpModule.MAX_DTO}\n\r";
            da += $"protocol version:{XcpModule.XCP_Protocol_Version}\n\r";
            da += $"transport version:{XcpModule.XCP_Transport_Version}\n\r";
            MessageBox.Show(da);
        }

        private void startDAQToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dependencyXCPDAQSignals.RegisterOrUnRegisterDataRecieve(this.projectItem, canChannel: (int)CurrentCanValue);
        }
    }

    /// <summary>
    /// XCP DAQ 配置
    /// </summary>
    public class DependencyXCPDAQSignals : XCPDAQViewModel
    {
        public DependencyXCPDAQSignals()
        {
            XCPSignals = new XCPSignals() { xCPSignalList = new List<XCPSignal>() };
        }

        public Dictionary<string, List<XCPSignal>> XCPSignalsByForm { get; private set; } = new Dictionary<string, List<XCPSignal>>();

        public void Add(XCPSignal signal, string formName,ref XCPSignal refSignal)
        {
            if (XCPSignalsByForm.ContainsKey(formName))
            {
                if (!XCPSignalsByForm[formName].Contains(signal))
                    XCPSignalsByForm[formName].Add(signal);
            }
            else
            {
                XCPSignalsByForm.Add(formName, new List<XCPSignal> { signal });
            }

            if (!XCPSignals.xCPSignalList.Contains(signal))
            {
                XCPSignals.xCPSignalList.Add(signal);
            }
            else
            {
                refSignal = XCPSignals.xCPSignalList.Find(x => x.SignalName == signal.SignalName);
            }
        }

        public void Add(List<XCPSignal> signals, string formName)
        {
            for (int i = 0; i < signals.Count; i++)
            {
                XCPSignal s = null;
                Add(signals[i], formName,ref s);
                if (s != null)
                    signals[i] = s;
            }
        }

        public void Add(XCPSignals signals, string formName)
        {
            for (int i = 0; i < signals.xCPSignalList.Count; i++)
            {
                XCPSignal s = null;
                Add(signals.xCPSignalList[i], formName, ref s);
                if (s != null)
                    signals.xCPSignalList[i] = s;
            }
        }

        public void Remove(XCPSignal signal, string formName)
        {
            //删除form所属的信号
            if (XCPSignalsByForm.ContainsKey(formName))
            {
                XCPSignalsByForm[formName].Remove(signal);
            }
            //是否需要从DAQ中删除
            bool remove = true;
            //查找每个Form
            foreach (var item in XCPSignalsByForm)
            {
                if (item.Value.Contains(signal))
                    remove = false;
            }

            if (remove)
            {
                XCPSignals.xCPSignalList.Remove(signal);
            }
        }

        /// <summary>
        /// 是否在获取数据，true/false
        /// </summary>
        public bool GetDataState { get; set; } = false;

        public void RegisterOrUnRegisterDataRecieve(ProjectItem ownerProject,int canChannel)
        {
            GetDataState = !GetDataState;
            USBCanManager.Instance.Register(ownerProject, OnDataReceiveEvent, canChannel, GetDataState);
            if (GetDataState)
            {
                //启动
                InitDAQ((uint)canChannel);
                XcpModule.StartStopDAQ(0x01, (uint)canChannel);
            }
            else
            {
                //停止
                XcpModule.StartStopDAQ(0x00, (uint)canChannel);
            }
        }

        public override bool InitDAQ(uint canChannel)
        {
            foreach (var item in XCPSignals.xCPSignalList)
            {
                item.StrValue = "0";
            }

            recieveData = new Dictionary<int, List<byte>>();
            DAQList.Clear();
            //根据事件ID排序
            this.XCPSignals.xCPSignalList.Sort((x, y) => { return x.EventID.CompareTo(y.EventID); });
            foreach (var signal in this.XCPSignals.xCPSignalList)
            {
                DAQList.AddSignal(signal);
            }

            foreach (var daq in DAQList.DAQs)
            {
                recieveData.Add(daq.Event_Channel_Number, new List<byte>());
            }

            var initDaqTrue = XcpModule.SetDAQ(DAQList.DAQs, canChannel);
            if (!initDaqTrue)
            {
                LogHelper.Info($"DAQ 配置{(initDaqTrue ? "成功" : "失败")}");
            }
            return initDaqTrue;
        }

        public override async Task ParseResponeToXCPSignalAsync(List<byte> data, int eventIndex)
        {
            var signals = XCPSignals.xCPSignalList.FindAll(x => x.EventID == eventIndex);
            //解析时间戳
            int timestamp = BitConverterExt.ToUInt16(data.ToArray(), 0, 0);
            List<SignalEntity> signalEntities = new List<SignalEntity>();
            var datatimeStr = DateTime.Now.ToString(Global.DATETIMEFORMAT);
            foreach (var signal in signals)
            {
                byte size = (byte)signal.Length;
                var da = new byte[size];
                try
                {
                    for (int i = 0; i < size; i++)
                    {
                        da[i] = data[signal.StartIndex + i + 2];
                    }
                    signal.StrValue = XCPHelper.DealData4Byte(signal, da);
                }
                catch (System.InvalidOperationException errOp)
                {
                    LogHelper.Error($"{signal.SignalName} Data :{signal.StrValue}", errOp);
                }
                catch (ArgumentOutOfRangeException errRange)
                {
                    LogHelper.Error($"{signal.SignalName} size :{size};startInd:{signal.StartIndex};dataLength:{data.Count}", errRange);
                }
                catch (Exception err)
                {
                    //ShowLog($"{signal.SignalName} Parse Data error ");
                    string log = string.Empty;
                    foreach (var item in data)
                    {
                        log += $" {item:X}";
                    }
                    LogHelper.Error($"{Form.Name} {signal.SignalName} Parse Data error :{log}", err);
                }
            }

            if (signalEntities.Count > 0 && Form.IsSaveData)
            {
                LogHelper.WriteToOutput(Form.Name, $"Start Save db.");
                var dbAsync = await DBHelper.GetDb();
                var result = await dbAsync.InsertAllAsync(signalEntities);
                LogHelper.WriteToOutput(Form.Name, $"Save Success，Counter:{result}.");
            }

            signalEntities.Clear();
        }

        public override void OnDataReceiveEvent(object sender, CANDataReceiveEventArgs args)
        {
            var rx_mails = args.can_msgs;
            if (null == rx_mails)
                throw new Exception("接收数据错误。");

            foreach (var item in rx_mails)
            {
                //判断ID
                if (item.cid != XcpModule.Slaveid)
                {
                    continue;
                }
                //判断是否是DTO以及DTO　ID
                if (XCPHelper.CTOCheck(item.b[0]))
                    continue;

                //拼接数据报文
                lock (recieveData)
                {
                    foreach (var daq in DAQList.DAQs)
                    {
                        //PID 在该DAQ中
                        if (item.b[0] == daq.ODTs[0].ID)//第一帧
                        {
                            var data = new byte[daq.ODTs[0].UsedSize + 2];//将时间戳也获取
                            Array.Copy(item.b, 1, data, 0, daq.ODTs[0].UsedSize + 2);
                            recieveData[daq.Event_Channel_Number].AddRange(data);
                        }

                        if (item.b[0] == daq.ODTs[0].ID + daq.ODTs.Count - 1)//最后一帧
                        {
                            if (item.b[0] != daq.ODTs[0].ID)
                            {
                                //添加数据
                                byte size = daq.ODTs.Find(x => x.ID == item.b[0]).UsedSize;
                                var data = new byte[size];
                                Array.Copy(item.b, 1, data, 0, size);
                                recieveData[daq.Event_Channel_Number].AddRange(data);
                            }
                            //解析数据
                            ParseResponeToXCPSignalAsync(recieveData[daq.Event_Channel_Number], daq.Event_Channel_Number);

                            //清空数据
                            recieveData[daq.Event_Channel_Number].Clear();
                        }
                        else if (item.b[0] > daq.ODTs[0].ID && item.b[0] < daq.ODTs[0].ID + daq.ODTs.Count - 1)
                        {
                            byte size = daq.ODTs.Find(x => x.ID == item.b[0]).UsedSize;
                            var data = new byte[size];
                            Array.Copy(item.b, 1, data, 0, size);
                            recieveData[daq.Event_Channel_Number].AddRange(data);
                        }
                    }
                }
               
            }
        }
    }

}
