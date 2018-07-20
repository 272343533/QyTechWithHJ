using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SunMvcExpress.BLL;
using SunMvcExpress.Core.BLL;
using SunMvcExpress.Core.Common;
using SunMvcExpress.Core.Helpers;
using SunMvcExpress.Core.Models;
using SunMvcExpress.Dao;
using SunMvcExpress.Core;

using QyTech.Core.Common;
using QyTech.Core.Helpers;
using QyTech.Core.BLL;

namespace SunMvcExpress.Controllers
{
    public class FileUpJsonResult
    {
        public String id { set; get; }
        public String fileName { set; get; }
        public String attachmentPath { set; get; }
        public String attachmentSize { set; get; }
    }
    public class FileUpController : WSExpressController
    {
        
        //
        // GET: /FileUp/

        public JsonResult db_attachmentBrightBack(string DirName)
        {
            JsonResult json = new JsonResult();
            FileUpJsonResult fujs = null;

            HttpPostedFileBase file =Request.Files["file"];
            if (file != null)
            {
                try
                {
                    string newfilename = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(file.FileName);
                    string filePath = Path.Combine(HttpContext.Server.MapPath("../Uploads/" + DirName.Trim()), newfilename);
                    //需要重新命名，在文件名后面加上日期时间
                    file.SaveAs(filePath);

                    fujs = new FileUpJsonResult
                        {
                            id = "1000",
                            fileName = newfilename,
                            attachmentPath = filePath,
                            attachmentSize = "1024"
                        };
                    json.Data = fujs;
                }
                catch (Exception ex)
                {
                    json.Data = new FileUpJsonResult();
                    ExceptionMessage.Parse(ex);
                }
            }
            return json;
         
        }
        public ActionResult db_attachmentLookup(string DirName,string filter)
        {
            ViewBag.DirName = DirName;
            ViewBag.Filter = filter;
            return View();
        }
    }
}
