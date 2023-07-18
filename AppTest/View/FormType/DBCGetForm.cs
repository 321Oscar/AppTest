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
            vm.VMModifiedGetdata += ModifiedGetdata;
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
            base.metroGrid1.DataSource = bs;

            InitIDAndProtocolCmd();

            if (FormType == FormType.Get)
            {
                HistoryDataView = new HistoryDataUC();
                HistoryDataView.ProjectName = this.OwnerProject.Name;
                HistoryDataView.FormName = this.Name;
                HistoryDataView.Dock = DockStyle.Fill;
                HistoryDataView.StartTime = this.SaveDataStartTime;
                metroTabPage2.Controls.Clear();
                metroTabPage2.Controls.Add(HistoryDataView);
                HistoryDataView.ChangeColorTheme(GetColor);
            }
            base.InitSignalUC();
        }

        protected override void BaseDataForm_OnSaveTimelChange(DateTime signalValue)
        {
            base.BaseDataForm_OnSaveTimelChange(signalValue);
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
            if (vm.ModifiedSignals())
                ReLoadSignal();
        }

        protected override void DataControl()
        {
            if (vm.VMIsGetdata)//正在获取数据
            {
                vm.VMIsGetdata = false;
                ShowLog("已关闭获取数据");
            }
            else
            {
                string log = string.Empty;
                if (!USBCanManager.Instance.Exist(OwnerProject) || !USBCanManager.Instance.SendTest(OwnerProject,ref log))
                {
                    ShowLog($"CAN未打开!{log}", LPLogLevel.Warn);
                    return;
                }
                foreach (var item in vm.DBCSignals.SignalList)
                {
                    item.StrValue = "0";
                }

                vm.VMIsGetdata = true;
                ShowLog("已启动获取数据");
            }
        }

        public override void OnDataReceiveEvent(object sender, CANDataReceiveEventArgs args)
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

        protected override void ShowSignalInfo(DataGridViewRow row)
        {
            vm.ShowSignalDetails(row);
        }

        protected override void SetOrSend()
        {
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
                            ShowLog( $"{frame[0].ID:X}发送成功:{frame[0]}",LPLogLevel.Debug);
                        }
                        else
                        {
                            ShowLog($"{frame[0].ID:X}发送失败:{frame[0]}", LPLogLevel.Warn);
                        }
                    }
                    catch (USBCANOpenException ex)
                    {
                        ShowLog(ex.Message,LPLogLevel.Warn);
                        break;
                    }
                    catch(Exception ex)
                    {
                        ShowLog(ex.Message, LPLogLevel.Error);
                        break;
                    }

                }
            }
        }
    }
}
