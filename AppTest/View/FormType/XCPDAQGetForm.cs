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
        protected readonly Color GetColor = Color.FromArgb(0, 174, 219);
        //DAQList DAQList = new DAQList();
        //XCPModule XcpModule;
        //private Dictionary<int, List<byte>> recieveData;
        readonly XCPDAQGetViewModel vm;

        public XCPDAQGetForm()
        {
            InitializeComponent();

            vm = new XCPDAQGetViewModel(this);

            base.MDIModeVisible = false;

            InitStateStrip();

            //cycleTimeDataGridViewTextBoxColumn.Visible = false;

            ChangeBaseColor(GetColor);

            LoadDatGridViewContext();
        }

        protected override void InitSignalUC()
        {
            vm.XCPSignals = this.FormItem.XCPSingals;
            BindingList<XCPSignal> bs = new BindingList<XCPSignal>(vm.XCPSignals.xCPSignalList);
            this.dataGridView1.DataSource = bs;

            HistoryDataView = new HistoryDataUC();
            HistoryDataView.ProjectName = this.OwnerProject.Name;
            HistoryDataView.FormName = this.Name;
            HistoryDataView.Dock = DockStyle.Fill;
            metroTabPage2.Controls.Clear();
            metroTabPage2.Controls.Add(HistoryDataView);
            HistoryDataView.ChangeColorTheme(GetColor);

            ProjectForm parent = (ProjectForm)this.MdiParent;
            vm.XcpModule = parent.XcpModule;
        }

        protected override void ModifiedSignals()
        {
            if (vm.ModifiedSignals())
            {
                ReLoadSignal();
            }
                
        }

        protected override void ReLoadSignal()
        {
            base.ReLoadSignal();
            try
            {
                if (IsGetdata)
                {
                    vm.XcpModule.StartStopDAQ(XCPModule.STOPALLDAQ, (uint)this.CanChannel);
                    IsGetdata = false;
                }

                if (!vm.InitDAQ((uint)this.CanChannel))
                {
                    ShowLog("DAQ 配置失败");
                }
                else
                {
                    vm.XcpModule.StartStopDAQ(XCPModule.STARTSELECTEDDAQ, (uint)this.CanChannel);
                    IsGetdata = true;
                }
            }
            catch (USBCANOpenException canerr)
            {
                ShowLog(canerr.Message,LPLogLevel.Warn);
            }
            catch (Exception err)
            {
                ShowLog(err.Message, LPLogLevel.Error);
            }
        }

        protected override void InitStateStrip()
        {
            base.InitStateStrip();
            statusStrip1.Location = new Point(0, this.Size.Height);
        }

        protected override void ChangeBaseColor(Color c)
        {
            base.ChangeBaseColor(c);
            metroButtonStart.Style = MetroFramework.MetroColorStyle.Blue;
            metroTabControl1.Style = MetroFramework.MetroColorStyle.Blue;
            this.Style = MetroFramework.MetroColorStyle.Blue;
            this.dataGridView1.DefaultCellStyle.SelectionBackColor = GetColor;
        }
         
        public override void OnDataReceiveEvent(object sender, CANDataReceiveEventArgs args)
        {
            vm.OnDataRecieveEvent(sender, args);
        }

        protected override void ModifiedGetdata(bool get)
        {
            metroButtonStart.Text = !get ? "Start" : "Stop";
            RegisterOrUnRegisterDataRecieve(get);
        }

        protected void LoadDatGridViewContext()
        {
            foreach (DataGridViewColumn item in dataGridView1.Columns)
            {
                if (string.IsNullOrEmpty(item.DataPropertyName))
                    continue;
                ToolStripMenuItem tsb = new ToolStripMenuItem();
                tsb.Checked = item.Visible;
                tsb.Click += Tsb_Click;
                tsb.Text = item.HeaderText;
                tsb.Name = item.Index.ToString();
                contextMenuStrip1.Items.Add(tsb);
            }
            ToolStripMenuItem tsbShowInfo = new ToolStripMenuItem();
            //tsb.Checked = item.Visible;
            tsbShowInfo.Click += TsbShowInfo_Click; ;
            tsbShowInfo.Text = "信号详细";
            tsbShowInfo.Name = "SignalDetails";
            contextMenuStrip1.Items.Add(new ToolStripSeparator());
            contextMenuStrip1.Items.Add(tsbShowInfo);
        }

        private void TsbShowInfo_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.CurrentRow == null)
                return;
            vm.ShowSignalDetails(dataGridView1.CurrentRow);
        }

        private void Tsb_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsb = sender as ToolStripMenuItem;
            int idx = int.Parse(tsb.Name);
            dataGridView1.Columns[idx].Visible = !dataGridView1.Columns[idx].Visible;
            tsb.Checked = dataGridView1.Columns[idx].Visible;
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
                    ShowLog("CAN未打开!", LPLogLevel.Warn);
                    return;
                }

                if (!vm.InitDAQ((uint)this.CanChannel))
                {
                    ShowLog("DAQ配置失败!", LPLogLevel.Error);
                    return;
                }

                vm.XcpModule.StartStopDAQ(0x01, (uint)this.CanChannel);
                IsGetdata = true;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            vm.ShowSignalDetails(this.dataGridView1.Rows[e.RowIndex]);
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].DataPropertyName == "StrValue")
            {
                try
                {
                    var v = Convert.ToDouble(e.Value);
                    var signal = dataGridView1.Rows[e.RowIndex].DataBoundItem as BaseSignal;
                    if (v > signal.Maximum || v < signal.Minimum)
                    {
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    }
                    else
                    {
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
                catch
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                }

            }
        }
    }
}
