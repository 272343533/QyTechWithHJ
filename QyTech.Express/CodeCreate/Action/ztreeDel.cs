using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QyTech.Core.Common;
namespace QyTech.Express.CodeCreate.Action
{
    public class ztreeDel : IControllerCreate
    {
        public override void Create()
        {
            sw.WriteLine("");
            sw.WriteLine("\t" + "\t" + "public ActionResult " + ActionType.ztreeDel.ToString() + "(Guid "+Ifc.fc.TPK+")");
            sw.WriteLine("\t" + "\t" + "{");

            sw.WriteLine("\t" + "\t" + "    JsonResult json = new JsonResult();");
            sw.WriteLine("\t" + "\t" + "    AjaxJsonResult rs = null;");

            sw.WriteLine("\t" + "\t" + "    var info = EntityManager<" + Ifc.fc.TName + ">.GetByPk<" + Ifc.fc.TName + ">(\"" + Ifc.fc.TPK + "\"," + Ifc.fc.TPK + ");");
            //}
            sw.WriteLine("\t" + "\t" + "    if (info != null)");
            sw.WriteLine("\t" + "\t" + "    {");
            sw.WriteLine("\t" + "\t" + "        string errMsg = EntityManager<" + Ifc.fc.TName + ">.Delete(info);");
            sw.WriteLine("\t" + "\t" + "        if (errMsg.Equals(\"\"))");
            sw.WriteLine("\t" + "\t" + "        {");
            sw.WriteLine("\t" + "\t" + "            rs = DwzReturn.SuccessAjaxJsonResultNotClosed(\"" + Ifc.fc.TName + "\");");
            sw.WriteLine("\t" + "\t" + "        }");
            sw.WriteLine("\t" + "\t" + "        else");
            sw.WriteLine("\t" + "\t" + "        {");
            sw.WriteLine("\t" + "\t" + "            rs = DwzReturn.FailAjaxJsonResult(errMsg);");
            sw.WriteLine("\t" + "\t" + "        }");
            sw.WriteLine("\t" + "\t" + "    }");
            sw.WriteLine("\t" + "\t" + "    json.Data = rs;");
            sw.WriteLine("\t" + "\t" + "    return json;");
            sw.WriteLine("\t" + "\t" + "}");
            sw.WriteLine("");

        }
    }
}
