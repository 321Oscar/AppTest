using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTest.Model.Interface
{
    internal interface ILogForm
    {
        void ShowLog(string log,LPLogLevel level = LPLogLevel.Info, bool showtip = false);
    }
}
