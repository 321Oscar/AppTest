using AppTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTest.Helper
{
    public class XCPModuleManager
    {
        static List<XCPModule> XCPModules = new List<XCPModule>();

        public static void AddXCPModule(XCPModule xCPModule)
        {
            XCPModules.Add(xCPModule);
        }

        public static XCPModule GetXCPModule(ProjectItem projectItem)
        {
            var module = XCPModules.Find(x => x.ProjectItem == projectItem);
            if (module == null)
                throw new Exception("无有效的XCP连接信息");
            return module;
        }

        public static void Remove(XCPModule xCPModule)
        {
            XCPModules.Remove(xCPModule);
        }
    }

}
