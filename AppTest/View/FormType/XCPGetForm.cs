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
        }

        #region Override

        protected override void DataControl()
        {
            if (vm.IsGetdata)//正在获取数据
            {
                //IsGetdata = false;
                vm.IsGetdata = false;
            }
            else
            {
                if (!USBCanManager.Instance.Exist(OwnerProject))
                {
                    LeapMessageBox.Instance.ShowInfo("CAN未打开!");
                    return;
                }
                foreach (var item in vm.XCPSignals.xCPSignalList)
                {
                    item.StrValue = "0";
                }

                //IsGetdata = true;
                vm.IsGetdata = true;
            }
        }

        public override void OnDataRecieveEvent(object sender, CANDataRecieveEventArgs args)
        {
            return;
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

            vm.ModifiedGetdata += ModifiedGetdata;

            InitIDAndProtocolCmd();

            ProjectForm parent = (ProjectForm)this.MdiParent;
            vm.XcpModule = parent.XcpModule;
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
            vm.ModifiedSignals();
            ReLoadSignal();
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

        protected override void ShowSignalInfo(DataGridViewCellEventArgs e)
        {
            vm.ShowSignalDetai(this.dataGridView1, e);
        }

        protected override void SetOrSend()
        {
            foreach (var item in vm.XCPSignals.xCPSignalList)
            {
                ShowLog("");
                if (!item.WhetherSendOrGet)
                    continue;
                try
                {
                    vm.XcpModule.Download(item , (uint)CanChannel);
                }
                catch (XCPException xcpEx)
                {
                    ShowLog($"{item.SignalName}发送错误：{xcpEx.Message}");
                    break;
                }
                catch (Exception ex)
                {
                    ShowLog($"{item.SignalName}发送错误：{ex.Message}");
                    continue;
                }

            }

        }
        #endregion
    }
}