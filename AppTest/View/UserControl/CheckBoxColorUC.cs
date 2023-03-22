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
    public partial class CheckBoxColorUC : UserControl
    {
        /// <summary>
        /// checkbox 索引，用来对应曲线图中的曲线
        /// </summary>
        public int Index;
        /// <summary>
        /// 对应曲线图的线条颜色
        /// </summary>
        private Color color;
        private BaseSignal dBCSignal;
        private string signalName;
        public BaseSignal DBCSignal { get => dBCSignal;private set => dBCSignal = value; }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Checked { get { return this.metroCheckBox1.Checked; } set { this.metroCheckBox1.Checked = value; } }

        public string SignalName
        {
            get
            {
                if (DBCSignal != null)
                    return DBCSignal.SignalName;
                else { return signalName; }
            }
            set => signalName = value;
        }

        private delegate void SignalValueChange(string signalValue);

        private event SignalValueChange OnSignalValueChange;

        private string signalvalue = "0";

        //[Obsolete("弃用")]
        public string SignalValue
        {
            get { return signalvalue; }
            set
            {
                if (!signalvalue.Equals(value) && !string.IsNullOrEmpty(value))
                {
                    if (OnSignalValueChange != null)
                    {
                        OnSignalValueChange(value);
                    }
                }
                signalvalue = value;
            }
        }

        /// <summary>
        /// 对应曲线图的线条颜色
        /// </summary>
        public Color Color { get => color; set => color = value; }

        public CheckBoxColorUC(BaseSignal dBCSignal, Color color)
        {
            InitializeComponent();
            this.Name = dBCSignal.ToString();
            DBCSignal = dBCSignal;
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.metroCheckBox1.Text = dBCSignal.ToString();
            this.metroCheckBox1.Checked = true;
            //this.cbSignalName.Text = DBCSignal.SignalName;
            //this.cbSignalName.Checked = true;
            this.panelColor.BackColor = color;
            ToolTip tTip = new ToolTip();
            tTip.SetToolTip(metroCheckBox1, DBCSignal.SignalName);
            tTip.SetToolTip(panelColor, DBCSignal.SignalName);

            this.OnSignalValueChange += CheckBoxColorUC_OnSignalValueChange;

            var threadSafeModel = new SynchronizedNotifyPropertyChanged<BaseSignal>(dBCSignal, this);
            tbSignalData.DataBindings.Clear();
            tbSignalData.DataBindings.Add("Text", threadSafeModel, nameof(DBCSignal.StrValue), false, DataSourceUpdateMode.OnPropertyChanged, "0");
        }

        public CheckBoxColorUC(string name, Color color)
        {
            InitializeComponent();
            this.Name = name;
            //= dBCSignal;
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.metroCheckBox1.Text = name;
            this.metroCheckBox1.Checked = true;
            //this.cbSignalName.Text = name;
            //this.cbSignalName.Checked = true;
            this.panelColor.BackColor = color;
            ToolTip tTip = new ToolTip();
            tTip.SetToolTip(metroCheckBox1, name);
            tTip.SetToolTip(panelColor, name);

            this.OnSignalValueChange += CheckBoxColorUC_OnSignalValueChange;
        }

        private void CheckBoxColorUC_OnSignalValueChange(string signalValue)
        {
            if (tbSignalData.InvokeRequired)
            {
                tbSignalData.Invoke(new Action(() => {
                    tbSignalData.Text = signalValue;
                }));
            }
            else
            {
                tbSignalData.Text = signalValue;
            }
        }

        public delegate void BtnClickHandle(object sender, EventArgs e);
        /// <summary>
        /// checkbox 选中修改事件
        /// </summary>
        public event BtnClickHandle UserControlBtnClicked;
        private void btn_Click(object sender, EventArgs e)
        {
            if (UserControlBtnClicked != null)
                UserControlBtnClicked(this, new EventArgs());//把按钮自身作为参数传递
        }

        public delegate void PanelColorChangeHandle(object sender, EventArgs e);

        /// <summary>
        /// 颜色改变事件
        /// </summary>
        public event PanelColorChangeHandle ColorChanged;

        private void panel1_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                this.Color = colorDialog.Color;
                panelColor.BackColor = Color;
                if (ColorChanged != null)
                {
                    ColorChanged(this, new EventArgs());//把按钮自身作为参数传递
                }
            }
        }
    }
}
