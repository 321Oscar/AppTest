using AppTest.FormType.Helper;
using AppTest.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppTest.ViewModel
{
    public class HistoryViewModel: INotifyPropertyChanged
    {
        public const string datetimeFormat = "yy/MM/dd HH:mm:ss";

        public string ProjectName { get; set; }
        public string FormName { get; set; }

        private int TotalCount = 0;
        private int currentPage = 1;
        public int CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                if (value > totalPage)
                {
                    currentPage = totalPage;
                }
                else if (value <= 0)
                {
                    currentPage = 1;
                }
                NotifyPropertyChanged();
            }
        }
        private int totalPage = 0;
        public int TotalPage
        {
            get => totalPage;
            set
            {
                totalPage = value;
                TotalPageStr = totalPage.ToString();
            }
        }
        private string totalPageStr = "/共 ？页";
        public string TotalPageStr
        {
            get => totalPageStr;
            set {
                totalPageStr = $"/共{totalPage}页";
                NotifyPropertyChanged();
            }
        }

        public int PageInfoCount
        {
            get => pageInfoCount; 
            set
            {
                pageInfoCount = value; NotifyPropertyChanged();
            }
        }

        public string QueryName { get => queryName; set => queryName = value; }
        public DateTime Start { get => start; set => start = value; }
        public DateTime End { get => end; set => end = value; }

        private int pageInfoCount = 1000;

        private string queryName;

        private DateTime start;
        private  DateTime end;

        private string queryLog;

        public DataGridView DataGridView { get; set; }
        public string QueryLog { get => queryLog; set { queryLog = value; NotifyPropertyChanged(); } }

        public Task Query(bool page = true, int pageIdx = 1, bool andorlike = false)
        {
            return Task.Run(new Action(async () => {
                string sqlCount = "select Count(TimeStamp) from";
                string sqlStr = $"SignalEntity where ProjectName ='{ProjectName}' and" +
                    $" FormName = '{FormName}' and CreatedON > '{Start:yyyy-MM-dd HH:mm:ss}' and CreatedON < '{End:yyyy-MM-dd HH:mm:ss}'";
                if (!string.IsNullOrEmpty(QueryName))
                {
                    if (!andorlike)
                    {
                        sqlStr += $" and SignalName like '%{QueryName}%'";
                    }
                    else
                    {
                        sqlStr += $" and SignalName = '{QueryName}'";
                    }
                }
                string sqlorder = $" Order by CANTimeStamp asc";
                QueryLog = "查询中...";
                var db = await DBHelper.GetDb();
                try
                {
                    TotalCount = await db.ExecuteScalarAsync<int>($"{sqlCount} {sqlStr} ");

                    if (TotalCount == 0)
                    {
                        QueryLog = ("无数据！");
                        return;
                    }
                    else
                    {
                        QueryLog = $"查询到{TotalCount}条数据";
                    }

                    if (TotalCount > pageInfoCount)
                    {
                        TotalPage = TotalCount / pageInfoCount + 1;
                    }
                    else
                    {
                        TotalPage = 1;
                    }

                    string sqlAll = "select * from " + sqlStr + sqlorder;

                    if (page)
                    {
                        int offset = pageInfoCount * (pageIdx - 1);
                        sqlAll += $" Limit {pageInfoCount} offset {offset}";
                    }

                    var e = await db.QueryAsync<SignalEntity>(sqlAll);
                    if (DataGridView.InvokeRequired) {
                        DataGridView.Invoke(new Action(() =>
                        {
                            DataGridView.DataSource = null;
                            DataGridView.DataSource = e;
                        }));
                    }
                    
                    if (e.Count == 0)
                        QueryLog = ("无数据！");
                }
                catch (Exception err)
                {
                    LeapMessageBox.Instance.ShowError(err.Message);
                }
            }));
           
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
