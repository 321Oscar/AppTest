using AppTest.FormType;
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
    public partial class DBCGetForm : DataForm
    {
        DBCGetViewModel vm;
        public DBCGetForm()
        {
            InitializeComponent();
        }

        public DBCGetForm(FormType formType, ProtocolType protocolType = ProtocolType.DBC) : base(formType,protocolType)
        {
            vm = new DBCGetViewModel(this);
        }

        protected override void InitSignalUC()
        {
            //CanIndexItem canIndex = OwnerProject.CanIndex.Find(x => x.CanChannel == CanChannel);
            vm.DBCSignals = FormItem.DBCSignals;

            if (vm.DBCSignals != null && vm.DBCSignals.SignalList.Count > 0)
                vm.CurrentIDs = vm.DBCSignals.SignalList.Select(x => int.Parse(x.MessageID, System.Globalization.NumberStyles.HexNumber)).Distinct().ToList();

            metroComboBox_Signal.DataSource = null;
            metroComboBox_Signal.DataSource = vm.DBCSignals.SignalList;
            metroComboBox_Signal.DisplayMember = "SignalName";

            BindingList<DBCSignal> bs = new BindingList<DBCSignal>(vm.DBCSignals.SignalList);
            this.dataGridView1.DataSource = bs;

            InitIDAndProtocolCmd();

            if (FormType == FormType.Get)
            {
                HistoryDataUC hduc = new HistoryDataUC();
                hduc.ProjectName = this.OwnerProject.Name;
                hduc.FormName = this.Name;
                hduc.Dock = DockStyle.Fill;
                metroTabPage2.Controls.Clear();
                metroTabPage2.Controls.Add(hduc);
                hduc.ChangeColorTheme(GetColor);

            }

            vm.ModifiedGetdata += ModifiedGetdata;
            vm.ShowLog += ShowLog;
        }

        protected override void InitIDAndProtocolCmd()
        {
            CanIndexItem canIndex = OwnerProject.CanIndex.Find(x => x.CanChannel == CanChannel);
            vm.InitProtocol(canIndex.ProtocolFileName);
        }

        protected override void ModifiedGetdata(bool get)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    base.ModifiedGetdata(get);
                }));
            }
            else
            {
                base.ModifiedGetdata(get);
            }

        }

        protected override void ModifiedSignals()
        {
            vm.ModifiedSignals();
            ReLoadSignal();
        }

        protected override void DataControl()
        {
            if (vm.IsGetdata)//正在获取数据
            {
                vm.IsGetdata = false;
            }
            else
            {
                if (!USBCanManager.Instance.Exist(OwnerProject))
                {
                    LeapMessageBox.Instance.ShowInfo("CAN未打开!");
                    return;
                }
                foreach (var item in vm.DBCSignals.SignalList)
                {
                    item.StrValue = "0";
                }

                vm.IsGetdata = true;
            }
        }

        public override void OnDataRecieveEvent(object sender, CANDataRecieveEventArgs args)
        {
            vm.OnDataRecieveEvent(sender, args);
        }

        protected override void ChangeValue(object sender)
        {
            if (metroComboBox_Signal.SelectedIndex != -1)
            {
                DBCSignal selectSignal = metroComboBox_Signal.SelectedItem as DBCSignal;

                decimal oldValue = Convert.ToDecimal(selectSignal.StrValue);
                Button bt = sender as Button;
                decimal newValue;
                try
                {
                    if (bt.Name == btnAdd.Name)
                        newValue = oldValue + nudStep.Value;
                    else if (bt.Name == btnReduce.Name)
                    {
                        newValue = oldValue - nudStep.Value;
                    }
                    else if (bt.Name == btnMultip.Name)
                    {
                        newValue = oldValue * nudStep.Value;
                    }
                    else if (bt.Name == btnDivision.Name)
                    {
                        newValue = oldValue / nudStep.Value;
                    }
                    else
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    LeapMessageBox.Instance.ShowError(ex);
                    return;
                }


                selectSignal.StrValue = newValue.ToString();

                SelectedSignalChanged();

                SetOrSend();
            }
        }

        protected override void SelectedSignalChanged()
        {
            if (metroComboBox_Signal.SelectedIndex != -1)
            {
                DBCSignal selectSignal = metroComboBox_Signal.SelectedItem as DBCSignal;

                ///不能绑定数据，rollingcounter 模式下改变数据会跳变
                ///9->10 先发1，再发10
                if (this.FormType == FormType.Set)
                {
                    var threadSafeModel = new SynchronizedNotifyPropertyChanged<DBCSignal>(selectSignal, this);
                    tbCurrent.DataBindings.Clear();
                    tbCurrent.DataBindings.Add("Text", threadSafeModel, "StrValue", false, DataSourceUpdateMode.OnPropertyChanged, "0");
                }

                tbCurrent.Text = selectSignal.StrValue;

            }
        }

        protected override void ShowSignalInfo(DataGridViewCellEventArgs e)
        {
            vm.ShowSignalDetai(this.dataGridView1, e);
        }

        protected override void SetOrSend()
        {
            string log = string.Empty;
            ShowLog("");

            if (this.FormType == FormType.Set)//Set  发送数据
            {
                byte sendtype = (byte)tscbb.SelectedIndex;
                for (int i = 0; i < vm.CurrentIDs.Count; i++)
                {
                    Dictionary<BaseSignal, string> keyValues = new Dictionary<BaseSignal, string>();
                    var signals = vm.DBCSignals.SignalList.Where(x => x.MessageID == vm.CurrentIDs[i].ToString("X"));
                    foreach (var item in signals)
                    {
                        keyValues.Add(item, item.StrValue);
                    }
                    var frame = vm.DBCProtocol.BuildFrames(keyValues);
                    try
                    {
                        if (USBCanManager.Instance.Send(OwnerProject, CanChannel, sendData: frame[0], $"[{this.FormType}]{this.Name}", sendtype))
                        {
                            log += $"{frame[0].ID:X}发送成功";
                        }
                    }
                    catch (USBCANOpenException ex)
                    {
                        LeapMessageBox.Instance.ShowInfo(ex.Message);
                        break;
                    }

                }
            }
           

            if (!string.IsNullOrEmpty(log))
                ShowLog(log);

        }
    }
}
