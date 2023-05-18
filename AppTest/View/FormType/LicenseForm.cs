using AppTest.Model;
using AppTest.Utils.LogUtils;
using AppTest.View.UC;
using log4net;
using log4net.Layout;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppTest.View.FormType
{
    public partial class LicenseForm : Form
    {
        private ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public LicenseForm()
        {
            InitializeComponent();
            dataGridView1.MultiSelect = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.RowHeadersVisible = true;

            //LogListview log = new LogListview();
            //log.Dock = DockStyle.Bottom;
            //this.Controls.Add(log);
        }

        private void LicenseForm_Load(object sender, EventArgs e)
        {
           btnFresh_Click(null,null);

            var logger = (log4net.Repository.Hierarchy.Logger)LogManager.GetLogger("root").Logger;
            var logPattern = "%d{yyyy-MM-dd HH:mm:ss} --%-5p-- %m%n";
            var textbox_logAppender = new TextBoxBaseAppender()
            {
                TextBox = this.richTextBox1,
                Layout = new PatternLayout(logPattern)
            };

            log4net.Config.BasicConfigurator.Configure(logger.Repository, textbox_logAppender);
        }

        private async void btnFresh_Click(object sender, EventArgs e)
        {
            var db = await DBHelper.GetAuthenticationDb();

            // var allEntities = await db.signalEntities.ToListAsync();
            string sqlStr = $"select * from AuthenticationEntity";

            var entities = await db.QueryAsync<AuthenticationEntity>(sqlStr);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = entities;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            log.Debug("debug");
            log.Info("info");
            log.Warn("warn");
            log.Error("error");
        }
    }
}
