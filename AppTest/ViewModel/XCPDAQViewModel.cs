using AppTest.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppTest.ViewModel
{
    public abstract class XCPDAQViewModel : XCPViewModel, IXCPDAQ
    {
        public DAQList DAQList = new DAQList();
        
        public Dictionary<int, List<byte>> recieveData;

        public abstract bool InitDAQ(uint canChannel);
        public abstract Task ParseResponeToXCPSignalAsync(List<byte> data, int eventIndex);

    }
}
