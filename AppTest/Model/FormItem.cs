using System;

namespace AppTest.Model
{
    public class FormItem : IComparable<FormItem>
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int FormType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int CanChannel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DBCSignals DBCSignals { get; set; }

        public XCPSignals XCPSingals { get; set; }

        public DAQSignals DAQSignals { get; set; }

        /// <summary>
        /// 窗口是否打开
        /// </summary>
        public bool IsOpen { get; set; }
        /// <summary>
        /// 窗口起始位置X
        /// </summary>
        public int LocationX { get; set; }
        /// <summary>
        /// 窗口起始位置Y
        /// </summary>
        public int LocationY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int CompareTo(FormItem other)
        {
            return this.FormType.CompareTo(other.FormType);
        }

        public void Copy(FormItem newItem)
        {
            this.Name = newItem.Name;
            FormType = newItem.FormType;
            CanChannel = newItem.CanChannel;
            DBCSignals = newItem.DBCSignals;
        }
    }

}
