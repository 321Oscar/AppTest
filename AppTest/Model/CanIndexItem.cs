using System;

namespace AppTest.Model
{
    public class CanIndexItem : IComparable<CanIndexItem>
    {
        /// <summary>
        /// 
        /// </summary>
        public int CanChannel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool isUsed { get; set; }

        /// <summary>
        /// 是否用于软件校验
        /// </summary>
        public bool IsAuth { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ProtocolType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ProtocolFileName { get; set; }

        public int BaudRate { get; set; }

        public int MasterID { get; set; }

        public int SlaveID { get; set; }

        public int CompareTo(CanIndexItem other)
        {
            return this.CanChannel.CompareTo(other.CanChannel);
        }
    }

}
