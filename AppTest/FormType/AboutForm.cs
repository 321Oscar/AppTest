using AppTest.Helper;
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
    public partial class AboutForm : BaseForm
    {
        public AboutForm()
        {
            InitializeComponent();
            this.Font = Global.CurrentFont;
            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;

            this.Text = "关于";

            this.label2.Text = Global.SoftVersion;


        }
    }
}
