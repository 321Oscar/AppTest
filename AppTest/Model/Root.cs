using AppTest.FormType.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace AppTest.Model
{
    public static class RootHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Json路径</param>
        /// <param name="oldRoot">root</param>
        /// <returns></returns>
        public static Root InitRootByJson(string path,Root oldRoot = null)
        {
            string jsonStr;

            try
            {
                FileStream fs = null;
                StreamReader sr = null;

                try
                {
                    fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    sr = new StreamReader(fs, System.Text.Encoding.UTF8);

                    jsonStr = sr.ReadToEnd();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (sr != null)
                    {
                        sr.Close();
                    }
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }

                if (oldRoot == null)
                {
                    oldRoot = JsonConvert.DeserializeObject<Root>(jsonStr);
                }
                else
                {
                    Root rImport = JsonConvert.DeserializeObject<Root>(jsonStr);
                    foreach (var project in rImport.project)
                    {
                        if (oldRoot.project.Find(x => x.Name == project.Name) == null)
                        {
                            oldRoot.project.Add(project);
                        }
                    }
                }

                //LoadTreeView();
            }
            catch (Exception ex)
            {
                LeapMessageBox.Instance.ShowError(ex.Message);
            }

            return oldRoot;
        }
    } 

    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ProjectItem> project { get; set; }

        public Root()
        {
            this.project = new List<ProjectItem>();
        }
    }



}
