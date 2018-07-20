using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SunMvcExpress.Core.Helpers
{
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    //public class LoginFilterAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        if (!LoginHelper.IsLogin() || HttpContext.Current.Request.Cookies["Zhongwei"] == null)
    //        {
    //            HttpContext.Current.Response.Redirect("/Home/Login");
    //            return;
    //        }
    //        else
    //        {
    //            if (LoginHelper.GetLoginUserID() == Guid.Empty)
    //            {
    //                return;
    //            }
    //            string sysfunc = filterContext.Controller.ToString();
    //            int m = sysfunc.LastIndexOf('.');
    //            int msize = sysfunc.Length - m - 11;
    //            sysfunc = sysfunc.Substring(m + 1, msize);
    //            string actionname = filterContext.ActionDescriptor.ActionName;

    //            if (!LoginHelper.isusefunc(LoginHelper.GetLoginUserID(), sysfunc, actionname))
    //            {
    //                HttpContext.Current.Response.Redirect("/Home/Error");
    //                return;
    //            }
    //        }
    //    }

    //}
}