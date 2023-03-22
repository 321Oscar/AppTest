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
        /// <param name="data"></param>
        /// <param name="eventIndex"></param>
        /// <returns></returns>
        Task ParseResponeToXCPSignalAsync(List<byte> data, int eventIndex);
    }
}
