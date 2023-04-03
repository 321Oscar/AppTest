using AppTest.FormType;
using LPCanControl.CANInfo;
using System.Windows.Forms;

namespace AppTest.ViewModel
{
    public abstract class BaseViewModel
    {
        public BaseDataForm Form { get; set; }
        public abstract void OnDataRecieveEvent(object sender, CANDataReceiveEventArgs args);

        private bool isGetdata;

        public bool IsGetdata
        {
            get => isGetdata;
            set
            {
                isGetdata = value;
                ModifiedGetdata?.Invoke(value);
            }
        }

        /// <summary>
        /// 获取数据时，界面按钮变化，需要设置invoke
        /// </summary>
        /// <param name="get"></param>
        public ModifiedGetState ModifiedGetdata;

        public SetLog ShowLog;

        public delegate void SetLog(string log); 
        public delegate void ModifiedGetState(bool getOrnot);

        public abstract void ModifiedSignals();

        public abstract void ShowSignalDetail(DataGridView dataGridView, DataGridViewCellEventArgs e);
    }
}
