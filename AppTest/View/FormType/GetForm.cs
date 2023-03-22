using AppTest.FormType.Helper;
using AppTest.Helper;
using AppTest.Model;
using AppTest.ProtocolLib;
using LPCanControl.CANInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppTest.FormType
{
    public partial class GetForm : BaseDataForm
    {
        //const string FileDirectory = @"/SaveData/Get/";
        /// <summary>
        /// 生成测试数据随机数
        /// </summary>
        readonly Random r = new Random();

        /// <summary>
        /// 获取数据定时器
        /// </summary>
        //System.Threading.Timer getDataTimer;
        //Thread getDataThread;
        Task getDataTask;
        CancellationTokenSource cancellation;

        bool isGetdata = false;

        /// <summary>
        /// 获取信号
        /// </summary>
        public GetForm()
        {
            InitializeComponent();

            this.FormType = FormType.Get;

            SignalUC = new Dictionary<string, SignalInfoUC>();
            RegisterCancelToken();
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

        /// <summary>
        /// 是否在获取数据，更改按钮可用性
        /// </summary>
        public bool IsGetdata {
            get => isGetdata;
            set { isGetdata = value;
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        btnGet.Text = !value ? "Start" : "Stop";
                        lbLog.Visible = !value;
                        nudTimerInterval.Enabled = !value;
                    }));
                }
                else
                {
                    btnGet.Text = !value ? "Start" : "Stop";
                    lbLog.Visible = !value;
                    nudTimerInterval.Enabled = !value;
                }
                isGetdata = value;
            }
        }

        //private List<int> currentIDs;

        protected override void InitSignalUC()
        {
            if (null == Signals || Signals.SignalList.Count == 0)
                return;

            AddSignalToPanel(out int minHeight);

            this.MinimumSize = new Size(0, minHeight + panel1.Height + 70);

            //getDataTimer = new System.Timers.Timer();
            //getDataTimer.Elapsed += GetDataTimer_Elapsed;

            if (ProtocolCommand != null)
            {
                Protocol = ReflectionHelper.CreateInstance<BaseProtocol>(ProtocolCommand, Assembly.GetExecutingAssembly().ToString());
            }

            HistoryDataUC hduc = new HistoryDataUC();
            hduc.ProjectName = this.OwnerProject.Name;
            hduc.FormName = this.Name;
            hduc.Dock = DockStyle.Fill;
            tabPage2.Controls.Clear();
            tabPage2.Controls.Add(hduc);
        }

        protected override void ReLoadSignal()
        {
            AddSignalToPanel(out int minHeight);

            this.MinimumSize = new Size(0, minHeight + panel1.Height + 70);
        }

        private void AddSignalToPanel(out int minHeight)
        {
            minHeight = 50;

            SignalUC.Clear();
            pnSignals.Controls.Clear();
            IsGetdata = false;

            CurrentIDs = new List<int>();

            int minHeigt = this.gbSignals.Location.Y;
            //Signals.Signal.Sort();
            ///modified by xwd 2022-02-17
            ///要求特定信号优先显示，排序
            ///dock.Top 先添加的control显示在下方
            ///倒序添加
            for (int i = Signals.SignalList.Count - 1; i > -1; i--)
            {
                SignalInfoUC signalInfoUS = new SignalInfoUC(Signals.SignalList[i], true);
                minHeigt = signalInfoUS.Height;
                signalInfoUS.Dock = DockStyle.Top;
                pnSignals.Controls.Add(signalInfoUS);
                signalInfoUS.Show();

                SignalUC.Add(Signals.SignalList[i].SignalName, signalInfoUS);

                if (!CurrentIDs.Contains(int.Parse(Signals.SignalList[i].MessageID, System.Globalization.NumberStyles.HexNumber)))
                {
                    CurrentIDs.Add(int.Parse(Signals.SignalList[i].MessageID, System.Globalization.NumberStyles.HexNumber));
                }
            }

        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            lbLog.Text = "";
            if ((double)nudTimerInterval.Value == 0.0)
            {
                LeapMessageBox.Instance.ShowInfo("间隔时间不能为0");
                return;
            }
            if (IsGetdata)//正在获取数据
            {
                //getDataTimer.Dispose();
                //getDataTimer = null;
                IsGetdata = false;
                cancellation.Cancel();
                getDataTask = null;
            }
            else
            {
                //getDataTimer = new System.Threading.Timer(GetDataTimer_Elapsed, null, 0, (int)nudTimerInterval.Value);
                //getDataThread = new Thread(new ThreadStart(AutoGetByThread));
                //getDataThread.IsBackground = true;
                //getDataThread.Start();
                IsGetdata = true;
                RegisterCancelToken();
                getDataTask = new Task(new Action(AutoGetByThread),this.cancellation.Token);
                getDataTask.Start();
            }

        }

        private async void GetDataTimer_Elapsed(object sender)//, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                //DataRow dr = dt.NewRow();
                List<SignalEntity> signalEntities = new List<SignalEntity>();
                var datatimeStr = DateTime.Now.ToString(Global.DATETIMEFORMAT);
                SignalEntity entity;// = new SignalEntity();
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
                //protocol.Multip(new CAN_msg[] { new CAN_msg(1707, new byte[] { 255, 255, 254, 54, 60, 128, 0, 0 })}, Signals);
                if (null == rx_mails)
                    throw new Exception("接收数据错误。");

                foreach (var item in Protocol.MultipYeild(rx_mails, Signals.SignalList.Cast<BaseSignal>().ToList()))
                {
                    SignalUC[item.SignalName].SignalValue = item.StrValue;
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
                    //dbAsync.
                    //using (var db = new SignalDB($"{Environment.CurrentDirectory}{Global.DBPATH}"))
                    //{
                    //    int count = db.InsertAll(signalEntities);
                    //}
                }

                signalEntities = null;
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                return;
#region 保存为CSV文件
                //dt.Rows.Add(dr);
                ////
                //string filepath = Application.StartupPath + FileDirectory + this.Name + DateTime.Now.ToString("yyyyMMdd") + ".csv";
                ////var fileInfo = new FileInfo(Application.StartupPath + FileDirectory + fileName + ".csv");
                //var fileInfo = new FileInfo(filepath);
                ////string path = ;
                //if (!fileInfo.Directory.Exists)
                //{
                //    fileInfo.Directory.Create();
                //}
                //if (!fileInfo.Exists)
                //{
                //    var fs = new FileStream(filepath, FileMode.Create, FileAccess.Write);

                //    //写列名称
                //    var sw = new StreamWriter(fs, encoding: Encoding.UTF8);
                //    var data = "";
                //    for (int i = 0; i < dt.Columns.Count; i++)
                //    {
                //        data += dt.Columns[i].ColumnName;
                //        if (i < dt.Columns.Count - 1)
                //        {
                //            data += ",";
                //        }
                //    }
                //    sw.WriteLine(data);
                //    sw.Close();
                //    fs.Close();
                //}
                ////string filepath = Application.StartupPath + FileDirectory + this.Name + DateTime.Now.ToString("yyyyMMdd") + ".csv";
                //var swOpen = File.AppendText(filepath);
                //string dataOpen = "";
                //for (int i = 0; i < dt.Columns.Count; i++)
                //{
                //    var str = dt.Rows[dt.Rows.Count - 1][i].ToString();
                //    str = str.Replace("\"", "\"\"");
                //    if (str.Contains(',') || str.Contains('"')
                //        || str.Contains('\r') || str.Contains('\n'))
                //    {
                //        str = string.Format("\"{0}\"", str);
                //    }

                //    dataOpen += str;
                //    if (i < dt.Columns.Count - 1)
                //    {
                //        dataOpen += ",";
                //    }
                //}
                //swOpen.WriteLine(dataOpen);
                //swOpen.Close();
                //dt.Rows.RemoveAt(dt.Rows.Count - 1);
                //fsOpen.Close();
#endregion
            }
            catch (USBCANOpenException usbex)
            {
                //getDataTimer.Dispose();
                IsGetdata = false;
                this.Invoke(new Action<string>((x) => {
                    lbLog.Text = x;
                    toolTip1.SetToolTip(lbLog, x);
                }), usbex.Message);
            }
            catch (Exception ex)
            {
				//getDataTimer.Dispose();
                IsGetdata = false;
                this.Invoke(new Action<string>((x) => {
                    lbLog.Text = x;
                    toolTip1.SetToolTip(lbLog, x);
                }), ex.Message);
                LogHelper.Error(this.Name, ex);
            }
        }

        private void AutoGetByThread()
        {
            while (isGetdata)
            {
                GetDataTimer_Elapsed(null);
                Thread.Sleep((int)nudTimerInterval.Value);
            }
            LogHelper.WriteToOutput(this.Name, "结束获取数据。");
        }

        private void GetForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (getDataTimer != null)
            //    getDataTimer.Dispose();
            
        }

        private void GetForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isGetdata)
            {
                isGetdata = false;
                //LeapMessageBox.Instance.ShowInformation("请先取消自动获取数据。"); 
                //MessageBox.Show(,"提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                //e.Cancel = true;
                //return;
            }
        }
    }
}
