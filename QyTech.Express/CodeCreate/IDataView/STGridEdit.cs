using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QyTech.Express.CodeCreate;
using SunMvcExpress.Dao;
using QyTech.Core.Common;
using System.IO;
using SunMvcExpress.Core.BLL;
namespace QyTech.Express.CodeCreate.IDataView
{
    public class STGridEdit: IDataViewCreate
    {
        public override void Create(ISysFunConf ifc, StreamWriter sw)
        {
           //protected void CreateEditGrid(StreamWriter sw)
            //{
            sw.WriteLine("@model List<SunMvcExpress.Dao."+Ifc.fc.TName+"> ");
            sw.WriteLine("@using SunMvcExpress.Dao;");
            sw.WriteLine("");
            sw.WriteLine("<form method=\"post\" action=\"@Url.Action(\"GridEdit\",\"" + Ifc.fc.TName + "\")\"  class=\"pageForm required-validate\" onsubmit=\"return validateCallback(this,  "+ Ifc.fc.FunContr + "IndexdialogAjaxDone);\">");

            sw.WriteLine("<div class=\"pageContent\">");
            sw.WriteLine("    <div class=\"panelBar\">");
            sw.WriteLine("        <ul class=\"toolBar\">");
            //sw.WriteLine("\t" + "\t" + "\t" + " @Html.Raw(@ViewBag.OperRights)");
            sw.WriteLine("            <li><a class=\"add\" href=\"@Url.Action(\"GridAdd\",\""+Ifc.fc.FunContr +"\")?"+Ifc.fc.PFK+"=@ViewBag."+Ifc.fc.PFK +"\"  target=\"ajaxTodo\" id=\"tabadd\"><span>添加</span></a></li>");
            sw.WriteLine("        </ul>");
            sw.WriteLine("    </div>");
            sw.WriteLine("    <div class=\"tabsContent\" layoutH=\"55\">");
            sw.WriteLine("           <dd> <input type=\"hidden\" name=\"" + Ifc.fc.TPK + "\" /></dd>");
            sw.WriteLine("           <dd> <input type=\"hidden\" name=\"DelIds\" /></dd>");

            List<string> LookupTnames = new List<string>();
            //明细内容
            LookupTnames.Clear();

            sw.WriteLine("       <table class=\"list nowrap itemDetailDIY\" addbutton=\"增加项\" width=\"100%\">");

            sw.WriteLine("          <thead>");
            sw.WriteLine("              <tr>");
            foreach (bsFuncField bf in Ifc.dbFunfields)
            {
                if (!(bool)bf.ListVisible)
                {
                    sw.WriteLine("\t" +"			<th size=\"12\" style=\"display:none;\"></th>");
                }
                else
                {
                    if (bf.TypeGroup.ToLower() == TypeGroup.Decimal.ToString().ToLower())
                    {
                        //sw.WriteLine("			<dd><input class=\"" + ((Boolean)bf.Editable ? "required" : "readonly") + "\" name=\"" + bf.FName + "\" type=\"required digital\" size=\"30\" /></dd>");
                        sw.WriteLine("\t" +"			<th type=\"text\" name=\"items[#index#]." + bf.FName + "\" defaultVal=\"0\" size=\"12\" fieldClass=\"number\">" + bf.FDesp + "</th>");
                    }
                    else if (bf.TypeGroup == TypeGroup.Datetime.ToString())
                    {
                        sw.WriteLine("\t"+ "			<th type=\"time\" name=\"items[#index#]." + bf.FName + "\" defaultVal=\"yyyy-MM-dd\" size=\"12\">" + bf.FDesp + "</th>");
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
                                    sw.WriteLine("\t" + "                  <th type=\"lookup\" bringbackname=\"items[#index#]." + bf.LookupLink + "\" lookupGroup=\"items[#index#]." + lookupT2f[0] + "\" lookupUrl=\"@Url.Action(\"" + lookupT2f[0] + "lookup\",\"" + lookupT2f[0] + "\")\" name=\"items[#index#]." + bf.FName + "\" size=\"12\" fieldClass=\"required\">" + bf.FDesp + "</th>");
                                    LookupTnames.Add(lookupT2f[0]);
                                }
                                else
                                {
                                    sw.WriteLine("\t" + "                  <th type=\"lookup\" bringbackname=\"items[#index#]." + bf.LookupLink + "\" lookupGroup=\"items[#index#]." + lookupT2f[0] + "\" lookupUrl=\"@Url.Action(\"" + lookupT2f[0] + "lookup\",\"" + lookupT2f[0] + "\")\" name=\"items[#index#]." + bf.FName + "\" size=\"12\" fieldClass=\"required\">" + bf.FDesp + "</th>");
                                }
                            }
                        }
                        else if (bf.EditType.ToLower() == EditType.fileup.ToString().ToLower())
                        {
                            sw.WriteLine("\t" + "			<th type=\"attach\" name=\"items[#index#].attachment.fileName\" lookupGroup=\"items[#index#].attachment\" lookupUrl=\"demo/database/db_attachmentLookup.html\" size=\"12\">" + bf.FDesp + "</th>");

                        }
                        else
                        {
                            sw.WriteLine("\t" + "			<th type=\"label\" name=\"items[#index#]." + bf.FName + "\" size=\"12\" fieldAttrs=\"{maxlength:10}\">" + bf.FDesp + "</th>");
                        }
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
            sw.WriteLine("             if (@Model!=null)");
            sw.WriteLine("             {");
            sw.WriteLine("                 foreach(" + Ifc.fc.TName + " detaobj in @Model as List<" + Ifc.fc.TName + ">)");
            sw.WriteLine("                 {");
            sw.WriteLine("                      index++;");
            sw.WriteLine("                     <tr>");
            foreach (bsFuncField bf in Ifc.dbFunfields)
            {
                if (!(bool)bf.ListVisible)
                {
                    sw.WriteLine("\t" + "\t" + "\t" + "			<td style=\"display:none;\"><input value=\"@detaobj." + bf.FName + "\" class=\"required\" name=\"items[@index]." + bf.FName + "\" type=\"text\" size=\"12\" /></td>");
                }
                else
                {
                    if (bf.TypeGroup.ToLower() == TypeGroup.Decimal.ToString().ToLower())
                    {
                        sw.WriteLine("\t" + "\t" + "\t" + "			<td  align=\"left\"><input value=\"@detaobj." + bf.FName + "\" class=\"" + ((Boolean)bf.FormEditable ? "required" : "readonly") + "\" name=\"items[@index]." + bf.FName + "\" type=\"required digital\" size=\"12\" /></td>");
                    }

                    else if (bf.TypeGroup.ToLower() == TypeGroup.Datetime.ToString().ToLower())
                    {
                        sw.WriteLine("\t" + "\t" + "\t" + "			<td  align=\"left\"><input value=\"@detaobj." + bf.FName + "\" class=\"" + ((Boolean)bf.FormEditable ? "required" : "readonly") + "\" name=\"items[@index]." + bf.FName + "\" type=\"date\" size=\"12\" /></td>");
                    }
                    else
                    {
                        if (bf.EditType.ToLower() == EditType.lookup.ToString().ToLower())
                        {
                            if (bf.LookupLink != null)
                            {
                                string[] lookupT2f = bf.LookupLink.Split(new char[] { '.' });
                                bsSysFunc funobj = new bsSysFunc();//EntityManager<bsSysFunc>.GetByStringFieldName<bsSysFunc>("ActionName",lookupT2f[0]);
                                sw.WriteLine("\t" + "\t" + "\t" + "          <td><input type=\"text\" value=\"@detaobj." + bf.FName + "\" class=\"required\" name=\"items[@index]." + funobj.TName + "." + lookupT2f[1] + "\" postfield=\"item[@index]." + bf.FName + "\"  size=\"12\" />");

                                //如果已经有了，就不在出了
                                if (!LookupTnames.Contains(lookupT2f[0]))
                                {
                                    sw.WriteLine("\t" + "\t" + "\t" + "          <a class=\"btnLook\" href=\"@Url.Action(\"" + lookupT2f[0] + "Lookup\", \"" + lookupT2f[0] + "\")\" lookupGroup=\"items[@index]." + lookupT2f[0] + "\"  title=\"查找带回\">查找带回</a></td>");
                                    LookupTnames.Add(lookupT2f[0]);
                                }
                            }
                        }
                        else if (bf.EditType.ToLower() == EditType.select.ToString().ToLower())
                        {
                            //要根据是固定的还是链接进行不同的处理
                            sw.WriteLine("\t" + "\t" + "\t" + "			<td>");
                            sw.WriteLine("\t" + "\t" + "\t" + "			    <select class=\"required\" name=\"items[@index]." + bf.FName + "\">");
                            if (bf.LookupLink.Length>0)
                            {
                                if (bf.LookupLink.Substring(0,1)=="{")
                                {
                                    string[] splits=bf.LookupLink.Split(new char[] {'{','}',';'});
                                    if (splits[0].IndexOf(',')<0)
                                    {
                                        foreach (string s in splits)
                                        {
                                            sw.WriteLine("\t" + "\t" + "\t" + "			        <option value=\"" +s + "\" @(detaobj."+bf.FName+" ==" + s + "? \"selected\" : \"\")>" + s + "</option>");
                                        }
                                    }
                                    else
                                    {
                                         foreach (string s in splits)
                                        {
                                             string[] s2=s.Split(new char[] {','});
                                             sw.WriteLine("\t" + "\t" + "\t" + "			        <option value=\"" +s2[0] + "\" @(detaobj."+bf.FName+" ==" + s2[0]+ "? \"selected\" : \"\")>" +s2[1] + "</option>");
                                        }
                                    }
                                }
                                else
                                {
                                    //要获取对应的接口对象
                                    try
                                    {
                                        string[] interfs = bf.LookupLink.Split(new char[] { '(', '.' });
                                        bsSysFunc fc = EntityManager<bsSysFunc>.GetBySql<bsSysFunc>("FunContr='" + interfs[0].Substring(0, interfs[0].Length - 3) + "'");
                                        bsSysFuncInterface fi=EntityManager<bsSysFuncInterface>.GetBySql<bsSysFuncInterface>("FcId='"+fc.FcId.ToString()+"' and InterfName='"+bf.LookupLink.Split(new char[]{'(','.'})[1]+"'");

                                        sw.WriteLine("\t" + "\t" + "\t" + "			    @foreach (SunMvcExpress.Dao." + fi.RetObject + " vbi in ViewBag.vb" + bf.FName + "s as List<" + fi.RetObject + ">)");
                                        sw.WriteLine("\t" + "\t" + "\t" + "			    {");
                                        sw.WriteLine("\t" + "\t" + "\t" + "			        <option value=\"@vbi." + bf.FName + "\" @(detaobj." + bf.FName + " ==@vbi." + bf.FName + "? \"selected\" : \"\")>@vbi." + bf.FName + "</option>");
                                        sw.WriteLine("\t" + "\t" + "\t" + "			    }");
                                    }
                                    catch(Exception ex)
                                    {
                                        log.Error(ex.Message);
                                    }
                                }
                            }
                            sw.WriteLine("\t" + "\t" + "\t" + "			    </select>");
                            sw.WriteLine("\t" + "\t" + "\t" + "			</td>");
                           
                        }
                        else
                        {
                            sw.WriteLine("\t" + "\t" + "\t" + "			<td><input value=\"@detaobj." + bf.FName + "\" class=\"" + ((Boolean)bf.FormEditable ? "required" : "readonly") + "\" name=\"items[@index]." + bf.FName + "\" type=\"text\" size=\"12\" /></td>");
                        }
                    }
                }
            }
            sw.WriteLine("<td><a href=\"@Url.Action(\"GridDel\", \"" + Ifc.fc.TName + "\")?id=@detaobj." + Ifc.fc.TPK + "\" class=\"btnDel '+ field.fieldClass + '\">删除</a></td>");

            sw.WriteLine("                     </tr>");
            sw.WriteLine("                 }");
            sw.WriteLine("             }");
            sw.WriteLine("          }");
            sw.WriteLine("         </tbody>");
            sw.WriteLine("     </table>");
            sw.WriteLine("\t" + "</div>");
            sw.WriteLine("</div>");

            sw.WriteLine("\t" + "<div class=\"formBar\">");
            sw.WriteLine("\t" + "    <ul>");
            sw.WriteLine("\t" + "        <li><div class=\"buttonActive\"><div class=\"buttonContent\"><button type=\"submit\">保存</button></div></div></li>");
            sw.WriteLine("\t" + "        <li><div class=\"button\"><div class=\"buttonContent\"><button type=\"button\" class=\"close\">取消</button></div></div></li>");
            sw.WriteLine("\t" + "	  </ul>");
            sw.WriteLine("\t"+"</div>");
            sw.WriteLine("</form>");

        }

        public override void Create(ISysFunConf ifc)
        {
            base.Create(ifc);

            CreateFileName = FILEPATHHEAD + ifc.fc.FunContr + @"\" + ActionType.Index.ToString() + ".cshtml";
            base.CreateFileHead(ifc, CreateFileName);

            Create(ifc, sw);

            CreateJs(sw);

            base.CreateFileEnd();
        }

        protected override void CreateJs(StreamWriter sw)
        {
            sw.WriteLine("");
            sw.WriteLine("<script type=\"text/javascript\">");
            //sw.WriteLine("\t" + "$('[name=numPerPage]').last().val('@ViewBag.numPerPage');");
            sw.WriteLine("    function "+Ifc.fc.FunContr +"IndexdialogAjaxDone(json) {");
            sw.WriteLine("       DWZ.ajaxDone(json);");
            sw.WriteLine("       if (json.statusCode == DWZ.statusCode.ok) {");
            sw.WriteLine("            $.pdialog.closeCurrent();");
            sw.WriteLine("       }");
            sw.WriteLine("   }");
            sw.WriteLine("</script>");
        }
    }
}
