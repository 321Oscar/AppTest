using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTest.Model
{
    public class LogModel
    {
        public LogModel(string log, LPLogLevel LogLevel)
        {
            LogStr = log;
            this.LogLevel = LogLevel;
            LogTime = DateTime.Now;
        }

        public string LogStr { get; set; }
        public LPLogLevel LogLevel { get; set; }

        public Color ShowColor { get=> GetColor(LogLevel); }

        public DateTime LogTime { get; set; } = DateTime.MinValue;
        public string LogTimeStr { get => LogTime.ToString("yyyy/MM/dd HH:mm:ss fff"); } 

        public override string ToString()
        {
            if (LogTime == DateTime.MinValue)
                return LogStr;
            else
            {
                return $"{LogTimeStr}:{LogStr}";
            }
        }

        public Color GetColor(LPLogLevel LogLevel)
        {
            switch (LogLevel)
            {
                case LPLogLevel.Debug:
                    return Color.Gray;
                case LPLogLevel.Info:
                    return Color.Black;
                case LPLogLevel.Warn:
                    return Color.FromArgb(255, 193, 37);
                case LPLogLevel.Error:
                    return Color.Red;
                default:
                    return Color.Gray;
            }
        }

    }

    public enum LPLogLevel
    {
        Debug,//gray
        Info,//black
        Warn,//yellow
        Error,
        All,
    }
}
