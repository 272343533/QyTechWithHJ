using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunMvcExpress.Dao;

using QyTech.Core.Common;

namespace QyTech.Express.CodeCreate.IDataView
{
    public class PCCommEdit:IDataViewCreate
    {
        protected void CreateEditGrid(ISysFunConf ifc)
        {
            this.Ifc = ifc;
            base.Create(ifc);
           
            sw.WriteLine("           <dd> <input type=\"hidden\" name=\"" + Ifc.fc.TPK + "\" /></dd>");
            sw.WriteLine("           <dd> <input type=\"hidden\" name=\"DelIds\" /></dd>");

            List<string> LookupTnames = new List<string>();
            //明细内容
            LookupTnames.Clear();

            sw.WriteLine("       <table class=\"list nowrap itemDetailDIY\" addbutton=\"增加项\" width=\"100%\">");
            sw.WriteLine("          <div class=\"panelBar\">");
            sw.WriteLine("              <ul class=\"toolBar\">");
            //sw.WriteLine("\t" + "\t" + "\t" + " @Html.Raw(@ViewBag.OperRights)");
            sw.WriteLine("                  <li><a class=\"add\" href=\"#\" id=\"tabadd\"><span>添加</span></a></li>");
            sw.WriteLine("              </ul>");
            sw.WriteLine("          </div>");
            sw.WriteLine("          <thead>");
            sw.WriteLine("              <tr>");
            foreach (bsFuncField bf in Ifc_Child.dbFunfields)
            {
                if (!(bool)bf.ListVisible)
                    continue;
                if (bf.TypeGroup.ToLower() == TypeGroup.Decimal.ToString().ToLower())
                {
                    //sw.WriteLine("			<dd><input class=\"" + ((Boolean)bf.Editable ? "required" : "readonly") + "\" name=\"" + bf.FName + "\" type=\"required digital\" size=\"30\" /></dd>");
                    sw.WriteLine("			<th type=\"text\" name=\"items[#index#]." + bf.FName + "\" defaultVal=\"0\" size=\"12\" fieldClass=\"number\">" + bf.FDesp + "</th>");
                }
                else if (bf.TypeGroup == TypeGroup.Datetime.ToString())
                {
                    sw.WriteLine("			<th type=\"time\" name=\"items[#index#]." + bf.FName + "\" defaultVal=\"yyyy-MM-dd\" size=\"12\">" + bf.FDesp + "</th>");
                }
                else
                {
                    if (bf.EditType == EditType.lookup.ToString())
                    {
                        if (bf.LookupLink != null)
                        {
                            string[] lookupT2f = bf.LookupLink.Split(new char[] { '.' });
                            if (!LookupTnames.Contains(lookupT2f[0]))
                            {
                                sw.WriteLine("                  <th type=\"lookup\" bringbackname=\"items[#index#]." + bf.LookupLink + "\" lookupGroup=\"items[#index#]." + lookupT2f[0] + "\" lookupUrl=\"@Url.Action(\"" + lookupT2f[0] + "lookup\",\"" + lookupT2f[0] + "\")\" name=\"items[#index#]." + bf.FName + "\" size=\"12\" fieldClass=\"required\">" + bf.FDesp + "</th>");
                                LookupTnames.Add(lookupT2f[0]);
                            }
                            else
                            {
                                sw.WriteLine("                  <th type=\"lookup\" bringbackname=\"items[#index#]." + bf.LookupLink + "\" lookupGroup=\"items[#index#]." + lookupT2f[0] + "\" lookupUrl=\"@Url.Action(\"" + lookupT2f[0] + "lookup\",\"" + lookupT2f[0] + "\")\" name=\"items[#index#]." + bf.FName + "\" size=\"12\" fieldClass=\"required\">" + bf.FDesp + "</th>");
                            }
                        }
                    }
                    else if (bf.EditType.ToLower() == EditType.fileup.ToString().ToLower())
                    {
                        sw.WriteLine("			<th type=\"attach\" name=\"items[#index#].attachment.fileName\" lookupGroup=\"items[#index#].attachment\" lookupUrl=\"demo/database/db_attachmentLookup.html\" size=\"12\">" + bf.FDesp + "</th>");

                    }
                    else
                    {
                        sw.WriteLine("			<th type=\"label\" name=\"items[#index#]." + bf.FName + "\" size=\"12\" fieldAttrs=\"{maxlength:10}\">" + bf.FDesp + "</th>");
                    }
                }
            }

            LookupTnames.Clear();


            sw.WriteLine("                 <th type=\"del\" width=\"60\">操作</th>");
            sw.WriteLine("             </tr>");
            sw.WriteLine("        </thead>");
            sw.WriteLine("         <tbody>");
            sw.WriteLine("         @{");
            sw.WriteLine("             int index=-1;");
            sw.WriteLine("             if (ViewBag.DetailObj!=null)");
            sw.WriteLine("             {");
            sw.WriteLine("                 foreach(" + Ifc_Child.fc.TName + " detaobj in ViewBag.DetailObj as List<" + Ifc_Child.fc.TName + ">)");
            sw.WriteLine("                 {");
            sw.WriteLine("                      index++;");
            sw.WriteLine("                     <tr>");
            foreach (bsFuncField bf in Ifc_Child.dbFunfields)
            {
                if (!(bool)bf.ListVisible)
                    continue;
                if (bf.TypeGroup.ToLower() == TypeGroup.Decimal.ToString().ToLower())
                {
                    sw.WriteLine("			<td  align=\"left\"><input value=\"@detaobj." + bf.FName + "\" class=\"" + ((Boolean)bf.FormEditable ? "required" : "readonly") + "\" name=\"items[@index]." + bf.FName + "\" type=\"required digital\" size=\"12\" /></td>");
                }

                else if (bf.TypeGroup.ToLower() == TypeGroup.Datetime.ToString().ToLower())
                {
                    sw.WriteLine("			<td  align=\"left\"><input value=\"@detaobj." + bf.FName + "\" class=\"" + ((Boolean)bf.FormEditable ? "required" : "readonly") + "\" name=\"items[@index]." + bf.FName + "\" type=\"date\" size=\"12\" /></td>");
                }
                else
                {
                    if (bf.EditType.ToLower() == EditType.lookup.ToString().ToLower())
                    {
                        if (bf.LookupLink != null)
                        {
                            string[] lookupT2f = bf.LookupLink.Split(new char[] { '.' });
                            bsSysFunc funobj = new bsSysFunc();//EntityManager<bsSysFunc>.GetByStringFieldName<bsSysFunc>("ActionName",lookupT2f[0]);
                            sw.WriteLine("          <td><input type=\"text\" value=\"@detaobj." + bf.FName + "\" class=\"required\" name=\"items[@index]." + funobj.TName + "." + lookupT2f[1] + "\" postfield=\"item[@index]." + bf.FName + "\"  size=\"12\" />");

                            //如果已经有了，就不在出了
                            if (!LookupTnames.Contains(lookupT2f[0]))
                            {
                                sw.WriteLine("          <a class=\"btnLook\" href=\"@Url.Action(\"" + lookupT2f[0] + "Lookup\", \"" + lookupT2f[0] + "\")\" lookupGroup=\"items[@index]." + lookupT2f[0] + "\"  title=\"查找带回\">查找带回</a></td>");
                                LookupTnames.Add(lookupT2f[0]);
                            }
                        }
                    }
                    else
                    {
                        sw.WriteLine("			<td><input value=\"@detaobj." + bf.FName + "\" class=\"" + ((Boolean)bf.FormEditable ? "required" : "readonly") + "\" name=\"items[@index]." + bf.FName + "\" type=\"text\" size=\"12\" /></td>");
                    }
                }
            }
            sw.WriteLine("<td><a href=\"javascript:void(0)\" class=\"btnDel '+ field.fieldClass + '\">删除</a></td>");

            sw.WriteLine("                     </tr>");
            sw.WriteLine("                 }");
            sw.WriteLine("             }");
            sw.WriteLine("          }");
            sw.WriteLine("         </tbody>");
            sw.WriteLine("     </table>");
        }
    }
}
