using AppTest.FormType.Helper;
using AppTest.Helper;
using AppTest.Model;
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
    public partial class HistoryDataUC : UserControl
    {
        public HistoryDataUC()
        {
            InitializeComponent();
            dateTimePickerStart.Format = DateTimePickerFormat.Custom;
            dateTimePickerStart.CustomFormat = datetimeFormat;
            dateTimePickerEnd.Format = DateTimePickerFormat.Custom;
            dateTimePickerEnd.CustomFormat = datetimeFormat;

            dataGridView1.MultiSelect = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.RowHeadersVisible = true;

            progressBar1.Maximum = 100;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
        }

        public void ChangeColorTheme(Color c)
        {
            this.btnQuery.BackColor = c;
            btnQuery.ForeColor = Color.White;

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

        const string datetimeFormat = "yy/MM/dd HH:mm:ss"; 

        public string ProjectName;
        public string FormName;

        private void btnQuery_Click(object sender, EventArgs e)
        {
            Query();
            //if (!backgroundWorker1.IsBusy)
            //{
            //    backgroundWorker1.RunWorkerAsync();
            //}
            //Query();
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

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
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
            var db = await DBHelper.GetDb();
            var dateStart = dateTimePickerStart.Value;
            var dateEnd = dateTimePickerEnd.Value;
            // var allEntities = await db.signalEntities.ToListAsync();
            string sqlStr = $"select * from SignalEntity where ProjectName ='{ProjectName}' and" +
                 $" FormName = '{FormName}' and CreatedON > '{dateStart:yyyy-MM-dd HH:mm:ss}'";
            if (!string.IsNullOrEmpty(textBox1.Text.Trim()))
            {
                sqlStr += $" and SignalName like '%{textBox1.Text}%'";
            }
            var entities = await db.QueryAsync<SignalEntity>(sqlStr);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = entities;
            if (entities.Count == 0)
                LeapMessageBox.Instance.ShowInformation("无数据！");
            btnQuery.Enabled = true;
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
    }
}
