using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using AppTest.UI;

namespace AppTest.FormType.MaterailSkin
{
    public partial class MSForm : MaterialForm
    {
        public MSForm()
        {
            InitializeComponent();

            // Create a material theme manager and add the form to manage (this)
            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;

            // Configure color schema
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Blue400, Primary.Blue500, Primary.Blue500, Accent.LightBlue200, TextShade.WHITE
            );

            //this.IsMdiContainer = true;
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            UINotifier.Show("111", UINotifierType.INFO, UILocalize.InfoTitle, false, 2000);
           
        }

        private void materialRaisedButton2_Click(object sender, EventArgs e)
        {
            UINotifier.Show("11221", UINotifierType.WARNING, UILocalize.WarningTitle, false, 2000);
        }

        private void materialRaisedButton3_Click(object sender, EventArgs e)
        {
            UIMessageTip.Show("111", TipStyle.Red, 1000, true);
        }
    }
}
