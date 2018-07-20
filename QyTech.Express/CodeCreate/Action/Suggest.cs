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
    public class Suggest:IControllerCreate
    {
        public override void Create(bsSysFunc bsSysfunc)
        {
            sw.WriteLine("\t" + "\t" + "public ActionResult " + ActionType.Lookup.ToString() + "(" + GetIndexParameters() + ")");
            sw.WriteLine("\t" + "\t" + "{");
            sw.WriteLine("\t" + "\t" + "\t" + "int totalCount;");
            sw.WriteLine("\t" + "\t" + "\t" + "orderbys = \"" + bsSysfunc.Orderbys + "\";");

            SetQueryConditions();

            sw.WriteLine("\t" + "\t" + "\t" + "List<" + bsSysfunc.TName + "> objs=new List<" + bsSysfunc.TName + ">();");
            sw.WriteLine("\t" + "\t" + "\t" + "objs = EntityManager<" + bsSysfunc.TName + ">.Paging<" + bsSysfunc.TName + ">(Login_User_ID,conditions,orderbys,pageNum, numPerPage, out totalCount);");

            sw.WriteLine("\t" + "\t" + "\t" + "ViewBag.numPerPage = numPerPage;");
            sw.WriteLine("\t" + "\t" + "\t" + "ViewBag.pageNum = pageNum;");
            sw.WriteLine("\t" + "\t" + "\t" + "ViewBag.totalCount = totalCount;");

            foreach (bsSysFuncQuery bf in Ifc.FuncQuerys)
            {
                if (bf == null)
                    continue;
                //日期型数据的处理。
                ////if (bf.Type.ToLower() == FieldType.Varchar.ToString().ToLower())
                ////{
                ////    sw.WriteLine("\t" + "\t" + "\t" + "ViewBag." + bf.Name + "=query_" + bf.Name + ";");
                ////}
                ////else if (bf.Type.ToLower() == FieldType.Datetime.ToString().ToLower() && bf.Type.ToLower() == FieldType.Date.ToString().ToLower())
                ////{
                ////    if (!RangeFields.Contains(bf.Name))
                ////        sw.WriteLine("\t" + "\t" + "\t" + "ViewBag." + bf.Name + "=query_" + bf.Name + ";");
                ////    else
                ////    {
                ////        sw.WriteLine("\t" + "\t" + "\t" + "ViewBag." + bf.Name + "_start=query_" + bf.Name + "_start;");
                ////        sw.WriteLine("\t" + "\t" + "\t" + "ViewBag." + bf.Name + "_end=query_" + bf.Name + "_end;");
                ////    }
                ////}
            }
            sw.WriteLine("\t" + "\t" + "\t" + "return View(objs);");
            sw.WriteLine("\t" + "\t" + "}");
        }
    }
}
