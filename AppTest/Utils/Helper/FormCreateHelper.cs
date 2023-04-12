using AppTest.Helper;
using AppTest.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTest.FormType
{
    public class BaseFormEqualityComparer : IEqualityComparer<BaseDataForm>
    {
        public bool Equals(BaseDataForm x, BaseDataForm y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(BaseDataForm obj)
        {
            return obj.Name.GetHashCode();
        }
    }
    internal class FormCreateHelper
    {

        public static BaseDataForm CreateForm(FormItem formItem, ProjectItem owner)
        {
            BaseDataForm userForm = null;

            switch ((FormType)formItem.FormType)
            {
                case FormType.Scope:
                    //userForm = new ZedGraphForm();
                    //userForm.Icon = Icon.FromHandle(Global.IconHandle_Measure);
                    if (owner.CanIndex[0].ProtocolType == (int)ProtocolType.DBC)
                        userForm = new MeasureForm();
                    else if (owner.CanIndex[0].ProtocolType == (int)ProtocolType.XCP)
                        userForm = new XCPScopeForm();
                    else
                        throw new Exception("不支持的协议");
                    break;
                case FormType.Get:
                    //userForm = new GetForm();
                    //userForm.Icon = Icon.FromHandle(Global.IconHandle_Get);
                    if (owner.CanIndex[0].ProtocolType == (int)ProtocolType.DBC)
                        userForm = new DBCGetForm(FormType.Get);
                    else if (owner.CanIndex[0].ProtocolType == (int)ProtocolType.XCP)
                        userForm = new XCPGetForm(FormType.Get);
                    else
                        throw new Exception("不支持的协议");
                    break;
                case FormType.Set:
                    //userForm = new SetForm();
                    // userForm = new BaseDataForm();
                    //userForm.Icon = Icon.FromHandle(Global.IconHandle_Set);
                    if (owner.CanIndex[0].ProtocolType == (int)ProtocolType.DBC)
                        userForm = new DBCGetForm(FormType.Set);
                    else if (owner.CanIndex[0].ProtocolType == (int)ProtocolType.XCP)
                        userForm = new XCPGetForm(FormType.Set);
                    else
                        throw new Exception("不支持的协议");
                    break;
                case FormType.RollingCounter:
                    //Rollingcounter还是用原来的，dataform 发送间隔时间不满足10ms内
                    //userForm = new DataForm(FormType.RollingCounter);
                    //userForm.Icon = Icon.FromHandle(Global.IconHandle_RL);
                    userForm = new RollingCounterForm();
                    break;
                case FormType.XCP_DAQ:
                    userForm = new XCPDAQGetForm();
                    break;
                case FormType.XCP_DAQScope:
                    userForm = new XCPDAQScopeForm();
                    break;
            }
            if (userForm == null)
                return null;
            //userForm.TopLevel = false;
            userForm.Name = userForm.Text = formItem.Name;
            userForm.CanChannel = formItem.CanChannel;
            userForm.Signals = formItem.DBCSignals;
            //userForm.xCPSingals = formItem.XCPSingals;
            userForm.OwnerProject = owner;
            userForm.FormItem = formItem;
            //userForm.Title = formItem.Name;

            CanIndexItem canIndex = owner.CanIndex.Find(x => x.CanChannel == formItem.CanChannel);
            if(canIndex == null)
            {
                throw new Exception("Can通道配置错误");
            }
            else
            {
                switch ((ProtocolType)canIndex.ProtocolType)
                {
                    case ProtocolType.DBC:
                        userForm.ProtocolCommand = "AppTest.ProtocolLib.DBCProtocol";
                        break;
                    //case ProtocolType.Excel:
                    //    throw new Exception("Excel协议未实现");
                }
            }

            return userForm;
        }
    }

    public enum FormType
    {
        /// <summary>
        /// 示波   
        /// </summary>
        Scope,
        
        Get,

        Set,

        RollingCounter,
        XCP_DAQ,
        XCP_DAQScope
        //XCPGet,
        //XCPSet
    }

    public enum ProtocolType
    {
        /// <summary>
        /// DBC文件
        /// </summary>
        [Description("*.dbc")]
        DBC,
        [Description("*.a2l")]
        XCP,
        //[Description("*.xls")]
        //Excel
    }

    public enum BaudRateType
    {
        [Description("1000Kbps")]
        Kbps1000 = 0x060003,
        [Description("800Kbps")]
        Kbps800 = 0x060004,
        [Description("500Kbps")]
        Kbps500 = 0x060007,
        [Description("250Kbps")]
        Kbps250 = 0x1C0008,
        [Description("125Kbps")]
        Kbps125 = 0x1C0011,
        [Description("100Kbps")]
        Kbps100 = 0x160023,
        [Description("50Kbps")]
        Kbps50 = 0x1C002C,
        [Description("20Kbps")]
        Kbps20 = 0x1600B3,
        [Description("10Kbps")]
        Kbps10 = 0x1C00E0,
        [Description("5Kbps")]
        Kbps5 = 0x1C01C1,

    }

    public enum DeviceType
    {
        //VCI_PCI5121 = 1,
        //VCI_PCI9810 = 2,
        VCI_USBCAN1 = 3,
        //VCI_USBCAN2 = 4,
        //VCI_USBCAN2A = 4,
        //VCI_PCI9820 = 5,
        //VCI_CAN232 = 6,
        //VCI_PCI5110 = 7,
        //VCI_CANLITE = 8,
        //VCI_ISA9620 = 9,
        //VCI_ISA5420 = 10,
        //VCI_PC104CAN = 11,
        //VCI_CANETUDP = 12,
        //VCI_CANETE = 12,
        //VCI_DNP9810 = 13,
        //VCI_PCI9840 = 14,
        //VCI_PC104CAN2 = 15,
        //VCI_PCI9820I = 16,
        //VCI_CANETTCP = 17,
        //VCI_PEC9920 = 18,
        //VCI_PCI5010U = 19,
        //VCI_USBCAN_E_U = 20,
        VCI_USBCAN_2E_U = 21,
        //VCI_PCI5020U = 22,
        //VCI_EG20T_CAN = 23
        USBCANFD_200U = 41,
    }

    public enum CANSendType
    {
        /// <summary>
        /// 正常发送
        /// </summary>
        Normal,
        /// <summary>
        /// 单次发送
        /// </summary>
        Single,
        /// <summary>
        /// 自发自收
        /// </summary>
        Self,
        /// <summary>
        /// 单次自发自收
        /// </summary>
        SingleSelf
    }
}
