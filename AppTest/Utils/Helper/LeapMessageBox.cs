using AppTest.Helper;
using AppTest.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppTest.FormType.Helper
{
    /// <summary>
    /// 提示框显示
    /// </summary>
    public class LeapMessageBox
    {
        private static readonly Lazy<LeapMessageBox> lazy =
            new Lazy<LeapMessageBox>(() => new LeapMessageBox());
        //static List<MessageBoxUC> boxes = new List<MessageBoxUC>();
        public static LeapMessageBox Instance { get { return lazy.Value; } }
        MessageBoxUC messageBoxUC;
        /// <summary>
        /// 提示框-右下角显示3秒
        /// </summary>
        /// <param name="content"></param>
        public void ShowInfo(string content)
        {
            LogHelper.Warn(content);

            UINotifier.Show(content, UINotifierType.INFO, UILocalize.InfoTitle, false, 2000);
            //try
            //{
            //    messageBoxUC = new MessageBoxUC("Info", content, 3);
            //    //boxes.Add(messageBoxUC);
            //    Point p = new Point(Screen.PrimaryScreen.WorkingArea.Width - messageBoxUC.Width,
            //    Screen.PrimaryScreen.WorkingArea.Height - messageBoxUC.Height);
            //    messageBoxUC.PointToClient(p);
            //    messageBoxUC.Location = p;
            //    messageBoxUC.TopMost = true;
            //    messageBoxUC.Show();
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Error("error",ex);
            //}

            //// }
        }

        public void ShowWarn(string content)
        {
            LogHelper.Warn(content);

            UINotifier.Show(content, UINotifierType.WARNING, UILocalize.WarningTitle, false, 2000);
        }

        /// <summary>
        /// 弹出提示框
        /// </summary>
        /// <param name="text"></param>
        public void ShowInformation(string text)
        {
            ShowInfo(text);
        }

        /// <summary>
        /// 错误提示框-右下角
        /// </summary>
        /// <param name="content"></param>
        /// <param name="time">错误框显示时长</param>
        public void ShowError(string content, int time = 10)
        {
            //MessageBoxUC messageBoxUC = new MessageBoxUC("Error", content, time);
            LogHelper.Error(content, null);
            //Point p = new Point(Screen.PrimaryScreen.WorkingArea.Width - messageBoxUC.Width,
            //    Screen.PrimaryScreen.WorkingArea.Height - messageBoxUC.Height);
            //messageBoxUC.PointToClient(p);
            //messageBoxUC.Location = p;
            //messageBoxUC.TopMost = true;
            //messageBoxUC.Show();
            UINotifier.Show(content, UINotifierType.ERROR, UILocalize.ErrorTitle, false, time * 100);
        }

        /// <summary>
        /// 错误消息框-居中置顶
        /// </summary>
        /// <param name="ex"></param>
        public void ShowError(Exception ex)
        {
            ExceptionMessageBox exceptionMessageBox = new ExceptionMessageBox(ex);
            LogHelper.Error(ex.ToString(), null);
            exceptionMessageBox.StartPosition = FormStartPosition.CenterParent;
            exceptionMessageBox.ShowDialog();
        }
    }
}
