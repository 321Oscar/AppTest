using System;

namespace AppTest.Model
{
    [Serializable]
    public class DBCSignal : BaseSignal,IComparable<DBCSignal>
    {
        private int messageID;

        /// <summary>
        /// 16进制
        /// </summary>
        public string MessageID {
            get { return messageID.ToString("X"); }
            set { messageID = int.Parse(value, System.Globalization.NumberStyles.HexNumber); }
        }

       
        /// <summary>
        /// 起始位
        /// </summary>
        [Signal("起始位", min:0,max :62)]
        public int StartBit { get; set; }
       
        /// <summary>
        /// 大小端：Intel 小端格式：地址的增长顺序与值的增长顺序相同
        /// </summary>
        [Signal("1:Intel;0:Motorola", new int[2] { 0, 1 }, new string[] { "Motorola", "Intel" })]
        public int ByteOrder { get; set; }
        [Signal("1:signed;0:unsigned",new int[2]{0,1},new string[] { "unsigned", "signed" })]
        public new int ValueType { get; set; }
        /// <summary>
        /// 分辨率
        /// </summary>
        public double Factor { get; set; }
        /// <summary>
        /// 偏移量
        /// </summary>
        public double Offset { get; set; }
        /// <summary>
        /// 发送周期
        /// </summary>
        public int CycleTime { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else if (this.GetType() != obj.GetType())
                return false;
            else
                return ((DBCSignal)obj).SignalName == this.SignalName && ((DBCSignal)obj).MessageID == MessageID;
        }

        public override int GetHashCode()
        {
            return (this.MessageID + this.SignalName).GetHashCode();
        }

        public int CompareTo(DBCSignal other)
        {
            if (other.messageID > this.messageID)
            {
                return -1;
            }
            else if (other.messageID == this.messageID)
            {
                return this.StartBit.CompareTo(other.StartBit);
            }
            else
                return 1;
            
        }
    }

}
