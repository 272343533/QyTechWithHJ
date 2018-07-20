using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QyTech.Express.CodeCreate;
using SunMvcExpress.Dao;
using QyTech.Core.Common;

namespace QyTech.Express.CodeCreate.Action
{
    public class Edit:IControllerCreate
    {
        public override void Create()
        {
            sw.WriteLine("\t" + "\t" + "public JsonResult " + ActionType.Edit.ToString() + "(FormCollection fc)");
            sw.WriteLine("\t" + "\t" + "{");
            sw.WriteLine("\t" + "\t" + "\t" + "JsonResult json = new JsonResult();");
            sw.WriteLine("\t" + "\t" + "\t" + "AjaxJsonResult rs = null;");

           
            //sw.WriteLine("\t" + "\t" + "\t" + "errMsg=EntityManager<" + bsSysfunc.TName + ">.Modify(objdb);");
            sw.WriteLine("\t" + "\t" + "\t" + "errMsg = SaveForAddOrEdit(fc, AddorModify.Modify);");
            sw.WriteLine("\t" + "\t" + "\t" + "if (errMsg==\"\")");
            sw.WriteLine("\t" + "\t" + "\t" + "{");
            sw.WriteLine("\t" + "\t" + "\t" + "      rs = DwzReturn.SuccessAjaxJsonResult( \"editpage\");");

            sw.WriteLine("\t" + "\t" + "\t" + " }");
            sw.WriteLine("\t" + "\t" + "\t" + " else");
            sw.WriteLine("\t" + "\t" + "\t" + " {");
            sw.WriteLine("\t" + "\t" + "\t" + "      rs = DwzReturn.FailAjaxJsonResult();");

            sw.WriteLine("\t" + "\t" + "\t" + "}");
            sw.WriteLine("\t" + "\t" + "\t" + "json.Data = rs;");


            sw.WriteLine("\t" + "\t" + "\t" + "return json;");
            sw.WriteLine("\t" + "\t" + "}");

        }

    }
}
