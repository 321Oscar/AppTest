using AppTest.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTest.ProjectClass
{
    public interface IProtocol
    {
        /// <summary>
        /// 单个信号解析
        /// </summary>\
        /// <param name="can_msg">原始数据</param>
        /// <returns></returns>
        string Single(CAN_msg[] can_msg,DBCSignal signalItem);

        /// <summary>
        /// 多个信号解析
        /// </summary>
        /// <param name="can_msg">原始数据</param>
        /// <param name="singals">测量信号</param>
        /// <returns>Dictionary <see cref="DBCSignal"/>, string></returns>
        Dictionary<DBCSignal, string> Multip(CAN_msg[] can_msg, Singals singals);

        /// <summary>
        /// 协议文档解析出信号信息
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns><see cref="Singals"/></returns>
        Singals ProtocolFile(string FilePath);
    }
}
