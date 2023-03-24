using AppTest.FormType;
using AppTest.Helper;
using AppTest.Model;
using AppTest.ProtocolLib;
using LPCanControl.CANInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AppTest.ViewModel
{

    public abstract class DBCViewModel : BaseViewModel
    {
        public DBCSignals DBCSignals;

        public List<int> CurrentIDs;

        public override void ModifiedSignals()
        {
            AddNewForm editForm;
            editForm = new AddNewForm(Form.OwnerProject, Form.OwnerProject.Form.Find(x => x.Name == Form.Name));
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                DBCSignals = editForm.FormItem.DBCSignals;
            }
        }

        public override void ShowSignalDetail(DataGridView dataGridView, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                var signal = dataGridView.Rows[e.RowIndex].DataBoundItem as DBCSignal;
                SignalItemForm<DBCSignal> siF = new SignalItemForm<DBCSignal>(signal, signal.SignalName);
                siF.Show();
            }
            catch (Exception ex)
            {
                ShowLog?.Invoke(ex.Message);
            }
        }
    }

    public class DBCGetViewModel : DBCViewModel
    {
        public DBCProtocol DBCProtocol { get; private set; }

        public string DBCFilePath { get; private set; }

        public DBCGetViewModel(BaseDataForm form)
        {
            Form = form;
            
        }
        public DBCGetViewModel(BaseDataForm form, string dbcFilePath) : this(form)
        {
            InitProtocol(dbcFilePath);
        }

        public void InitProtocol(string dbcFilePath)
        {
            DBCFilePath = dbcFilePath;
            DBCProtocol = new DBCProtocol(dbcFilePath);
        }

        public async override void OnDataRecieveEvent(object sender, CANDataRecieveEventArgs args)
        {
            try
            {
                List<SignalEntity> signalEntities = new List<SignalEntity>();
                SignalEntity entity;
                var datatimeStr = DateTime.Now.ToString(Global.DATETIMEFORMAT);
                var rx_mails = args.can_msgs;
                if (null == rx_mails)
                    throw new Exception("接收数据错误。");
                foreach (var item in DBCProtocol.MultipYeild(rx_mails, DBCSignals.SignalList.Cast<BaseSignal>().ToList()))
                {
                    //signalUC[item.SignalName].SignalValue = item.StrValue;
                    entity = new SignalEntity();
                    entity.DataTime = datatimeStr;
                    entity.ProjectName = Form.OwnerProject.Name;
                    entity.FormName = Form.Name;
                    entity.SignalName = item.SignalName;
                    entity.SignalValue = item.StrValue;
                    entity.CreatedOn = DateTime.Now.ToString(Global.DATETIMEFORMAT);
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
            catch (Exception ex)
            {
                IsGetdata = false;
                ShowLog?.Invoke(ex.Message);
            }
        }

    }
}
