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
    public class Del:IControllerCreate
    {

        public override void Create()
        {
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("");

            sw.WriteLine("\t" + "\t" + "public JsonResult " + ActionType.Del + "(Guid id)");
            sw.WriteLine("\t" + "\t" + "{");
            sw.WriteLine("\t" + "\t" + "\t" + "JsonResult json = new JsonResult();");
            sw.WriteLine("\t" + "\t" + "\t" + "AjaxJsonResult rs = null;");

            sw.WriteLine("\t" + "\t" + "    var info = EntityManager<" + Ifc.fc.TName + ">.GetByPk<" + Ifc.fc.TName + ">(\"" + Ifc.fc.TPK + "\",id);");
            if (Ifc.fc.SaveAuditer != null && (bool)Ifc.fc.SaveAuditer)
            {
                sw.WriteLine("\t" + "\t" + "\t" + "    if (info.FLowStatus != null && (FlowStatus)Enum.Parse(typeof(FlowStatus), info.FLowStatus.ToLower()) > FlowStatus.已保存)");
                sw.WriteLine("\t" + "\t" + "\t" + "    {");
                sw.WriteLine("\t" + "\t" + "\t" + "        rs = new AjaxJsonResult");
                sw.WriteLine("\t" + "\t" + "\t" + "        {");
                sw.WriteLine("\t" + "\t" + "\t" + "            statusCode = \"300\",");
                sw.WriteLine("\t" + "\t" + "\t" + "            message = \"已审核，不可删除\",");
                sw.WriteLine("\t" + "\t" + "\t" + "            navTabId = \"\",");
                sw.WriteLine("\t" + "\t" + "\t" + "            rel = \"\",");
                sw.WriteLine("\t" + "\t" + "\t" + "            callbackType = \"closeCurrent\",");
                sw.WriteLine("\t" + "\t" + "\t" + "            forwardUrl = \"\"");
                sw.WriteLine("\t" + "\t" + "\t" + "        };");
                sw.WriteLine("\t" + "\t" + "\t" + "    }");
                sw.WriteLine("\t" + "\t" + "\t" + "    else");
                sw.WriteLine("\t" + "\t" + "\t" + "    {");
            }
            sw.WriteLine("\t" + "\t" + "\t" + "    if (info != null)");
            sw.WriteLine("\t" + "\t" + "\t" + "    {");
            if (HaveSpecialFields(SpeicalFields.DelStatus))
            {
                sw.WriteLine("\t" + "\t" + "\t" + "\t" + "    info.datastatus = true;");
                sw.WriteLine("\t" + "\t" + "\t" + "\t" + "    if (db.SaveChanges()>0)");
            }
            else //真正删除
            {
                sw.WriteLine("\t" + "\t" + "\t" + "\t" + "    errMsg=EntityManager<" + Ifc.fc.TName + ">.Delete(db,info);");
                sw.WriteLine("\t" + "\t" + "\t" + "\t" + "    if (errMsg==\"\")");

            }
            sw.WriteLine("\t" + "\t" + "\t" + "        {");
            sw.WriteLine("\t" + "\t" + "\t" + "             rs = DwzReturn.SuccessAjaxJsonResultNotClosed( \"editpage\");");
            sw.WriteLine("\t" + "\t" + "\t" + "        }");
            sw.WriteLine("\t" + "\t" + "\t" + "        else");
            sw.WriteLine("\t" + "\t" + "\t" + "        {");
            sw.WriteLine("\t" + "\t" + "\t" + "            rs = DwzReturn.FailAjaxJsonResult();");
            sw.WriteLine("\t" + "\t" + "\t" + "        }");
            sw.WriteLine("\t" + "\t" + "\t" + "    }");

             if (Ifc.fc.SaveAuditer != null && (bool)Ifc.fc.SaveAuditer)
            {
                sw.WriteLine("\t" + "\t" + "\t" +"\t" + "}");
            }
            sw.WriteLine("\t" + "\t" + "\t" + "json.Data = rs;");
            sw.WriteLine("\t" + "\t" + "\t" + "return json;");
            sw.WriteLine("\t" + "\t" + "}");
        }

    }
}
