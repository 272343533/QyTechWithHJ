using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QyTech.Express.CodeCreate;
using SunMvcExpress.Dao;
using QyTech.Core.Common;

namespace QyTech.Express.CodeCreate.IQueryView
{
    public class Right : IQueryViewCreate
    {
        public override void Create(bsSysFuncQuery fq)
        {
            if (fq.NeedRange == null || !(bool)(fq.NeedRange))
            {
                sw.WriteLine("\t" + "\t" + "<td>");
                IQueryViewCreate simpCond = FacQueryView.Create(fq.QueryType);
                simpCond.Create(fq);
                sw.WriteLine("\t" + "\t" + "</td>");
            }
            else
            {
                sw.WriteLine("\t" + "\t" + "<td>");
                sw.WriteLine("\t" + "\t" + fq.FFDesp + "：<input type=\"text\" id=\"query_" + fq.FFName + "_Start\" name=\"query_" + fq.FFName + "_Start\" />");
                sw.WriteLine("\t" + "\t" + "</td>");
                sw.WriteLine("\t" + "\t" + "<td>");
                sw.WriteLine("\t" + "\t" + fq.FFDesp + "：<input type=\"text\" id=\"query_" + fq.FFName + "_End\" name=\"query_" + fq.FFName + "_End\" />");
                sw.WriteLine("\t" + "\t" + "</td>");
 
            }
        }
    }
}
