using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppTest.FormType
{
    public partial class TestBorderForm : BaseForm
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get => this.lbTitle.Text; set => lbTitle.Text = value; }
        public Panel PanelLog { get => this.panelLog; }
        
        public TestBorderForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.BackColor = Color.White;

            this.DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);

            int[] inx = new int[] { 0,1,2,3,4,5,6,7,8,9};
            var x = DelDoubleInt(inx);
            string str = string.Empty;
            for (int i = 0; i < x.Length; i++)
            {
                str += x[i] + ";";
            }
            MessageBox.Show(str);
        }

        private int[] DelDoubleInt(int[] arrayInt)
        {
            List<int> res = new List<int>();
            //Dictionary<int, int> keyValuesIndx = new Dictionary<int, int>();
            for (int i = 0; i < arrayInt.Length; i++)
            {
                if ((i + 1) % 3 == 0)
                {
                    //记录删除位置
                    continue;
                }
                res.Add(arrayInt[i]);
            }

            if(res.Count > 2)
            {
               return DelDoubleInt(res.ToArray());
            }

            return res.ToArray();
        }
       

        #region -- Move --
        private Point mouseOff;//鼠标移动位置变量
        private bool leftFlag;//鼠标是否为左键
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y);//获得当前鼠标的坐标
                leftFlag = true;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;//获得移动后鼠标的坐标
                mouseSet.Offset(mouseOff.X, mouseOff.Y);//设置移动后的位置
                Location = mouseSet;
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;
            }
        }

        private void TestBorderForm_MouseClick(object sender, MouseEventArgs e)
        {
            Win32.ReleaseCapture();
            Win32.SendMessage(this.Handle, Win32.WM_SYSCOMMAND, Win32.SC_MOVE + Win32.HTCAPTION, 0);
        }
        #endregion -- Move --

        #region -- Override --

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (DesignMode)
                return;
            switch (m.Msg)
            {
                case 0x0084:
                    base.WndProc(ref m);
                    Point vPoint = new Point((int)m.LParam & 0xFFFF, (int)m.LParam >> 16 & 0xFFFF);
                    vPoint = PointToClient(vPoint);
                    if (vPoint.X <= 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)Win32.HTTOPLEFT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)Win32.HTBOTTOMLEFT;
                        else m.Result = (IntPtr)Win32.HTLEFT;
                    else if (vPoint.X >= ClientSize.Width - 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)Win32.HTTOPRIGHT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)Win32.HTBOTTOMRIGHT;
                        else m.Result = (IntPtr)Win32.HTRIGHT;
                    else if (vPoint.Y <= 5)
                        m.Result = (IntPtr)Win32.HTTOP;
                    else if (vPoint.Y >= ClientSize.Height - 5)
                        m.Result = (IntPtr)Win32.HTBOTTOM;
                    break;
                case 0x0201: //鼠标左键按下的消息
                    m.Msg = 0x00A1; //更改消息为非客户区按下鼠标
                    m.LParam = IntPtr.Zero; //默认值
                    m.WParam = new IntPtr(2);//鼠标放在标题栏内
                    base.WndProc(ref m);
                    break;
                case Win32.WM_ERASEBKGND:
                    m.Result = IntPtr.Zero;
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            //RenderHelper.SetFormRoundRectRgn(this, 10);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                //cp.ExStyle |= 0x02000000;
                cp.ClassStyle |= 0x00020000; //0x00020000//添加阴影
                //if (this.isSized)
                //{
                //    cp.ExStyle |= 0x02000000;//导致拖动有残影
                //}
                //else
                //{
                //    cp.ExStyle &= ~0x02000000;
                //}
                return cp;
            }
        }
        #endregion -- Override --

        #region -- Min Max Close --
        private void buttonMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void buttonMax_Click(object sender, EventArgs e)
        {
            if(this.WindowState != FormWindowState.Maximized)
                this.WindowState = FormWindowState.Maximized;
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void panelTitle_DoubleClick(object sender, EventArgs e)
        {
            buttonMax_Click(null, null);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion -- Min Max Close --

        #region -- Private --

        private void WmNcHitTest(ref Message m)
        {
            int wparam = m.LParam.ToInt32();
            Point mouseLocation = new Point(RenderHelper.LOWORD(wparam), RenderHelper.HIWORD(wparam));
            mouseLocation = PointToClient(mouseLocation);

            if (WindowState != FormWindowState.Maximized)
            {
                if (mouseLocation.X < 5 && mouseLocation.Y < 5)
                {
                    m.Result = new IntPtr(Win32.HTTOPLEFT);
                    return;
                }

                if (mouseLocation.X > Width - 5 && mouseLocation.Y < 5)
                {
                    m.Result = new IntPtr(Win32.HTTOPRIGHT);
                    return;
                }

                if (mouseLocation.X < 5 && mouseLocation.Y > Height - 5)
                {
                    m.Result = new IntPtr(Win32.HTBOTTOMLEFT);
                    return;
                }

                if (mouseLocation.X > Width - 5 && mouseLocation.Y > Height - 5)
                {
                    m.Result = new IntPtr(Win32.HTBOTTOMRIGHT);
                    return;
                }

                if (mouseLocation.Y < 3)
                {
                    m.Result = new IntPtr(Win32.HTTOP);
                    return;
                }

                if (mouseLocation.Y > Height - 3)
                {
                    m.Result = new IntPtr(Win32.HTBOTTOM);
                    return;
                }

                if (mouseLocation.X < 3)
                {
                    m.Result = new IntPtr(Win32.HTLEFT);
                    return;
                }

                if (mouseLocation.X > Width - 3)
                {
                    m.Result = new IntPtr(Win32.HTRIGHT);
                    return;
                }
            }
            m.Result = new IntPtr(Win32.HTCAPTION);
        }

        private void ControlBorder_Paint(object sender, Graphics g, Color color)
        {
            Pen pen = new Pen(Color.FromArgb(255, color), 1f);
            g.DrawRectangle(pen, new Rectangle(new Point(this.Location.X - 1, this.Location.Y - 1), new Size(this.Size.Width + 1, this.Size.Height + 1)));
            foreach (System.Windows.Forms.Control ctr in Controls)
            {
                //if (ctr is Control.Controls.TextBoxs.TextBoxEx || ctr is ComboBox)
                //{

                //}
            }
            pen.Dispose();

            //Render.MoveWindow
        }
       
        private void TestBorderForm_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            ControlBorder_Paint(sender, e.Graphics, ColorTranslator.FromHtml("#cccccc"));
        }

        #endregion  -- Private --

    }
    /// <summary>
    /// 圆角
    /// </summary>
    public class RenderHelper
    {
        /// <summary>
        /// 设置窗体的圆角矩形
        /// </summary>
        /// <param name="form">需要设置的窗体</param>
        /// <param name="rgnRadius">圆角矩形的半径</param>
        public static void SetFormRoundRectRgn(Form form, int rgnRadius)
        {
            int hRgn = 0;
            hRgn = Win32.CreateRoundRectRgn(0, 0, form.Width + 1, form.Height + 1, rgnRadius, rgnRadius);
            Win32.SetWindowRgn(form.Handle, hRgn, true);
            Win32.DeleteObject(hRgn);
        }

        internal static int HIWORD(int wparam)
        {
            return wparam & 0xFFFF;
        }

        internal static int LOWORD(int wparam)
        {
            return wparam >> 16 & 0xFFFF;
        }

       
    }

    public class Win32
    {
        [DllImport("gdi32.dll")]
        public static extern int CreateRoundRectRgn(int x1, int y1, int x2, int y2, int x3, int y3);

        [DllImport("user32.dll")]
        public static extern int SetWindowRgn(IntPtr hwnd, int hRgn, Boolean bRedraw);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject", CharSet = CharSet.Ansi)]
        public static extern int DeleteObject(int hObject);
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowLong(IntPtr hWnd, int Index);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowLong(IntPtr hWnd, int Index, int Value);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int X, int Y, int cx, int cy, uint uFlags);

        public const int GWL_STYLE = -16;
        public const int GWL_EXSTYLE = -20;
        public const int WS_BORDER = 0x00800000;
        public const int WS_EX_CLIENTEDGE = 0x00000200;
        public const uint SWP_NOSIZE = 0x0001;
        public const uint SWP_NOMOVE = 0x0002;
        public const uint SWP_NOZORDER = 0x0004;
        public const uint SWP_NOACTIVATE = 0x0010;
        public const uint SWP_FRAMECHANGED = 0x0020;
        public const uint SWP_NOOWNERZORDER = 0x0200;

        public const int HTLEFT = 10;
        public const int HTRIGHT = 11;
        public const int HTTOP = 12;
        public const int HTTOPLEFT = 13;
        public const int HTTOPRIGHT = 14;
        public const int HTBOTTOM = 15;
        public const int HTBOTTOMLEFT = 0x10;
        public const int HTBOTTOMRIGHT = 17;
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        //public const int HTLEFT = 10;
        //public const int HTRIGHT = 11;
        // public const int HTTOP = 12;
        //public const int HTTOPLEFT = 13;
        //public const int HTTOPRIGHT = 14;
        //public const int HTBOTTOM = 15;
        //public const int HTBOTTOMLEFT = 0x10;
        //public const int HTBOTTOMRIGHT = 17;
        internal static readonly int HTCAPTION = 2;
        public const int WM_ERASEBKGND = 0x0014;
    }
}
