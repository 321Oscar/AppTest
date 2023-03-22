using AppTest.Helper;
using Nito.AsyncEx;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTest.ProjectClass
{
    public class SignalEntity : IComparable<SignalEntity>
    { 
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }

        public string ProjectName { get; set; }
        public string FormName { get; set; }
        public string SignalName { get; set; }
        public string SignalValue { get; set; }
        public string DataTime { get; set; }
        public string CreatedOn { get; set; }

        public int CompareTo(SignalEntity other)
        {
            //return other.DataTime.CompareTo(this.DataTime);Excel中不支持ms的时间，导入导出数据有问题
            DateTime otherTime = DateTime.ParseExact(other.DataTime, Global.DATETIMEFORMAT, System.Globalization.CultureInfo.InvariantCulture);
            DateTime time = DateTime.ParseExact(this.DataTime, Global.DATETIMEFORMAT, System.Globalization.CultureInfo.InvariantCulture);
            return otherTime.CompareTo(time);
        }
    }

    public class SignalDB : SQLiteConnection
    {
        public TableQuery<SignalEntity> signalEntities { get { return this.Table<SignalEntity>(); } }

        public SignalDB(string dbPath) : base(dbPath,false)
        {
            CreateTable<SignalEntity>();
        }

        /*SQLiteAsyncConnection*/
    }

    public class SignalAsyncDB : SQLiteAsyncConnection
    {
        public AsyncTableQuery<SignalEntity> signalEntities { get { return this.Table<SignalEntity>(); } }

        public SignalAsyncDB(string databasePath, bool storeDateTimeAsTicks = false) : base(databasePath, storeDateTimeAsTicks)
        {
            CreateTableAsync<SignalEntity>();
            //Sqliteco
            //SQLite.SQLiteAsyncConnection.ResetPool
        }
    }

    public class DBHelper
    {
        private static SignalAsyncDB _db;
        private static readonly AsyncLock _mutex = new AsyncLock();

        public static async Task<SignalAsyncDB> GetDb()
        {
            try
            {
                using (await _mutex.LockAsync())
                {
                    if(_db != null)
                    {
                        return _db;
                    }

                    _db = new SignalAsyncDB($"{Environment.CurrentDirectory}{Global.DBPATH}");

                    return _db;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void ResetConnectPool()
        {
            try
            {
                SQLiteAsyncConnection.ResetPool();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static async void DisposeConnect()
        {
            using (await _mutex.LockAsync())
            {
                if (_db == null)
                {
                    return;
                }

                await Task.Factory.StartNew(() =>
                {
                    _db.GetConnection().Close();
                    _db.GetConnection().Dispose();
                    _db = null;

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                });
            }
        }
    }
}
