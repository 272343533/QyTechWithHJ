using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QyTech.Core.Common;
namespace QyTech.Express.CodeCreate.Action
{
    public class ztreeEdit : IControllerCreate
    {
        public override void Create()
        {
            sw.WriteLine("");
            sw.WriteLine("\t" + "\t" + "public JsonResult " + ActionType.ztreeEdit.ToString() + "(Guid Id, String Name)");
            sw.WriteLine("\t" + "\t" + "{");

            sw.WriteLine("\t" + "\t" + "    var info = EntityManager<" + Ifc.fc.TName + ">.GetByPk<" + Ifc.fc.TName + ">(\"" + Ifc.fc.TPK + "\", Id);");
            sw.WriteLine("\t" + "\t" + "    info."+Ifc.fc.NotNullField+" = Name;");
            sw.WriteLine("\t" + "\t" + "    EntityManager<" + Ifc.fc.TName + ">.Modify<" + Ifc.fc.TName + ">(info);");
            sw.WriteLine("\t" + "\t" + "    return null;");

            sw.WriteLine("\t" + "\t" + "}");
            sw.WriteLine("");
        }
    }
}
