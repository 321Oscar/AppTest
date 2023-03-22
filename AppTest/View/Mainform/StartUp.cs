using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppTest.Mainform
{
    public partial class StartUp : BaseForm
    {
        public StartUp():base()
        {
            InitializeComponent();
            CenterToScreen();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (rbMainForm.Checked)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.Yes;
                //Application.Run(new MainFormV2());
            }
            this.Close();
            this.Dispose();
        }
    }
}
