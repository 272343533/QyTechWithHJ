using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using SunMvc.Core.Helpers;
namespace SunMvc.Core.Common
{
    public class BaseController : Controller
    {
        public Guid Login_User_ID = LoginHelper.GetLoginUserId();
        public string Login_User_Name = LoginHelper.GetLoginUserName();
        public Guid Login_UserOrgId = LoginHelper.GetLoginCompanyId();
		
        public static ILog log = log4net.LogManager.GetLogger("BaseController");

        public void SetTitle(string title)
        {
            ViewData["Title"] = title;
        }

        public class DiyViewPage : System.Web.Mvc.ViewPage
        {
            public DiyViewPage()
            {
                this.Title = "???";
            }


        }
    }
}