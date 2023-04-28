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
    public partial class ExceptionMessageBox : BaseForm
    {
        public ExceptionMessageBox()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            
        }
        public ExceptionMessageBox(Exception ex):this()
        {
            this.rtbMessage.Text = ex.Message;
            rtbStackTrace.Text = ex.StackTrace;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
