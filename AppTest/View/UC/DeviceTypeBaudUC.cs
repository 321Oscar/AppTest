using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppTest.FormType
{
    /// <summary>
    /// 波特率设置UC
    /// </summary>
    public partial class DeviceTypeBaudUC : UserControl
    {
        /// <summary>
        /// 波特率
        /// </summary>
        public byte[] Baud = new byte[2];
        public DeviceTypeBaudUC()
        {
            InitializeComponent();
        }

        public void InitUC(int type)
        {
            switch ((DeviceType)type)
            {
                case DeviceType.VCI_USBCAN_2E_U:
                    { 
                    ///change value-key 
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
