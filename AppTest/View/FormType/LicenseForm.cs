using AppTest.Model;
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
        public LicenseForm()
        {
            InitializeComponent();
            dataGridView1.MultiSelect = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.RowHeadersVisible = true;
        }

        private void LicenseForm_Load(object sender, EventArgs e)
        {
           btnFresh_Click(null,null);
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
    }
}
