using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppTest.Helper
{
    public static class ReflectionHelper
    {
        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullname">命名控件.类型名</param>
        /// <param name="assemblyName">程序集</param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T CreateInstance<T>(string fullname, string assemblyName,object[] args=null)
        {
            string path = fullname + "," + assemblyName;//命名控件.类型名,程序集

            Type o = Type.GetType(path);//加载类型
            object obj = Activator.CreateInstance(o,args:args);//根据类型创建实例

            return (T)obj;
        }

        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <typeparam name="T">要创建对象的类型</typeparam>
        /// <param name="assemblyName">类型所在程序集名称</param>
        /// <param name="nameSpace">类型所在命名空间</param>
        /// <param name="className">类型名</param>
        /// <returns></returns>
        public static T CreateInstance<T>(string assemblyName, string nameSpace, string className)
        {
            try
            {
                string fullName = nameSpace + "." + className;

                object ect = Assembly.Load(assemblyName).CreateInstance(fullName);

                return (T)ect;
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
