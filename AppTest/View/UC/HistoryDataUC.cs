using AppTest.FormType.Helper;
using AppTest.Helper;
using AppTest.Model;
using AppTest.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppTest.FormType
{
    public partial class HistoryDataUC : UserControl
    {
        HistoryViewModel _historyViewModel; 
        public HistoryDataUC()
        {
            InitializeComponent();

            _historyViewModel = new HistoryViewModel();

            dateTimePickerStart.Format = DateTimePickerFormat.Custom;
            dateTimePickerStart.CustomFormat = HistoryViewModel.datetimeFormat;
            dateTimePickerEnd.Format = DateTimePickerFormat.Custom;
            dateTimePickerEnd.CustomFormat = HistoryViewModel.datetimeFormat;

            dataGridView1.MultiSelect = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.RowHeadersVisible = true;

            var threadSafeModel = new SynchronizedNotifyPropertyChanged<HistoryViewModel>(_historyViewModel, this);
            tbCurrentPage.DataBindings.Add("Text", threadSafeModel, "CurrentPage",false, updateMode: DataSourceUpdateMode.OnPropertyChanged);
            tbCurPage.DataBindings.Add("Text", threadSafeModel, "CurrentPage",false, updateMode: DataSourceUpdateMode.OnPropertyChanged);
            lbToPage.DataBindings.Add("Text", threadSafeModel, "TotalPageStr", false, updateMode: DataSourceUpdateMode.OnPropertyChanged);
            lbTotalPage.DataBindings.Add("Text", threadSafeModel, "TotalPageStr", false, DataSourceUpdateMode.OnPropertyChanged);
            textBox1.DataBindings.Add("Text", threadSafeModel, "QueryName", false, DataSourceUpdateMode.OnPropertyChanged);
            lbQueryLog.DataBindings.Add("Text", threadSafeModel, "QueryLog", false, DataSourceUpdateMode.OnPropertyChanged);

            _historyViewModel.DataGridView = dataGridView1;
        }

        public void ChangeColorTheme(Color c)
        {
            btnQuery.BackColor = c;
            btnQuery.ForeColor = Color.White;

            btnNextPage.BackColor = c;
            btnNextPage.ForeColor = Color.White;
            btnPrevPage.BackColor = c;
            btnPrevPage.ForeColor = Color.White; 
            btnPageStart.BackColor = c;
            btnPageStart.ForeColor = Color.White;
            btnPageEnd.BackColor = c;
            btnPageEnd.ForeColor = Color.White;

            button1.BackColor = c;
            button1.ForeColor = Color.White;
            button2.BackColor = c;
            button2.ForeColor = Color.White;
            button3.BackColor = c;
            button3.ForeColor = Color.White;
            button4.BackColor = c;
            button4.ForeColor = Color.White;

            dataGridView1.DefaultCellStyle.SelectionBackColor = c;
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnQuery.Text = "查询";
            if (e.Cancelled)
            {
                MessageBox.Show("取消");
                return;
            }

            if(e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                return;
            }
            if(progressBar1.Value == 100)
            MessageBox.Show("完成");
            progressBar1.Value = 0;
        }

        public string ProjectName { get => _historyViewModel.ProjectName; set => _historyViewModel.ProjectName = value; }
        public string FormName { get => _historyViewModel.FormName; set => _historyViewModel.FormName = value; }
        

        private void btnQuery_Click(object sender, EventArgs e)
        {
            Query();
        }

        private void CurveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                //var entities = dataGridView1.SelectedRows as List<SingalEntity>;
                List<SignalEntity> entities = new List<SignalEntity>();
                foreach (DataGridViewRow item in dataGridView1.SelectedRows)
                {
                    SignalEntity entity = item.DataBoundItem as SignalEntity;
                    entities.Add(entity);
                }
                HistoryCurveForm hcF = new HistoryCurveForm(entities);
                hcF.Show();
            }
            else
            {
                LeapMessageBox.Instance.ShowInfo("选中数据");
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(
                x: e.RowBounds.Location.X,
                y: e.RowBounds.Location.Y,
                width: dataGridView1.RowHeadersWidth - 4,
                height: e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1 +((_historyViewModel.CurrentPage-1) * _historyViewModel.PageInfoCount)).ToString(),
                dataGridView1.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private  void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundWorker1.ReportProgress(0,"查询中");
            using (var db = new SignalDB($"{Environment.CurrentDirectory}{Global.DBPATH}"))
            {
                //var db = new SignalAsyncDB($"{Environment.CurrentDirectory}{Global.DBPATH}");
                var dateStart = dateTimePickerStart.Value;
                var dateEnd = dateTimePickerEnd.Value;
                var allEntities =  db.signalEntities.ToList();
                var entities =  db.signalEntities.Where(x => x.ProjectName == ProjectName && x.FormName == FormName).OrderByDescending(x => x.CreatedOn).ToList();
                backgroundWorker1.ReportProgress(0, "查询中");

                List<SignalEntity> showEntities = new List<SignalEntity>();
                for (int i = 0; i < entities.Count; i++)
                {
                    var entity = entities[i];
                    
                    if (DateTime.ParseExact(Global.DATETIMEFORMAT,entity.CreatedOn,null) >= dateStart)
                    {
                        if (DateTime.ParseExact(Global.DATETIMEFORMAT, entity.CreatedOn, null) <= dateEnd)
                        {
                            if (entity.SignalName.ToLower().Contains(textBox1.Text.ToLower()))
                            {
                                showEntities.Add(entity);
                                int progress = i * 100 / entities.Count;
                                if(progress > 0)
                                {
                                    backgroundWorker1.ReportProgress(progress);
                                }
                            }
                        }
                    }
                }
                //entities = (from i in entities
                //            where DateTime.ParseExact(i.CreatedOn, Global.DATETIMEFORMAT, System.Globalization.CultureInfo.InvariantCulture) >= dateStart
                //           & DateTime.ParseExact(i.CreatedOn, Global.DATETIMEFORMAT, System.Globalization.CultureInfo.InvariantCulture) <= dateEnd
                //           & i.signalName.Contains(textBox1.Text)
                //            select i).ToList();
                this.BeginInvoke(new Action(() => {
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = showEntities; }));
                backgroundWorker1.ReportProgress(100);
                if (showEntities.Count == 0)
                {
                    backgroundWorker1.ReportProgress(0);
                    MessageBox.Show("无匹配数据");
                    return;
                }
                //backgroundWorker1.ReportProgress(90);

                ////this.dataGridView1.DataSource = entities;
                //backgroundWorker1.ReportProgress(100);
            }
        }

        private async void Query()
        {
            btnQuery.Enabled = false;

            try
            {
                _historyViewModel.Start = dateTimePickerStart.Value;
                _historyViewModel.End = dateTimePickerEnd.Value;
                //_historyViewModel.que = dateTimePickerEnd.Value;
                await _historyViewModel.Query();
            }
            catch (Exception err)
            {
                LeapMessageBox.Instance.ShowError(err.Message);
            }
            finally
            {
                btnQuery.Enabled = true;
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            if(e.UserState != null)
            {
                var x = e.UserState.ToString();
                btnQuery.Text = x;
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<SignalEntity> entities = new List<SignalEntity>();
            foreach (DataGridViewRow item in dataGridView1.SelectedRows)
            {
                SignalEntity entity = item.DataBoundItem as SignalEntity;
                entities.Add(entity);
            }
            //选择路径
            string SavePath;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "excel文件(*.xls)|*.xls";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                SavePath = (saveFileDialog.FileName);
                //string prpjectJson = JsonConvert.SerializeObject(Root);

                //File.WriteAllText(SavePath, prpjectJson);
                NPIOHelper.ListExportExcel(entities, SavePath);

                LeapMessageBox.Instance.ShowInfo("保存成功。地址：" + SavePath);
            }
        }

        private void copyNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DataGridViewClipboardCopyMode.
            if (dataGridView1.SelectedRows.Count == 0)
                return;
            string signalName = (dataGridView1.SelectedRows[0].DataBoundItem as SignalEntity).SignalName;
            Clipboard.SetDataObject(signalName);
            LeapMessageBox.Instance.ShowInfo($"信号名称：【{signalName}】已复制到剪切板");
        }

        private async void btnPageStart_Click(object sender, EventArgs e)
        {
            //第一页
            _historyViewModel.CurrentPage = 1;
            _historyViewModel.Start = dateTimePickerStart.Value;
            _historyViewModel.End = dateTimePickerEnd.Value;

            await _historyViewModel.Query(true, _historyViewModel.CurrentPage);

            dataGridView1.Refresh();
        }

        private void BtnPrevPage_Click(object sender, EventArgs e)
        {
            if(_historyViewModel.CurrentPage > 1)
            {
                _historyViewModel.CurrentPage--;
                _historyViewModel.Query(pageIdx:_historyViewModel.CurrentPage);
            }
            
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            if (_historyViewModel.CurrentPage < _historyViewModel.TotalPage)
            {
                _historyViewModel.CurrentPage++;
                _historyViewModel.Query(pageIdx: _historyViewModel.CurrentPage);
            }
        }

        private void tbCurrentPage_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _historyViewModel.Query(pageIdx: _historyViewModel.CurrentPage);
            }
        }

        private void btnPageEnd_Click(object sender, EventArgs e)
        {
            _historyViewModel.CurrentPage = _historyViewModel.TotalPage;
            _historyViewModel.Query(pageIdx: _historyViewModel.CurrentPage);
        }
    }
}
