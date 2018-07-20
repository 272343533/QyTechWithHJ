using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QyTech.Express.CodeCreate;
using SunMvcExpress.Dao;
using QyTech.Core.Common;
using System.IO;
namespace QyTech.Express.CodeCreate.IQueryView
{
    public class Top : IQueryViewCreate
    {
        public override void Create(bsSysFuncQuery fq,StreamWriter sw)
        {
            if (fq.NeedRange == null || !(bool)(fq.NeedRange))
            {
                sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<td>");
                IQueryViewCreate simpCond = FacQueryView.Create(fq.QueryType);
                simpCond.Create(fq,sw);
                sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "</td>");
            }
            else
            {
                sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<td>");
                sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + fq.FFDesp + "：<input type=\"text\" id=\"query_" + fq.FFName + "_Start\" name=\"query_" + fq.FFName + "_Start\"  class=\"date\" dateFmt=\"yyyy-MM-dd\"  />");
                sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "</td>");
                sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<td>");
                sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + fq.FFDesp + "：<input type=\"text\" id=\"query_" + fq.FFName + "_End\" name=\"query_" + fq.FFName + "_End\"  class=\"date\" dateFmt=\"yyyy-MM-dd\" />");
                sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "</td>");
 
            }
        }
    }
}
