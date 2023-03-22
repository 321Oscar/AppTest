using AppTest.DBCLib;
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

namespace AppTest
{
    public partial class DBCForm : BaseForm
    {
        public DBCForm()
        {
            InitializeComponent();
            LoadView();
            this.MaximizeBox = false;
            this.CenterToScreen();
            //long[] tempArray = new long[3];
            //byte x = 0;
            //try
            //{
            //    //_ = Array.IndexOf(tempArray, 9) == -1;
            //    //x++;
            //    long y = 3;
            //    long y2 = (long)Math.Pow(2, y);
            //    x = 0x0b;
            //    y2 = 17 / 8;
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
            //MessageBox.Show(x.ToString());

            plotModel = new PlotModel()
            {
                Title = "实时数据",
                IsLegendVisible = false
            };

            //x轴
            plotModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
            });

            //y轴
            plotModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Angle = 60
            });

            //增加曲线
            //添加两条曲线
            //for (int i = 0; i < 2; i++)
            //{
            //    //axis1.Palette.Colors.Add(OxyColor.FromArgb((byte)i, 255, 0, 0));
            //    // SignalItem signal = Singals.Signal[i];
            //    var series = new LineSeries()
            //    {
            //        Color = OxyColor.FromArgb((byte)i, 255, (byte)i, 0),
            //        StrokeThickness = 2,
            //        MarkerSize = 3,
            //        MarkerStroke = OxyColors.DarkGreen,
            //        MarkerType = MarkerType.Diamond,
            //        Title = i.ToString()
            //    };

            //    plotModel.Series.Add(series);
            //    //  AddcheckBox(signal, Color.FromArgb((byte)i, 255, (byte)i, 0), i);
            //}

            plotView1.Model = plotModel;
        }

        #region OxyPlot

        private DateTimeAxis dateTimeAxis;//X轴
        private LinearAxis linearAxis;//Y轴

        private PlotModel plotModel;
        private Random randomOxy = new Random();

        CancellationTokenSource cancelTokenSource;

        private void InitPlotModel()
        {
            Task.WaitAll();


            plotModel = new PlotModel()
            {
                Title = "Humi & Temp",
                IsLegendVisible = false,
                //LegendTitle = "Legend",
                //LegendOrientation = LegendOrientation.Horizontal,
                //LegendPlacement = LegendPlacement.Inside,
                //LegendPosition = LegendPosition.TopRight,
                //LegendBackground = OxyColor.FromAColor(200,OxyColors.Beige),
                //LegendBorder = OxyColors.Black
            };
            plotModel.Axes.Clear();
            //x轴
            //plotModel.Axes.Add(new LinearAxis()
            //{
            //    Position = AxisPosition.Bottom,
            //});

            //plotModel.Axes.Add(new LinearAxis()
            //{
            //    Position = AxisPosition.Left,
            //    Angle = 60
            //});
            dateTimeAxis = new DateTimeAxis()
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                //IntervalLength = 10,
                IsZoomEnabled = true,//缩放
                IsPanEnabled = true,//平移
                StringFormat = "HH:mm:ss fff"
            };
            plotModel.Axes.Add(dateTimeAxis);

            linearAxis = new LinearAxis()
            {
                MajorGridlineStyle = LineStyle.Solid,//设置坐标轴大网格线的样式，实线
                MinorGridlineStyle = LineStyle.Dot,//虚线
                                                   //IntervalLength = 10,
                                                   // Angle = 60,
                IsZoomEnabled = true,
                IsPanEnabled = true,
                //Maximum = 100,
                //Minimum = -1,
                //MaximumRange = 20,
                //MinimumRange = 5,
                MaximumPadding = 0.2,
                MinimumPadding = 0.1,
            };
            plotModel.Axes.Add(linearAxis);

            //添加标注线，温度上下限和湿度上下限
            var lineTempMaxAnnotation = new OxyPlot.Annotations.LineAnnotation()
            {
                Type = OxyPlot.Annotations.LineAnnotationType.Horizontal,
                Color = OxyColors.Red,
                LineStyle = LineStyle.Solid,
                Y = 10,
                Text = "Temp Max:10"
            };
            plotModel.Annotations.Add(lineTempMaxAnnotation);

            var lineTempMinAnnotation = new OxyPlot.Annotations.LineAnnotation()
            {
                Type = OxyPlot.Annotations.LineAnnotationType.Horizontal,
                Color = OxyColors.Red,
                LineStyle = LineStyle.Solid,
                Y = 30,
                Text = "Temp Min:10"
            };
            plotModel.Annotations.Add(lineTempMinAnnotation);

            var lineHumiMaxAnnotation = new OxyPlot.Annotations.LineAnnotation()
            {
                Type = OxyPlot.Annotations.LineAnnotationType.Horizontal,
                Color = OxyColors.Red,
                LineStyle = LineStyle.Solid,
                Y = 75,
                Text = "Humi Max:75"
            };
            plotModel.Annotations.Add(lineHumiMaxAnnotation);

            var lineHumiMinAnnotation = new OxyPlot.Annotations.LineAnnotation()
            {
                Type = OxyPlot.Annotations.LineAnnotationType.Horizontal,
                Color = OxyColors.Red,
                LineStyle = LineStyle.Solid,
                Y = 35,
                Text = "Humi Min:35"
            };
            plotModel.Annotations.Add(lineHumiMinAnnotation);

            //添加两条曲线
            //for (int i = 0; i < 2; i++)
            //{
            //    var series = new LineSeries()
            //    {
            //        Color = OxyColors.Green,
            //        StrokeThickness = 2,
            //        MarkerSize = 3,
            //        MarkerStroke = OxyColors.DarkGreen,
            //        MarkerType = MarkerType.Diamond,
            //        Title = "Temp" + i.ToString()
            //    };
            //    plotModel.Series.Add(series);
            //}
            //for (int i = 0; i < 2; i++)
            //{
            //    //axis1.Palette.Colors.Add(OxyColor.FromArgb((byte)i, 255, 0, 0));
            //    // SignalItem signal = Singals.Signal[i];
            //    var series = new LineSeries()
            //    {
            //        Color = OxyColor.FromArgb(100, 255, (byte)i, 0),
            //        StrokeThickness = 2,
            //        MarkerSize = 3,
            //        MarkerStroke = OxyColors.DarkGreen,
            //        MarkerType = MarkerType.Diamond,
            //        Title = i.ToString(),

            //    };

            //    plotModel.Series.Add(series);
            //    //  AddcheckBox(signal, Color.FromArgb((byte)i, 255, (byte)i, 0), i);
            //}
            var series = new LineSeries()
            {
                Color = OxyColor.FromArgb(100, 255, 222, 0),
                StrokeThickness = 2,
                MarkerSize = 3,
                MarkerStroke = OxyColors.DarkGreen,
                MarkerType = MarkerType.None,
                Title = "1"
            };

            plotModel.Series.Add(series);

            series = new LineSeries()
            {
                Color = OxyColors.Blue,
                StrokeThickness = 2,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.Star,
                Title = "2",
                InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline,
            };
            plotModel.Series.Add(series);

            plotView1.Model = plotModel;

            var controller = new PlotController();
            controller.UnbindMouseDown(OxyMouseButton.Right);
            controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);

            plotView1.Controller = controller;

            btnOxyPlot.Enabled = false;

            //添加数据
            cancelTokenSource = new CancellationTokenSource();
            var token = cancelTokenSource.Token;
            Task.Factory.StartNew(() =>
                {
                    while (!token.IsCancellationRequested)
                    {
                        var date = DateTime.Now;
                        plotModel.Axes[0].Maximum = DateTimeAxis.ToDouble(date.AddSeconds(1));

                        var data = randomOxy.Next(100, 300) / 10.0;

                        var lineSer = plotView1.Model.Series[0] as LineSeries;
                        lineSer.Points.Add(new DataPoint(DateTimeAxis.ToDouble(date), data));
                        //if (lineSer.Points.Count > 200)
                        //{
                        //    lineSer.Points.RemoveAt(0);  
                        //}

                        lineSer = plotView1.Model.Series[1] as LineSeries;
                        lineSer.Points.Add(new DataPoint(DateTimeAxis.ToDouble(date), randomOxy.Next(350, 750) / 10.0));//randomOxy.Next(350, 750) / 10.0
                        //if (lineSer.Points.Count > 200)
                        //{
                        //    lineSer.Points.RemoveAt(0);
                        //}

                        plotModel.InvalidatePlot(true);

                        Thread.Sleep(10);

                    }
                }, token
            );

            token.Register(()=>
            {
                MessageBox.Show("取消生成数据","提示");
            });
        }

        private void btnOxyPlot_Click(object sender, EventArgs e)
        {
            InitPlotModel();
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            if (cancelTokenSource != null)
            {
                cancelTokenSource.Cancel();
                btnOxyPlot.Enabled = true;
            }
        }

        private void ZoomChange_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.Name == checkBox1.Name)
            {
                plotModel.Axes[0].IsZoomEnabled = cb.Checked;
            }
            else if (cb.Name == checkBox2.Name)
            {
                plotModel.Axes[1].IsZoomEnabled = cb.Checked;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            plotModel.ResetAllAxes();
        }

        int positionTier = 0;
        private void btnAddLinear_Click(object sender, EventArgs e)
        {
            positionTier++;
            var linear = new LinearAxis() { 
                Key = "2",
                PositionTier = positionTier
            };

            plotModel.Axes.Add(linear);
            var ser = plotView1.Model.Series.Where(x => x.Title == "2").ToArray()[0] as LineSeries;
            ser.YAxisKey = "2";

        }
        #endregion


        #region DBCFile

        private void LoadView()
        {
            treeView1.Nodes.Add("Nodes");
            treeView1.Nodes.Add("Message");

            listView1.View = System.Windows.Forms.View.Details;
            listView1.GridLines = true;

            listView1.Columns.Add("Name");
            listView1.Columns.Add("StartBit");
            listView1.Columns.Add("Length");
            listView1.Columns.Add("Byte Order");
            listView1.Columns.Add("Value Type");
            listView1.Columns.Add("Factor");
            listView1.Columns.Add("Offset");
            listView1.Columns.Add("Minimum");
            listView1.Columns.Add("Maximum");
            listView1.Columns.Add("unit");
            listView1.Columns.Add("cycleTime");
            listView1.Columns.Add("comment");
        }

        DBCReadHelper dbcHelper = new DBCReadHelper();

        private void button_load_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "请选择DBC文件";
            openFileDialog.Filter = "DBC文件|*.dbc";
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox_path.Text = openFileDialog.FileName;

                try
                {
                    dbcHelper.Parse(textBox_path.Text);
                    UpdateDbcTreeView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void UpdateDbcTreeView()
        {
            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                treeView1.Nodes[i].Nodes.Clear();
            }

            for (int i = 0; i < dbcHelper.dbcFile.nodes.Count; i++)
            {
                treeView1.Nodes[0].Nodes.Add(dbcHelper.dbcFile.nodes[i]);
            }
            for (int i = 0; i < dbcHelper.dbcFile.messages.Count; i++)
            {
                treeView1.Nodes[1].Nodes.Add(dbcHelper.dbcFile.messages[i].messageName);
            }

            treeView1.ExpandAll();
        }

        private void UpdateDbcListView(int index)
        {
            ListViewItem item;
            listView1.BeginUpdate();
            listView1.Items.Clear();

            for (int i = 0; i < dbcHelper.dbcFile.messages[index].signals.Count; i++)
            {
                item = new ListViewItem();
                item.Text = dbcHelper.dbcFile.messages[index].signals[i].signalName;
                item.SubItems.Add(dbcHelper.dbcFile.messages[index].signals[i].startBit.ToString());
                item.SubItems.Add(dbcHelper.dbcFile.messages[index].signals[i].signalSize.ToString());
                item.SubItems.Add(ByteOrderStr(dbcHelper.dbcFile.messages[index].signals[i].byteOrder));
                item.SubItems.Add(VauleTypeStr(dbcHelper.dbcFile.messages[index].signals[i].valueType));
                item.SubItems.Add(dbcHelper.dbcFile.messages[index].signals[i].factor.ToString());
                item.SubItems.Add(dbcHelper.dbcFile.messages[index].signals[i].offset.ToString());
                item.SubItems.Add(dbcHelper.dbcFile.messages[index].signals[i].minimum.ToString());
                item.SubItems.Add(dbcHelper.dbcFile.messages[index].signals[i].maximum.ToString());
                item.SubItems.Add(dbcHelper.dbcFile.messages[index].signals[i].uintStr.ToString());
                item.SubItems.Add(dbcHelper.dbcFile.messages[index].cycleTime.ToString());

                Comment comment = dbcHelper.dbcFile.comments.Find(x => x.signalName == dbcHelper.dbcFile.messages[index].signals[i].signalName 
                    && x.messageID == dbcHelper.dbcFile.messages[index].messageID.ToString());
                if(comment != null)
                {
                    item.SubItems.Add(comment.comment);
                }
                listView1.Items.Add(item);
            }
            listView1.EndUpdate();
        }

        private string ByteOrderStr(uint byteOrder)
        {
            if (byteOrder == 1)
            {
                return "Intel";
            }
            else if (byteOrder == 0)
            {
                return "Motorola";
            }
            else
            {
                return "--";
            }
        }

        private string VauleTypeStr(uint valueType)
        {
            if (valueType == 1)
            {
                return "signed";
            }
            else if (valueType == 0)
            {
                return "unsigned";
            }
            else
            {
                return "--";
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if ((e.Node.Parent != null) && (e.Node.Parent.Text == "Message"))
            {
                try
                {
                    UpdateDbcListView(e.Node.Index);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        private void BtnParse_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] dataByte = new byte[8];

                //填充数据组

                dataByte[0] = (byte)(int.Parse(this.tbDataByte0.Text, System.Globalization.NumberStyles.HexNumber));
                dataByte[1] = (byte)(int.Parse(this.tbDataByte1.Text,System.Globalization.NumberStyles.HexNumber));
                dataByte[2] = (byte)(int.Parse(this.tbDataByte2.Text, System.Globalization.NumberStyles.HexNumber));
                dataByte[3] = (byte)(int.Parse(this.tbDataByte3.Text, System.Globalization.NumberStyles.HexNumber));
                dataByte[4] = (byte)(int.Parse(this.tbDataByte4.Text, System.Globalization.NumberStyles.HexNumber));
                dataByte[5] = (byte)(int.Parse(this.tbDataByte5.Text, System.Globalization.NumberStyles.HexNumber));
                dataByte[6] = (byte)(int.Parse(this.tbDataByte6.Text, System.Globalization.NumberStyles.HexNumber));
                dataByte[7] = (byte)(int.Parse(this.tbDataByte7.Text, System.Globalization.NumberStyles.HexNumber));

                //获取起始位，信号长度
                int startBit = int.Parse(tbStartBit.Text);
                int length = int.Parse(tbLength.Text);
                double offset = double.Parse(tbOffset.Text);
                double factor = double.Parse(tbFactor.Text);

                int len_rem1 = (int)(8 - (startBit % 8));
                int byte_start = (int)(startBit / 8);
                int len_rem2 = (int)((startBit + length) % 8);
                int byte_end = (int)((startBit + length) / 8);
                long tmp = 0;
                if ((byte_start + 1) <= byte_end)
                {
                    //for (int k = byte_start + 1; k < byte_end; k++)
                    //{
                    //    tmp = tmp * 256;
                    //    tmp += dataByte[k];
                    //}

                    for (int k = byte_end - 1; k > byte_start; k--)
                    {
                        tmp = tmp * 256;
                        tmp += dataByte[k];
                    }

                    tmp = tmp * (long)Math.Pow(2, len_rem1) + (long)(dataByte[byte_start] >> (8 - len_rem1));

                    long tmp2 = 0;
                    if (byte_end >= 8)
                        tmp2 = 0;
                    else
                        tmp2 = dataByte[byte_end] % (long)Math.Pow(2, len_rem2);
                    tmp2 = tmp2 * (long)Math.Pow(2, (length - len_rem2));

                    tmp = tmp + tmp2;
                }
                else
                {
                    tmp = (dataByte[byte_start] % (int)Math.Pow(2, len_rem2)) >> (8 - len_rem1);
                }

                double tmp_value = (double)((double)tmp * (double)factor + offset);

                tbValue.Text = tmp_value.ToString();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        #endregion

        
    }
}
