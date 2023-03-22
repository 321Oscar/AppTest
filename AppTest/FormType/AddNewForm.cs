using AppTest.DBCLib;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace AppTest.FormType
{
    public partial class AddNewForm : MetroForm
    {
        private FormItem formItem;
        BindingList<DBCSignal> SelectedSignals;
        //public FormItem FormItem { get { return this.formItem; } }

        private ProjectItem projectItem;

        /// <summary>
        /// 新增还是编辑;true:编辑；False：新增
        /// </summary>
        private bool isEdit = false;

        public AddNewForm()
        {
            InitializeComponent();
            MaximizeBox = false;
            cbbFormType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbbFormType.DataSource = Enum.GetValues(typeof(FormType));
            lbSelectedNode.Sorted = true;
            metroGrid_Signals.AllowUserToAddRows = false;
            dataGridView1.AllowUserToAddRows = false;
            metroGrid_Signals.AutoGenerateColumns = false;
            dataGridView1.AutoGenerateColumns = false;
        }

        public AddNewForm(ProjectItem projectItem) :this()
        {
            this.projectItem = projectItem;

            foreach (var item in projectItem.CanIndex)
            {
                if (item.isUsed)
                {
                    cbbCanIndex.Items.Add(item.CanChannel);
                }
            }
            SelectedSignals = new BindingList<DBCSignal>();
            dataGridView1.DataSource = SelectedSignals;
        }

        /// <summary>
        /// 编辑信号
        /// </summary>
        /// <param name="projectItem"></param>
        /// <param name="formitem"></param>
        public AddNewForm(ProjectItem projectItem, FormItem formitem) : this(projectItem)
        {
            this.Text = "修改信号";
            isEdit = true;
            cbbCanIndex.Enabled = false;
            cbbCanIndex.SelectedItem = formitem.CanChannel;
            tbFormName.Enabled = false;
            tbFormName.Text = formitem.Name;
            cbbFormType.SelectedIndex = formitem.FormType;
            cbbFormType.Enabled = false;
            this.formItem = formitem;
            //load selected signals
            SelectedSignals = new BindingList<DBCSignal>(formitem.Singals.Signal);
            dataGridView1.DataSource = SelectedSignals;
            metroGrid_Signals.DataSource = SelectedSignals;

            //modified by xwd 2022-02-17
            //显示有顺序要求，不用foreach改用for
            lbSelectedNode.Sorted = false;

            for (int i = 0; i < formitem.Singals.Signal.Count; i++)
            {
                var signal = formitem.Singals.Signal[i];
                //添加CustomName
                if (!string.IsNullOrEmpty(signal.CustomName))
                {
                    DBCSignal s = allSingals.Signal.Find(x => x.SignalName == signal.SignalName);
                    s.CustomName = signal.CustomName;
                }
                //添加到右边listbox
                lbSelectedNode.Items.Add(signal.SignalName);

                //自定义信号，协议文档中没有的信号
                if (allSingals.Signal.Find(x => x.SignalName == signal.SignalName && x.MessageID == signal.MessageID) == null)
                {
                    allSingals.Signal.Add(signal);
                }
            }
            //foreach (var signal in formitem.Singals.Signal)
            //{
            //    lbSelectedNode.Items.Add(signal.SignalName);
            //    if (allSingals.Signal.Find(x=>x.SignalName == signal.SignalName && x.MessageID == signal.MessageID) == null)
            //    {
            //        allSingals.Signal.Add(signal);
            //    }
            //}
            
        }

        public AddNewForm(ProjectItem projectItem, FormItem formitem, bool modifiedName) : this(projectItem, formitem)
        {
            cbbCanIndex.Enabled = modifiedName;
            tbFormName.Enabled = modifiedName;
            cbbFormType.Enabled = modifiedName;
        }

        public FormItem FormItem { get => formItem; }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbFormName.Text.Trim()))
            {
                LeapMessageBox.Instance.ShowInfo("名称不能为空");
                tbFormName.Focus();
                return;
            }
            if (!isEdit && projectItem.Form.Find(x => x.Name == tbFormName.Text) != null)
            {
                LeapMessageBox.Instance.ShowInfo("Form名称重复");
                tbFormName.Focus();
                return;
            }
            
            //获取选中的信号
            if (!isEdit)
            {
                formItem = new FormItem();
            }
            formItem.Name = tbFormName.Text;
            formItem.FormType = cbbFormType.SelectedIndex;
            FormItem.CanChannel = (int)cbbCanIndex.SelectedItem;

            if (this.lbSelectedNode.Items == null || lbSelectedNode.Items.Count == 0)
            {
                MessageBox.Show("没有选中测量信号");
            }
            else
            {
                formItem.Singals = new Singals();
                formItem.Singals.Signal = SelectedSignals.ToList();
                //foreach (var item in lbSelectedNode.Items)
                //{
                //    formItem.Singals.Signal.Add(allSingals.Signal.Find(x => x.SignalName == item.ToString()));
                //}
            }
            if (cbbFormType.SelectedIndex == (int)FormType.RollingCounter)
            {
                //校验周期
                //foreach (var item in formItem.Singals.Signal)
                //{

                //}
                //if (formItem.Singals.Signal[0].CycleTime == 0)
                //{
                //    LeapMessageBox.Instance.ShowInfo($"【{formItem.Singals.Signal[0].MessageID}】该信号周期为0，不支持自动发送");
                //    return;

                //}
            }


            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cbbCanIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Load Protocol
            string fileName = projectItem.CanIndex.Find(x => x.CanChannel == (int)cbbCanIndex.SelectedItem).ProtocolFileName;
            lbSelectedNode.Items.Clear();
            tvAllNode.Nodes.Clear();
            //find File from bin

            try
            {
                ///modified by xwd 2022-02-21 单通道支持多个协议文档
                var fileNames = fileName.Split(';');
                allSingals = BaseProtocol.GetSingalsByProtocol(projectItem.CanIndex.Find(x => x.CanChannel == (int)cbbCanIndex.SelectedItem).ProtocolType, fileNames);
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

        Singals allSingals;

        private void btnAdd_Click(object sender, EventArgs e)
        {
            foreach (TreeNode msgID in tvAllNode.Nodes)
            {
                foreach (TreeNode signal in msgID.Nodes)
                {
                    if (signal.Checked && !lbSelectedNode.Items.Contains(signal.Text))
                    {
                        lbSelectedNode.Items.Add(signal.Text);
                        SelectedSignals.Add(allSingals.Signal.Find(x => x.SignalName == signal.Text));
                        //bs.ad
                    }
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            while(lbSelectedNode.SelectedItems.Count > 0)
            {
                string signalName = lbSelectedNode.SelectedItem.ToString();
                lbSelectedNode.Items.Remove(signalName);
            }
            while(dataGridView1.SelectedRows.Count > 0)
            {
                DBCSignal s = dataGridView1.SelectedRows[0].DataBoundItem as DBCSignal;
                SelectedSignals.Remove(s);
            }
           
        }

        private static List<T> Swap<T>(List<T> list, int index1, int index2)
        {
            var temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
            return list;
        }

        private void tvAllNode_AfterCheck(object sender, TreeViewEventArgs e)
        {
            CheckAllChildren(e.Node);
        }

        private void CheckAllChildren(TreeNode tn)
        {
            foreach (TreeNode item in tn.Nodes)
            {
                item.Checked = tn.Checked;
                CheckAllChildren(item);
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if(allSingals == null || allSingals.Signal.Count == 0)
            {
                LeapMessageBox.Instance.ShowInfo("请先选择通道，或者该协议中没有读取到信号");
                return;
            }
            string queryText = tbSignalText.Text.Trim();
            QueryBySignalName(queryText);
        }

        private void QueryBySignalName(string signalName)
        {
            tvAllNode.Nodes.Clear();
            if (!string.IsNullOrEmpty(signalName))
            {
                var x = (from c in allSingals.Signal
                         where c.SignalName.ToLower().IndexOf(signalName.ToLower()) >= 0 || c.MessageID.ToLower().IndexOf(signalName.ToLower()) >= 0
                         select c).ToList();
                foreach (var item in x)
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

                    // listView1.Items.Add();
                    //lbAllNode.Items.Add(item.SignalName);
                }
                tvAllNode.ExpandAll();
            }
            else
            {
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

                    // listView1.Items.Add();
                    //lbAllNode.Items.Add(item.SignalName);
                }
            }
        }

        private void cbbFormType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if(cbbFormType.SelectedIndex == (int)FormType.RollingCounter)
            //{
            //    allSingals.Signal = (from c in allSingals.Signal
            //             where c.SignalName.ToLower().IndexOf("rolling") >= 0
            //             select c).ToList();
            //    tvAllNode.Nodes.Clear();
            //    foreach (var item in allSingals.Signal)
            //    {
            //        if (tvAllNode.Nodes.Find(item.MessageID, false).Count() == 0)
            //        {
            //            TreeNode tn = new TreeNode()
            //            {
            //                Name = item.MessageID,
            //                Text = item.MessageID
            //            };
            //            tvAllNode.Nodes.Add(tn);
            //            tvAllNode.Nodes[tvAllNode.Nodes.Count - 1].Nodes.Add(item.SignalName);
            //        }
            //        else
            //        {
            //            tvAllNode.Nodes[item.MessageID].Nodes.Add(item.SignalName);
            //        }

            //        // listView1.Items.Add();
            //        lbAllNode.Items.Add(item.SignalName);
            //    }
            //}
            //else
            //{
            //    allSingals = BaseProtocol.GetSingalsByProtocol(projectItem.CanIndex.Find(x => x.CanChannel == cbbCanIndex.SelectedIndex).ProtocolType, fileName);
            //}
        }

        private void tbSignalText_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnQuery_Click(null,null);
            }
        }

        private void btnCustomSignal_Click(object sender, EventArgs e)
        {
            SignalItemForm<DBCSignal> sif = new SignalItemForm<DBCSignal>(false);
            if(sif.ShowDialog() == DialogResult.OK)
            {
                if (allSingals.Signal.Find(x => x.MessageID == sif.TValue.MessageID && x.StartBit == sif.TValue.StartBit) != null)
                {
                    allSingals.Signal.Remove(allSingals.Signal.Find(x => x.MessageID == sif.TValue.MessageID && x.StartBit == sif.TValue.StartBit));
                }
                allSingals.Signal.Add(sif.TValue);
                lbSelectedNode.Items.Add(sif.TValue.SignalName);
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            //上移
            if (lbSelectedNode.SelectedIndex != 0 && lbSelectedNode.SelectedIndex != -1)
            {
                int oldIndex = lbSelectedNode.SelectedIndex;
                var upValue = lbSelectedNode.Items[oldIndex - 1];
                lbSelectedNode.Items[oldIndex - 1] = lbSelectedNode.SelectedItem;
                lbSelectedNode.Items[oldIndex] = upValue;
                lbSelectedNode.SetSelected(oldIndex - 1, true);
                lbSelectedNode.SetSelected(oldIndex, false);
            }
            UpDataGridView(this.dataGridView1);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            //下移
            if (lbSelectedNode.SelectedIndex != lbSelectedNode.Items.Count - 1 && lbSelectedNode.SelectedIndex != -1)
            {
                int oldIndex = lbSelectedNode.SelectedIndex;
                var upValue = lbSelectedNode.Items[oldIndex + 1];
                lbSelectedNode.Items[oldIndex + 1] = lbSelectedNode.SelectedItem;
                lbSelectedNode.Items[oldIndex] = upValue;
                lbSelectedNode.SetSelected(oldIndex + 1, true);
                lbSelectedNode.SetSelected(oldIndex, false);
            }
            DownDataGridView(this.dataGridView1);
        }

        /// 
        /// 上移        /// 
        /// 
        public void UpDataGridView(DataGridView dataGridView)
        {
            try
            {
                DataGridViewSelectedRowCollection dgvsrc = dataGridView.SelectedRows;//获取选中行的集合
                if (dgvsrc.Count > 0)
                {
                    int index = dataGridView.SelectedRows[0].Index;//获取当前选中行的索引
                    if (index > 0)
                    {//如果该行不是第一行
                        var temp = SelectedSignals[index];
                        SelectedSignals[index] = SelectedSignals[index - 1];
                        SelectedSignals[index - 1] = temp;
                        dataGridView1.Rows[index].Selected = false;
                        dataGridView1.Rows[index - 1].Selected = true;
                        //DataGridViewRow dgvr = dataGridView.Rows[index - dgvsrc.Count];//获取选中行的上一行
                        //dataGridView.Rows.RemoveAt(index - dgvsrc.Count);//删除原选中行的上一行
                        //dataGridView.Rows.Insert((index), dgvr);//将选中行的上一行插入到选中行的后面
                        //for (int i = 0; i < dgvsrc.Count; i++)//选中移动后的行                        {
                        //    dataGridView.Rows[index - i - 1].Selected = true;
                    }
                }
            }
                
               catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }        /// 
        public void DownDataGridView(DataGridView dataGridView)
        {
            try
            {
                DataGridViewSelectedRowCollection dgvsrc = dataGridView.SelectedRows;//获取选中行的集合
                if (dgvsrc.Count > 0)
                {
                    int index = dataGridView.SelectedRows[0].Index;//获取当前选中行的索引
                    if (index >= 0 & (dataGridView.RowCount - 1) != index)
                    {//如果该行不是最后一行
                        var temp = SelectedSignals[index];
                        SelectedSignals[index] = SelectedSignals[index + 1];
                        SelectedSignals[index + 1] = temp;
                        dataGridView1.Rows[index].Selected = false;
                        dataGridView1.Rows[index + 1].Selected = true;
                        //dataGridViel
                        //DataGridViewRow dgvr = dataGridView.Rows[index + 1];//获取选中行的下一行
                        //dataGridView.Rows.RemoveAt(index + 1);//删除原选中行的上一行
                        //dataGridView.Rows.Insert((index + 1 - dgvsrc.Count), dgvr);//将选中行的上一行插入到选中行的后面
                        //for (int i = 0; i < dgvsrc.Count; i++)
                        //{//选中移动后的行                        
                        //    dataGridView.Rows[index + 1 - i].Selected = true;
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }        /// 


        private void lbSelectedNode_DoubleClick(object sender, EventArgs e)
        {
            string name = lbSelectedNode.SelectedItem.ToString();
            DBCSignal signal = allSingals.Signal.Find(x => x.SignalName == name);
            if (signal == null)
                return;
            SignalItemForm<DBCSignal> sif = new SignalItemForm<DBCSignal>(signal,isReadOnly:false);
            if (sif.ShowDialog() == DialogResult.OK)
            {
                lbSelectedNode.Items.Remove(name);
                if (allSingals.Signal.Find(x => x.MessageID == sif.TValue.MessageID && x.StartBit == sif.TValue.StartBit) != null)
                {
                    allSingals.Signal.Remove(allSingals.Signal.Find(x => x.MessageID == sif.TValue.MessageID && x.StartBit == sif.TValue.StartBit));
                }
                allSingals.Signal.Add(sif.TValue);
                lbSelectedNode.Items.Add(sif.TValue.SignalName);
            }
        }
    }
}
