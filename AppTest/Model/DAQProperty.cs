using System;

namespace AppTest.Model
{
    /// <summary>
    /// DAQ信息 -- DA
    /// </summary>
    public class DAQProperty
    {
        public DAQProperty(byte[] data,byte byteOrder = 0)
        {
            if(byteOrder == 0)
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
        }

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

        public int Overrun_IndicationMethod { private set; get; }
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
