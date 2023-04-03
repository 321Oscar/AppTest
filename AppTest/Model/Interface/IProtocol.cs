using AppTest.Helper;
using AppTest.Model;
using LPCanControl.CANInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTest.ProtocolLib
{
    public interface IProtocol
    {
        /// <summary>
        /// 单个信号解析
        /// </summary>\
        /// <param name="can_msg">原始数据</param>
        /// <returns></returns>
        string Single(CANReceiveFrame[] can_msg,BaseSignal signalItem);

        /// <summary>
        /// 多个信号解析
        /// </summary>
        /// <param name="can_msg">原始数据</param>
        /// <param name="singals">测量信号</param>
        /// <returns>Dictionary <see cref="DBCSignal"/>, string></returns>
        Dictionary<BaseSignal, string> Multip(CANReceiveFrame[] can_msg, List<BaseSignal> singals);

        /// <summary>
        /// 协议文档解析出信号信息
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns><see cref="DBCSignals"/></returns>
        List<BaseSignal> ProtocolFile(string FilePath);
    }
}
