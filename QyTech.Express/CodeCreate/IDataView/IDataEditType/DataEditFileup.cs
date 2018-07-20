using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SunMvcExpress.Dao;


namespace QyTech.Express.CodeCreate.IDataView.IDataEditType
{
    public class DataEditFileup : IDataEditType
    {
        public override void Create(StreamWriter sw, bsFuncField bf, ISysFunConf Ifc)
        {
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<input  bringbackname=\"attachment.fileName\" id=\"" + Ifc.fc.TName + "_" + bf.FName + "\"  name=\"" + Ifc.fc.TName + "_" + bf.FName + "\" value=\"\" readonly=\"readonly\" type=\"text\" " + ((Boolean)bf.FormEditable ? "class=\"required\"" : "disabled") + " size=\"" + bf.FormFWidth.ToString() + "\"/>");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "   <a class=\"btnAttach\" href=\"@Url.Action(\"db_attachmentLookup\",\"FileUp\")\" lookupGroup=\"attachment\" width=\"560\" height=\"300\" title=\"附件\">附件</a>");
               
        }
    }
}
