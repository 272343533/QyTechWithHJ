using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using QyTech.Core.Common;
using QyTech.Core.Helpers;
using QyTech.Core.BLL;

namespace SunMvcExpress.Controllers
{
    public class FileDownController : Controller
    {
        //
        // GET: /FileDown/

        public ActionResult Download(string fun,string filename)
        {
            var path = Server.MapPath("~/Uploads/"+fun+"/"+filename);
            var name = Path.GetFileName(path);
            return File(path, "1", Url.Encode(name));
        }
    }
}
