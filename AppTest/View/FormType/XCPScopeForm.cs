using AppTest.Helper;
using AppTest.Model;
using AppTest.ViewModel;
using LPCanControl.CANInfo;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppTest.FormType
{
    public partial class XCPScopeForm : MeasureForm
    {
        XCPPollingViewModel vm;

        public XCPScopeForm() : base(protocolType: ProtocolType.XCP)
        {
            InitializeComponent();

            vm = new XCPPollingViewModel(this);
        }

        #region override
        protected override void InitSignalUC()
        {
            vm.XCPSignals = FormItem.XCPSingals;

            if (null == FormItem.XCPSingals || FormItem.XCPSingals.xCPSignalList.Count == 0)
            {
                tableLayoutPanel1.Enabled = false;
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
            metroTabPage2.Controls.Clear();
            metroTabPage2.Controls.Add(hduc);
            hduc.ChangeColorTheme(ScopeColor);

            ProjectForm parent = (ProjectForm)this.MdiParent;
            vm.XcpModule = parent.XcpModule;
        }

        protected override void AddLinetoPlotFromSignal()
        {
            plotModel.Series.Clear();

            while (plotView1.Model.Axes.Count > 1)
            {
                plotView1.Model.Axes.RemoveAt(1);
            }

            for (int i = 0; i < vm.XCPSignals.xCPSignalList.Count; i++)
            {
                var signal = vm.XCPSignals.xCPSignalList[i];
                vm.XCPSignals.xCPSignalList[i].StrValue = "0";
                //增加Y轴
                var existAxes = plotView1.Model.Axes.Where(x => x.Key == signal.SignalName).ToArray();
                if (existAxes.Length > 0)
                    plotView1.Model.Axes.Remove(existAxes[0]);
                plotView1.Model.Axes.Add(new LinearAxis
                {
                    Position = AxisPosition.Left,//i % 2 == 0 ? AxisPosition.Left : AxisPosition.Right,
                    StartPosition = i / (double)vm.XCPSignals.xCPSignalList.Count,
                    EndPosition = (i + 0.8) / (double)vm.XCPSignals.xCPSignalList.Count,
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
                    ,
                    YAxisKey = signal.SignalName//$"Y{i}" //y轴的Key
                };

                plotModel.Series.Add(series);
                AddcheckBox(signal, Color.FromArgb(255, (byte)i, 0), i);
            }

            plotModel.InvalidatePlot(true);
        }

        protected override void ReLoadSignal()
        {
            base.ReLoadSignal();
        }

        protected override void ModifiedGetdata(bool get)
        {
            base.ModifiedGetdata(get);
            if (get)
            {
                getDataThread = new Thread(new ParameterizedThreadStart(GetDataByInterval));
                getDataThread.IsBackground = true;
                getDataThread.Start((int)nudTime.Value);
            }
            else
            {
                if (getDataThread != null)
                {
                    getDataThread.Abort();
                    getDataThread = null;
                }
            }
                //启动定时器
        }

        public override void OnDataReceiveEvent(object sender, CANDataReceiveEventArgs args)
        {
            return;
            //base.OnDataRecieveEvent(sender, args);
        }

        protected override void AddValuetoPlotter()
        {
            return;
            sw.Reset();
            sw.Restart();
            ShowLog("");
            foreach (var item in vm.XCPSignals.xCPSignalList)
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

        protected override async void GetDataTimer_Elapsed(object sender)
        {
            try
            {
                //DataRow dr = dt.NewRow();
                List<SignalEntity> signalEntities = new List<SignalEntity>();
                var datatimeStr = DateTime.Now.ToString(Global.DATETIMEFORMAT);
                SignalEntity entity;// = new SignalEntity();

                ProjectForm parent = (ProjectForm)this.MdiParent;
                parent.XcpModule.ClearQueue();

                foreach (var item in vm.XCPSignals.xCPSignalList)
                {
                    XCPSignal signal = item as XCPSignal;
                    if (signal.Length <= 6)
                    {
                        parent.XcpModule.ShortUpload(ref signal, (uint)this.CanChannel);
                    }
                    else
                    {
                        parent.XcpModule.Upload(ref signal, (uint)this.CanChannel);
                    }

                    if (parent.XcpModule.CurrentCMDStatus == XCPCMDStatus.UploadFail)
                    {
                        //ShowLog($"{item.Name} Fail");
                        continue;
                    }
                    item.StrValue = signal.StrValue;
                    AddPointToSer(Convert.ToDouble(item.StrValue), item.SignalName);
                    entity = new SignalEntity();
                    entity.DataTime = datatimeStr;
                    entity.ProjectName = OwnerProject.Name;
                    entity.FormName = this.Name;
                    entity.SignalName = item.SignalName;
                    entity.SignalValue = signal.StrValue;
                    entity.CreatedOn = DateTime.Now.ToString(Global.DATETIMEFORMAT);
                    signalEntities.Add(entity);

                }
                if (signalEntities.Count > 0 && base.IsSaveData)
                {
                    LogHelper.Warn($"FormName:{this.Name};ThreadID:{Thread.CurrentThread.ManagedThreadId };Log:Start Save db.");
                    var dbAsync = await DBHelper.GetDb();
                    var result = await dbAsync.InsertAllAsync(signalEntities);
                    LogHelper.Warn($"FormName:{this.Name};ThreadID:{Thread.CurrentThread.ManagedThreadId };Log:Save Success，Counter:{result}.");
                    return;
                }
            }
            catch (Exception ex)
            {
                IsGetdata = false;
                //ShowLog(ex.Message);
                LogHelper.Error(this.Name, ex);
            }
        }

        private void AddPointToSer(double val, string serName)
        {
            var lineSer2 = plotView1.Model.Series.Where(x => x.Title == serName).ToArray()[0] as LineSeries;
            if (lineSer2 == null)
                return;
            DateTime dt = DateTime.Now;
            plotModel.Axes[0].Maximum = DateTimeAxis.ToDouble(dt.AddSeconds(1));

            lineSer2.Points.Add(new DataPoint(DateTimeAxis.ToDouble(dt), val));
            LogHelper.WriteToOutput(this.Name, $"{serName} 添加点【{val}】,");
            if (lineSer2.Points.Count > CurvePointMaxCount)
            {
                lineSer2.Points.RemoveAt(0);
            }
            plotModel.InvalidatePlot(true);
        }
        #endregion
    }
}
