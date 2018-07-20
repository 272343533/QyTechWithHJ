using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SunMvcExpress.Dao;

namespace QyTech.Express.CodeCreate.IDataView.IDataEditType
{
    public class DataEditTotal : IDataEditType
    {
        public override void Create(StreamWriter sw, bsFuncField bf, ISysFunConf Ifc)
        {
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<input id=\"" + Ifc.fc.TName + "_" + bf.FName + "\"  name=\"" + Ifc.fc.TName + "_" + bf.FName + "\" type=\"required digital\" size=\"" + bf.FormFWidth.ToString() + "\" " + ((Boolean)bf.FormEditable ? "class=\"required\"" : "disabled") + "/>");

        }
    }
}
