﻿using AppTest.FormType.Helper;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppTest.FormType
{
    public partial class XCPGetForm : DataForm
    {
        XCPPollingViewModel vm;

        public XCPGetForm():base()
        {
            InitializeComponent();
        }

        public XCPGetForm(FormType formType) : base(formType, ProtocolType.XCP)
        {
            vm = new XCPPollingViewModel(this);
            vm.VMModifiedGetdata += ModifiedGetdata;
            //vm.IsGetdata = this.IsGetdata;
        }

        #region Override

        protected override void DataControl()
        {
            if (vm.VMIsGetdata)//正在获取数据
            {
                //IsGetdata = false;
                vm.VMIsGetdata = false;
            }
            else
            {
                if (!USBCanManager.Instance.Exist(OwnerProject))
                {
                    ShowLog("CAN未打开!");
                    return;
                }
                foreach (var item in vm.XCPSignals.xCPSignalList)
                {
                    item.StrValue = "0";
                }

                //IsGetdata = true;
                vm.VMIsGetdata = true;
            }
        }

        public override void OnDataReceiveEvent(object sender, CANDataReceiveEventArgs args)
        {
            return;
        }

        protected override bool ChangeValueByCell(bool addorReduce, DataGridViewCellEventArgs e)
        {
            var signal = dataGridView1.Rows[e.RowIndex].DataBoundItem as XCPSignal;

            //获取步长
            if (dataGridView1.Rows[e.RowIndex].Cells[dataGridView1.Columns["ColumnStep"].Index].Value == null)
            {
                dataGridView1.Rows[e.RowIndex].Cells[dataGridView1.Columns["ColumnStep"].Index].Value = "1";
            }

            string stepStr = dataGridView1.Rows[e.RowIndex].Cells[dataGridView1.Columns["ColumnStep"].Index].Value.ToString();

            if (decimal.TryParse(stepStr, out decimal stepD))
            {
                //获取信号，根据step修改信号值，并发送数据
                decimal oldValue = Convert.ToDecimal(signal.StrValue);
                decimal newValue = oldValue;
                if (addorReduce)
                {
                    newValue = (oldValue + stepD);
                    //signal.StrValue = .ToString();
                }
                else
                {
                    newValue = (oldValue - stepD);
                    //signal.StrValue = (oldValue - stepD).ToString();
                }

                
                signal.StrValue = newValue.ToString();
                SetOrSend();
                return true;
            }
            else
            {
                ShowLog($"{signal.SignalName ?? signal.CustomName} Step 值格式错误", LPLogLevel.Warn);
            }

            return false;
        }

        protected override void InitSignalUC()
        {
            vm.XCPSignals = FormItem.XCPSingals;

            metroComboBox_Signal.DataSource = null;
            metroComboBox_Signal.DataSource = vm.XCPSignals.xCPSignalList;
            metroComboBox_Signal.DisplayMember = "SignalName";

            BindingList<XCPSignal> bs = new BindingList<XCPSignal>(vm.XCPSignals.xCPSignalList);
            dataGridView1.DataSource = bs;

            vm.InitCylceSignals();

            InitIDAndProtocolCmd();

            HistoryDataView = new HistoryDataUC();
            HistoryDataView.ProjectName = this.OwnerProject.Name;
            HistoryDataView.FormName = this.Name;
            HistoryDataView.Dock = DockStyle.Fill;
            metroTabPage2.Controls.Clear();
            metroTabPage2.Controls.Add(HistoryDataView);
            HistoryDataView.ChangeColorTheme(GetColor);

            ProjectForm parent = (ProjectForm)this.MdiParent;
            vm.XcpModule = parent.XcpModule;
            base.InitSignalUC();
        }

        protected override void ModifiedGetdata(bool get)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    base.ModifiedGetdata(get);
                    if (get)
                    {
                        vm.StartThread((uint)this.CanChannel);
                    }
                    else
                    {
                        vm.CloseThread();
                    }
                }));
            }
            else
            {
                base.ModifiedGetdata(get);
                if (get)
                {
                    vm.StartThread((uint)this.CanChannel);
                }
                else
                {
                    vm.CloseThread();
                }
            }
            
        }

        protected override void ModifiedSignals()
        {
            if (vm.ModifiedSignals())
            {
                ReLoadSignal();
            }
              
        }

        protected override void SelectedSignalChanged()
        {
            if (metroComboBox_Signal.SelectedIndex != -1)
            {
                XCPSignal selectSignal = metroComboBox_Signal.SelectedItem as XCPSignal;

                ///不能绑定数据，rollingcounter 模式下改变数据会跳变
                ///9->10 先发1，再发10
                if (this.FormType == FormType.Set)
                {
                    var threadSafeModel = new SynchronizedNotifyPropertyChanged<XCPSignal>(selectSignal, this);
                    tbCurrent.DataBindings.Clear();
                    tbCurrent.DataBindings.Add("Text", threadSafeModel, "StrValue", false, DataSourceUpdateMode.OnPropertyChanged, "0");
                }

                tbCurrent.Text = selectSignal.StrValue;
            }
        }

        protected override void ChangeValue(object sender)
        {
            if (metroComboBox_Signal.SelectedIndex != -1)
            {
                XCPSignal selectSignal = metroComboBox_Signal.SelectedItem as XCPSignal;

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

        protected override void ShowSignalInfo(DataGridViewRow row)
        {
            vm.ShowSignalDetails(row);
        }

        protected override void SetOrSend()
        {
            foreach (var item in vm.XCPSignals.xCPSignalList)
            {
                if (!item.WhetherSendOrGet)
                    continue;
                if (Convert.ToDouble(item.StrValue) > (double)item.Maximum || Convert.ToDouble(item.StrValue) < (double)item.Minimum)
                {
                    ShowLog($"{item.CustomName ?? item.SignalName}数值超限[{item.Minimum},{item.Maximum}]", LPLogLevel.Warn);
                    continue;
                }

                try
                {
                    vm.XcpModule.Download(item , (uint)CanChannel);
                }
                catch (XCPException xcpEx)
                {
                    ShowLog($"{item.SignalName}发送错误：{xcpEx.Message}",LPLogLevel.Warn);
                    break;
                }
                catch (Exception ex)
                {
                    ShowLog($"{item.SignalName}发送错误：{ex.Message}",LPLogLevel.Error);
                    continue;
                }

            }

        }
        #endregion
    }
}
