using AppTest.Helper;
using LPCanControl.CANInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AppTest.Model
{
    /*
    * 一个xcp模块
    * 
    * 属性：
    * 所属的project
    * mode:polling or block
    * master id
    * slave id
    * 
    * 字段：
    * 当前xcp状态：连接，未连接，upload，download等
    * 根据slaveid接收的数据队列
    * 
    * 方法：
    * xcp connect 
    *  通过CAN发送数据 并接收slaveid的消息
    *  返回 TRUE/False
    * xcp disconnect
    * xcp upload
    * xcp shortupload
    * xcp set_mta
    * xcp download
    * xcp getseed
    * xcp sendkey
    * 
    * 事件：
    * XCPConnectStatusChanged
    * XCPCMDStatusChanged
    * 
    * private：
    * sendCMD
    * recieve data 封装一层 判断slaveID
    * 
    */
    public class XCPModule
    {
        private uint masterid;
        private uint slaveid;
        private XCPMode current_mode = XCPMode.Polling;
        private XCPConnectStatus connectStatus = XCPConnectStatus.Init;
        private XCPCMDStatus currentCMDStatus;
        private readonly ProjectItem projectItem;
        public ProjectItem ProjectItem { get => projectItem; }
        private readonly Queue<CANRecieveFrame> receiveData;
        private readonly AutoResetEvent ReceiveEvent;
        private Thread receiveThread;
        private bool receiveRunning = false;
        private Global.ByteOrder byteOrder;
        private bool slaveBlockAvail;

        /// <summary>
        /// 主机 CAN ID
        /// </summary>
        public uint Masterid { get => masterid; set => masterid = value; }
        /// <summary>
        /// 从机CAN ID
        /// </summary>
        public uint Slaveid { get => slaveid; set => slaveid = value; }
        /// <summary>
        /// 主机 发送模式
        /// </summary>
        public XCPMode Current_Mode { get => current_mode; set => current_mode = value; }
        /// <summary>
        /// 当前的命令
        /// </summary>
        public XCPCMDStatus CurrentCMDStatus
        {
            get => currentCMDStatus;
            set
            {
                if (value != currentCMDStatus)
                {
                    WhenCMDChange(value.ToString());
                }
                currentCMDStatus = value;
            }
        }
        /// <summary>
        /// 当前连接状态
        /// </summary>
        public XCPConnectStatus ConnectStatus
        {
            get => connectStatus;
            set
            {
                if (value != connectStatus)
                {
                    WhenConnectChange(value.ToString());
                }
                connectStatus = value;
            }
        }

        /// <summary>
        /// 从机中的byteorder
        /// </summary>
        public Global.ByteOrder ByteOrder { get => byteOrder; set => byteOrder = value; }
        /// <summary>
        /// 从机是否支持block
        /// </summary>
        public bool SlaveBlockAvail { get => slaveBlockAvail; set => slaveBlockAvail = value; }

        public delegate void XCPConnectStatusChanged(object sender, EventArgs args);

        public event XCPConnectStatusChanged OnConnectStatusChanged;

        private void WhenConnectChange(string status)
        {
            if (OnConnectStatusChanged != null)
                OnConnectStatusChanged(status, null);
        }

        public delegate void XCPCMDStatusChanged(object sender, EventArgs args);

        public event XCPCMDStatusChanged OnCMDStatusChanged;

        private void WhenCMDChange(string status)
        {
            if (OnCMDStatusChanged != null)
                OnCMDStatusChanged(status, null);
        }

        public XCPModule(uint masterid, uint slaveid, ProjectItem projectItem)
        {
            this.masterid = masterid;
            this.slaveid = slaveid;
            this.projectItem = projectItem;

            ReceiveEvent = new AutoResetEvent(false);
            receiveData = new Queue<CANRecieveFrame>();
            ByteOrder = Global.ByteOrder.Intel;
            SlaveBlockAvail = true;
        }

        private void startReceiveThread(object canIndex)
        {
            if (receiveThread != null)
            {
                //receiveThread.Join();
                receiveThread = null;
            }

            receiveThread = new Thread(new ParameterizedThreadStart(Receive));
            receiveThread.IsBackground = true;
            receiveThread.Start(canIndex);
            receiveRunning = true;
        }

        private void startOrStopRec(int index, bool open)
        {
            USBCanManager.Instance.Register(projectItem, OnDataRecieveEvent, index, open);
        }

        public void ClearQueue()
        {
            this.receiveData.Clear();
        }

        /// <summary>
        /// 发送连接命令
        /// </summary>
        /// <returns></returns>
        public bool Connect(uint canIndex, uint connectMode = 0x00)
        {
            ConnectStatus = XCPConnectStatus.Connecting;

            if (SendCMD(new byte[] { XCPHelper.STD_CONNECT, (byte)connectMode }, out byte[] resData, canIndex) == XCPResponse.Ok)
            {
                ConnectStatus = XCPConnectStatus.Connected;
                //resource resData[1]
                ParseConnectInfo(resData);
                /// resData[2]
                /// Common_mode_basic 
                /// 解析byteorder 
                XCPHelper.ParseCommModeBasic(resData[2], out this.byteOrder, out this.slaveBlockAvail);

                return true;
            }
            else
            {
                ConnectStatus = XCPConnectStatus.ConnectFail;
            }

            return false;
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="canIndex"></param>
        /// <returns></returns>
        public bool DisConnect(uint canIndex)
        {
            if (!CheckConnect(canIndex))
            {
                return true;
            }

            SendCMD(new byte[] { 0xFE }, out _, canIndex);

            ConnectStatus = XCPConnectStatus.DisConnect;
            return true;
        }

        /// <summary>
        /// 读取长度小于6的数据
        /// </summary>
        /// <param name="signal"></param>
        /// <param name="canIndex"></param>
        public void ShortUpload(ref XCPSignal signal, uint canIndex)
        {
            if (!CheckConnect(canIndex))
            {
                throw new XCPException("XCP未连接！");
            }

            try
            {
                if (signal.Length > 6)
                    throw new XCPException("short upload 不支持长度大于 6 的数据!");
                if (signal.Length <= 0)
                    throw new XCPException("信号长度错误!");

                CurrentCMDStatus = XCPCMDStatus.ShortUpload;
                byte[] sendData = new byte[8];
                sendData[0] = XCPHelper.STD_SHORTUPLOAD;

                sendData[1] = (byte)signal.Length;
                sendData[2] = 0x00;//保留位
                sendData[3] = (byte)signal.AddressExtension;

                XCPHelper.TransformAddress(signal.ECUAddress, signal.ByteOrder_int, out byte[] addr);
                sendData[4] = addr[0];
                sendData[5] = addr[1];
                sendData[6] = addr[2];
                sendData[7] = addr[3];


                if (SendCMD(sendData, out byte[] resData, canIndex) == XCPResponse.Ok)
                {
                    CurrentCMDStatus = XCPCMDStatus.ShortUploadSucc;

                    byte[] valueByte = new byte[signal.Length];
                    for (int i = 0; i < signal.Length; i++)
                    {
                        valueByte[i] = resData[i + 1];//第三位开始才是数据
                    }
                    signal.StrValue = XCPHelper.DealData4Byte(signal, valueByte);
                }
                else
                {
                    CurrentCMDStatus = XCPCMDStatus.ShortUploadFail;
                }
            }
            catch (XCPException xcpEx)
            {
                throw xcpEx;
            }
        }

        /// <summary>
        /// 读取长度>6的数据，正确性待验证
        /// </summary>
        /// <param name="signal"></param>
        /// <param name="canIndex"></param>
        public void Upload(ref XCPSignal signal, uint canIndex)
        {
            if (!CheckConnect(canIndex))
            {
                throw new XCPException("XCP未连接！");
            }

            //set mta
            if (Set_MTA(signal, canIndex) != XCPResponse.Ok)
            {
                CurrentCMDStatus = XCPCMDStatus.Set_MTAFail;
                return;
            }

            //upload
            CurrentCMDStatus = XCPCMDStatus.Upload;
            List<byte> res = new List<byte>();

            int count = signal.Length / 7 + 1;
            for (int i = 0; i < count; i++)
            {
                int length;
                if ((signal.Length - (i * 7)) / 7 > 0)
                    length = 7;
                else
                    length = (signal.Length - (i * 7)) % 7;
                if (SendLongCMD(new byte[] { XCPHelper.STD_UPLOAD, (byte)length }, out List<byte[]> resData, canIndex, 1) == XCPResponse.Ok)
                {
                    CurrentCMDStatus = XCPCMDStatus.UploadSucc;
                    byte[] valueByte = new byte[length];
                    Array.Copy(resData[0], 1, valueByte, 0, length);
                    res.AddRange(valueByte);
                }
                else
                {
                    CurrentCMDStatus = XCPCMDStatus.UploadFail;
                    return;
                }
            }
            signal.StrValue = XCPHelper.DealData4Byte(signal, res.ToArray());

        }

        public void UnlockResource(uint resourceCode, uint canIndex)
        {
            if (!CheckConnect(canIndex))
            {
                throw new XCPException("XCP未连接！");
            }

            //get seed
            CurrentCMDStatus = XCPCMDStatus.GetSeed;
            //seed data
            List<byte> seeds = new List<byte>();
            if (SendCMD(new byte[] { XCPHelper.STD_GETSEED, 0x00, (byte)resourceCode }, out byte[] seedResp, canIndex) == XCPResponse.Ok)
            {
                if (seedResp[1] == 0)
                {
                    CurrentCMDStatus = XCPCMDStatus.UnlockSucc;
                    return;
                }

                for (int i = 2; i < seedResp.Length; i++)
                {
                    seeds.Add(seedResp[i]);
                }
                //seed 长度> CAN中最长字节数
                if (seedResp[1] > 6)
                {
                    do
                    {
                        if (SendCMD(new byte[] { XCPHelper.STD_GETSEED, 0x01 }, out seedResp,  canIndex) == XCPResponse.Ok)
                        {
                            for (int i = 2; i < seedResp.Length; i++)
                            {
                                seeds.Add(seedResp[i]);
                            }
                        }
                        else
                        {
                            CurrentCMDStatus = XCPCMDStatus.GetSeedFail;
                            return;
                        }
                    } while (seedResp[1] > 6);
                }
            }
            else
            {
                CurrentCMDStatus = XCPCMDStatus.GetSeedFail;
                return;
            }

            //calculate key & send key to slave
            byte[] key = XCPHelper.CalKeyWithSeed(seeds);

            //send key 
            if (key.Length <= 6)
            {
                byte[] sendData = new byte[8];
                sendData[0] = XCPHelper.STD_SENDKEY;
                sendData[1] = (byte)key.Length;
                for (int i = 2; i < key.Length + 2; i++)
                {
                    sendData[i] = key[i - 2];
                }
                if (SendCMD(sendData, out _, canIndex) == XCPResponse.Ok)
                {
                    CurrentCMDStatus = XCPCMDStatus.UnlockSucc;
                    return;
                }
            }
            else
            {
                int remainingByte = key.Length;
                int sendCount = key.Length / 6 + 1;
                for (int i = 0; i < sendCount; i++)
                {
                    byte[] sendData = new byte[8];
                    sendData[0] = XCPHelper.STD_SENDKEY;
                    sendData[1] = (byte)remainingByte;
                    //剩余数据
                    for (int j = 0; j < (remainingByte / 6 > 0 ? 6 : remainingByte); j++)
                    {
                        sendData[j + 2] = key[i * 6 + j];
                    }
                    if (SendCMD(sendData, out _, canIndex) != XCPResponse.Ok)
                    {
                        CurrentCMDStatus = XCPCMDStatus.SendKeyFail;
                        return;
                    }
                    remainingByte -= 6;
                }

                CurrentCMDStatus = XCPCMDStatus.UnlockSucc;
            }
        }

        public void Download(XCPSignal signal, uint canIndex)
        {
            if (!CheckConnect(canIndex))
            {
                throw new XCPException("XCP未连接！");
            }
            //unlock download resource

            //set mta
            if (Set_MTA(signal, canIndex) != XCPResponse.Ok)
            {
                CurrentCMDStatus = XCPCMDStatus.Set_MTAFail;
                return;
            }

            //send download
            CurrentCMDStatus = XCPCMDStatus.DownLoad;
            //数值转换
            byte[] convertBytes = XCPHelper.ConvertToByte(signal.StrValue, signal.ValueType, signal.ByteOrder_int);
            if(signal.Length > 6) 
            {
                //多次下发download
                byte[] dataByte = new byte[8];
                //CAL_Download
                dataByte[0] = XCPHelper.CAL_DOWNLOAD;
                //data length
                dataByte[1] = 6;
                Array.Copy(convertBytes, 0, dataByte, 2, 6);
                if (SendCMD(dataByte, out _, canIndex) != XCPResponse.Ok)
                { 
                    CurrentCMDStatus = XCPCMDStatus.DownLoadFail;
                    LogHelper.Info($"{signal.SignalName} 写入失败");
                    return; 
                }

                //额外发送次数
                int sendCount = (signal.Length - 6) / 7 + 1;
                for (int i = 0; i < sendCount; i++)
                {
                    int length = (signal.Length - 6 - i * 7) / 7 > 0 ? 7 : (signal.Length - 6 - i * 7) % 7;
                    int startIndex = 6 + i * 7;
                    Array.Copy(convertBytes, startIndex, dataByte, 2, length);
                    if (SendCMD(dataByte, out _, canIndex) != XCPResponse.Ok)
                    {
                        CurrentCMDStatus = XCPCMDStatus.DownLoadFail;
                        LogHelper.Info($"{signal.SignalName} 写入失败");
                        return;
                    }
                }
            } 
            else
            {
                byte[] dataByte = new byte[8];
                //CAL_Download
                dataByte[0] = XCPHelper.CAL_DOWNLOAD;
                //data length
                dataByte[1] = (byte)signal.Length;
                Array.Copy(convertBytes, 0, dataByte, 2, signal.Length);
                if (SendCMD(dataByte, out _, canIndex) == XCPResponse.Ok)
                    CurrentCMDStatus = XCPCMDStatus.DownLoadSucc;
                else
                    CurrentCMDStatus = XCPCMDStatus.DownLoadFail;
            }
        }

        private bool CheckConnect(uint canindx)
        {
            return this.Connect(canindx);
        }

        private XCPResponse Set_MTA(XCPSignal signal, uint canIndex)
        {
            try
            {
                //地址转变
                XCPHelper.TransformAddress(signal.ECUAddress, signal.ByteOrder_int, out byte[] address);

                byte[] sendData = new byte[8];
                sendData[0] = XCPHelper.STD_SET_MTA;
                sendData[1] = sendData[2] = 0;
                sendData[3] = (byte)signal.AddressExtension;
                sendData[4] = address[0];
                sendData[5] = address[1];
                sendData[6] = address[2];
                sendData[7] = address[3];

                return SendCMD(sendData, out byte[] resData, canIndex);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private readonly object locker = new object();

        /// <summary>
        /// 发送命令-返回一帧
        /// </summary>
        /// <param name="data"></param>
        /// <param name="resData"></param>
        /// <param name="canIndex"></param>
        /// <returns></returns>
        private XCPResponse SendCMD(byte[] data, out byte[] resData, uint canIndex)
        {
            resData = new byte[1] { 0xFE };
            //receiveData.Clear();
            lock (locker)
            {
                try
                {
                    if (!receiveRunning)
                        startOrStopRec((int)canIndex, true);

                    USBCanManager.Instance.Send(projectItem, (int)canIndex, new CANSendFrame((int)masterid, data));

                    //等待
                    if (ReceiveEvent.WaitOne(1000))
                    {
                        resData = receiveData.Dequeue().b;
                        return XCPHelper.TransformBytetoRes(resData[0]);
                    }
                    else
                    {
                        return XCPResponse.Out_time;
                    }
                }
                catch (USBCANOpenException canEx)
                {
                    this.ConnectStatus = XCPConnectStatus.ConnectFail;
                    throw canEx;
                }
                catch (Exception ex)
                {
                    LogHelper.Error("SendCMD", ex);
                    return XCPResponse.Out_time;
                }
                finally
                {
                    startOrStopRec((int)canIndex, false);
                    receiveData.Clear();
                    receiveRunning = false;
                    ReceiveEvent.Reset();
                }
            }

        }

        /// <summary>
        /// 返回多帧
        /// </summary>
        /// <param name="data"></param>
        /// <param name="resDatas"></param>
        /// <param name="canIndex"></param>
        /// <param name="resCount"></param>
        /// <returns></returns>
        private XCPResponse SendCMD(byte[] data, out List<byte[]> resDatas, uint canIndex, uint resCount = 1)
        {
            resDatas = new List<byte[]>();
            lock (locker)
            {
                try
                {
                    if (!receiveRunning)
                        startOrStopRec((int)canIndex, true);

                    USBCanManager.Instance.Send(projectItem, (int)canIndex, new CANSendFrame((int)masterid, data));

                    //等待
                    int count = 0;

                    if (ReceiveEvent.WaitOne(1000))
                    {
                        while (count < resCount)
                        {
                            resDatas.Add(receiveData.Dequeue().b);
                            count++;
                        }
                    }
                    else
                    {
                        return XCPResponse.Out_time;
                    }

                    return XCPHelper.TransformBytetoRes(resDatas[0][0]);
                }
                catch (USBCANOpenException canEx)
                {
                    this.ConnectStatus = XCPConnectStatus.ConnectFail;
                    throw canEx;
                }
                catch (Exception ex)
                {
                    LogHelper.Error("SendCMD", ex);
                    return XCPResponse.Out_time;
                }
                finally
                {
                    startOrStopRec((int)canIndex, false);
                    receiveRunning = false;
                    ReceiveEvent.Reset();
                }
            }

        }

        public List<string> EventNames { private set; get; }

        public DAQProperty DAQProperty { private set; get; }

        /// <summary>
        /// 获取事件通道信息 DA
        /// </summary>
        /// <returns></returns>
        public List<string> GetEventsName(uint canIndex)
        {
            if (!CheckConnect(canIndex))
            {
                LogHelper.Warn("XCP未连接！");
                return null;
            }

            EventNames = new List<string>();
            if (DAQProperty == null)
                GetDAQInfo(canIndex);

            for (int i = 0; i < DAQProperty.Max_Event_Channel; i++)
            {
                byte[] sendData = new byte[4];
                sendData[0] = 0xD7;
                sendData[1] = 0x00;
                var d = BitConverterExt.GetBytes((short)i, false);
                sendData[2] = d[0];
                sendData[3] = d[1];
                if (SendCMD(sendData, out byte[] resData, canIndex) == XCPResponse.Ok)
                {
                    DAQEventProperty dAQEventProperty = new DAQEventProperty(resData);

                    if (SendCMD(new byte[] { 0xF5, dAQEventProperty.EventChannelNameLength }, out List<byte[]> resDatas, canIndex, (uint)dAQEventProperty.EventChannelNameLength / 7 + 1) == XCPResponse.Ok)
                    {
                        List<byte> nameBytes = new List<byte>();
                        var len = dAQEventProperty.EventChannelNameLength;
                        for (int j = 0; j < resDatas.Count; j++)
                        {
                            var len1 = len / 7 > 0 ? 7 : len % 7;
                            len -= 7;
                            //从第二位开始，第一位是FF
                            for (int m = 1; m < len1 + 1; m++)
                            {
                                nameBytes.Add(resDatas[j][m]);
                            }
                            
                        }
                        dAQEventProperty.EventName = System.Text.Encoding.ASCII.GetString(nameBytes.ToArray());
                        EventNames.Add(dAQEventProperty.EventName);
                    }
                }
            }

            return EventNames;
        }

        public void GetDaqClock()
        {

        }

        public DAQProperty GetDAQInfo(uint canIndex)
        {
            DAQProperty = null;

            if (SendCMD(new byte[] { 0xDA }, out byte[] resData, canIndex) == XCPResponse.Ok)
            {
                DAQProperty = new DAQProperty(resData);
            }
            else
            {
                throw new XCPException("DA 命令失败");
            }

            return DAQProperty;
        }

        public XCPResponse FreeDAQ(uint canIndex)
        {
            return SendCMD(new byte[] { 0xD6 }, out _, canIndex);
        }

        /// <summary>
        /// DAQ 
        /// </summary>
        /// <param name="canIndex"></param>
        /// <returns></returns>
        public XCPResponse GetDAQProcessResolution(uint canIndex)
        {
            var res = SendCMD(new byte[] { 0xD9 }, out byte[] data, canIndex);
            if (res == XCPResponse.Ok)
            {
                //解析信息
                ParseDAQProcessInfo(data);
            }
            else
            {
                throw new XCPException("D9 命令失败");
            }
            return res;
        }

        #region Connect--0xFF
        /// <summary>
        /// 0 = not support
        /// </summary>
        public bool CalPageSupport { get; private set; }
        /// <summary>
        /// 0 = not support
        /// </summary>
        public bool DAQSupport { get; private set; }
        /// <summary>
        /// 0 = not support
        /// </summary>
        public bool STIMPageSupport { get; private set; }
        /// <summary>
        /// 0 = not support
        /// </summary>
        public bool PGMSupport { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public byte MAX_CTO { get; private set; }
        public short MAX_DTO { get; private set; }
        public byte XCP_Protocol_Version { get; private set; }
        public byte XCP_Transport_Version { get; private set; }
        /// <summary>
        /// 0xFF
        /// </summary>
        /// <param name="data"></param>
        private void ParseConnectInfo(byte[] data)
        {
            /// Parse RESSOURCE
            /// to do
            //Parse COMMON_MODE_BASIC
            XCPHelper.ParseCommModeBasic(data[2], out this.byteOrder, out this.slaveBlockAvail);
            MAX_CTO = data[3];
            MAX_DTO = BitConverterExt.ToInt16(data, 4, (int)byteOrder);
            XCP_Protocol_Version = data[6];
            XCP_Transport_Version = data[7];
        }
        #endregion

        #region 0xD9
       
        /// <summary>
        /// 0xD9
        /// </summary>
        /// <param name="data"></param>
        private void ParseDAQProcessInfo(byte[] data)
        {
            Granularity_ODT_Entry_Size_Daq = data[1];
            Max_ODT_Entry_Size_Daq = data[2];
            Granularity_ODT_Entry_Size_STIM = data[3];
            Max_ODT_Entry_Size_STIM = data[4];
            Timestamp_Mode = data[5];
            TimeStamp_Ticks = BitConverterExt.ToInt16(data, 6, (int)byteOrder);
        }
        
        public byte Granularity_ODT_Entry_Size_Daq { get; private set; }
        public byte Max_ODT_Entry_Size_Daq { get; private set; }
        public byte Granularity_ODT_Entry_Size_STIM { get; private set; }
        public byte Max_ODT_Entry_Size_STIM { get; private set; }
        public byte Timestamp_Mode { get; private set; }
        public short TimeStamp_Ticks { get; private set; }
        #endregion

        /// <summary>
        /// 根据信号，通道，配置DAQ列表
        /// </summary>
        public bool SetDAQ(List<DAQ> daqList, uint canIndex)
        {
            if (!CheckConnect(canIndex))
            {
                throw new XCPException("XCP未连接！");
            }
            //-获取DAQ信息(0xDA)
            GetDAQInfo(canIndex);
            //获取DAQ processing info（0xD9）
            GetDAQProcessResolution(canIndex);
            //-读取事件通道信息(0xD7)
            GetEventsName(canIndex);
            //-释放已配置的DAQ(0xD6)
            FreeDAQ(canIndex);
            //-根据DAQCount分配DAQ组（0xD5）
            short daqCount = (short)daqList.Count;
            byte[] sendData = new byte[8];
            sendData[0] = 0xD5;
            sendData[1] = 0x00;
            var number = BitConverterExt.GetBytes(daqCount, false);//daq Count
            sendData[2] = number[0];
            sendData[3] = number[1];

            if (SendCMD(sendData, out byte[] res, canIndex) != XCPResponse.Ok)
                throw new XCPException("分配 DAQ (D5) 失败:" + XCPHelper.ParseErrCode(res[1]));

            //-根据DAQ数量，将ODT填入DAQ列表中（0xD4）
            for (short n = DAQProperty.Min_DAQ; n < DAQProperty.Min_DAQ + daqCount; n++)
            {
                sendData[0] = 0xD4;
                sendData[1] = 0x00;
                number = BitConverterExt.GetBytes(n, false);//daq list number
                sendData[2] = number[0];
                sendData[3] = number[1];
                sendData[4] = (byte)daqList[n].ODTs.Count;//odt count
                if (SendCMD(sendData, out res, canIndex) != XCPResponse.Ok)
                    throw new XCPException($"分配 DAQ {n} (D4) 失败:" + XCPHelper.ParseErrCode(res[1]));
            }

            //-将ODT_Entry填入ODT中（0xD3）
            for (short n = DAQProperty.Min_DAQ; n < DAQProperty.Min_DAQ + daqCount; n++)
            {
                for (byte j = 0; j < daqList[n].ODTs.Count; j++)
                {
                    sendData[0] = 0xD3;
                    sendData[1] = 0x00;
                    number = BitConverterExt.GetBytes(n, false);            //daq list number
                    sendData[2] = number[0];
                    sendData[3] = number[1];
                    sendData[4] = j;                                        //ODT_Number
                    sendData[5] = (byte)daqList[n].ODTs[j].ODTEntries.Count;//ODT_Entries_Count
                    if (SendCMD(sendData, out res, canIndex) != XCPResponse.Ok)
                        throw new XCPException($"分配 DAQ {n} ODT {j} (D3) 失败:" + XCPHelper.ParseErrCode(res[1]));
                }
            }

            //-为ODT填入参数地址和长度（0xE2，0xE1）
            for (short n = DAQProperty.Min_DAQ; n < DAQProperty.Min_DAQ + daqCount; n++)
            {
                for (byte j = 0; j < daqList[n].ODTs.Count; j++)
                {
                    sendData[0] = 0xE2;
                    sendData[1] = 0x00;
                    number = BitConverterExt.GetBytes(n, false);            //DAQ List Number
                    sendData[2] = number[0];
                    sendData[3] = number[1];
                    sendData[4] = j;                                        //ODT_Number
                    sendData[5] = 0x00;                                     //ODT_Entry_Number
                    if (SendCMD(sendData, out res, canIndex) != XCPResponse.Ok)
                        throw new XCPException($"分配 DAQ {n} ODT {j} 地址 (E2) 失败:" + XCPHelper.ParseErrCode(res[1]));

                    for (byte i = 0; i < daqList[n].ODTs[j].ODTEntries.Count; i++)
                    {
                        sendData[0] = 0xE1;
                        sendData[1] = 0xFF;
                        sendData[2] = daqList[n].ODTs[j].ODTEntries[i].Size;
                        sendData[3] = daqList[n].ODTs[j].ODTEntries[i].AddressExtension;
                        var signalAddr = BitConverterExt.GetBytes(daqList[n].ODTs[j].ODTEntries[i].Address,false);
                        sendData[4] = signalAddr[0];
                        sendData[5] = signalAddr[1];
                        sendData[6] = signalAddr[2];
                        sendData[7] = signalAddr[3];
                        if (SendCMD(sendData, out res, canIndex) != XCPResponse.Ok)
                            throw new XCPException($"分配 DAQ {n} ODT {j} 地址 (E1) 失败:" + XCPHelper.ParseErrCode(res[1]));
                    }
                }
            }
            //-设置每个DAQ的模式（0xE0）,-选中DAQ（0xDE）
            for (short i = 0; i < daqList.Count; i++)
            {
                sendData[0] = 0xE0;
                sendData[1] = 0x10;//Mode=>direction=daq,timestamped
                var daqlistnumber = BitConverterExt.GetBytes(i, (int)byteOrder);
                sendData[2] = daqlistnumber[0]; 
                sendData[3] = daqlistnumber[1]; 
                var eventChannelNumber = BitConverterExt.GetBytes(daqList[i].Event_Channel_Number, (int)byteOrder);
                sendData[4] = eventChannelNumber[0];
                sendData[5] = eventChannelNumber[1];
                sendData[6] = 0x01;// transmission rate prescaler >= 1
                sendData[7] = 0x01;// DAQ list Priority
                if (SendCMD(sendData, out res, canIndex) != XCPResponse.Ok)
                    throw new XCPException($"设置每个DAQ的模式（0xE0） 失败:" + XCPHelper.ParseErrCode(res[1]));

                sendData[0] = 0xDE;
                sendData[1] = 0x02;//mode => select
                var daqListNumber = BitConverterExt.GetBytes(i, (int)byteOrder);
                sendData[2] = daqListNumber[0];
                sendData[3] = daqListNumber[1];
                if (SendCMD(sendData, out res, canIndex) != XCPResponse.Ok)
                    throw new XCPException($"选中DAQ（0xDE） 失败:" + XCPHelper.ParseErrCode(res[1]));
                else
                {
                    daqList[i].ODTs[0].ID = res[1];
                }
            }

            //-获取DAQ Clock（0xDC）
            sendData[0] = 0xDC;
            if (SendCMD(sendData, out _, canIndex) != XCPResponse.Ok)
                return false;

            return true;
        }

        /// <summary>
        /// Mode：
        /// 00=stop all；
        /// 01=start selected；
        /// 02=stop selected；
        /// </summary>
        /// <param name="canIndex"></param>
        /// <returns></returns>
        public bool StartStopDAQ(byte mode, uint canIndex)
        {
            byte[] sendData = new byte[] { 0xDD, mode };
            //sendData[1] = 0x01;mode =
            return SendCMD(sendData, out _, canIndex) == XCPResponse.Ok;
        }

        /// <summary>
        /// 发送命令，接收多条数据
        /// </summary>
        /// <param name="data">发送数据</param>
        /// <param name="resData">接收的数据</param>
        /// <param name="canIndex">调用的CAN通道号</param>
        /// <param name="receiveCount">接收次数</param>
        /// <returns></returns>
        private XCPResponse SendLongCMD(byte[] data, out List<byte[]> resData, uint canIndex, uint receiveCount)
        {
            resData = new List<byte[]>();

            lock (locker)
            {
                try
                {
                    if (!receiveRunning)
                        this.startReceiveThread(canIndex);

                    USBCanManager.Instance.Send(projectItem, (int)canIndex, new CANSendFrame((int)masterid, data),"XCPInfo");

                    //等待
                    if (ReceiveEvent.WaitOne(1000))
                    {
                        resData.Add(receiveData.Dequeue().b);
                        return XCPHelper.TransformBytetoRes(resData[0][0]);
                    }
                    else
                    {
                        return XCPResponse.Out_time;
                    }

                }
                catch (USBCANOpenException canEx)
                {
                    this.ConnectStatus = XCPConnectStatus.ConnectFail;
                    throw canEx;
                }
                catch (Exception ex)
                {
                    LogHelper.Error("SendLongCMD", ex);
                    return XCPResponse.Out_time;
                }
                finally
                {
                    receiveRunning = false;
                }
            }
        }

        private void Receive(object canindex)
        {
            int canIndex = Convert.ToInt32(canindex);
            do
            {
                try
                {
                    var data = USBCanManager.Instance.Receive(projectItem, ids: new int[] { (int)this.slaveid }, canIndex);
                    for (uint i = 0; i < data.Length; ++i)
                    {
                        var can = data[i];
                        uint id = (uint)data[i].cid;

                        ///判断回应
                        if (GetId(id) == slaveid)
                        {
                            receiveData.Enqueue(can);
                            ReceiveEvent.Set();
                        }
                    }
                }
                catch (USBCANOpenException usncanEx)
                {
                    LogHelper.Error("XCPModule.Recieve：Usb CAN　Error", usncanEx);
                    receiveRunning = false;
                }
                catch (Exception ex)
                {
                    LogHelper.Error("XCPModule.Recieve", ex);
                }

            } while (receiveRunning);
        }

        private void OnDataRecieveEvent(object sender, CANDataRecieveEventArgs args)
        {
            try
            {
                for (uint i = 0; i < args.can_msgs.Length; ++i)
                {
                    var can = args.can_msgs[i];
                    uint id = (uint)args.can_msgs[i].cid;

                    ///判断回应
                    if (GetId(id) == slaveid)
                    {
                        if (!XCPHelper.CTOCheck(can.b[0]))
                            continue;
                        receiveData.Enqueue(can);
                        ReceiveEvent.Set();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("XCPModule.OnDataRecieveEvent", ex);
            }
        }

        private void OnDaqDataRecieveEvent(object sender, CANDataRecieveEventArgs args)
        {

        }

        private uint GetId(uint id)
        {
            return id & 0x1FFFFFFFU;
        }
    }

    public class XCPException : Exception
    {
        public XCPException(string msg) : base(msg) { }
    }

    public class DAQList
    {
        public List<DAQ> DAQs { get; private set; } = new List<DAQ>();

        public byte ODTCount { get 
            {
                byte count = 0;

                if(DAQs.Count >0)
                {
                    foreach (var daq in DAQs)
                    {
                        if (daq.ODTs != null && daq.ODTs.Count > 0)
                            count = (byte)(daq.ODTs.Count + count);
                    }
                }

                return count;
            } 
        }

        public void Clear()
        {
            DAQs.Clear();
        }

        public void AddSignal(XCPSignal signal)
        {
            signal.StartIndex = 0;

            DAQ dAQ = DAQs.Find(x => x.EventName == signal.EventName);// new DAQ();
            if (dAQ == null)
            {
                dAQ = new DAQ
                {
                    EventName = signal.EventName,
                    Event_Channel_Number = (short)signal.EventID
                };
                DAQs.Add(dAQ);
            }

            if (dAQ.ODTs == null)
                dAQ.ODTs = new List<ODT>();

            ODT oDT;
            if (dAQ.ODTs.Count == 0)
            {
                oDT = new ODT
                {
                    MaxSize = 5,
                    ID = ODTCount
                };
                dAQ.ODTs.Add(oDT);
                signal.StartIndex = 0;
            }
            else
            {
                oDT = dAQ.ODTs[dAQ.ODTs.Count - 1];
                foreach (var item in dAQ.ODTs)
                {
                    signal.StartIndex += item.UsedSize;
                }
            }

            if (oDT.ODTEntries == null)
                oDT.ODTEntries = new List<ODTEntry>();

            ODTEntry oDTEntry = new ODTEntry();
            int.TryParse(signal.ECUAddress, System.Globalization.NumberStyles.HexNumber, null, out int Address);

            if (oDT.AvailableSize >= (byte)signal.Length)
            {
                oDTEntry.Size = (byte)signal.Length;
                oDTEntry.AddressExtension = (byte)signal.AddressExtension;
                oDTEntry.Address = Address;
                oDT.ODTEntries.Add(oDTEntry);
            }
            else//剩余长度不够一个信号长度，新建一个ODT
            {
                if (oDT.AvailableSize > 0)//可用的长度>0
                {
                    oDTEntry.Size = oDT.AvailableSize;
                    oDTEntry.AddressExtension = (byte)signal.AddressExtension;
                    oDTEntry.Address = Address;
                    oDT.ODTEntries.Add(oDTEntry);
                }

                //新建一个ODT
                ODT oDT1 = new ODT
                {
                    ID = ODTCount,
                    ODTEntries = new List<ODTEntry>()
                };
                dAQ.ODTs.Add(oDT1);
                ODTEntry oDTEntry1 = new ODTEntry
                {
                    Size = (byte)(signal.Length - oDTEntry.Size),
                    AddressExtension = (byte)signal.AddressExtension,
                    Address = Address + oDTEntry.Size
                };
                oDT1.ODTEntries.Add(oDTEntry1);
            }
        }
    }

    public class DAQ
    {
        public List<ODT> ODTs { get; set; }

        public short Event_Channel_Number { get; set; }

        public string EventName { get; set; }
    }

    public class ODT
    {
        public byte MaxSize { get; set; } = 7;

        /// <summary>
        /// 可用长度
        /// </summary>
        public byte AvailableSize
        {
            get
            {
                byte availableSize = MaxSize;
                if (ODTEntries != null && ODTEntries.Count >= 0)
                {
                    foreach (var item in ODTEntries)
                    {
                        availableSize -= item.Size;
                    }
                }
                return availableSize;
            }
        }
        /// <summary>
        /// 已使用的地址
        /// </summary>
        public byte UsedSize
        {
            get
            {
                byte usedSize = 0;
                if (ODTEntries != null && ODTEntries.Count >= 0)
                {
                    foreach (var item in ODTEntries)
                    {
                        usedSize += item.Size;
                    }
                }
                return usedSize;
            }
        }
        /// <summary>
        /// 序号
        /// </summary>
        public byte ID { get; set; }
        public List<ODTEntry> ODTEntries { get; set; }
    }

    public class ODTEntry
    {
        public byte Size {  set; get; }

        public byte AddressExtension {  set; get; }

        public int Address {  set; get; }
    }

    public enum XCPValueType
    {
        BOOLEAN,
        //UBYTE,
        UBYTE,//一字节无符号整型
        BYTE,//一字节有符号
        UWORD,//2字节无符号整型
        SWORD,//2字节有符号整型
        ULONG,//4字节无符号
        LONG,//四字节有符号
        FLOAT32,//4字节浮点型
        FLOAT64,//8字节浮点型
        Lookup1D_BOOLEAN,
    }

    public enum XCPResponse
    {
        Ok = 0xff,
        Err = 0xFE,
        EV = 0xfd,
        serv = 0xFC,
        Undefined = -2,
        Out_time,
    }

    public enum XCPCMDStatus
    {
        Upload,
        UploadSucc,
        UploadFail,
        ShortUpload,
        ShortUploadSucc,
        ShortUploadFail,
        Set_MTAFail,
        GetSeed,
        GetSeedFail,
        SendKey,
        SendKeyFail,
        UnlockSucc,
        DownLoadSucc,
        DownLoadFail,
        DownLoad
    }

    public enum XCPConnectStatus
    {
        Init,
        Connecting,
        ConnectFail,
        Connected,
        DisConnect,
    }

    public enum XCPMode
    {
        Polling,
        Block
    }

    public struct CCP_CHARACTERISTIC
    {
        public string cName;
        public string LongIdentifier;
        public string Characteristic_Type;
        public UInt64 cAddress;
        public string Record_Layout;
        public CCP_COMPU_METHOD Conversion_Method;
        public double Lower_Limit;
        public double Upper_Limit;
        public UInt64 cValue;
        public bool ValueValid;
        public byte DataLen;
        public string ValueDisp;
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public string[] Databuf_Y_disp;
        public CCP_AXIS_DESCR AXIS_DESCR_X;
        public CCP_AXIS_DESCR AXIS_DESCR_Y;
    }

    public struct CCP_MEASUREMENT
    {
        public string cName;
        public string LongIdentifier;
        public string Data_Type;
        public CCP_COMPU_METHOD Conversion_Method;
        public double Lower_Limit;
        public double Upper_Limit;
        public UInt64 cAddress;

        public UInt64 cValue;
        public bool ValueValid;
        public byte DataLen;
        public string ValueDisp;
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public string[] Databuf_Y_disp;
    }

    public struct CCP_AXIS_DESCR
    {
        public string Axis_Type;
        public string Reference_to_Input;
        public string Conversion_Method;
        public int Number_of_Axis_Pts;
        public string Lower_Limit;
        public string Upper_Limit;
        public string AXIS_PTS_REF;
        public CCP_AXIS_PTS AXIS_PTS;
        public string[] Databuf_X_disp;
    }

    public struct CCP_AXIS_PTS
    {
        public string Name;
        public string LongIdentifier;
        public UInt64 ECU_Address;
        public string InputQuantity;
        public string Record_Layout;
        public CCP_COMPU_METHOD Conversion_Method;
        public int Number_of_Axis_Pts;
        public string Lower_Limit;
        public string Upper_Limit;
        public byte DataLen;
    }


    public struct CCP_COMPU_METHOD
    {
        public string NameOfCompuMethod;
        public string LongIdentifier;
        public string Conversion_Type;
        public string Format;
        public string Units;
        public string Coefficients;
    }
}
