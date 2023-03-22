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
using ZedGraph;

namespace AppTest.FormType
{
    public partial class ZedGraphForm : BaseDataForm
    {
        private CancellationTokenSource tokenSource;
        readonly Random r = new Random();
        ToolStripLabel toolLog;
        Thread GetThread;
        private bool getOrStop = false;
        /// <summary>
        /// 曲线，主要用来修改颜色和显示
        /// </summary>
        List<LineItem> lineItems;
        /// <summary>
        /// 曲线中的点数据
        /// </summary>
        Dictionary<BaseSignal, PointPairList> pointLists;

        long xMax = 500;

        public bool GetOrStop
        {
            get => getOrStop;
            set
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        btnGetData.Text = !value ? "Start" : "Stop";
                        //lbLog.Visible = !value;
                        nudTime.Enabled = !value;
                        timer_AddPoint.Enabled = value;
                        nudPointCount.Enabled = !value;
                    }));
                }
                else
                {
                    btnGetData.Text = !value ? "Start" : "Stop";
                    //lbLog.Visible = !value;
                    nudTime.Enabled = !value;
                    timer_AddPoint.Enabled = value;
                    nudPointCount.Enabled = !value;
                }
                xMax = (long)nudPointCount.Value;
                getOrStop = value;
            }
        }

       
        public ZedGraphForm()
        {
            InitializeComponent();
        }

        protected override void InitSignalUC()
        {
            base.InitSignalUC();
        }

        protected override void ReLoadSignal()
        {
            loadPanel(this.zedGraphControl1);
        }

        private void ZedGraphForm_Load(object sender, EventArgs e)
        {
            lineItems = new List<LineItem>();
            pointLists = new Dictionary<BaseSignal, PointPairList>();
            CurrentIDs = new List<int>();

            zedGraphControl1.MouseDownEvent -= ZedGraphControl1_MouseDownEvent;
            zedGraphControl1.MouseDownEvent += ZedGraphControl1_MouseDownEvent;
            zedGraphControl1.MouseUpEvent += ZedGraphControl1_MouseUpEvent;
            zedGraphControl1.MouseMoveEvent += ZedGraphControl1_MouseMoveEvent;

            loadPanel(this.zedGraphControl1);

            ToolStripLabel tslbCanIndex = new ToolStripLabel();
            tslbCanIndex.Text = $"CanIndex: {CanChannel}";
            //分隔符
            ToolStripSeparator tss1 = new ToolStripSeparator();
            toolLog = new ToolStripLabel();
            this.statusStrip_LogInfo.Items.AddRange(new ToolStripItem[]
            {
                    tslbCanIndex,
                    tss1,
                    toolLog
            });
        }

        #region 坐标轴鼠标点击移动
        int startYPosition;
        bool isDragYAxis = false;
        private YAxis dragYAxis;
        private bool isDragXAxis;
        private int startXPosition;
        private XAxis dragXAxis;
        public int DxValue { get; private set; }
        public int DyValue { get; private set; }
        public int UxValue { get; private set; }
        public int UyValue { get; private set; }

        private bool ZedGraphControl1_MouseMoveEvent(ZedGraphControl sender, MouseEventArgs e)
        {
            if (isDragYAxis)
            {
                //速度
                float rate = (e.Y - startYPosition) / sender.GraphPane.Chart.Rect.Height;
                //
                float diff = rate * (float)(dragYAxis.Scale.Max - dragYAxis.Scale.Min);
                if(rate > 0.01)
                {
                    //
                    dragYAxis.Scale.Max += diff;
                    //
                    dragYAxis.Scale.Min += diff;
                    startYPosition = e.Y;
                    //
                    PointPair pp = new PointPair(dragYAxis.Scale.Min, dragYAxis.Scale.Max);
                    //
                    sender.AxisChange();
                    sender.Invalidate();
                }
                else if(rate <= -0.01)
                {
                    //
                    dragYAxis.Scale.Max += diff;
                    //
                    dragYAxis.Scale.Min += diff;
                    startYPosition = e.Y;
                    PointPair pp = new PointPair(dragYAxis.Scale.Min, dragYAxis.Scale.Max);
                    sender.AxisChange();
                    sender.Invalidate();
                }
            }

            if (isDragXAxis)
            {
                float rate = (e.X - startXPosition) / sender.GraphPane.Chart.Rect.Width;
                //
                float diff = rate * (float)(dragXAxis.Scale.Max - dragXAxis.Scale.Min);
                if (rate > 0.01)
                {
                    //
                    dragXAxis.Scale.Max -= diff;
                    //
                    dragXAxis.Scale.Min -= diff;
                    startXPosition = e.X;
                    //
                    PointPair pp = new PointPair(dragXAxis.Scale.Min, dragXAxis.Scale.Max);
                    //
                    sender.AxisChange();
                    sender.Invalidate();
                }
                else if (rate <= -0.01)
                {
                    //
                    dragXAxis.Scale.Max -= diff;
                    //
                    dragXAxis.Scale.Min -= diff;
                    startXPosition = e.X;
                    PointPair pp = new PointPair(dragXAxis.Scale.Min, dragXAxis.Scale.Max);
                    sender.AxisChange();
                    sender.Invalidate();
                }
            }

            if(e.Button == MouseButtons.Left)
            {
                Point mouseDownLocation = new Point(e.X, e.Y);
                GraphPane myPane = sender.GraphPane;
                UxValue = e.X;
                UyValue = e.Y;
            }
            else
            {
                UxValue = 0;
                UyValue = 0;
            }

            return default;
        }

        private bool ZedGraphControl1_MouseUpEvent(ZedGraphControl sender, MouseEventArgs e)
        {
            isDragXAxis = false;
            isDragYAxis = false;
            //
            if(e.Button == MouseButtons.Left)
            {
                Point mouseDownLocation = new Point(e.X, e.Y);
                GraphPane myPane = sender.GraphPane;
                UxValue = e.X;
                UyValue = e.Y;
            }
            else
            {
                UxValue = 0;
                UyValue = 0;
            }

            return default;
        }

        private bool ZedGraphControl1_MouseDownEvent(ZedGraphControl sender, MouseEventArgs e)
        {
            //初始化其实坐标轴
            float startX = 0;
            ///其中Rect;
            ///获取或设置包含由轴限定的区域的矩形
            ///如果手动设置此值，则IsRectAuto值将自动设置为false
            ///X是获取矩形区域左上角的坐标
            float endX = sender.GraphPane.Chart.Rect.X;
            //如果鼠标落下的位置在此范围内0到矩形区域左上角的图标 即鼠标落点在Y轴的区域
            if(e.X > startX && e.X < endX)
            {
                //拖动Y轴表示改为true
                isDragYAxis = true;

                startYPosition = e.Y;
                //获取Y轴数量
                int yAxisCount = sender.GraphPane.YAxisList.FindAll(x => x.IsVisible == true).Count;
                //就是获取Y左边Y轴的区域除以Y轴的数量 就是每个Y轴的单位宽度
                float unit = (endX - startX) / yAxisCount;
                //
                for (int i = 0; i < yAxisCount; i++)
                {
                    //
                    if(e.X < unit * (i+1) && e.X > unit * i)
                    {
                        //拖拽的Y轴索引就是反顺序的索引
                        //由于visible的设置，这里索引错了
                        dragYAxis = sender.GraphPane.YAxisList[yAxisCount - (i + 1)];
                        //if(!dragYAxis.IsVisible)
                        //    dragYAxis = sender.GraphPane.YAxisList[yAxisCount - (i + 1) + 1];
                        break;
                    }
                }
            }

            //否则就是拖拽X轴
            float startY = sender.GraphPane.Chart.Rect.Y + sender.GraphPane.Chart.Rect.Height;
            float endY = sender.GraphPane.Rect.Height;
            if(e.Y > startY && e.Y < endY)
            {
                //
                isDragXAxis = true;
                startXPosition = e.X;
                float unit = (endY - startY);
                dragXAxis = sender.GraphPane.XAxis;
            }

            if(e.Button == MouseButtons.Left)
            {
                //
                Point mouseDownLocation = new Point(e.X, e.Y);
                GraphPane myPane = sender.GraphPane;
                //
                DxValue = e.X;
                DyValue = e.Y;
            }
            else
            {
                DxValue = 0;
                DyValue = 0;
            }

            return default;
        }
        #endregion

        private void loadPanel(ZedGraphControl zgc)
        {
            GraphPane myPane = zgc.GraphPane;
            myPane.IsFontsScaled = false;
            myPane.Title.Text = "Real-Time Data";
            myPane.Title.FontSpec.FontColor = Color.White;
            myPane.XAxis.Color = Color.White;
            myPane.XAxis.Title.Text = "Point Count";
            myPane.XAxis.Title.FontSpec.FontColor = Color.White;
            myPane.XAxis.Scale.FontSpec.FontColor = Color.White;
            myPane.YAxis.Color = Color.White;
            myPane.YAxis.Scale.FontSpec.FontColor = Color.White;

            myPane.Fill.Color = Color.Black;// new Fill(Color.White, Color.FromArgb(200, 200, 255), 45.0f);
            zgc.MasterPane.Fill = new Fill(Color.White, Color.FromArgb(200, 200, 255), 45.0f);
            myPane.Legend.IsVisible = false;
            myPane.Chart.Fill = new Fill(Color.Black, Color.Black, 45.0f);
            myPane.Chart.Border.IsVisible = false;
            myPane.Chart.Border.Color = Color.White;
            myPane.CurveList.Clear();
            myPane.YAxisList.Clear();

            lineItems.Clear();
            pointLists.Clear();
            this.panelLegend.Controls.Clear();

            if (Signals.SignalList.Count > 0)
            {
                for (int i = 0; i < Signals.SignalList.Count; i++)
                {
                    DBCSignal item = Signals.SignalList[i];
                    /// 添加Y轴
                    YAxis yAxis = CreateYAxis(item.SignalName);
                    myPane.YAxisList.Add(yAxis);
                    /// 添加曲线
                    PointPairList list = new PointPairList();
                    //lineItem.Tag = "Test";
                    var lineItem = myPane.AddCurve(item.SignalName, list, Color.Red, SymbolType.None);
                    lineItem.Tag = item.SignalName;
                    lineItem.YAxisIndex = myPane.YAxisList.Count - 1;
                    lineItems.Add(lineItem);
                    pointLists.Add(item, list);

                    if (!CurrentIDs.Contains(int.Parse(Signals.SignalList[i].MessageID, System.Globalization.NumberStyles.HexNumber)))
                    {
                        CurrentIDs.Add(int.Parse(Signals.SignalList[i].MessageID, System.Globalization.NumberStyles.HexNumber));
                    }

                    AddcheckBox(item, Color.FromArgb(255, (byte)i, 0), i);
                }
                zedGraphControl1.AxisChange();
                zedGraphControl1.Refresh();

            }
        }

        private YAxis CreateYAxis(string title)
        {
            YAxis yAxis = new YAxis();
            yAxis.Title.Text = title;
            yAxis.Scale.FontSpec.FontColor = Color.White;

            #region 刻度设置-小刻度

            yAxis.MinorGrid.IsVisible = false;          //隐藏小刻度网格线
            yAxis.MinorTic.IsOpposite = false;          //隐藏对面的刻度-小刻度
            yAxis.MinorTic.IsInside = false;            //隐藏内测刻度线-小刻度
            yAxis.MinorTic.IsOutside = true;            //显示外侧刻度线-小刻度
            yAxis.MinorTic.IsCrossInside = false;       //隐藏内测交叉刻度线-小刻度
            yAxis.MinorTic.IsCrossOutside = false;      //隐藏外侧交叉刻度线-小刻度

            #endregion
            #region 刻度设置-主刻度

            yAxis.MajorGrid.IsVisible = true;           //显示主刻度网格线
            yAxis.MajorGrid.IsZeroLine = false;         //隐藏主刻度的0刻度线
            yAxis.MajorTic.IsOpposite = false;          //隐藏对面的刻度-主刻度
            yAxis.MajorTic.IsInside = false;            //隐藏内测刻度线-主刻度
            yAxis.MajorTic.IsOutside = true;            //显示外侧刻度线-主刻度
            yAxis.MajorTic.IsCrossInside = false;       //隐藏内测交叉刻度线-主刻度
            yAxis.MajorTic.IsCrossOutside = false;      //隐藏外侧交叉刻度线-主刻度
            yAxis.MajorGrid.Color = Color.Red;
            //yAxis.MajorGrid.Color = Color.White
            yAxis.Color = Color.Red;
            #endregion

            return yAxis;
        }

        protected void ShowLog(string log)
        {
            if (statusStrip_LogInfo.InvokeRequired)
            {
                this.Invoke(new Action(() => {
                    toolLog.Text = log;
                }));
            }
            else
            {
                toolLog.Text = log;
            }
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (GetOrStop)
            {
                GetOrStop = false;
                timer_AddPoint.Enabled = false;
            }
            else
            {
                foreach (var item in pointLists.Values)
                {
                    item.Clear();
                }
                xMax = 500;
                GetOrStop = true;
                timer_AddPoint.Enabled = true;
                GetThread = new Thread(new ThreadStart(() => {
                    Random random = new Random();
                    //LineItem lineItem;
                    //LineItem lineItem2;
                    

                    //PointPairList list = new PointPairList();
                    
                    //PointPairList list2 = new PointPairList();
                    ////lineItem.Tag = "Test";
                    //lineItem = zedGraphControl1.GraphPane.AddCurve("Test", list, Color.Red, SymbolType.None);
                    //lineItem2 = zedGraphControl1.GraphPane.AddCurve("Test2", list2, Color.Green, SymbolType.None);
                    //lineItem.IsVisible = true;
                    //lineItem.YAxisIndex = 1;
                    //for (int i = 0; i < 20; i++)
                    //{
                    //    list.Add(i, random.NextDouble() * 100);
                    //    list2.Add(i, random.NextDouble() * 1000 + 20);
                    //}
                    //zedGraphControl1.AxisChange();
                    var datetimeStart = DateTime.Now;
                    while (GetOrStop)
                    {
                        Debug.Print(DateTime.Now.ToString("ss fff"));
                        GetDataTimer_Elapsed();
                        
                        Thread.Sleep(10);
                    }
                }));
                GetThread.IsBackground = true;
                GetThread.Start();
            }
        }

        /// <summary>
        /// 获取数据具体方法
        /// </summary>
        /// <param name="sender"></param>
        private async void GetDataTimer_Elapsed()
        {
            try
            {
                var date = DateTime.Now;
                List<SignalEntity> signalEntities = new List<SignalEntity>();
                var datatimeStr = DateTime.Now.ToString(Global.DATETIMEFORMAT);
                SignalEntity entity;

                //debug 生成数据
                //foreach (var item in Signals.Signal)
                //{
                //    //从Can口中取数据，并解析
                //    //dt.Columns[item.SignalName] = 
                //    item.StrValue = r.Next(2, 2000).ToString();

                //    //signalUC[item.SignalName].SetData(data);
                //    entity = new SignalEntity();
                //    entity.DataTime = datatimeStr;
                //    entity.ProjectName = OwnerProject.Name;
                //    entity.FormName = this.Name;
                //    entity.SignalName = item.SignalName;
                //    entity.SignalValue = item.StrValue;
                //    entity.CreatedOn = DateTime.Now.ToString(Global.DATETIMEFORMAT);
                //    signalEntities.Add(entity);
                //}
                //return;

               var rx_mails = USBCanManager.Instance.Receive(OwnerProject, CurrentIDs.ToArray(), CanChannel, $"[{this.FormType}] [{this.Name}]");
                if (rx_mails == null || rx_mails.Length == 0)
                {
                    return;
                }
                //var data = protocol.Multip(rx_mails, Signals);
                foreach (var item in Protocol.MultipYeild(rx_mails, Signals.SignalList.Cast<BaseSignal>().ToList()))
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

                if (signalEntities.Count > 0 && base.IsSaveData)
                {
                    var dbAsync = await DBHelper.GetDb();
                    var result = await dbAsync.InsertAllAsync(signalEntities);
                    LogHelper.WriteToOutput(this.Name, $"插入数据成功-{result}");
                }
            }
            catch (USBCANOpenException ex)
            {
                GetOrStop = false;
                ShowLog(ex.Message);
            }
            catch (Exception ex)
            {
                LeapMessageBox.Instance.ShowError(ex);
                GetOrStop = false;
                ShowLog(ex.Message);
            }

        }


        /// <summary>
        /// Load Point to Panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_AddPoint_Tick(object sender, EventArgs e)
        {
            if(pointLists[Signals.SignalList[0]].Count > nudPointCount.Value)
            {
                xMax++;
            }
            foreach (var item in pointLists)
            {
                if (item.Value.Count > nudPointCount.Value)
                {
                    item.Value.RemoveAt(0);
                }
                if (!string.IsNullOrEmpty(item.Key.StrValue) && double.TryParse(item.Key.StrValue, out double dVal))
                {
                    item.Value.Add(item.Value.Count >= nudPointCount.Value ? xMax : item.Value.Count, double.Parse(item.Key.StrValue));
                    CheckBoxColorUC cbc = (CheckBoxColorUC)panelLegend.Controls.Find(item.Key.SignalName, false)[0];
                    cbc.SignalValue = item.Key.StrValue;
                    //item.Add(item.Count, Signals.Signal.Find(x=>x==));
                }
            }
            zedGraphControl1.GraphPane.XAxis.Scale.Max = xMax;
            zedGraphControl1.GraphPane.XAxis.Scale.Min = xMax - (double)(nudPointCount.Value-1);
            this.Invoke(new Action(() => {
                zedGraphControl1.AxisChange();
                zedGraphControl1.Refresh();
            }));
        }

        /// <summary>
        /// 曲线图例增加
        /// </summary>
        /// <param name="signal"></param>
        /// <param name="color"></param>
        /// <param name="index"></param>
        private void AddcheckBox(DBCSignal signal, Color color, int index)
        {
            CheckBoxColorUC cb = new CheckBoxColorUC(signal.SignalName, color);
            //cb.Name = index.ToString();
            cb.Text = signal.SignalName;
            cb.Tag = signal;
            cb.Dock = DockStyle.Top;
            cb.Index = index;
            //cb.Checked = true;
            cb.UserControlBtnClicked += Cb_CheckedChanged;
            cb.ColorChanged += Cb_ColorChanged;
            this.panelLegend.Controls.Add(cb);
        }

        private void Cb_CheckedChanged(object sender, EventArgs e)
        {
            CheckBoxColorUC cb = sender as CheckBoxColorUC;
            var lineSer2 = lineItems.Find(x => x.Tag.ToString() == cb.SignalName);
            lineSer2.IsVisible = cb.Checked;
            ///Y轴颜色
            var lineAxes = zedGraphControl1.GraphPane.YAxisList[lineSer2.YAxisIndex];
            lineAxes.IsVisible = cb.Checked;
            zedGraphControl1.AxisChange();
            zedGraphControl1.Refresh();
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
            var lineSer2 = lineItems.Find(x => x.Tag.ToString() == cb.SignalName);
            lineSer2.Color = Color.FromArgb(cb.Color.R, cb.Color.G, cb.Color.B);
            ///Y轴颜色
            var lineAxes = zedGraphControl1.GraphPane.YAxisList[lineSer2.YAxisIndex];
            lineAxes.MajorGrid.Color = Color.FromArgb(cb.Color.R, cb.Color.G, cb.Color.B);
            lineAxes.Color = Color.FromArgb(cb.Color.R, cb.Color.G, cb.Color.B);
            zedGraphControl1.AxisChange();
            zedGraphControl1.Refresh();
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

        private void ZedGraphForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (GetThread != null)
            {
                GetThread.Abort();
            }
        }
    }
}
