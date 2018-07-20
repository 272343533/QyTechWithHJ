using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SunMvcExpress.Dao;


namespace QyTech.Express.CodeCreate.IDataView.IDataEditType
{
    public class DataEditSuggext : IDataEditType
    {
        public override void Create(StreamWriter sw, bsFuncField bf, ISysFunConf Ifc)
        {
            #region suggest
            string[] lookupT2f = bf.LookupLink.Split(new char[] { '.' });
            //bsSysFunc funobj = EntityManager<bsSysFunc>.GetByStringFieldName<bsSysFunc>("ActionName", lookupT2f[0]);
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<input class=\"readonly\" id=\"" + Ifc.fc.TName + "_" + bf.FName + "\"  bringBackName=\"" + lookupT2f[0] + "." + lookupT2f[1] + "\" type=\"text\" name=\"" + bf.FName + "\" lookupGroup=\"" + lookupT2f[0] + "\"  size=\"" + bf.FormFWidth.ToString() + "\"/>");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<input class=\"required\" id=\"" + Ifc.fc.TName + "_" + bf.FName + "\"  name=\"" + Ifc.fc.TName + "_" + bf.FName + "\" suggestFields=\"" + lookupT2f[1] + "\" bringBackName=\"" + bf.FName + "\"." + lookupT2f[1] + "\"");
            //sw.WriteLine("			        suggestUrl=\"/" + funobj.FuncName + "/" + funobj.FuncName + "Suggest\" type=\"text\" value=\" \" lookupGroup=\"" + bf.FName + "\" size=\"" + bf.FormFWidth.ToString() + "\" />");
            #endregion
        }
    }
}
