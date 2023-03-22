using AppTest.FormType.Helper;
using AppTest.Helper;
using AppTest.Model;
using LPCanControl.CANInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppTest.FormType
{
    public partial class DataForm : BaseDataForm
    {
        private readonly Color GetColor = Color.FromArgb(0, 174, 219);
        private readonly Color SetColor = Color.FromArgb(243, 119, 53);
        private readonly Color RtlColor = Color.FromArgb(0, 177, 89);

        public DataForm() : base()
        {
            InitializeComponent();
        }

        public DataForm(FormType formType, ProtocolType protocolType = ProtocolType.DBC) : this()
        {
            this.FormType = formType;

            this.metroPanelMain.Controls.Add(this.tableLayoutPanel1);
            //tableLayoutPanel1.Dock = DockStyle.Fill;
            //metroComboBox_Signal.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //dataGridView1.Columns[0].ReadOnly = true;
            strValueDataGridViewTextBoxColumn.ReadOnly = formType != FormType.Set;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.RowHeadersVisible = false;
            messageIDDataGridViewTextBoxColumn.Visible = protocolType == ProtocolType.DBC;
            base.MDIModeVisible = protocolType == ProtocolType.DBC;

            

            switch (formType)
            {
                case FormType.Get:
                //case FormType.XCPGet:
                    gbRlcntControls.Visible = gbSetControls.Visible = false;
                    SetColr(GetColor);
                    metroTabControl1.Style  = MetroFramework.MetroColorStyle.Blue;
                    this.Style = MetroFramework.MetroColorStyle.Blue;
                    LoadDatGridViewContext();
                    break;
                case FormType.Set:
                //case FormType.XCPSet:
                    this.Style = MetroFramework.MetroColorStyle.Orange;
                    metroTabControl1.Style = MetroFramework.MetroColorStyle.Orange;
                    metroComboBox_Signal.Style = MetroFramework.MetroColorStyle.Orange;
                    SetColr(SetColor);
                    LoadDatGridViewContext();
                    gbRlcntControls.Visible = gbGetControls.Visible = false;
                    this.SaveDataVisible = false;
                    cbbSignals.AutoCompleteSource = AutoCompleteSource.ListItems;
                    cbbSignals.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

                    tscbb = new ToolStripComboBox();
                    tscbb.ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                    tscbb.ComboBox.Items.AddRange(new string[] { "正常发送", "单次发送", "自发自收", "单次自发自收" });
                    statusStrip1.Items.Add(tscbb);
                    tscbb.SelectedIndex = 0;
                    ToolStripSeparator tss = new ToolStripSeparator();
                    statusStrip1.Items.Add(tss);
                    metroTabControl1.TabPages.RemoveAt(1);
                    //tabControl1.TabPages.RemoveAt(1);
                    //tabPage2.vi
                    break;
                case FormType.RollingCounter:
                    this.Style = MetroFramework.MetroColorStyle.Green;
                    metroTabControl1.Style = MetroFramework.MetroColorStyle.Green;
                    metroComboBox_Signal.Style = MetroFramework.MetroColorStyle.Green;
                    SetColr(RtlColor);
                    btnReduce.BackColor = RtlColor;
                    btnAdd.BackColor = RtlColor;
                    btnSet.BackColor = RtlColor;
                    gbGetControls.Visible = false;
                    this.SaveDataVisible = false;
                    metroTabControl1.TabPages.RemoveAt(1);
                    timerList = new List<Thread>();
                    btnSet.Text = "Set";
                    break;
            }

            InitStateStrip();
            
            nudStep.DecimalPlaces = 2;

        }

        private void SetColr(Color c)
        {
            this.dataGridView1.DefaultCellStyle.SelectionBackColor = c;
            //this.cbbSignals.BackColor = c;
            //this.cbbSignals.ForeColor = Color.White;
            this.statusStrip1.BackColor = c;
            this.statusStrip1.ForeColor = Color.White;
        }

        /// <summary>
        /// 加载右键菜单
        /// </summary>
        protected void LoadDatGridViewContext()
        {
            foreach (DataGridViewColumn item in dataGridView1.Columns)
            {
                ToolStripMenuItem tsb = new ToolStripMenuItem();
                tsb.Checked = item.Visible;
                tsb.Click += Tsb_Click;
                tsb.Text = item.HeaderText;
                tsb.Name = item.Index.ToString();
                contextMenuStrip1.Items.Add(tsb);
            }
            
        }

        private void Tsb_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsb = sender as ToolStripMenuItem;
            int idx = int.Parse(tsb.Name);
            dataGridView1.Columns[idx].Visible = !dataGridView1.Columns[idx].Visible;
            tsb.Checked = dataGridView1.Columns[idx].Visible;
        }

        protected override void InitStateStrip()
        {
            tslbCanIndex = new ToolStripLabel();
            tslbCanIndex.Text = $"CanIndex: {CanChannel}";
            toolLog = new ToolStripStatusLabel();
            toolLog.ForeColor = Color.Red;
            //分隔符
            ToolStripSeparator tss1 = new ToolStripSeparator();

            this.statusStrip1.Items.AddRange(new ToolStripItem[]
                {
                    tslbCanIndex,
                    tss1,
                    toolLog
                }
            );

            statusStrip1.Location = new Point(0, this.Size.Height);
        }

        protected override void ReLoadSignal()
        {
            base.ReLoadSignal();
        }

        protected override void InitSignalUC()
        {
            //cbbSignals.DataSource = null;
            //cbbSignals.DataSource = Signals.Signal;
            //cbbSignals.DisplayMember = "SignalName";

            metroComboBox_Signal.DataSource = null;
            metroComboBox_Signal.DataSource = Signals.SignalList;
            metroComboBox_Signal.DisplayMember = "SignalName";

            BindingList<DBCSignal> bs = new BindingList<DBCSignal>(Signals.SignalList);
            this.dataGridView1.DataSource = bs;

            InitIDAndProtocolCmd();

            if (this.FormType == FormType.RollingCounter)
                AddSendThread();
            else if (FormType == FormType.Get)
            {
                HistoryDataUC hduc = new HistoryDataUC();
                hduc.ProjectName = this.OwnerProject.Name;
                hduc.FormName = this.Name;
                hduc.Dock = DockStyle.Fill;
                metroTabPage2.Controls.Clear();
                metroTabPage2.Controls.Add(hduc);
                hduc.ChangeColorTheme(GetColor);
                //tabPage2.Controls.Clear();
                //tabPage2.Controls.Add(hduc);
            }
        }

        private bool TestCanSend()
        {
            string errorlog =  string.Empty;

            bool send = USBCanManager.Instance.SendTest(OwnerProject,ref errorlog, canindex: this.CanChannel);
            if (!send)
            {
                MessageBox.Show(errorlog);
                ShowLog(errorlog);
            }
              
            return send;
        }

        protected virtual void ShowSignalInfo(DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                var signal = dataGridView1.Rows[e.RowIndex].DataBoundItem as DBCSignal;
                SignalItemForm<DBCSignal> siF = new SignalItemForm<DBCSignal>(signal, signal.SignalName);
                siF.Show();
            }
            catch (Exception ex)
            {
                ShowLog(ex.Message);
            }
        }

        #region DataGridView

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowSignalInfo(e);
        }

        /// <summary>
        /// 内嵌控件值改变后立即触发事件，而不需要离开该单元格时才触发，此时需要用到CurrentCellDirtyStateChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (grid != null)
            {
                if (grid.CurrentCell.OwningColumn.DataPropertyName == "WhetherSendOrGet")
                    grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].DataPropertyName == "StrValue")
            {
                try
                {
                    var v = Convert.ToDouble(e.Value);
                    var signal = dataGridView1.Rows[e.RowIndex].DataBoundItem as BaseSignal;
                    if (v > signal.Maximum || v < signal.Minimum)
                    {
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    }
                    else
                    {
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
                catch
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                }

            }
        }
        #endregion

        #region Get - Method

        protected override void ModifiedGetdata(bool get)
        {
            btnGet.Text = !get ? "Start" : "Stop";
            nudTimerInterval.Enabled = !get;
            ShowLog("");
            //WhetherSendOrGet.ReadOnly = get;
            RegisterOrUnRegisterDataRecieve(get);
        }

        private void btnDataControl_Click(object sender, EventArgs e)
        {
            DataControl();
        }

        protected virtual void DataControl()
        {
            if (IsGetdata)//正在获取数据
            {
                IsGetdata = false;
            }
            else
            {
                if (!USBCanManager.Instance.Exist(OwnerProject))
                {
                    LeapMessageBox.Instance.ShowInfo("CAN未打开!");
                    return;
                }
                foreach (var item in Signals.SignalList)
                {
                    item.StrValue = "0";
                }

                IsGetdata = true;

                //RegisterCancelToken();
                //getDataTask = new Task(new Action(AutoGetByThread), this.cancellation.Token);
                //getDataTask.Start();
            }
        }

        #region 弃用
        readonly Random r = new Random();
        Task getDataTask;
        CancellationTokenSource cancellation;

        /// <summary>
        /// 弃用
        /// </summary>
        private void AutoGetByThread()
        {
            while (IsGetdata)
            {
                ShowLog("");
                GetDataTimer_Elapsed(null);
                Thread.Sleep((int)nudTimerInterval.Value);
            }
            LogHelper.WriteToOutput(this.Name, "结束获取数据。");
        }
        /// <summary>
        /// 弃用
        /// </summary>
        private async void GetDataTimer_Elapsed(object sender)//, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                List<SignalEntity> signalEntities = new List<SignalEntity>();
                var datatimeStr = DateTime.Now.ToString(Global.DATETIMEFORMAT);
                SignalEntity entity;
                /*
                 * 测试
                 * 模拟数据
                 */

                //signalEntities.Clear();
                /*
                foreach (var item in Signals.Signal)
                {
                    //从Can口中取数据，并解析
                    //dt.Columns[item.SignalName] = 
                    string data = r.Next(2, 2000).ToString();
                    signalUC[item.SignalName].SignalValue = (data);
                    entity = new SignalEntity();
                    entity.DataTime = datatimeStr;
                    entity.ProjectName = OwnerProject.Name;
                    entity.FormName = this.Name;
                    entity.SignalName = item.SignalName;
                    entity.SignalValue = data;
                    entity.CreatedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
                    signalEntities.Add(entity);

                }
                */
                var rx_mails = USBCanManager.Instance.Receive(OwnerProject, CurrentIDs.ToArray(), CanChannel, $"[{this.FormType}] [{this.Name}]");
                //CAN_msg[] rx_mails = USBCanManager.Instance.Receive(OwnerProject, CanChannel, $"[{this.FormType}] [{this.Name}]");
                if (null == rx_mails)
                    throw new Exception("接收数据错误。");
                foreach (var item in Protocol.MultipYeild(rx_mails, Signals.SignalList.Cast<BaseSignal>().ToList()))
                //foreach (var item in protocol.MultipYeild(rx_mails, Signals))
                {
                    //signalUC[item.SignalName].SignalValue = item.StrValue;
                    entity = new SignalEntity();
                    entity.DataTime = datatimeStr;
                    entity.ProjectName = OwnerProject.Name;
                    entity.FormName = this.Name;
                    entity.SignalName = item.SignalName;
                    entity.SignalValue = item.StrValue;
                    entity.CreatedOn = DateTime.Now.ToString(Global.DATETIMEFORMAT);
                    signalEntities.Add(entity);
                }

                if (signalEntities.Count > 0 && base.IsSaveData)
                {
                    LogHelper.WriteToOutput(this.Name, $"ThreadID:{Thread.CurrentThread.ManagedThreadId };Log:Start Save db.");
                    var dbAsync = await DBHelper.GetDb();
                    var result = await dbAsync.InsertAllAsync(signalEntities);
                    LogHelper.WriteToOutput(this.Name, $"ThreadID:{Thread.CurrentThread.ManagedThreadId };Log:Save Success，Counter:{result}.");
                }

                signalEntities = null;

            }
            catch (Exception ex)
            {
                IsGetdata = false;
                ShowLog(ex.Message);
            }
        }

        public void RegisterCancelToken()
        {
            cancellation = null;
            //cancellation.Dispose();
            cancellation = new CancellationTokenSource();
            cancellation.Token.Register(() =>
            {
                LogHelper.WriteToOutput(this.Name, "取消获取数据线程");
            });
        }
        #endregion

        #endregion

        #region Set - Method

        private void btnSetOrSend_Click(object sender, EventArgs e)
        {
            SetOrSend();
        }

        protected virtual void SetOrSend() 
        {
            string log = string.Empty;
            ShowLog("");

            if (this.FormType == FormType.Set)//Set  发送数据
            {
                byte sendtype = (byte)tscbb.SelectedIndex;
                for (int i = 0; i < CurrentIDs.Count; i++)
                {
                    Dictionary<BaseSignal, string> keyValues = new Dictionary<BaseSignal, string>();
                    var signals = this.Signals.SignalList.Where(x => x.MessageID == CurrentIDs[i].ToString("X"));
                    foreach (var item in signals)
                    {
                        keyValues.Add(item, item.StrValue);
                    }
                    var frame = Protocol.BuildFrames(keyValues);
                    try
                    {
                        if (USBCanManager.Instance.Send(OwnerProject, CanChannel, sendData: frame[0], $"[{this.FormType}]{this.Name}", sendtype))
                        {
                            log += $"{frame[0].ID:X}发送成功";
                        }
                    }
                    catch (USBCANOpenException ex)
                    {
                        LeapMessageBox.Instance.ShowInfo(ex.Message);
                        break;
                    }

                }
            }
            else if (this.FormType == FormType.RollingCounter)
            {
                if (cbbSignals.SelectedIndex != -1)
                {
                    DBCSignal selectSignal = cbbSignals.SelectedItem as DBCSignal;

                    selectSignal.StrValue = tbCurrent.Text;
                }
            }

            if (!string.IsNullOrEmpty(log))
                ShowLog(log);
        }

        private void cbbSignals_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ComboBox cbb = sender as ComboBox;
            SelectedSignalChanged();
        }

        protected virtual void SelectedSignalChanged()
        {
            if (metroComboBox_Signal.SelectedIndex != -1)
            {
                DBCSignal selectSignal = metroComboBox_Signal.SelectedItem as DBCSignal;

                ///不能绑定数据，rollingcounter 模式下改变数据会跳变
                ///9->10 先发1，再发10
                if (this.FormType == FormType.Set)
                {
                    var threadSafeModel = new SynchronizedNotifyPropertyChanged<DBCSignal>(selectSignal, this);
                    tbCurrent.DataBindings.Clear();
                    tbCurrent.DataBindings.Add("Text", threadSafeModel, "StrValue", false, DataSourceUpdateMode.OnPropertyChanged, "0");
                }

                tbCurrent.Text = selectSignal.StrValue;

            }
        }

        private void btnChangeValue_Click(object sender, EventArgs e)
        {
            ChangeValue(sender);
        }

        protected virtual void ChangeValue(object sender)
        {
            if (metroComboBox_Signal.SelectedIndex != -1)
            {
                DBCSignal selectSignal = metroComboBox_Signal.SelectedItem as DBCSignal;

                decimal oldValue = Convert.ToDecimal(selectSignal.StrValue);
                Button bt = sender as Button;
                decimal newValue;
                try
                {
                    if (bt.Name == btnAdd.Name)
                        newValue = oldValue + nudStep.Value;
                    else if (bt.Name == btnReduce.Name)
                    {
                        newValue = oldValue - nudStep.Value;
                    }
                    else if (bt.Name == btnMultip.Name)
                    {
                        newValue = oldValue * nudStep.Value;
                    }
                    else if (bt.Name == btnDivision.Name)
                    {
                        newValue = oldValue / nudStep.Value;
                    }
                    else
                    {
                        return;
                        //LeapMessageBox.Instance.ShowInfo();
                    }
                }
                catch (Exception ex)
                {
                    LeapMessageBox.Instance.ShowError(ex);
                    return;
                }


                selectSignal.StrValue = newValue.ToString();

                SelectedSignalChanged();

                SetOrSend();
            }
        }

        #endregion

        #region RollingCounter - Method

        /// <summary>
        /// 是否在发送数据
        /// </summary>
        private bool isSend = false;

        //private List<System.Threading.Timer> timerList;
        /// <summary>
        /// 发送数据的线程组
        /// </summary>
        private readonly List<Thread> timerList;

        /// <summary>
        /// 是否在发送数据
        /// </summary>
        public bool IsSend
        {
            get => isSend;
            private set
            {
                if (btnAutoSend.InvokeRequired)
                {
                    btnAutoSend.Invoke(new Action(() => { btnAutoSend.Text = !value ? "启动" : "停止"; }));
                }
                else
                {
                    btnAutoSend.Text = !value ? "启动" : "停止";
                }
                isSend = value;
            }
        }

        private void btnAutoSend_Click(object sender, EventArgs e)
        {
            //启动所有
            if (isSend)
            {
                ClearTimer();
                IsSend = false;
            }
            else
            {
                //测试CAN盒是否能否发送数据
                if (TestCanSend())
                    AddSendThread();
            }
        }

        private void AddSendThread()
        {
            for (int i = 0; i < CurrentIDs.Count; i++)
            {
                int time = this.Signals.SignalList.Where(x => x.MessageID == CurrentIDs[i].ToString("X")).ToList()[0].CycleTime;
                if (time == 0)
                {
                    LeapMessageBox.Instance.ShowInformation("不支持周期为0的信号!");
                    return;
                }
                Thread t = new Thread(new ParameterizedThreadStart(AutoSendThread));
                t.Name = CurrentIDs[i].ToString("X");
                t.IsBackground = true;
                LogHelper.WriteToOutput(this.Name, $"{CurrentIDs[i]:X} 发送线程启动");
                t.Start(CurrentIDs[i]);

                timerList.Add(t);
                IsSend = true;

            }
            btnAutoSend.Text = "停止";
        }

        /// <summary>
        /// 发送数据线程
        /// </summary>
        /// <param name="MessageID">一个ID一个Groupbox，按照ID组帧发送</param>
        private void AutoSendThread(object MessageID)
        {
            int messageID = Convert.ToInt32(MessageID);
            int time = this.Signals.SignalList.Where(x => x.MessageID == messageID.ToString("X")).ToList()[0].CycleTime;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            LogHelper.WriteToOutput(this.Name, $"{messageID:X}启动发送:发送周期{time}ms");
            do
            {
                Send(messageID);
                sw.Stop();
                long times = sw.ElapsedMilliseconds;
                
                sw.Reset();
                sw.Restart();
                //修正
                int sleepTime = time - (int)times;
                LogHelper.WriteToOutput(this.Name, $"{messageID:X}发送结束【周期-{time}ms】,使用了{times}毫秒，等待{sleepTime}ms");
                if (sleepTime > 0)
                    Thread.Sleep(sleepTime);

            } while (isSend);
            LogHelper.WriteToOutput(this.Name, $"{messageID:X}停止发送");
        }

        private void ClearTimer()
        {
            isSend = false;
            timerList.Clear();
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="messageID"></param>
        private void Send(int messageID)
        {
            Dictionary<BaseSignal, string> keyValues = new Dictionary<BaseSignal, string>();
            if (!isSend)
                return;
            ShowLog("");
            try
            {
                bool needCheckSum = false;
                var signals = this.Signals.SignalList.Where(x => x.MessageID == messageID.ToString("X"));
                foreach (var item in signals)
                {
                    if (item.SignalName.ToLower().Contains("rolling"))
                    {
                        if (int.Parse(item.StrValue) >= 15)
                        {
                            item.StrValue = "0";
                        }
                        else
                        {
                            int nextValue = int.Parse(item.StrValue) + 1;
                            item.StrValue = nextValue.ToString();
                        }
                    }
                    if (item.SignalName.ToLower().Contains("checksum"))
                        needCheckSum = true;
                    keyValues.Add(item, item.StrValue);
                }

                if (keyValues.Count == 0)
                    return;
                var frame = Protocol.BuildFrames(keyValues);

                /// 这里直接写死了checksum的位置和长度
                /// 重新计算checksum
                if (needCheckSum)
                {
                    byte crc = 0;
                    for (UInt16 i = 0; i < 7; i++)
                    {
                        crc = (byte)(crc + frame[0].Data[i]);
                    }
                    crc = (byte)(crc ^ 0xff);
                    frame[0].Data[7] = crc;
                }

                ///组帧
                USBCanManager.Instance.Send(OwnerProject, canindex: this.CanChannel, sendData: frame[0], $"[{this.FormType}]{this.Name}");
            }
            catch (Exception ex)
            {
                LogHelper.Error(this.Name, ex);
                ShowLog(ex.Message);
                IsSend = false;
                var thread = timerList.Find(x => x.Name == messageID.ToString("X"));
                if (thread != null)
                {
                    thread.Abort();
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            isSend = false;
        }

        #endregion

        #region 双缓冲-尝试改善界面闪烁
        private void DataForm_Load(object sender, EventArgs e)
        {
            //Type dgvType = this.dataGridView1.GetType();
            //PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);

            //pi.SetValue(this.dataGridView1, true, null);

        }



        #endregion

        private void metroComboBox_Signal_DropDown(object sender, EventArgs e)
        {
            int width = metroComboBox_Signal.DropDownWidth;
            Graphics g = metroComboBox_Signal.CreateGraphics();
            Font f = metroComboBox_Signal.Font;
            int vertScrollBarWidth = (metroComboBox_Signal.Items.Count > metroComboBox_Signal.MaxDropDownItems) ? SystemInformation.VerticalScrollBarWidth : 0;
            foreach (var item in metroComboBox_Signal.Items)
            {
                var itemWidth = (int)g.MeasureString(item.ToString().Trim(), f).Width + 10 + vertScrollBarWidth;
                if(itemWidth > width)
                {
                    width = itemWidth;
                }
            }
            metroComboBox_Signal.DropDownWidth = width;
        }
    }
}
