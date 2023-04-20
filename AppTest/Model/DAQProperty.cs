using AppTest.Helper;
using System;

namespace AppTest.Model
{
    /// <summary>
    /// DAQ信息 -- DA
    /// </summary>
    public class DAQProperty : BaseXCPResponse
    {
        #region Byte 1

        /// <summary>
        /// 0 = static DAQ list configuration；1 = dynamic DAQ.
        /// </summary>
        public int DAQ_Config_type { private set; get; }
        /// <summary>
        /// 0 = Prescaler not support;1 = Prescaler supported.
        /// </summary>
        public int Prescaler_Supported { private set; get; }
        public int Resume_Supported { private set; get; }
        public int Bit_STIM_Supported { private set; get; }
        public int Timestamp_Supported { private set; get; }
        public int PID_OFF_Supported { private set; get; }
        /// <summary>
        /// overload event + overload msb
        /// <para>0: no overload indication</para>
        /// <para>1: overload indication in MSB of PID</para>
        /// <para>2: overload indication by Event Packet</para>
        /// <para>3: not allowed</para>
        /// </summary>
        public int Overrun_IndicationMethod { private set; get; }
        public string Overrun_IndicationMethod_Str { 
            get 
            {
                switch (Overrun_IndicationMethod)
                {
                    case 0:
                        return "no overload indication";
                    case 1:
                        return "overload indication in MSB of PID";
                    case 2:
                        return "overload indication by Event Packet";
                    case 3:
                        return "not allowed";
                    default:
                        break;
                }
                return string.Empty;
            } 
        }
        #endregion

        public DAQProperty(byte[] data,byte byteOrder = 0) : base(data)
        {
            #region byte 1
            byte daqProperties = data[1];
            
            DAQ_Config_type = daqProperties.GetBit(0);
            Prescaler_Supported = daqProperties.GetBit(1);
            Resume_Supported = daqProperties.GetBit(2);
            Bit_STIM_Supported = daqProperties.GetBit(3);
            Timestamp_Supported = daqProperties.GetBit(4);
            PID_OFF_Supported = daqProperties.GetBit(5);
            Overrun_IndicationMethod = daqProperties.GetBits(6, 7);           
            #endregion

            if (byteOrder == 0)
            {
                Max_DAQ = BitConverter.ToInt16(new byte[] { data[3], data[2] }, 0);
                Max_Event_Channel = BitConverter.ToInt16(new byte[] { data[5], data[4] }, 0);
            }
            else
            {
                Max_DAQ = BitConverter.ToInt16(data, 2);
                Max_Event_Channel = BitConverter.ToInt16(data, 4);
            }
            
            Min_DAQ = data[6];

            //byte 7:DAQ key byte
            Optimisation_Type = data[7].GetBits(0, 3);
            Address_Extension_Type = data[7].GetBits(4, 5);
            Identification_Field_Type = data[7].GetBits(6, 7);
        }

        /// <summary>
        /// 可用DAQ列表总数:byte[2]+byte[3]
        /// </summary>
        public short Max_DAQ { private set; get; }
        /// <summary>
        /// 可用事件通道总数:byte[4]+byte[5]
        /// </summary>
        public short Max_Event_Channel { private set; get; }
        /// <summary>
        /// 预定义DAQ列表的总数:byte[6]
        /// </summary>
        public byte Min_DAQ { private set; get; }

        public int Optimisation_Type { private set; get; }

        public int Address_Extension_Type { private set; get; }

        public int Identification_Field_Type { private set; get; }
    }
}
