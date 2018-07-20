using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SunMvcExpress.Dao;


namespace QyTech.Express.CodeCreate.IDataView.IDataEditType
{
    public class DataEditCombox : IDataEditType
    {
        public override void Create(StreamWriter sw, bsFuncField bf, ISysFunConf Ifc)
        {
            #region combox
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<select type=\"combox\" id=\"" + Ifc.fc.TName + "_" + bf.FName + "\"  name=\"" + Ifc.fc.TName + "_" + bf.FName + "\" " + ((Boolean)bf.FormEditable ? "class=\"required\"" : "disabled") + ">");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<option selected value=\"\">请选择------</option>");
            if (bf.LookupLink.IndexOf(';') > 0)
            {
                string[] strcom = bf.LookupLink.Split(new char[] { ',', ';' });
                for (int i = 0; i < strcom.Length / 2; i++)
                {
                    sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<option value=\"" + strcom[i * 2 + 0] + "\">" + strcom[i * 2 + 1] + "</option>");
                }
            }
            else
            {
                string[] strcom = bf.LookupLink.Split(new char[] { ',' });
                for (int i = 0; i < strcom.Length; i++)
                {
                    sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<option value=\"" + strcom[i] + "\">" + strcom[i] + "</option>");
                }
            }
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "</select>");
            #endregion
        }
    }
}
