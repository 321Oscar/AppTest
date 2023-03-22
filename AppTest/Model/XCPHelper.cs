using AppTest.Helper;
using System;
using System.Collections.Generic;

namespace AppTest.Model
{
    /// <summary>
    /// XCP通用方法
    /// <para><see cref="XCPHelper"/></para>
    /// </summary>
    public static class XCPHelper
    {
        public const byte STD_CONNECT = 0xFF;
        public const byte STD_DISCONNECT = 0xFE;
        public const byte STD_SHORTUPLOAD = 0xF4;
        public const byte STD_SET_MTA = 0xF6;
        public const byte STD_UPLOAD = 0xF5;
        public const byte STD_SENDKEY = 0xF7;
        public const byte STD_GETSEED = 0xF8;
        public const byte CAL_DOWNLOAD = 0xF0;

        public static XCPResponse TransformBytetoRes(byte code)
        {
            switch (code)
            {
                case 0xFF:
                    return XCPResponse.Ok;
                case 0xFE:
                    return XCPResponse.Err;
                default:
                    return XCPResponse.Ok;
            }
        }

        internal static byte[] CalKeyWithSeed(List<byte> seeds)
        {
            throw new NotImplementedException();
        }

        internal static byte[] ConvertToByte(string writeData, int valueType,int byteorder)
        {
            byte[] rntData = new byte[8];
            switch ((XCPValueType)valueType)
            {
                case XCPValueType.BOOLEAN:
                    int vBool = int.Parse(writeData);
                    bool b = Convert.ToBoolean(vBool);
                    rntData[0] = Convert.ToByte(b);
                    break;
                case XCPValueType.UBYTE:
                    //case XCPValueType.UBYTE:
                    UInt16 ui = Convert.ToUInt16(writeData);
                    rntData[0] = Convert.ToByte(ui);
                    break;
                case XCPValueType.BYTE:
                    Int16 i = Convert.ToInt16(writeData);
                    rntData[0] = Convert.ToByte(i);
                    break;
                case XCPValueType.UWORD:
                    {
                        UInt64 temp = Convert.ToUInt64(writeData);
                        rntData[1] = Convert.ToByte(temp & 0xff);//
                        rntData[0] = Convert.ToByte((temp >> 8) & 0xff);//
                    }
                    break;
                case XCPValueType.SWORD:
                    {
                        Int64 temp = Convert.ToInt64(writeData);
                        rntData[1] = Convert.ToByte(temp & 0xff);//
                        rntData[0] = Convert.ToByte((temp >> 8) & 0xff);//
                    }
                    break;
                case XCPValueType.ULONG:
                    {
                        UInt64 temp = Convert.ToUInt64(writeData);
                        rntData[3] = Convert.ToByte(temp & 0xff);//
                        rntData[2] = Convert.ToByte((temp >> 8) & 0xff);//
                        rntData[1] = Convert.ToByte((temp >> 16) & 0xff);//
                        rntData[0] = Convert.ToByte((temp >> 24) & 0xff);//
                    }
                    break;
                case XCPValueType.LONG:
                    {
                        Int64 temp = Convert.ToInt64(writeData);
                        rntData[3] = Convert.ToByte(temp & 0xff);//
                        rntData[2] = Convert.ToByte((temp >> 8) & 0xff);//
                        rntData[1] = Convert.ToByte((temp >> 16) & 0xff);//
                        rntData[0] = Convert.ToByte((temp >> 24) & 0xff);//
                    }
                    break;
                case XCPValueType.FLOAT32:
                    {
                        Single temp = Convert.ToSingle(writeData);
                        byte[] temp1 = BitConverter.GetBytes(temp);
                        rntData[0] = temp1[3];
                        rntData[1] = temp1[2];
                        rntData[2] = temp1[1];
                        rntData[3] = temp1[0];
                    }
                    break;
                case XCPValueType.FLOAT64:
                    {
                        double temp = Convert.ToDouble(writeData);
                        byte[] temp1 = BitConverter.GetBytes(temp);

                        rntData[0] = temp1[7];
                        rntData[1] = temp1[6];
                        rntData[2] = temp1[5];
                        rntData[3] = temp1[4];
                        rntData[4] = temp1[3];
                        rntData[5] = temp1[2];
                        rntData[6] = temp1[1];
                        rntData[7] = temp1[0];
                    }
                    break;
                case XCPValueType.Lookup1D_BOOLEAN:
                    {
                        bool temp = Convert.ToBoolean(writeData);
                        rntData[0] = Convert.ToByte(temp);//length
                    }
                    break;
            }

            if(byteorder == 0)//motorala
            {

            }
            else //intel
            {
                Array.Reverse(rntData);
            }

            return rntData;
        }

        public static bool CTOCheck(byte firstByte)
        {
            switch (firstByte)
            {
                case 0xff:
                case 0xFE:
                case 0xfd:
                case 0xfc:
                    return true;
                
                default:
                    return false;
            }
        }

        public static string ParseErrCode(byte errCode)
        {
            switch (errCode)
            {
                case 0x00: return "Command Processor synchronization.";

                case 0x10: return "Command was not executed.";
                case 0x11: return "Command rejected because DAQ is running.";
                case 0x12: return "Command rejected because PGM is running.";

                case 0x20: return "Unknown command or not implemented optional command";
                case 0x21: return "Command syntax invalid";
                case 0x22: return "Command syntax valid but command parameter(s) out of range.";
                case 0x23: return "The memory location is write protected.";
                case 0x24: return "The memory location is not accessible.";
                case 0x25: return "Access denied,Seed & Key is required.";
                case 0x26: return "Selected page not available.";
                case 0x27: return "Selected page mode not available.";
                case 0x28: return "Selected segment not valid.";
                case 0x29: return "Sequence error.";
                case 0x2A: return "DAQ configuration not valid.";

                case 0x30: return "Memory overflow error.";
                case 0x31: return "Generic error.";
                case 0x32: return "The slave internal program verify routine detects an error.";

                default: return $"Invaild Err Code:0x{errCode:16}";
            }
            
        }

        /// <summary>
        /// 解析4位byte数据
        /// </summary>
        /// <param name="signal"></param>
        /// <param name="resData"></param>
        /// <returns></returns>
        internal static string DealData4Byte(XCPSignal signal, byte[] resData)
        {
            Int64 temp = 0;
            for (int i = 0; i < signal.Length; i++)
            {
                temp = temp << 8 | resData[i];
            }

            switch ((XCPValueType)signal.ValueType)
            {
                case XCPValueType.BOOLEAN:
                case XCPValueType.UBYTE:
                case XCPValueType.UWORD:
                case XCPValueType.SWORD:
                case XCPValueType.ULONG:
                case XCPValueType.LONG:
                    return Convert.ToString(temp);
                case XCPValueType.BYTE:
                    return Convert.ToString((int)temp);
                case XCPValueType.FLOAT32:
                    {
                        if (signal.ByteOrder_int == 0)
                            Array.Reverse(resData);
                        var v = BitConverter.ToSingle(resData, 0);
                        return XCPHelper.MethodStruConvert(signal.Compu_Methd, v);
                    }
                case XCPValueType.FLOAT64:
                    byte[] temp1 = new byte[4];
                    temp1[0] = (byte)(temp & 0xff);
                    temp1[1] = (byte)((temp >> 8) & 0xff);
                    temp1[2] = (byte)((temp >> 16) & 0xff);
                    temp1[3] = (byte)((temp >> 24) & 0xff);
                    return Convert.ToString(BitConverter.ToSingle(temp1, 0));
            }

            return "--";
        }

        internal static void TransformAddress(string eCUAddress, int address_TYPE, out byte[] address)
        {
            address = new byte[4];
            //int add = int.Parse(eCUAddress,System.Globalization.NumberStyles.HexNumber)
            if (ulong.TryParse(eCUAddress, System.Globalization.NumberStyles.HexNumber, null, out ulong Address))
            {
                if (address_TYPE == 0)
                {
                    address[0] = (Byte)(Address >> 24 & 0xff);
                    address[1] = (Byte)(Address >> 16 & 0xff);
                    address[2] = (Byte)(Address >> 8 & 0xff);
                    address[3] = (Byte)(Address & 0xff);
                }
                else if (address_TYPE == 1)//EcuType.CompareTo("Intel") == 0)
                {
                    address[0] = (Byte)(Address & 0xff);
                    address[1] = (Byte)(Address >> 8 & 0xff);
                    address[2] = (Byte)(Address >> 16 & 0xff);
                    address[3] = (Byte)(Address >> 24 & 0xff);
                }
            }
        }

        public static int RecordStrConvert(string record)
        {
            if (Enum.TryParse(record, out XCPValueType valueType))
            {
                return (int)valueType;
            }
            else
            {
                List<string> valueTypes = new List<string>();
                valueTypes.Add(XCPValueType.BOOLEAN.ToString());
                valueTypes.Add(XCPValueType.UBYTE.ToString());
                valueTypes.Add(XCPValueType.LONG.ToString());
                valueTypes.Add(XCPValueType.SWORD.ToString());
                valueTypes.Add(XCPValueType.UWORD.ToString());
                valueTypes.Add(XCPValueType.ULONG.ToString());
                valueTypes.Add(XCPValueType.FLOAT32.ToString());
                valueTypes.Add(XCPValueType.FLOAT64.ToString());
                valueTypes.Add(XCPValueType.BYTE.ToString());
                valueTypes.Add(XCPValueType.Lookup1D_BOOLEAN.ToString());

                var ty = valueTypes.Find(x => record.ToUpper().Contains(x));

                if (ty != null)
                {
                    Enum.TryParse(ty, out valueType);
                    return (int)valueType;
                }
                else
                {
                    return -1;
                }

            }
        }

        /// <summary>
        /// 将method转成系数
        /// </summary>
        /// <param name="conversion_Method"></param>
        /// <returns></returns>
        internal static string MethodStruConvert(CCP_COMPU_METHOD conversion_Method, float oldValue)
        {
            try
            {
                if (conversion_Method.Format == null)
                    return oldValue.ToString();
                var formats = conversion_Method.Format.Split('%', '.', '"');
                int length = int.Parse(formats[2]);//总长
                int length2 = int.Parse(formats[3]);//小数位数
                string format = $"f{length2}";
                if (conversion_Method.Conversion_Type.ToLower().Contains("rat"))
                {
                    string[] coffs = conversion_Method.Coefficients.Split(' ');
                    float a = float.Parse(coffs[1]);
                    float b = float.Parse(coffs[2]);
                    float c = float.Parse(coffs[3]);
                    float d = float.Parse(coffs[4]);
                    float e = float.Parse(coffs[5]);
                    float f = float.Parse(coffs[6]);
                    float x = ((a * oldValue * oldValue) + (b * oldValue) + c) / (d * oldValue * oldValue + e * oldValue + f);
                    return x.ToString(format);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("MethodStruConvert", ex);

            }
            return oldValue.ToString();
        }

        internal static void ParseCommModeBasic(byte v, out Global.ByteOrder bo, out bool slaveBlockavali)
        {
            int by = v & 1;
            bo = by == 0 ? Global.ByteOrder.Intel : Global.ByteOrder.Motorola;
            slaveBlockavali = (v & (1 >> 6)) != 0;
        }
    }
}
