using AppTest.FormType.Helper;
using AppTest.Helper;
using AppTest.Model;
using LPCanControl.CANInfo;
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
    public partial class XCPDAQGetForm : BaseDataForm
    {
        DAQList DAQList = new DAQList();
        XCPModule XcpModule;
        private Dictionary<int, List<byte>> recieveData;

        public XCPDAQGetForm()
        {
            InitializeComponent();

            base.MDIModeVisible = false;

            this.metroPanelMain.Controls.Add(this.tableLayoutPanel1);

            InitStateStrip();

            cycleTimeDataGridViewTextBoxColumn.Visible = false;
        }

        protected override void InitSignalUC()
        {
            BindingList<XCPSignal> bs = new BindingList<XCPSignal>(xCPSingals.xCPSignalList);
            this.dataGridView1.DataSource = bs;

            HistoryDataUC hduc = new HistoryDataUC();
            hduc.ProjectName = this.OwnerProject.Name;
            hduc.FormName = this.Name;
            hduc.Dock = DockStyle.Fill;
            metroTabPage2.Controls.Clear();
            metroTabPage2.Controls.Add(hduc);
            //hduc.ChangeColorTheme(ScopeColor);

            ProjectForm parent = (ProjectForm)this.MdiParent;
            this.XcpModule = parent.XcpModule;
        }

        protected override void InitStateStrip()
        {
            base.InitStateStrip();
            statusStrip1.Location = new Point(0, this.Size.Height);
        }

        protected override void OnDataRecieveEvent(object sender, CANDataRecieveEventArgs args)
        {
            var rx_mails = args.can_msgs;
            if (null == rx_mails)
                throw new Exception("接收数据错误。");

            foreach (var item in rx_mails)
            {
                //判断ID
                if (item.cid != XcpModule.Slaveid)
                {
                    continue;
                }
                //判断是否是DTO以及DTO　ID
                if (XCPHelper.CTOCheck(item.b[0]))
                    continue;

                //拼接数据报文
                foreach (var daq in DAQList.DAQs)
                {
                    //PID 在该DAQ中
                    if (item.b[0] == daq.ODTs[0].ID)//第一帧
                    {
                        var data = new byte[daq.ODTs[0].UsedSize + 2];//将时间戳也获取
                        Array.Copy(item.b, 1, data, 0, daq.ODTs[0].UsedSize + 2);
                        recieveData[daq.Event_Channel_Number].AddRange(data);
                    }

                    if (item.b[0] == daq.ODTs[0].ID + daq.ODTs.Count - 1)//最后一帧
                    {
                        if (item.b[0] != daq.ODTs[0].ID)
                        {
                            //添加数据
                            byte size = daq.ODTs.Find(x => x.ID == item.b[0]).UsedSize;
                            var data = new byte[size];
                            Array.Copy(item.b, 1, data, 0, size);
                            recieveData[daq.Event_Channel_Number].AddRange(data);
                        }
                        //解析数据
                        ParseResponeToXCPSignal(recieveData[daq.Event_Channel_Number], daq.Event_Channel_Number);

                        //清空数据
                        recieveData[daq.Event_Channel_Number].Clear();


                    }
                    else if (item.b[0] > daq.ODTs[0].ID && item.b[0] < daq.ODTs[0].ID + daq.ODTs.Count - 1)
                    {
                        byte size = daq.ODTs.Find(x => x.ID == item.b[0]).UsedSize;
                        var data = new byte[size];
                        Array.Copy(item.b, 1, data, 0, size);
                        recieveData[daq.Event_Channel_Number].AddRange(data);
                    }
                }
            }
        }

        protected override void ModifiedGetdata(bool get)
        {
            metroButtonStart.Text = !get ? "Start" : "Stop";
            ShowLog("");
            RegisterOrUnRegisterDataRecieve(get);
        }

       
        private void metroButtonSetDaq_Click(object sender, EventArgs e)
        {
#if DEBUG
            recieveData = new Dictionary<int, List<byte>>();
            InitDAQ();
#else
            int daqodtID = 0;
            DAQList.Clear();
            
            /// 一个信号为一个ODT entry
            this.xCPSingals.xCPSignalList.Sort((x, y) => { return x.EventID.CompareTo(y.EventID); });
            foreach (var signal in this.xCPSingals.xCPSignalList)
            { 
                signal.StartIndex = 0;
                DAQ dAQ = DAQList.Find(x => x.EventName == signal.EventName);// new DAQ();
                if (dAQ == null)
                {
                    dAQ = new DAQ();
                    DAQList.Add(dAQ);
                    dAQ.EventName = signal.EventName;
                    dAQ.Event_Channel_Number = (short)signal.EventID;
                    recieveData.Add(dAQ.Event_Channel_Number, new List<byte>());
                }
                    
                if (dAQ.ODTs == null)
                    dAQ.ODTs = new List<ODT>();
                ODT oDT;
                if (dAQ.ODTs.Count == 0)
                {
                    oDT = new ODT();
                    oDT.MaxSize = 5;
                    oDT.ID = (byte)daqodtID;
                    dAQ.ODTs.Add(oDT);
                    daqodtID++;
                    signal.StartIndex = 0;
                }
                else
                {
                    oDT = dAQ.ODTs[dAQ.ODTs.Count - 1];
                    foreach (var item in dAQ.ODTs)
                    {
                        signal.StartIndex += item.UsedSize;
                    }
                }

                if(oDT.ODTEntries == null)
                    oDT.ODTEntries = new List<ODTEntry>();

                ODTEntry oDTEntry = new ODTEntry();
                int.TryParse(signal.ECUAddress, System.Globalization.NumberStyles.HexNumber, null, out int Address);

                if (oDT.AvailableSize >= (byte)signal.Length)
                {
                    oDTEntry.Size = (byte)signal.Length;
                    oDTEntry.AddressExtension = (byte)signal.AddressExtension;
                    oDTEntry.Address = Address;
                    oDT.ODTEntries.Add(oDTEntry);
                }
                else//剩余长度不够一个信号长度，新建一个ODT
                {
                    if (oDT.AvailableSize != 0)
                    {
                        oDTEntry.Size = oDT.AvailableSize;
                        oDTEntry.AddressExtension = (byte)signal.AddressExtension;
                        oDTEntry.Address = Address;
                        oDT.ODTEntries.Add(oDTEntry);
                    }

                    //新建一个ODT
                    ODT oDT1 = new ODT();
                    oDT1.ID = (byte)daqodtID;
                    dAQ.ODTs.Add(oDT1);
                    daqodtID++;
                    oDT1.ODTEntries = new List<ODTEntry>();
                    ODTEntry oDTEntry1 = new ODTEntry();
                    oDTEntry1.Size = (byte)(signal.Length - oDTEntry.Size);
                    oDTEntry1.AddressExtension = (byte)signal.AddressExtension;
                    oDTEntry1.Address = Address + oDTEntry.Size;
                    oDT1.ODTEntries.Add(oDTEntry1);
                }
                
                signal.DAQID = daqodtID;
                //dAQ.ODTs.Add()
            }
#endif
            LeapMessageBox.Instance.ShowInfo($"DAQ 配置{(XcpModule.SetDAQ(DAQList.DAQs,(uint)this.CanChannel) ? "成功" : "失败")}");

        }

        private void InitDAQ()
        {
            DAQList.Clear();
            //根据事件ID排序
            this.xCPSingals.xCPSignalList.Sort((x, y) => { return x.EventID.CompareTo(y.EventID); });
            foreach (var signal in this.xCPSingals.xCPSignalList)
            {
                DAQList.AddSignal(signal);
            }

            foreach (var daq in DAQList.DAQs)
            {
                recieveData.Add(daq.Event_Channel_Number, new List<byte>());
            }
        }

        private void metroButtonStart_Click(object sender, EventArgs e)
        {
            //ProjectForm parent = (ProjectForm)this.MdiParent;
            if (IsGetdata)//正在获取数据
            {
                XcpModule.StartStopDAQ(0x00, (uint)this.CanChannel);
                IsGetdata = false;
            }
            else
            {
                if (!USBCanManager.Instance.Exist(OwnerProject))
                {
                    LeapMessageBox.Instance.ShowInfo("CAN未打开!");
                    return;
                }
                foreach (var item in xCPSingals.xCPSignalList)
                {
                    item.StrValue = "0";
                }

                recieveData = new Dictionary<int, List<byte>>();
                InitDAQ();

                var initDaqTrue = XcpModule.SetDAQ(DAQList.DAQs, (uint)this.CanChannel);
                if (!initDaqTrue)
                {
                    LeapMessageBox.Instance.ShowInfo($"DAQ 配置{(initDaqTrue ? "成功" : "失败")}");
                    return;
                }

                XcpModule.StartStopDAQ(0x01, (uint)this.CanChannel);
                IsGetdata = true;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowSignalInfo(e);
        }

        protected virtual void ShowSignalInfo(DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                var signal = dataGridView1.Rows[e.RowIndex].DataBoundItem as XCPSignal;
                SignalItemForm<XCPSignal> siF = new SignalItemForm<XCPSignal>(signal, signal.SignalName);
                siF.Show();
            }
            catch (Exception ex)
            {
                ShowLog(ex.Message);
            }
        }
    }
}
