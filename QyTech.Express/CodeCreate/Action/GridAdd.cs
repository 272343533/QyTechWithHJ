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
    public class GridAdd : IControllerCreate
    {
        public override void Create()
        {
            base.Create();
            sw.WriteLine("");
            string PFK_type = "Guid";
            foreach (bsFuncField ff in Ifc.dbFunfields)
            {
                if (ff.FName == Ifc.fc.PFK)
                {
                    PFK_type = ff.Type;
                    break;
                }
            }
            PFK_type = PreDefOperate.SqlTypeMap2CSType(PFK_type);
            if (Ifc.fc.PFK != null && Ifc.fc.PFK != "")
            {
                if (PFK_type == "Guid" || PFK_type == "string")
                {
                    sw.WriteLine("\t" + "\t" + "public ActionResult " + ActionType.GridAdd.ToString() + "(string " + Ifc.fc.PFK + ")");
                }
                else
                {
                    sw.WriteLine("\t" + "\t" + "public ActionResult " + ActionType.GridAdd.ToString() + "(" + PFK_type + " " + Ifc.fc.PFK + ")");

                }
            }
            else
                sw.WriteLine("\t" + "\t" + "public ActionResult " + ActionType.GridAdd.ToString() + "()");

            sw.WriteLine("\t" + "\t" + "{");
            sw.WriteLine("\t" + "\t" + "    JsonResult json = new JsonResult();");
            sw.WriteLine("\t" + "\t" + "    AjaxJsonResult rs = null;");
            sw.WriteLine("\t" + "\t" + "    " + Ifc.fc.TName + " obj = new  "+ Ifc.fc.TName+"();");

            string csType="";
            foreach (bsFuncField ff in Ifc.dbFunfields)
            {
                if (ff.FName == Ifc.fc.NotNullField)
                {
                    csType = PreDefOperate.SqlTypeMap2CSType(ff.Type);

                    if (csType == "int" || csType=="decimal"|| csType=="bigint")
                        sw.WriteLine("\t" + "\t" + "        obj." + ff.FName + " = 0;");
                    else if (csType=="datetime")
                        sw.WriteLine("\t" + "\t" + "        obj." + ff.FName + " = DateTime.Now;");
                    else
                        sw.WriteLine("\t" + "\t" + "        obj." + ff.FName + " = \"新增\";");
                }
                else if (ff.FName == Ifc.fc.TPK)
                {
                    if (ff.Type == "uniqueidentifier")
                        sw.WriteLine("\t" + "\t" + "        obj." + ff.FName + " =  Guid.NewGuid();");
                }
                else if (ff.FName == Ifc.fc.PFK)// 外键的处理
                {
                    if (ff.Type=="uniqueidentifier")
                     sw.WriteLine("\t" + "\t" + "        obj." + ff.FName + " = new Guid(" + ff.FName + ");");
                    else
                        sw.WriteLine("\t" + "\t" + "        obj." + ff.FName + " = " + ff.FName + ";");
                }
            }
            //sw.WriteLine("\t" + "\t" + "        obj.OperStatus = true;");


            sw.WriteLine("\t" + "\t" + "    string errMsg = EntityManager<" + Ifc.fc.TName + ">.Add<" + Ifc.fc.TName + ">(obj);");
            sw.WriteLine("\t" + "\t" + "    if (errMsg.Equals(\"\"))");
            sw.WriteLine("\t" + "\t" + "    {");
            sw.WriteLine("\t" + "\t" + "        rs = DwzReturn.SuccessAjaxJsonResultNotClosed(\"FuncOper\");");
            sw.WriteLine("\t" + "\t" + "    }");
            sw.WriteLine("\t" + "\t" + "    else");
            sw.WriteLine("\t" + "\t" + "    {");
            sw.WriteLine("\t" + "\t" + "        rs = DwzReturn.FailAjaxJsonResult(errMsg);");
            sw.WriteLine("\t" + "\t" + "    }");
            sw.WriteLine("\t" + "\t" + "    json.Data = rs;");
            sw.WriteLine("\t" + "\t" + "    json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;");
            sw.WriteLine("\t" + "\t" + "    return json;");
            sw.WriteLine("\t" + "\t" + "    }");
        }
    }
}
