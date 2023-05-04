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

namespace AppTest.View.UC
{
    public partial class LogListview : UserControl
    {
        private List<LogModel> _logs = new List<LogModel>();

        private LPLogLevel _currentLevel = LPLogLevel.Info;

        public LPLogLevel CurrentLevel { get => _currentLevel; private set => _currentLevel = value; }

        public LogListview()
        {
            InitializeComponent();
            cbbLogLevel.DataSource = Enum.GetValues(typeof(LPLogLevel));
            cbbLogLevel.SelectedIndex = (int)LPLogLevel.All;
            //lvLog.Columns[2].Width = lvLog.ClientSize.Width - lvLog.Columns[0].Width - lvLog.Columns[1].Width - 10;
        }

        public void AddLog(string log, LPLogLevel logLevel = LPLogLevel.Info,Exception ex = null)
        {
            var l = new LogModel(log, logLevel);
            Task.Run(new Action(()=>
            {
                switch (logLevel)
                {
                    case LPLogLevel.Debug:
                        LogHelper.Debug(log);
                        break;
                    case LPLogLevel.Info:
                        LogHelper.Info(log);
                        break;
                    case LPLogLevel.Warn:
                        LogHelper.Warn(log);
                        break;
                    case LPLogLevel.Error:
                        LogHelper.Error(log, ex);
                        break;
                    case LPLogLevel.All:
                        break;
                }
            }));
            _logs.Add(l);
            if(_logs.Count > 1000)
            {
                _logs.RemoveAt(0);
            }

            if(_currentLevel == logLevel || CurrentLevel == LPLogLevel.All)
            {
                AddItems(l);
            }
        }

        private void cbbLogLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentLevel = (LPLogLevel)cbbLogLevel.SelectedIndex;

            LoadLog(CurrentLevel);
        }

        private void LoadLog(LPLogLevel level)
        {
            List<LogModel> s;
            switch (level)
            {
                case LPLogLevel.Debug:
                case LPLogLevel.Info:
                case LPLogLevel.Warn:
                case LPLogLevel.Error:
                    s = _logs.Where(x => x.LogLevel == level).ToList();
                    break;
                default:
                    s = _logs;
                    break;
            }
            lvLog.Items.Clear();
            foreach (var l in s)
            {
                AddItems(l);
            }
        }

        private void AddItems(LogModel l)
        {
            ListViewItem item = new ListViewItem(l.LogTimeStr)
            {
                ImageIndex = (int)l.LogLevel,
                ForeColor = l.ShowColor
            };
            item.SubItems.Add(l.LogLevel.ToString());
            item.SubItems.Add(l.LogStr);

            lvLog.Items.Add(item);
            lvLog.Items[lvLog.Items.Count - 1].EnsureVisible();
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            lvLog.Items.Clear();
            _logs = new List<LogModel>();
            //_logModelsInfo = new List<LogModel>();
            //_logModelsWarn = new List<LogModel>();
            //_logModelsError = new List<LogModel>();
        }
       // Timer t = new Timer();
        private void btnAddLog_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            AddLog("xxx", CurrentLevel);
        }

        private void LogListview_SizeChanged(object sender, EventArgs e)
        {
           // lvLog.Columns[2].Width = lvLog.ClientSize.Width - lvLog.Columns[0].Width - lvLog.Columns[1].Width -10;
        }
    }

}
