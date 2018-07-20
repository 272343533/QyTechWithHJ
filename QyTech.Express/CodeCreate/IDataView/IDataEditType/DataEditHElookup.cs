using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SunMvcExpress.Dao;


namespace QyTech.Express.CodeCreate.IDataView.IDataEditType
{
    public class DataEditHElookup : IDataEditType
    {
        public override void Create(StreamWriter sw, bsFuncField bf, ISysFunConf Ifc)
        {
            if (bf.LookupLink != null)
            {
                string[] lookupT2f = bf.LookupLink.Split(new char[] { '.' });
                sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<input class=\"readonly\" id=\"" + Ifc.fc.TName + "_" + bf.FName + "\"  name=\"" + Ifc.fc.TName + "_" + bf.FName + "\" bringBackName=\"" + lookupT2f[0] + "." + lookupT2f[1] + "\" type=\"hidden\"  size=\"" + bf.FormFWidth.ToString() + "\"/>");
            }
        }
    }
}
