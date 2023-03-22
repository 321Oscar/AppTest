using AppTest.FormType.Helper;
using AppTest.Helper;
using AppTest.ProjectClass;
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
    public partial class ProjectPanelUC : TabPage//Form//TabPage
    {
        //Panel panelForm;
        ProjectItem projectItem;

        /// <summary>
        /// 打开的Project 
        /// </summary>
        public ProjectItem ProjectItem { get => projectItem; set {
                projectItem = value;
            } }

        public bool IsAdded { get => isAdded; set {
                if (value != isAdded || value == true)
                    WhenProjectFormChange();
                isAdded = value;
            } 
        }

        bool isAdded;

        public delegate void ProjectFormAddedEventHandler(object sender,EventArgs args);

        /// <summary>
        /// Form 增加事件
        /// </summary>
        public event ProjectFormAddedEventHandler ProjectFormChanged;

        private void WhenProjectFormChange()
        {
            if (ProjectFormChanged != null)
                ProjectFormChanged(this, null);
        }

        //Timer timer;
        Button btnStartCan;

        public ProjectPanelUC()
        {
            InitializeComponent();
            this.SetAutoSizeMode(mode: AutoSizeMode.GrowOnly);
            this.Font = Global.CurrentFont;
            panel2.AutoScroll = true;
            panel2.AutoScrollMinSize = new Size(100, 100);
            //Panel p = new Panel();
            //p.Height = 50;
            //p.Dock = DockStyle.Top;

            btnStartCan  = new Button();
            btnStartCan.Text = "StartCan";
            btnStartCan.Width = 100;
            btnStartCan.Height = 30;
            btnStartCan.Location = new Point(10, 5);
            btnStartCan.Click += BtnStartCan_Click;
            panel1.Controls.Add(btnStartCan);

            Button btnNewForm = new Button();
            btnNewForm.Text = "NewForm";
            btnNewForm.Width = 100;
            btnNewForm.Height = 30;
            btnNewForm.Click += BtnNewForm_Click;
            btnNewForm.Location = new Point(10+ btnStartCan.Width + btnStartCan.Location.X, 5);
            panel1.Controls.Add(btnNewForm);
            
            //TextBox tbRecieveInteral = new TextBox();
            //tbRecieveInteral.Name = "tbRecieveInteral";
            //tbRecieveInteral.Width = 150;
            //tbRecieveInteral.Height = 30;
            //tbRecieveInteral.Location = new Point(10 + btnNewForm.Width + btnNewForm.Location.X, 10);
            //panel1.Controls.Add(tbRecieveInteral);

            //Button btnChangeTimerInterval = new Button();
            //btnChangeTimerInterval.Text = "Change";
            //btnChangeTimerInterval.Width = 100;
            //btnChangeTimerInterval.Height = 30;
            //btnChangeTimerInterval.Click += BtnNewForm_Click;
            //btnChangeTimerInterval.Location = new Point(10 + tbRecieveInteral.Width + tbRecieveInteral.Location.X, 5);
            //panel1.Controls.Add(btnChangeTimerInterval);
            //cbbRecieveInteral.DropDownStyle = ComboBoxStyle.DropDownList;

            //Button btnSaveConfig = new Button();
            //btnSaveConfig.Text = "SaveConfig";
            //btnSaveConfig.Click += BtnNewForm_Click;
            //btnSaveConfig.Location = new Point(10 + btnNewForm.Width + btnNewForm.Location.X, 5);


            //this.Controls.Add(p);

            //panelForm = new Panel();
            //panelForm.Dock = DockStyle.Fill;
            //this.Controls.Add(panelForm);
        }

        private void BtnNewForm_Click(object sender, EventArgs e)
        {
            AddNewForm addNewForm = new AddNewForm(this.projectItem);
            addNewForm.StartPosition = FormStartPosition.CenterScreen;
            if (addNewForm.ShowDialog() == DialogResult.OK)
            {
                //
                if(ProjectItem.Form.Find(x=>x.Name == addNewForm.FormItem.Name) != null)
                {
                    MessageBox.Show("名称重复");
                    return;
                }
                this.ProjectItem.Form.Add(addNewForm.FormItem);
                IsAdded = true;
                BaseDataForm userForm = FormCreateHelper.CreateForm(addNewForm.FormItem, projectItem); 
                if(userForm != null)
                {
                    this.panel2.Controls.Add(userForm);
                    userForm.ShowAndLoad();
                }
                else
                {
                    MessageBox.Show("打开【"+ addNewForm.FormItem.Name + "】窗口失败");
                }
            }
        }

        private bool CanIsOpen = false;

        private void BtnStartCan_Click(object sender, EventArgs e)
        {
            ///can added to USBCanManager
            if (!USBCanManager.Instance.Exist(projectItem))
            {
                UsbCan usbCan = new UsbCan(projectItem.DeviceType, projectItem.DeviceIndex, 0);
                USBCanManager.Instance.AddUsbCan(usbCan, projectItem);
            }

            if (CanIsOpen)
            {
                if (USBCanManager.Instance.Close(projectItem))
                {
                    btnStartCan.Text = "Start";
                    CanIsOpen = false;
                }
            }
            else
            {
                ///can.open 
                if (USBCanManager.Instance.Open(projectItem))
                {
                    foreach (var item in projectItem.CanIndex)
                    {
                        try
                        {
                            switch ((DeviceType)projectItem.DeviceType)
                            {
                                case DeviceType.VCI_USBCAN_2E_U:
                                    ///Can.SetReference
                                    USBCanManager.Instance.SetRefenrece(projectItem, item.CanChannel);
                                    break;
                            }
                            ///can.init
                            USBCanManager.Instance.InitCan(projectItem, item.CanChannel);
                            ///can.start
                            USBCanManager.Instance.StartCanIndex(projectItem, item.CanChannel);
                        }
                        catch (Exception ex)
                        {
                            LeapMessageBox.Instance.ShowError("Init Can Error：" + ex.Message);
                            continue;
                        }
                        
                    }
                    CanIsOpen = true;
                    btnStartCan.Text = "Close";
                    LeapMessageBox.Instance.ShowInfo("Open and Start Can");
                }
                else
                {
                    LeapMessageBox.Instance.ShowInfo("打开失败，请检查配置和设备");
                }
                
            }
            
            
            //MessageBox.Show("Start Can");

            //CanDevice start 
        }

        /// <summary>
        /// 界面上增加控件
        /// </summary>
        /// <param name="c"></param>
        public void AddControl(Control c)
        {
            foreach (Control control in panel2.Controls)
            {
                if (control.Name == c.Name&& control is Form) 
                {
                   // (control as Form).TopMost = true;
                    (control as Form).WindowState = FormWindowState.Maximized;
                    return;
                }
            }

            if(c is Form)
            {
                (c as Form).TopLevel = false;
                //(c as Form).WindowState = FormWindowState.Minimized;
            }

            c.Move += C_Move;
            c.LocationChanged += C_LocationChanged;

            panel2.Controls.Add(c);
        }

        private void C_LocationChanged(object sender, EventArgs e)
        {
            Control c = sender as Control;
            int x = c.Location.X > 0 ? c.Location.X : 0;
            int y = c.Location.Y > 0 ? c.Location.Y : 0;
            c.Location = new Point(x,y);
        }

        private void C_Move(object sender, EventArgs e)
        {
            Control f = sender as Control;
            int? x = null, y = null;
            if (f.Location.X + f.Width > this.DisplayRectangle.Width)
            {
                x = f.Location.X + f.Width;// > this.DisplayRectangle.Width ? f.Location.X + f.Width : this.DisplayRectangle.Width;
            }
            if (f.Location.Y + f.Height > this.DisplayRectangle.Height)
            {
                y = f.Location.Y + f.Height;// > this.DisplayRectangle.Height ? f.Location.Y + f.Height : this.DisplayRectangle.Height;
            }
            if (x != null || y != null)
            {
                Label l = new Label();
                panel2.Controls.Add(l);
                panel2.Controls.Remove(l);
            }
        }
    }
}
