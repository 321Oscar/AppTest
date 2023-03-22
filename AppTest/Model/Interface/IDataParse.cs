using LPCanControl.CANInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AppTest.Model.Interface
{
    public interface IDataParse
    {
        void OnDataRecieveEvent(object sender, CANDataRecieveEventArgs args);
    }
}
