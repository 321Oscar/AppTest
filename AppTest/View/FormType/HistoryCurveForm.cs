﻿using AppTest.FormType.Helper;
using AppTest.Helper;
using AppTest.Model;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppTest.FormType
{
    public partial class HistoryCurveForm : BaseForm
    {
        private const string Value = "DataTime";
        PlotModel plotModel;

        /// <summary>
        /// X 轴坐标
        /// </summary>
        public string XAxes { get; set; } = Value;

        public HistoryCurveForm()
        {
            InitializeComponent();

            this.Font = Global.CurrentFont; 

            plotModel = new PlotModel()
            {
                Title = "历史数据",
                IsLegendVisible = false
            };

            //x轴
            plotModel.Axes.Add(new DateTimeAxis()
            {
                Position = AxisPosition.Bottom,
            });

            //var annotation1 = new LineAnnotation()
            //{
            //    Type = LineAnnotationType.Vertical,
            //    X = 5,
            //    LineStyle = LineStyle.Dash,
            //    ClipByXAxis=true,
            //};
            //plotModel.Annotations.Add(annotation1);
            //y轴
            //plotModel.Axes.Add(new LinearAxis()
            //{
            //    Position = AxisPosition.Left,
            //    Angle = 60
            //});

            //var controller = new PlotController();
            //controller.UnbindMouseDown(OxyMouseButton.Right);
            //controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
            //plotView1.Controller = controller;

            #region RectangleAnnotation
            //var range = new RectangleAnnotation { Fill = OxyColor.FromAColor(120, OxyColors.SkyBlue), MinimumX = 0, MaximumX = 0 };
            //plotModel.Annotations.Add(range);

            //double startx = double.NaN;

            //plotModel.MouseDown += (s, e) =>
            //{
            //    if (e.ChangedButton == OxyMouseButton.Left)
            //    {
            //        startx = range.InverseTransform(e.Position).X;
            //        range.MinimumX = startx;
            //        range.MaximumX = startx;
            //        plotModel.InvalidatePlot(true);
            //        e.Handled = true;
            //    }
            //};
            //plotModel.MouseMove += (s, e) =>
            //{
            //    if (!double.IsNaN(startx))
            //    {
            //        var x = range.InverseTransform(e.Position).X;
            //        range.MinimumX = Math.Min(x, startx);
            //        range.MaximumX = Math.Max(x, startx);
            //        range.Text = string.Format("{0:0.00}", range.MaximumX - range.MinimumX);
            //        plotModel.Subtitle = string.Format("Integrating from {0:0.00} to {1:0.00}", range.MinimumX, range.MaximumX);
            //        plotModel.InvalidatePlot(true);
            //        e.Handled = true;
            //    }
            //};

            //plotModel.MouseUp += (s, e) =>
            //{
            //    startx = double.NaN;
            //};
            #endregion

            plotView1.Model = plotModel;
            backgroundWorker1.DoWork += BackgroundWorker1_DoWork;
            //backgroundWorker1.DoWork += (o, ea) => {
            //    Worker_DoWork(null);
            //};
            backgroundWorker1.RunWorkerCompleted += (o, ea) =>
            {
                LeapMessageBox.Instance.ShowInfo("加载完成");
            };

            //load x for entity's int or datetime property
            Type listType = typeof(SignalEntity);
            PropertyInfo[] properties = listType.GetProperties();

            foreach (var item in properties)
            {
                if (item.Name.Contains(Value) || item.PropertyType == typeof(int) || item.PropertyType == typeof(uint))
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem(item.Name);
                    tsmi.Click += Tsmi_Click;
                    changeXToolStripMenuItem.DropDownItems.Add(tsmi);
                }
            }
        }

        /// <summary>
        /// 加载Excel数据的历史曲线图
        /// </summary>
        /// <param name="isImport"></param>
        public HistoryCurveForm(bool isImport) : this()
        {
            importToolStripMenuItem.Enabled = isImport;

        }
        private IList<SignalEntity> signalEntities;
        public HistoryCurveForm(List<SignalEntity> entities) : this(false)
        {
            signalEntities = entities;
            AddSeriesByEntity(entities);

            backgroundWorker1.RunWorkerAsync(signalEntities);
        }

        /// <summary>
        /// change x 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tsmi_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item) 
            {
                XAxes = item.Text;
                //clear axes and point
                plotModel.Axes.Clear();
                foreach (LineSeries ser in plotModel.Series.OfType<LineSeries>())
                {
                    ser.Points.Clear();
                }
                //add new bottom axes
                if (XAxes.Contains(Value))
                {
                    plotModel.Axes.Add(new DateTimeAxis() { Position = AxisPosition.Bottom, Title = XAxes });
                }
                else
                {
                    plotModel.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = XAxes });
                }
                //add point
                backgroundWorker1.RunWorkerAsync(signalEntities);
            }
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            List<SignalEntity> objs = e.Argument as List<SignalEntity>;
            Worker_DoWork(objs);
        }

        private void Worker_DoWork(List<SignalEntity> entities)
        {
            if (entities == null)
                return;
            entities.Sort();
            foreach (var entity in entities)
            {
                var lineSer2 = plotView1.Model.Series.Where(x => x.Title == entity.SignalName).ToArray()[0] as LineSeries;

                if (XAxes.Contains(Value))
                {
                    DateTime date = DateTime.ParseExact(entity.DataTime, Global.DATETIMEFORMAT, System.Globalization.CultureInfo.InvariantCulture);
                    lineSer2.Points.Add(new DataPoint(DateTimeAxis.ToDouble(date), Convert.ToDouble(entity.SignalValue)));
                }
                else
                {
                    string val = entity.GetValue(XAxes).ToString();
                    if (uint.TryParse(val, out uint valint))
                    {
                        lineSer2.Points.Add(new DataPoint(valint, Convert.ToDouble(entity.SignalValue)));
                    }
                    else
                    {
                        lineSer2.Points.Add(new DataPoint(0, Convert.ToDouble(entity.SignalValue)));
                    }
                }
               
            }
            plotModel.InvalidatePlot(true);
        }

        private void AddSeriesByEntity(List<SignalEntity> entities)
        {
            plotModel.Series.Clear();
            this.panelLegend.Controls.Clear();

            var signals = entities.Where((x, i) => entities.FindIndex(z => z.SignalName == x.SignalName) == i).ToList();
            for (int i = 0; i < signals.Count(); i++)
            {
                var signal = signals[i];
                var series = new LineSeries()
                {
                    Color = OxyColor.FromRgb(255, (byte)i, 0),
                    StrokeThickness = 1,
                    MarkerSize = 1,
                    MarkerStroke = OxyColors.DarkGreen,
                    MarkerType = MarkerType.Circle,
                    Title = signal.SignalName,
                    //InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline
                };

                plotModel.Series.Add(series);
                AddcheckBox(signal, Color.FromArgb(255, (byte)i, 0), i);
            }
        }

        private void AddcheckBox(SignalEntity signal, Color color, int index)
        {
            CheckBoxColorUC cb = new CheckBoxColorUC(signal.SignalName, color);
            cb.Name = index.ToString();
            cb.Text = signal.SignalName;
            cb.Tag = signal;
            cb.Dock = DockStyle.Top;
            cb.Index = index;
            //cb.Checked = true;
            cb.UserControlBtnClicked += Cb_CheckedChanged;
            cb.ColorChanged += Cb_ColorChanged;
            this.panelLegend.Controls.Add(cb);
        }

        private void Cb_ColorChanged(object sender, EventArgs e)
        {
            CheckBoxColorUC cb = sender as CheckBoxColorUC;
            LineSeries series = plotModel.Series[cb.Index] as LineSeries;
            series.Color = OxyColor.FromRgb(cb.Color.R, cb.Color.G, cb.Color.B);
            plotModel.InvalidatePlot(true);
        }

        private void Cb_CheckedChanged(object sender, EventArgs e)
        {
            CheckBoxColorUC cb = sender as CheckBoxColorUC;
            plotModel.Series[cb.Index].IsVisible = cb.Checked;
            plotModel.InvalidatePlot(true);
        }

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

        /// <summary>
        /// 曲线/折线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if((sender as CheckBox).Checked)
            {
                foreach (LineSeries item in plotModel.Series)
                {
                    item.InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline;//曲线
                }
            }
            else
            {
                foreach (LineSeries item in plotModel.Series)
                {
                    item.InterpolationAlgorithm = null;
                }
            }
            plotModel.InvalidatePlot(true);

        }

        /// <summary>
        /// 线上是否显示点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
            {
                foreach (LineSeries item in plotModel.Series)
                {
                    item.MarkerType = MarkerType.Circle;//点
                }
            }
            else
            {
                foreach (LineSeries item in plotModel.Series)
                {
                    item.MarkerType = MarkerType.None;
                }
            }
            plotModel.InvalidatePlot(true);
        }

        /// <summary>
        /// 线条粗细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            foreach (LineSeries item in plotModel.Series)
            {
                item.StrokeThickness = (double)numericUpDown1.Value;
                item.MarkerSize = (double)numericUpDown1.Value;
            }
            plotModel.InvalidatePlot(true);
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择导入文件";
            openFileDialog.Filter = "Excel文件|*.xls";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var entities = NPIOHelper.ExcelToList<SignalEntity>(openFileDialog.FileName);
                signalEntities = entities;
                AddSeriesByEntity(entities);
                backgroundWorker1.RunWorkerAsync(signalEntities);
            }
        }

        private void cbXZoom_CheckedChanged(object sender, EventArgs e)
        {
            plotModel.Axes[0].IsZoomEnabled = this.cbXZoom.Checked;
        }

        private void cbYZoom_CheckedChanged(object sender, EventArgs e)
        {
            plotModel.Axes[1].IsZoomEnabled = this.cbYZoom.Checked;
        }

        private void btnAutoSize_Click(object sender, EventArgs e)
        {
            plotModel.ResetAllAxes();
            plotModel.InvalidatePlot(true);
        }
    }
}
