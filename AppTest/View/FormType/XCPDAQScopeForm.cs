using AppTest.FormType.Helper;
using AppTest.Helper;
using AppTest.Model;
using AppTest.ViewModel;
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
    public partial class XCPDAQScopeForm : MeasureForm
    {
        readonly XCPDAQGetViewModel vm;
        readonly DependencyXCPDAQSignals _dependencyXCPDAQSignals;


        public XCPDAQScopeForm(DependencyXCPDAQSignals dependencyXCPDAQSignals) : base(ProtocolType.XCP)
        {
            InitializeComponent();

            vm = new XCPDAQGetViewModel(this);

            _dependencyXCPDAQSignals = dependencyXCPDAQSignals;
            //plotView1.Model.Axes.Clear();

            //plotView1.Model.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom});
        }

        protected override void InitSignalUC()
        {
            if (null == this.FormItem.XCPSingals || this.FormItem.XCPSingals.xCPSignalList.Count == 0)
            {
                this.tableLayoutPanel1.Enabled = false;
                return;
            }

            vm.XCPSignals = this.FormItem.XCPSingals;
            _dependencyXCPDAQSignals.Add(this.FormItem.XCPSingals, this.Name);

            //vm.XCPSignals = new XCPSignals() { xCPSignalList = _dependencyXCPDAQSignals.XCPSignalsByForm[this.Name]};

            AddLinetoPlotFromSignal();

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
                XCPSignal signal = vm.XCPSignals.xCPSignalList[i];
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
                    Key = signal.SignalName,//$"Y{i}"
                    StringFormat ="g10"
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
                    InterpolationAlgorithm = null,//InterpolationAlgorithms.CanonicalSpline，
                    YAxisKey = signal.SignalName,//$"Y{i}" //y轴的Key
                    BrokenLineColor = OxyColors.Automatic,
                    BrokenLineThickness = 0.5,
                    BrokenLineStyle = LineStyle.Dot,
            };

                plotModel.Series.Add(series);
                AddcheckBox(signal, Color.FromArgb(255, (byte)i, 0), i);
            }

            plotModel.InvalidatePlot(true);
        }

        protected override void ModifiedGetdata(bool get)
        {
            //if (get)
            //{
            //    var initDaqTrue = _dependencyXCPDAQSignals.InitDAQ((uint)this.CanChannel);
            //    if (!initDaqTrue)
            //    {
            //        LeapMessageBox.Instance.ShowInfo($"DAQ 配置{(initDaqTrue ? "成功" : "失败")}");
            //        return;
            //    }
            //    vm.XcpModule.StartStopDAQ(0x01, (uint)this.CanChannel);
            //    LeapMessageBox.Instance.ShowInfo($"DAQ 启动");
            //}
            //else
            //{
            //    /// stop daq
            //    vm.XcpModule.StartStopDAQ(0x00, (uint)this.CanChannel);
            //}
            MidifiedControl(get);
            //_dependencyXCPDAQSignals.RegisterOrUnRegisterDataRecieve(get, OwnerProject, CanChannel);
        }

        public override void OnDataRecieveEvent(object sender, CANDataRecieveEventArgs args)
        {
            return;
            //_dependencyXCPDAQSignals.OnDataRecieveEvent(sender, args);
            //}
        }

        protected override void AddValuetoPlotter(object obj)
        {
            int timeSpan = (int)obj;
            do
            {
                if (plotView1.InvokeRequired)
                {
                    plotView1.Invoke(new Action(() => {
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
                    }));
                }

                Thread.Sleep(timeSpan);
            } while (IsGetdata);

            return;

            ShowLog("");
            foreach (var item in vm.XCPSignals.xCPSignalList)
            {
                var lineSer2 = plotView1.Model.Series.Where(x => x.Title == item.SignalName).ToArray()[0] as LineSeries;
#if DEBUG
                DateTime dt = DateTime.Now;
                plotModel.Axes[0].Maximum = DateTimeAxis.ToDouble(dt.AddSeconds(1));

                lineSer2.Points.Add(new DataPoint(DateTimeAxis.ToDouble(dt), Convert.ToDouble(item.StrValue)));
                Console.WriteLine( $"{item.SignalName} 添加点【{item.StrValue}】,", this.Name);
                plotModel.Axes[0].Maximum = DateTimeAxis.ToDouble(dt.AddSeconds(1));
#else //使用时间戳作为X轴
                plotModel.Axes[0].Maximum = item.TimeStamp + 1;
                if(lineSer2.Points.Count > 0)
                {
                    ///上一个点的时间戳
                    var perTime = lineSer2.Points[lineSer2.Points.Count - 1].X;
                    var interval = (item.TimeStamp - perTime);
                    if (interval == 0)//相同时间戳
                        continue;
                    else if(interval < 0)//时间戳重新计数
                    {
                        lineSer2.Points.Clear();
                        lineSer2.Points.Add(new DataPoint(item.TimeStamp, Convert.ToDouble(item.StrValue)));
                        LogHelper.WriteToOutput(this.Name, $"{item.SignalName} 添加点【{item.TimeStamp},{item.StrValue}】");
                    }
                    else
                    {
                        int lostPointCount = (int)(interval / item.CycleTime) - 1;
                        for (int i = 0; i < lostPointCount; i++)
                        {
                            lineSer2.Points.Add(DataPoint.Undefined);
                            LogHelper.WriteToOutput(this.Name, $"{item.SignalName} 丢失点【{perTime + (i+1)*item.CycleTime}】");
                        }
                        lineSer2.Points.Add(new DataPoint(item.TimeStamp, Convert.ToDouble(item.StrValue)));
                        LogHelper.WriteToOutput(this.Name, $"{item.SignalName} 添加点【{item.TimeStamp},{item.StrValue}】");
                    }

                }
                else
                {
                    lineSer2.Points.Add(new DataPoint(item.TimeStamp, Convert.ToDouble(item.StrValue)));
                    LogHelper.WriteToOutput(this.Name, $"{item.SignalName} 添加点【{item.TimeStamp},{item.StrValue}】");
                }
#endif
                if (lineSer2.Points.Count > CurvePointMaxCount)
                {
                    lineSer2.Points.RemoveAt(0);
                }
            }
            plotModel.InvalidatePlot(true);
        }

        protected override void ModifiedSignals()
        {
            vm.ModifiedSignals();
            ReLoadSignal();
        }

    }
}
