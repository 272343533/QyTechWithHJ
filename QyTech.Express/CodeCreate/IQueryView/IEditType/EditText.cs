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
    public class EditText : IQueryViewCreate
    {
        public override void Create(bsSysFuncQuery fq, StreamWriter sw)
        {
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + fq.FFDesp.Trim() + "：<input type=\"text\" id=\"query_" + fq.FFName + "\" name=\"query_" + fq.FFName + "\" />");
        }
    }
}
