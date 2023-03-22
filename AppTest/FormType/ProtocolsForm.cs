using AppTest.FormType.Helper;
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
    public partial class ProtocolsForm : Form
    {
        public ProtocolsForm()
        {
            InitializeComponent();
            this.CenterToParent();
            //this.Location = Parent.
        }

        ProtocolType protocolType;

        public ProtocolsForm(ProtocolType protocolType,string selectedFiles) : this()
        {
            this.protocolType = protocolType;
            InitLoad(protocolType);
            var fileNames = selectedFiles.Split(';').ToList();
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (fileNames.Find(x => x == checkedListBox1.Items[i].ToString()) != null)
                {
                    checkedListBox1.SetItemChecked(i, true);
                }
            }
        }

        private void InitLoad(ProtocolType protocolType)
        {
            checkedListBox1.Items.Clear();
            string filePath = Path.Combine(Application.StartupPath, protocolType.ToString());
            if (Directory.Exists(filePath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(filePath);

                string filetype = GetFileTypeEnum(protocolType);

                FileInfo[] files = directoryInfo.GetFiles(filetype);
                for (int i = 0; i < files.Length; i++)
                {
                    this.checkedListBox1.Items.Add(files[i].Name);
                } 
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

        private void btnAddFile_Click(object sender, EventArgs e)
        {
            string filetype = GetFileTypeEnum(protocolType);
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = $"{protocolType}|" + filetype;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //file.copy 
                string configPath = Application.StartupPath + "\\" + protocolType;
                if (!Directory.Exists(configPath))
                {
                    Directory.CreateDirectory(configPath);
                }
                File.Copy(ofd.FileName, configPath + ofd.FileName.Substring(ofd.FileName.LastIndexOf('\\')), true);
                checkedListBox1.Items.Add(ofd.SafeFileName);
            }
        }

        private void btnDeleteFile_Click(object sender, EventArgs e)
        {
            string x = string.Empty;
            string configPath = Application.StartupPath + "\\" + protocolType;
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    string filename = checkedListBox1.Items[i].ToString();
                    if(File.Exists(configPath+"\\"+ filename))
                    {
                        File.Delete(configPath + "\\" + filename);
                    }
                }
            }
            //MessageBox.Show(x);
            InitLoad(protocolType);
        }

        public string FileNames = string.Empty;

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    FileNames += checkedListBox1.Items[i].ToString() + ";";
                }
            }
            FileNames = FileNames.Substring(0, FileNames.Length - 1);
            this.Close();
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
