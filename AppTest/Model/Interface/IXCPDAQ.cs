using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTest.Model
{
    interface IXCPDAQ
    {
        /// <summary>
        /// 配置DAQ列表
        /// </summary>
        /// <param name="canChannel"></param>
        /// <returns></returns>
        bool InitDAQ(uint canChannel);
        /// <summary>
        /// 解析报文
        /// </summary>
        /// <param name="data">拼接后的报文</param>
        /// <param name="eventIndex">信号所属的 daq 事件通道索引</param>
        /// <returns></returns>
        Task ParseResponeToXCPSignalAsync(List<byte> data, int eventIndex);
    }
}
