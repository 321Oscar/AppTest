using System;
using System.Collections.Generic;

namespace AppTest.Model
{
    public class FormItem : IComparable<FormItem>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 类型 <see cref="AppTest.FormType.FormType"/>
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
        /// <summary>
        /// Get/Set 窗口显示的列索引
        /// </summary>
        public List<int> ShowColumnIndexes { get; set; }

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
