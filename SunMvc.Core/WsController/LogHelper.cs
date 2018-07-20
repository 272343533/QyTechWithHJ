using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Reflection;
using log4net;
using SunMvcExpress.Core.Helpers;
using System.Web.Routing;
using System.Web;
using System.Diagnostics;

namespace QyTech.Core
{
    public class LogHelper 
    {
        public static ILog log = log4net.LogManager.GetLogger("WSExpress");


        public static void Info(string methodname,string msg)
        {
            log = LogManager.GetLogger(methodname);
            log.Info(msg);
        }

        public static void Error(Exception ex)
        {
            string methodName = GetMethodName(2);
            string msg = ex.Message;
            if (ex.InnerException != null)
            {
                msg += "(" + ex.InnerException.Message + ")";
            }
            Error(methodName, msg);
        }
        public static void Error(string adddesp, Exception ex)
        {
            string methodName = GetMethodName(2);
            string msg = adddesp+"---"+ex.Message;
            if (ex.InnerException != null)
            {
                msg += "(" + ex.InnerException.Message + ")";
            }
            Error(methodName, msg);
        }
        public static void Error(string adddesp, string msg)
        {
            string methodName = GetMethodName(2);

            log = LogManager.GetLogger(methodName+"-"+adddesp);
            log.Error(msg);
        }

        public static string GetMethodName(int layer)
        {
            var method = new StackFrame(layer).GetMethod(); // 这里忽略1层堆栈，也就忽略了当前方法GetMethodName，这样拿到的就正好是外部调用GetMethodName的方法信息
            var property = (
            from p in method.DeclaringType.GetProperties(
            BindingFlags.Instance |
            BindingFlags.Static |
            BindingFlags.Public |
            BindingFlags.NonPublic)
            where p.GetGetMethod(true) == method || p.GetSetMethod(true) == method
            select p).FirstOrDefault();
            return property == null ? method.Name : property.Name;
        }
    }
}
