using AppTest.Helper;
using AppTest.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppTest.FormType
{
    public partial class SignalInfoUC : UserControl
    {
        /// <summary>
        /// 当前信号
        /// </summary>
        public DBCSignal Signal { get => signal; }
        private DBCSignal signal;

        /// <summary>
        /// 数据读写
        /// </summary>
        public bool DataReadOnly;
        /// <summary>
        /// 是否在UI上显示数据变化
        /// </summary>
        [Obsolete("弃用,使用IPropertychanged接口实现UI变更")]
        public bool IsShowDataChangeUI = true;

        private delegate void SignalValueChange(string signalValue);

        private event SignalValueChange OnSignalValueChange;
        [Obsolete("弃用，直接赋值Signal.StrValue")]
        private string signalvalue = "0";

        /// <summary>
        /// 信号的值
        /// </summary>
        public string SignalValue
        {
            get { return Signal.StrValue; }
            set
            {
                bool isDecimal = double.TryParse(value, out _);
                if (isDecimal)
                {
                    Signal.StrValue = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signal">关联的信号</param>
        /// <param name="dataReadOnly">仅读</param>
        public SignalInfoUC(DBCSignal signal, bool dataReadOnly) : base()
        {
            InitializeComponent();

            DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            this.OnSignalValueChange += SignalInfoUS_OnSignalValueChange;

            this.BorderStyle = BorderStyle.FixedSingle;
            this.Font = Global.CurrentFont;
            this.signal = signal;

            #region 数据绑定
            var threadSafeModel = new SynchronizedNotifyPropertyChanged<DBCSignal>(Signal, this);
            tbSignalData.DataBindings.Add("Text", threadSafeModel, "StrValue", false, DataSourceUpdateMode.OnPropertyChanged,"0");
            #endregion

            this.lbSignalName.Text = signal.ToString();

            tbSignalData.ReadOnly = dataReadOnly;
            //set signal's uint if unit exist.
            this.lbSignalUnit.Text = signal.Unit ?? "--";
            //this.panel1.
            toolTip1.SetToolTip(lbSignalName, $"{signal.Comment}:[{signal.Minimum},{signal.Maximum}]");
            toolTip1.SetToolTip(tbSignalData, $"{signal.Comment}:[{signal.Minimum},{signal.Maximum}]");
            toolTip1.SetToolTip(this, $"{signal.Comment}:[{signal.Minimum},{signal.Maximum}]");

            this.splitter1.DoubleClick += Splitter_DoubleClick;
            this.splitter2.DoubleClick += Splitter_DoubleClick;
            this.splitter3.DoubleClick += Splitter_DoubleClick;
            this.panelData.MinimumSize = new Size(20, 20);
            this.Height = lbSignalName.Height;

            AutoWidth();

            lbSignalUnit.AutoEllipsis = true;
            tbSignalData.AutoSize = false;
            tbSignalData.Height = this.Height;
        }

        private void SignalInfoUS_OnSignalValueChange(string signalValue)
        {
            if (IsShowDataChangeUI)
            {
                if (tbSignalData.InvokeRequired)
                {
                    tbSignalData.Invoke(new Action(()=> {
                        tbSignalData.Text = signalValue;
                    }));
                }
                else
                {
                    tbSignalData.Text = signalValue;
                }
            }
        }

        private void Splitter_DoubleClick(object sender, EventArgs e)
        {
            AutoWidth();
        }

        /// <summary>
        /// 自动分配宽度
        /// </summary>
        public void AutoWidth()
        {
            this.panelSignalName.Width = this.lbSignalName.Width;
            this.panelUnit.Width = this.lbSignalUnit.Width;
            this.panelStatus.Width = this.lbStatus.Width;
        }

        //private object data;

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="data"></param>
        [Obsolete("Use SignalInfoUS.SignalValue")]
        public void SetData(string data)
        {
            if (!tbSignalData.Text.Equals(data))
            {
                if (tbSignalData.InvokeRequired)
                {
                    this.Invoke(new Action<string>(x => { tbSignalData.Text = x.ToString(); }), data);
                }
                else
                {
                    tbSignalData.Text = data;
                }
            }
        }

        /// <summary>
        /// 获取界面上的数值
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use SignalInfoUS.SignalValue")]
        public string GetData()
        {
            if (string.IsNullOrEmpty(this.tbSignalData.Text))
                throw new Exception($"{this.lbSignalName.Text}设定值为空");
            return this.tbSignalData.Text;
        }

        /// <summary>
        /// 设置当前信号的状态（发送成功、失败等）
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [Obsolete("状态标识已删除")]
        public void SetStatus(string status, Color? color)
        {
            if (tbSignalData.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    if (color != null)
                        lbStatus.ForeColor = (Color)color;
                    this.lbStatus.Text = status;
                    //AutoWidth();
                }));
                //BeginInvoke();
            }
            else
            {
                if (color != null)
                    lbStatus.ForeColor = (Color)color;
                this.lbStatus.Text = status;

                //AutoWidth();
            }
        }

        private void tbSignalData_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = NumberDotTextBox_KeyPress(sender,e);
            if (e.KeyChar == '\x1')
            {
                ((TextBox)sender).SelectAll();
                e.Handled = true;
            }
        }

        public bool NumberDotTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '。')
                e.KeyChar = '.';
            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != (char)('.') && e.KeyChar != (char)('-'))
            {
                return true;
            }
            if (e.KeyChar == (char)('-'))
            {
                //输入负号时
                if ((sender as TextBox).Text != "" && ((TextBox)sender).SelectionStart != 0)
                {
                    return true;
                }
            }
            //小数点只能输入一次
            if (e.KeyChar == (char)('.') && ((TextBox)sender).Text.IndexOf('.') != -1)
            {
                return true;
            }
            //第一位不能为小数点
            if (e.KeyChar == (char)('.') && ((TextBox)sender).Text == "")
            {
                return true;
            }
            //第一位是零，第二位必须是小数点
            //if (e.KeyChar != (char)('.') && e.KeyChar != 8 && ((TextBox)sender).Text == "0")
            //{
            //    return true;
            //}
            //第一位是负号，第二位不能为小数点
            if (e.KeyChar == '.' && ((TextBox)sender).Text == "-")
            {
                return true;
            }


            return false;
        }

        private void tbSignalData_TextChanged(object sender, EventArgs e)
        {
            this.Signal.StrValue = tbSignalData.Text;
            if (double.TryParse(tbSignalData.Text, out double x))
            {
                if (x > Signal.Maximum || x < Signal.Minimum)
                {
                    tbSignalData.BackColor = Color.Red;
                    tbSignalData.ForeColor = Color.White;
                    //SetStatus("值超限", Color.Red);
                }
                else
                {
                    tbSignalData.BackColor = tbSignalData.ReadOnly ? System.Drawing.SystemColors.Control : Color.White;
                    tbSignalData.ForeColor = Color.Black;
                    //SetStatus("", null);
                }
            }
        }

        private void tbSignalData_MouseClick(object sender, MouseEventArgs e)
        {
            if (Global.ShowType == 0)
                tbSignalData.SelectionStart = tbSignalData.Text.Length;
        }

        private void tbSignalData_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (DataSendToCan != null)
                {
                    DataSendToCan(this, null);
                }
                else
                {
                    MessageBox.Show("数据发送事件未绑定，不支持回车发送");
                }
            }
        }

        public delegate void SendHandler(object sender, EventArgs args);

        /// <summary>
        /// 数据发送事件
        /// </summary>
        public event SendHandler DataSendToCan;

        #region 边框


        /*
        private Color _BorderColor = Color.Black;
        [Browsable(true), Description("边框颜色"), Category("自定义分组")]
        public Color BorderColor
        {
            get
            {
                return _BorderColor;
            }
            set
            {
                _BorderColor = value;
                this.Invalidate();
            }
        }

        private int _BorderSize = 0;
        [Browsable(true), Description("边框粗细"), Category("自定义分组")]
        public int BorderSize
        {
            get { return _BorderSize; }
            set { _BorderSize = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics,
                this.ClientRectangle,
                leftColor: this._BorderColor,
                leftWidth: this._BorderSize,
                leftStyle: ButtonBorderStyle.Solid,
                topColor: this._BorderColor,
                topWidth: this._BorderSize,
                topStyle: ButtonBorderStyle.Solid,
                rightColor: _BorderColor,
                rightWidth: _BorderSize,
                rightStyle: ButtonBorderStyle.Solid,
                bottomColor: _BorderColor,
                bottomWidth: _BorderSize,
                bottomStyle: ButtonBorderStyle.Solid);
        }
        */
        private void tbSignalData_MouseHover(object sender, EventArgs e)
        {
            //this.BorderColor = Color.Red;
            //BorderSize = 1;
        }

        private void tbSignalData_MouseLeave(object sender, EventArgs e)
        {
            this._IsMouseOver = false;
            ///如果不启用HotTrack，则开始重绘
            ///如果不加判断，则当不启用HotTrack
            ///鼠标在控件上移动时，控件边框不不断重绘，
            ///导致控件边框闪烁
            if (this._HotTrack)
            {
                this.Invalidate();
            }
            //base.OnMouseLeave(e);
            //this.BorderColor = Color.Black;
            //BorderSize = 0;
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr GetWindowDC(IntPtr hwnd);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hwnd, IntPtr hDC);

        /// <summary>
        /// 是否启用热点效果
        /// </summary>
        private bool _HotTrack = true;

        /// <summary>
        /// 边框颜色
        /// </summary>
        private Color _BorderColor = Color.FromArgb(0xA7, 0xA6, 0xAA);
        /// <summary>
        /// 热点边框颜色
        /// </summary>
        //private Color _HotColor = Color.FromArgb(0x33, 0x5E, 0xA8);
        private Color _HotColor = Color.FromArgb(0, 212, 106);

        private bool _IsMouseOver = false;

        #region 属性

        [Category("行为"),
            Description(""),
            DefaultValue(true)]
        public bool HotTrack
        {
            get => _HotTrack;
            set
            {
                _HotTrack = value;
                //在该值发生变化时重绘控件，下同
                //在设计模式下，更改该属性时，如果不调用该语句，
                //则不能立即看到设计视图中该控件相应的变化
                this.Invalidate();
            }
        }

        [Category("边框颜色"),
           Description(""),
           DefaultValue(typeof(Color), "#A7A6AA")]
        public Color BorderColor
        {
            get => _BorderColor;
            set
            {
                _BorderColor = value;
                //在该值发生变化时重绘控件，下同
                //在设计模式下，更改该属性时，如果不调用该语句，
                //则不能立即看到设计视图中该控件相应的变化
                this.Invalidate();
            }
        }

        [Category("外观"),
           Description(""),
           DefaultValue(typeof(Color), "#335EA8")]
        public Color HotColor
        {
            get => _HotColor;
            set
            {
                _HotColor = value;
                //在该值发生变化时重绘控件，下同
                //在设计模式下，更改该属性时，如果不调用该语句，
                //则不能立即看到设计视图中该控件相应的变化
                this.Invalidate();
            }
        }

        #endregion

        protected override void OnMouseMove(MouseEventArgs e)
        {
            //鼠标状态
            this._IsMouseOver = true;
            ///如果不启用HotTrack，则开始重绘
            ///如果不加判断，则当不启用HotTrack
            ///鼠标在控件上移动时，控件边框不不断重绘，
            ///导致控件边框闪烁
            if (this._HotTrack)
            {
                this.Invalidate();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            //鼠标状态
            this._IsMouseOver = false;
            ///如果不启用HotTrack，则开始重绘
            ///如果不加判断，则当不启用HotTrack
            ///鼠标在控件上移动时，控件边框不不断重绘，
            ///导致控件边框闪烁
            if (this._HotTrack)
            {
                this.Invalidate();
            }
            base.OnMouseLeave(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (this._HotTrack)
            {
                this.Invalidate();
            }
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (this._HotTrack)
            {
                this.Invalidate();
            }
            base.OnLostFocus(e);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0xf || m.Msg == 0x133)
            {
                IntPtr hDC = GetWindowDC(m.HWnd);
                if (hDC.ToInt32() == 0)
                {
                    return;
                }

                if (this.BorderStyle == BorderStyle.FixedSingle)
                {
                    System.Drawing.Pen pen = new Pen(this._BorderColor, 1);
                    if (this._HotTrack)
                    {
                        if (this.Focused)
                        {
                            pen.Color = this._HotColor;
                        }
                        else
                        {
                            if (this._IsMouseOver)
                            {
                                pen.Color = this._HotColor;
                            }
                            else
                            {
                                pen.Color = this._BorderColor;
                            }
                        }
                    }
                    System.Drawing.Graphics g = Graphics.FromHdc(hDC);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
                    pen.Dispose();
                }
                m.Result = IntPtr.Zero;
                ReleaseDC(m.HWnd, hDC);
            }
        }

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000;//用双缓冲绘制窗口的所有子控件
        //        return cp;
        //    }
        //}

        private void tbSignalData_MouseMove(object sender, MouseEventArgs e)
        {
            this._IsMouseOver = true;
            ///如果不启用HotTrack，则开始重绘
            ///如果不加判断，则当不启用HotTrack
            ///鼠标在控件上移动时，控件边框不不断重绘，
            ///导致控件边框闪烁
            if (this._HotTrack)
            {
                this.Invalidate();
            }
            base.OnMouseMove(e);
        }

        private void tbSignalData_Leave(object sender, EventArgs e)
        {
            this._IsMouseOver = false;
            ///如果不启用HotTrack，则开始重绘
            ///如果不加判断，则当不启用HotTrack
            ///鼠标在控件上移动时，控件边框不不断重绘，
            ///导致控件边框闪烁
            if (this._HotTrack)
            {
                this.Invalidate();
            }
            //base.OnMouseMove(e);
        }
        #endregion

        private void lbSignalName_Click(object sender, EventArgs e)
        {
            SignalItemForm<DBCSignal> siF = new SignalItemForm<DBCSignal>(Signal,Signal.SignalName);
            siF.Show();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle &= ~0x02000000;
                return cp;
            }
        }
    }
}
