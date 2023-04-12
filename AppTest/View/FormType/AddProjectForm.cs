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
    public partial class AddProjectForm : Form
    {
        private ProjectItem projectItem;

        int channelCount = 0;

        Dictionary<BaudRateType, string> baudRateValue0;
        Dictionary<BaudRateType, string> baudRateValue1 = new Dictionary<BaudRateType, string>() {
                            { BaudRateType.Kbps5,"0xFF"},
                            { BaudRateType.Kbps10,"0x1C"},
                            { BaudRateType.Kbps20,"0x1C"},
                            //{ BaudRateType.Kbps40,"0x87"},
                            { BaudRateType.Kbps50,"0x1C"},
                            { BaudRateType.Kbps100,"0x1C"},
                            { BaudRateType.Kbps125,"0x1C"},
                            { BaudRateType.Kbps250,"0x1C"},
                            { BaudRateType.Kbps500,"0x1C"},
                            { BaudRateType.Kbps800,"0x16"},
                            { BaudRateType.Kbps1000,"0x14"}, };
        public AddProjectForm():base()
        {
            this.CenterToParent();
            this.MaximizeBox = false;
            InitializeComponent();

            if (null == projectItem)
                projectItem = new ProjectItem();

            baudRateValue0 = new Dictionary<BaudRateType, string>()
            {
                { BaudRateType.Kbps5, "0x1c01c1" },
                {BaudRateType.Kbps1000,"0x060003"},
                {BaudRateType.Kbps800,"0x060004"},
                {BaudRateType.Kbps500,"0x060007"},
                {BaudRateType.Kbps250,"0x1c0008"},
                {BaudRateType.Kbps125,"0x1c0011"},
                {BaudRateType.Kbps100,"0x160023"},
                {BaudRateType.Kbps50,"0x1c002c"},
                {BaudRateType.Kbps20,"0x1600b3"},
                {BaudRateType.Kbps10,"0x1c00e0"},
            };
            //默认波特率为500
            cbbBaud.DataSource = Enum.GetValues(typeof(BaudRateType));
            cbbBaud.SelectedItem = BaudRateType.Kbps500;

            cbbProtocolType.DataSource = Enum.GetValues(typeof(ProtocolType));
            cbbDeviceType.DataSource = Enum.GetValues(typeof(DeviceType));
            cbbProtocolType.Enabled = cbbProtocolFiles.Enabled =btnSelectProtocol.Enabled= btnImportProtocolFile.Enabled = false;
        }

        public AddProjectForm(ProjectItem projectItem) : this()
        {
            this.projectItem = projectItem;
            ///

            this.tbProjectName.Text = projectItem.Name;
            cbbDeviceType.SelectedItem = (DeviceType)projectItem.DeviceType;
            cbbDeviceIndex.SelectedIndex = projectItem.DeviceIndex;
            //选择第一个Can
            cbbCanIndex.SelectedIndex = -1;
            if (projectItem.CanIndex.Count > 0)
            {
                cbbCanIndex.SelectedIndex = 0;
            }
            //Can口波特率
            cbbBaud.SelectedItem = (BaudRateType)projectItem.CanIndex[0].BaudRate;
        }

        public ProjectItem ProjectItem { get => projectItem; set => projectItem = value; }

        #region Button Click
        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.tbProjectName.Text.Trim()))
            {
                LeapMessageBox.Instance.ShowInfo("Project Name is null!");
                return;
            }

            if(MessageBox.Show("请确认每个Can通道配置已保存","提示", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            if(cbbDeviceIndex.SelectedIndex == -1 || cbbDeviceType.SelectedIndex == -1)
            {
                LeapMessageBox.Instance.ShowInfo("设备类型或设备通道未选择");
                return;
            }

            if (null == projectItem)
                projectItem = new ProjectItem();

            projectItem.Name = this.tbProjectName.Text;
            projectItem.DeviceIndex = this.cbbDeviceIndex.SelectedIndex;
            projectItem.DeviceType = (int)this.cbbDeviceType.SelectedItem;

            switch ((DeviceType)cbbDeviceType.SelectedItem)
            {
                case DeviceType.VCI_USBCAN_2E_U:
                    {
                        
                    }
                    break;
                case DeviceType.VCI_USBCAN1:
                    {
                        //删除多余的通道
                        if (projectItem.CanIndex.Count > 1)
                            projectItem.CanIndex.RemoveAt(1);
                    }
                    break;
            }

            this.BtnSaveChannel_Click(null, null);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cbbCanChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //change channel protocol
            if (cbbCanIndex.SelectedIndex < 0)
            {
                groupBox1.Enabled = false;
            }
            else
            {
                groupBox1.Enabled = true;
                CanIndexItem indexItem = this.projectItem.CanIndex.Find(x => x.CanChannel == cbbCanIndex.SelectedIndex);//改成selectitem
                if (this.projectItem.CanIndex.Count > 0 && null != indexItem)
                {
                    cbChannelUsed.Checked = indexItem.isUsed;
                    cbbProtocolType.SelectedIndex = indexItem.ProtocolType;
                    //add by xwd 2022-03-30 增加xcp
                    if (indexItem.ProtocolType == (int)ProtocolType.XCP)
                    {
                        lbMasterID.Visible = lbSlaveID.Visible = tbMasterID.Visible = tbSlaveID.Visible = true;
                        tbMasterID.Text = indexItem.MasterID.ToString("X");
                        tbSlaveID.Text = indexItem.SlaveID.ToString("X");
                    }
                    else
                    {
                        lbMasterID.Visible = lbSlaveID.Visible = tbMasterID.Visible = tbSlaveID.Visible = false;
                    }
                    //cbbProtocolFiles.SelectedIndex = GetIndexOfCBB(indexItem.ProtocolFileName);
                    ///modified by xwd 2022-02-21
                    ///协议支持多个文件，由单一的combox改为checkbox
                    tBoxProtocols.Text = indexItem.ProtocolFileName;
                    cbbBaud.SelectedItem = (BaudRateType)indexItem.BaudRate;
                }
                else
                {
                    ClearChannelProtocol();
                }
            }
        }

        private void BtnAddChannel_Click(object sender, EventArgs e)
        {
            this.cbbCanIndex.Items.Add(channelCount++);
            cbbCanIndex.SelectedIndex = channelCount - 1;
            LeapMessageBox.Instance.ShowInfo("添加成功，当前通道总数:" + channelCount);
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (projectItem.CanIndex.Count > cbbCanIndex.SelectedIndex)
            {
                projectItem.CanIndex.RemoveAt(this.cbbCanIndex.SelectedIndex);
            }
            this.cbbCanIndex.Items.Remove(this.cbbCanIndex.SelectedItem);
            cbbCanIndex.SelectedIndex = cbbCanIndex.Items.Count - 1;
            channelCount--;
        }

        private void BtnSaveChannel_Click(object sender, EventArgs e)
        {
            CanIndexItem canIndex = projectItem.CanIndex.Find(x => x.CanChannel == cbbCanIndex.SelectedIndex);
            try
            {
                if (null == canIndex)
                {
                    canIndex = new CanIndexItem();
                    projectItem.CanIndex.Add(canIndex);
                }
                canIndex.isUsed = cbChannelUsed.Checked;
                canIndex.CanChannel = cbbCanIndex.SelectedIndex;
                if (cbChannelUsed.Checked)
                {
                    canIndex.ProtocolType = cbbProtocolType.SelectedIndex;
                    ///modified by xwd 2022-02-21 协议文档支持多文件
                    //canIndex.ProtocolFileName = cbbProtocolFiles.SelectedItem.ToString();
                    canIndex.ProtocolFileName = tBoxProtocols.Text;
                    canIndex.BaudRate = (int)(BaudRateType)Enum.Parse(typeof(BaudRateType), cbbBaud.SelectedItem.ToString(), false);
                    if (cbbProtocolType.SelectedIndex == (int)ProtocolType.XCP)
                    {
                        canIndex.MasterID = int.Parse(tbMasterID.Text.Trim(), System.Globalization.NumberStyles.HexNumber);
                        canIndex.SlaveID = int.Parse(tbSlaveID.Text.Trim(), System.Globalization.NumberStyles.HexNumber);
                    }
                }
                else
                {
                    canIndex.ProtocolFileName = "";
                }
            }
            catch (Exception ex)
            {
                LeapMessageBox.Instance.ShowInfo(ex.Message);
                return;
            }
            LeapMessageBox.Instance.ShowInfo("通道【" + cbbCanIndex.SelectedIndex + "】保存成功");

            ///更新信号
            if (projectItem.Form.Count > 0)
            {
                //新的信号
                string[] fileName = projectItem.CanIndex.Find(x => x.CanChannel == cbbCanIndex.SelectedIndex).ProtocolFileName.Split(';');
                if ((ProtocolType)cbbProtocolType.SelectedItem == ProtocolType.DBC)
                {
                    var signals = BaseProtocol.GetSingalsByProtocol(projectItem.CanIndex.Find(x => x.CanChannel == cbbCanIndex.SelectedIndex).ProtocolType, fileName);
                    foreach (var item in projectItem.Form)
                    {
                        if (item.CanChannel == cbbCanIndex.SelectedIndex)
                        {
                            for (int i = 0; i < item.DBCSignals.SignalList.Count; i++)
                            {
                                var newSignal = signals.Find(x => x.SignalName == item.DBCSignals.SignalList[i].SignalName && ((DBCSignal)x).MessageID == ((DBCSignal)item.DBCSignals.SignalList[i]).MessageID);
                                if (newSignal != null)
                                {
                                    item.DBCSignals.SignalList[i] = (DBCSignal)newSignal;
                                }
                            }
                        }
                    }
                }
            }
            LeapMessageBox.Instance.ShowInfo("通道【" + cbbCanIndex.SelectedIndex + "】信号更新成功");
        }

        private void cbbProtocolType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //InitProtocolFiles();
            if (cbbProtocolType.SelectedIndex == -1)
                return;

            if ((ProtocolType)cbbProtocolType.SelectedItem == ProtocolType.XCP)
            {
                this.lbMasterID.Visible = lbSlaveID.Visible = tbMasterID.Visible = tbSlaveID.Visible = true;
            }
            else
            {
                this.lbMasterID.Visible = lbSlaveID.Visible = tbMasterID.Visible = tbSlaveID.Visible = false;
            }
        }

        private void btnImportProtocolFile_Click(object sender, EventArgs e)
        {
            if (cbbProtocolType.SelectedIndex == -1)
                return;
            string filetype = GetFileTypeEnum((ProtocolType)cbbProtocolType.SelectedIndex);

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = $"{(ProtocolType)cbbProtocolType.SelectedIndex}|" + filetype;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //file.copy 
                string configPath = Application.StartupPath + "\\" + this.cbbProtocolType.SelectedValue;
                if (!Directory.Exists(configPath))
                {
                    Directory.CreateDirectory(configPath);
                }
                File.Copy(ofd.FileName, configPath+ofd.FileName.Substring(ofd.FileName.LastIndexOf('\\')), true);

                //select protocol file name change
                InitProtocolFiles();
                this.cbbProtocolFiles.SelectedIndex = cbbProtocolFiles.FindStringExact(ofd.SafeFileName);
            }
        }

        #endregion


        #region Private Method

        private void ClearChannelProtocol()
        {
            cbChannelUsed.Checked = false;
            cbbProtocolType.SelectedIndex = -1;
            cbbProtocolFiles.SelectedIndex = -1;
        }

        private int GetIndexOfCBB(string fileName)
        {
            int index = -1;
            if (string.IsNullOrEmpty(fileName))
                return index;
            for (int i = 0; i < cbbProtocolFiles.Items.Count; i++)
            {
                if (cbbProtocolFiles.Items[i].ToString().Contains(fileName))
                    return i;
            }

            return index;
        }

        private void InitProtocolFiles()
        {
            if (this.cbbProtocolType.SelectedIndex == -1)
            {
                return;
            }

            this.cbbProtocolFiles.DataSource = null;
            string path = Application.StartupPath;

            string protocolPath = path + "\\" + this.cbbProtocolType.SelectedValue as string;

            if (Directory.Exists(protocolPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(protocolPath);

                string filetype = GetFileTypeEnum((ProtocolType)cbbProtocolType.SelectedIndex);

                FileInfo[] files = directoryInfo.GetFiles(filetype);

                this.cbbProtocolFiles.DataSource = files;
            }
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

        #endregion

        private void cbChannelUsed_CheckedChanged(object sender, EventArgs e)
        {
            tbBaudValue0.Enabled= tbBaudValue1.Enabled = cbbBaud.Enabled = cbbProtocolFiles.Enabled = cbbProtocolType.Enabled=btnSelectProtocol.Enabled =btnImportProtocolFile.Enabled = cbChannelUsed.Checked;
            //tbBaudValue0.Text = tbBaudValue1.Text = "";
        }

        private void cbbBaud_SelectedIndexChanged(object sender, EventArgs e)
        {
            //设备类型不同，波特率设置方式不同，选择的波特显示值不同
            bool v = baudRateValue0.TryGetValue((BaudRateType)Enum.Parse(typeof(BaudRateType), cbbBaud.SelectedItem.ToString(), false), out string x);
            tbBaudValue0.Text = v ? x : "无效";

            v = baudRateValue1.TryGetValue((BaudRateType)Enum.Parse(typeof(BaudRateType), cbbBaud.SelectedItem.ToString(), false), out x);
            tbBaudValue1.Text = v ? x : "无效";
        }

        private void cbbDeviceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //设备类型不同，波特率设置方式不同，选择的波特显示值不同
            switch ((DeviceType)cbbDeviceType.SelectedItem)
            {
                ///删减设备类型
                //case DeviceType.VCI_PCI5010U:
                //case DeviceType.VCI_PCI5020U:
                //case DeviceType.VCI_USBCAN_E_U:
                case DeviceType.VCI_USBCAN_2E_U:
                    {
                        tbBaudValue1.Visible = lbBaudValue1.Visible = false;
                        baudRateValue0 = new Dictionary<BaudRateType, string>()
                        {
                            { BaudRateType.Kbps5, "0x1c01c1" },
                            {BaudRateType.Kbps1000,"0x060003"},
                            {BaudRateType.Kbps800,"0x060004"},
                            {BaudRateType.Kbps500,"0x060007"},
                            {BaudRateType.Kbps250,"0x1c0008"},
                            {BaudRateType.Kbps125,"0x1c0011"},
                            {BaudRateType.Kbps100,"0x160023"},
                            {BaudRateType.Kbps50,"0x1c002c"},
                            {BaudRateType.Kbps20,"0x1600b3"},
                            {BaudRateType.Kbps10,"0x1c00e0"},
                        };
                        cbbCanIndex.Items.Clear();
                        cbbCanIndex.Items.Add(0);
                        cbbCanIndex.Items.Add(1);
                        cbbCanIndex.SelectedIndex = 0;
                    }
                    break;
                case DeviceType.USBCANFD_200U:
                    tbBaudValue1.Visible = lbBaudValue1.Visible = false;
                    cbbCanIndex.Items.Clear();
                    cbbCanIndex.Items.Add(0);
                    cbbCanIndex.Items.Add(1);
                    cbbCanIndex.SelectedIndex = 0;
                    break;
                default:
                    {
                        baudRateValue0 = new Dictionary<BaudRateType, string>()
                        {
                            { BaudRateType.Kbps5,"0xBF"},
                            { BaudRateType.Kbps10,"0x31"},
                            { BaudRateType.Kbps20,"0x18"},
                            //{ BaudRateType.Kbps40,"0x87"},
                            { BaudRateType.Kbps50,"0x09"},
                            { BaudRateType.Kbps100,"0x04"},
                            { BaudRateType.Kbps125,"0x03"},
                            { BaudRateType.Kbps250,"0x01"},
                            { BaudRateType.Kbps500,"0x00"},
                            { BaudRateType.Kbps800,"0x00"},
                            { BaudRateType.Kbps1000,"0x00"},
                        };
                        baudRateValue1 = new Dictionary<BaudRateType, string>() { 
                            { BaudRateType.Kbps5,"0xFF"},
                            { BaudRateType.Kbps10,"0x1C"},
                            { BaudRateType.Kbps20,"0x1C"},
                            //{ BaudRateType.Kbps40,"0x87"},
                            { BaudRateType.Kbps50,"0x1C"},
                            { BaudRateType.Kbps100,"0x1C"},
                            { BaudRateType.Kbps125,"0x1C"},
                            { BaudRateType.Kbps250,"0x1C"},
                            { BaudRateType.Kbps500,"0x1C"},
                            { BaudRateType.Kbps800,"0x16"},
                            { BaudRateType.Kbps1000,"0x14"}, };
                    }
                    tbBaudValue1.Visible = lbBaudValue1.Visible = true;
                    cbbCanIndex.Items.Clear();
                    cbbCanIndex.Items.Add(0);
                    cbbCanIndex.SelectedIndex = 0;
                    
                    break;
            }

            cbbBaud_SelectedIndexChanged(null,null);
        }

        

        private void btnSelectProtocol_Click(object sender, EventArgs e)
        {
            ProtocolsForm pForm = new ProtocolsForm((ProtocolType)cbbProtocolType.SelectedIndex, this.tBoxProtocols.Text);
            if(pForm.ShowDialog() == DialogResult.OK)
            {
                this.tBoxProtocols.Text = pForm.FileNames;
            }
        }
    }
}
