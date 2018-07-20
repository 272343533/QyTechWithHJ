using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SunMvcExpress.Dao;

namespace QyTech.Express.CodeCreate.IDataView.IDataEditType
{
    public class DataEditDecimal : IDataEditType
    {
        public override void Create(StreamWriter sw, bsFuncField bf, ISysFunConf Ifc)
        {
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<input id=\"" + Ifc.fc.TName + "_" + bf.FName + "\"  name=\"" + Ifc.fc.TName + "_" + bf.FName + "\" type=\"text \" size=\"" + bf.FormFWidth.ToString() + "\" " + ((Boolean)bf.FormEditable ? "class=\"required digital\"" : "disabled") + "/>");
        }
    }
}
