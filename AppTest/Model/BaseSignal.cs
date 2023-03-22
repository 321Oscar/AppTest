using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AppTest.Model
{
    public class BaseSignal : INotifyPropertyChanged, IComparable<BaseSignal>
    {
        protected string name;
        private string strValue = "0";
        private int valueType;
        private int length;
        private int byteOrder;
        private string unit;
        private string comment;
        private string customName;
        private double minimum;
        private double max;
        private bool whetherSendorGet = true;
        /// <summary>
        /// 信号名称
        /// </summary>
        public string SignalName { get => name; set => name = value; }
        public string Unit { get => unit; set => unit = value; }
        public string Comment { get => comment; set => comment = value; }
        public double Minimum { get => minimum; set => minimum = value; }
        public double Maximum { get => max; set => max = value; }
        public string StrValue
        {
            get { return this.strValue; }
            set
            {
                if (value != strValue)
                {
                    strValue = value;
                    NotifyPropertyChanged();
                }
            }
        }
       
        public int ValueType { get => valueType; set => valueType = value; }
        [SignalAttribute("长度", min: 0, max: 16)]
        public int Length { get => length; set => length = value; }
        /// <summary>
        /// 大小端：Intel 小端格式：地址的增长顺序与值的增长顺序相同
        /// </summary>
        [Signal("1:Intel;0:Motorola", new int[2] { 0, 1 }, new string[] { "Motorola", "Intel" })]
        public int ByteOrder_int { get => byteOrder; set => byteOrder = value; }
        /// <summary>
        /// 自定义名称
        /// </summary>
        public string CustomName { get => customName; set => customName = value; }

        public bool WhetherSendOrGet {
            get => whetherSendorGet;
            set 
            {
                if (value != whetherSendorGet) 
                {
                    whetherSendorGet = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;
            if(SynchronizationContext.Current == null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            if (handler != null && SynchronizationContext.Current != null)
                SynchronizationContext.Current.Post(_ => handler(this, new PropertyChangedEventArgs(propertyName)), null);
        }
        #region 比较
        public int CompareTo(BaseSignal other)
        {
            return this.SignalName.CompareTo(other.SignalName);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else if (this.GetType() != obj.GetType())
                return false;
            else
                return ((BaseSignal)obj).SignalName == this.SignalName;
        }

        public override int GetHashCode()
        {
            return this.SignalName.GetHashCode();
        }
        #endregion
        public override string ToString()
        {
            return string.IsNullOrEmpty(CustomName) ? SignalName : CustomName;
        }

        public static TChild AutoCopy<TParent, TChild>(TParent parent) where TChild : TParent, new()
        {
            TChild child = new TChild();
            var ParentType = typeof(TParent);
            var Properties = ParentType.GetProperties();
            foreach (var Propertie in Properties)
            {
                //循环遍历属性
                if (Propertie.CanRead && Propertie.CanWrite)
                {
                    //进行属性拷贝
                    Propertie.SetValue(child, Propertie.GetValue(parent, null), null);
                }
            }
            return child;
        }

        public static List<TChild> AutoCopyList<TParent, TChild>(List<TParent> parent) where TChild : TParent, new()
        {
            List<TChild> children = new List<TChild>();
            foreach (var item in parent)
            {
                var child = AutoCopy<TParent, TChild>(item);
                children.Add(child);
            }

            return children;
        }
    }

}
