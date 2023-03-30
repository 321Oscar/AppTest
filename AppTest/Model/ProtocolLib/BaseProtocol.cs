using AppTest.FormType;
using AppTest.FormType.Helper;
using AppTest.Helper;
using AppTest.Model;
using LPCanControl.CANInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTest.ProtocolLib
{
    public abstract class BaseProtocol 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t">协议类型</param>
        /// <param name="fileName">协议文档名称,以“;”分割</param>
        /// <returns></returns>
        public static List<BaseSignal> GetSingalsByProtocol(int t, string[] fileName)
        {
            List<BaseSignal> singals = new List<BaseSignal>();
            BaseProtocol protocol;
            for (int i = 0; i < fileName.Length; i++)
            {
                switch (t)
                {
                    case (int)ProtocolType.DBC:
                            protocol = new DBCProtocol();
                        break;
                    case (int)ProtocolType.Excel:
                        throw new Exception("Excel协议暂不支持");
                    //singals = new Singals();
                    //break;
                    case (int)ProtocolType.XCP:
                        protocol = new XCPProtocol();
                        break;
                    default:
                        LeapMessageBox.Instance.ShowInfo("不支持的协议");
                        continue;
                }

                List<BaseSignal> sigals = new List<BaseSignal>();
                for (int j = 0; j < fileName.Length; j++)
                {
                    sigals = protocol.ProtocolFile(fileName[i]);
                    if (sigals.Count == 0)
                        continue;
                    foreach (var item in sigals)
                    {
                        if (singals.Find(x => x.SignalName == item.SignalName) == null)
                            singals.Add(item);
                    }
                }
            }

            return singals;
        }

        public static Task<List<BaseSignal>> GetSingalsByProtocolTask(int t, string[] fileName)
        {
            return Task.Run(async () => {
                List<BaseSignal> singals = new List<BaseSignal>();
                BaseProtocol protocol;
                for (int i = 0; i < fileName.Length; i++)
                {
                    switch (t)
                    {
                        case (int)ProtocolType.DBC:
                            protocol = new DBCProtocol();
                            break;
                        case (int)ProtocolType.Excel:
                            throw new Exception("Excel协议暂不支持");
                        //singals = new Singals();
                        //break;
                        case (int)ProtocolType.XCP:
                            protocol = new XCPProtocol();
                            break;
                        default:
                            LeapMessageBox.Instance.ShowInfo("不支持的协议");
                            continue;
                    }

                    List<BaseSignal> sigals = new List<BaseSignal>();
                    for (int j = 0; j < fileName.Length; j++)
                    {
                        sigals = await protocol.ProtocolFileTask(fileName[i]);
                        if (sigals.Count == 0)
                            continue;
                        foreach (var item in sigals)
                        {
                            if (singals.Find(x => x.SignalName == item.SignalName) == null)
                                singals.Add(item);
                        }
                    }
                }

                return singals;
            });
            
        }
        /// <summary>
        /// 协议名称
        /// </summary>
        public abstract string ProtocolName { get; set; }

        public string ProtocolClassPath;

        /// <summary>
        /// 解析报文中的信号
        /// </summary>
        /// <param name="can_msg"></param>
        /// <param name="singals"></param>
        /// <returns><see cref="Dictionary{SignalItem, string}"/>信号+值</returns>
        public abstract Dictionary<BaseSignal, string> Multip(CANReceiveFrame[] can_msg, List<BaseSignal> singals);

        /// <summary>
        /// 解析报文，yeild return
        /// </summary>
        /// <param name="can_msg"></param>
        /// <param name="singals"></param>
        /// <returns></returns>
        public abstract IEnumerable<BaseSignal> MultipYield(CANReceiveFrame[] can_msg, List<BaseSignal> singals);

        /// <summary>
        /// 解析协议中的信号
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public abstract Task<List<BaseSignal>> ProtocolFileTask(string FilePath);
        public abstract List<BaseSignal> ProtocolFile(string FilePath);

        /// <summary>
        /// 解析报文中的信号
        /// </summary>
        /// <param name="can_msg"></param>
        /// <param name="signalItem"></param>
        /// <returns>信号值</returns>
        public abstract string Single(CANReceiveFrame[] can_msg, BaseSignal signalItem);

        /// <summary>
        /// 将信号值组帧
        /// </summary>
        /// <param name="signalValue"></param>
        /// <returns></returns>
        public abstract CANSendFrame[] BuildFrames(Dictionary<BaseSignal, string> signalValue);
        public abstract CANSendFrame BuildFrame(BaseSignal signal, string value);

    }
}
