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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppTest.FormType
{
    public partial class AddNewXcpForm : AddNewForm
    {
        #region 构造函数
        DataGridViewTextBoxColumn eventNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn
        {
            DataPropertyName = "EventName",
            Name = "eventNameDataGridViewTextBoxColumn",
            HeaderText = "EventName"
        };
        DataGridViewTextBoxColumn DAQIDDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn
        {
            DataPropertyName = "DAQID",
            Name = "DAQIDDataGridViewTextBoxColumn",
            HeaderText ="DaqID",
            Visible = false,
            ReadOnly = true
        };
        DataGridViewTextBoxColumn eventIDDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn
        {
            DataPropertyName = "EventID",
            Name = "eventIDDataGridViewTextBoxColumn",
            HeaderText = "EventID",
            ReadOnly = true
        };

        public AddNewXcpForm():base()
        {
            InitializeComponent();
        }

        public AddNewXcpForm(ProjectItem projectItem) : base(projectItem)
        {
            this.Text = "新建XCP窗口";
        }

        /// <summary>
        /// 编辑信号
        /// </summary>
        /// <param name="projectItem"></param>
        /// <param name="formitem"></param>
        public AddNewXcpForm(ProjectItem projectItem, FormItem formitem) : base(projectItem, formitem)
        {
            this.Text = "修改XCP窗口信号";
        }

        public AddNewXcpForm(ProjectItem projectItem, FormItem formitem, bool modifiedName) : base(projectItem, formitem, modifiedName)
        {
            
        }
        #endregion

        XCPSignals allSingals;
        BindingList<XCPSignal> SelectedXcpSignals;
        ComboBox eventCombobox;
        public uint CurrentCanValue { get => uint.TryParse(cbbCanIndex.SelectedItem.ToString(), out uint r) ? r : 0; }
        #region Override
        protected override void AddCustomSignal()
        {
            SignalItemForm<XCPSignal> sif = new SignalItemForm<XCPSignal>(false);
            sif.StartPosition = FormStartPosition.CenterParent;
            if (sif.ShowDialog() == DialogResult.OK)
            {
                if (allSingals.xCPSignalList.Find(x => x.SignalName == sif.TValue.SignalName && x.ECUAddress == sif.TValue.ECUAddress) != null)
                {
                    allSingals.xCPSignalList.Remove(allSingals.xCPSignalList.Find(x => x.SignalName == sif.TValue.SignalName && x.ECUAddress == sif.TValue.ECUAddress));
                }
                allSingals.xCPSignalList.Add(sif.TValue);
                lbSelectedNode.Items.Add(sif.TValue.SignalName);
            }
            
        }

        protected override void ChangeType()
        {
            base.ChangeType();
            switch (cbbFormType.SelectedIndex)
            {
                case (int)FormType.XCP_DAQ:
                case (int)FormType.XCP_DAQScope:
                    LoadAllSignals();
                    eventNameDataGridViewTextBoxColumn.Visible = true;
                    eventIDDataGridViewTextBoxColumn.Visible = true;
                    DAQIDDataGridViewTextBoxColumn.Visible = true;
                    if(cbbCanIndex.SelectedIndex < 0)
                    {
                        LeapMessageBox.Instance.ShowInfo("先选择CAN通道");
                        return;
                    }
                    var data = XCPModuleManager.GetXCPModule(this.projectItem).GetEventsName(CurrentCanValue);
                    if (data != null && data.Count > 0)
                    {
                        eventCombobox.DataSource = data;
                    }
                    else
                    {
                        LeapMessageBox.Instance.ShowError("未获取到 XCP DAQ 事件通道信息", 20);
                    }
                    break;
                case (int)FormType.Set://Set 对信号进行筛选，只能选character
                    {
                        LoadCharacters();

                        eventNameDataGridViewTextBoxColumn.Visible = false;
                        eventIDDataGridViewTextBoxColumn.Visible = false;
                        DAQIDDataGridViewTextBoxColumn.Visible = false;
                    }
                    break;
                default:
                    LoadAllSignals();
                    eventNameDataGridViewTextBoxColumn.Visible = false;
                    eventIDDataGridViewTextBoxColumn.Visible = false;
                    DAQIDDataGridViewTextBoxColumn.Visible = false;
                    break;
            }
        }

        protected override bool CheckNameNull()
        {
            if (!CheckDaqEventNull(out string erSignals))
            {
                LeapMessageBox.Instance.ShowError($"{erSignals} ");
                return false;
            }
            return base.CheckNameNull();
        }

        protected override bool CheckSignalNull()
        {
            if (allSingals == null || allSingals.xCPSignalList.Count == 0)
            {
                LeapMessageBox.Instance.ShowInfo("请先选择通道，或者该协议中没有读取到信号");
                return false;
            }
            return true;
        }

        protected override void Init()
        {
            SelectedXcpSignals = new BindingList<XCPSignal>();

            eventCombobox = new ComboBox();
            eventCombobox.Visible = false;
            eventCombobox.DrawItem += EventCombobox_DrawItem;
            eventCombobox.SelectedIndexChanged += EventCombobox_SelectedIndexChanged;
            
            //eventCombobox.DataSource = new List<string>() { "10ms","100ms"};
            dataGridView1.Controls.Add(eventCombobox);
            
            dataGridView1.CurrentCellChanged += DataGridView1_CurrentCellChanged;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { 
                eventNameDataGridViewTextBoxColumn, 
                eventIDDataGridViewTextBoxColumn,
                DAQIDDataGridViewTextBoxColumn});
            dataGridView1.Columns[eventNameDataGridViewTextBoxColumn.Name].DisplayIndex = 1;
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = SelectedXcpSignals;

            dataGridView1.DataError += DataGridView1_DataError;

            SetColumnVisible(false);
        }

        protected async override void LoadAllSignalsFromFile()
        {
            string fileName = projectItem.CanIndex.Find(x => x.CanChannel == CurrentCanValue).ProtocolFileName;
            lbSelectedNode.Items.Clear();
            tvAllNode.Nodes.Clear();
            try
            {
                ///modified by xwd 2022-02-21 单通道支持多个协议文档
                var fileNames = fileName.Split(';');
                allSingals = new XCPSignals();
                allSingals.xCPSignalList = new List<XCPSignal>();
                foreach (var signal in await BaseProtocol.GetSingalsByProtocolTask(projectItem.CanIndex.Find(x => x.CanChannel == CurrentCanValue).ProtocolType, fileNames))
                {
                    if (signal is XCPSignal)
                        allSingals.xCPSignalList.Add((XCPSignal)signal);
                }

                allSingals.xCPSignalList.Sort();
                foreach (var item in allSingals.xCPSignalList)
                {
                    if(cbbFormType.SelectedIndex == (int)FormType.Set)
                    {
                        if (!item.MeaOrCal)
                            continue;
                    }
                    tvAllNode.Nodes.Add(item.SignalName);
                }
            }
            catch (Exception ex)
            {
                LeapMessageBox.Instance.ShowError(ex.ToString());
            }
        }

        protected override async void LoadSelectedSignals(FormItem formitem)
        {
            await Task.Run(()=> {
                int count = 0;
                do {
                    Task.Delay(1000);
                    count++;
                }
                while (allSingals.xCPSignalList == null || allSingals.xCPSignalList.Count == 0 || count < 60);

            });

            foreach (var item in formitem.XCPSingals.xCPSignalList)
            {
                SelectedXcpSignals.Add(item);
            }

            if (formitem.XCPSingals != null)
            {
                for (int i = 0; i < formitem.XCPSingals.xCPSignalList.Count; i++)
                {
                    var signal = formitem.XCPSingals.xCPSignalList[i];
                    lbSelectedNode.Items.Add(signal.SignalName);
                    if (allSingals.xCPSignalList.Find(x => x.SignalName == signal.SignalName && ((XCPSignal)x).ECUAddress == ((XCPSignal)signal).ECUAddress) == null)
                    {
                        allSingals.xCPSignalList.Add(signal);
                    }
                }
            }
        }

        protected override void ModifiedSignal()
        {
            string name = lbSelectedNode.SelectedItem.ToString();
            XCPSignal signal = allSingals.xCPSignalList.Find(x => x.SignalName == name);
            if (signal == null)
                return;
            SignalItemForm<XCPSignal> sif = new SignalItemForm<XCPSignal>(signal, isReadOnly: false);
            if (sif.ShowDialog() == DialogResult.OK)
            {
                lbSelectedNode.Items.Remove(name);
                if (allSingals.xCPSignalList.Find(x => x.SignalName == sif.TValue.SignalName && x.ECUAddress == sif.TValue.ECUAddress) != null)
                {
                    allSingals.xCPSignalList.Remove(allSingals.xCPSignalList.Find(x => x.SignalName == sif.TValue.SignalName && x.ECUAddress == sif.TValue.ECUAddress));
                }
                allSingals.xCPSignalList.Add(sif.TValue);
                lbSelectedNode.Items.Add(sif.TValue.SignalName);
            }
        }

        protected override void QueryBySignalName(string signalName)
        {
            tvAllNode.Nodes.Clear();
            List<XCPSignal> result;
            if (!string.IsNullOrEmpty(signalName))
            {
                result = (from c in allSingals.xCPSignalList
                          where c.SignalName.ToLower().IndexOf(signalName.ToLower()) >= 0
                          select c).ToList();
            }
            else
            {
                result = allSingals.xCPSignalList;
            }
            foreach (var item in result)
            {
                tvAllNode.Nodes.Add(item.SignalName);
            }
            //tvAllNode.ExpandAll();
        }

        protected override void AddSignal()
        {
            foreach (TreeNode signal in tvAllNode.Nodes)
            {
                if (signal.Checked && !(SelectedXcpSignals.Any(x => x.SignalName == signal.Text)))
                {
                    //lbSelectedNode.Items.Add(signal.Text);
                    SelectedXcpSignals.Add(allSingals.xCPSignalList.Find(x => x.SignalName == signal.Text));
                }
            }
        }

        protected override void RemoveSignal()
        {
            dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0];
            while (dataGridView1.SelectedRows.Count > 0)
            {
                XCPSignal s = dataGridView1.SelectedRows[0].DataBoundItem as XCPSignal;
                SelectedXcpSignals.Remove(s);
            }
        }

        protected override void SaveSignals()
        {
            formItem.XCPSingals = new XCPSignals();
            formItem.XCPSingals.xCPSignalList = SelectedXcpSignals.ToList();
           
        }

        protected override void UpDataGridView(DataGridView dataGridView)
        {
            try
            {
                DataGridViewSelectedRowCollection dgvsrc = dataGridView.SelectedRows;//获取选中行的集合
                if (dgvsrc.Count > 0)
                {
                    int index = dataGridView.SelectedRows[0].Index;//获取当前选中行的索引
                    if (index > 0)
                    {   
                        //如果该行不是第一行
                        var temp = SelectedXcpSignals[index];
                        SelectedXcpSignals[index] = SelectedXcpSignals[index - 1];
                        SelectedXcpSignals[index - 1] = temp;
                        dataGridView1.Rows[index].Selected = false;
                        dataGridView1.Rows[index - 1].Selected = true;
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        protected override void DownDataGridView(DataGridView dataGridView)
        {
            try
            {
                DataGridViewSelectedRowCollection dgvsrc = dataGridView.SelectedRows;//获取选中行的集合
                if (dgvsrc.Count > 0)
                {
                    int index = dataGridView.SelectedRows[0].Index;//获取当前选中行的索引
                    if (index >= 0 & (dataGridView.RowCount - 1) != index)
                    {//如果该行不是最后一行
                        var temp = SelectedXcpSignals[index];
                        SelectedXcpSignals[index] = SelectedXcpSignals[index + 1];
                        SelectedXcpSignals[index + 1] = temp;
                        dataGridView1.Rows[index].Selected = false;
                        dataGridView1.Rows[index + 1].Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        private void DataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
            if (anError.Context == DataGridViewDataErrorContexts.Display)
            {
                MessageBox.Show("Display error");
            }
            //MessageBox.Show("Error happened " + anError.Context.ToString());

            if (anError.Context == DataGridViewDataErrorContexts.Commit)
            {
                MessageBox.Show("Commit error");
            }
            if (anError.Context == DataGridViewDataErrorContexts.CurrentCellChange)
            {
                MessageBox.Show("Cell change");
            }
            if (anError.Context == DataGridViewDataErrorContexts.Parsing)
            {
                MessageBox.Show("parsing error");
            }
            if (anError.Context == DataGridViewDataErrorContexts.LeaveControl)
            {
                MessageBox.Show("leave control error");
            }

            if ((anError.Exception) is ConstraintException)
            {
                DataGridView view = (DataGridView)sender;
                view.Rows[anError.RowIndex].ErrorText = "an error";
                view.Rows[anError.RowIndex].Cells[anError.ColumnIndex].ErrorText = "an error";

                anError.ThrowException = false;
            }
        }

        private void EventCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                if (eventCombobox.SelectedIndex < eventCombobox.Items.Count)
                {
                    DataGridViewColumn column = dataGridView1.CurrentCell.OwningColumn;
                    //如果是要显示下拉列表的列的话
                    if (column.Name.Equals("eventNameDataGridViewTextBoxColumn"))
                    {
                        dataGridView1.CurrentCell.Value = eventCombobox.Items[eventCombobox.SelectedIndex];
                        dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[eventIDDataGridViewTextBoxColumn.Index].Value = eventCombobox.SelectedIndex;
                    }
                }
            }
        }

        private void EventCombobox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.Graphics.DrawString(eventCombobox.Items[e.Index].ToString(), e.Font, Brushes.Black,
                e.Bounds, StringFormat.GenericDefault);
            //————————————————
            //版权声明：本文为CSDN博主「※※冰馨※※」的原创文章，遵循CC 4.0 BY - SA版权协议，转载请附上原文出处链接及本声明。
            //原文链接：https://blog.csdn.net/Pei_hua100/article/details/124492408
        }

        private void DataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell == null)
                return;
            DataGridViewColumn column = dataGridView1.CurrentCell.OwningColumn;
            //如果是要显示下拉列表的列的话
            if (column.Name.Equals("eventNameDataGridViewTextBoxColumn"))
            {
                int columnIndex = dataGridView1.CurrentCell.ColumnIndex;
                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                Rectangle rect = dataGridView1.GetCellDisplayRectangle(columnIndex, rowIndex, false);
                eventCombobox.Left = rect.Left;
                eventCombobox.Top = rect.Top;
                eventCombobox.Width = rect.Width;
                eventCombobox.Height = rect.Height;
                //将单元格的内容显示为下拉列表的当前项\

                if (dataGridView1.Rows[rowIndex].Cells[columnIndex].Value != null)
                {
                    string consultingRoom = dataGridView1.Rows[rowIndex].Cells[columnIndex].Value.ToString();
                    int index = eventCombobox.Items.IndexOf(consultingRoom);
                    eventCombobox.SelectedIndex = index;
                }

                eventCombobox.Visible = true;
            }
            else
            {
                eventCombobox.Visible = false;
            }
        }

        private bool CheckDaqEventNull(out string errStr)
        {
            bool res = true;
            errStr = string.Empty;
            switch (cbbFormType.SelectedIndex)
            {
                case (int)FormType.XCP_DAQ:
                case (int)FormType.XCP_DAQScope:
                    {
                        errStr = "以下事件为空：";
                        foreach (var signal in SelectedXcpSignals)
                        {
                            if (string.IsNullOrEmpty(signal.EventName))
                                errStr += signal.SignalName + "\n\r";
                            res &= !string.IsNullOrEmpty(signal.EventName);
                        }
                        break;
                    }
                case (int)FormType.RollingCounter:
                    errStr = "XCP 不支持RollingCounter窗口";
                    return false;
                default:
                    return true;
            }
            return res;
        }

        private void LoadCharacters()
        {
            if (allSingals != null && allSingals.xCPSignalList != null && allSingals.xCPSignalList.Count > 0)
            {
                tvAllNode.Nodes.Clear();
                allSingals.xCPSignalList.Sort();
                foreach (var item in allSingals.xCPSignalList)
                {
                    if (item.MeaOrCal == false)
                        tvAllNode.Nodes.Add(item.SignalName);
                }
            }
        }

        private void LoadAllSignals()
        {
            if (allSingals != null && allSingals.xCPSignalList != null && allSingals.xCPSignalList.Count > 0)
            {
                tvAllNode.Nodes.Clear();
                allSingals.xCPSignalList.Sort();
                foreach (var item in allSingals.xCPSignalList)
                {
                    tvAllNode.Nodes.Add(item.SignalName);
                }
            }
        }
    }
}
