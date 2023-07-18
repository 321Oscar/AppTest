using AppTest.FormType;
using AppTest.Model;
using LPCanControl.CANInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaudRateType = AppTest.FormType.BaudRateType;

namespace AppTest.Helper
{
    public sealed class USBCanManager
    {
        private static readonly Lazy<USBCanManager> lazy = new Lazy<USBCanManager>(() => new USBCanManager());

        Dictionary<BaudRateType, string> BaudRateTypeValue_Special = new Dictionary<BaudRateType, string>()
        {
            { BaudRateType.Kbps5, "0x1c01c1" },
            { BaudRateType.Kbps1000,"0x060003"},
            { BaudRateType.Kbps800,"0x060004"},
            { BaudRateType.Kbps500,"0x060007"},
            { BaudRateType.Kbps250,"0x1c0008"},
            { BaudRateType.Kbps125,"0x1c0011"},
            { BaudRateType.Kbps100,"0x160023"},
            { BaudRateType.Kbps50,"0x1c002c"},
            { BaudRateType.Kbps20,"0x1600b3"},
            { BaudRateType.Kbps10,"0x1c00e0"},
        };

        Dictionary<BaudRateType, string> BaudRateTypeValue_Normal_Timer0 = new Dictionary<BaudRateType, string>()
        {
            { BaudRateType.Kbps5,"0xBF"},
            { BaudRateType.Kbps10,"0x31"},
            { BaudRateType.Kbps20,"0x18"},
            //{ BaudRateType.Kbps40,"0x87"},
            { BaudRateType.Kbps50,"0x09"},
            { BaudRateType.Kbps100,"0x04"},
            { BaudRateType.Kbps125,"0x03"},
            { BaudRateType.Kbps250,"0x01"},
            { BaudRateType.Kbps500,"0x00"},
            { BaudRateType.Kbps800,"0x00"},
            { BaudRateType.Kbps1000,"0x00"},

        };

        Dictionary<BaudRateType, string> BaudRateTypeValue_Normal_Timer1 = new Dictionary<BaudRateType, string>()
        {
            { BaudRateType.Kbps5,"0xFF"},
            { BaudRateType.Kbps10,"0x1C"},
            { BaudRateType.Kbps20,"0x1C"},
            //{ BaudRateType.Kbps40,"0x87"},
            { BaudRateType.Kbps50,"0x1C"},
            { BaudRateType.Kbps100,"0x1C"},
            { BaudRateType.Kbps125,"0x1C"},
            { BaudRateType.Kbps250,"0x1C"},
            { BaudRateType.Kbps500,"0x1C"},
            { BaudRateType.Kbps800,"0x16"},
            { BaudRateType.Kbps1000,"0x14"}

        };

        private readonly Dictionary<ProjectItem, USBCAN> usbCans;
        private readonly Dictionary<ProjectItem, ZCANInstance> zCANInstances;
        private readonly Dictionary<ProjectItem, ICAN> CANs;
        public Dictionary<ProjectItem, USBCAN> USBCans { get { return usbCans; } }
        public Dictionary<ProjectItem, ZCANInstance> ZCans { get { return zCANInstances; } }
        public static USBCanManager Instance { get { return lazy.Value; } }

        private USBCanManager()
        {
            usbCans = new Dictionary<ProjectItem, USBCAN>();
            zCANInstances = new Dictionary<ProjectItem, ZCANInstance>();
            CANs = new Dictionary<ProjectItem, ICAN>();
        }

        public bool Exist(ProjectItem project)
        {
            bool exist = CANs.TryGetValue(project, out _);
            if (exist)
                return true;
            return false;
        }

        public void AddICAN(ProjectItem project, ICAN can)
        {
            CANs.Add(project, can);
        }
        /// <summary>
        /// 注册/注销 接收事件
        /// </summary>
        /// <param name="project"></param>
        /// <param name="recieveMethod"></param>
        /// <param name="canChannel"></param>
        /// <param name="get"></param>
        public void Register(ProjectItem project, ReceiveDataEventHandler recieveMethod, int canChannel, bool get)
        {
            if (get)
            {
                CANs[project].Register(recieveMethod, canChannel);
            }
            else
            {
                CANs[project].UnRegister(recieveMethod, canChannel);
            }
        }

        /// <summary>
        /// 删除CAN
        /// </summary>
        /// <param name="project"></param>
        /// <param name="isOpen">若CAN打开，则先关闭再删除</param>
        public void RemoveUsbCan(ProjectItem project, bool isOpen = true)
        {
            bool isExist = CANs.TryGetValue(project, out ICAN usbCan);
            if (isExist)
            {
                if (isOpen)
                {
                    try
                    {
                        usbCan.Close();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error("关闭失败", ex);
                    }
                }
                CANs.Remove(project);
            }
        }
        /// <summary>
        /// 打开CAN盒
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public bool Open(ProjectItem project)
        {
            bool isExist = CANs.TryGetValue(project, out ICAN usbCan);
            if (isExist)
            {
                return (usbCan.Open());

            }
            return false;
        }
        /// <summary>
        /// 打开CAN并初始化通道，启动通道
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public bool OpenInitStart(ProjectItem project)
        {
            return CANs[project].OpenInitStart(null);
        }
        /// <summary>
        /// 打开CAN并初始化通道，启动通道，并开始接收数据线程
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public bool OpenInitStartReceive(ProjectItem project)
        {
            return CANs[project].OpenInitStartReceive(project.CanIndex.Select(x => x.CanChannel).ToArray());
        }

        /// <summary>
        /// 启动接收数据
        /// </summary>
        /// <param name="project"></param>
        /// <param name="canind"></param>
        public void StartRecv(ProjectItem project, int[] canind)
        {
            bool isExist = CANs.TryGetValue(project, out ICAN usbCan);
            if (isExist)
            {
                usbCan.StartReceive(true, canind);
            }
            else
            {

            }
        }

        /// <summary>
        /// 关闭接收数据
        /// </summary>
        /// <param name="project"></param>
        public void CloseRecv(ProjectItem project)
        {
            bool isExist = CANs.TryGetValue(project, out ICAN usbCan);
            if (isExist)
            {
                usbCan.StartReceive(false, new int[] { 0, 1 });
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="project">根据Project获取CanDevice</param>
        /// <param name="canindex">发送的CAN通道</param>
        /// <param name="sendData">数据</param>
        /// <param name="formName">发送的窗口</param>
        /// <exception cref="USBCANOpenException"></exception>
        /// <returns>发送成功/失败<see cref="bool"/></returns>
        internal bool Send(ProjectItem project, int canindex = 0, CANSendFrame sendData = null, string formName = null, byte sendtype = 0)
        {
            bool isadd = CANs.TryGetValue(project, out ICAN usbCan);

            if (!isadd)
            {
                throw new USBCANOpenException($"{project.Name},Can口未打开");
            }

            if (!usbCan.IsOpen)
                throw new USBCANOpenException($"{project.Name},Can口为关闭状态");

            bool sendFlag = usbCan.Send(canindex, sendData, (LPCanControl.CANInfo.CANSendType)sendtype);

            LogHelper.InfoSend($"{formName} 发送{(sendFlag ? "成功" : "失败")} [通道{canindex}]：{sendData}");
            

            return sendFlag;
        }

        /// <summary>
        /// 测试CAN是否通
        /// </summary>
        /// <param name="project"></param>
        /// <param name="errorLog"></param>
        /// <param name="canindex"></param>
        /// <param name="sendtype"></param>
        /// <returns></returns>
        internal bool SendTest(ProjectItem project, ref string errorLog, int canindex = 0, byte sendtype = 0)
        {
            bool isadd = CANs.TryGetValue(project, out ICAN usbCan);

            if (!isadd)
            {
                errorLog = ($"{project.Name},Can口未打开");
                return false;

            }

            if (!usbCan.IsOpen)
            {
                errorLog = ($"{project.Name},Can口为关闭状态");
                return false;
            }

            var sendData = new CANSendFrame(0x11, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });

            bool sendFlag = usbCan.Send(canindex, sendData, (LPCanControl.CANInfo.CANSendType)sendtype);

            if (!sendFlag)
                errorLog = $"CAN index {canindex} 发送失败！";

            return sendFlag;
        }

        internal bool Close(ProjectItem project)
        {
            bool isExist = CANs.TryGetValue(project, out ICAN usbCan);
            if (isExist)
            {
                CloseRecv(project);

                return usbCan.Close();
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 关闭所有Can口
        /// </summary>
        internal void Close()
        {
            try
            {
                foreach (var item in CANs)
                {
                    if (!item.Value.Close())
                    {
                        LogHelper.Warn($"关闭失败：{item.Key.Name}");
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("关闭失败", ex);
            }

        }

        #region [Obsolete]

        [Obsolete]
        public bool ExistZcan(ProjectItem project)
        {
            return zCANInstances.TryGetValue(project, out _);
        }
        [Obsolete]
        public void AddUsbCan(USBCAN usbCan, ProjectItem project)
        {
            usbCans.Add(project, usbCan);
        }
        [Obsolete]
        public void AddZCan(ProjectItem project, ZCANInstance zCAN)
        {
            zCANInstances.Add(project, zCAN);
        }
        [Obsolete]
        public bool Exist_old(ProjectItem project)
        {
            bool exist = usbCans.TryGetValue(project, out _);
            if (exist)
                return true;
            else
            {
                return ExistZcan(project);
            }

        }
        [Obsolete]
        public void RemoveUsbCan_old(ProjectItem project, bool isOpen = true)
        {
            bool isExist = usbCans.TryGetValue(project, out USBCAN usbCan);
            if (isExist)
            {
                if (isOpen)
                {
                    try
                    {
                        usbCan.Close_device();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error("关闭失败", ex);
                    }
                }
                usbCans.Remove(project);
            }
            else
            {
                isExist = ZCans.TryGetValue(project, out ZCANInstance zCAN);
                if (isExist)
                {
                    if (isOpen)
                    {
                        try
                        {
                            zCAN.CloseCan();
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error("关闭失败", ex);
                        }
                    }
                    ZCans.Remove(project);
                }

            }
        }
        [Obsolete]
        public bool Open_old(ProjectItem project)
        {
            bool isExist = usbCans.TryGetValue(project, out USBCAN usbCan);
            if (isExist)
            {
                if (usbCan.Open_Device() == 0)
                {
                    return false;
                }

            }
            else
            {
                isExist = ZCans.TryGetValue(project, out ZCANInstance zCAN);
                if (isExist)
                {
                    return zCAN.OpenDevice();
                }
                return false;
            }
            return true;
        }


        [Obsolete]
        public void StartRecv_old(ProjectItem project, int[] canind)
        {
            bool isExist = usbCans.TryGetValue(project, out USBCAN usbCan);
            if (isExist)
            {
                usbCan.StartReceive(true, canind);
            }
            else
            {
                ZcanStartReceive(project, canind);
            }
        }
        [Obsolete]
        public void ZcanStartReceive(ProjectItem project, int[] canind)
        {
            bool isExist = ZCans.TryGetValue(project, out ZCANInstance usbCan);
            if (isExist)
            {
                usbCan.StartReceive(true, canind);
            }
        }
        [Obsolete]
        public bool OpenZCan(ProjectItem project)
        {
            bool isExist = ZCans.TryGetValue(project, out ZCANInstance usbCan);
            if (isExist)
            {
                if (!usbCan.OpenDevice())
                {
                    //usbCan.IsOpen = false;
                    return false;
                }
                else
                {
                    //usbCan.IsOpen = true;
                }
            }
            else
            {
                return false;
            }
            return true;
        }
        [Obsolete]
        public void CloseRecv_old(ProjectItem project)
        {
            bool isExist = usbCans.TryGetValue(project, out USBCAN usbCan);
            if (isExist)
            {
                usbCan.StartReceive(false, new int[] { 0, 1 });
            }
            else
            {
                ZcanCloseReceive(project);
            }
        }
        [Obsolete]
        public void ZcanCloseReceive(ProjectItem project)
        {
            bool isExist = ZCans.TryGetValue(project, out ZCANInstance usbCan);
            if (isExist)
            {
                usbCan.StartReceive(false, new int[] { 0 });
            }
        }
        [Obsolete]
        public bool InitCan(ProjectItem project, int can_index, uint startid = 0x00, uint endid = 0xffffffff)
        {
            bool isExist = usbCans.TryGetValue(project, out USBCAN usbCan);
            if (isExist)
            {
                var canIdx = project.CanIndex.Find(x => x.CanChannel == can_index);
                bool canGetBaud = BaudRateTypeValue_Normal_Timer0.TryGetValue((BaudRateType)canIdx.BaudRate, out string baudStr0);
                bool canGetBaud1 = BaudRateTypeValue_Normal_Timer1.TryGetValue((BaudRateType)canIdx.BaudRate, out string baudStr1);
                if (canGetBaud && canGetBaud1)
                {
                    int timer0 = Convert.ToInt32(baudStr0, 16);
                    int timer1 = Convert.ToInt32(baudStr1, 16);

                    if (usbCan.InitCan((byte)0, can_index, (byte)timer0, Timing1: (byte)timer1) == 0)
                        return false;

                }
                else
                {
                    throw new Exception("未知波特率，波特率转换错误");
                }
            }
            else
            {
                return InitZCan(project, can_index, startid, endid);
            }

            return true;
        }
        [Obsolete]
        public bool InitZCan(ProjectItem project, int can_index, uint startid = 0x00, uint endid = 0xFFFFFFFF)
        {
            bool isExist = ZCans.TryGetValue(project, out ZCANInstance usbCan);
            if (!isExist)
                return isExist;
            uint baud;
            var canIdx = project.CanIndex.Find(x => x.CanChannel == can_index);
            switch ((BaudRateType)canIdx.BaudRate)
            {
                case BaudRateType.Kbps500:
                    baud = 500000;
                    break;
                default:
                    baud = 500000;
                    break;
            }
            usbCan.StartID = startid;
            usbCan.EndID = endid;

            return usbCan.InitCan(baud, can_index);
        }
        [Obsolete]
        public bool StartCanIndex(ProjectItem project, int can_index)
        {
            bool isExist = usbCans.TryGetValue(project, out USBCAN usbCan);
            if (isExist)
            {
                if (usbCan.Start_can(can_index) == 0)
                    return false;
            }
            else
            {
                return StartZCanIndex(project, can_index);
            }
            return true;
        }
        [Obsolete]
        public bool StartZCanIndex(ProjectItem project, int can_index)
        {
            bool isExist = ZCans.TryGetValue(project, out ZCANInstance usbCan);
            if (isExist)
            {
                if (!usbCan.StartCan(can_index))
                    return false;
            }
            else
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 设置CAN通道波特率
        /// </summary>
        /// <param name="project"></param>
        /// <param name="can_index"></param>
        /// <returns></returns>
        [Obsolete]
        unsafe public bool SetRefenrece(ProjectItem project, int can_index)
        {
            bool isExist = usbCans.TryGetValue(project, out USBCAN usbCan);
            var canIdx = project.CanIndex.Find(x => x.CanChannel == can_index);
            bool canGetBaud = BaudRateTypeValue_Special.TryGetValue((BaudRateType)canIdx.BaudRate, out string baudStr);
            uint baud;
            if (canGetBaud)
            {
                baud = Convert.ToUInt32(baudStr, 16);
            }
            else
            {
                throw new Exception("未知波特率");
            }
            if (isExist)
            {
                return usbCan.SetReference(baud, can_index);
            }
            else
            {
                return false;
            }
        }
        [Obsolete]
        internal bool Send_old(ProjectItem project, int canindex = 0, CANSendFrame sendData = null, string formName = null, byte sendtype = 0)
        {
            bool isadd = usbCans.TryGetValue(project, out USBCAN usbCan);

            if (!isadd)
            {
                isadd = ZCans.TryGetValue(project, out ZCANInstance zCAN);
                if (!isadd)
                    throw new USBCANOpenException($"{project.Name},Can口未打开");
                if (!zCAN.IsOpen)
                    throw new USBCANOpenException($"{project.Name},Can口为关闭状态");

                bool sendSuc = zCAN.TransmitData((uint)sendData.ID, sendData.Data, sendtype, canindex, 0);

                LogHelper.InfoSend($"{formName} 发送{(sendSuc ? "成功" : "失败")} [通道{canindex}]：{sendData}");

                if (!sendSuc)
                    throw new USBCANOpenException("发送失败！");
                return sendSuc;
            }

            if (!usbCan.IsOpen)
                throw new USBCANOpenException($"{project.Name},Can口为关闭状态");

            bool sendFlag = usbCan.CAN_Send(sendData, 0, canindex, sendtype) > 0;

            LogHelper.InfoSend($"{formName} 发送{(sendFlag ? "成功" : "失败")} [通道{canindex}]：{sendData}");

            if (!sendFlag)
                return false;
            //throw new USBCANOpenException($"CanIndex【{canindex}】发送失败！");
            return sendFlag;
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="project">can信息</param>
        /// <param name="canindex">can口通道索引：默认为0</param>
        /// <param name="formName">接收的窗口</param>
        /// <returns><see cref="CAN_msg"/>[]</returns>
        [Obsolete]
        public CANReceiveFrame[] Receive(ProjectItem project, int canindex = 0, string formName = "recieve")
        {
            bool isadd = usbCans.TryGetValue(project, out USBCAN usbCan);

            if (!isadd)
                throw new USBCANOpenException($"{DateTime.Now}  {project.Name},Can口未打开");

            if (!usbCan.IsOpen)
                throw new USBCANOpenException($"{DateTime.Now}  {project.Name},Can口为关闭状态");

            var data = usbCan.CAN_Receive(canindex);
            for (int i = 0; i < data.Length; i++)
            {
                string log = data[i].ToString();
                LogHelper.WriteToOutput(formName, $"通道[{canindex}] [{data[i].TimeStampStr:yyyy-MM-dd HH:mm:ss fff}] 接收：" + log);
            }

            return data;
        }

        /// <summary>
        /// 接收特定ID数据
        /// </summary>
        /// <param name="project">can信息</param>
        /// <param name="ids">ID</param>
        /// <param name="canindex">can口通道索引：默认为0</param>
        /// <param name="formName">接收的窗口</param>
        /// <returns><see cref="CAN_msg"/>[]</returns>
        [Obsolete]
        public CANReceiveFrame[] Receive(ProjectItem project, int[] ids, int canindex = 0, string formName = "recieve")
        {
            bool isadd = usbCans.TryGetValue(project, out USBCAN usbCan);

            if (!isadd)
                throw new USBCANOpenException($"{project.Name},Can口未打开");

            if (!usbCan.IsOpen)
                throw new USBCANOpenException($"{project.Name},Can口为关闭状态");

            var data = usbCan.Receive(canindex, ids);
            if (data == null)
                return null;
            for (int i = 0; i < data.Length; i++)
            {
                string log = data[i].ToString();
                LogHelper.WriteToOutput(formName, $"通道[{canindex}] [{data[i].TimeStampStr:yyyy-MM-dd HH:mm:ss fff}] 接收：" + log);
            }

            return data;
        }

        [Obsolete]
        internal void Close_old()
        {
            try
            {
                foreach (var item in usbCans)
                {
                    if (!item.Value.Close_device())
                    {
                        LogHelper.Warn($"关闭失败：{item.Key.Name}");
                    }
                }

                foreach (var item in ZCans)
                {
                    if (!item.Value.CloseCan())
                    {
                        LogHelper.Warn($"关闭失败：{item.Key.Name}");
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("关闭失败", ex);
            }

        }

        internal bool Close_old(ProjectItem project)
        {
            bool isExist = usbCans.TryGetValue(project, out USBCAN usbCan);
            if (isExist)
            {
                CloseRecv(project);

                return usbCan.Close_device();
            }
            else
            {
                isExist = ZCans.TryGetValue(project, out ZCANInstance zcan);
                if (isExist)
                {
                    CloseRecv(project);

                    return zcan.CloseCan();
                }
                return false;
            }
        }
        #endregion [Obsolete]
    }
}
