using AppTest.FormType.Helper;
using AppTest.Helper;
using AppTest.Model;
using AppTest.ViewModel;
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
        //DAQList DAQList = new DAQList();
        //XCPModule XcpModule;
        //private Dictionary<int, List<byte>> recieveData;
        readonly XCPDAQGetViewModel vm;

        public XCPDAQGetForm()
        {
            InitializeComponent();

            vm = new XCPDAQGetViewModel(this);

            base.MDIModeVisible = false;

            this.metroPanelMain.Controls.Add(this.tableLayoutPanel1);

            InitStateStrip();

            cycleTimeDataGridViewTextBoxColumn.Visible = false;
        }

        protected override void InitSignalUC()
        {
            vm.XCPSignals = this.FormItem.XCPSingals;
            BindingList<XCPSignal> bs = new BindingList<XCPSignal>(vm.XCPSignals.xCPSignalList);
            this.dataGridView1.DataSource = bs;

            HistoryDataUC hduc = new HistoryDataUC();
            hduc.ProjectName = this.OwnerProject.Name;
            hduc.FormName = this.Name;
            hduc.Dock = DockStyle.Fill;
            metroTabPage2.Controls.Clear();
            metroTabPage2.Controls.Add(hduc);
            //hduc.ChangeColorTheme(ScopeColor);

            ProjectForm parent = (ProjectForm)this.MdiParent;
            vm.XcpModule = parent.XcpModule;
        }

        protected override void ModifiedSignals()
        {
            vm.ModifiedSignals();
            ReLoadSignal();
        }

        protected override void ReLoadSignal()
        {
            base.ReLoadSignal();

            vm.XcpModule.StartStopDAQ(0x00, (uint)this.CanChannel);
            IsGetdata = false;

            if (!vm.InitDAQ((uint)this.CanChannel))
            {
                
            }
            else
            {
                vm.XcpModule.StartStopDAQ(0x01, (uint)this.CanChannel);
                IsGetdata = true;
            }
        }

        protected override void InitStateStrip()
        {
            base.InitStateStrip();
            statusStrip1.Location = new Point(0, this.Size.Height);
        }

        public override void OnDataRecieveEvent(object sender, CANDataRecieveEventArgs args)
        {
            vm.OnDataRecieveEvent(sender, args);
        }

        protected override void ModifiedGetdata(bool get)
        {
            metroButtonStart.Text = !get ? "Start" : "Stop";
            ShowLog("");
            RegisterOrUnRegisterDataRecieve(get);
        }

        private void metroButtonStart_Click(object sender, EventArgs e)
        {
            if (IsGetdata)//正在获取数据
            {
                vm.XcpModule.StartStopDAQ(0x00, (uint)this.CanChannel);
                IsGetdata = false;
            }
            else
            {
                if (!USBCanManager.Instance.Exist(OwnerProject))
                {
                    LeapMessageBox.Instance.ShowInfo("CAN未打开!");
                    return;
                }

                if (!vm.InitDAQ((uint)this.CanChannel))
                {
                    LeapMessageBox.Instance.ShowInfo("DAQ配置失败!");
                    return;
                }

                vm.XcpModule.StartStopDAQ(0x01, (uint)this.CanChannel);
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
