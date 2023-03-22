namespace AppTest.Model
{
    /// <summary>
    /// 事件通道信息 -- D7
    /// </summary>
    public class DAQEventProperty
    {
        public DAQEventProperty(byte[] data)
        {
            Max_DAQ_List = data[2];
            EventChannelNameLength = data[3];
            EventChannelTimeCycle = data[4];
            EventChannelTimeUnit = data[5];
            EventChannelPriority = data[6];
        }

        /// <summary>
        /// DAQ/STIM/Both : byte[1]
        /// </summary>
        public int Event_Channel_Type { private set; get; }
        /// <summary>
        /// 此事件通道中DAQ列表的最大数量
        /// </summary>
        public byte Max_DAQ_List { private set; get; }
        /// <summary>
        /// 事件通道名称长度
        /// </summary>
        public byte EventChannelNameLength { private set; get; }
        /// <summary>
        /// 事件通道时间周期
        /// </summary>
        public byte EventChannelTimeCycle { private set; get; }
        /// <summary>
        /// 事件通道时间单位
        /// </summary>
        public int EventChannelTimeUnit { private set; get; }
        /// <summary>
        /// 事件通道优先级（FF最高）
        /// </summary>
        public byte EventChannelPriority { private set; get; }

        public string EventName { get; set; }
    }
}
