using AppTest.FormType.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace AppTest.ProjectClass
{
    public static class RootHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Json路径</param>
        /// <param name="oldRoot">root</param>
        /// <returns></returns>
        public static Root InitRootByJson(string path,Root oldRoot = null)
        {
            string jsonStr;

            try
            {
                FileStream fs = null;
                StreamReader sr = null;

                try
                {
                    fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    sr = new StreamReader(fs, System.Text.Encoding.UTF8);

                    jsonStr = sr.ReadToEnd();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (sr != null)
                    {
                        sr.Close();
                    }
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }

                if (oldRoot == null)
                {
                    oldRoot = JsonConvert.DeserializeObject<Root>(jsonStr);
                }
                else
                {
                    Root rImport = JsonConvert.DeserializeObject<Root>(jsonStr);
                    foreach (var project in rImport.project)
                    {
                        if (oldRoot.project.Find(x => x.Name == project.Name) == null)
                        {
                            oldRoot.project.Add(project);
                        }
                    }
                }

                //LoadTreeView();
            }
            catch (Exception ex)
            {
                LeapMessageBox.Instance.ShowError(ex.Message);
            }

            return oldRoot;
        }
    } 
    

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
        /// 
        /// </summary>
        public int ProtocolType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ProtocolFileName { get; set; }

        public int BaudRate { get; set; }

        public int CompareTo(CanIndexItem other)
        {
            return this.CanChannel.CompareTo(other.CanChannel);
        }
    }

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
        public new int ByteOrder { get; set; }
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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StrValue)));
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
        public int ByteOrder { get => byteOrder; set => byteOrder = value; }
        /// <summary>
        /// 自定义名称
        /// </summary>
        public string CustomName { get => customName; set => customName = value; }

        public event PropertyChangedEventHandler PropertyChanged;
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
    }

    public class SignalEqualityComarer : IEqualityComparer<DBCSignal>
    {
        public bool Equals(DBCSignal x, DBCSignal y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else if (x.SignalName == y.SignalName && x.MessageID == y.MessageID)
                return true;
            else
                return false;
        }

        public int GetHashCode(DBCSignal obj)
        {
            return (obj.MessageID+obj.SignalName).GetHashCode();
        }
    }

    public class Singals
    {
        /// <summary>
        /// 
        /// </summary>
        public List<DBCSignal> Signal { get; set; }

        public Singals()
        {
            Signal = new List<DBCSignal>();
        }
    }

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
        public Singals Singals { get; set; }

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
            Singals = newItem.Singals;
        }
    }

    public class ProjectItem 
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int DeviceType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int DeviceIndex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<CanIndexItem> CanIndex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<FormItem> Form { get; set; }

        public ProjectItem()
        {
            Form = new List<FormItem>();
            CanIndex = new List<CanIndexItem>();    
        }

        public void Copy(ProjectItem newItem)
        {
            this.Name = newItem.Name;
            DeviceType = newItem.DeviceType;
            DeviceIndex = newItem.DeviceIndex;
            CanIndex = newItem.CanIndex;
            Form = newItem.Form;
        }
    }

    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ProjectItem> project { get; set; }

        public Root()
        {
            this.project = new List<ProjectItem>();
        }
    }

    /// <summary>
    /// 信号特性，根据特性生成控件 TextBox、CheckBox...
    /// </summary>
    [AttributeUsage(AttributeTargets.Class |
        AttributeTargets.Constructor|
        AttributeTargets.Field|
        AttributeTargets.Method|
        AttributeTargets.Property,
        AllowMultiple =true)]
    public class SignalAttribute : System.Attribute
    {
        private string description;
        /// <summary>
        /// textBox
        /// </summary>
        /// <param name="description"></param>
        public SignalAttribute(string description)
        {
            this.description = description;
        }

        /// <summary>
        /// NumricBox
        /// </summary>
        /// <param name="description">描述</param>
        /// <param name="min">最小值，数值类型</param>
        /// <param name="max">最大值，数值类型</param>
        public SignalAttribute(string description, double min = 0, double max = 0)
        {
            this.description = description;
            this.type = "int";
            this.min = min;
            this.max = max;
        }

        private int[] enumKey;
        private string[] enumString;

        /// <summary>
        /// 枚举类型
        /// </summary>
        /// <param name="description"></param>
        /// <param name="enumKey">枚举值</param>
        /// <param name="enumString">枚举显示值</param>
        public SignalAttribute(string description, int[] enumKey, string[] enumString)
        {
            this.enumKey = enumKey;
            this.enumString = enumString;
            this.description = description;
            this.type = "enum";
        }

        private string type;
        private double min;
        private double max;
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get => description;  }
        /// <summary>
        /// 数据类型
        /// </summary>
        public string Type { get => type;  }
        /// <summary>
        /// 最小值，数值类型
        /// </summary>
        public double Min { get => min;  }
        /// <summary>
        /// 最大值，数值类型
        /// </summary>
        public double Max { get => max;  }
        /// <summary>
        /// 枚举类型
        /// </summary>
        public int[] EnumKey { get => enumKey; }
        public string[] EnumString { get => enumString;}
    }

}
