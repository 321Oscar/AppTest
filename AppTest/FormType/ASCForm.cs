using AppTest.FormType.Helper;
using AppTest.Helper;
using AppTest.ProjectClass;
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
    public partial class ASCForm : Form
    {
        private Singals allSingals;
        private List<ASCSignal> ASCSignals;
        private string ascFilePath;

        //private 
        public ASCForm()
        {
            InitializeComponent();

            metroComboBoxDBCFiles.DataSource = GetDBCFiles();

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.DoWork += BackgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += BackgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
        }

        private List<string> GetDBCFiles()
        {
            List<string> dbcFiles = new List<string>();

            string filePath = Path.Combine(Application.StartupPath, ProtocolType.DBC.ToString());
            if (Directory.Exists(filePath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(filePath);

                string filetype = GetFileTypeEnum(ProtocolType.DBC);

                FileInfo[] files = directoryInfo.GetFiles(filetype);
                for (int i = 0; i < files.Length; i++)
                {
                    dbcFiles.Add(files[i].Name);
                }
            }

            return dbcFiles;
        }

        private string GetFileTypeEnum(Enum x)
        {
            try
            {
                Type type = x.GetType();
                MemberInfo[] memberInfos = type.GetMember(x.ToString());
                if (null != memberInfos && memberInfos.Length > 0)
                {
                    object[] attrs = memberInfos[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (null != attrs && attrs.Length > 0)
                    {
                        return ((DescriptionAttribute)attrs[0]).Description;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                LeapMessageBox.Instance.ShowError(ex.ToString());
                return null;
            }
        }

        private void metroButton_DBCFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "DBC|*.dbc";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string configPath = Application.StartupPath + "\\" + ProtocolType.DBC.ToString();
                if (!Directory.Exists(configPath))
                {
                    Directory.CreateDirectory(configPath);
                }
                File.Copy(ofd.FileName, configPath + ofd.FileName.Substring(ofd.FileName.LastIndexOf('\\')), true);

                metroComboBoxDBCFiles.DataSource = GetDBCFiles();
            }
        }

        private void metroComboBoxDBCFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(metroComboBoxDBCFiles.SelectedIndex > -1)
            {
                string fileName = metroComboBoxDBCFiles.SelectedItem.ToString();
                //lbSelectedNode.Items.Clear();
                tvAllNode.Nodes.Clear();
                //find File from bin

                try
                {
                    allSingals = BaseProtocol.GetSingalsByProtocol((int)ProtocolType.DBC,new string[] { fileName });
                    allSingals.Signal.Sort();
                    foreach (var item in allSingals.Signal)
                    {
                        if (tvAllNode.Nodes.Find(item.MessageID, false).Count() == 0)
                        {
                            TreeNode tn = new TreeNode()
                            {
                                Name = item.MessageID,
                                Text = item.MessageID
                            };
                            tvAllNode.Nodes.Add(tn);
                            tvAllNode.Nodes[tvAllNode.Nodes.Count - 1].Nodes.Add(item.SignalName);
                        }
                        else
                        {
                            tvAllNode.Nodes[item.MessageID].Nodes.Add(item.SignalName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LeapMessageBox.Instance.ShowError(ex.ToString());
                }
            }
        }

        private void metroButton_Import_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "ASC|*.asc";

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                ascFilePath = ofd.FileName;
                ASCSignals = new List<ASCSignal>();
                if (!backgroundWorker1.IsBusy)
                {
                    backgroundWorker1.RunWorkerAsync();
                }
            }
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                LeapMessageBox.Instance.ShowWarn("Cancelled");
            }
            else if(e.Error != null)
            {
                LeapMessageBox.Instance.ShowError(e.Error);
            }
            else
            {
                dataGridView1.DataSource = null;
                BindingList<ASCSignal> bs = new BindingList<ASCSignal>(ASCSignals);
                dataGridView1.DataSource = bs;
                LeapMessageBox.Instance.ShowInfo("读取完成");
            }
        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.metroProgressBar1.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string ascData = string.Empty;
            //解析ASC文件
            LoadFile(ascFilePath, ref ascData);
            string[] bufferAry = ascData.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int lineNum = bufferAry.Length;
            //this.metroProgressBar1.Maximum = lineNum;
            for (int i = 2; i < lineNum; i++)
            {
                string[] lineData = bufferAry[i].Split('\t', ' ');
                ASCSignal ascSignal = new ASCSignal();
                ascSignal.Timestap = decimal.Parse(lineData[0]);
                ascSignal.Reserved = int.Parse(lineData[1]);
                //ascSignal.Msgid = int.Parse(lineData[2], System.Globalization.NumberStyles.HexNumber);
                ascSignal.SendOrRec = lineData[3];
                ascSignal.Reserved2 = lineData[4];
                ascSignal.Datalength = int.Parse(lineData[5]);
                byte[] candata = new byte[8];
                for (int j = 0; j < 8; j++)
                {
                    candata[j] = (byte)int.Parse(lineData[6 + j], System.Globalization.NumberStyles.HexNumber);
                }
                CAN_msg msg = new CAN_msg(int.Parse(lineData[2], System.Globalization.NumberStyles.HexNumber), candata, 0);
                ascSignal.CAN_Msg = msg;
                ASCSignals.Add(ascSignal);

                backgroundWorker1.ReportProgress((int)(i / (lineNum * 1.00m) * 100));
            }
        }

        /// <summary>
        /// 加载文本文件到字符串
        /// </summary>
        /// <param name="path"></param>
        /// <param name="stringOut"></param>
        /// <returns></returns>
        public int LoadFile(string path, ref string stringOut)
        {
            FileStream fs = null;
            StreamReader sr = null;

            try
            {
                fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                sr = new StreamReader(fs, System.Text.Encoding.Default);

                stringOut = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                LogHelper.Warn(ex.ToString());
                if (sr != null)
                {
                    sr.Close();
                }
                if (fs != null)
                {
                    fs.Close();
                }
                return 1;
            }

            if (sr != null)
            {
                sr.Close();
            }
            if (fs != null)
            {
                fs.Close();
            }

            return 0;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                var signal = dataGridView1.Rows[e.RowIndex].DataBoundItem as ASCSignal;

                var signals = allSingals.Signal.FindAll(x => x.MessageID == signal.CAN_Msg.cid.ToString("X"));
                Singals singals = new Singals();
                singals.Signal = signals;
                DBCProtocol dbc = new DBCProtocol();
                string value = string.Empty;
                foreach (var item in dbc.MultipYeild(new CAN_msg[] { signal.CAN_Msg }, singals))
                {
                    value += item.SignalName +"："+ item.StrValue + "\r\n";
                }
                MessageBox.Show(value);
                //SignalItemForm<DBCSignal> siF = new SignalItemForm<DBCSignal>(signal, signal.SignalName);
                //siF.Show();
            }
            catch (Exception ex)
            {
                LeapMessageBox.Instance.ShowWarn(ex.Message);
            }
        }

        private void metroButton_Query_Click(object sender, EventArgs e)
        {
            if(ASCSignals == null || ASCSignals.Count == 0)
            {
                LeapMessageBox.Instance.ShowInfo("先加载ASC文件！");
                return;
            }

            List<ASCSignal> allQuery = new List<ASCSignal>();

            foreach (TreeNode msgID in tvAllNode.Nodes)
            {
                if (msgID.Checked)
                {
                    var query = ASCSignals.FindAll(x => x.CAN_Msg.cid == int.Parse(msgID.Text, System.Globalization.NumberStyles.HexNumber));

                    allQuery.AddRange(query);
                }
            }

            BindingList<ASCSignal> bs = new BindingList<ASCSignal>(allQuery);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = bs;
        }
    }

    public class ASCSignal
    {
        private decimal timestap;
        private int reserved;
        private string sendOrRec;
        private string reserved2;
        private int datalength;
        private CAN_msg cAN_Msg;

        public decimal Timestap { get => timestap; set => timestap = value; }
        public int Reserved { get => reserved; set => reserved = value; }
        public string SendOrRec { get => sendOrRec; set => sendOrRec = value; }
        public string Reserved2 { get => reserved2; set => reserved2 = value; }
        public CAN_msg CAN_Msg { get => cAN_Msg; set => cAN_Msg = value; }
        public int Datalength { get => datalength; set => datalength = value; }
    }
}
