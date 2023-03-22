using AppTest.ProjectClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppTest.FormType
{
    public partial class SignalItemForm<T> : BaseForm
        where T :new()
    {
        T t;
        public T TValue { get => t; }
        
        public SignalItemForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 新建自定义信号
        /// </summary>
        public SignalItemForm(bool isNew) : this()
        {
            this.Text = "新建自定义信号";
            LoadPanel(new T(), false);
        }

        /// <summary>
        /// 查看信号的信息
        /// </summary>
        /// <param name="signal"></param>
        public SignalItemForm(T t,string formName = null,bool isReadOnly = true) : this()
        {
            if (formName != null)
                this.Text = formName;
            LoadPanel(t, isReadOnly);
        }

        private void LoadPanel(T t,bool read = true)
        {
            //this.Name = 
            this.t = t;
            panelInfo.Controls.Clear();
            
            Type listType = typeof(T);
            PropertyInfo[] properties = listType.GetProperties();
            int x = -10;
            int y = 10;
            for (int i = 0; i < properties.Length; i++)
            {
                object o = properties[i].GetValue(t);
                Type type = properties[i].PropertyType;
                var tValue = Convert.ChangeType(o, type);
                string propertyValue = o == null ? "--" : o.ToString();
                Label l = new Label();
                l.Text = properties[i].Name + ":";
                l.TextAlign = ContentAlignment.MiddleRight;
                l.Location = new Point(x, y);
                //l.AutoSize = true;
                panelInfo.Controls.Add(l);
                var des = properties[i].GetCustomAttribute(typeof(SignalAttribute), false);
                Control c;
               
                
                if (des != null)
                {
                    SignalAttribute sa = des as SignalAttribute;
                    switch (sa.Type)
                    {
                        case "int":
                            c = new NumericUpDown();
                            //panelInfo.Controls.Add(c);
                            ((NumericUpDown)c).Minimum = (decimal)sa.Min;
                            ((NumericUpDown)c).Maximum = (decimal)sa.Max;
                            c.Text = propertyValue;
                            break;
                        case "bool":
                            c = new CheckBox();
                            break;
                        case "enum":
                            c = new ComboBox();
                            ((ComboBox)c).Parent = this;
                           
                            Dictionary<int, string> comboxDatasource = new Dictionary<int, string>();
                            //var enumkey = sa.EnumKey;
                            for (int j = 0;j < sa.EnumKey.Length; j++)
                            {
                                comboxDatasource.Add(sa.EnumKey[j], sa.EnumString[j]);
                            }
                            ((ComboBox)c).DataSource = new BindingSource() { DataSource = comboxDatasource };
                            ((ComboBox)c).ValueMember = "Key";
                            ((ComboBox)c).DisplayMember = "Value";
                            ///绑定数据后，设置Parent后，设置selectedvalue才起效果
                            ((ComboBox)c).Parent = this.panelInfo;
                            ((ComboBox)c).DropDownStyle = ComboBoxStyle.DropDownList;
                            
                            ((ComboBox)c).SelectedValue = tValue;
                            break; 
                        default:
                            c = new TextBox();
                            c.Text = propertyValue;
                            break;
                    }
                    
                }
                else
                {
                    c = new TextBox();
                    c.Text = propertyValue;
                }
                panelInfo.Controls.Add(c);
                c.Name = properties[i].Name;
                Graphics g = c.CreateGraphics();
                int w = Convert.ToInt32(g.MeasureString(c.Text, c.Font).Width);
                c.Width = (w > l.Width) ? w : l.Width;
                if (des != null && ((SignalAttribute)des).Description != null)
                {
                    toolTip1.SetToolTip(c, ((SignalAttribute)des).Description);
                    toolTip1.SetToolTip(l, ((SignalAttribute)des).Description);
                }

                c.Enabled = !read;
                c.Location = new Point(x + l.Width, y);
                y = y + c.Height + 10;
               
            }

            this.btnConfirm.Enabled = !read;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            Type listType = typeof(T);
            var properties = listType.GetProperties().ToList();
            foreach (Control item in panelInfo.Controls)
            {
                if (string.IsNullOrEmpty(item.Name))
                    continue;
                var property = properties.Find(x => x.Name == item.Name);

                Type type = property.PropertyType;
                if (item is TextBox || item is NumericUpDown)
                {
                    var targetValue = Convert.ChangeType(item.Text, type);
                    property.SetValue(t, targetValue);
                }
                else if (item is ComboBox)
                {
                    var targetValue = Convert.ChangeType((item as ComboBox).SelectedValue, type);
                    property.SetValue(t, targetValue);
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
            this.Dispose();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            this.Dispose();

        }
    }
}
