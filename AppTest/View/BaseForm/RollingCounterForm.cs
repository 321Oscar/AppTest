﻿using AppTest.FormType.Helper;
using AppTest.Helper;
using AppTest.Model;
using AppTest.ProtocolLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace AppTest.FormType
{
    /// <summary>
    /// 自动发送RollingCounter数据
    /// <para>add by xwd 2021-12-20</para>
    /// </summary>
    public partial class RollingCounterForm : BaseDataForm
    {
        #region 属性--变量
        private readonly Color RtlColor = Color.FromArgb(0, 177, 89);
        /// <summary>
        /// Message组
        /// </summary>
        private readonly List<GroupBox> gb = new List<GroupBox>();

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
                    btnAutoSend.Invoke(new Action(() => { btnAutoSend.Text = !value ?"启动":"停止"; }));
                }
                else
                {
                    btnAutoSend.Text = !value ? "启动" : "停止";
                }
                isSend = value;
            }
        }
        #endregion
    
        public RollingCounterForm():base()
        {
            InitializeComponent();

            base.SaveDataVisible = false;
            base.metroPanelMain.Controls.Add(this.tableLayoutPanel1);
            this.FormType = FormType.RollingCounter;

            //this.OnCanChannelChange += DataForm_OnCanChannelChange;

            //timerList = new List<System.Windows.Forms.Timer>();
            //timerList = new List<System.Threading.Timer>();
            timerList = new List<Thread>();
            SignalUC = new Dictionary<string, SignalInfoUC>();
            nudCoe.DecimalPlaces = 2;
            metroComboBox_Signals.AutoCompleteSource = AutoCompleteSource.ListItems;
            metroComboBox_Signals.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            #region StatusStrip

            tscbb = new ToolStripComboBox();
            tscbb.ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            tscbb.ComboBox.Items.AddRange(new string[] { "正常发送", "单次发送", "自发自收", "单次自发自收" });
            tscbb.SelectedIndex = 0;
            ToolStripSeparator tss = new ToolStripSeparator();
            tslbCanIndex = new ToolStripLabel();
            tslbCanIndex.Text = $"CanIndex: {CanChannel}";
            //分隔符
            ToolStripSeparator tss1 = new ToolStripSeparator();
            toolLog = new ToolStripStatusLabel();
            toolLog.ForeColor = Color.White;
            toolLog.Text = "实时日志";
            statusStrip1.Items.AddRange(new ToolStripItem[]
            {
                    tscbb,
                    tss,
                    tslbCanIndex,
                    tss1,
                    toolLog,
            });
            statusStrip1.Visible = true;
            statusStrip1.BackColor = RtlColor;
            statusStrip1.ForeColor = Color.White;
            //this.statusStrip1.Items.AddRange(new ToolStripItem[]
            //{
            //        tscbb,
            //        tss,
            //        tslbCanIndex,
            //        tss1,
            //        this.tsslbLog
            //});

            //PanelLog.Location = new Point(10, this.Height - 30);
            #endregion
        }

        #region Override

        protected override void InitSignalUC()
        {
            if (null == Signals || Signals.SignalList.Count == 0)
                return;

            ClearTimer();
            pnSignals.Controls.Clear();

            //保留原有信号的值
            Dictionary<string, SignalInfoUC> oldSignals = new Dictionary<string, SignalInfoUC>(SignalUC);

            SignalUC.Clear();
            gb.Clear();

            int minHeigt = 100;
            metroComboBox_Signals.DataSource = null;
            for (int i = 0; i < Signals.SignalList.Count; i++)
            {
                SignalInfoUC signalInfoUS = new SignalInfoUC(Signals.SignalList[i], true);
                signalInfoUS.Name = Signals.SignalList[i].SignalName;
                minHeigt = signalInfoUS.Height;
                //signalInfoUS.SetData("0");
                signalInfoUS.Dock = DockStyle.Top;
                SignalUC.Add(Signals.SignalList[i].SignalName, signalInfoUS);

                //保留原有信号的值
                if (oldSignals.ContainsKey(Signals.SignalList[i].SignalName))
                {
                    signalInfoUS.SignalValue = oldSignals[Signals.SignalList[i].SignalName].SignalValue;
                }

                GroupBox groupBox = gb.Find(x => x.Name == Signals.SignalList[i].MessageID + ";" + Signals.SignalList[i].CycleTime);

                if (groupBox == null)
                {
                    groupBox = new GroupBox()
                    {
                        Name = Signals.SignalList[i].MessageID + ";" + Signals.SignalList[i].CycleTime,
                        Text = "ID:" + Signals.SignalList[i].MessageID,
                        AutoSize = true,
                        //Tag = Signals.Signal[i].CycleTime
                    };
                    groupBox.Resize += GroupBox_Resize;
                    gb.Add(groupBox);
                    groupBox.Dock = DockStyle.Top;
                    pnSignals.Controls.Add(groupBox);
                    FlowLayoutPanel flp = new FlowLayoutPanel();
                    //Type dgvType = flp.GetType();
                    //PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);

                    //pi.SetValue(flp, true, null);
                    flp.Dock = DockStyle.Fill;
                    flp.AutoSize = true;
                    groupBox.Controls.Add(flp);
                }
                foreach (Control item in groupBox.Controls)
                {
                    if (item is FlowLayoutPanel)
                    {
                        item.Controls.Add(signalInfoUS);
                    }
                }
            }

            this.MinimumSize = new Size(0, minHeigt + plSend.Height + 70);

            metroComboBox_Signals.DataSource = Signals.SignalList;
            //metroComboBox_Signals.DisplayMember = "SignalName";


            //getDataTimer = new System.Timers.Timer();
            //getDataTimer.Elapsed += GetDataTimer_Elapsed;

            if (ProtocolCommand != null)
            {
                CanIndexItem canIndex = OwnerProject.CanIndex.Find(x => x.CanChannel == CanChannel);
                Protocol = ReflectionHelper.CreateInstance<BaseProtocol>(ProtocolCommand, Assembly.GetExecutingAssembly().ToString(), new string[] { canIndex.ProtocolFileName });
            }

            AddSendThread();
        }

        protected override void ReLoadSignal()
        {
            //base.ReLoadSignal();
            

            InitSignalUC();
        }
        #endregion

        #region Event

        /// <summary>
        /// 启动定时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAutoSend_Click(object sender, EventArgs e)
        {
            //启动所有
            if (isSend)
            {
                ClearTimer();
                IsSend = false;
                //btnAutoSend.Text = "启动";
                // pnSignals.Enabled = true;
            }
            else
            {
                //测试CAN盒是否能否发送数据
                if (TestCanSend())
                {
                    ShowLog("");
                    AddSendThread();
                }
            }
        }

        private bool TestCanSend()
        {
            string errorlog = string.Empty;

            bool send = USBCanManager.Instance.SendTest(OwnerProject, ref errorlog, canindex: this.CanChannel);
            if (!send)
            {
                MessageBox.Show(errorlog);
                ShowLog(errorlog);
            }

            return send;
        }

        private void GroupBox_Resize(object sender, EventArgs e)
        {
            if (this.IsSized)
                return;
            GroupBox gb = sender as GroupBox;
            int signalWidth = 0;
            int signalHeight = 0;
            int signalCount = 0;
            int flpHeight = 0;
            foreach (Control item in gb.Controls)
            {
                if (item is FlowLayoutPanel)
                {
                    foreach (Control signalUC in item.Controls)
                    {
                        if (signalUC is SignalInfoUC)
                        {
                            signalWidth = signalUC.Width;
                            signalHeight = signalUC.Height;
                            signalCount++;
                        }
                    }
                    flpHeight = item.Height;
                }
                //每行N个
                int n = 1;
                if (signalWidth != 0)
                    n = gb.Width / signalWidth;

                //行
                int row = n == 0 ? signalCount : signalCount / n + 1;

                if (n == 0)
                {
                    row = signalCount;
                   // row++;
                }
                else if (n > signalCount)
                {
                    row = 1;
                }
                else
                {
                    if (signalCount % n > 0)
                    {
                        row = signalCount / n + 1;
                    }
                    else
                    {
                        row = signalCount / n;
                    }
                }

                gb.Height = (signalHeight) * (row) + item.Location.Y;
            }
        }

        private void RollingCounterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            isSend = false;
        }

        #endregion

        #region private Method
        
        private void AddSendThread()
        {
            for (int i = 0; i < pnSignals.Controls.Count; i++)
            {
                Control ct = pnSignals.Controls[i];
                if (ct is GroupBox)
                {
                    int time = int.Parse(ct.Name.Split(new char[] { ';' })[1]);
                    if (time == 0)
                    {
                        LeapMessageBox.Instance.ShowInformation("不支持周期为0的信号!");
                        return;
                    }
                    //System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                    //timer.Interval = time;
                    //timer.Tick += new EventHandler((o, ev) => Timer_Tick(o, ev, ct as GroupBox));
                    //timer.Enabled = true;
                    Thread t = new Thread(new ParameterizedThreadStart(AutoSendThread));
                    t.Name = ct.Name;
                    t.IsBackground = true;
                    LogHelper.WriteToOutput( this.Name, $"{ct.Name} 发送线程启动");
                    t.Start(ct);
                    //System.Threading.Timer timer = new System.Threading.Timer(callback: Send, state: ct, 0, time);
                    //timer.Change(0, (int)1000);

                    timerList.Add(t);
                    IsSend = true;
                }
            }
            btnAutoSend.Text = "停止";
        }

        /// <summary>
        /// 发送数据线程
        /// </summary>
        /// <param name="groupbox">一个ID一个Groupbox，按照ID组帧发送</param>
        private void AutoSendThread(object groupbox)
        {
            GroupBox gb = groupbox as GroupBox;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            LogHelper.WriteToOutput( this.Name, gb.Name + "启动发送");
            do
            {
                sw.Stop();
                long times = sw.ElapsedMilliseconds;
                LogHelper.WriteToOutput(this.Name, gb.Name + "发送结束,使用了" + times + "毫秒");
                sw.Reset();
                sw.Restart();
                int time = int.Parse(gb.Name.Split(new char[] { ';' })[1]);
                Send(groupbox);

                //修正
                Thread.Sleep(time - 2);

            } while (isSend);
            LogHelper.WriteToOutput(this.Name, gb.Name + "停止发送");
        }

        private void ClearTimer()
        {
            isSend = false;
            //foreach (var item in timerList)
            //{
            //    item.Abort();
            //}
            timerList.Clear();
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="groupbox"></param>
        private void Send(object groupbox)
        {
            //SignalEqualityComarer signalEqualityComarer = new SignalEqualityComarer();
            Dictionary<BaseSignal, string> keyValues = new Dictionary<BaseSignal, string>();
            if (!(groupbox is GroupBox gb))
                return;
            if (!isSend)
                return;
            try
            {
                bool needCheckSum = false;
                Control lfp = gb.Controls[0];

                needCheckSum = false;
                if (lfp is FlowLayoutPanel)
                {
                    foreach (var item in lfp.Controls)
                    {
                        if (item is SignalInfoUC uS)
                        {
                            string value = uS.SignalValue;
                            if (uS.Signal.SignalName.ToLower().Contains("rolling"))
                            {
                                if (int.Parse(value) == 15)
                                {
                                    //uS.SetData("0");
                                    uS.SignalValue = "0";
                                }
                                else
                                {
                                    int nextValue = int.Parse(value) + 1;
                                    uS.SignalValue = nextValue.ToString();
                                }
                            }
                            if (uS.Signal.SignalName.ToLower().Contains("checksum"))
                                needCheckSum = true;

                            keyValues.Add(uS.Signal, value);
                        }
                    }
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
                /*
                * 模拟发送成功
                   SetLog("ID:" + gb.Name +
                     $"--{DateTime.Now}--已发送：{frame[0].cid:X},DATA:{frame[0].w[0]:X} {frame[0].w[1]:X} {frame[0].w[2]:X} {frame[0].w[3]:X} {frame[0].w[4]:X} {frame[0].w[5]:X} {frame[0].w[6]:X} {frame[0].w[7]:X}\n");

               */
                USBCanManager.Instance.Send(OwnerProject, canindex: this.CanChannel, sendData: frame[0], $"[{this.FormType}]{this.Name}");
            }
            catch (Exception ex)
            {
                LogHelper.Error(this.Name, ex);
                ShowLog(ex.Message);
                IsSend = false;
                var thread = timerList.Find(x => x.Name == gb.Name);
                if (thread != null)
                {
                    thread.Abort();
                }
            }
        }

        #endregion

        private void cbbSignals_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (metroComboBox_Signals.SelectedIndex != -1)
            {
                DBCSignal selectSignal = metroComboBox_Signals.SelectedItem as DBCSignal;

                metroTextBox_CurVal.Text = SignalUC[selectSignal.SignalName].SignalValue;
            }
        }

        private void btnChangeValue_Click(object sender, EventArgs e)
        {
            if (metroComboBox_Signals.SelectedIndex != -1)
            {
                DBCSignal selectSignal = metroComboBox_Signals.SelectedItem as DBCSignal;

                decimal oldValue = Convert.ToDecimal(SignalUC[selectSignal.SignalName].SignalValue);

                decimal newValue;

                Button btn = sender as Button;
                try
                {
                    switch (btn.Name)
                    {
                        case "btnAdd":
                            newValue = oldValue + nudCoe.Value;
                            break;
                        case "btnReduce":
                            newValue = oldValue - nudCoe.Value;
                            break;
                        case "btnMultip":
                            newValue = oldValue * nudCoe.Value;
                            break;
                        case "btnDivision":
                            newValue = oldValue / nudCoe.Value;
                            break;
                        default:
                            return;
                    }

                }
                catch (Exception ex)
                {
                    LeapMessageBox.Instance.ShowError(ex);
                    return;
                }
               
                //newValue = oldValue + nudCoe.Value;
                SignalUC[selectSignal.SignalName].SignalValue = newValue.ToString();
                cbbSignals_SelectedIndexChanged(null, null);

                //this.btnSetValue_Click(null, null);
            }
        }

        private void btnSetValue_Click(object sender, EventArgs e)
        {
            if (metroComboBox_Signals.SelectedIndex != -1 && !string.IsNullOrEmpty(metroTextBox_CurVal.Text.Trim()))
            {
                DBCSignal selectSignal = metroComboBox_Signals.SelectedItem as DBCSignal;
                SignalUC[selectSignal.SignalName].SignalValue = metroTextBox_CurVal.Text.Trim();
            }
        }

        private void tbCurrent_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSetValue_Click(null,null);
            }
        }

    }

}