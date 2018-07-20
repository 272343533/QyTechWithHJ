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
    public class Add:IControllerCreate
    {
        /// <summary>
        /// 单表的add
        /// </summary>
        public override void Create()
        {
            sw.WriteLine("");
            //把richTextBox1中的内容写入文件 
            sw.WriteLine("\t" + "\t" + "public ActionResult " + ActionType.Add.ToString() + "(Guid? id, FormCollection fc,string BrowseFlag=\"false\")");
            sw.WriteLine("\t" + "\t" + "{");
            sw.WriteLine("\t" + "\t" + "\t" + "JsonResult json = new JsonResult();");
            sw.WriteLine("\t" + "\t" + "\t" + "AjaxJsonResult rs = null;");
            sw.WriteLine("\t" + "\t" + "\t" + "ViewBag.BrowseFlag = BrowseFlag;");
            sw.WriteLine("\t" + "\t" + "\t" + "ViewBag.Operater=Login_User_Name;");
            sw.WriteLine("\t" + "\t" + "\t" + "if (id == null && fc[\"" + Ifc.fc.TName + "_" + Ifc.fc.TPK + "\"] == null && fc[\"" + Ifc.fc.TName + "_" + Ifc.fc.NotNullField + "\"] != null && !fc[\"" + Ifc.fc.TName + "_" + Ifc.fc.NotNullField + "\"].Equals(\"\") )");
            sw.WriteLine("\t" + "\t" + "\t" + "{");

            //sw.WriteLine("\t" + "\t" + "\t" + "    errMsg=EntityManager<" + bsSysfunc.TName + ">.Add(obj);");
            sw.WriteLine("\t" + "\t" + "\t" + "    errMsg = SaveForAddOrEdit(fc, AddorModify.Add);");
            sw.WriteLine("\t" + "\t" + "\t" + "    if (errMsg==\"\")");
            sw.WriteLine("\t" + "\t" + "\t" + "    {");

            sw.WriteLine("\t" + "\t" + "\t" + "         rs = DwzReturn.SuccessAjaxJsonResult( \"editpage\");");

            sw.WriteLine("\t" + "\t" + "\t" + "    }");
            sw.WriteLine("\t" + "\t" + "\t" + "    else");
            sw.WriteLine("\t" + "\t" + "\t" + "    {");
            sw.WriteLine("\t" + "\t" + "\t" + "         rs = DwzReturn.FailAjaxJsonResult();");

            sw.WriteLine("\t" + "\t" + "\t" + "    }");
            sw.WriteLine("\t" + "\t" + "\t" + "    json.Data = rs;");

            sw.WriteLine("\t" + "\t" + "\t" + "    return json;");
            sw.WriteLine("\t" + "\t" + "\t" + "}");
            sw.WriteLine("\t" + "\t" + "\t" + "else");
            sw.WriteLine("\t" + "\t" + "\t" + "{");
            sw.WriteLine("\t" + "\t" + "\t" + "    if (id != null)");
            sw.WriteLine("\t" + "\t" + "\t" + "    {");
            sw.WriteLine("\t" + "\t" + "\t" + "        var info = EntityManager<." + GetWordss(Ifc.fc.TName) + ">.GetByPk<bsEmployee>(\"" + Ifc.fc.TPK + "\", id);");
            sw.WriteLine("\t" + "\t" + "\t" + "        return View(info);");
            sw.WriteLine("\t" + "\t" + "\t" + "    }");
            sw.WriteLine("\t" + "\t" + "\t" + "    return View();");
            sw.WriteLine("\t" + "\t" + "\t" + "}");
            sw.WriteLine("\t" + "\t" + "}");
        }
    }
}
