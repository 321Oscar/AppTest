using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AppTest.Helper
{
    public class HiperfTimer
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        private long startTime, stopTime;
        private long freq;

        public HiperfTimer()
        {
            startTime = stopTime = 0;
            if(QueryPerformanceFrequency(out freq) == false)
            {
                throw new Win32Exception();
            }
        }

        public delegate void Send(object obj);

        /// <summary>
        /// 定时器执行事件
        /// </summary>
        public event Send SendData;

        public void Start(object obj,int interval)
        {
            if(SendData != null)
            {
                SendData(obj);
            }

            QueryPerformanceCounter(out startTime);
        }

        ///public void Stop                                                                           
    }
}
