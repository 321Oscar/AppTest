using AppTest.Helper;
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
    public partial class AutoSendRollingCounterForm : BaseDataForm
    {
        public AutoSendRollingCounterForm()
        {
            InitializeComponent();
        }

        private byte Eop_rollcnt = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            int txid = 0xEA;
            byte[] txdata = new byte[8];
            txdata[1] = 0;
            txdata[2] = 0;
            txdata[3] = 0;
            txdata[4] = 0;
            txdata[5] = 0;
            Eop_rollcnt++;
            if (Eop_rollcnt > 15)
            {
                Eop_rollcnt = 0;
            }
            txdata[6] = (byte)(Eop_rollcnt << 4);

            byte crc = 0;
            for (UInt16 i = 0; i < 7; i++)
            {
                crc = (byte)(crc + txdata[i]);
            }
            crc = (byte)(crc ^ 0xff);
            txdata[7] = crc;

            byte[] txDataEB = new byte[8];
            txDataEB[1] = 0;
            txDataEB[2] = 0;
            txDataEB[3] = 0;
            txDataEB[4] = 0;
            txDataEB[5] = 0;
            txDataEB[6] = 0;
            txDataEB[7] = (byte)(Eop_rollcnt);
            CAN_msg_byte[] data = new CAN_msg_byte[] { new CAN_msg_byte(txid, txdata),new CAN_msg_byte(0xEB, txDataEB) };
            if (USBCanManager.Instance.Send(OwnerProject, sendData:data[0]))
            {
                //groupbox.Text = "ID:" + groupbox.Name + $"--{DateTime.Now}--已发送";
                ////item.Value.SetStatus("已发送", color: Color.Black);
            }
            else
            {
                //groupbox.Text = "ID:" + groupbox.Name + $"--{DateTime.Now}--发送失败";
                //// item.Value.SetStatus("发送失败", color: Color.Red);
            }
            if(USBCanManager.Instance.Send(OwnerProject, sendData: data[1]))
            {

            }
            //CAN_msg_byte[] data = new CAN_msg_byte[] { new CAN_msg_byte(txid, txdata) };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = !timer1.Enabled;
        }
    }
}
