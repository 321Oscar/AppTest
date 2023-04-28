using AppTest.FormType;
using AppTest.Model;
using LPCanControl.CANInfo;
using System.Windows.Forms;

namespace AppTest.ViewModel
{
    public abstract class BaseViewModel
    {
        public BaseDataForm Form;
        public abstract void OnDataRecieveEvent(object sender, CANDataReceiveEventArgs args);

        private bool isGetdata;

        public bool VMIsGetdata
        {
            get => isGetdata;
            set
            {
                isGetdata = value;
                VMModifiedGetdata?.Invoke(value);
            }
        }

        /// <summary>
        /// 获取数据时，界面按钮变化，需要设置invoke
        /// </summary>
        /// <param name="get"></param>
        public ModifiedGetState VMModifiedGetdata;

        public SetLog ShowLog;

        public delegate void SetLog(string log,LPLogLevel level = LPLogLevel.Info); 
        public delegate void ModifiedGetState(bool getOrnot);

        public abstract void ModifiedSignals();

        public abstract void ShowSignalDetai(DataGridView dataGridView, DataGridViewCellEventArgs e);
    }
}
