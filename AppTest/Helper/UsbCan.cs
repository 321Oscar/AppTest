using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppTest.Helper
{
    public class UsbCan
    {
        private int DeviceType;
        private int DeviceInd;
        private int[] CANInd;
        INIT_CONFIG InitConfig;
        //PVCI_ERR_INFO pErrInfo;
        //private uint RefType;

        public bool IsOpen = false;

        /// <summary>
        /// USBCAN口
        /// </summary>
        /// <param name="Device_Type">设备类型</param>
        /// <param name="Device_Ind">设备索引</param>
        /// <param name="CAN_Ind">CAN口索引</param>
        public UsbCan(int Device_Type, int Device_Ind, int[] CAN_Ind)
        {
            DeviceType = Device_Type;
            DeviceInd = Device_Ind;
            CANInd = CAN_Ind;
        }

        /// <summary>
        /// USBCAN口
        /// </summary>
        /// <param name="Device_Type">设备类型</param>
        /// <param name="Device_Ind">设备索引</param>
        /// <param name="CAN_Ind">CAN口索引</param>
        public UsbCan(int Device_Type, int Device_Ind, int CAN_Ind)
        {
            DeviceType = Device_Type;
            DeviceInd = Device_Ind;
            CANInd = new int[] { CAN_Ind };
        }

        /// <summary>
        /// 配置Can口
        /// </summary>
        /// <param name="AccCode"></param>
        /// <param name="AccMask"></param>
        /// <param name="Reserved"></param>
        /// <param name="Filter"></param>
        /// <param name="Timing0">设置波特率</param>
        /// <param name="Timing1"></param>
        /// <param name="Mode"></param>
        /// <param name="canIndex">第几路CAN</param>
        /// <returns></returns>
        public int Config(char Mode, int canIndex, char Timing0, uint AccCode = 0,uint AccMask = 0xffffffff, uint Reserved = 0, 
            char Filter = (char)1 ,char Timing1 = (char)28)
        {
            InitConfig = new INIT_CONFIG();
            InitConfig.AccCode = AccCode;
            InitConfig.AccMask = AccMask;
            InitConfig.Reserved = Reserved;
            InitConfig.Filter = Filter;
            InitConfig.Timing0 = Timing0;
            InitConfig.Timing1 = Timing1;
            InitConfig.Mode = Mode;

            INIT_CONFIG[] cfg = new INIT_CONFIG[1];
            cfg[0] = InitConfig;

            return (int)ZLG_VCI_InitCAN(DeviceType, DeviceInd, canIndex, cfg);
        }

        public uint Open_Device()
        {
            return ZLG_VCI_OpenDevice(DeviceType, DeviceInd, 0);
        }

        public bool close_device()
        {
            IsOpen = false;
            return ZLG_VCI_CloseDevice(DeviceType, DeviceInd);
        }

        public int start_can(int canindex = 0)
        {
            return ZLG_VCI_StartCAN(DeviceType, DeviceInd, canindex);
        }

        /// <summary>
        /// 设置波特率（个别设备）
        /// </summary>
        /// <param name="baud"></param>
        /// <param name="canIndex"></param>
        /// <returns></returns>
        unsafe public bool SetReference(uint baud, int canIndex = 0)
        {
            return ZLG_VCI_SetReference((uint)DeviceType, (uint)DeviceInd, (uint)canIndex, 0, (byte*)&baud) == 1;
        }
        
        /// <summary>
        /// 发送Can口数据
        /// </summary>
        /// <param name="msg">数据</param>
        /// <param name="extframe">数据帧类型</param>
        /// <param name="canindex">Can通道</param>
        /// <param name="sendType">发送类型<see cref="CANSendType"/></param>
        /// <returns></returns>
        public int CAN_Send(CAN_msg_byte msg, int extframe,int canindex = 0,byte sendType = 2)
        {

            ZLG_PVCI_CAN_OBJ obj = new ZLG_PVCI_CAN_OBJ();

            obj.DataLen = 8;
            obj.SendType = sendType;
            obj.RemoteFlag = 0;
            obj.ExternFlag = (byte)extframe;
            obj.ID = (uint)msg.cid;
            obj.data = new byte[8];

            obj.data[0] = msg.w[0];
            obj.data[1] = msg.w[1];
            obj.data[2] = msg.w[2];
            obj.data[3] = msg.w[3];
            obj.data[4] = msg.w[4];
            obj.data[5] = msg.w[5];
            obj.data[6] = msg.w[6];
            obj.data[7] = msg.w[7];

            ZLG_PVCI_CAN_OBJ[] cans = new ZLG_PVCI_CAN_OBJ[1];
            cans[0] = obj;

           
            return (int)ZLG_VCI_Transmit(DevType: DeviceType,
                                         DevIndex: DeviceInd,
                                         CANIndex: canindex,
                                         pSend: cans,
                                         Len: 1);
        }

        public CAN_msg[] CAN_Receive(int canindex = 0)
        {
            uint res = VCI_GetReceiveNum((uint)DeviceType, (uint)DeviceInd, (uint)canindex);

            ZLG_PVCI_CAN_OBJ[] rcvbuf = new ZLG_PVCI_CAN_OBJ[res];
            CAN_msg[] can_mail = new CAN_msg[res];

            if (res == 0)
                return can_mail;

            int NumValue = (int)ZLG_VCI_Receive(DevType: DeviceType,
                                DevIndex: DeviceInd,
                                CANIndex: canindex,
                                pReceive: rcvbuf,
                                Len: (uint)rcvbuf.Length,
                                WaitTime: 5);

            if ((NumValue < 0) || (NumValue > res))
                return null;


            //for (int i = 0; i < NumValue && i < 200; i++)
            for (int i = 0; i < NumValue; i++)
            {
                can_mail[i] = new CAN_msg((int)rcvbuf[i].ID, rcvbuf[i].data, rcvbuf[i].TimeStamp);
                LogHelper.InfoReceive($"接收 通道{canindex}：{can_mail[i]}");
            }

            return can_mail;
        }

        /// <summary>
        /// 接收数据，按照can索引分组
        /// </summary>
        public List<List<CAN_msg>> ReceiveData;

        /// <summary>
        /// 接收数据线程
        /// </summary>
        Thread recv_thread;

        /// <summary>
        /// 是否启动接收数据
        /// </summary>
        bool recv_start;

        static readonly object locker = new object();

        /// <summary>
        /// 启动/关闭接收数据线程
        /// </summary>
        /// <param name="start">启动/关闭</param>
        /// <param name="canind">接收数据的can索引</param>
        public void StartReceive(bool start,int[] canind)
        {
            this.CANInd = canind;
            var canids = CANInd.ToList();
            canids.Sort();
            CANInd = canids.ToArray();
            recv_start = start;
            if (start)
            {
                recv_thread = new Thread(RecvDataFunc);
                recv_thread.IsBackground = true;
                recv_thread.Start();
            }
            else
            {
                if (recv_thread != null)
                    recv_thread.Join();
                recv_thread = null;
                ReceiveData = null;
            }
        }
        private int maxDataCount = 100000;
        /// <summary>
        /// 接收数据的最大存储数量，默认100000
        /// </summary>
        public int MaxDataCount { get => maxDataCount; set => maxDataCount = value; }
        private readonly Stopwatch sw = new Stopwatch();
        /// <summary>
        /// 接收数据循环
        /// </summary>
        private void RecvDataFunc()
        {
            while (recv_start)
            {
                sw.Reset();
                sw.Restart();
                lock (locker)
                {
                    if (ReceiveData == null)
                    {
                       
                        ReceiveData = new List<List<CAN_msg>>();
                        for (int i = 0; i < CANInd.Length; i++)
                            ReceiveData.Add(new List<CAN_msg>());
                    }

                    for (int i = 0; i < CANInd.Length; i++)
                    {
                        var recv = this.CAN_Receive(CANInd[i]);
                        CANDataRecieveEventArgs args = new CANDataRecieveEventArgs(recv);
                        if (CANInd[i] == 0 && OnChannal0DataRecieveEvent != null)
                        {
                            OnChannal0DataRecieved(args);
                        }
                        if (CANInd[i] == 1 && OnChannal1DataRecieveEvent != null)
                        {
                            OnChannal1DataRecieved(args);
                        }

                        for (int j = 0; j < recv.Length; j++)
                        {
                            if (ReceiveData[i].Count > MaxDataCount)
                            {
                                ReceiveData[i].RemoveAt(0);
                            }
                            ReceiveData[i].Add(recv[j]);
                        }

                    }
                }
                sw.Stop();
                long times = sw.ElapsedMilliseconds;
                LogHelper.WriteToOutput("USBCAN", "获取数据并下发,使用了" + times + "毫秒");
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// CAN接收事件参数
        /// </summary>
        public class CANDataRecieveEventArgs : EventArgs
        {
            public CAN_msg[] can_msgs;

            public CANDataRecieveEventArgs(CAN_msg[] can_msgs)
            {
                this.can_msgs = can_msgs;
            }

        }

        static void OnDataRecievedComplete(IAsyncResult asyncResult)
        {
            AsyncResult result = (AsyncResult)asyncResult;
            RecieveDataEventHandler del = (RecieveDataEventHandler)result.AsyncDelegate;
            del.EndInvoke(asyncResult);
        }

        /// <summary>
        /// 添加订阅
        /// </summary>
        /// <param name="recieveMethod"></param>
        /// <param name="canChannel"></param>
        public void Register(RecieveDataEventHandler recieveMethod,int canChannel)
        {
            if (canChannel == 0)
            {
                OnChannal0DataRecieveEvent += recieveMethod;
            }
            else if (canChannel == 1)
            {
                OnChannal1DataRecieveEvent += recieveMethod;
            }
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="recieveMethod"></param>
        /// <param name="canChannel"></param>
        public void UnRegister(RecieveDataEventHandler recieveMethod, int canChannel)
        {
            if (canChannel == 0)
            {
                OnChannal0DataRecieveEvent -= recieveMethod;
            }
            else if (canChannel == 1)
            {
                OnChannal1DataRecieveEvent -= recieveMethod;
            }
        }

        protected virtual void OnChannal0DataRecieved(CANDataRecieveEventArgs args)
        {
            //AsyncCallback callback = new AsyncCallback(OnDataRecievedComplete);
            //多播委托不支持异步
            //OnChannal0DataRecieveEvent?.BeginInvoke(this, args, callback, null);
            OnChannal0DataRecieveEvent?.Invoke(this, args);
        }

        protected virtual void OnChannal1DataRecieved(CANDataRecieveEventArgs args)
        {
            //OnChannal0DataRecieveEvent?.BeginInvoke(this, args, null, null);
            OnChannal1DataRecieveEvent?.Invoke(this, args);
            //AsyncCallback callback = new AsyncCallback(OnDataRecievedComplete);
            //OnChannal1DataRecieveEvent?.BeginInvoke(this, args, callback, null);
        }

        public delegate void RecieveDataEventHandler(object sender, CANDataRecieveEventArgs args);
        /// <summary>
        /// 通道0接收数据
        /// </summary>
        private event RecieveDataEventHandler OnChannal0DataRecieveEvent;
        /// <summary>
        /// 通道1接收数据
        /// </summary>
        private event RecieveDataEventHandler OnChannal1DataRecieveEvent;

        /// <summary>
        /// 根据ID接收数据
        /// </summary>
        /// <param name="canIndex"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public CAN_msg[] Receive(int canIndex, int[] ids)
        {
            List<CAN_msg> data = new List<CAN_msg>();

            string idstr = string.Empty;
            for (int i = 0; i < ids.Length; i++)
            {
                idstr += $"{ids[i]},";
            }

            try
            {
                //List<CAN_msg> alldata = new List<CAN_msg>(ReceiveData[canIndex]);
                List<CAN_msg> alldata = Clone<CAN_msg>(ReceiveData[canIndex]);

                if (alldata == null || alldata.Count == 0)
                    return data.ToArray();
                data = alldata.Where(x => x != null && idstr.Contains(x.cid.ToString())).ToList();
                data.Sort();
                if (data.Count > 0)
                {
                    int removeCount = ReceiveData[canIndex].RemoveAll(x => x != null && idstr.Contains(x.cid.ToString()));
                    LogHelper.InfoReceive($"接收：{data.Count}。删除：{removeCount}");
                }
                else
                {
                    LogHelper.InfoReceive($"没有接收到目标ID数据帧{ids:X}");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Receive",ex);
            }

            return data.ToArray();

        }

        public static List<T> Clone<T>(object List)
        {
            using(Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, List);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream) as List<T>;
            }
        }

       // public IEnumerable

        #region CAN接受和发送数据定义
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct ZLG_PVCI_CAN_OBJ
        {
            public uint ID;
            /// <summary>
            /// 时间戳：时间标示从 CAN 卡上电开始计时，计时单位为 0.1ms
            /// </summary>
            public uint TimeStamp;
            /// <summary>
            /// 是否使用时间标识。为 1 时 TimeStamp 有效， TimeFlag 和 TimeStamp 只在此帧为接收帧时有意义。
            /// </summary>
            public byte TimeFlag;
            public byte SendType;
            public byte RemoteFlag;
            public byte ExternFlag;
            public byte DataLen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] data;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] Reserved;
        }
        #endregion
        #region 其他CAN函数数据定义
        //public struct PVCI_ERR_INFO
        //{
        //    //public uint ErrCode;
        //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        //    //public byte[] Passive_ErrData;
        //    //public byte ArLost_ErrData;
        //}

        //5.定义初始化CAN的数据类型
        /// <summary>
        /// 初始化参数结构
        /// </summary>
        public struct INIT_CONFIG
        {
            public uint AccCode;
            public uint AccMask;
            /// <summary>
            /// 保留
            /// </summary>
            public uint Reserved;
            /// <summary>
            /// 滤波方式，1 表示单滤波，0 表示双滤波
            /// </summary>
            public char Filter;
            /// <summary>
            /// 用来设置波特率-设备类型为 PCI-5010-U、PCI-5020-U、USBCAN-E-U、 USBCAN-2E-U、USBCAN-4E-U、CANDTU 时，波特率和帧过滤不在这里设置
            /// </summary>
            public char Timing0;
            public char Timing1;
            /// <summary>
            /// 模式，0 表示正常模式，1 表示只听模式
            /// </summary>
            public char Mode;
        }

        #endregion
        #region DLL连接
        /// <summary>
        /// 打开CAN口
        /// </summary>
        /// <param name="DeviceType">设备类型</param>
        /// <param name="DeviceInd">设备索引</param>
        /// <param name="Reserved">保留参数，通常为0</param>
        /// <returns>1表示操作成功，0表示操作失败</returns>
        [DllImport(@"ControlCAN.dll", EntryPoint = "VCI_OpenDevice", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZLG_VCI_OpenDevice(int DeviceType, int DeviceInd, int Reserved);
        /// <summary>
        /// 初始化Can通道，有多个CAN通道时，需要多次调用
        /// <remarks>
        ///<para>
        /// 当设备类型为 PCI-5010-U、PCI-5020-U、USBCAN-E-U、USBCAN-2E-U、USBCAN-4E-U 时，必须在调用此函数之前调用 VCI_SetReference 对波特率进行设置。
        ///</para> 
        /// </remarks>
        /// </summary>
        /// <param name="DeviceType">设备类型</param>
        /// <param name="DeviceInd">设备索引</param>
        /// <param name="CANInd">Can口索引</param>
        /// <param name="pInitConfig"></param>
        /// <returns>为 1 表示操作成功，0 表示操作失败。</returns>
        [DllImport("ControlCAN.dll", EntryPoint = "VCI_InitCAN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZLG_VCI_InitCAN(int DeviceType, int DeviceInd, int CANInd, INIT_CONFIG[] pInitConfig);

        //
        /// <summary>
        /// 调用接受设备动态连接库
        /// </summary>
        /// <param name="DevType">设备类型</param>
        /// <param name="DevIndex">设备索引</param>
        /// <param name="CANIndex">第几路 CAN。</param>
        /// <param name="pReceive">用来接收的帧结构体 VCI_CAN_OBJ 数组的首指针。</param>
        /// <param name="Len">用来接收的帧结构体数组的长度（本次接收的最大帧数，实际返回值小于等于这个值）。</param>
        /// <param name="WaitTime">缓冲区无数据，函数阻塞等待时间，以毫秒为单位。若为-1 则表示无超时，一直等待。</param>
        /// <returns>返回实际读取到的帧数。如果返回值为 0xFFFFFFFF，则表示读取数据失败，有错误发生，请调用 VCI_ReadErrInfo 函数来获取错误码。</returns>
        [DllImport("ControlCAN.dll", EntryPoint = "VCI_Receive", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZLG_VCI_Receive(int DevType, int DevIndex, int CANIndex, [Out] ZLG_PVCI_CAN_OBJ[] pReceive, uint Len, int WaitTime);
        //调用报错动态连接库
        //[DllImport("ControlCAN.dll", EntryPoint = "VCI_ReadErrInfo", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        //public static extern int ZLG_VCI_ReadErrInfo(int DevType, int DevIndex, int CANIndex, PVCI_ERR_INFO pErrInfo);
        /*关闭设备连接库*/
        /// <summary>
        /// 关闭设备连接库
        /// </summary>
        /// <param name="DevType">设备类型</param>
        /// <param name="DevIndex">设备索引</param>
        /// <returns></returns>
        [DllImport("ControlCAN.dll", EntryPoint = "VCI_CloseDevice", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ZLG_VCI_CloseDevice(int DevType, int DevIndex);
        /*CAN启动CAN连接库*/
        /// <summary>
        /// 此函数用以启动 CAN 卡的某一个 CAN 通道。有多个 CAN 通道时，需要多次调用
        /// </summary>
        /// <param name="DevType">设备类型</param>
        /// <param name="DevIndex">设备索引号，比如当只有一个 PCIe-9221 时，索引号为 0，这时再插入一个 PCIe-9221，那么后面插入的这个设备索引号就是 1，以此类推。</param>
        /// <param name="CANIndex">第几路 CAN。即对应卡的 CAN 通道号，CAN0 为 0，CAN1 为 1，以此类推。</param>
        /// <returns>为 1 表示操作成功，0 表示操作失败。</returns>
        [DllImport("ControlCAN.dll", EntryPoint = "VCI_StartCAN", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int ZLG_VCI_StartCAN(int DevType, int DevIndex, int CANIndex);
        /*CAN发送连接库*/
        /// <summary>
        /// CAN发送连接库
        /// </summary>
        /// <param name="DevType">设备类型</param>
        /// <param name="DevIndex">设备索引</param>
        /// <param name="CANIndex">第几路 CAN。</param>
        /// <param name="pSend">要发送的帧结构体 VCI_CAN_OBJ 数组的首指针。</param>
        /// <param name="Len">要发送的帧结构体数组的长度（发送的帧数量）</param>
        /// <returns>返回实际发送成功的帧数。</returns>
        [DllImport("ControlCAN.dll", EntryPoint = "VCI_Transmit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZLG_VCI_Transmit(int DevType, int DevIndex, int CANIndex, ZLG_PVCI_CAN_OBJ[] pSend, uint Len);

        /// <summary>
        /// 此函数用以设置 CANET 与 PCI-5010-U/PCI-5020-U/USBCAN-E-U/ USBCAN-2E-U/USBCAN-4E-U/CANDTU/USBCAN-8E-U/CANDTU-NET 等设备的相应参数，
        /// 主要处理不同设备的特定操作
        /// </summary>
        /// <param name="DeviceType"></param>
        /// <param name="DeviceInd"></param>
        /// <param name="CANInd"></param>
        /// <param name="RefType">参数类型</param>
        /// <param name="pData">用来存储参数有关数据缓冲区地址首指针</param>
        /// <returns></returns>
        [DllImport("controlcan.dll", EntryPoint = "VCI_SetReference", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        //static extern UInt32 VCI_SetReference(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, UInt32 RefType, ref byte pData);
        public unsafe static extern uint ZLG_VCI_SetReference(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, UInt32 RefType, byte* pData);

        //[DllImport("controlcan.dll")]
        //public static extern uint ZLG_VCI_ReadErrInfo(uint DevType,uint DeviceIndex,uint CanIndex,[Out] );
        /// <summary>
        /// 此函数用以获取指定 CAN 通道的接收缓冲区中，接收到但尚未被读取的帧数量。 
        /// 主要用途是配合 VCI_Receive 使用，即缓冲区有数据，再接收。
        /// 用户无需一直调用 VCI_Receive，可以节约 PC 系统资源，提高程序效率
        /// </summary>
        /// <param name="DeviceType"></param>
        /// <param name="DeviceInd"></param>
        /// <param name="CANInd"></param>
        /// <returns>返回尚未被读取的帧数</returns>
        [DllImport("controlcan.dll")]
        public static extern UInt32 VCI_GetReceiveNum(UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd);
        #endregion

    }

    public class CAN_msg_byte
    {
        public long cid = 0;
        public Byte[] w = new Byte[8];

        public CAN_msg_byte(int id, byte[] data)
        {
            if (data.Length < 8)
                return;
            cid = id;

            w[0] = data[0];
            w[1] = data[1];
            w[2] = data[2];
            w[3] = data[3];
            w[4] = data[4];
            w[5] = data[5];
            w[6] = data[6];
            w[7] = data[7];
        }
        public override string ToString()
        {
            string log = $"ID:{this.cid:X};Data:";//(int) + "\n\r";
            for (int j = 0; j < this.w.Length; j++)
            {
                log += this.w[j].ToString("X") + " ";
            }
            return log;
        }
    }

    [Serializable]
    public class CAN_msg :IComparable<CAN_msg>,ICloneable
    {
        public int cid = 0;
        public int index = 0;
        public UInt16[] w = new ushort[4];
        public Byte[] b = new byte[8];
        public string timeStamp;

        public CAN_msg(int id, byte[] data, uint timeStamp)
        {
            if (data.Length < 8)
                return;

            this.timeStamp = timeStampConvertDateTimeStr(timeStamp);

            cid = id;
            index = data[0] >> 4;
            b[0] = data[0];
            b[1] = data[1];
            b[2] = data[2];
            b[3] = data[3];
            b[4] = data[4];
            b[5] = data[5];
            b[6] = data[6];
            b[7] = data[7];

            w[0] = (ushort)((data[0] << 8) + data[1]);
            w[1] = (ushort)((data[2] << 8) + data[3]);
            w[2] = (ushort)((data[4] << 8) + data[5]);
            w[3] = (ushort)((data[6] << 8) + data[7]);

        }

        public object Clone()
        {
            BinaryFormatter formatter = new BinaryFormatter(null, new StreamingContext(System.Runtime.Serialization.StreamingContextStates.Clone));
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, this);
            stream.Position = 0;
            object cloneObj = formatter.Deserialize(stream);
            stream.Close();
            return cloneObj;
        }

        public int CompareTo(CAN_msg other)
        {
            int rst = this.timeStamp.CompareTo(other.timeStamp);
            return rst;
        }

        public override string ToString()
        {
            string log = $"ID:{this.cid:X};Data:";//(int) + "\n\r";
            for (int j = 0; j < this.b.Length; j++)
            {
                log += this.b[j].ToString("X").PadLeft(2,'0') + " ";
            }
            return log;
        }

        private string timeStampConvertDateTimeStr(uint timeStamp)
        {
            //DateTime t = DateTime.Now;

            //System.DateTime startTime = new DateTime(t.Year, t.Month, t.Day,0,0,0);
            //t = startTime.AddMilliseconds(timeStamp);
            int hour = (int)(timeStamp / 36000000);
            int minute = (int)(timeStamp - hour*36000000)/600000;
            int second = (int)(timeStamp - hour * 36000000  -minute * 600000)/10000;
            int ms = (int)(timeStamp - hour * 36000000  -minute * 600000 - second* 10000) / 10;
            int mms = (int)(timeStamp - hour * 36000000  -minute * 600000 - second * 10000 - ms*10);
            //t = new DateTime(t.Year, t.Month, t.Day, hour, minute, second, ms);
            return $"{hour.ToString().PadLeft(2, '0')}:{minute.ToString().PadLeft(2, '0')}:{second.ToString().PadLeft(2, '0')} {ms.ToString().PadLeft(3, '0')}.{mms}";
        }
    }
}
