using AppTest.FormType;
using AppTest.Helper;
using AppTest.Model;
using LPCanControl.CANInfo;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AppTest.ViewModel
{
    public class XCPDAQGetViewModel : XCPDAQViewModel
    {
        public XCPDAQGetViewModel(BaseDataForm form)
        {
            Form = form;
            this.ShowLog += form.ShowLog;
        }

        public override bool InitDAQ(uint canChannel)
        {
            foreach (var item in XCPSignals.xCPSignalList)
            {
                item.StrValue = "0";
                item.TimeStamp = -1;
            }

            recieveData = new Dictionary<int, List<byte>>();
            daqTimeCount = new Dictionary<int, int>();
            DAQList.Clear();
            //根据事件ID排序
            this.XCPSignals.xCPSignalList.Sort((x, y) => { return x.EventID.CompareTo(y.EventID); });
            foreach (var signal in this.XCPSignals.xCPSignalList)
            {
                DAQList.AddSignal(signal);
            }

            foreach (var daq in DAQList.DAQs)
            {
                recieveData.Add(daq.Event_Channel_Number, new List<byte>());
                daqTimeCount.Add(daq.Event_Channel_Number, 0);
            }
            try
            {
                var initDaqTrue = XcpModule.SetDAQ(DAQList.DAQs, canChannel);

                ShowLog?.Invoke($"DAQ 配置{(initDaqTrue ? "成功" : "失败")}");

                return initDaqTrue;
            }
            catch (XCPException xcperr)
            {
                ShowLog?.Invoke(xcperr.Message, LPLogLevel.Warn);
            }
            catch (Exception err)
            {
                ShowLog?.Invoke(err.Message, LPLogLevel.Error);
            }
            return false;
        }

        public override void OnDataRecieveEvent(object sender, CANDataReceiveEventArgs args)
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
                        ParseResponeToXCPSignalAsync(recieveData[daq.Event_Channel_Number], daq.Event_Channel_Number, item.TimeStampInt);

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

        public override async Task ParseResponeToXCPSignalAsync(List<byte> data, int eventIndex,uint cantimeStamp)
        {
            var signals = XCPSignals.xCPSignalList.FindAll(x => x.EventID == eventIndex);
            //解析时间戳
            int timestamp = BitConverterExt.ToUInt16(data.ToArray(), 0, 0);
            List<SignalEntity> signalEntities = new List<SignalEntity>();
            SignalEntity entity;
            var datatimeStr = DateTime.Now.ToString(Global.DATETIMEFORMAT);
            if (signals[0].TimeStamp != -1 && signals[0].TimeStamp - 0x10000 * daqTimeCount[eventIndex] - timestamp > 0)
            {
                //时间戳重新开始
                daqTimeCount[eventIndex]++;
            }
            timestamp += 0x10000 * daqTimeCount[eventIndex];
            foreach (var signal in signals)
            {
                byte size = (byte)signal.Length;
                var da = new byte[size];
                try
                {
                    for (int i = 0; i < size; i++)
                    {
                        da[i] = data[signal.StartIndex + i + 2];
                    }
                   
                    signal.StrValue = XCPHelper.DealData4Byte(signal, da);
                }
                catch (System.InvalidOperationException errOp)
                {
                    ShowLog?.Invoke($"{Form.Name} {signal.SignalName} Data :{signal.StrValue};{errOp.Message}",LPLogLevel.Warn );
                }
                catch (ArgumentOutOfRangeException errRange)
                {
                    ShowLog?.Invoke($"{Form.Name} {signal.SignalName} size :{size};startInd:{signal.StartIndex};dataLength:{data.Count} {errRange.Message}" , LPLogLevel.Warn);
                }
                catch (Exception err)
                {
                    //ShowLog($"{signal.SignalName} Parse Data error ");
                    string log = string.Empty;
                    foreach (var item in data)
                    {
                        log += $" {item:X}";
                    }
                    ShowLog?.Invoke($"{Form.Name} {signal.SignalName} Parse Data error {err.Message} :{log}", LPLogLevel.Error);
                }
                signal.TimeStamp = timestamp;
                entity = new SignalEntity
                {
                    DataTime = datatimeStr,
                    ProjectName = Form.OwnerProject.Name,
                    FormName = Form.Name,
                    SignalName = signal.SignalName,
                    SignalValue = signal.StrValue,
                    CreatedOn = DateTime.Now.ToString(Global.DATETIMEFORMAT),
                    TimeStamp = signal.TimeStamp,
                    CANTimeStamp = cantimeStamp
                };
                signalEntities.Add(entity);
            }

            if (signalEntities.Count > 0 && Form.IsSaveData)
            {
                LogHelper.WriteToOutput(Form.Name, $"ThreadID:{Thread.CurrentThread.ManagedThreadId };Log:Start Save db.");
                var dbAsync = await DBHelper.GetDb();
                var result = await dbAsync.InsertAllAsync(signalEntities);
                LogHelper.WriteToOutput(Form.Name, $"ThreadID:{Thread.CurrentThread.ManagedThreadId };Log:Save Success，Counter:{result}.");
            }

            signalEntities.Clear();
        }

    }
}
