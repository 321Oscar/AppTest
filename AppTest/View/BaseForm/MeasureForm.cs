using AppTest.FormType.Helper;
using AppTest.Helper;
using AppTest.Model;
using LPCanControl.CANInfo;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Concurrent;
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
    public partial class MeasureForm : BaseDataForm
    {
        /// <summary>
        /// 计时器
        /// </summary>
        protected readonly Stopwatch sw = new Stopwatch();

        protected int CurvePointMaxCount = 200;

        /// <summary>
        /// 曲线图控件
        /// </summary>
        protected readonly PlotModel plotModel;

        protected readonly Color ScopeColor = Color.FromArgb(0, 170, 173);

        public MeasureForm() : base()
        {
            InitializeComponent();

            this.metroPanelMain.Controls.Add(metroTabControl1);
            //metroTabControl1.Dock = DockStyle.Fill;
            metroTabControl1.Style = MetroFramework.MetroColorStyle.Teal;
            btnCheckOther.BackColor = ScopeColor;
            btnFocus.BackColor = ScopeColor;
            btnGetData.BackColor = ScopeColor;
            this.statusStrip1.BackColor = ScopeColor;
            this.statusStrip1.ForeColor = Color.White;

            this.FormType = FormType.Scope;

            plotModel = new PlotModel()
            {
                Title = "实时数据",
                IsLegendVisible = false,
                //Background = OxyColors.Black,
                //TextColor = OxyColors.White,
            };

            //x轴
            plotModel.Axes.Add(new DateTimeAxis()
            {
                Position = AxisPosition.Bottom,
            });

            //y轴
            //plotModel.Axes.Add(new LinearAxis()
            //{
            //    Position = AxisPosition.Left,
            //    Angle = 60
            //});

            var controller = new PlotController();
            controller.UnbindMouseDown(OxyMouseButton.Right);
            controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
            plotView1.Controller = controller;
            plotView1.Model = plotModel;
        }

        /// <summary>
        /// 曲线图
        /// </summary>
        public MeasureForm(ProtocolType protocolType = ProtocolType.DBC) : this()
        {
            base.MDIModeVisible = protocolType == ProtocolType.DBC;
        }

        private void MeasureForm_Load(object sender, EventArgs e)
        {
            Type dgvType = this.plotView1.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);

            pi.SetValue(this.plotView1, true, null);
        }

        #region override

        protected override void InitSignalUC()
        {
            if (null == Signals || Signals.SignalList.Count == 0)
            {
                this.tableLayoutPanel1.Enabled = false;
                return;
            }

            AddLinetoPlotFromSignal();

            //getDataTimer = new System.Threading.Timer(GetDataTimer_Elapsed,null,10,10);
            //getDataTimer.Elapsed += GetDataTimer_Elapsed;
            InitIDAndProtocolCmd();

            InitStateStrip();

            HistoryDataUC hduc = new HistoryDataUC();
            hduc.ProjectName = this.OwnerProject.Name;
            hduc.FormName = this.Name;
            hduc.Dock = DockStyle.Fill;
            metroTabPage2.Controls.Clear() ;
            metroTabPage2.Controls.Add(hduc);
            hduc.ChangeColorTheme(ScopeColor);
        }

        protected override void ReLoadSignal()
        {
            IsGetdata = false;

            plotModel.Series.Clear();
            panelLegend.Controls.Clear();
            //if (getDataTimer != null)
            //    getDataTimer.Dispose();

            AddLinetoPlotFromSignal();
        }
       
        protected override void ModifiedGetdata(bool get)
        {
            btnGetData.Text = !get ? "Start" : "Stop";//按钮显示
            nudTime.Enabled = !get;//时间间隔
            if (get)
                AddValueToPlottimer.Interval = (int)nudTime.Value;
            AddValueToPlottimer.Enabled = get;//描点定时器
            nudPointCount.Enabled = !get;//曲线点数可修改
            CurvePointMaxCount = (int)nudPointCount.Value;//曲线点数
            RegisterOrUnRegisterDataRecieve(get);
        }

        public override async void OnDataReceiveEvent(object sender, CANDataReceiveEventArgs args)
        {
            base.OnDataReceiveEvent(sender, args);

            await Task.Delay(1000);
            //AddValueToPlottimer_Tick(null,null);
        }
        #endregion override

        #region - Init Curve Controls

        /// <summary>
        /// 曲线图例增加
        /// </summary>
        /// <param name="signal"></param>
        /// <param name="color"></param>
        /// <param name="index"></param>
        protected void AddcheckBox(BaseSignal signal, Color color, int index)
        {
            CheckBoxColorUC cb = new CheckBoxColorUC(signal, color);
            //cb.Name = index.ToString();
            cb.Text = signal.SignalName;
            cb.Tag = signal;
            cb.Dock = DockStyle.Top;
            cb.Index = index;
            //cb.Checked = true;
            cb.UserControlBtnClicked += Cb_CheckedChanged;
            cb.ColorChanged += Cb_ColorChanged;
            this.panelLegend.Controls.Add(cb);
            panelLegend.Controls.SetChildIndex(cb, 0);
        }

        /// <summary>
        /// 添加曲线，Y轴
        /// </summary>
        protected virtual void AddLinetoPlotFromSignal()
        {
            plotModel.Series.Clear();

            while (plotView1.Model.Axes.Count > 1) 
            {
                plotView1.Model.Axes.RemoveAt(1);
            }

            for (int i = 0; i < Signals.SignalList.Count; i++)
            {
                DBCSignal signal = Signals.SignalList[i];
                Signals.SignalList[i].StrValue = "0";
                //增加Y轴
                var existAxes = plotView1.Model.Axes.Where(x => x.Key == signal.SignalName).ToArray();
                if (existAxes.Length > 0)
                    plotView1.Model.Axes.Remove(existAxes[0]);
                plotView1.Model.Axes.Add(new LinearAxis
                {
                    Position = AxisPosition.Left,//i % 2 == 0 ? AxisPosition.Left : AxisPosition.Right,
                    StartPosition = i / (double)Signals.SignalList.Count,
                    EndPosition = (i + 0.8) / (double)Signals.SignalList.Count,
                    AxislineStyle = LineStyle.Solid,
                    TextColor = OxyColor.FromRgb(255, (byte)i, 0),
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Dot,//MaximumPadding
                    Key = signal.SignalName//$"Y{i}"
                }); 

                //增加曲线
                var series = new LineSeries()
                {
                    Color = OxyColor.FromRgb(255, (byte)i, 0),
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.DarkGreen,
                    MarkerType = MarkerType.None,
                    Title = signal.SignalName,
                    InterpolationAlgorithm = null//InterpolationAlgorithms.CanonicalSpline，
                    ,YAxisKey = signal.SignalName//$"Y{i}" //y轴的Key
                };

                plotModel.Series.Add(series);
                AddcheckBox(signal, Color.FromArgb(255, (byte)i, 0), i);
            }

            plotModel.InvalidatePlot(true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cb_CheckedChanged(object sender, EventArgs e)
        {
            CheckBoxColorUC cb = sender as CheckBoxColorUC;
            var lineSer2 = plotView1.Model.Series.Where(x => x.Title == cb.SignalName).ToArray()[0] as LineSeries;
            lineSer2.IsVisible = cb.Checked;
            plotModel.InvalidatePlot(true);
        }

        /// <summary>
        /// 更换颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cb_ColorChanged(object sender, EventArgs e)
        {
            CheckBoxColorUC cb = sender as CheckBoxColorUC;
            ///线条颜色
            //LineSeries series = plotModel.Series[cb.Index] as LineSeries;
            var lineSer2 = plotView1.Model.Series.Where(x => x.Title == cb.SignalName).ToArray()[0] as LineSeries;
            lineSer2.Color = OxyColor.FromRgb(cb.Color.R, cb.Color.G, cb.Color.B);
            ///Y轴颜色
            var lineAxes = plotView1.Model.Axes.Where(x => x.Key == cb.SignalName).ToArray()[0] as LinearAxis;
            lineAxes.TextColor = OxyColor.FromRgb(cb.Color.R, cb.Color.G, cb.Color.B);
            //plotView1.Model.Axes[0].Key = 
            plotModel.InvalidatePlot(true);
        }

        #region PlotModel
        private void btnCheckOther_Click(object sender, EventArgs e)
        {
            foreach (var control in panelLegend.Controls)
            {
                if (control is CheckBoxColorUC uC)
                {
                    uC.Checked = !uC.Checked;
                }
            }
        }

        protected void btnFocus_Click(object sender, EventArgs e)
        {
            plotModel.ResetAllAxes();
            plotModel.InvalidatePlot(true);
        }

        private void cbCheckAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var control in panelLegend.Controls)
            {
                if (control is CheckBoxColorUC uC)
                {
                    uC.Checked = (sender as CheckBox).Checked;
                }
            }
        }

        private void cbXZoom_CheckedChanged(object sender, EventArgs e)
        {
            plotModel.Axes[0].IsZoomEnabled = this.metroCheckBox_X.Checked;
        }

        private void cbYZoom_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 1; i < plotModel.Axes.Count; i++)
            {
                plotModel.Axes[i].IsZoomEnabled = this.metroCheckBox_Y.Checked;
            }
        }

        protected void ClearOxyData()
        {
            foreach (var item in plotView1.Model.Series)
            {
                (item as LineSeries).Points.Clear();
            }
        }
        #endregion PlotModel

        #endregion - Init Curve Controls

        /// <summary>
        /// 曲线描点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddValueToPlottimer_Tick(object sender, EventArgs e)
        {
            AddValuetoPlotter();
        }

        protected virtual void AddValuetoPlotter()
        {
            sw.Reset();
            sw.Restart();
            ShowLog("");
            foreach (var item in Signals.SignalList)
            {
                var lineSer2 = plotView1.Model.Series.Where(x => x.Title == item.SignalName).ToArray()[0] as LineSeries;

                DateTime dt = DateTime.Now;
                plotModel.Axes[0].Maximum = DateTimeAxis.ToDouble(dt.AddSeconds(1));

                lineSer2.Points.Add(new DataPoint(DateTimeAxis.ToDouble(dt), Convert.ToDouble(item.StrValue)));
                LogHelper.WriteToOutput(this.Name, $"{item.SignalName} 添加点【{item.StrValue}】,");
                if (lineSer2.Points.Count > CurvePointMaxCount)
                {
                    lineSer2.Points.RemoveAt(0);
                }
            }
            plotModel.InvalidatePlot(true);
            sw.Stop();
            long times = sw.ElapsedMilliseconds;
            LogHelper.WriteToOutput(this.Name, "描点结束,使用了" + times + "毫秒");
        }

        /// <summary>
        /// 开启/关闭获取数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetData_Click(object sender, EventArgs e)
        {
            GetDataChange();
        }

        protected virtual void GetDataChange()
        {
            if (IsGetdata)
            {
                IsGetdata = false;
            }
            else
            {
                if (nudTime.Value == 0)
                {
                    LeapMessageBox.Instance.ShowInfo("间隔不能为0");
                    return;
                }

                //check can is open 
                if (!USBCanManager.Instance.Exist(OwnerProject))
                {
                    LeapMessageBox.Instance.ShowInfo("CAN未打开!");
                    return;
                }

                //clear data in oxyplot
                ClearOxyData();
                btnFocus_Click(null, null);

                IsGetdata = true;

            }
        }

        #region 弃用-改用事件驱动signal数值变化
        readonly Random r = new Random();
        /// <summary>
        /// 获取数据定时器
        /// </summary>
        //System.Threading.Timer getDataTimer;
        protected Thread getDataThread;

        object synbObj = new object();
        /// <summary>
        /// 数据队列
        /// </summary>
        //private Queue<SignalEntity> signalsQueue = new Queue<SignalEntity>();
        [Obsolete("弃用队列")]
        private ConcurrentQueue<SignalEntity> signalsCurrentQueue = new ConcurrentQueue<SignalEntity>();

        /// <summary>
        /// 获取数据循环
        /// </summary>
        /// <param name="interval"></param>
        protected virtual void GetDataByInterval(object interval)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            do
            {
                sw.Stop();
                long times = sw.ElapsedMilliseconds;
                LogHelper.WriteToOutput(this.Name, "获取数据花费了" + times + "ms");
                sw.Reset();
                sw.Restart();
                GetDataTimer_Elapsed(null);
                Thread.Sleep((int)nudTime.Value);
            } while (IsGetdata);

            LogHelper.WriteToOutput(this.Name, "获取线程结束。");
        }

        //private static readonly object _lock = new object();
        /// <summary>
        /// 获取数据具体方法
        /// </summary>
        /// <param name="sender"></param>
        protected virtual async void GetDataTimer_Elapsed(object sender)
        {

            try
            {
                var date = DateTime.Now;
                List<SignalEntity> signalEntities = new List<SignalEntity>();
                var datatimeStr = DateTime.Now.ToString(Global.DATETIMEFORMAT);
                SignalEntity entity;
#if DEBUG
                //debug 生成数据
                var rx_mails = USBCanManager.Instance.Receive(OwnerProject, CurrentIDs.ToArray(), CanChannel, $"[{this.FormType}] [{this.Name}]");
                if (rx_mails == null || rx_mails.Length == 0)
                {
                    return;
                }
                //var data = protocol.Multip(rx_mails, Signals);
                foreach (var item in Protocol.MultipYield(rx_mails, Signals.SignalList.Cast<BaseSignal>().ToList()))
                {
                    entity = new SignalEntity();
                    entity.DataTime = datatimeStr;
                    entity.ProjectName = OwnerProject.Name;
                    entity.FormName = this.Name;
                    entity.SignalName = item.SignalName;
                    entity.SignalValue = item.StrValue;
                    entity.CreatedOn = DateTime.Now.ToString(Global.DATETIMEFORMAT);
                    signalEntities.Add(entity);

                    //signalsCurrentQueue.Enqueue(entity);
                }

                //foreach (var item in Signals.Signal)
                //{
                //    //从Can口中取数据，并解析
                //    //dt.Columns[item.SignalName] = 
                //    string value = r.Next(2, 2000).ToString();

                //    //signalUC[item.SignalName].SetData(data);
                //    entity = new SignalEntity();
                //    entity.DataTime = datatimeStr;
                //    entity.ProjectName = OwnerProject.Name;
                //    entity.FormName = this.Name;
                //    entity.SignalName = item.SignalName;
                //    entity.SignalValue = value;
                //    entity.CreatedOn = DateTime.Now.ToString(Global.DATETIMEFORMAT);
                //    signalEntities.Add(entity);
                    
                //    signalsCurrentQueue.Enqueue(entity);

                //    LogHelper.WriteToOutput(this.Name, $"缓存数量-{signalsCurrentQueue.Count}");
                //    //var lineSer2 = plotView1.Model.Series.Where(x => x.Title == item.SignalName).ToArray()[0] as LineSeries;
                //    //lineSer2.Points.Add(new DataPoint(DateTimeAxis.ToDouble(date), Convert.ToDouble(value)));
                //    //if (lineSer2.Points.Count > 200)
                //    //{
                //    //    lineSer2.Points.RemoveAt(0);
                //    //}

                //}
#else

                CAN_msg[] rx_mails = USBCanManager.Instance.Receive(OwnerProject,CurrentIDs.ToArray(), CanChannel, $"[{this.FormType}] [{this.Name}]");
                if (rx_mails == null || rx_mails.Length == 0)
                {
                    return;
                }
                var data = Protocol.Multip(rx_mails, Signals);
                foreach (var item in data)
                {
                    entity = new SignalEntity();
                    entity.DataTime = datatimeStr;
                    entity.ProjectName = OwnerProject.Name;
                    entity.FormName = this.Name;
                    entity.SignalName = item.Key.SignalName;
                    entity.SignalValue = item.Value;
                    entity.CreatedOn = DateTime.Now.ToString(Global.DATETIMEFORMAT);
                    signalEntities.Add(entity);

                    signalsCurrentQueue.Enqueue(entity);
                }
#endif
                if (signalEntities.Count > 0 && base.IsSaveData)
                {
                    var dbAsync = await DBHelper.GetDb();
                    var result = await dbAsync.InsertAllAsync(signalEntities);
                    LogHelper.WriteToOutput(this.Name, $"插入数据成功-{result}");
                }
            }
            catch (USBCANOpenException ex)
            {
                IsGetdata = false;
                this.Invoke(new Action<string>((x) =>
                {
                    toolLog.Text = x;
                    // getDataTimer.Dispose();
                }), ex.Message);

            }
            catch (Exception ex)
            {
                //getDataTimer.Change(-1, 0);
                LeapMessageBox.Instance.ShowError(ex);
                IsGetdata = false;
                this.Invoke(new Action<string>((x) =>
                {
                    toolLog.Text = DateTime.Now.ToString() + "：" + x;
                    // getDataTimer.Dispose();
                }), ex.Message);
            }

        }

        #endregion

        private void btnChangeMode_Click(object sender, EventArgs e)
        {
           
        }
        
    }
}
