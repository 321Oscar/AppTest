using AppTest.Helper;

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
