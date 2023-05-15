using AppTest.DBCLib;
using AppTest.FormType.Helper;
using AppTest.Helper;
using AppTest.Model;
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
using AppTest.ProtocolLib;

namespace AppTest.FormType
{
    public partial class AddNewForm : MetroForm
    {
        protected FormItem formItem;
        BindingList<DBCSignal> SelectedDbcSignals;
        //public FormItem FormItem { get { return this.formItem; } }

        protected ProjectItem projectItem;

        /// <summary>
        /// 新增还是编辑;true:编辑；False：新增
        /// </summary>
        protected bool isEdit = false;

        public AddNewForm()
        {
            InitializeComponent();
            MaximizeBox = false;
            cbbFormType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbbFormType.DataSource = Enum.GetValues(typeof(FormType));
            lbSelectedNode.Sorted = true;
            //metroGrid_Signals.AllowUserToAddRows = false;
            dataGridView1.AllowUserToAddRows = false;
            //metroGrid_Signals.AutoGenerateColumns = false;
            dataGridView1.AutoGenerateColumns = false;
        }

        public void SetColumnVisible(bool visible)
        {
            MessageIDColumn.Visible = visible;
            startBitDataGridViewTextBoxColumn1.Visible = visible;
            byteOrderDataGridViewTextBoxColumn1.Visible = visible;
            valueTypeDataGridViewTextBoxColumn1.Visible = visible;
            factorDataGridViewTextBoxColumn1.Visible = visible;
            offsetDataGridViewTextBoxColumn1.Visible = visible;
            strValueDataGridViewTextBoxColumn1.Visible = visible;
            commentDataGridViewTextBoxColumn1.Visible = visible;
        }

        public AddNewForm(ProjectItem projectItem) :this()
        {
            this.projectItem = projectItem;
            projectItem.CanIndex.Sort();
            foreach (var item in projectItem.CanIndex)
            {
                if (item.isUsed)
                {
                    cbbCanIndex.Items.Add(item.CanChannel);
                }
            }
            Init();
        }

       
        /// <summary>
        /// 编辑信号
        /// </summary>
        /// <param name="projectItem"></param>
        /// <param name="formitem"></param>
        public AddNewForm(ProjectItem projectItem, FormItem formitem) : this(projectItem)
        {
            this.Text = "修改信号";
            this.formItem = formitem;
            isEdit = true;
            cbbCanIndex.Enabled = false;
            cbbCanIndex.SelectedItem = formitem.CanChannel;
            tbFormName.Enabled = false;
            tbFormName.Text = formitem.Name;
            cbbFormType.SelectedIndex = formitem.FormType;
            cbbFormType.Enabled = false;

            //modified by xwd 2022-02-17
            //显示有顺序要求，不用foreach改用for
            lbSelectedNode.Sorted = false;

            
        }

        public AddNewForm(ProjectItem projectItem, FormItem formitem, bool modifiedName) : this(projectItem, formitem)
        {
            cbbCanIndex.Enabled = modifiedName;
            tbFormName.Enabled = modifiedName;
            cbbFormType.Enabled = modifiedName;
        }

        protected virtual void Init()
        {
            SelectedDbcSignals = new BindingList<DBCSignal>();
            dataGridView1.DataSource = SelectedDbcSignals;
            //metroGrid_Signals.DataSource = SelectedDbcSignals;

            //messageIDDataGridViewTextBoxColumn1.Visible = true;
        }


        public FormItem FormItem { get => formItem; }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!CheckNameNull())
                return;

            //获取选中的信号
            if (!isEdit)
            {
                formItem = new FormItem();
            }
            formItem.Name = tbFormName.Text;
            formItem.FormType = cbbFormType.SelectedIndex;
            FormItem.CanChannel = (int)cbbCanIndex.SelectedItem;

            SaveSignals();
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private  void cbbCanIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAllSignalsFromFile();

            if (this.FormItem != null)
                LoadSelectedSignals(this.FormItem);
        }

        DBCSignals allSingals;

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddSignal();

            //foreach (TreeNode msgID in tvAllNode.Nodes)
            //{
            //    foreach (TreeNode signal in msgID.Nodes)
            //    {
            //        if (signal.Checked)// && !lbSelectedNode.Items.Contains(signal.Text))
            //        {
            //            if (!SelectedSignals.Any(x => x.SignalName == signal.Text))
            //            {
            //                lbSelectedNode.Items.Add(signal.Text);
            //                SelectedSignals.Add(allSingals.SignalList.Find(x => x.SignalName == signal.Text));
            //                //bs.ad
            //            }

            //        }
            //    }
            //}
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            RemoveSignal();
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
            if (!CheckSignalNull())
                return;
            string queryText = tbSignalText.Text.Trim();
            QueryBySignalName(queryText);
        }

        private void cbbFormType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeType();
            
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
            AddCustomSignal();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            //上移
            //if (lbSelectedNode.SelectedIndex != 0 && lbSelectedNode.SelectedIndex != -1)
            //{
            //    int oldIndex = lbSelectedNode.SelectedIndex;
            //    var upValue = lbSelectedNode.Items[oldIndex - 1];
            //    lbSelectedNode.Items[oldIndex - 1] = lbSelectedNode.SelectedItem;
            //    lbSelectedNode.Items[oldIndex] = upValue;
            //    lbSelectedNode.SetSelected(oldIndex - 1, true);
            //    lbSelectedNode.SetSelected(oldIndex, false);
            //}
            UpDataGridView(this.dataGridView1);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            //下移
            //if (lbSelectedNode.SelectedIndex != lbSelectedNode.Items.Count - 1 && lbSelectedNode.SelectedIndex != -1)
            //{
            //    int oldIndex = lbSelectedNode.SelectedIndex;
            //    var upValue = lbSelectedNode.Items[oldIndex + 1];
            //    lbSelectedNode.Items[oldIndex + 1] = lbSelectedNode.SelectedItem;
            //    lbSelectedNode.Items[oldIndex] = upValue;
            //    lbSelectedNode.SetSelected(oldIndex + 1, true);
            //    lbSelectedNode.SetSelected(oldIndex, false);
            //}
            DownDataGridView(this.dataGridView1);
        }

        /// 
        /// 上移        /// 
        /// 
        protected virtual void UpDataGridView(DataGridView dataGridView)
        {
            try
            {
                DataGridViewSelectedRowCollection dgvsrc = dataGridView.SelectedRows;//获取选中行的集合
                if (dgvsrc.Count > 0)
                {
                    int index = dataGridView.SelectedRows[0].Index;//获取当前选中行的索引
                    if (index > 0)
                    {//如果该行不是第一行
                        var temp = SelectedDbcSignals[index];
                        SelectedDbcSignals[index] = SelectedDbcSignals[index - 1];
                        SelectedDbcSignals[index - 1] = temp;
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
        protected virtual void DownDataGridView(DataGridView dataGridView)
        {
            try
            {
                DataGridViewSelectedRowCollection dgvsrc = dataGridView.SelectedRows;//获取选中行的集合
                if (dgvsrc.Count > 0)
                {
                    int index = dataGridView.SelectedRows[0].Index;//获取当前选中行的索引
                    if (index >= 0 & (dataGridView.RowCount - 1) != index)
                    {//如果该行不是最后一行
                        var temp = SelectedDbcSignals[index];
                        SelectedDbcSignals[index] = SelectedDbcSignals[index + 1];
                        SelectedDbcSignals[index + 1] = temp;
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
            ModifiedSignal();
        }

        #region Override Method

        /// <summary>
        /// 修改窗口类型执行的动作
        /// </summary>
        protected virtual void ChangeType()
        {
            //f(cbbFormType.SelectedIndex == (int)FormType.RollingCounter)
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

        /// <summary>
        /// 根据信号的层次，dbc信号根据ID分组，所以node遍历2层
        /// </summary>
        protected virtual void AddSignal()
        {
            foreach (TreeNode msgID in tvAllNode.Nodes)
            {
                foreach (TreeNode signal in msgID.Nodes)
                {
                    if (signal.Checked)
                    {
                        //lbSelectedNode.Items.Add(signal.Text);
                        if (SelectedDbcSignals.FirstOrDefault(x => x.SignalName == signal.Text && x.MessageID == msgID.Text) == null)
                            SelectedDbcSignals.Add(allSingals.SignalList.Find(x => x.SignalName == signal.Text && x.MessageID == msgID.Text));
                    }
                }
            }
        }

        protected virtual void RemoveSignal()
        {
            //while (lbSelectedNode.SelectedItems.Count > 0)
            //{
            //    string signalName = lbSelectedNode.SelectedItem.ToString();
            //    lbSelectedNode.Items.Remove(signalName);
            //}
            while (dataGridView1.SelectedRows.Count > 0)
            {
                DBCSignal s = dataGridView1.SelectedRows[0].DataBoundItem as DBCSignal;
                SelectedDbcSignals.Remove(s);
            }
        }

        /// <summary>
        /// 添加自定义信号
        /// </summary>
        protected virtual void AddCustomSignal()
        {
            SignalItemForm<DBCSignal> sif = new SignalItemForm<DBCSignal>(false);
            sif.StartPosition = FormStartPosition.CenterParent;
            if (sif.ShowDialog() == DialogResult.OK)
            {
                if (allSingals.SignalList.Find(x => x.MessageID == sif.TValue.MessageID && x.StartBit == sif.TValue.StartBit) != null)
                {
                    allSingals.SignalList.Remove(allSingals.SignalList.Find(x => x.MessageID == sif.TValue.MessageID && x.StartBit == sif.TValue.StartBit));
                }
                allSingals.SignalList.Add(sif.TValue);
                lbSelectedNode.Items.Add(sif.TValue.SignalName);
                SelectedDbcSignals.Add(allSingals.SignalList.Find(x => x.SignalName == sif.TValue.SignalName));
            }
        }

        /// <summary>
        /// 查找信号
        /// </summary>
        /// <param name="signalName"></param>
        protected virtual void QueryBySignalName(string signalName)
        {
            tvAllNode.Nodes.Clear();
            List<DBCSignal> result;
            if (!string.IsNullOrEmpty(signalName))
            {
                result = (from c in allSingals.SignalList
                          where c.SignalName.ToLower().IndexOf(signalName.ToLower()) >= 0 || ((DBCSignal)c).MessageID.ToLower().IndexOf(signalName.ToLower()) >= 0
                          select c).ToList();
            }
            else
            {
                result = allSingals.SignalList;
            }
            foreach (var item in result)
            {
                if (item is DBCSignal)
                {
                    DBCSignal dBCSignal = item as DBCSignal;
                    if (tvAllNode.Nodes.Find(dBCSignal.MessageID, false).Count() == 0)
                    {
                        TreeNode tn = new TreeNode()
                        {
                            Name = dBCSignal.MessageID,
                            Text = dBCSignal.MessageID
                        };
                        tvAllNode.Nodes.Add(tn);
                        tvAllNode.Nodes[tvAllNode.Nodes.Count - 1].Nodes.Add(dBCSignal.SignalName);
                    }
                    else
                    {
                        tvAllNode.Nodes[dBCSignal.MessageID].Nodes.Add(dBCSignal.SignalName);
                    }
                }

            }
            tvAllNode.ExpandAll();
        }

        protected virtual bool CheckNameNull()
        {
            if (string.IsNullOrEmpty(tbFormName.Text.Trim()))
            {
                LeapMessageBox.Instance.ShowInfo("名称不能为空");
                tbFormName.Focus();
                return false;
            }
            if (!isEdit && projectItem.Form.Find(x => x.Name == tbFormName.Text) != null)
            {
                LeapMessageBox.Instance.ShowInfo("Form名称重复");
                tbFormName.Focus();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 是否加载信号
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckSignalNull()
        {
            if (allSingals == null || allSingals.SignalList.Count == 0)
            {
                LeapMessageBox.Instance.ShowInfo("请先选择通道，或者该协议中没有读取到信号");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 信号存储
        /// </summary>
        protected virtual void SaveSignals()
        {
            formItem.DBCSignals = new DBCSignals();
            formItem.DBCSignals.SignalList = SelectedDbcSignals.ToList();
            foreach (var item in lbSelectedNode.Items)
            {
               
                //formItem.DBCSignals.SignalList.Add(allSingals.SignalList.Find(x => x.SignalName == item.ToString()));
            }
        }

        /// <summary>
        /// 加载已选中的信号
        /// </summary>
        protected virtual void LoadSelectedSignals(FormItem formitem)
        {
            foreach (var item in formitem.DBCSignals.SignalList)
            {
                SelectedDbcSignals.Add(item);
            }

            if (formitem.DBCSignals != null)
            {
                for (int i = 0; i < formitem.DBCSignals.SignalList.Count; i++)
                {
                    var signal = formitem.DBCSignals.SignalList[i];
                    lbSelectedNode.Items.Add(signal.SignalName);
                    if (allSingals.SignalList.Find(x => x.SignalName == signal.SignalName && ((DBCSignal)x).MessageID == ((DBCSignal)signal).MessageID) == null)
                    {
                        allSingals.SignalList.Add(signal);
                    }
                }
            }
        }

        /// <summary>
        /// 从协议文档中加载所有信号
        /// </summary>
        protected virtual void LoadAllSignalsFromFile()
        {
            //Load Protocol
            string fileName = projectItem.CanIndex.Find(x => x.CanChannel == cbbCanIndex.SelectedIndex).ProtocolFileName;
            lbSelectedNode.Items.Clear();
            tvAllNode.Nodes.Clear();
            //find File from bin

            try
            {
                ///modified by xwd 2022-02-21 单通道支持多个协议文档
                var fileNames = fileName.Split(';');
                allSingals = new DBCSignals();
                allSingals.SignalList = new List<DBCSignal>();
                foreach (var signal in BaseProtocol.GetSingalsByProtocol(projectItem.CanIndex.Find(x => x.CanChannel == cbbCanIndex.SelectedIndex).ProtocolType, fileNames))
                {
                    if (signal is DBCSignal)
                        allSingals.SignalList.Add((DBCSignal)signal);
                }

                allSingals.SignalList.Sort();
                foreach (var item in allSingals.SignalList)
                {
                    if (item is DBCSignal)
                    {
                        DBCSignal dBCSignal = item as DBCSignal;
                        if (tvAllNode.Nodes.Find(dBCSignal.MessageID, false).Count() == 0)
                        {
                            TreeNode tn = new TreeNode()
                            {
                                Name = dBCSignal.MessageID,
                                Text = dBCSignal.MessageID
                            };
                            tvAllNode.Nodes.Add(tn);
                            tvAllNode.Nodes[tvAllNode.Nodes.Count - 1].Nodes.Add(dBCSignal.SignalName);
                        }
                        else
                        {
                            tvAllNode.Nodes[dBCSignal.MessageID].Nodes.Add(dBCSignal.SignalName);
                        }
                    }
                    // listView1.Items.Add();
                    //lbAllNode.Items.Add(item.SignalName);
                }
            }
            catch (Exception ex)
            {
                LeapMessageBox.Instance.ShowError(ex.ToString());
            }
        }

        /// <summary>
        /// 查看/修改信号信息
        /// </summary>
        protected virtual void ModifiedSignal()
        {
            string name = lbSelectedNode.SelectedItem.ToString();
            DBCSignal signal = allSingals.SignalList.Find(x => x.SignalName == name);
            if (signal == null)
                return;
            SignalItemForm<DBCSignal> sif = new SignalItemForm<DBCSignal>(signal, isReadOnly: false);
            if (sif.ShowDialog() == DialogResult.OK)
            {
                lbSelectedNode.Items.Remove(name);
                if (allSingals.SignalList.Find(x => x.MessageID == sif.TValue.MessageID && x.StartBit == sif.TValue.StartBit) != null)
                {
                    allSingals.SignalList.Remove(allSingals.SignalList.Find(x => x.MessageID == sif.TValue.MessageID && x.StartBit == sif.TValue.StartBit));
                }
                allSingals.SignalList.Add(sif.TValue);
                lbSelectedNode.Items.Add(sif.TValue.SignalName);
            }
        }


        #endregion
    }
}
