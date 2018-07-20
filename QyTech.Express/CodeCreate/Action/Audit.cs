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
    public class Audit:IControllerCreate
    {
        public virtual void CreateAudit()
        {
            sw.WriteLine("         public JsonResult " + ActionType.Audit.ToString() + "(Guid id)");
            sw.WriteLine("         {");
            sw.WriteLine("\t" + "         JsonResult json = new JsonResult();");
            sw.WriteLine("\t" + "           " + Ifc.fc.TName + " objdb= EntityManager<" + Ifc.fc.TName + ">.GetByPk<" + Ifc.fc.TName + ">(\"" + Ifc.fc.TPK + "\",id); ");
            //sw.WriteLine("\t" + "          var objdb = db." + GetWordss(bsSysfunc.TName) + ".SingleOrDefault(o => o."+bsSysfunc.TPK+" == id);");
            sw.WriteLine("\t" + "           if (objdb != null)");
            sw.WriteLine("\t" + "           {");
            sw.WriteLine("\t" + "               AjaxJsonResult rs = null;");
            sw.WriteLine("\t" + "               if (objdb.FLowStatus != null && (FlowStatus)Enum.Parse(typeof(FlowStatus), objdb.FLowStatus.ToLower()) > FlowStatus.已保存)");
            sw.WriteLine("\t" + "               {");
            sw.WriteLine("\t" + "                   rs = new AjaxJsonResult");
            sw.WriteLine("\t" + "                   {");
            sw.WriteLine("\t" + "                       statusCode = \"300\",");
            sw.WriteLine("\t" + "                       message = \"已经审核，不需再审\",");
            sw.WriteLine("\t" + "                       navTabId = \"\",");
            sw.WriteLine("\t" + "                       rel = \"\",");
            sw.WriteLine("\t" + "                       callbackType = \"\",");
            sw.WriteLine("\t" + "                       forwardUrl = \"\"");
            sw.WriteLine("\t" + "                   };");
            sw.WriteLine("\t" + "               }");
            sw.WriteLine("\t" + "               else");
            sw.WriteLine("\t" + "               {");
            sw.WriteLine("\t" + "                   objdb.FLowStatus = FlowStatus.已审核.ToString();");
            sw.WriteLine("\t" + "                   objdb.AuditDT = DateTime.Now;");
            sw.WriteLine("\t" + "                   objdb.Auditer = Login_User_Name;");
            sw.WriteLine("\t" + "                    errMsg = EntityManager<Receipt>.Modify<Receipt>(objdb);");
            sw.WriteLine("\t" + "                    if (errMsg==\"\")");
            sw.WriteLine("\t" + "                   {");
            sw.WriteLine("\t" + "                       rs = DwzReturn.SuccessAjaxJsonResult( \"editpage\");");
            sw.WriteLine("\t" + "                    }");
            sw.WriteLine("\t" + "                    else");
            sw.WriteLine("\t" + "                   {");
            sw.WriteLine("\t" + "                        rs = DwzReturn.FailAjaxJsonResult(errMsg);");
            sw.WriteLine("\t" + "                   }");
            sw.WriteLine("\t" + "               }");
            sw.WriteLine("\t" + "               json.Data = rs;");
            sw.WriteLine("\t" + "           }");
            sw.WriteLine("\t" + "        return json;");
            sw.WriteLine("        }");
        }
    }
}
