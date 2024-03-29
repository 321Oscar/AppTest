﻿using AppTest.Helper;
using AppTest.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using LPCanControl.CANInfo;
using AppTest.ProtocolLib;
using AppTest.Model.Interface;
using AppTest.View.UC;

namespace AppTest.FormType
{

    public partial class BaseDataForm : MetroForm,ILogForm
    {
        #region kernel32
       
        //private readonly MaterialSkinManager materialSkinManager;
        [DllImport("kernel32.dll")]
        private static extern bool SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        /// <summary>
        /// 发送消息到指定窗口
        /// </summary>
        /// <param name="hwnd">其窗口程序将接收消息的窗口的句柄。如果此参数为HWND_BROADCAST，则消息将被发送到系统中所有顶层窗口，
        /// 包括无效或不可见的非自身拥有的窗口、被覆盖的窗口和弹出式窗口，但消息不被发送到子窗口</param>
        /// <param name="wMsg">指定被发送的消息</param>
        /// <param name="wParam">指定附加的消息指定信息</param>
        /// <param name="lParam">指定附加的消息指定信息</param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        private const int
            HTLEFT = 10,
            HTRIGHT = 11,
            HTTOP = 12,
            HTTOPLEFT = 13,
            HTTOPRIGHT = 14,
            HTBOTTOM = 15,
            HTBOTTOMLEFT = 16,
            HTBOTTOMRIGHT = 17;

        const int _ = 2;
        private const int cGrip = 16;//Grip Size
        private const int cCaption = 32;//Caption bar height
        const int CS_DropSHADOW = 0x20000;
        const int GCL_STYLE = (-26);

        //Rectangle Top { get { return new Rectangle(0, 0, this.ClientSize.Width, _); } }
        //Rectangle Left { get { return new Rectangle(0, 0, _, this.ClientSize.Height); } }
        //Rectangle Bottom { get { return new Rectangle(0, this.ClientSize.Height - _, this.ClientSize.Width, _); } }
        //Rectangle Right { get { return new Rectangle(this.ClientSize.Width - _, 0, _, this.ClientSize.Height); } }

        Rectangle TopLeft { get { return new Rectangle(0, 0, _, _); } }
        Rectangle TopRight { get { return new Rectangle(this.ClientSize.Width - _, 0, _, _); } }
        Rectangle BottomLeft { get { return new Rectangle(0, this.ClientSize.Height - _, _, _); } }
        Rectangle BottomRight { get { return new Rectangle(this.ClientSize.Width - _, this.ClientSize.Height - _, _, _); } }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex);

        #endregion

        #region 属性，变量

        [Obsolete()]
        private bool isSized;
        private bool isSaveData = false;
        private int canChannel = 0;
        private bool isGetdata;

        /// <summary>
        /// 测量信号
        /// </summary>
        public DBCSignals Signals
        {
            get; set;
        }

        protected List<int> CurrentIDs { get; set; }

        /// <summary>
        /// 控件,方便获取/设置显示值
        /// <para>
        /// 信号名称；对应控件；
        /// </para>
        /// </summary>
        public Dictionary<DBCSignal, SignalInfoUC> SignalUC { get; set; }

        /// <summary>
        /// 所属工程，获取Can Device
        /// </summary>
        public ProjectItem OwnerProject { get; set; }

        /// <summary>
        /// Can通道号
        /// </summary>
        public int CanChannel
        {
            set
            {
                if (canChannel != value && OnCanChannelChange != null)
                {
                    OnCanChannelChange(value.ToString());
                }
                canChannel = value;
            }

            get { return canChannel; }
        }

        protected HistoryDataUC HistoryDataView { get; set; }

        private DateTime saveStartTime = DateTime.Now;
        public DateTime SaveDataStartTime { 
            get => saveStartTime; 
            set 
            { 
                saveStartTime = value; 
                OnSaveTimelChange(saveStartTime); 
            }
        }

        /// <summary>
        /// 协议类名
        /// </summary>
        public string ProtocolCommand { get; set; }

        /// <summary>
        /// 协议实例，用以解析can报文
        /// </summary>
        protected BaseProtocol Protocol { get; set; }
       
        /// <summary>
        /// 是否开启缩放
        /// </summary>
        [Obsolete()]
        public bool IsSized { set { this.isSized = value; } get => isSized; }

        /// <summary>
        /// 窗口类型
        /// </summary>
        public FormType FormType { get; set; }

        /// <summary>
        /// 为了获取位置
        /// </summary>
        public FormItem FormItem { get; set; }

        /// <summary>
        /// 是否启用数据库存储数据
        /// </summary>
        public bool IsSaveData { get => isSaveData; set 
            { 
                isSaveData = value;
                isSaveDataToolStripMenuItem.Checked = IsSaveData;
                savedataBtn.IsTransparentLayerVisible = IsSaveData;
                if (IsSaveData)
                {
                    savedataBtn.Text = "保存中";
                    SaveDataStartTime = DateTime.Now;
                }
                else
                {
                    savedataBtn.Text = "未保存";
                }
            } 
        }

        /// <summary>
        /// 存储数据按钮可见性
        /// </summary>
        public bool SaveDataVisible
        {
            get { return isSaveDataToolStripMenuItem.Visible && savedataBtn.Visible; }
            set
            {
                isSaveDataToolStripMenuItem.Visible = value;
                savedataBtn.Visible = value;
            }
        }
        /// <summary>
        /// MDI模式切换按钮
        /// </summary>
        public bool MDIModeVisible { get => mDIToolStripMenuItem.Visible; set => mDIToolStripMenuItem.Visible = value; }
        /// <summary>
        /// 是否在获取数据，更改按钮可用性
        /// </summary>
        public bool IsGetdata
        {
            get => isGetdata;
            set
            {
                isGetdata = value;
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        ModifiedGetdata(value);
                    }));
                }
                else
                {
                    ModifiedGetdata(value);
                }
                this.ShowLog($"数据获取{(isGetdata ? "启动" : "关闭")}");
            }
        }

        public Form MdiContainer { get; set; }
        #endregion

        #region Public delegate
        public delegate void FormClosedDo(string formName);

        /// <summary>
        /// 关闭时删除窗口
        /// </summary>
        public event FormClosedDo Delete;

        public delegate void CanChannelChange(string signalValue);

        public event CanChannelChange OnCanChannelChange;

        public delegate void SaveTimeChange(DateTime signalValue);

        public event SaveTimeChange OnSaveTimelChange;
        #endregion


        /// <summary>
        /// 窗口所属CAN索引
        /// </summary>
        protected ToolStripLabel tslbCanIndex;
        /// <summary>
        /// CAN发送模式
        /// </summary>
        protected ToolStripComboBox tscbb;
        protected ToolStripStatusLabel toolLog;
        protected TransparentLayerToolStripLabel savedataBtn;
        public BaseDataForm()
        {
            InitializeComponent();

            //this.FormBorderStyle = FormBorderStyle.None;
            //DoubleBuffered = true;
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            //this.UpdateStyles();
            //ToolStripComboBox
            savedataBtn = new TransparentLayerToolStripLabel() { Text = "未保存",Dock = DockStyle.Right};
            savedataBtn.Click += SavedataBtn_Click;
            
            this.statusStrip1.Items.AddRange(new ToolStripItem[] { savedataBtn, new ToolStripSeparator() });

            this.ShowIcon = false;
            //x = Width;
            //y = Height;
            //setTag(this);
            this.OnCanChannelChange += DataForm_OnCanChannelChange;
            this.OnSaveTimelChange += BaseDataForm_OnSaveTimelChange;
        }

       

        private void SavedataBtn_Click(object sender, EventArgs e)
        {
            this.IsSaveData = !this.IsSaveData;
            Invalidate();
        }

        #region -- virtual --
       
        public void ShowAndLoad()
        {
            if (FormItem != null)
            {
                if (this.FormItem.LocationX == 0 && this.FormItem.LocationY == 0 && !FormItem.IsOpen)
                    this.StartPosition = FormStartPosition.WindowsDefaultLocation;
                else
                {
                    this.StartPosition = FormStartPosition.Manual;
                    this.Location = new Point(this.FormItem.LocationX, this.FormItem.LocationY);
                    this.Width = FormItem.Width;
                    this.Height = FormItem.Height;
                }
            }

            InitSignalUC();
            //setTag(this);
            this.Show();
        }

        /// <summary>
        /// 获取数据时，界面按钮变化
        /// </summary>
        /// <param name="get"></param>
        protected virtual void ModifiedGetdata(bool get)
        {

        }

        /// <summary>
        /// 注册/注销数据接收时的处理方法
        /// </summary>
        /// <param name="get"></param>
        protected virtual void RegisterOrUnRegisterDataRecieve(bool get)
        {
            USBCanManager.Instance.Register(OwnerProject, OnDataReceiveEvent, CanChannel, get);
            ShowLog($"RegisterOrUnRegisterDataRecieve {get}", LPLogLevel.Debug);
        }
        /// <summary>
        /// Can Channel改变时的事件
        /// </summary>
        /// <param name="signalValue">can channel名称</param>
        protected virtual void DataForm_OnCanChannelChange(string signalValue)
        {
            if (tslbCanIndex != null)
                tslbCanIndex.Text = $"CanIndex: {signalValue}";
        }
        /// <summary>
        /// 初始化ID和协议实例
        /// </summary>
        protected virtual void InitIDAndProtocolCmd()
        {
            //if (Signals != null && Signals.SignalList.Count > 0)
            //    CurrentIDs = Signals.SignalList.Select(x => int.Parse(x.MessageID, System.Globalization.NumberStyles.HexNumber)).Distinct().ToList();

            if (ProtocolCommand != null)
            {
                CanIndexItem canIndex = OwnerProject.CanIndex.Find(x => x.CanChannel == CanChannel);

                Protocol = ReflectionHelper.CreateInstance<BaseProtocol>(ProtocolCommand, Assembly.GetExecutingAssembly().ToString()
                    , new string[] { canIndex.ProtocolFileName });
            }
        }

        /// <summary>
        /// 初始化用户控件
        /// </summary>
        protected virtual void InitSignalUC()
        {
           
        }

        /// <summary>
        /// 增删信号后，重新加载
        /// </summary>
        protected virtual void ReLoadSignal()
        {
            InitSignalUC();
        }

        /// <summary>
        /// 数据接收事件方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public virtual async void OnDataReceiveEvent(object sender, CANDataReceiveEventArgs args)
        {
            try
            {
                List<SignalEntity> signalEntities = new List<SignalEntity>();
                //SignalEntity entity;
                var datatimeStr = DateTime.Now.ToString(Global.DATETIMEFORMAT);
                var rx_mails = args.can_msgs;
                if (null == rx_mails)
                {
                    ShowLog(this.Name + "未接收到报文", LPLogLevel.Warn);
                }

                foreach (var item in Protocol.MultipYield(rx_mails, Signals.SignalList.Cast<BaseSignal>().ToList()))
                //foreach (var item in protocol.MultipYeild(rx_mails, Signals))
                {
                    signalEntities.Add(new SignalEntity()
                    {
                        DataTime = datatimeStr,
                        ProjectName = OwnerProject.Name,
                        FormName = this.Name,
                        SignalName = item.SignalName,
                        SignalValue = item.StrValue,
                        CreatedOn = DateTime.Now.ToString(Global.DATETIMEFORMAT),
                        CANTimeStamp = item.TimeStamp
                    });
                }

                if (signalEntities.Count > 0 && IsSaveData)
                {
                    //LogHelper.WriteToOutput(this.Name, $"ThreadID:{Thread.CurrentThread.ManagedThreadId };Log:Start Save db.");
                    var dbAsync = await DBHelper.GetDb();
                    var result = await dbAsync.InsertAllAsync(signalEntities);
                    //LogHelper.WriteToOutput(this.Name, $"ThreadID:{Thread.CurrentThread.ManagedThreadId };Log:Save Success，Counter:{result}.");
                }

                signalEntities.Clear();

            }
            catch (Exception ex)
            {
                IsGetdata = false;
                ShowLog(ex.Message,LPLogLevel.Error);
            }
        }

        public async Task OnDataReceiveEvent_new(object sender, CANDataReceiveEventArgs args)
        {
            try
            {
                var signalEntities = Protocol.MultipYield(args.can_msgs, Signals.SignalList.Cast<BaseSignal>().ToList())
                             .Select(item => new SignalEntity
                             {
                                 DataTime = DateTime.Now.ToString(Global.DATETIMEFORMAT),
                                 ProjectName = OwnerProject.Name,
                                 FormName = this.Name,
                                 SignalName = item.SignalName,
                                 SignalValue = item.StrValue,
                                 CreatedOn = DateTime.Now.ToString(Global.DATETIMEFORMAT),
                             })
                             .ToList();

                if (signalEntities.Count == 0 || !IsSaveData)
                    return;

                using (var dbAsync = DBHelper.GetDb())
                {
                    var result = await dbAsync.Result.InsertAllAsync(signalEntities);
                    LogHelper.WriteToOutput(this.Name, $"ThreadID:{Thread.CurrentThread.ManagedThreadId};Log: Save Success，Counter:{result}.");
                }
            }
            catch (Exception ex)
            {
                IsGetdata = false;
                ShowLog(ex.Message,LPLogLevel.Error);
            }
        }
        /// <summary>
        /// 显示日志
        /// </summary>
        /// <param name="log"></param>
        public virtual void ShowLog(string log, LPLogLevel level = LPLogLevel.Info, bool showtip = false)
        {
            try
            {
                if (statusStrip1.InvokeRequired)
                {
                    this.Invoke(new Action(() => {
                        toolLog.Text = log;
                        //toolTip1.SetToolTip(toolLog,log);
                    }));
                }
                else
                {
                    toolLog.Text = log;
                }
                if (MdiParent != null && this.MdiParent is ILogForm && !string.IsNullOrEmpty(log))
                {
                    ((ILogForm)MdiParent).ShowLog($"[{this.Name}] {log}", level);
                }
                LogHelper.WriteToOutput(this.Name, log);
            }
            catch (Exception ex)
            {
                LogHelper.Error("显示日志错误",ex);
            }
            
        }
        /// <summary>
        /// 初始化下方status
        /// </summary>
        protected virtual void InitStateStrip()
        {
            tslbCanIndex = new ToolStripLabel();
            tslbCanIndex.Text = $"CanIndex: {CanChannel}";
            toolLog = new ToolStripStatusLabel() { Spring = true };
            toolLog.ForeColor = Color.White;
            //分隔符
            ToolStripSeparator tss1 = new ToolStripSeparator();

            this.statusStrip1.Items.AddRange(new ToolStripItem[]
                {
                    tslbCanIndex,
                    tss1,
                    toolLog
                }
            );
        }

        protected virtual void ChangeBaseColor(Color c)
        {
            this.statusStrip1.BackColor = c;
            this.statusStrip1.ForeColor = Color.White;
        }

        protected virtual void BaseDataForm_OnSaveTimelChange(DateTime signalValue)
        {
            if (HistoryDataView != null)
            {
                HistoryDataView.StartTime = signalValue;
            }
        }

        #endregion

        #region 右键菜单

        /// <summary>
        /// 修改信号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addSignalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModifiedSignals();
        }

        /// <summary>
        /// 修改信号
        /// </summary>
        protected virtual void ModifiedSignals()
        {
            //ReLoadSignal();return;
            AddNewForm editForm;
            if (OwnerProject.CanIndex[0].ProtocolType == (int)ProtocolType.DBC)
            {
                editForm = new AddNewForm(this.OwnerProject, this.OwnerProject.Form.Find(x => x.Name == this.Name));
            }
            else if (OwnerProject.CanIndex[0].ProtocolType == (int)ProtocolType.XCP)
            {
                editForm = new AddNewXcpForm(this.OwnerProject, this.OwnerProject.Form.Find(x => x.Name == this.Name));
            }
            else
            {
                return;
            }

            editForm.Parent = this;
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                //IsGetdata = false;
                //reload Signals
                this.Signals = editForm.FormItem.DBCSignals;
                //this.xCPSingals = editForm.FormItem.XCPSingals;
                this.CanChannel = editForm.FormItem.CanChannel;
                ReLoadSignal();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void isSaveDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.IsSaveData = !IsSaveData;
            
        }

        #endregion
       


        private void mDIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (this.IsMdiChild)
            {
                this.MdiParent = null;
            }
            else
            {
                this.MdiParent = MdiContainer;
            }
            mDIToolStripMenuItem.Checked = this.IsMdiChild;
        }

        private void BaseDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsGetdata)
                IsGetdata = false;

            this.FormItem.LocationX = this.Location.X;
            this.FormItem.LocationY = this.Location.Y;
            this.FormItem.Width = this.Width;
            this.FormItem.Height = this.Height;

            if (Delete != null)
                Delete(this.FormItem.Name);
        }

        private void BaseDataForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
            FlushMemory();
        }

        private static void FlushMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if(Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
            }
        }

        private void SetControlDouble(Control s)
        {
            Type dgvType = s.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);

            pi.SetValue(s, true, null);
            if (s.Controls.Count> 0)
            {
                foreach (Control item in s.Controls)
                {
                    SetControlDouble(item);
                }
            }
        }

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle &= ~0x02000000;
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
        private void BaseDataForm_MouseClick(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            return;
#pragma warning disable CS0162 // 检测到无法访问的代码
            if (IsSaveData)
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(128, Color.Gray)))
                {
                    Rectangle rect = new Rectangle(statusStrip1.Location.X + savedataBtn.Bounds.X,
                                                    statusStrip1.Location.Y + savedataBtn.Bounds.Y,
                                                    savedataBtn.Bounds.Width,
                                                    savedataBtn.Bounds.Height);
                    e.Graphics.FillRectangle(brush, rect);
                }
            }
#pragma warning restore CS0162 // 检测到无法访问的代码

            return;
            //e.Graphics.FillRectangle(Brushes.Green, Top);
            //e.Graphics.FillRectangle(Brushes.DarkBlue, Left);
            //e.Graphics.FillRectangle(Brushes.DarkBlue, Right);
            //e.Graphics.FillRectangle(Brushes.DarkBlue, Bottom);

            Rectangle rc = new Rectangle(this.ClientSize.Width - cGrip, this.ClientSize.Height - cGrip, cGrip, cGrip);
            ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor, rc);
            rc = new Rectangle(0, 0, this.ClientSize.Width, cCaption);
            e.Graphics.FillRectangle(Brushes.DarkBlue, rc);
            StringFormat vStringFormat = new StringFormat();
            vStringFormat.Alignment = StringAlignment.Center;
            vStringFormat.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawString(this.Text, this.Font, Brushes.White, rc, vStringFormat);

            
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            return;
#pragma warning disable CS0162 // 检测到无法访问的代码
            if (m.Msg == 0x84)
            {
                Point pos = new Point(m.LParam.ToInt32());
                pos = this.PointToClient(pos);

                if (pos.Y < cCaption)
                {
                    m.Result = (IntPtr)2;//HTCAPTION
                    return;
                }

                var cursor = this.PointToClient(Cursor.Position);

                if (TopLeft.Contains(cursor))
                    m.Result = (IntPtr)HTTOPLEFT;
                else if (TopRight.Contains(cursor))
                {
                    m.Result = (IntPtr)HTTOPRIGHT;
                }
                else if (BottomLeft.Contains(cursor))
                {
                    m.Result = (IntPtr)HTBOTTOMLEFT;
                }
                else if (BottomRight.Contains(cursor))
                {
                    m.Result = (IntPtr)HTBOTTOMRIGHT;
                }
                //else if (Top.Contains(cursor))
                //{
                //    m.Result = (IntPtr)HTTOP;
                //}
                //else if (Left.Contains(cursor))
                //{
                //    m.Result = (IntPtr)HTLEFT;
                //}
                //else if (Right.Contains(cursor))
                //{
                //    m.Result = (IntPtr)HTRIGHT;
                //}
                //else if (Bottom.Contains(cursor))
                //{
                //    m.Result = (IntPtr)HTBOTTOM;
                //}
            }
#pragma warning restore CS0162 // 检测到无法访问的代码
        }

        #region -- 弃用 -- 控件大小随窗口大小等比例缩放 --
#pragma warning disable CS0649 // 从未对字段“BaseDataForm.x”赋值，字段将一直保持其默认值 0
        private float x;
#pragma warning restore CS0649 // 从未对字段“BaseDataForm.x”赋值，字段将一直保持其默认值 0

        private void BaseDataForm_MouseMove(object sender, MouseEventArgs e)
        {
            //Debug.a
            return;
#pragma warning disable CS0162 // 检测到无法访问的代码
            if(this.MdiParent != null)
            {
                var closeForm = FindChildForm(this);
                int x, y = 0;
                if(closeForm.Location.Y + closeForm.Height < this.Location.Y)
                {
                    y = closeForm.Location.Y + closeForm.Height + 10;
                }
                else
                {
                    y = this.Location.Y;
                }

                if(closeForm.Location.X + closeForm.Width > this.Location.X)
                {
                    x = closeForm.Location.X + closeForm.Width + 10;
                }
                else
                {
                    x = this.Location.X;
                }

                this.Location = new Point(x, y);
            }
#pragma warning restore CS0162 // 检测到无法访问的代码
        }

        /// <summary>
        /// 查找距离最近的一个form
        /// </summary>
        /// <returns></returns>
        private Form FindChildForm(Form source)
        {
            Form f = null;
            double minlength = double.MaxValue;
            foreach (var form in MdiParent.MdiChildren)
            {
                if(form != source && form.Visible)
                {
                    double length = Math.Sqrt(Math.Pow(form.Location.X - source.Location.X, 2) + Math.Pow(form.Location.Y - source.Location.Y, 2));
                    if(minlength > length)
                    {
                        minlength = length;
                        f = form;
                    }
                }
            }

            return f;
        }

#pragma warning disable CS0649 // 从未对字段“BaseDataForm.y”赋值，字段将一直保持其默认值 0
        private float y;
#pragma warning restore CS0649 // 从未对字段“BaseDataForm.y”赋值，字段将一直保持其默认值 0


        private void setTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ";" + con.Height + ";" + con.Left + ";" + con.Top + ";" + con.Font.Size;
                if (con.Controls.Count > 0)
                {
                    setTag(con);
                }
            }
        }

        private void setControls(float newx, float newy, Control cons)
        {
            //遍历窗口中控件，重新设计控件的值
            foreach (Control con in cons.Controls)
            {
                //获取Tag属性，并分割后存储字符串数组
                if (con.Tag != null)
                {
                    string[] mytag = con.Tag.ToString().Split(new char[] { ';' });
                    //根据窗体缩放的比例确定控件的值
                    con.Width = Convert.ToInt32(Convert.ToSingle(mytag[0]) * newx);
                    con.Height = Convert.ToInt32(Convert.ToSingle(mytag[1]) * newy);
                    con.Left = Convert.ToInt32(Convert.ToSingle(mytag[2]) * newx);
                    con.Top = Convert.ToInt32(Convert.ToSingle(mytag[3]) * newy);
                    Single currentSize = Convert.ToInt32(Convert.ToSingle(mytag[4]) * newy);
                    con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    if (con.Controls.Count > 0)
                    {
                        setControls(newx, newy, con);
                    }
                }
            }
        }


        private void BaseForm_ResizeEnd(object sender, EventArgs e)
        {
            //return;
            float newx = (this.Width) / x;
            float newy = (Height) / y;
#pragma warning disable CS0612 // “BaseDataForm.isSized”已过时
            if (isSized)
                setControls(newx, newy, this);
#pragma warning restore CS0612 // “BaseDataForm.isSized”已过时
        }

        private void autoSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
#pragma warning disable CS0612 // “BaseDataForm.isSized”已过时
#pragma warning disable CS0612 // “BaseDataForm.isSized”已过时
            isSized = !isSized;
#pragma warning restore CS0612 // “BaseDataForm.isSized”已过时
#pragma warning restore CS0612 // “BaseDataForm.isSized”已过时
#pragma warning disable CS0612 // “BaseDataForm.isSized”已过时
            if (isSized)
                autoSizeToolStripMenuItem.Image = Properties.Resources.Checked;
            else
                autoSizeToolStripMenuItem.Image = null;
#pragma warning restore CS0612 // “BaseDataForm.isSized”已过时
        }

        private void BaseForm_SizeChanged(object sender, EventArgs e)
        {
            //if (helper.oldCtrl == null ||helper.oldCtrl.Count == 0)
            //    helper.controllInitializeSize(this);

            //helper.controlAutoSize(this);
            //helper.controlAutoSize()
        }

        private void BaseForm_Resize(object sender, EventArgs e)
        {
            return;
            //float newx = (this.Width) / x;
            //float newy = (Height) / y;
            //setControls(newx, newy, this);
        }

        #endregion
    }

}
