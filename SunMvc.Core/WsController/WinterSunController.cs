using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Reflection;
using log4net;
using SunMvcExpress.Core.Helpers;
using SunMvcExpress.Dao;
using QyTech.Core;
namespace SunMvcExpress.Core
{
    public class WSExpressController:Controller 
    {
        public bsUser LoginUser = new bsUser();
        public Guid Login_User_ID = Guid.Empty;
        public string Login_User_Name = "";
        public Guid Login_UserCustId =Guid.Empty;
        public Guid Login_UserOrgId = Guid.Empty;

        public bsOrganize Login_UserOrg = null;
        public LogHelper logHelper = new LogHelper();

        public static ILog log = log4net.LogManager.GetLogger("WSExpress");

        public string errMsg;

        public string RefreshRel = "";
        public string FcId;

        public WSExpressController()
        {
            try
            {
                if (LoginHelper.IsLogin())
                {
                    Login_User_ID = LoginHelper.GetLoginUserId();
                    Login_User_Name = LoginHelper.GetLoginUserName();
                    Login_UserCustId = LoginHelper.GetLoginCompanyId();
                    Login_UserOrgId = LoginHelper.GetLoginOrgId();

                    Login_UserOrg = LoginHelper.GetLoginOrg();
                    LoginUser = LoginHelper.GetLoginUser();
                    logHelper = new LogHelper();
                }
            }
            catch(Exception ex)
            {
                LogHelper.Error(ex);
                RedirectToAction("newindex", "Home");
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LogHelper.Info(Request.Url.ToString(), "验证登录信息");
            
            if (Login_User_ID == null || Login_User_ID==Guid.Empty)
            {
                LogHelper.Info(Request.Url.ToString(), "登录信息已过期，请重新登录，然后操作");
                filterContext.Result = RedirectToAction("newlogin", "Home"); // new RedirectToRouteResult("Login", new RouteValueDictionary { { "from", Request.Url.ToString() } });
            }
            else
            {
                LogHelper.Info(Request.Url.ToString(), "登录信息有效，继续操作");
                base.OnActionExecuting(filterContext);
            }
        }
     
        public string  ReturnErrMessage(Exception ex)
        {
            string errmsg = ex.Message;
            if (ex.InnerException != null)
                errmsg += " detail:" + ex.InnerException.Message;
            return errmsg;
        }
        public ILog GetCurrentLogWithAction()
        {
            return log4net.LogManager.GetLogger(RouteData.Route.GetRouteData(this.HttpContext).Values["action"].ToString());
        }
        public ILog GetCurrentLogWithController()
        {
            //            RouteData.Route.GetRouteData(this.HttpContext).Values["controller"] 
            //RouteData.Route.GetRouteData(this.HttpContext).Values["action"] 
            //或 
            //RouteData.Values["controller"] 
            //RouteData.Values["action"] 
            return log4net.LogManager.GetLogger(RouteData.Values["controller"].ToString());
        }

        public void logInfo(string msg)
        {
            log = GetCurrentLogWithAction();
            log.Info(msg);
        }
        public void logFatal(Exception ex)
        {
            log = GetCurrentLogWithAction();
            string msg = ex.Message;
            if (ex.InnerException != null)
            log.Fatal(msg);
        }



        public void logError(string msg)
        {
            log = GetCurrentLogWithAction();
            log.Error(msg);
        }
        public void logError(Exception ex)
        {
            log = GetCurrentLogWithAction();
            string msg = ex.Message;
            if (ex.InnerException != null)
                msg += "(detail:" + ex.InnerException.Message + ")";
            log.Error(msg);
        }
        //public JsonResult treeDrag<T>(Guid id, Guid pid)
        //{
        //    T objdb = EntityManager<T>.GetByPk<T>("Id",id);
        //    Type type = typeof(T);
        //    object obj = Activator.CreateInstance(type);
        //    PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        //    foreach (PropertyInfo item in props)
        //    {
        //        if (item.Name == "PId")
        //        {
        //            item.SetValue(obj, Convert.ChangeType(item.Name, item.PropertyType), null);
        //        }
        //    }
        //    EntityManager<T>.Modify<T>(objdb);

        //    return null;
        //}
        protected ActionResult ReturnFile(string saveToPath, string downFileName)
        {
            LogHelper.Info("ReturnFile", saveToPath + " down(ws) to:" + downFileName + "浏览器：" + HttpContext.Request.Browser.Browser);
        
            if (HttpContext.Request.Browser.Browser == "InternetExplorer" || HttpContext.Request.Browser.Browser == "IE")
            {
                return File(saveToPath, "application/octet-stream; charset=utf-8", Url.Encode(downFileName));
            }
            else if (HttpContext.Request.Browser.Browser == "Chrome")
            {
                //return File(saveToPath, "application/octet-stream; charset=utf-8", downFileName);
                return File(saveToPath, "application/msexcel; charset=utf-8", downFileName);
            }
            else
            {
                return File(saveToPath, "application/octet-stream; charset=utf-8", HttpContext.Request.Browser.Browser + downFileName);
            }
        }
    }
}
