using AppTest.Mainform;
using AppTest.Model;
using log4net.Appender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTest.Helper
{
    /// <summary>
    /// 日志记录类
    /// </summary>
    public class LogHelper
    {
        private static readonly log4net.ILog logWarn = log4net.LogManager.GetLogger("Warn");
        private static readonly log4net.ILog logError = log4net.LogManager.GetLogger("Error");
        private static readonly log4net.ILog logDebug = log4net.LogManager.GetLogger("LogDebug");
        private static readonly log4net.ILog logInfo = log4net.LogManager.GetLogger("LogInfo");
        private static readonly log4net.ILog logSend = log4net.LogManager.GetLogger("LogSend");
        private static readonly log4net.ILog logReceive = log4net.LogManager.GetLogger("LogReceive");
        private static readonly LogForm logForm = new LogForm();

        //private static Queue<LogModel> logs = new Queue<LogModel>();

        public static LogForm LogForm { get => logForm; }

        public static void WriteToOutput(string logFileName,string logInfo)
        {
            try
            {
                if (_ilogDict == null || !_ilogDict.ContainsKey(logFileName))
                {
                    InitOutput(logFileName);
                }

                _ilogDict[logFileName].Info(logInfo);
                logForm.ShowLog(logInfo);
            }
            catch(Exception ex)
            {
                LogHelper.Warn(ex.Message);
            }
            
        }

        static Dictionary<string, log4net.ILog> _ilogDict;

        private static void InitOutput(string logFileName)
        {
            if(_ilogDict == null)
            {
                _ilogDict = new Dictionary<string, log4net.ILog>();
            }

            try
            {
                if (!_ilogDict.ContainsKey(logFileName))
                {
                    //创建日志目录
                    log4net.LogManager.CreateRepository(logFileName);
                    //获取日志对象
                    log4net.ILog logger = log4net.LogManager.GetLogger(logFileName, logFileName);

                    _ilogDict.Add(logFileName,logger);

                    //配置输出日志格式。
                    log4net.Layout.PatternLayout layout = new log4net.Layout.PatternLayout(@"[%date] %m%n");
                    layout.ActivateOptions();

                    log4net.Filter.LevelMatchFilter filter = new log4net.Filter.LevelMatchFilter();

                    filter.LevelToMatch = log4net.Core.Level.All;

                    filter.ActivateOptions();

                    //配置日志【循环附加，累加】
                    log4net.Appender.RollingFileAppender appender = new log4net.Appender.RollingFileAppender();

                    appender.File = string.Format("Log//");

                    appender.ImmediateFlush = true;
                    appender.MaxSizeRollBackups = 10;

                    appender.StaticLogFileName = false;
                    //appender.RollingStyle = RollingFileAppender.RollingMode.Composite;
                    //appender.MaxFileSize = 10 * 1024 * 1024;
                    appender.DatePattern = $"yyyyMMdd//'{logFileName}.txt'";

                    appender.LockingModel = new FileAppender.MinimalLock();

                    appender.CountDirection = 0;
                    appender.PreserveLogFileNameExtension = true;

                    appender.AddFilter(filter);

                    appender.Layout = layout;
                    appender.AppendToFile = true;
                    appender.ActivateOptions();

                    //配置缓存，增加日志效率
                    var aa = new BufferingForwardingAppender();
                    aa.AddAppender(appender);
                    aa.BufferSize = 500;
                    aa.Lossy = false;
                    aa.Fix = log4net.Core.FixFlags.None;
                    aa.ActivateOptions();

                    log4net.Config.BasicConfigurator.Configure(log4net.LogManager.GetRepository(logFileName), aa);
                }
            }
            catch (Exception ee)
            {

                //throw;
            }
        }

        public static void Info(string context)
        {
            //logs.Enqueue(new LogModel(context, LPLogLevel.Info));
            if (logInfo.IsInfoEnabled)
            {
                logInfo.Info(context);
            }
            //log4net.LogManager.
        }

        public static void InfoReceive(string sendByte)
        {
            if (logReceive.IsInfoEnabled)
            {
                logReceive.Info(sendByte);
                logForm.ShowLog(sendByte);
            }
        }

        public static void InfoSend(string sendByte)
        {
            if (logSend.IsInfoEnabled)
            {
                logSend.Info(sendByte);
                logForm.ShowLog(sendByte);
            }
        }

        public static void Debug(string content)
        {
            if (logDebug.IsDebugEnabled)
            {
                logDebug.Debug(content);
            }
        }

        public static void Warn(string type,string formName,string info)
        {
            if (logWarn.IsWarnEnabled)
                logWarn.Warn(string.Format("【{0}：{2}】{1}",type,info,formName));
        }

        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="info"></param>
        public static void Warn(string info)
        {
            if (logWarn.IsWarnEnabled)
                logWarn.Warn(info);
        }
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ex"></param>
        public static void Error(string info,Exception ex)
        {
            if (logError.IsErrorEnabled)
            {
                if(null != ex)
                    logError.Error(info, ex);
                else
                {
                    logError.Error(info);
                }
            }
                
        }


    }
}
