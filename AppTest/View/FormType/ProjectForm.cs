using AppTest.FormType;
using AppTest.FormType.Helper;
using AppTest.Helper;
using AppTest.Model;
using ELFSharp.ELF;
using ELFSharp.ELF.Sections;
using ELFA2LTool;
using LPCanControl.CANInfo;
using LPCanControl.UDS;
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
using AppTest.Model.Interface;

namespace AppTest
{
    public partial class ProjectForm : BaseForm, ILogForm
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
        private bool canIsOpen;
        public bool CanIsOpen 
        { 
            get { return canIsOpen; } 
            set
            {
                canIsOpen = value;
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => {
                        toolStripButton_DisConnect.Enabled = canIsOpen;
                        this.tslbCanStatus.Text = $"{(FormType.DeviceType)projectItem.DeviceType} 通道数：[{projectItem.CanIndex.Count}] 已{(canIsOpen ? "开启" : "关闭")} ";
                    }));
                }
                else
                {
                    toolStripButton_DisConnect.Enabled = canIsOpen;
                    this.tslbCanStatus.Text = $"{(FormType.DeviceType)projectItem.DeviceType} 通道数：[{projectItem.CanIndex.Count}] 已{(canIsOpen ? "开启" : "关闭")} ";
                }
            }
        }

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
                    ShowLog($"【{userForm.Name}】已经打开");
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
                    MessageBox.Show("名称重复", "新建窗口错误", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error) ;
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
                    ShowLog("打开【" + addNewForm.FormItem.Name + "】窗口失败");
                }
            }
        }

       
        public async void toolStripButton_Connect_Click(object sender, EventArgs e)
        {
            if (!USBCanManager.Instance.Exist(projectItem))
            {
                ICAN can;
                if(projectItem.DeviceType == (int)FormType.DeviceType.VCI_USBCAN_2E_U )
                {
                    can = new USBCAN(projectItem.DeviceType, projectItem.DeviceIndex, new int[] { 0, 1 });
                    //usbCan.SaveLog += LogHelper.InfoReceive;
                    //USBCanManager.Instance.AddUsbCan(can, projectItem);
                }else if(projectItem.DeviceType == (int)FormType.DeviceType.VCI_USBCAN1)
                {
                    can = new USBCAN(projectItem.DeviceType, projectItem.DeviceIndex, 0);
                }
                else
                {
                    can = new ZCANInstance((uint)projectItem.DeviceType, (uint)projectItem.DeviceIndex, new int[] { 0 ,1});
                    //zcan.SaveLog += LogHelper.InfoReceive;
                    //USBCanManager.Instance.AddZCan( projectItem, zcan);
                }
                USBCanManager.Instance.AddICAN(projectItem, can);
            }

            if (CanIsOpen)
            {
                ShowLog(this.Name + "，Can口已经打开成功。", showtip: true);
            }
            else
            {
                ///can.open 
                if (USBCanManager.Instance.OpenInitStart(projectItem))
                {
                    //foreach (var item in projectItem.CanIndex)
                    //{
                    //    try
                    //    {
                    //        switch ((FormType.DeviceType)projectItem.DeviceType)
                    //        {
                    //            case FormType.DeviceType.VCI_USBCAN_2E_U:
                    //                ///Can.SetReference
                    //                USBCanManager.Instance.SetRefenrece(projectItem, item.CanChannel);
                    //                break;
                    //        }
                    //        ///can.init
                    //        if (tsb_ConnectXCP.Visible)
                    //        {
                    //            //USBCanManager.Instance.InitCan(projectItem, item.CanChannel, (uint)projectItem.CanIndex[0].SlaveID - 1, (uint)projectItem.CanIndex[0].SlaveID +1 );
                    //            USBCanManager.Instance.InitCan(projectItem, item.CanChannel);
                    //        }
                    //        else
                    //        {
                    //            USBCanManager.Instance.InitCan(projectItem, item.CanChannel);
                    //        }
                    //        ///can.start
                    //        USBCanManager.Instance.StartCanIndex(projectItem, item.CanChannel);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        ShowLog("Init Can Error：" + ex.Message, LPLogLevel.Error);
                    //        continue;
                    //    }

                    //}
                    CanIsOpen = true;
                    //btnStartCan.Text = "Close";
                    ShowLog("CAN 打开成功");

                    //增加DID读写
                    foreach (Control item in Controls)
                    {
                        item.Enabled = false;
                    }
                    if (Global.Protected == 1)
                    {
#pragma warning disable CS0162 // 检测到无法访问的代码
                        ShowLog("软件校验中..请等待");
#pragma warning restore CS0162 // 检测到无法访问的代码

                        if (await Task.Run(CheckAuth))
                        {
                            foreach (Control item in Controls)
                            {
                                item.Enabled = true;
                            }
                            ShowLog($"软件校验成功.");
                        }
                    }
                    else
                    {
                        ShowLog("0x0304 写入中...");
                        await Task.Run(new Action(() =>
                        {
                            WriteDID(new LPCanControl.Model.DIDInfo() { Name = "0x0304", DID = 0x0304, Length = 1, DIDType = LPCanControl.Model.DIDType.enc_HEX }, "1", out bool suc);
                            ShowLog($"write did 0x0304 to 1 {(suc ? "success" : "Fail")}", LPLogLevel.Debug);
                        }));
                        foreach (Control item in Controls)
                        {
                            item.Enabled = true;
                        }
                    }

                    USBCanManager.Instance.StartRecv(projectItem, projectItem.CanIndex.Select(x => x.CanChannel).ToArray());//caninds
                    ShowLog("CAN 数据接收已启动");
                }
                else
                {
                    this.tslbCanStatus.Text = $"{(FormType.DeviceType)projectItem.DeviceType} 打开失败 ";
                    ShowLog($"{(FormType.DeviceType)projectItem.DeviceType} 打开失败 ",LPLogLevel.Warn);
                    //LeapMessageBox.Instance.ShowWarn(tslbCanStatus.Text);
                    USBCanManager.Instance.RemoveUsbCan(projectItem);
                }

            }
        }

        private async Task<bool> CheckAuth()
        {
            ReadDID(UDSHelper.DIDInfos[9], out string productName);
            ReadDID(UDSHelper.DIDInfos[13], out string date);
            ReadDID(UDSHelper.DIDInfos[3], out string version);
           
            if (productName == LPCanControl.Model.Global.ReadDIDFail ||
                date == LPCanControl.Model.Global.ReadDIDFail||
                version == LPCanControl.Model.Global.ReadDIDFail)
            {
                ShowLog("软件校验失败,请重新连接");
                LogHelper.Info("did读取失败");
                return false;
            }
            bool res = true;
            var db = await DBHelper.GetAuthenticationDb();

            string sqlStr = $"select * from AuthenticationEntity where Code ='{productName}' and Code1 ='{date}' and Code2 ='{version}' ";

            var entities = await db.QueryAsync<AuthenticationEntity>(sqlStr);

            if(entities.Count == 0)
            {
                AuthenticationEntity entity = new AuthenticationEntity()
                {
                    Code = productName,
                    Code1 = date,
                    Code2 = version,
                    Count = 1
                };

                await db.InsertAsync(entity);
                //ShowLog("软件校验成功.");
            }
            else
            {
                if(entities[0].Count >= 10)
                {
                    //禁用帧
                    WriteDID(new LPCanControl.Model.DIDInfo() {Name="0x0304",DID = 0x0304,Length = 1,DIDType = LPCanControl.Model.DIDType.enc_HEX }, "0", out bool suc);
                    LogHelper.Info($"write did 0304 to 0 {(suc ? "success" : "Fail")}");
                    ShowLog("软件Lisence失效，请刷写新程序");
                  
                    res = false;
                }
                else
                {
                    WriteDID(new LPCanControl.Model.DIDInfo() { Name = "0x0304", DID = 0x0304, Length = 1, DIDType = LPCanControl.Model.DIDType.enc_HEX }, "1", out bool suc);
                    LogHelper.Info($"write did 0x0304 to 1 {(suc ? "success" : "Fail")}");
                    entities[0].Count++;
                    await db.UpdateAsync(entities[0]);
                    //ShowLog("软件校验成功.");
                }
            }

            USBCanManager.Instance.StartRecv(projectItem, projectItem.CanIndex.Select(x => x.CanChannel).ToArray());//caninds
            return res;
        }

        /// <summary>
        /// 调用UDS库 读取DID，调用后需重新调用开启CAN接收
        /// </summary>
        /// <param name="did"></param>
        /// <param name="result">读取结果，若读取失败则为<see cref="LPCanControl.Model.Global.ReadDIDFail"/></param>
        private void ReadDID(LPCanControl.Model.DIDInfo did,out string result)
        {
            USBCanManager.Instance.CloseRecv(projectItem);//caninds
            result = LPCanControl.Model.Global.ReadDIDFail;
            //遍历can口
            foreach (var item in projectItem.CanIndex)
            {
                if ((FormType.DeviceType)projectItem.DeviceType == FormType.DeviceType.USBCANFD_200U)
                {
                    result = UDSHelper.ReadDID(currentUpgradeType: UDSHelper.UpGradeIDs[LPCanControl.Model.UpgradeType.MCU],
                    dIDInfo: did,
                    canDeviceHandle: USBCanManager.Instance.ZCans[this.projectItem].DeviceHandle,
                    canIndHandle: USBCanManager.Instance.ZCans[this.projectItem].GetChannelHandle(item.CanChannel),
                    canDeviceInd: projectItem.DeviceIndex,
                    canInd: item.CanChannel);
                }
                else
                {
                    result = UDSHelper.ReadDID(currentUpgradeType: UDSHelper.UpGradeIDs[LPCanControl.Model.UpgradeType.MCU],
                    dIDInfo: did,
                    canDeviceType: this.projectItem.DeviceType,
                    canDeviceInd: projectItem.DeviceIndex, canInd: item.CanChannel);
                }
                if (result != LPCanControl.Model.Global.ReadDIDFail)
                    break;
            }
            //USBCanManager.Instance.StartRecv(projectItem, projectItem.CanIndex.Select(x=>x.CanChannel).ToArray());//caninds
        }

        /// <summary>
        /// 调用UDS库 写入DID，调用后需重新开启CAN接收
        /// </summary>
        /// <param name="did"></param>
        /// <param name="val"></param>
        /// <param name="result">写入成功</param>
        private void WriteDID(LPCanControl.Model.DIDInfo did, string val, out bool result)
        {
            USBCanManager.Instance.CloseRecv(projectItem);//caninds
            result = false;
            //遍历can口
            foreach (var item in projectItem.CanIndex)
            {
                if ((FormType.DeviceType)projectItem.DeviceType == FormType.DeviceType.USBCANFD_200U)
                {
                    result = UDSHelper.WriteDID(val,
                        currentUpgradeType: UDSHelper.UpGradeIDs[LPCanControl.Model.UpgradeType.MCU],
                    dIDInfo: did,
                    canDeviceIndHandle: USBCanManager.Instance.ZCans[this.projectItem].DeviceHandle,
                    canIndHandle: USBCanManager.Instance.ZCans[this.projectItem].GetChannelHandle(item.CanChannel),
                    canDeviceInd: projectItem.DeviceIndex,
                    canInd: item.CanChannel,
                    isFactory:true);
                }
                else
                {
                    result = UDSHelper.WriteDID(val, currentUpgradeType: UDSHelper.UpGradeIDs[LPCanControl.Model.UpgradeType.MCU],
                    dIDInfo: did,
                    canDeviceType: this.projectItem.DeviceType,
                    canDeviceInd: projectItem.DeviceIndex, canInd: item.CanChannel, isFactory: true);
                }
                if (result)
                    break;
            }
            //USBCanManager.Instance.StartRecv(projectItem, caninds.ToArray());//caninds
        }

        public void toolStripButton_DisConnect_Click(object sender, EventArgs e)
        {
            if (USBCanManager.Instance.Close(projectItem))
            {
                CanIsOpen = false;
                ShowLog(this.Name + "，关闭Can成功。",showtip:true);
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
            ToolStripLabel tsl = new ToolStripStatusLabel
            {
                Text = "Can通道",
                BorderSides = ToolStripStatusLabelBorderSides.Left
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

            //CAN打开状态
            tslbCanStatus = new ToolStripStatusLabel() {BorderSides = ToolStripStatusLabelBorderSides.Left };
            //tslCanStatus.Text = "";
            statusStrip1.Items.Add(tslbCanStatus);

            //XCP
            tslbXcpStatus = new ToolStripStatusLabel() { BorderSides = ToolStripStatusLabelBorderSides.Left };
            statusStrip1.Items.Add(tslbXcpStatus);

            tslbMasterID = new ToolStripStatusLabel { BorderSides = ToolStripStatusLabelBorderSides.Left };
            statusStrip1.Items.Add(tslbMasterID);

            tslbSlaveID = new ToolStripStatusLabel { BorderSides = ToolStripStatusLabelBorderSides.Left };
            statusStrip1.Items.Add(tslbSlaveID);

            tslbXCPCmdStatus = new ToolStripStatusLabel()
            {
                BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Right,
                Spring = true
            };
            statusStrip1.Items.Add(tslbXCPCmdStatus);

            tscbb.SelectedIndex = 0;
        }

        private void XcpModule_OnConnectStatusChanged(object sender, EventArgs args)
        {
            string status = sender.ToString();
            this.ShowLog($"XCP:{status}");
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
            ShowLog($"XCP CMD:{status}");
            //if (this.InvokeRequired)
            //{
            //    this.Invoke(new Action(() => { tslbXCPCmdStatus.Text = $"XCP CMD:{status}"; }));
            //}
            //else
            //{
            //    tslbXCPCmdStatus.Text = $"XCP CMD:{status}";
            //}
        }

        public void ShowLog(string log, LPLogLevel level = LPLogLevel.Info, bool showtip = false)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    tslbXCPCmdStatus.Text = log;
                    logListview1.AddLog(log, level);
                    if (showtip)
                    {
                        LeapMessageBox.Instance.ShowInfo(log);
                    }
                }));
            }
            else
            {
                tslbXCPCmdStatus.Text = log;
                logListview1.AddLog(log, level);
                if (showtip)
                {
                    LeapMessageBox.Instance.ShowInfo(log);
                }
            }
        }
        /// <summary>
        /// 当前选择的CAN索引，主要用来连接XCP
        /// </summary>
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
            }
            else if (projectItem.CanIndex.Count > 1 && projectItem.CanIndex[1].ProtocolType == (int)ProtocolType.XCP)
            {
                this.tslbXcpStatus.Visible = true;
                tsp_ImportElf.Visible = true;
                tslbXcpStatus.Text = $"XCP 未连接";
                this.tsb_ConnectXCP.Visible = tslbMasterID.Visible = tslbSlaveID.Visible = true;
                this.xcpModule = new XCPModule((uint)projectItem.CanIndex[1].MasterID, (uint)projectItem.CanIndex[1].SlaveID, this.projectItem);
                XCPModuleManager.AddXCPModule(xcpModule);
                this.XcpModule.OnCMDStatusChanged += XcpModule_OnCMDStatusChanged;
                this.xcpModule.OnConnectStatusChanged += XcpModule_OnConnectStatusChanged;
                this.xCPToolStripMenuItem.Visible = true;
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
                        ShowLog(this.Name + "，断开XCP成功");
                    }
                }
                    
                else if (this.XcpModule.Connect(CurrentCanValue))
                {
                    tsb_ConnectXCP.ToolTipText = "断开XCP";
                    ShowLog(this.Name + "，连接XCP成功",showtip:true);

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
                /// 更新地址
                /// 解析a2l文件，并更新measurement 和characterics中的地址
                /// 1.import a2l
                /// 解析ELF文件，读取其中变量
                var elf = ELFReader.Load(openFileDialog.FileName);
                var s = (SymbolTable<uint>)elf.GetSection(".symtab");
                var parameters = s.Entries.Where(x => x.Type == SymbolType.Object);

                string fileName = null;
                foreach (var item in projectItem.CanIndex)
                {
                    if (item.isUsed)
                    {
                        fileName = item.ProtocolFileName;
                    }
                }
                if(fileName == null)
                    ShowLog($"无a2l文件。");
                string filePath = AppDomain.CurrentDomain.BaseDirectory  + ProtocolType.XCP.ToString() + "\\" + fileName;
                var jarPath = ".\\Config\\a2lparser.jar";
                var a2lJson = ELFParseForm.ParseA2lFile(jarPath, filePath);
                if(a2lJson == null)
                {
                    ShowLog($"{filePath}信号地址更新失败.",LPLogLevel.Error);
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
                            ShowLog($"{formItem.Name}-{signal.SignalName} 更新地址。",LPLogLevel.Debug);
                        }

                    }
                    ShowLog($"{formItem.Name}信号地址更新完成.");
                }
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
            da += $"calPage Support:{XcpModule.ConnectResponse.CalPageSupport}\n\r";
            da += $"DAQ Support:{XcpModule.ConnectResponse.DAQSupport}\n\r";
            da += $"STIM Support:{XcpModule.ConnectResponse.STIMPageSupport}\n\r";
            da += $"PGM Support:{XcpModule.ConnectResponse.PGMSupport}\n\r";
            da += $"MAX CTO:{XcpModule.ConnectResponse.MAX_CTO}\n\r";
            da += $"MAX DTO:{XcpModule.ConnectResponse.MAX_DTO}\n\r";
            da += $"protocol version:{XcpModule.ConnectResponse.XCP_Protocol_Version}\n\r";
            da += $"transport version:{XcpModule.ConnectResponse.XCP_Transport_Version}\n\r";
            da += "\n\r";
            da += $"DAQProtected:{XcpModule.GetStatusResponse.CurrentResourceProtectionStatus.DAQProtected}\n\r";
            da += $"DaqRunning:{XcpModule.GetStatusResponse.DaqRunning}\n\r";
            MessageBox.Show(da, "XCP 基本信息",MessageBoxButtons.OK ,MessageBoxIcon.Information);
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logListview1.Visible = !logListview1.Visible;
            logViewToolStripMenuItem.Checked = logToolStripMenuItem.Checked = logListview1.Visible;
        }

        private void logViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logListview1.Visible = !logListview1.Visible;
            logViewToolStripMenuItem.Checked = logToolStripMenuItem.Checked = logListview1.Visible;
        }
    }
}
