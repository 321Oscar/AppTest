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

            this.metroPanelMain.Controls.Add(this.tableLayoutPanel1);

            InitStateStrip();

            cycleTimeDataGridViewTextBoxColumn.Visible = false;

            ChangeBaseColor(GetColor);
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
            hduc.ChangeColorTheme(GetColor);

            ProjectForm parent = (ProjectForm)this.MdiParent;
            vm.XcpModule = parent.XcpModule;
        }

        protected override void ModifiedSignals()
        {
            if (vm.ModifiedSignals())
                ReLoadSignal();
        }

        protected override void ReLoadSignal()
        {
            base.ReLoadSignal();

            vm.XcpModule.StartStopDAQ(0x00, (uint)this.CanChannel);
            IsGetdata = false;

            if (!vm.InitDAQ((uint)this.CanChannel))
            {
                ShowLog("DAQ 配置失败");
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
            vm.ShowSignalDetails(this.dataGridView1, e);
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
