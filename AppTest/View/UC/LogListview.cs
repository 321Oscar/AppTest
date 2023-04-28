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
        private List<LogModel> _logModelsDebug = new List<LogModel>();
        private List<LogModel> _logModelsInfo = new List<LogModel>();
        private List<LogModel> _logModelsWarn = new List<LogModel>();
        private List<LogModel> _logModelsError = new List<LogModel>();

        private LPLogLevel _currentLevel = LPLogLevel.Info;

        public LPLogLevel CurrentLevel { get => _currentLevel; private set => _currentLevel = value; }

        public LogListview()
        {
            InitializeComponent();
            cbbLogLevel.DataSource = Enum.GetValues(typeof(LPLogLevel));

            //lvLog.Columns[2].Width = lvLog.ClientSize.Width - lvLog.Columns[0].Width - lvLog.Columns[1].Width - 10;
        }

        public void AddLog(string log, LPLogLevel logLevel = LPLogLevel.Info)
        {
            var l = new LogModel(log, logLevel);
            switch (logLevel)
            {
                case LPLogLevel.Debug:
                    _logModelsDebug.Add(l);
                    break;
                case LPLogLevel.Info:
                    _logModelsInfo.Add(l);
                    break;
                case LPLogLevel.Warn:
                    _logModelsWarn.Add(l);
                    break;
                case LPLogLevel.Error:
                    _logModelsError.Add(l);
                    break;
                default:
                    _logModelsInfo.Add(l);
                    break;
            }

            if(_currentLevel == logLevel || CurrentLevel == LPLogLevel.All)
            {
                ListViewItem item = new ListViewItem(l.LogTime.ToString());
                item.SubItems.Add(l.LogLevel.ToString());
                item.SubItems.Add(l.LogStr);
                this.lvLog.Items.Add(item);
                lvLog.Items[lvLog.Items.Count - 1].EnsureVisible();
                //lvLog.sel
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
                    s = _logModelsDebug;
                    break;
                case LPLogLevel.Info:
                    s = _logModelsInfo;
                    break;
                case LPLogLevel.Warn:
                    s = _logModelsWarn;
                    break;
                case LPLogLevel.Error:
                    s = _logModelsError;
                    break;
                default:
                    s = _logModelsInfo;
                    _logModelsInfo.AddRange(_logModelsDebug);
                    _logModelsInfo.AddRange(_logModelsWarn);
                    _logModelsInfo.AddRange(_logModelsError);
                    break;
            }
            lvLog.Items.Clear();
            foreach (var l in s)
            {
                ListViewItem item = new ListViewItem(l.LogTime.ToString());
                item.SubItems.Add(l.LogLevel.ToString());
                item.SubItems.Add(l.LogStr);
                this.lvLog.Items.Add(item);
                lvLog.Items[lvLog.Items.Count - 1].EnsureVisible();
            }
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            lvLog.Items.Clear();
            _logModelsDebug = new List<LogModel>();
            _logModelsInfo = new List<LogModel>();
            _logModelsWarn = new List<LogModel>();
            _logModelsError = new List<LogModel>();
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
