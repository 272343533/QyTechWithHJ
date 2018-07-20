using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QyTech.Express.CodeCreate;
using SunMvcExpress.Dao;
using QyTech.Core.Common;
using System.IO;

namespace QyTech.Express.CodeCreate.IQueryView.IEditType
{
     public class EditRadio : IQueryViewCreate
    {
        public override void Create(bsSysFuncQuery fq, StreamWriter sw)
        {
            string[] strcom = fq.Url.Split(new char[] { ',', ';', ' ' });
            for (int i = 0; i < strcom.Length; i += 2)
            {
                sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<input type=\"radio\" name=\"" + Ifc.fc.TName + "_" + fq.FFDesp.Trim() + "\" value=\"" + strcom[i] + "\"/>" + strcom[i + 1]);
            }
         }
    }
}
