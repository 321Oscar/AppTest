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
    /// <summary>
    /// 提示消息显示窗口
    /// </summary>
    public partial class MessageBoxUC : BaseForm
    {
        public string Title { get { return this.title; } }
        private readonly string title;
        private bool isShowing = true;
        public MessageBoxUC(string title,string content,int showtime = 5)
        {
            InitializeComponent();
            lblTitle.Text = this.title = title;
            tbContent.Text = content;
            this.timer1.Interval = showtime * 1000;
            this.timer1.Enabled = true;
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //透明度为0后关闭该窗口
            if(this.Opacity == 0.0)
            {
                this.timer1.Enabled = false;
                lblClose_Click(null, null);
            }
            else
            {
                if (isShowing)
                {
                    this.timer1.Interval = 200;
                }
                isShowing = false;
                this.Opacity = Math.Max(0.0d, this.Opacity - 0.1d);
            }
        }

        private void MessageBoxUC_Click(object sender, EventArgs e)
        {
            this.Opacity = 0.9d;
            timer1.Enabled = false;
        }

        public void Append(string context)
        {
            this.BeginInvoke(new Action<string>((x) => { 
                this.Opacity = 0.9d; 
                tbContent.Text += "\n\r" + context;
                timer1.Enabled = false;
            }), context);// this.Opacity = 0.9d;
            
        }
    }
}
