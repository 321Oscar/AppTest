using AppTest.Helper;
using Nito.AsyncEx;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTest.Model
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
        public int TimeStamp { get; set; }

        public uint CANTimeStamp { get; set; }

        public int CompareTo(SignalEntity other)
        {
            //return other.DataTime.CompareTo(this.DataTime);Excel中不支持ms的时间，导入导出数据有问题
            DateTime otherTime = DateTime.ParseExact(other.DataTime, Global.DATETIMEFORMAT, System.Globalization.CultureInfo.InvariantCulture);
            DateTime time = DateTime.ParseExact(this.DataTime, Global.DATETIMEFORMAT, System.Globalization.CultureInfo.InvariantCulture);
            int rtn = otherTime.CompareTo(time);
            if (rtn == 0)
                return other.TimeStamp.CompareTo(this.TimeStamp);
            return otherTime.CompareTo(time);
        }
    }

    /// <summary>
    /// 启动时判断是否有该数据库，没有的话则不可用，
    /// 有的话？比对板子(需要连接can盒并且读取DID)的Code是否匹配，Count是否>10
    /// 满足Code.count <10 则软件可用
    /// </summary>
    public class AuthenticationEntity
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// ProductName
        /// </summary>
        public string Code
        {
            get; set;
        }
        /// <summary>
        /// F199编译日期
        /// </summary>
        public string Code1 { get; set; }
        /// <summary>
        /// F189软件版本号
        /// </summary>
        public string Code2 { get; set; }

        public int Count { get; set; }
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

    public class AuthenticationAsyncDB : SQLiteAsyncConnection
    {
        public AsyncTableQuery<AuthenticationEntity> AuthenticationEntities { get { return this.Table<AuthenticationEntity>(); } }

        public AuthenticationAsyncDB(string databasePath, bool storeDateTimeAsTicks = false) : base(databasePath, storeDateTimeAsTicks)
        {
            CreateTableAsync<AuthenticationEntity>();
            //Sqliteco
            //SQLite.SQLiteAsyncConnection.ResetPool
        }
    }

    public class DBHelper
    {
        private static SignalAsyncDB _db;
        private static AuthenticationAsyncDB _db_au;
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
            catch (Exception er)
            {
                throw er;
            }
        }

        public static async Task<AuthenticationAsyncDB> GetAuthenticationDb()
        {
            try
            {
                using (await _mutex.LockAsync())
                {
                    if (_db_au != null)
                    {
                        return _db_au;
                    }

                    _db_au = new AuthenticationAsyncDB($"{Environment.CurrentDirectory}{Global.CONFIGPATH}");

                    return _db_au;
                }
            }
            catch (Exception er)
            {
                throw er;
            }
        }

        public static void ResetConnectPool()
        {
            try
            {
                SQLiteAsyncConnection.ResetPool();
            }
            catch (Exception er)
            {

                throw er;
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
