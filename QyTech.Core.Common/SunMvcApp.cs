using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using SunMvcExpress.Common.Xml;
namespace QyTech.Core.Common
{
    public class SunMvcApp
    {

        public static bool isValid()
        {
            bool SafeModeForQy;// = XmlConfig.GetValue("SafeModeForQy") == "9999" ? true : false ;
            SafeModeForQy = true;
            if ((AppMapPath == @"D:\QinYiMvc\SunMvcExpress\" && WebSiteUrlHost == @"http://localhost" && SafeModeForQy))//|| (WebSiteUrlHost == @"http://122.114.38.213")
                return true;
            else
                return false;
       }
        //如下代码是获取URL
        public static string WebSiteUrl
        {
            get
            {
                string url = System.Web.HttpContext.Current.Request.Url.ToString();
                return url;
            }
        }
        public static string WebSiteUrlHost
        {
            get
            {
                string url= "http://" + System.Web.HttpContext.Current.Request.Url.Host.ToString();
                return url;
            }
        }
        public static string AppUrl
        {
            get
            {
                return WebSiteUrl + AppSiteName;
            }
        }

        
        public static string AppSiteName
        {
            get
            {
                string SiteAddress = "";
                SiteAddress = System.Web.HttpContext.Current.Request.ApplicationPath.ToString();
                if (System.Web.HttpContext.Current.Request.ApplicationPath.ToString() == "/")
                {
                    SiteAddress = "";
                }
                else
                {
                    SiteAddress = System.Web.HttpContext.Current.Request.ApplicationPath.ToString();
                }
                return SiteAddress.ToString();
            }
        }

        //如下代码是获取物理地址
        public static string AppMapPath
        {
            get
            {
                string ApplicationPath = System.Web.HttpContext.Current.Server.MapPath("~/");
                if (ApplicationPath.EndsWith("//") == true)
                {
                    ApplicationPath = ApplicationPath.Remove(ApplicationPath.Length - 1);
                }
                return ApplicationPath;
     　　       }
  　　      }
  　　  }
　　}

