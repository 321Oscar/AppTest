using AppTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

    public class XCPSeedKeyHelper
    {
        [DllImport("SeedNKeyDll.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "XCP_ComputeKeyFromSeed")]
        public static extern void XCP_ComputeKeyFromSeedOut(byte privilege, byte byteLenSeed, byte[] seed, ref byte byteLenkey, [Out] IntPtr key);

        [DllImport("SeedNKeyDll.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "XCP_ComputeKeyFromSeed")]
        public static extern void XCP_ComputeKeyFromSeedOut(byte privilege, byte byteLenSeed, byte[] seed, ref byte byteLenkey, [Out] byte[] key);

        public static void GetKeyBySeed(byte[] seed, out byte[] key)
        {
            byte privilege = 0x01;
            byte byteLenkey = 0x0a;
            byte seedLength = (byte)seed.Length;

            key = new byte[4];

            XCP_ComputeKeyFromSeedOut(privilege, seedLength, seed, ref byteLenkey, key);
        }

        public static void GetKeyBySeedIntPtr(byte[] seed, out byte[] key)
        {
            byte privilege = 0x01;
            byte byteLenkey = 0x0a;
            byte seedLength = (byte)seed.Length;

            IntPtr keyPtr = IntPtr.Zero;

            XCP_ComputeKeyFromSeedOut(privilege, seedLength, seed, ref byteLenkey, keyPtr);

            key = new byte[byteLenkey];

            Marshal.Copy(keyPtr, key, 0, byteLenkey);

            try
            {
                Marshal.FreeHGlobal(keyPtr);
            }
            catch (Exception)
            {

            }
        }
    }

}
