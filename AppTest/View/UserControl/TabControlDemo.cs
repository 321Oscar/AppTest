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
    public partial class TabControlDemo : TabControl
    {
        const int CLOSE_SIZE = 15;

        public TabControlDemo()
        {
            InitializeComponent();

            this.Font = AppTest.Helper.Global.CurrentFont;

            this.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.Padding = new System.Drawing.Point(CLOSE_SIZE, 5);
            this.DrawItem += TabControlDemo_DrawItem;
            this.MouseDown += TabControlDemo_MouseDown;
        }

        private void TabControlDemo_MouseDown(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.SelectedTab.Text))
            {
                if(e.Button == MouseButtons.Left)
                {
                    int x = e.X, y = e.Y;

                    //计算关闭区域
                    Rectangle myTabRect = this.GetTabRect(this.SelectedIndex);

                    myTabRect.Offset(myTabRect.Width - 0x12, 2);
                    myTabRect.Width = CLOSE_SIZE;
                    myTabRect.Height = CLOSE_SIZE;

                    bool isClose = x > myTabRect.X && x < myTabRect.Right
                        && y > myTabRect.Y && y < myTabRect.Bottom;
                    if (isClose)
                    {
                        this.TabPages.Remove(this.SelectedTab);
                    }
                }
            }
        }

        private void TabControlDemo_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                //获取当前tab选项卡的绘图区域
                Rectangle myTabRect = this.GetTabRect(e.Index);

                //绘制标签头背景色
                using (Brush brBack = new SolidBrush(Color.White))
                {
                    if(e.Index == this.SelectedIndex)
                    {
                        e.Graphics.FillRectangle(brBack, myTabRect);//设置当前选中的tabpage的背景色
                    }
                }

                //先添加TabPage属性
                e.Graphics.DrawString(this.TabPages[e.Index].Text,
                    this.Font,SystemBrushes.ControlText,myTabRect.X + 2,myTabRect.Y +2);

                //再画一个矩形框
                using(Pen p = new Pen(Color.Transparent))
                {
                    myTabRect.Offset(myTabRect.Width - (CLOSE_SIZE + 3), 2);
                    myTabRect.Width = CLOSE_SIZE;
                    myTabRect.Height = CLOSE_SIZE;
                    e.Graphics.DrawRectangle(p, myTabRect);
                }
                //填充矩形框
                Color reColor = (e.State == DrawItemState.Selected) ? Color.Transparent :
                    Color.Transparent;

                using (Brush b = new SolidBrush(reColor))
                {
                    e.Graphics.FillRectangle(b, myTabRect);
                }

                //画Tab选项卡右上方关闭按钮
                using (Pen objpen = new Pen(Color.SlateBlue, 1.8f))
                {
                    //自己画X
                    //"\"线
                    Point p1 = new Point(myTabRect.X + 3, myTabRect.Y + 3);
                    Point p2 = new Point(myTabRect.X + myTabRect.Width - 3,
                        myTabRect.Y + myTabRect.Height - 3);
                    e.Graphics.DrawLine(objpen, p1, p2);
                    //"/"线
                    Point p3 = new Point(myTabRect.X + 3, myTabRect.Y + myTabRect.Height - 3);
                    Point p4 = new Point(myTabRect.X + myTabRect.Width - 3,myTabRect.Y + 3);
                    e.Graphics.DrawLine(objpen, p3, p4);
                }
                e.Graphics.Dispose();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        public void ShowTabPage(string tabPageName)
        {
            if (this == null || this.TabPages == null || this.TabPages.Count == 0)
                return;
            else
            {
                foreach (TabPage item in TabPages)
                {
                    if(item.Name == tabPageName)
                    {
                        this.SelectedTab = item;
                    }
                }
            }
        }
    }
}
