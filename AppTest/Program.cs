using AppTest.FormType.Helper;
using AppTest.Mainform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AppTest.Helper;
using AppTest.Model;

namespace AppTest
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            BindExceptionHandler();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            InitDB();
            Application.Run(new MainFormV2());
            //StartUp startUp = new StartUp();
            //DialogResult dialogResult = startUp.ShowDialog();
            //if (dialogResult.Equals(DialogResult.OK))
            //{
            //    Global.ShowType = 0;
            //    Application.Run(new MainForm());
            //}
            //else if (dialogResult.Equals(DialogResult.Yes))
            //{
            //    Global.ShowType = 1;
            //    Application.Run(new MainFormV2());
            //}


            //Application.Run(new MainForm());
            //Application.Run(new Form1());
        }

        private static void BindExceptionHandler()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LeapMessageBox.Instance.ShowError(e.ExceptionObject.ToString());
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            LeapMessageBox.Instance.ShowError(e.Exception);
        }

        private static void InitDB()
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Data"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Data");
            }
            var dbAsync = DBHelper.GetDb();
            var configAsync = DBHelper.GetAuthenticationDb();
            //Hide config.db
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\Config\\Config.db";
            FileAttributes attributes = File.GetAttributes(filePath);
            if ((attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
            {
                // 文件未隐藏，需要隐藏
                attributes |= FileAttributes.Hidden;
                File.SetAttributes(filePath, attributes);
            }
        }
    }
}
