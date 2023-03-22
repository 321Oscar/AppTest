using AppTest.FormType.Helper;
using AppTest.Helper;
using AppTest.Model;
using LPCanControl.CANInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppTest.FormType
{
    public partial class DataForm : BaseDataForm
    {
        protected readonly Color GetColor = Color.FromArgb(0, 174, 219);
        protected readonly Color SetColor = Color.FromArgb(243, 119, 53);
        protected readonly Color RtlColor = Color.FromArgb(0, 177, 89);

        public DataForm() : base()
        {
            InitializeComponent();
        }

        public DataForm(FormType formType, ProtocolType protocolType = ProtocolType.DBC) : this()
        {
            this.FormType = formType;

            this.metroPanelMain.Controls.Add(this.tableLayoutPanel1);
           
            strValueDataGridViewTextBoxColumn.ReadOnly = formType != FormType.Set;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.RowHeadersVisible = false;
            messageIDDataGridViewTextBoxColumn.Visible = protocolType == ProtocolType.DBC;
            base.MDIModeVisible = protocolType == ProtocolType.DBC;

            switch (formType)
            {
                case FormType.Get:
                    gbRlcntControls.Visible = gbSetControls.Visible = false;
                    SetColr(GetColor);
                    metroTabControl1.Style  = MetroFramework.MetroColorStyle.Blue;
                    this.Style = MetroFramework.MetroColorStyle.Blue;
                    LoadDatGridViewContext();
                    break;
                case FormType.Set:
                    this.Style = MetroFramework.MetroColorStyle.Orange;
                    metroTabControl1.Style = MetroFramework.MetroColorStyle.Orange;
                    metroComboBox_Signal.Style = MetroFramework.MetroColorStyle.Orange;
                    SetColr(SetColor);
                    LoadDatGridViewContext();
                    gbRlcntControls.Visible = gbGetControls.Visible = false;
                    this.SaveDataVisible = false;
                    cbbSignals.AutoCompleteSource = AutoCompleteSource.ListItems;
                    cbbSignals.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

                    tscbb = new ToolStripComboBox();
                    tscbb.ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                    tscbb.ComboBox.Items.AddRange(new string[] { "正常发送", "单次发送", "自发自收", "单次自发自收" });
                    statusStrip1.Items.Add(tscbb);
                    tscbb.SelectedIndex = 0;
                    ToolStripSeparator tss = new ToolStripSeparator();
                    statusStrip1.Items.Add(tss);
                    metroTabControl1.TabPages.RemoveAt(1);
                    //tabControl1.TabPages.RemoveAt(1);
                    //tabPage2.vi
                    break;
                //case FormType.RollingCounter:
                //    this.Style = MetroFramework.MetroColorStyle.Green;
                //    metroTabControl1.Style = MetroFramework.MetroColorStyle.Green;
                //    metroComboBox_Signal.Style = MetroFramework.MetroColorStyle.Green;
                //    SetColr(RtlColor);
                //    btnReduce.BackColor = RtlColor;
                //    btnAdd.BackColor = RtlColor;
                //    btnSet.BackColor = RtlColor;
                //    gbGetControls.Visible = false;
                //    this.SaveDataVisible = false;
                //    metroTabControl1.TabPages.RemoveAt(1);
                //    timerList = new List<Thread>();
                //    btnSet.Text = "Set";
                //    break;
            }

            InitStateStrip();
            
            nudStep.DecimalPlaces = 2;

        }

        private void SetColr(Color c)
        {
            this.dataGridView1.DefaultCellStyle.SelectionBackColor = c;
            //this.cbbSignals.BackColor = c;
            //this.cbbSignals.ForeColor = Color.White;
            this.statusStrip1.BackColor = c;
            this.statusStrip1.ForeColor = Color.White;
        }

        /// <summary>
        /// 加载右键菜单
        /// </summary>
        protected void LoadDatGridViewContext()
        {
            foreach (DataGridViewColumn item in dataGridView1.Columns)
            {
                ToolStripMenuItem tsb = new ToolStripMenuItem();
                tsb.Checked = item.Visible;
                tsb.Click += Tsb_Click;
                tsb.Text = item.HeaderText;
                tsb.Name = item.Index.ToString();
                contextMenuStrip1.Items.Add(tsb);
            }
            
        }

        private void Tsb_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsb = sender as ToolStripMenuItem;
            int idx = int.Parse(tsb.Name);
            dataGridView1.Columns[idx].Visible = !dataGridView1.Columns[idx].Visible;
            tsb.Checked = dataGridView1.Columns[idx].Visible;
        }

        protected override void InitStateStrip()
        {
            base.InitStateStrip();

            statusStrip1.Location = new Point(0, this.Size.Height);
        }

        protected override void ReLoadSignal()
        {
            base.ReLoadSignal();
        }

        protected override void InitSignalUC()
        {
            
        }

        private bool TestCanSend()
        {
            string errorlog =  string.Empty;

            bool send = USBCanManager.Instance.SendTest(OwnerProject,ref errorlog, canindex: this.CanChannel);
            if (!send)
            {
                MessageBox.Show(errorlog);
                ShowLog(errorlog);
            }
              
            return send;
        }

        protected virtual void ShowSignalInfo(DataGridViewCellEventArgs e)
        {
            
        }

        #region DataGridView

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowSignalInfo(e);
        }

        /// <summary>
        /// 内嵌控件值改变后立即触发事件，而不需要离开该单元格时才触发，此时需要用到CurrentCellDirtyStateChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (grid != null)
            {
                if (grid.CurrentCell.OwningColumn.DataPropertyName == "WhetherSendOrGet")
                    grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].DataPropertyName == "StrValue")
            {
                try
                {
                    var v = Convert.ToDouble(e.Value);
                    var signal = dataGridView1.Rows[e.RowIndex].DataBoundItem as BaseSignal;
                    if (v > signal.Maximum || v < signal.Minimum)
                    {
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    }
                    else
                    {
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
                catch
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                }

            }
        }
        #endregion

        #region Get - Method

        protected override void ModifiedGetdata(bool get)
        {
            btnGet.Text = !get ? "Start" : "Stop";
            nudTimerInterval.Enabled = !get;
            ShowLog("");
            //WhetherSendOrGet.ReadOnly = get;
            RegisterOrUnRegisterDataRecieve(get);
        }

        private void btnDataControl_Click(object sender, EventArgs e)
        {
            DataControl();
        }

        protected virtual void DataControl()
        {
        }

        #endregion

        #region Set - Method

        private void btnSetOrSend_Click(object sender, EventArgs e)
        {
            SetOrSend();
        }

        protected virtual void SetOrSend() 
        {
            
        }

        private void cbbSignals_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedSignalChanged();
        }

        protected virtual void SelectedSignalChanged()
        {
            
        }

        private void btnChangeValue_Click(object sender, EventArgs e)
        {
            ChangeValue(sender);
        }

        protected virtual void ChangeValue(object sender)
        {
            
        }

        #endregion

        #region 双缓冲-尝试改善界面闪烁
        private void DataForm_Load(object sender, EventArgs e)
        {
            //Type dgvType = this.dataGridView1.GetType();
            //PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);

            //pi.SetValue(this.dataGridView1, true, null);

        }

        #endregion

        /// <summary>
        /// 下拉框宽度自适应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroComboBox_Signal_DropDown(object sender, EventArgs e)
        {
            int width = metroComboBox_Signal.DropDownWidth;
            Graphics g = metroComboBox_Signal.CreateGraphics();
            Font f = metroComboBox_Signal.Font;
            int vertScrollBarWidth = (metroComboBox_Signal.Items.Count > metroComboBox_Signal.MaxDropDownItems) ? SystemInformation.VerticalScrollBarWidth : 0;
            foreach (var item in metroComboBox_Signal.Items)
            {
                var itemWidth = (int)g.MeasureString(item.ToString().Trim(), f).Width + 10 + vertScrollBarWidth;
                if(itemWidth > width)
                {
                    width = itemWidth;
                }
            }
            metroComboBox_Signal.DropDownWidth = width;
        }
    }
}
