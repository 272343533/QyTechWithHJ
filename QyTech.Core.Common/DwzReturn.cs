using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Web;

using System.Web.Mvc;
using System.Data.Objects.DataClasses;
using System.Data.Objects;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;

using QyTech.Core.Helpers;


namespace QyTech.Core.Common
{
    public class DwzReturn
    {
        public static AjaxJsonResult SuccessAjaxJsonResult(string navtabid)
        {
            AjaxJsonResult rs = null;
            rs = new AjaxJsonResult
            {
                statusCode = "200",
                message = "操作成功",
                navTabId = navtabid,
                rel = "",
                callbackType = "closeCurrent",
                forwardUrl = ""
            };
            return rs;
        }
        public static AjaxJsonResult SuccessAjaxJsonResultNotClosed(string navtabid)
        {
            AjaxJsonResult rs = null;
            rs = new AjaxJsonResult
            {
                statusCode = "200",
                message = "操作成功",
                navTabId = navtabid,
                rel = "",
                callbackType = "",
                forwardUrl = ""
            };
            return rs;
        }

        public static AjaxJsonResult FailAjaxJsonResult()
        {
            AjaxJsonResult rs = null;
            rs = new AjaxJsonResult
            {
                statusCode = "300",
                message = "操作失败",
                navTabId = "",
                rel = "",
                callbackType = "",
                forwardUrl = ""
            };

            return rs;
        }

        public static AjaxJsonResult FailAjaxJsonResult(string reason)
        {
            AjaxJsonResult rs = null;
            rs = new AjaxJsonResult
            {
                statusCode = "300",
                message = reason,
                navTabId = "",
                rel = "",
                callbackType = "",
                forwardUrl = ""
            };

            return rs;
        }
        public static AjaxJsonResult FailAjaxJsonResult(Exception ex)
        {
            string reason = ex.Message;
            if (ex.InnerException != null)
            {
                reason += "(" + ex.InnerException.Message + ")";
            }
            AjaxJsonResult rs = null;
            rs = new AjaxJsonResult
            {
                statusCode = "300",
                message = reason,
                navTabId = "",
                rel = "",
                callbackType = "",
                forwardUrl = ""
            };

            return rs;
        }

    }

}
