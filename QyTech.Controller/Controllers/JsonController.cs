using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SunMvcExpress.Dao;
using SunMvcExpress.Core.Models;
using SunMvcExpress.Core.BLL;
using SunMvcExpress.Core.Helpers;
using SunMvcExpress.BLL;
using SunMvcExpress.Core.Common;

using QyTech.Core.Common;
using QyTech.Core.Helpers;
using QyTech.Core.BLL;


namespace SunMvcExpress.Controllers
{
    public class PointTimeCorr {
        public DateTime X;
        public decimal? Y;
    }
    public class GathCurve
    {
        public string CurveName;
        public List<PointTimeCorr> Points;
    }
    public class JsonController:Controller 
    {

        public JsonResult EditType()
        {
            List<SimpleSuggetJsonObject> list = new List<SimpleSuggetJsonObject>();
            SimpleSuggetJsonObject obj = new SimpleSuggetJsonObject(1,"单选框","Radio");
            list.Add(obj);
            obj = new SimpleSuggetJsonObject(2, "复选框", "Radio");
            list.Add(obj);
            obj = new SimpleSuggetJsonObject(3, "下拉框", "Radio");
            list.Add(obj);
            obj = new SimpleSuggetJsonObject(4, "提示框", "Radio");
            list.Add(obj);
            obj = new SimpleSuggetJsonObject(5, "单一带回", "Radio");
            list.Add(obj);
            obj = new SimpleSuggetJsonObject(6, "多个带回", "Radio");
            list.Add(obj);
            obj = new SimpleSuggetJsonObject(7, "从属带回", "Radio");
            list.Add(obj);
            var source = from c in list select c;
            return Json(source, JsonRequestBehavior.AllowGet);
        }
    }
}