using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppTest.ProjectClass
{
    public static class CCP
    {
        static CCP()
        {
            _ctr = 0;
            MCUAndPCLinkCPPStateE = CPPOrderStateEnum.NullCCPOrderState;
            AutoResetEvent = new AutoResetEvent(true);
            PollingPath = null;
            DAQPath = null;
        }

        #region Private var
        /// <summary>
        /// 用于存储地址
        /// </summary>
        private static int address = 0;//地址大小
        /// <summary>
        /// 用于CCP通信延迟计数
        /// </summary>
        private static int count = 0;

        /// <summary>
        /// 用于存储解密数据
        /// </summary>
        private static byte[] pSeed = new byte[4];

        /// <summary>
        /// 用于存储上传或是下传的数据
        /// </summary>
        private static byte[] bytedata = new byte[5];
        /// <summary>
        /// 存储上传数据临时数组
        /// </summary>
        private static byte[] TempBuf;

        public static AnalysisVarStruct AnalysisVarS = new AnalysisVarStruct();

        #endregion

        #region AutoResetEvent
        /// <summary>
        /// 线程同步，并将起状态设置为有信号（每一次值允许一个线程工作，每次开线程前都要Set一下，）
        /// </summary>
        public static AutoResetEvent AutoResetEvent;
        #endregion

        #region 定义命令序号Ctr属性
        /// <summary>
        /// 命令序号 _ctr字段
        /// </summary>
        private static byte _ctr;

        /// <summary>
        /// 命令序号Ctr属性
        /// </summary>
        public static byte Ctr
        {
            get
            {
                if (_ctr >= 255)
                    _ctr = 0;
                return _ctr;
            }
            set
            {
                _ctr = value;
                if(_ctr >= 255)
                    _ctr = 0;
            }
        }
        #endregion

        #region SeedKey状态标志
        /// <summary>
        /// 打开Polling功能的DLL路径
        /// </summary>
        public static string PollingPath;

        /// <summary>
        /// 打开DAQ功能的dll路径
        /// </summary>
        public static string DAQPath;
        #endregion

        #region CCP枚举状态
        #region CCP指令枚举状态
        public enum CPPOrderStateEnum
        {
            #region MCU和PC连接功能枚举状态
            Send0x01CCPOrderState = 1,
            Send0x01CCPOrderStateSuccess = 2,

            Send0x12CCPOrderPollingState = 3,
            Send0x12CCPOrderPollingSuccess = 4,

            Send0x13CCPOrderPollingState = 5,
            Send0x13CCPOrderPollingSuccess = 6,

            Send0x12CCPOrderDAQState = 7,
            Send0x12CCPOrderDAQStateSuccess = 8,

            Send0x13CCPOrderDAQState = 9,
            Send0x13CCPOrderDAQStateSuccess = 10,

            Send0x14CCPOrderOneState = 11,
            Send0x14CCPOrderOneStateSuccess = 12,

            Send0x14CCPOrderTwoState = 13,
            Send0x14CCPOrderTwoStateSuccess = 14,

            MCULinkPCStateSuccess = 15,
            #endregion

            #region Polling模式枚举状态（采集量上传数据枚举状态；标定量下发数据枚举状态）
            Send0x0FCCPOrderState  =23,
            Send0x0FCCPOrderStateSuccess = 24,
            CCPUpDataStateSuccess = 25,

            Send0x02CCPOrderState = 26,
            Send0x02CPPOrderStateSuccess = 27,
            Send0x03CCPOrderState =28,
            Send0x03CPPOrderStateSuccess = 29,

            CCPDownDataStateSuccess = 30,
            #endregion

            #region 没有发送任何CPP指令（空状态）
            NullCCPOrderState = 100,
            #endregion
        }
        #endregion

        #region MCU和PC连接CCP指令枚举状态
        /// <summary>
        /// MCU 和PC连接CCP指令枚举状态
        /// </summary>
        private static CPPOrderStateEnum MCUAndPCLinkCPPStateE;
        #endregion

        #region 下传数据CCP指令枚举状态
        /// <summary>
        /// 下传数据CCP指令枚举状态
        /// </summary>
        private static CPPOrderStateEnum DownDataCCPStateE;
        #endregion

        #region 上传数据CCP指令枚举状态
        /// <summary>
        /// 上传数据CCP指令枚举状态
        /// </summary>
        private static CPPOrderStateEnum UpDataCCPStateE;
        #endregion

        #endregion

        #region 定义存储A2L文件中的变量，算法集合的变量
        public struct VariableInformationStruct
        {
            /// <summary>
            /// 采集变量的名字
            /// </summary>
            public string Name;
            /// <summary>
            /// 采集变量的地址
            /// </summary>
            public string Address;
            /// <summary>
            /// 采集变量的地址类型
            /// </summary>
            public string Address_Type;
            /// <summary>
            /// 采集变量的数据类型
            /// </summary>
            public string Type;
            /// <summary>
            /// 采集变量的数据类型的大小
            /// </summary>
            public string Size;
            /// <summary>
            /// 采集变量的算法
            /// </summary>
            public string Conversion;
           /// <summary>
           /// 数组变量的数组大小（如果是数组类型的变量）
           /// </summary>
            public string Array_Size;
            /// <summary>
            /// 变量类型的名字
            /// </summary>
            public string Variable_Type_Name;
        }
        #endregion

        #region 将A2L文件中变量的地址、变量大小、数组大小、地址类型字符串解析成字节存储在AnalysisVarStruct结构体中
        public struct AnalysisVarStruct
        {
            /// <summary>
            /// 变量地址
            /// </summary>
            public byte[] Address;
            /// <summary>
            /// 变量大小
            /// </summary>
            public byte Size;
            /// <summary>
            /// 数组大小
            /// </summary>
            public byte Array_Size;
            /// <summary>
            /// 算法
            /// </summary>
            public double Conversion;
            /// <summary>
            /// 地址大小，即大小端
            /// </summary>
            public string Address_Type;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AnalysisVarS"></param>
        /// <param name="VariableInformationS"></param>
        /// <returns></returns>
        public static bool Deal_Dictionary_Data(out AnalysisVarStruct AnalysisVarS,VariableInformationStruct VariableInformationS)
        {
            AnalysisVarS = new AnalysisVarStruct();
            try
            {
                AnalysisVarS.Address = new byte[4];
                AnalysisVarS.Size = Convert.ToByte(VariableInformationS.Size);
                for (int i = 1; i < VariableInformationS.Address.Length / 2; i++)
                {
                    AnalysisVarS.Address[i - 1] = Convert.ToByte(VariableInformationS.Address.Substring(i * 2, 2), 16);
                }
                AnalysisVarS.Array_Size = Convert.ToByte(VariableInformationS.Array_Size);
                if(VariableInformationS.Conversion != "NULL METHOD")
                {
                    AnalysisVarS.Conversion = Convert.ToDouble(VariableInformationS.Conversion.Substring(2));
                }
                else
                {
                    AnalysisVarS.Conversion = 1;
                }
                AnalysisVarS.Address_Type = VariableInformationS.Address_Type;
                return true;
            }
            catch
            {

                return false;
            }
        }
        #endregion

        #region 接受CAN发送的数据信息结构体
        public struct CanRecDataStruct
        {
            /// <summary>
            /// 存储CAN接受到的8个字节数据
            /// </summary>
            public byte[] bytedata;
            /// <summary>
            /// true表示CAN接受到信息；false表示没有接收到信息
            /// </summary>
            public bool status;
            /// <summary>
            /// ID号
            /// </summary>
            public int id;

        }
        /// <summary>
        /// 定义CAN发接受数据信息结构体变量
        /// </summary>
        public static CanRecDataStruct CanRecDataS;
        #endregion

        #region CAN发送数据事件
        /// <summary>
        /// 定义CAN发送数据委托
        /// </summary>
        /// <param name="can_send_byte">要发送的8个字节的数据</param>
        /// <param name="id">PC端CAN的ID号</param>
        /// <param name="dlc"></param>
        /// <param name="timeout">发送等待接受事件</param>
        /// <returns>true:发送成功；false：发送失败</returns>
        public delegate bool SendMessageDelegate(byte[] can_send_byte, int id, int dlc, long timeout);

        /// <summary>
        /// CAN发送数据事件
        /// </summary>
        public static event SendMessageDelegate SendMessageD;
        #endregion

        #region 处理要下传的数据
        /// <summary>
        /// 处理要下传的数据
        /// </summary>
        /// <param name="bytedata">要处理的数据</param>
        /// <param name="data">处理之后的数据</param>
        /// <param name="variableInformationStruct"></param>
        public static void DealDownData(out byte[] bytedata,double data,VariableInformationStruct variableInformationStruct)
        {
            #region 定义变量
            double match = 0;
            bytedata = new byte[5];
            Int64 dataValue = 0;
            #endregion

            #region 根据算法处理数据
            if(variableInformationStruct.Conversion != "NULL METHOD")
            {
                match = Convert.ToDouble(variableInformationStruct.Conversion.Substring(2));
            }
            else
            {
                match = 1;
            }
            data *= match;
            #endregion

            #region 判断变量类型判断数据是否溢出，并作出处理
            switch (variableInformationStruct.Type)
            {
                case "SBYTE":
                    if(data > ((0xff - 1) / 2))
                    {
                        data -= 0xff - 1;
                    }
                    break;
                case "SWORD":
                    if (data > ((0xffff - 1) / 2))
                    {
                        data -= 0xffff - 1;
                    }
                    break;
                case "SLONG":
                    if (data > ((0xffffffff - 1) / 2))
                    {
                        data -= 0xffffffff - 1;
                    }
                    break;
            }
            #endregion
            dataValue = (Int64)data;
            #region 根据大小端处理数据，并将其赋值给bytedata数组
            if(variableInformationStruct.Address_Type == "BYTE_ORDER MSB_FIRST")
            {
                //大端高字节在前对应第一个元素
                switch (variableInformationStruct.Size)
                {//高字节在前
                    case "1":
                        bytedata[0] = (byte)(0xFF & (dataValue >> 0));
                        bytedata[1] = bytedata[2] = bytedata[3] = bytedata[4] = 0x00;
                        break;
                    case "2":
                        bytedata[0] = (byte)(0xFF & (dataValue >> 8));
                        bytedata[1] = (byte)(0xFF & (dataValue >> 0));
                        bytedata[2] = bytedata[3] = bytedata[4] = 0x00;
                        break;
                    case "4":
                        bytedata[0] = (byte)(0xFF & (dataValue >> 24));
                        bytedata[1] = (byte)(0xFF & (dataValue >> 16));
                        bytedata[2] = (byte)(0xFF & (dataValue >> 8));
                        bytedata[3] = (byte)(0xFF & (dataValue >> 0));
                        bytedata[4] = 0x00;
                        break;
                }
            }else if(variableInformationStruct.Address_Type == "BYTE_ORDER MSB_LAST")
            {
                switch (variableInformationStruct.Size)
                {//低字节在前
                    case "1":
                        bytedata[0] = (byte)(0xFF & (dataValue >> 0));
                        bytedata[1] = bytedata[2] = bytedata[3] = bytedata[4] = 0x00;
                        break;
                    case "2":
                        bytedata[0] = (byte)(0xFF & (dataValue >> 0));
                        bytedata[1] = (byte)(0xFF & (dataValue >> 8));
                        bytedata[2] = bytedata[3] = bytedata[4] = 0x00;
                        break;
                    case "4":
                        bytedata[0] = (byte)(0xFF & (dataValue >> 0));
                        bytedata[1] = (byte)(0xFF & (dataValue >> 8));
                        bytedata[2] = (byte)(0xFF & (dataValue >> 16));
                        bytedata[3] = (byte)(0xFF & (dataValue >> 24));
                        bytedata[4] = 0x00;
                        break;
                }
            }
            #endregion
        }
        #endregion

        #region 处理上传数据
        public static void DealUpData(byte[] bytedata,out double data,VariableInformationStruct VariableInforamtionS)
        {
            #region 定义变量
            double match = 0;
            data = 0;
            #endregion

            #region 根据大小端处理数据，并将其赋值给bytedata数组
            if (VariableInforamtionS.Address_Type == "BYTE_ORDER MSB_FIRST")
            {
                switch (VariableInforamtionS.Size)
                {
                    case "1":
                        data = bytedata[0];
                        break;
                    case "2":
                        data = (bytedata[0] << 8) | bytedata[1];
                        break;
                    case "4":
                        data = (bytedata[0] << 24) | bytedata[1] << 16 | bytedata[2] << 8 | bytedata[3];
                        break;
                    default:
                        break;
                }
            }
            else if (VariableInforamtionS.Address_Type == "BYTE_ORDER MSB_LAST")
            {
                switch (VariableInforamtionS.Size)
                {
                    case "1":
                        data = bytedata[0];
                        break;
                    case "2":
                        data = (bytedata[1] << 8) | bytedata[0];
                        break;
                    case "4":
                        data = (bytedata[3] << 24) | bytedata[2] << 16 | bytedata[1] << 8 | bytedata[0];
                        break;
                    default:
                        break;
                }
            }
            #endregion

            #region 判断变量类型判断数据是否溢出，并作出处理
            switch (VariableInforamtionS.Type)
            {
                case "SBYTE":
                    if (data > ((0xff - 1) / 2))
                    {
                        data -= 0xff - 1;
                    }
                    break;
                case "SWORD":
                    if (data > ((0xffff - 1) / 2))
                    {
                        data -= 0xffff - 1;
                    }
                    break;
                case "SLONG":
                    if (data > ((0xffffffff - 1) / 2))
                    {
                        data -= 0xffffffff - 1;
                    }
                    break;
            }
            #endregion

            #region 根据算法处理数据
            if (VariableInforamtionS.Conversion != "NULL METHOD")
            {
                match = Convert.ToDouble(VariableInforamtionS.Conversion.Substring(2));
            }
            else
            {
                match = 1;
            }
            data /= match;
            #endregion
        }
        #endregion

        #region MCU和PC进行连接
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pollingPath"></param>
        /// <param name="dAQPath"></param>
        /// <returns></returns>
        public static bool MCUAndPCLink(int id,string pollingPath,string dAQPath)
        {
            try
            {
                #region 等待线程信号
                AutoResetEvent.WaitOne();
                #endregion

                #region 定义变量
                count = 0;
                byte pKey = 10;
                UInt16 sizeKey = 10;
                #endregion

                #region 获取.dll解密文件路径
                if (pollingPath == null) PollingPath = "";
                else PollingPath = pollingPath;
                if (dAQPath == null) DAQPath = "";
                else DAQPath = dAQPath;
                #endregion

                #region 初始化状态机
                MCUAndPCLinkCPPStateE = CPPOrderStateEnum.Send0x01CCPOrderState;
                #endregion

                #region 没有SeedKey连接
                if(PollingPath == "" && DAQPath == "")
                {
                    while (true)
                    {
                        switch (MCUAndPCLinkCPPStateE)//发送0x01和0x14指令给下位机创建连接
                        {
                            #region 发送0x01CPP指令
                            case CPPOrderStateEnum.Send0x01CCPOrderState:
                                count = 0;
                                MCUAndPCLinkCPPStateE = CPPOrderStateEnum.Send0x01CCPOrderStateSuccess;
                                if(!SendMessageD(new byte[] { 0x01, Ctr, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, id, 8, 10))//发送连接的第一条指令
                                {
                                    MCUAndPCLinkCPPStateE = CPPOrderStateEnum.NullCCPOrderState;
                                    return false;
                                }
                                break;
                            #endregion

                            #region 发送两次0x14CPP指令
                            case CPPOrderStateEnum.Send0x14CCPOrderOneState:
                                count = 0;
                                MCUAndPCLinkCPPStateE = CPPOrderStateEnum.Send0x14CCPOrderOneStateSuccess;
                                if (!SendMessageD(new byte[] { 0x14, Ctr, 0x00, 0x00, 0x00, 0x00, 0x00, 0x36 }, id, 8, 10))
                                {
                                    MCUAndPCLinkCPPStateE = CPPOrderStateEnum.NullCCPOrderState;
                                    return false;
                                }
                                break;
                            case CPPOrderStateEnum.Send0x14CCPOrderTwoState:
                                count = 0;
                                MCUAndPCLinkCPPStateE = CPPOrderStateEnum.Send0x14CCPOrderTwoStateSuccess;
                                if (!SendMessageD(new byte[] { 0x14, Ctr, 0x01, 0x00, 0x00, 0x00, 0x00, 0x36 }, id, 8, 10))
                                {
                                    MCUAndPCLinkCPPStateE = CPPOrderStateEnum.NullCCPOrderState;
                                    return false;
                                }
                                break;
                            #endregion
                        }

                        if(MCUAndPCLinkCPPStateE == CPPOrderStateEnum.MCULinkPCStateSuccess)
                        {
                            MCUAndPCLinkCPPStateE = CPPOrderStateEnum.NullCCPOrderState;
                            return true;
                        }

                        #region 循环超时
                        if(count > 1000)
                        {
                            MCUAndPCLinkCPPStateE = CPPOrderStateEnum.NullCCPOrderState;
                            return false;
                        }
                        #endregion
                    }
                }
                #endregion

                else if(PollingPath != "" || DAQPath != "")
                {
                    while (true)
                    {
                        switch (MCUAndPCLinkCPPStateE)
                        {
                            #region 发送0x01CPP指令
                            case CPPOrderStateEnum.Send0x01CCPOrderState:
                                count = 0;
                                MCUAndPCLinkCPPStateE = CPPOrderStateEnum.Send0x01CCPOrderStateSuccess;
                                if (!SendMessageD(new byte[] { 0x01, Ctr, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, id, 8, 10))//发送连接的第一条指令
                                {
                                    MCUAndPCLinkCPPStateE = CPPOrderStateEnum.NullCCPOrderState;
                                    return false;
                                }
                                break;
                            #endregion

                            #region Polling模式
                            case CPPOrderStateEnum.Send0x12CCPOrderPollingState:
                                count = 0;
                                MCUAndPCLinkCPPStateE = CPPOrderStateEnum.Send0x12CCPOrderPollingSuccess;
                                if(!SendMessageD(new byte[] { 0x12, Ctr, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 }, id, 8, 10))
                                {
                                    MCUAndPCLinkCPPStateE = CPPOrderStateEnum.NullCCPOrderState;
                                    return false;
                                }
                                break;
                            case CPPOrderStateEnum.Send0x13CCPOrderPollingState:
                                count = 0;
                                Array.Copy(CanRecDataS.bytedata, 4, pSeed, 0, 4);
                                /*CallDll.Function_Dll(PollingPath,"ASAP1A_CPP_ComputeKeyFromSeed",pSeed,10,out pKey,10,out sizeKey);
                                 * CallDll.Achieve_Secret_key(ref pKey,out pSeed,sizeKey);
                                 */
                                MCUAndPCLinkCPPStateE = CPPOrderStateEnum.Send0x13CCPOrderPollingSuccess;
                                if (!SendMessageD(new byte[] { 0x13, Ctr, pSeed[0], pSeed[1], pSeed[2], pSeed[3], 0x00, 0x00 }, id, 8, 10))
                                {
                                    MCUAndPCLinkCPPStateE = CPPOrderStateEnum.NullCCPOrderState;
                                    return false;
                                }
                                break;
                            #endregion

                            #region DAQ模式
                            case CPPOrderStateEnum.Send0x12CCPOrderDAQState:
                                count = 0;
                                MCUAndPCLinkCPPStateE = CPPOrderStateEnum.Send0x12CCPOrderDAQStateSuccess;
                                if (!SendMessageD(new byte[] { 0x12, Ctr, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00 }, id, 8, 10))//发送连接的第一条指令
                                {
                                    MCUAndPCLinkCPPStateE = CPPOrderStateEnum.NullCCPOrderState;
                                    return false;
                                }
                                break;
                            case CPPOrderStateEnum.Send0x13CCPOrderDAQState:
                                count = 0;
                                PollingPath = "";
                                Array.Copy(CanRecDataS.bytedata, 4, pSeed, 0, 4);
                                /*
                                 CallDll.Function_Dll(DAQPath,"ASAP1A_CCP_ComputeKeyFromSeed" ,pSeed,0 ,out pKey,0,out sizeKey);
                                CallDll.Achieve_Secret_key(ref pKey,out pSeed,sizeKey);
                                 */
                                MCUAndPCLinkCPPStateE = CPPOrderStateEnum.Send0x13CCPOrderDAQStateSuccess ;
                                if (!SendMessageD(new byte[] { 0x13, Ctr, pSeed[0], pSeed[1], pSeed[2], pSeed[3], 0x00, 0x00 }, id, 8, 10))
                                {
                                    MCUAndPCLinkCPPStateE = CPPOrderStateEnum.NullCCPOrderState;
                                    return false;
                                }
                                break;
                            #endregion

                            #region 发送两次0x14CPP指令
                            case CPPOrderStateEnum.Send0x14CCPOrderOneState:
                                count = 0;
                                MCUAndPCLinkCPPStateE = CPPOrderStateEnum.Send0x14CCPOrderOneStateSuccess;
                                if (!SendMessageD(new byte[] { 0x14, Ctr, 0x00, 0x00, 0x00, 0x00, 0x01, 0x10 }, id, 8, 10))
                                {
                                    MCUAndPCLinkCPPStateE = CPPOrderStateEnum.NullCCPOrderState;
                                    return false;
                                }
                                break;
                            case CPPOrderStateEnum.Send0x14CCPOrderTwoState:
                                count = 0;
                                MCUAndPCLinkCPPStateE = CPPOrderStateEnum.Send0x14CCPOrderTwoStateSuccess;
                                if (!SendMessageD(new byte[] { 0x14, Ctr, 0x01, 0x00, 0x00, 0x00, 0x01, 0x01 }, id, 8, 10))
                                {
                                    MCUAndPCLinkCPPStateE = CPPOrderStateEnum.NullCCPOrderState;
                                    return false;
                                }
                                break;

                                #endregion
                        }

                        if(MCUAndPCLinkCPPStateE == CPPOrderStateEnum.MCULinkPCStateSuccess)
                        {
                            MCUAndPCLinkCPPStateE = CPPOrderStateEnum.NullCCPOrderState;
                            return true;
                        }

                        #region 循环超时
                        if (count > 1000)//连接失败
                        {
                            MCUAndPCLinkCPPStateE = CPPOrderStateEnum.NullCCPOrderState;
                            return false;
                        }
                        #endregion

                        #region 线程休眠，循环次数++
                        Thread.Sleep(1);
                        count++;
                        #endregion
                    }
                }
                MCUAndPCLinkCPPStateE = CPPOrderStateEnum.NullCCPOrderState;
                return true;
            }
            catch
            {
                MCUAndPCLinkCPPStateE = CPPOrderStateEnum.NullCCPOrderState;
                return false;
            }
            finally
            {
                AutoResetEvent.Set();
            }
        }
        #endregion

        #region 下发数据
        #region 下发选中的单个标定量
        /// <summary>
        /// 下发选中的单个标定量
        /// </summary>
        /// <param name="id">PC端CAN的ID号</param>
        /// <param name="data"></param>
        /// <param name="VariableInformationS"></param>
        /// <returns></returns>
        public static bool CCPDownOneData(int id,double data,VariableInformationStruct VariableInformationS)
        {
            try
            {
                #region 等待线程信号
                AutoResetEvent.WaitOne();
                #endregion

                #region 定义变量
                count = 0;
                address = 0;
                int firstNew = 0;
                #endregion

                #region 数据处理
                //AnalysisVarStruct AnalysisVarS;
                Deal_Dictionary_Data(out AnalysisVarS, VariableInformationS);
               // byte[] bytedata;
                DealDownData(out bytedata, data, VariableInformationS);
                #endregion

                #region 地址处理
                address = (AnalysisVarS.Address[0] << 24) + (AnalysisVarS.Address[1] << 16) + (AnalysisVarS.Address[2] << 8) + (AnalysisVarS.Address[3]);
                if (VariableInformationS.Name.Contains("--"))
                {
                    firstNew = Convert.ToInt32(VariableInformationS.Name.Substring(VariableInformationS.Name.LastIndexOf("--") + 2));
                    address += AnalysisVarS.Size + firstNew;
                }
                if(VariableInformationS.Address_Type == "BYTE_ORDER MSB_LAST")
                {
                    address = ((address & 0x000000FF) << 24 )+ ((address & 0x0000FF00) << 8 )+ ((address & 0x00FF0000) >> 8) + ((address & 0x00FF0000) >> 24);
                }
                #endregion

                DownDataCCPStateE = CPPOrderStateEnum.Send0x02CCPOrderState;
                while (true)
                {
                    switch (DownDataCCPStateE)
                    {
                        case CPPOrderStateEnum.Send0x02CCPOrderState:
                            DownDataCCPStateE = CPPOrderStateEnum.Send0x02CPPOrderStateSuccess;//
                            if(!SendMessageD(new byte[] { 0x02,Ctr,0x00,0x00,(byte)(0xFF&(address >> 24)), (byte)(0xFF & (address >> 16)),(byte)(0xFF & (address >> 8)), (byte)(0xFF & address) }, id, 8, 10))
                            {
                                DownDataCCPStateE = CPPOrderStateEnum.NullCCPOrderState;
                                return false;
                            }
                            break;

                        case CPPOrderStateEnum.Send0x03CCPOrderState:
                            DownDataCCPStateE = CPPOrderStateEnum.Send0x03CPPOrderStateSuccess;//
                            if (!SendMessageD(new byte[] { 0x03, Ctr, AnalysisVarS.Size, bytedata[0], bytedata[1], bytedata[2], bytedata[3], bytedata[4] }, id, 8, 10))
                            {
                                DownDataCCPStateE = CPPOrderStateEnum.NullCCPOrderState;
                                return false;
                            }
                            break;
                    }
                    if(DownDataCCPStateE == CPPOrderStateEnum.CCPDownDataStateSuccess)
                    {
                        count = 0;
                        DownDataCCPStateE = CPPOrderStateEnum.NullCCPOrderState;
                        return true;
                    }
                    if(count >= 1000)
                    {
                        DownDataCCPStateE = CPPOrderStateEnum.NullCCPOrderState;
                        return false;
                    }
                    Thread.Sleep(1);
                    count++;
                }
            }
            catch //(Exception)
            {
                DownDataCCPStateE = CPPOrderStateEnum.NullCCPOrderState;
                return false;
                //throw;
            }
            finally
            {
                AutoResetEvent.Set();
            }
        }
        #endregion

        #region 下载多个
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dicData"></param>
        /// <param name="VariableInformationDic"></param>
        /// <returns></returns>
        public　static bool CPPDownManyData(int id,Dictionary<int,double> dicData,Dictionary<int,VariableInformationStruct> VariableInformationDic)
        {
            try
            {
                #region 等待线程信号
                AutoResetEvent.WaitOne();
                #endregion

                #region 定义变量
                count = 0;
                address = 0;
                int firstNew = 0;
                #endregion

                foreach (KeyValuePair<int,VariableInformationStruct> dic in VariableInformationDic)
                {
                    #region 数据处理
                    //AnalysisVarStruct AnalysisVarS;
                    Deal_Dictionary_Data(out AnalysisVarS, dic.Value);
                    //byte[] bytedata;
                    DealDownData(out bytedata, dicData[dic.Key], dic.Value);
                    #endregion

                    #region 地址处理
                    address = (AnalysisVarS.Address[0] << 24) + (AnalysisVarS.Address[1] << 16) + (AnalysisVarS.Address[2] << 8) + (AnalysisVarS.Address[3]);
                    if (dic.Value.Name.Contains("--"))
                    {
                        firstNew = Convert.ToInt32(dic.Value.Name.Substring(dic.Value.Name.LastIndexOf("--") + 2));
                        address += AnalysisVarS.Size + firstNew;
                    }
                    if (dic.Value.Address_Type == "BYTE_ORDER MSB_LAST")
                    {
                        address = ((address & 0x000000FF) << 24) + ((address & 0x0000FF00) << 8) + ((address & 0x00FF0000) >> 8) + ((address & 0x00FF0000) >> 24);
                    }
                    #endregion

                    DownDataCCPStateE = CPPOrderStateEnum.Send0x02CCPOrderState;
                    while (true)
                    {
                        switch (DownDataCCPStateE)
                        {
                            case CPPOrderStateEnum.Send0x02CCPOrderState:
                                DownDataCCPStateE = CPPOrderStateEnum.Send0x02CPPOrderStateSuccess;//
                                if (!SendMessageD(new byte[] { 0x02, Ctr, 0x00, 0x00, (byte)(0xFF & (address >> 24)), (byte)(0xFF & (address >> 16)), (byte)(0xFF & (address >> 8)), (byte)(0xFF & address) }, id, 8, 10))
                                {
                                    DownDataCCPStateE = CPPOrderStateEnum.NullCCPOrderState;
                                    return false;
                                }
                                break;

                            case CPPOrderStateEnum.Send0x03CCPOrderState:
                                DownDataCCPStateE = CPPOrderStateEnum.Send0x03CPPOrderStateSuccess;//
                                if (!SendMessageD(new byte[] { 0x03, Ctr, AnalysisVarS.Size, bytedata[0], bytedata[1], bytedata[2], bytedata[3], bytedata[4] }, id, 8, 10))
                                {
                                    DownDataCCPStateE = CPPOrderStateEnum.NullCCPOrderState;
                                    return false;
                                }
                                break;
                        }
                        if (DownDataCCPStateE == CPPOrderStateEnum.CCPDownDataStateSuccess)
                        {
                            count = 0;
                            DownDataCCPStateE = CPPOrderStateEnum.NullCCPOrderState;
                            break;
                        }
                        if (count >= 1000)
                        {
                            DownDataCCPStateE = CPPOrderStateEnum.NullCCPOrderState;
                            return false;
                        }
                        Thread.Sleep(1);
                        count++;
                    }
                }
                DownDataCCPStateE = CPPOrderStateEnum.NullCCPOrderState;
                return true;
            }
            catch
            {
                DownDataCCPStateE = CPPOrderStateEnum.NullCCPOrderState;
                return false;
            }
            finally
            {
                AutoResetEvent.Set();
            }
        }
        #endregion
        #endregion

        #region 上传数据
        #region 上传单个数据
        public static bool CPPUpOneData(int id,out double dicData,VariableInformationStruct VariableInformationS)
        {
            dicData = 0; 
            try
            {
                #region 等待线程信号
                AutoResetEvent.WaitOne();
                #endregion

                #region 定义变量
                count = 0;
                address = 0;//地址大小
                int firstNew = 0;//第几个数组元素
                #endregion

                #region 地址处理
                AnalysisVarStruct AnalysisVarS;
                Deal_Dictionary_Data(out AnalysisVarS, VariableInformationS);
                address = (AnalysisVarS.Address[0] << 24) + (AnalysisVarS.Address[1] << 16) + (AnalysisVarS.Address[2] << 8) + (AnalysisVarS.Address[3]);
                if (VariableInformationS.Name.Contains("--"))
                {
                    firstNew = Convert.ToInt32(VariableInformationS.Name.Substring(VariableInformationS.Name.LastIndexOf("--") + 2));
                    address += AnalysisVarS.Size + firstNew;
                }
                if (VariableInformationS.Address_Type == "BYTE_ORDER MSB_LAST")
                {
                    address = ((address & 0x000000FF) << 24) + ((address & 0x0000FF00) << 8) + ((address & 0x00FF0000) >> 8) + ((address & 0x00FF0000) >> 24);
                }
                #endregion

                UpDataCCPStateE = CPPOrderStateEnum.Send0x0FCCPOrderStateSuccess;
                while (true)
                {
                    switch (UpDataCCPStateE)
                    {
                        case CPPOrderStateEnum.Send0x0FCCPOrderState:
                            UpDataCCPStateE = CPPOrderStateEnum.Send0x0FCCPOrderStateSuccess;
                            if (!SendMessageD(new byte[] { 0x0F, Ctr, AnalysisVarS.Size, 0x00, (byte)(0xFF & (address >> 24)), (byte)(0xFF & (address >> 16)), (byte)(0xFF & (address >> 8)), (byte)(0xFF & address) }, id, 8, 10))
                            {
                                DownDataCCPStateE = CPPOrderStateEnum.NullCCPOrderState;
                                return false;
                            }
                            count = 0;
                            break;
                    }
                    if (UpDataCCPStateE == CPPOrderStateEnum.Send0x0FCCPOrderStateSuccess) //成功
                    {
                        count = 0;
                        DealUpData(TempBuf, out dicData, VariableInformationS);
                        UpDataCCPStateE = CPPOrderStateEnum.NullCCPOrderState;
                        return true;
                    }
                    if(count >= 1000)
                    {
                        count = 0;
                        UpDataCCPStateE = CPPOrderStateEnum.NullCCPOrderState;
                        return false;
                    }
                    Thread.Sleep(1);
                    count++;
                }

            }
            catch //(Exception)
            {
                UpDataCCPStateE = CPPOrderStateEnum.NullCCPOrderState;
                return false;
            }
            finally
            {
                AutoResetEvent.Set();
            }
        }
        #endregion

        #region 上传多个数据
        public static bool CCPUpManyData(int id,out Dictionary<int,double> dicData,Dictionary<int,VariableInformationStruct> VariableInformationDic)
        {
            dicData = new Dictionary<int, double>();
            try
            {
                #region 等待线程信号
                AutoResetEvent.WaitOne();
                #endregion

                #region 定义变量
                count = 0;
                address = 0;//地址大小
                int firstNew = 0;//第几个数组元素
                double data = 0;
                #endregion

                UpDataCCPStateE = CPPOrderStateEnum.Send0x0FCCPOrderStateSuccess;
                foreach (KeyValuePair<int,VariableInformationStruct> dic in VariableInformationDic)
                {
                    #region 地址处理
                    AnalysisVarStruct AnalysisVarS;
                    Deal_Dictionary_Data(out AnalysisVarS, dic.Value);
                    address = (AnalysisVarS.Address[0] << 24) + (AnalysisVarS.Address[1] << 16) + (AnalysisVarS.Address[2] << 8) + (AnalysisVarS.Address[3]);
                    if (dic.Value.Name.Contains("--"))
                    {
                        firstNew = Convert.ToInt32(dic.Value.Name.Substring(dic.Value.Name.LastIndexOf("--") + 2));
                        address += AnalysisVarS.Size + firstNew;
                    }
                    if (dic.Value.Address_Type == "BYTE_ORDER MSB_LAST")
                    {
                        address = ((address & 0x000000FF) << 24) + ((address & 0x0000FF00) << 8) + ((address & 0x00FF0000) >> 8) + ((address & 0x00FF0000) >> 24);
                    }
                    #endregion

                    while (true)
                    {
                        switch (UpDataCCPStateE)
                        {
                            case CPPOrderStateEnum.Send0x0FCCPOrderState:
                                UpDataCCPStateE = CPPOrderStateEnum.Send0x0FCCPOrderStateSuccess;
                                if (!SendMessageD(new byte[] { 0x0F, Ctr, AnalysisVarS.Size, 0x00, (byte)(0xFF & (address >> 24)), (byte)(0xFF & (address >> 16)), (byte)(0xFF & (address >> 8)), (byte)(0xFF & address) }, id, 8, 10))
                                {
                                    DownDataCCPStateE = CPPOrderStateEnum.NullCCPOrderState;
                                    return false;
                                }
                                count = 0;
                                break;
                        }
                        if (UpDataCCPStateE == CPPOrderStateEnum.Send0x0FCCPOrderStateSuccess) //成功
                        {
                            count = 0;
                            DealUpData(TempBuf, out data, dic.Value);
                            UpDataCCPStateE = CPPOrderStateEnum.NullCCPOrderState;
                            dicData.Add(dic.Key, data);
                            break;
                            //return true;
                        }
                        if (count >= 1000)
                        {
                            count = 0;
                            UpDataCCPStateE = CPPOrderStateEnum.NullCCPOrderState;
                            return false;
                        }
                        Thread.Sleep(1);
                        count++;
                    }
                }

                UpDataCCPStateE = CPPOrderStateEnum.NullCCPOrderState;
                return true;
            }
            catch //()
            {
                UpDataCCPStateE = CPPOrderStateEnum.NullCCPOrderState;
                return false;
                //     throw;
            }
            finally
            {
                AutoResetEvent.Set();
            }
        }
        #endregion

        #endregion

        #region 响应CPP指令
        #region 响应CPP指令线程
        public static Thread CCPOrderResponseThread;
        #endregion

        #region 响应CPP指令
        public static void CCPOrderResponese()
        {
            while (true)
            {
                try
                {
                    if (CanRecDataS.status)
                    {
                        CanRecDataS.status = false;

                        switch (MCUAndPCLinkCPPStateE)
                        {
                            #region 发送0x01 CPP指令
                            case CPPOrderStateEnum.Send0x01CCPOrderStateSuccess:
                                if (PollingPath != "" && PollingPath != null)
                                {
                                    MCUAndPCLinkCPPStateE = CPPOrderStateEnum.Send0x12CCPOrderPollingState;
                                }
                                else if (DAQPath != "" && DAQPath != null)
                                {
                                    MCUAndPCLinkCPPStateE = CPPOrderStateEnum.Send0x12CCPOrderDAQState;
                                }
                                else
                                {
                                    MCUAndPCLinkCPPStateE = CPPOrderStateEnum.Send0x14CCPOrderOneState;
                                }
                                break;
                            #endregion

                            #region Polling模式
                            case CPPOrderStateEnum.Send0x12CCPOrderPollingSuccess:
                                MCUAndPCLinkCPPStateE = CPPOrderStateEnum.Send0x13CCPOrderPollingState;
                                break;
                            case CPPOrderStateEnum.Send0x13CCPOrderPollingSuccess:
                                if (DAQPath != "" && DAQPath != null)
                                {
                                    MCUAndPCLinkCPPStateE = CPPOrderStateEnum.Send0x12CCPOrderDAQState;
                                }
                                else
                                {
                                    MCUAndPCLinkCPPStateE = CPPOrderStateEnum.Send0x14CCPOrderOneState;
                                }
                                break;
                            #endregion

                            #region DAQ模式
                            case CPPOrderStateEnum.Send0x12CCPOrderDAQStateSuccess:
                                MCUAndPCLinkCPPStateE = CPPOrderStateEnum.Send0x13CCPOrderDAQStateSuccess;
                                break;
                            case CPPOrderStateEnum.Send0x13CCPOrderDAQStateSuccess:
                                MCUAndPCLinkCPPStateE = CPPOrderStateEnum.Send0x14CCPOrderOneState;
                                break;
                            #endregion

                            #region 发送两次0x14 CCP指令
                            case CPPOrderStateEnum.Send0x14CCPOrderOneStateSuccess:
                                MCUAndPCLinkCPPStateE = CPPOrderStateEnum.Send0x14CCPOrderTwoState;
                                break;
                            case CPPOrderStateEnum.Send0x14CCPOrderTwoStateSuccess:
                                MCUAndPCLinkCPPStateE = CPPOrderStateEnum.MCULinkPCStateSuccess;
                                break;
                                #endregion
                        }

                        switch (DownDataCCPStateE)
                        {
                            case CPPOrderStateEnum.Send0x02CPPOrderStateSuccess:
                                DownDataCCPStateE = CPPOrderStateEnum.Send0x03CCPOrderState;
                                break;

                            case CPPOrderStateEnum.Send0x03CPPOrderStateSuccess:
                                DownDataCCPStateE = CPPOrderStateEnum.CCPDownDataStateSuccess;
                                break;

                        }

                        switch (UpDataCCPStateE)
                        {
                            case CPPOrderStateEnum.Send0x0FCCPOrderStateSuccess:
                                TempBuf = new byte[5];
                                Array.Copy(CanRecDataS.bytedata, 3, TempBuf, 0, 5);
                                UpDataCCPStateE = CPPOrderStateEnum.CCPDownDataStateSuccess;
                                break;
                        }
                    }
                    Thread.Sleep(1);
                }
                catch// (Exception)
                {

                    //throw;
                }
            }
        }
        #endregion

        #region 打开响应CPP指令线程
        public static bool OpenCCPOrderResponseThread()
        {
            try
            {
                if (CCPOrderResponseThread != null) CCPOrderResponseThread.Abort();
                CCPOrderResponseThread = new Thread(new ThreadStart(CCPOrderResponese));
                CCPOrderResponseThread.IsBackground = true;
                CCPOrderResponseThread.Start();
                while (!CCPOrderResponseThread.IsAlive == true) 

                {
                    Thread.Sleep(1);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region 关闭响应CPP指令线程
        public static bool CloseCCPOrderResponseThread()
        {
            try
            {
                if(CCPOrderResponseThread != null)
                {
                    if (CCPOrderResponseThread.IsAlive == true) CCPOrderResponseThread.Abort();
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        #endregion

        #endregion
    }
}
