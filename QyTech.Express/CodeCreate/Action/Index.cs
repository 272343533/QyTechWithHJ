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
    public class Index:IControllerCreate
    {
        public override void Create()
        {
            sw.WriteLine("\t" + "\t" + "public ActionResult " + ActionType.Index.ToString() + "(" + GetIndexParameters() + ")");
            sw.WriteLine("\t" + "\t" + "{");

            sw.WriteLine("\t" + "\t" + "    return View(); ");// + ActionType.Index.ToString() + "Data(" + GetIndexParametersNoType() + ");");
            sw.WriteLine("\t" + "\t" + "}");

            sw.WriteLine("\t" + "\t" + "public ActionResult " + ActionType.Index.ToString() + "Data(" + GetIndexParameters() + ")");
            sw.WriteLine("\t" + "\t" + "{");
            sw.WriteLine("\t" + "\t" + "\t" + "int totalCount;");
             sw.WriteLine("\t" + "\t" + "\t" + "if (orderbys == null || orderbys.Equals(\"\"))");
             sw.WriteLine("\t" + "\t" + "\t" + "{");
             sw.WriteLine("\t" + "\t" + "\t" + "    orderbys = \""+Ifc.fc.Orderbys+"\";");
             sw.WriteLine("\t" + "\t" + "\t" + "}");
            sw.WriteLine("\t" + "\t" + "\t" + "List<" + Ifc.fc.TName + "> objs=new List<" + Ifc.fc.TName + ">();");

            SetQueryConditions();

            sw.WriteLine("\t" + "\t" + "\t" + "objs = EntityManager<" + Ifc.fc.TName + ">.Paging<" + Ifc.fc.TName + ">(Login_User_ID,conditions,orderbys, pageNum, numPerPage, out totalCount);");

            sw.WriteLine("\t" + "\t" + "\t" + "ViewBag.numPerPage = numPerPage;");
            sw.WriteLine("\t" + "\t" + "\t" + "ViewBag.pageNum = pageNum;");
            sw.WriteLine("\t" + "\t" + "\t" + "ViewBag.totalCount = totalCount;");

            sw.WriteLine("\t" + "\t" + "\t" + "return View(objs);");
            sw.WriteLine("\t" + "\t" + "}");
        }

       
    }
}
