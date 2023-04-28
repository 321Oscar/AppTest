using AppTest.FormType;
using AppTest.Helper;
using AppTest.Model;
using LPCanControl.CANInfo;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AppTest.ViewModel
{
    public class XCPPollingViewModel : XCPViewModel
    {
        /// <summary>
        /// 10ms 信号
        /// </summary>
        private List<XCPSignal> tenSignals;
        /// <summary>
        /// 10ms 线程
        /// </summary>
        private Thread tenThread;
        /// <summary>
        /// 100ms 信号
        /// </summary>
        private List<XCPSignal> hSignals;
        /// <summary>
        /// 100 ms 线程
        /// </summary>
        private Thread hThread;
        /// <summary>
        /// 1000ms/1s 信号
        /// </summary>
        private List<XCPSignal> tSignals;
        /// <summary>
        /// 1000ms/1s 线程
        /// </summary>
        private Thread tThread;
        public XCPPollingViewModel(BaseDataForm form)
        {
            Form = form;
        }
        public override void OnDataRecieveEvent(object sender, CANDataReceiveEventArgs args)
        {
            return;
        }

        public void InitCylceSignals()
        {
            tenSignals = new List<XCPSignal>();
            hSignals = new List<XCPSignal>();
            tSignals = new List<XCPSignal>();

            foreach (var item in XCPSignals.xCPSignalList)
            {
                if (item.CycleTime == 10)
                {
                    tenSignals.Add(item);
                }
                else if (item.CycleTime == 100)
                {
                    hSignals.Add(item);
                }
                else
                {
                    tSignals.Add(item);
                }
            }
        }

        public void StartThread(uint canChannel)
        {
            // 启动get thread，分为10ms，100ms，1s三种线程
                // 线程一直启动？一直在获取数据
                // 按照cycletime 分组（三个信号列表），启动就新建线程（从列表中获取信号，遍历获取信号值），关闭则停止线程
                tenThread = new Thread(Get10ms);
                hThread = new Thread(Get100ms);
                tThread = new Thread(Get1000ms);
                tenThread.Start(canChannel);
                hThread.Start(canChannel);
                tThread.Start(canChannel);
        }

        public void CloseThread()
        {
            ClearThread(tenThread);
            ClearThread(hThread);
            ClearThread(tThread);
        }

        private void ClearThread(Thread t)
        {
            if (t != null)
            {
                t.Abort();
            }
        }

        private void Get10ms(object canChannel)
        {
            uint cInd = (uint)canChannel;
            do
            {
                if (tenSignals == null || tenSignals.Count == 0)
                    break;
                GetDataByCycleTime(tenSignals, cInd);
                Thread.Sleep(10);
            } while (VMIsGetdata);
        }

        private void Get100ms(object canChannel)
        {
            uint cInd = (uint)canChannel;
            do
            {
                if (hSignals == null || hSignals.Count == 0)
                    break;
                GetDataByCycleTime(hSignals, cInd);
                Thread.Sleep(100);
            } while (VMIsGetdata);
        }

        private void Get1000ms(object canChannel)
        {
            uint cInd = (uint)canChannel;
            do
            {
                if (tSignals == null || tSignals.Count == 0)
                    break;
                GetDataByCycleTime(tSignals, cInd);
                Thread.Sleep(1000);
            } while (VMIsGetdata);
        }

        private async void GetDataByCycleTime(List<XCPSignal> signals,uint canChannel)
        {
            //根据信号取值
            try
            {
                List<SignalEntity> signalEntities = new List<SignalEntity>();
                var datatimeStr = DateTime.Now.ToString(Global.DATETIMEFORMAT);
                SignalEntity entity;// = new SignalEntity();

                XcpModule.ClearQueue();
                foreach (var item in signals)
                {
                    if (!item.WhetherSendOrGet)
                        continue;

                    //发送读取数据命令
                    XCPSignal signal = item as XCPSignal;
                    if (signal.Length <= 6)
                    {
                        XcpModule.ShortUpload(ref signal, canChannel);
                    }
                    else
                    {
                        XcpModule.Upload(ref signal, canChannel);
                    }

                    if (XcpModule.CurrentCMDStatus == XCPCMDStatus.UploadFail)
                    {
                        ShowLog($"{item.SignalName} Upload Fail");
                        continue;
                    }

                    entity = new SignalEntity();
                    entity.DataTime = datatimeStr;
                    entity.ProjectName = Form.OwnerProject.Name;
                    entity.FormName = Form.Name;
                    entity.SignalName = item.SignalName;
                    entity.SignalValue = signal.StrValue;
                    entity.CreatedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
                    signalEntities.Add(entity);

                }
                if (signalEntities.Count > 0 && Form.IsSaveData)
                {
                    LogHelper.Warn($"FormName:{Form.Name};ThreadID:{Thread.CurrentThread.ManagedThreadId };Log:Start Save db.");
                    var dbAsync = await DBHelper.GetDb();
                    var result = await dbAsync.InsertAllAsync(signalEntities);
                    LogHelper.Warn($"FormName:{Form.Name};ThreadID:{Thread.CurrentThread.ManagedThreadId };Log:Save Success，Counter:{result}.");
                    return;
                }
            }
            catch (Exception ex)
            {
                VMIsGetdata = false;
                ShowLog?.Invoke(ex.Message);
                LogHelper.Error(Form.Name, ex);
            }
        }

        
    }
}
