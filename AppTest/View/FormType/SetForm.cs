using AppTest.FormType.Helper;
using AppTest.Helper;
using AppTest.Model;
using AppTest.ProtocolLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppTest.FormType
{
    //public class MyGroupBox : GroupBox
    //{
    //    public bool Equals(MyGroupBox myGroupBox)
    //    {
    //        return (this.Name == myGroupBox.Name);
    //    }
    //}

    public partial class SetForm : BaseDataForm
    {
        public SetForm()
        {
            InitializeComponent();

            this.FormType = FormType.Set;
            base.SaveDataVisible = false;

            SignalUC = new Dictionary<string, SignalInfoUC>();
            nudStep.DecimalPlaces = 2;
        }
        /// <summary>
        /// 按照ID分组
        /// </summary>
        List<GroupBox> gb = new List<GroupBox>();

        protected override void InitSignalUC()
        {
            if (null == Signals || Signals.SignalList.Count == 0)
                return;

            AddSignalToPanel(out int minHeight);

            this.MinimumSize = new Size(0, minHeight + panel1.Height + 70);

            if (ProtocolCommand != null)
            {
                CanIndexItem canIndex = OwnerProject.CanIndex.Find(x => x.CanChannel == CanChannel);

                Protocol = ReflectionHelper.CreateInstance<BaseProtocol>(ProtocolCommand, Assembly.GetExecutingAssembly().ToString()
                    , new string[] { canIndex.ProtocolFileName });
                
            }

        }

        protected override void ReLoadSignal()
        {
            AddSignalToPanel(out int _);
        }

        private void GroupBox_Click(object sender, EventArgs e)
        {
            /*
            GroupBox gb = sender as GroupBox;
            string height = $"GBHeight:{gb.Height}\n\r";
            foreach (Control item in gb.Controls)
            {
                height += $"{item.GetType()}:Height :{item.Height};X:{item.Location.X};Y{item.Location.Y}\n\r";
                if (item.Controls.Count > 0)
                {
                    foreach (Control cont in item.Controls)
                    {
                        height += $"{cont.GetType()}:Height :{cont.Height};X:{cont.Location.X};Y{cont.Location.Y}\n\r";
                    }
                }
            }

            MessageBox.Show(height);
            */

        }

        private void GroupBox_Resize(object sender, EventArgs e)
        {
            if (this.IsSized)
                return;
            GroupBox gb = sender as GroupBox;
            int signalWidth = 0;
            int signalHeight = 0;
            int signalCount = 0;
            foreach (Control item in gb.Controls)
            {
                if(item is FlowLayoutPanel)
                {
                    foreach (Control signalUC in item.Controls)
                    {
                        if (signalUC is SignalInfoUC)
                        {
                            signalWidth =  signalUC.Width > signalWidth ? signalUC.Width : signalWidth;
                            signalHeight = signalUC.Height > signalHeight ? signalUC.Height : signalHeight;
                            signalCount++;
                        }
                    }
                    //int flpHeight = item.Height;
                }

                //每行N个
                int n = 1;
                if (signalWidth != 0)
                    n = gb.Width / signalWidth;

                //行
                int row = n == 0 ? signalCount : signalCount / n + 1;

                if(n == 0)
                {
                    row = signalCount;
                    //if (gb.Width < signalWidth)
                    //    row++;
                }
                else if (n > signalCount)
                {
                    row = 1;
                }
                else
                {
                    if(signalCount % n > 0)
                    {
                        row = signalCount / n + 1;
                    }
                    else
                    {
                        row = signalCount / n;
                    }
                }
                
                // column = signalCount / row + signalCount % row;
                gb.Height = (signalHeight) * (row) + item.Location.Y;
                //if (gb.Width < )
            }

        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            lblog.Text = "";
            //SignalEqualityComarer signalEqualityComarer = new SignalEqualityComarer();
            Dictionary<BaseSignal, string> keyValues = new Dictionary<BaseSignal, string>();
            foreach (var groupbox in gb)
            {
                try
                {
                    foreach (Control lfp in groupbox.Controls)
                    {
                        if(lfp is FlowLayoutPanel)
                        {
                            foreach (var item in lfp.Controls)
                            {
                                if (item is SignalInfoUC uS)
                                {
                                    keyValues.Add(uS.Signal, uS.SignalValue);
                                }
                            }
                        }
                    }
                    
                    if (keyValues.Count == 0)
                        continue;
                    var frame = Protocol.BuildFrames(keyValues);
                    ///组帧
                    if (USBCanManager.Instance.Send(OwnerProject,CanChannel, sendData: frame[0],$"[{this.FormType}]{this.Name}"))
                    {
                        groupbox.Text = $"ID: {groupbox.Name} --{DateTime.Now}--已发送";
                    }
                    else
                    {
                        groupbox.Text = $"ID:{ groupbox.Name }--{DateTime.Now}--发送失败";
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(this.Name, ex);
                    this.lblog.Text += ex.Message + "\r\n";
                    continue;
                }
                
            }

        }

        //private void richTextBox1_ContentsResized(object sender, ContentsResizedEventArgs e)
        //{
        //    this.rtbLog.SelectionStart = rtbLog.Text.Length;
        //    rtbLog.ScrollToCaret();
        //}

        private void btnChangeValue_Click(object sender, EventArgs e)
        {
            if(cbbSignals.SelectedIndex != -1)
            {
                DBCSignal selectSignal = cbbSignals.SelectedItem as DBCSignal;

                decimal oldValue = Convert.ToDecimal(SignalUC[selectSignal.SignalName].SignalValue);
                Button bt = sender as Button;
                decimal newValue;
                try
                {
                    if (bt.Name == btnChangeValue.Name)
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
                        //LeapMessageBox.Instance.ShowInfo();
                    }
                }
                catch (Exception ex)
                {
                    LeapMessageBox.Instance.ShowError(ex);
                    return;
                }
                

                SignalUC[selectSignal.SignalName].SignalValue = newValue.ToString();

                cbbSignals_SelectedIndexChanged(null, null);

                this.btnSet_Click(null,null);
            }
        }

        private void btnReduceValue_Click(object sender, EventArgs e)
        {
            if (cbbSignals.SelectedIndex != -1)
            {
                DBCSignal selectSignal = cbbSignals.SelectedItem as DBCSignal;

                decimal oldValue = Convert.ToDecimal(SignalUC[selectSignal.SignalName].SignalValue);//signalUC[selectSignal.SignalName].GetData()

                decimal newValue = oldValue - nudStep.Value;

                SignalUC[selectSignal.SignalName].SignalValue = newValue.ToString();//.SetData(newValue.ToString());

                cbbSignals_SelectedIndexChanged(null, null);

                this.btnSet_Click(null, null);


            }
        }

        private void cbbSignals_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbSignals.SelectedIndex != -1)
            {
                DBCSignal selectSignal = cbbSignals.SelectedItem as DBCSignal;

                tbCurrent.Text = SignalUC[selectSignal.SignalName].SignalValue;//signalUC[selectSignal.SignalName].GetData();

                //this.btnSet_Click(null, null);
            }
        }

        private void AddSignalToPanel(out int minHeight)
        {
            minHeight = 50;
            foreach (Control item in pnSignals.Controls)
            {
                item.Controls.Clear();
            }

            //保留原有信号的值
            Dictionary<string, SignalInfoUC> oldSignals = new Dictionary<string, SignalInfoUC>(SignalUC);

            pnSignals.Controls.Clear();
            SignalUC.Clear();
            gb.Clear();
            cbbSignals.DataSource = null;
            //Signals.Signal.Sort();
            //信号显示使用dock.Top来布局，排序
            for (int i = Signals.SignalList.Count-1; i >= 0; i--)
            {
                SignalInfoUC signalInfoUS = new SignalInfoUC(Signals.SignalList[i], false);
                minHeight = signalInfoUS.Height;
                //signalInfoUS.SetData("0");
                //signalInfoUS.Location = new Point(locationX, locationY);
                signalInfoUS.Dock = DockStyle.Top;
                SignalUC.Add(Signals.SignalList[i].SignalName, signalInfoUS);
                //modified by xwd 2021-12-28
                //增加回车发送
                signalInfoUS.DataSendToCan += btnSet_Click;
                //保留原有信号的值
                if (oldSignals.ContainsKey(Signals.SignalList[i].SignalName))
                {
                    signalInfoUS.SignalValue = oldSignals[Signals.SignalList[i].SignalName].SignalValue;
                }

                //signalInfoUS.Show();
                //pnSignals.Controls.Add(signalInfoUS);
                GroupBox groupBox = gb.Find(x => x.Name == Signals.SignalList[i].MessageID);

                if (groupBox == null)
                {
                    groupBox = new GroupBox()
                    {
                        Name = Signals.SignalList[i].MessageID,
                        Text = "ID:" + Signals.SignalList[i].MessageID,
                        AutoSize = true
                    };
                    groupBox.Resize += GroupBox_Resize;
                    groupBox.Click += GroupBox_Click;
                    gb.Add(groupBox);
                    groupBox.Dock = DockStyle.Top;
                    pnSignals.Controls.Add(groupBox);
                    FlowLayoutPanel flp = new FlowLayoutPanel();
                    flp.Dock = DockStyle.Fill;
                    flp.AutoSize = true;
                    groupBox.Controls.Add(flp);
                }
                foreach (Control item in groupBox.Controls)
                {
                    if (item is FlowLayoutPanel)
                    {
                        item.Controls.Add(signalInfoUS);
                    }
                }
                //.Add(signalInfoUS);

            }                    

            cbbSignals.DataSource = Signals.SignalList;
            cbbSignals.DisplayMember = "SignalName";

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (var groupbox in gb) 
            {
                foreach (Control lfp in groupbox.Controls)
                {
                    if (lfp is FlowLayoutPanel)
                    {
                        foreach (var item in lfp.Controls)
                        {
                            if (item is SignalInfoUC uS)
                            {
                                //this.Signals.Signal.Find(x => x.SignalName == uS.Signal.SignalName).Value = uS.SignalValue;
                                this.Signals.SignalList.Find(x => x == uS.Signal).StrValue = uS.SignalValue;
                            }
                        }
                    }
                }
            }
            LeapMessageBox.Instance.ShowInfo("配置保存成功");
                
        }
    }
}
