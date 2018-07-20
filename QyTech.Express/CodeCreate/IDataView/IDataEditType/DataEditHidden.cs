using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SunMvcExpress.Dao;


namespace QyTech.Express.CodeCreate.IDataView.IDataEditType
{
    public  class DataEditHidden : IDataEditType
    {
        public override void Create(StreamWriter sw, bsFuncField bf, ISysFunConf Ifc)
        {
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<input type=\"hidden\" id=\"" + Ifc.fc.TName + "_" + bf.FName + "\"  name=\"" + Ifc.fc.TName + "_" + bf.FName + "\" size=\"" + bf.FormFWidth.ToString() + "\" )  />");

        }
    }
}
