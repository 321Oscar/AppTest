namespace AppTest.Model
{
    public class XCPSignal : BaseSignal
    {
        /// <summary>
        /// 地址
        /// </summary>
        [Signal("ECU地址，16进制")]
        public string ECUAddress { get; set; }
        /// <summary>
        /// 转换方式
        /// </summary>
        [Signal("转换系数")]
        public string Conversion { get; set; }

        /// <summary>
        /// 转换方法
        /// </summary>
        [Signal(false)]
        public CCP_COMPU_METHOD Compu_Methd { get; set; }
        /// <summary>
        /// 标定 False/测量 True
        /// </summary>
        [Signal("标定/测量信号", "测量信号", "标定信号")]
        public bool MeaOrCal { get; set; }
        /// <summary>
        /// 大小端
        /// </summary>
        [Signal("大小端", new int[2] { 0, 1 }, new string[] { "Motorola", "Intel" })]
        public new int ByteOrder_int { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        [Signal("数据类型", new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new string[10] {"BOOLEAN","UBYTE" ,
            "BYTE","UWORD","SWORD","ULONG","LONG","FLOAT32_IEEE","FLOAT64_IEEE","Lookup1D_Boolean"})]
        public new int ValueType { get; set; }
        /// <summary>
        /// 拓展地址
        /// </summary>
        [Signal("拓展地址，Hex")]
        public int AddressExtension { get => addressExtension; set => addressExtension = value; }
        private int addressExtension = 0;

        [Signal(false)]
        public CompuMethods CompuMethods { get; set; }

        /// <summary>
        /// 发送周期
        /// </summary>
        public int CycleTime { get; set; } = 1000;

        /// <summary>
        /// 标识号
        /// </summary>
        public int DAQID { get; set; }

        /// <summary>
        /// 在DAQ DTO中的起始坐标
        /// </summary>
        public int StartIndex { get; set; }

        /// <summary>
        /// 事件通道编号
        /// </summary>
        public int EventID { get; set; }
        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
#pragma warning disable CS0108 // “XCPSignal.TimeStamp”隐藏继承的成员“BaseSignal.TimeStamp”。如果是有意隐藏，请使用关键字 new。
        public int TimeStamp { get; set; }
#pragma warning restore CS0108 // “XCPSignal.TimeStamp”隐藏继承的成员“BaseSignal.TimeStamp”。如果是有意隐藏，请使用关键字 new。
    }
}
