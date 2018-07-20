using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SunMvcExpress.Dao;
namespace QyTech.Express.CodeCreate.IDataView.IDataEditType
{
    public class DataEditCheckbox :IDataEditType
    {
        public override void Create(StreamWriter sw, bsFuncField bf, ISysFunConf Ifc)
        {
            if (bf.LookupLink.Length > 0)
            {
                if (bf.LookupLink.Substring(0, 1) == "{")
                {
                    string[] splits = bf.LookupLink.Split(new char[] { '{', '}', ';' ,','});
                    foreach (string s1 in splits)
                    {
                        if (s1.Trim() == "")
                            continue;
                        sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<input type=\"checkbox\" id=\"" + Ifc.fc.TName + "_" + bf.FName + "\"  name=\"" + Ifc.fc.TName + "_" + bf.FName + "\" " + ((Boolean)bf.FormEditable ? "class=\"required\"" : "disabled") + ">"+s1);
                    }
                }
                }
               
        }
    }
}
