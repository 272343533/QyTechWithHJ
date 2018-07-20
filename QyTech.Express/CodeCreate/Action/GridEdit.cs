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
    public class GridEdit:IControllerCreate
    {
        public override void Create()
        {
            sw.WriteLine("\t" + "\t" + "public JsonResult " + ActionType.GridEdit.ToString() + "(FormCollection fc)");
            sw.WriteLine("\t" + "\t" + "{");
            sw.WriteLine("\t" + "\t" + "    JsonResult json = new JsonResult();");
            sw.WriteLine("\t" + "\t" + "    AjaxJsonResult rs = null;");
            sw.WriteLine("\t" + "\t" + "    if (SaveNoDel(fc))");
            sw.WriteLine("\t" + "\t" + "    {");
            sw.WriteLine("\t" + "\t" + "        rs = DwzReturn.SuccessAjaxJsonResultNotClosed(\"" + Ifc.fc.FunContr + "\");");
            sw.WriteLine("\t" + "\t" + "    }");
            sw.WriteLine("\t" + "\t" + "    else");
            sw.WriteLine("\t" + "\t" + "    {");
            sw.WriteLine("\t" + "\t" + "        rs = DwzReturn.FailAjaxJsonResult();");
            sw.WriteLine("\t" + "\t" + "    }");
            sw.WriteLine("\t" + "\t" + "    json.Data = rs;");
            sw.WriteLine("\t" + "\t" + "    return json;");
            sw.WriteLine("\t" + "\t" + "}");


            sw.WriteLine("\t" + "\t" + "private static bool SaveNoDel(FormCollection fc)");
            sw.WriteLine("\t" + "\t" + "{");
            sw.WriteLine("\t" + "\t" + "    bool saveflag = true;");
            sw.WriteLine("\t" + "\t" + "    try");
            sw.WriteLine("\t" + "\t" + "    {");
            string TPK_type = "Guid";
            foreach (bsFuncField ff in Ifc.dbFunfields)
            {
                if (ff.FName == Ifc.fc.TPK)
                {
                    TPK_type = ff.Type;
                    break;
                }
            }
            string para_TPK_type = PreDefOperate.SqlTypeMap2CSType(TPK_type);
            sw.WriteLine("\t" + "\t" + "        if (fc.AllKeys[fc.Count - 1].Substring(0, 4) == \"item\")");
            sw.WriteLine("\t" + "\t" + "        {");
            sw.WriteLine("\t" + "\t" + "            for (int i = 0; i < Convert.ToInt32(fc.AllKeys[fc.Count - 1].Substring(6, fc.AllKeys[fc.Count - 1].IndexOf('.') - 7)) + 1; i++)");
            sw.WriteLine("\t" + "\t" + "            {");
            if (TPK_type == "uniqueidentifier")
                sw.WriteLine("\t" + "\t" + "              Guid  tpk = Guid.Parse( fc[\"items[\" + i.ToString() + \"]." + Ifc.fc.TPK + "\"].ToString());");
            else
                sw.WriteLine("\t" + "\t" + "              " +TPK_type + "  tpk = Convert.ToInt32(fc[\"items[\" + i.ToString() + \"]." + Ifc.fc.TPK + "\"]);");
            sw.WriteLine("\t" + "\t" + "                " + Ifc.fc.TName + " cdobj = EntityManager<" + Ifc.fc.TName + ">.GetByPk<" + Ifc.fc.TName + ">(\"" + Ifc.fc.TPK + "\", tpk);");
            
            foreach (bsFuncField ff in Ifc.dbFunfields)
            {
                if (ff.FName == Ifc.fc.TPK)
                    continue;
                sw.WriteLine("\t" + "\t" + "               if (fc[\"items[\" + i.ToString() + \"]." + ff.FName + "\"] != null && !fc[\"items[\" + i.ToString() + \"]." + ff.FName + "\"].Equals(\"\"))");

                if (ff.Type == "uniqueidentifier")
                {
                    sw.WriteLine("\t" + "\t" + "\t" + "                cdobj." + ff.FName + " = Guid.Parse(fc[\"items[\" + i.ToString() + \"]." + ff.FName + "\"].ToString());");
                }
                else if (ff.Type == "int")
                {
                    sw.WriteLine("\t" + "\t" + "\t" + "                cdobj." + ff.FName + " =Convert.ToInt32(fc[\"items[\" + i.ToString() + \"]." + ff.FName + "\"]);");
                }
                else if (ff.Type == "decimal")
                {
                    sw.WriteLine("\t" + "\t" + "\t" + "                cdobj." + ff.FName + " =Convert.ToDecimal(fc[\"items[\" + i.ToString() + \"]." + ff.FName + "\"]);");
                }
                else if (ff.Type == "varchar")
                {
                    sw.WriteLine("\t" + "\t" + "\t" + "                cdobj." + ff.FName + " = fc[\"items[\" + i.ToString() + \"]." + ff.FName + "\"].ToString();");
                }
                else if (ff.Type == "bit")
                {
                    sw.WriteLine("\t" + "\t" + "\t" + "                cdobj." + ff.FName + "  = fc[\"items[\" + i.ToString() + \"]." + ff.FName + "\"] == null ? false : true;");
                }
                else
                    sw.WriteLine("\t" + "\t" + "\t" + "                cdobj." + ff.FName + " = fc[\"items[\" + i.ToString() + \"]." + ff.FName + "\"].ToString();");
            }
            sw.WriteLine("\t" + "\t" + "               if (EntityManager<" + Ifc.fc.TName + ">.Modify(cdobj) != \"\")");
            sw.WriteLine("\t" + "\t" + "               {");
            sw.WriteLine("\t" + "\t" + "                   saveflag = false; break;");
            sw.WriteLine("\t" + "\t" + "               }");
            sw.WriteLine("\t" + "\t" + "           }");
            sw.WriteLine("\t" + "\t" + "       }");
            sw.WriteLine("\t" + "\t" + "    }");
            sw.WriteLine("\t" + "\t" + "    catch (Exception ex)");
            sw.WriteLine("\t" + "\t" + "    {");
            sw.WriteLine("\t" + "\t" + "        saveflag = false;");
            sw.WriteLine("\t" + "\t" + "        //throw new Exception(ex.Message);");
            sw.WriteLine("\t" + "\t" + "    }");
            sw.WriteLine("\t" + "\t" + "    return saveflag;");
            sw.WriteLine("\t" + "\t" + "}");

        }

    }
}
