using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QyTech.Core.Common;
namespace QyTech.Express.CodeCreate.Action
{
    public class ztreeAdd : IControllerCreate
    {
        public override void Create()
        {
            sw.WriteLine("");
            sw.WriteLine("\t" + "\t" + "public JsonResult " + ActionType.ztreeAdd.ToString() + "(Guid "+Ifc.fc.TPK+", String Name, Guid P"+Ifc.fc.TPK+")");
            sw.WriteLine("\t" + "\t" + "{");
            sw.WriteLine("\t" + "\t" + "\t" + "JsonResult json = new JsonResult();");
            sw.WriteLine("\t" + "\t" + "\t" + Ifc.fc.TName+" pNode = EntityManager<"+ Ifc.fc.TName+">.GetByPk<"+ Ifc.fc.TName+">(\""+Ifc.fc.TPK+"\", P"+Ifc.fc.TPK+");");
            sw.WriteLine("\t" + "\t" + "\t"  + Ifc.fc.TName+ " obj = new  "+ Ifc.fc.TName+"();");

            sw.WriteLine("\t" + "\t" + "\t" + "try");
            sw.WriteLine("\t" + "\t" + "\t" + "{");
            sw.WriteLine("\t" + "\t" + "\t" + "    obj." + Ifc.fc.TPK + " =  " + Ifc.fc.TPK + ";");
            sw.WriteLine("\t" + "\t" + "\t" + "    obj." + Ifc.fc.NotNullField + " = Name;");
            sw.WriteLine("\t" + "\t" + "\t" + "    obj.P" + Ifc.fc.TPK + " = P" + Ifc.fc.TPK + ";");
            //如果包含bsO_Id或Cust_Id 就加进去，这里没有判断。
            //sw.WriteLine("\t" + "\t" + "\t" + "    bo.bsO_Id = pOrg.bsO_Id;");
            //sw.WriteLine("\t" + "\t" +  "\t" +"bo.DelStatus = false;");
            sw.WriteLine("\t" + "\t" + "\t" + "    EntityManager<" + Ifc.fc.TName + ">.Add<" + Ifc.fc.TName + ">(obj);");
            
            sw.WriteLine("\t" + "\t" + "\t" + "    json.Data = DwzReturn.SuccessAjaxJsonResult(\""+Ifc.fc.TName+"\");");
            sw.WriteLine("\t" + "\t" + "\t" + "}");
            sw.WriteLine("\t" + "\t" + "\t" + "catch (Exception ex)");
            sw.WriteLine("\t" + "\t" + "\t" + "{");
            sw.WriteLine("\t" + "\t" + "\t" + "    json.Data = DwzReturn.FailAjaxJsonResult(\"" + Ifc.fc.TName + "\");");
            sw.WriteLine("\t" + "\t" +  "\t" +"}");
            sw.WriteLine("\t" + "\t" +  "\t" +"return json;");
            sw.WriteLine("\t" + "\t" +  "}");
            
        }

    }
}
