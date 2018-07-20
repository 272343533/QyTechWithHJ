using System;
using System.IO;
using System.Xml;

namespace SunMvcExpress.Common.Xml
{
    /// <summary>
    /// 读写XML配置文件appSettings节点下的键值，如注册服务、DataBase的IP
    /// </summary>
    public class XmlConfig
    {
        private static string fullFileName = String.Format("{0}app.config", AppDomain.CurrentDomain.BaseDirectory);
        // private static ILog log = LogManager.GetLogger(typeof(object));
        
        public static void SetValue(string node, string AppKey, string AppValue, string FullFileName)
        {
            if (File.Exists(FullFileName))
            {
                XmlDocument xDoc = new XmlDocument();

                try
                {
                    //此处配置文件在程序目录下
                    xDoc.Load(FullFileName);
                    XmlNode xNode;
                    XmlElement xElem1;
                    XmlElement xElem2;
                    xNode = xDoc.SelectSingleNode("//" + node);
                    xElem1 = (XmlElement)xNode.SelectSingleNode("//add[@key='" + AppKey + "']");
                    if (xElem1 != null)
                    {
                        xElem1.SetAttribute("value", AppValue);
                    }
                    else
                    {
                        xElem2 = xDoc.CreateElement("add");
                        xElem2.SetAttribute("key", AppKey);
                        xElem2.SetAttribute("value", AppValue);
                        xNode.AppendChild(xElem2);
                    }
                    xDoc.Save(FullFileName);
                }
                catch (Exception)
                {
                    xDoc = null;   
                }
                
            }
        }

        public static void SetValue(string AppKey, string AppValue, string FullFileName)
        {
            SetValue("appSettings", AppKey, AppValue, FullFileName);
        }

        public static void SetValue(string AppKey, string AppValue)
        {
            SetValue(AppKey, AppValue, fullFileName);
        }

        public static string GetValue(string node, string AppKey, string FullFileName)
        {
            // log.Info(FullFileName);
            string Result = "";

            if (File.Exists(FullFileName))
            {
                XmlDocument xDoc = new XmlDocument();

                try
                {
                    //此处配置文件在程序目录下
                    xDoc.Load(FullFileName);
                    XmlNode xNode;
                    XmlElement xElem1;
                    xNode = xDoc.SelectSingleNode("//" + node );
                    xElem1 = (XmlElement)xNode.SelectSingleNode("//add[@key='" + AppKey + "']");
                    if (xElem1 != null)
                    {
                        Result = xElem1.GetAttribute("value");
                    }
                }
                catch (Exception ex)
                {                    
                     xDoc = null;   
                }                
            }

            return Result.Trim();
        }

        public static string GetValue(string AppKey, string FullFileName)
        {
            return GetValue("appSettings", AppKey, FullFileName);
        }

        public static string GetValue(string AppKey)
        {
            return GetValue(AppKey, fullFileName);
        }
    }
}
