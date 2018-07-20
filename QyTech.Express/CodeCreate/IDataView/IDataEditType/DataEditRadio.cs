using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SunMvcExpress.Dao;


namespace QyTech.Express.CodeCreate.IDataView.IDataEditType
{
     public class DataEditRadio :IDataEditType
    {
        public override void Create(StreamWriter sw, bsFuncField bf,ISysFunConf Ifc)
        {
            #region combox
            
            string[] strcom;
            if (bf.LookupLink.Substring(0,1)=="{")
                strcom=bf.LookupLink.Substring(1,bf.LookupLink.Length-2).Split(new char[] { ',', ';', ' ' });
            else
                strcom= bf.LookupLink.Split(new char[] { ',', ';', ' ' });
            for (int i = 0; i < strcom.Length; i += 2)
            {
                sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<input type=\"radio\" name=\"" + Ifc.fc.TName + "_" + bf.FName + "\" value=\"" + strcom[i] + "\"/>" + strcom[i + 1]);
            }
            #endregion
        }
    }

}
