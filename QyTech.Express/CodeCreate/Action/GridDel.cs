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
    public class GridDel:IControllerCreate
    {
        public override void Create()
        {
            base.Create();
            sw.WriteLine("");
            string PFK_type = "Guid";
            foreach (bsFuncField ff in Ifc.dbFunfields)
            {
                if (ff.FName == Ifc.fc.TPK)
                {
                    PFK_type = ff.Type;
                    break;
                }
            }
            string LangPFK_type = PreDefOperate.SqlTypeMap2CSType(PFK_type);
            //if (PFK_type == "uniqueidentifier")
            //{
            //    sw.WriteLine("\t" + "\t" + "public JsonResult " + ActionType.GridDel + "(string id)");
            //}
            //else
            //{
                sw.WriteLine("\t" + "\t" + "public JsonResult " + ActionType.GridDel + "(" + LangPFK_type + " id)");
          
            //}
            sw.WriteLine("\t" + "\t" + "{");
            sw.WriteLine("\t" + "\t" + "    JsonResult json = new JsonResult();");
            sw.WriteLine("\t" + "\t" + "    AjaxJsonResult rs = null;");
             //if (PFK_type == "uniqueidentifier")
            //{
            //    sw.WriteLine("\t" + "\t" + "        var info = db." + Ifc.fc.TName + ".SingleOrDefault(o => o." + Ifc.fc.TPK + " ==  Guid.Parse(id));");
            //}
            //else
            //{
            sw.WriteLine("\t" + "\t" + "    var info = EntityManager<"+Ifc.fc.TName + ">.GetByPk<"+Ifc.fc.TName+">(\"" + Ifc.fc.TPK + "\",id);");
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
        }
    }
}
