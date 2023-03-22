using AppTest.FormType;
using AppTest.Helper;
using AppTest.Model;
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

namespace AppTest.View.FormType
{
    public partial class XCPDAQBaseForm : BaseDataForm
    {
        protected DAQList DAQList = new DAQList();
        protected XCPModule XcpModule;
        protected Dictionary<int, List<byte>> recieveData;

        public XCPDAQBaseForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">数据，包含时间戳</param>
        /// <param name="eventIndex">事件通道编号</param>
        protected virtual async void ParseResponeToXCPSignal(List<byte> data, int eventIndex)
        {
            var signals = this.xCPSingals.xCPSignalList.FindAll(x => x.EventID == eventIndex);
            //解析时间戳
            int timestamp = BitConverterExt.ToUInt16(data.ToArray(), 0, 0);
            List<SignalEntity> signalEntities = new List<SignalEntity>();
            SignalEntity entity;
            var datatimeStr = DateTime.Now.ToString(Global.DATETIMEFORMAT);
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
                    LogHelper.Error($"{this.Name} {signal.SignalName} Data :{signal.StrValue}", errOp);
                }
                catch (ArgumentOutOfRangeException errRange)
                {
                    LogHelper.Error($"{this.Name} {signal.SignalName} size :{size};startInd:{signal.StartIndex};dataLength:{data.Count}", errRange);
                }
                catch (Exception err)
                {
                    ShowLog($"{signal.SignalName} Parse Data error ");
                    string log = string.Empty;
                    foreach (var item in data)
                    {
                        log += $" {item:X}";
                    }
                    LogHelper.Error($"{this.Name} {signal.SignalName} Parse Data error :{log}", err);
                }
                signal.TimeStamp = timestamp;
                entity = new SignalEntity
                {
                    DataTime = datatimeStr,
                    ProjectName = OwnerProject.Name,
                    FormName = this.Name,
                    SignalName = signal.SignalName,
                    SignalValue = signal.StrValue,
                    CreatedOn = DateTime.Now.ToString(Global.DATETIMEFORMAT),
                    TimeStamp = signal.TimeStamp
                };
                signalEntities.Add(entity);
            }

            if (signalEntities.Count > 0 && IsSaveData)
            {
                LogHelper.WriteToOutput(this.Name, $"ThreadID:{Thread.CurrentThread.ManagedThreadId };Log:Start Save db.");
                var dbAsync = await DBHelper.GetDb();
                var result = await dbAsync.InsertAllAsync(signalEntities);
                LogHelper.WriteToOutput(this.Name, $"ThreadID:{Thread.CurrentThread.ManagedThreadId };Log:Save Success，Counter:{result}.");
            }

            signalEntities.Clear();

        }

    }
}
