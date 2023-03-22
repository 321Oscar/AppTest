using AppTest.FormType.Helper;
using AppTest.Helper;
using AppTest.Model;
using AppTest.ProtocolLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppTest.FormType
{
    public partial class SetFormV2 : BaseDataForm
    {
        /// <summary>
        /// 获取信号
        /// </summary>
        public SetFormV2()
        {
            InitializeComponent();
            SignalUC = new Dictionary<string, SignalInfoUC>();
        }


        protected override void InitSignalUC()
        {
            if (null == Signals || Signals.SignalList.Count == 0)
                return;

            //int locationX = this.gbSignals.Left + 10;
            //int minHeigt = this.gbSignals.Location.Y;

            for (int i = 0; i < Signals.SignalList.Count; i++)
            {
                SignalInfoUC signalInfoUS = new SignalInfoUC(Signals.SignalList[i], true);
               // minHeigt = signalInfoUS.Height;
                //signalInfoUS.Location = new Point(locationX, locationY);
                signalInfoUS.Dock = DockStyle.Top;
                pnSignals.Controls.Add(signalInfoUS);
                signalInfoUS.Show();

                SignalUC.Add(Signals.SignalList[i].SignalName, signalInfoUS);
            }

           // this.MinimumSize = new Size(0, minHeigt + panel1.Height + 70);

            //getDataTimer = new System.Timers.Timer();
            //getDataTimer.Elapsed += GetDataTimer_Elapsed;

            if (ProtocolCommand != null)
            {
                Protocol = ReflectionHelper.CreateInstance<BaseProtocol>(ProtocolCommand, Assembly.GetExecutingAssembly().ToString());
            }
        }

        private void SetFormV2_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            //using(var db = new SignalDB($"{Environment.CurrentDirectory}{Global.DBPATH}"))
            //{
            //    var entities = db.signalEntities.Where(x => x.projectName == OwnerProject.Name && x.FormName == this.Name).OrderByDescending(x=>x.CreatedOn).ToList();
            //    this.dataGridView1.DataSource = entities;
            //}
        }
    }
}
