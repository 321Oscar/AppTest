using AppTest.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppTest
{
    /// <summary>
    /// 设置字体，需要继承的窗口字体为默认才能生效
    /// </summary>
    public partial class BaseForm : Form
    {
        public BaseForm()
        {
            InitializeComponent();
            //this.AutoScaleMode = AutoScaleMode.Dpi;
            this.Font = Global.CurrentFont;
        }
    }
}
