using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppTest.Mainform
{
    public partial class LogForm : Form
    {
        public LogForm()
        {
            InitializeComponent();
            this.ControlBox = false;
            ShowLogUI = true;
        }

        public void ShowLog(string log)
        {
            if (listBox1.InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(addLogToListBox),log);
            }
            else
            {
                addLogToListBox(log);
            }
        }

        private static readonly object locker = new object();

        private void addLogToListBox(string log)
        {
            lock (locker)
            {
                if (!ShowLogUI)
                    return;
                if (listBox1.Items.Count > 1000)
                    listBox1.Items.RemoveAt(0);
                int x = listBox1.Items.Add(log);
                listBox1.SelectedIndex = x;
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private bool showLog;

        /// <summary>
        /// 是否显示最新Log
        /// </summary>
        public bool ShowLogUI { get => showLog; 
            set 
            {
                showLog = value;
                btnStart.Text = showLog ? "暂停" : "继续";
            } 
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            ShowLogUI = !ShowLogUI;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }
    }
}
