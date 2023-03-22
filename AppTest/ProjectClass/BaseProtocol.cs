using AppTest.FormType;
using AppTest.FormType.Helper;
using AppTest.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTest.ProjectClass
{
    public abstract class BaseProtocol 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t">协议类型</param>
        /// <param name="fileName">协议文档名称,以“;”分割</param>
        /// <returns></returns>
        public static Singals GetSingalsByProtocol(int t, string[] fileName)
        {
            Singals singals = new Singals();
            BaseProtocol protocol;
            //string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + ((ProtocolType)t).ToString() + "\\" + fileName;
            //if(!File.Exists(filePath))
            //{
            //    throw new Exception("协议文档丢失");
            //}
            for (int i = 0; i < fileName.Length; i++)
            {
                Singals sigals = new Singals();
                switch (t)
                {
                    case (int)ProtocolType.DBC:
                        {
                            protocol = new DBCProtocol();
                            sigals = protocol.ProtocolFile(fileName[i]);
                        }

                        break;
                    case (int)ProtocolType.Excel:
                        throw new Exception("Excel协议暂不支持");
                    //singals = new Singals();
                    //break;
                    default:
                        LeapMessageBox.Instance.ShowInfo("不支持的协议");
                        break;

                }
                if (sigals.Signal.Count == 0)
                    continue;
                foreach (var item in sigals.Signal)
                {
                    if (singals.Signal.Find(x => x.SignalName == item.SignalName) == null)
                        singals.Signal.Add(item);
                }
            }
            

            return singals;
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
        public abstract Dictionary<DBCSignal, string> Multip(CAN_msg[] can_msg, Singals singals);

        /// <summary>
        /// 解析报文，yeild return
        /// </summary>
        /// <param name="can_msg"></param>
        /// <param name="singals"></param>
        /// <returns></returns>
        public abstract IEnumerable<DBCSignal> MultipYeild(CAN_msg[] can_msg, Singals singals);

        /// <summary>
        /// 解析协议中的信号
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public abstract Singals ProtocolFile(string FilePath);

        /// <summary>
        /// 解析报文中的信号
        /// </summary>
        /// <param name="can_msg"></param>
        /// <param name="signalItem"></param>
        /// <returns>信号值</returns>
        public abstract string Single(CAN_msg[] can_msg, DBCSignal signalItem);

        /// <summary>
        /// 将信号值组帧
        /// </summary>
        /// <param name="signalValue"></param>
        /// <returns></returns>
        public abstract CAN_msg_byte[] BuildFrames(Dictionary<DBCSignal, string> signalValue);
        public abstract CAN_msg_byte BuildFrame(DBCSignal signal, string value);

    }
}
