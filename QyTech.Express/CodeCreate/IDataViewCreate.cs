using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using log4net;
using System.Reflection;
using SunMvcExpress.Dao;
using SunMvcExpress.Core.Common;
using QyTech.Express.CodeCreate.IDataView.IDataEditType;

using QyTech.Core;
using QyTech.Core.Common;
using QyTech.Core.BLL;


namespace QyTech.Express
{
    public class IDataViewCreate: ICSharpMvcCode
    {
        
        public IDataViewCreate()
        {
            FILEPATHHEAD = FILEPATHHEAD + @"Views\";
        }


        public virtual void Create(ISysFunConf ifc)
        {
            this.Ifc = ifc;

               //生成view目录
            if (!System.IO.Directory.Exists(FILEPATHHEAD + Ifc.fc.FunContr))
            {
                System.IO.Directory.CreateDirectory(FILEPATHHEAD + Ifc.fc.FunContr);
            }
        }

        public virtual void Create(ISysFunConf ifc, StreamWriter sw)
        {
            Create(ifc);
        }
        protected virtual void CreateLookup()
        {
            string strline;
            bsSysFunc fc = Ifc.fc;
            string CreateFileName = FILEPATHHEAD + fc.FunContr + @"\" + ActionType.Lookup.ToString() + ".cshtml";
            BackupFile(CreateFileName);

            FileStream fs = new FileStream(CreateFileName, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, utf8WithBom);
            //使用StreamWriter来往文件中写入内容
            sw.BaseStream.Seek(0, SeekOrigin.Begin);
            sw.WriteLine("@model List<" + DAONAMESPACE + "." + fc.TName + ">");
            sw.WriteLine("<form id=\"pagerForm\" method=\"post\" action=\"@Url.Action(\"@Url.Action(" + fullUrl[ActionType.Lookup] + ")\"> ");
            sw.WriteLine("\t" + "<input type=\"hidden\" name=\"funid\" value=\"" + fc.FunId.ToString() + "\" />");
            if (Ifc.FuncQuerys != null)
            {
                foreach (bsSysFuncQuery bf in Ifc.FuncQuerys)
                {
                    if (bf == null)
                        continue;
                    //日期型数据的处理。
                    ////if (bf.Type.ToLower() == FieldType.Varchar.ToString().ToLower())
                    ////{
                    ////    sw.WriteLine("\t" + " <input type=\"hidden\" name=\"query_" + bf.Name + "\" value=\"@ViewBag." + bf.Name + "\" />");
                    ////}
                    ////else if (bf.Type.ToLower() == FieldType.Datetime.ToString().ToLower() || bf.Type.ToLower() == FieldType.Date.ToString().ToLower())
                    ////{
                    ////    sw.WriteLine("\t" + " <input type=\"hidden\" name=\"query_" + bf.Name + "_start\" value=\"@ViewBag." + bf.Name + "_start\" />");
                    ////    sw.WriteLine("\t" + " <input type=\"hidden\" name=\"query_" + bf.Name + "_end\" value=\"@ViewBag." + bf.Name + "_end\" />");
                    ////}
                }
            }
            sw.WriteLine("\t" + "<input type=\"hidden\" name=\"pageNum\" value=\"1\" />");
            sw.WriteLine("\t" + "<input type=\"hidden\" name=\"numPerPage\" value=\"@ViewBag.numPerPage\" />");
            sw.WriteLine("\t" + "<input type=\"hidden\" name=\"orderField\" value=\"@ViewBag.orderField\" />");
            sw.WriteLine("</form>");

            sw.WriteLine("<div class=\"pageHeader\">");
            sw.WriteLine("\t" + "<form rel=\"pagerForm\" method=\"post\" action=\"@Url.Action(" + ActionType.Lookup.ToString() + ")\" onsubmit=\"return dwzSearch(this, 'dialog');\">");
            sw.WriteLine("\t" + "\t" + "<div class=\"searchBar\">");
            sw.WriteLine("\t" + "\t" + "<input type=\"hidden\" name=\"pageNum\" value=\"@ViewBag.pageNum\" />");
            sw.WriteLine("\t" + "\t" + "<input type=\"hidden\" name=\"numPerPage\" value=\"@ViewBag.numPerPage\" />");
            sw.WriteLine("\t" + "\t" + "\t" + "<table class=\"searchContent\">");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "<tr>");
            foreach (bsSysFuncQuery bf in Ifc.FuncQuerys)
            {
                if (bf == null)
                    continue;
                sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<td>");
                sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + bf.FFDesp + "：<input type=\"text\" name=\"query_" + bf.FFName + "\" value=\"@ViewBag." + bf.FFName + "\"/>");
                sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "</td>");
            }
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "</tr>");
            sw.WriteLine("\t" + "\t" + "\t" + "</table>");
            sw.WriteLine("\t" + "\t" + "\t" + "<div class=\"subBar\">");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "<ul>");

            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<li><div class=\"buttonActive\"><div class=\"buttonContent\"><button type=\"submit\" onClick=\"beforeSubmit()\">检索</button></div></div></li>");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "</ul>");
            sw.WriteLine("\t" + "\t" + "\t" + "</div>");
            sw.WriteLine("\t" + "\t" + "</div>");
            sw.WriteLine("\t" + "</form>");
            sw.WriteLine("</div>");
            sw.WriteLine("<div class=\"pageContent\">");
            sw.WriteLine("\t" + "<table class=\"table\" layoutH=\"118\" targetType=\"dialog\" width=\"100%\">");
            sw.WriteLine("\t" + "\t" + "<thead>");
            sw.WriteLine("\t" + "\t" + "<tr>");
            foreach (bsFuncField dbffield in Ifc.dbFunfields)
            {
                if ((bool)dbffield.ListVisible)
                {
                    sw.WriteLine("\t" + "\t" + "<th width=\"" + dbffield.ListFWidth.ToString() + "\" orderfield=\"" + dbffield.FName + "\">" + dbffield.FDesp + "</th>");
                }
            }
            sw.WriteLine("\t" + "\t" + "<th width=\"80\">选择</th>");
            sw.WriteLine("\t" + "\t" + "</tr>");
            sw.WriteLine("\t" + "\t" + "</thead>");
            sw.WriteLine("\t" + "\t" + "<tbody>");
            sw.WriteLine("\t" + "\t" + "@{");
            sw.WriteLine("\t" + "\t" + "\t" + "if (Model != null && Model.Count > 0)");
            sw.WriteLine("\t" + "\t" + "\t" + "{");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "foreach (var m in Model)");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "{");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<tr target=\"oid\" rel=\"@m." + fc.TPK + "\" > ");
            //循环处理每一个字段
            foreach (bsFuncField dbffield in Ifc.dbFunfields)
            {
                if ((bool)dbffield.ListVisible)
                {
                    sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "<td>@m." + dbffield.FName + "</td>");
                }
            }
            strline = "\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "<td><a class=\"btnSelect\" href=\"javascript:$.bringBack({ ";
            //首先带回主键
            strline += fc.TPK + ":'@m." + fc.TPK + "',";

            if (fc.LookupFields != null && !fc.LookupFields.Equals(""))
            {
                string[] backfields = fc.LookupFields.Split(new char[] { ',' });
                foreach (string f in backfields)
                {
                    strline += f + ":'@m." + f + "',";
                }

                strline = strline.Substring(0, strline.Length - 1);
            }
            strline += "})\" title=\"查找带回\">选择</a></td>";
            sw.WriteLine(strline);

            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "</tr>");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "}");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "}");
            sw.WriteLine("\t" + "\t" + "\t" + "}			");
            sw.WriteLine("\t" + "\t" + "\t" + "</tbody>");
            sw.WriteLine("\t" + "\t" + "</table>");

            sw.WriteLine("");
            sw.WriteLine("\t" + "<div class=\"panelBar\">");
            sw.WriteLine("\t" + "\t" + "<div class=\"pages\">");
            sw.WriteLine("\t" + "\t" + "\t" + "<span>显示</span>");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "<select class=\"combox\" name=\"numPerPage\" onchange=\"dwzPageBreak({targetType:dialog, numPerPage:'10'})\">");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<option value=\"10\">10</option>");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<option value=\"20\">20</option>");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<option value=\"50\">50</option>");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<option value=\"100\">100</option>");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "</select>");
            sw.WriteLine("\t" + "\t" + "\t" + "<span>条，共 @ViewBag.totalCount 条</span>");
            sw.WriteLine("\t" + "\t" + "</div>");
            sw.WriteLine("\t" + "\t" + "<div class=\"pagination\" targetType=\"dialog\" totalCount=\"@ViewBag.totalCount\" numPerPage=\"@ViewBag.numPerPage\" pageNumShown=\"10\" currentPage=\"@ViewBag.pageNum\"></div>");
            sw.WriteLine("\t" + "</div>");
            sw.WriteLine("");
            sw.WriteLine("</div>");
            sw.WriteLine("");
            sw.WriteLine("<script type=\"text/javascript\">");
            sw.WriteLine("\t" + "$('[name=numPerPage]').last().val('@ViewBag.numPerPage');");
            sw.WriteLine("</script>");

            sw.Flush();
            //关闭此文件
            sw.Close();

        }

        protected void CreateEditForm(StreamWriter sw)
        {
            //主数据显示
            IDataEditType vObj = new IDataEditType();
               
            foreach (bsFuncField bf in Ifc.dbFunfields)
            {
                if (bf.EditType == "Hidden")
                {
                    vObj = FacDataEditCreate.Create(bf.EditType);
                    vObj.Create(sw, bf, Ifc);
                }
                else
                {
                    sw.WriteLine("\t" + "\t" + "\t" + "<dl>");
                    sw.WriteLine("\t" + "\t" + "\t" + "\t" + "<dt> " + bf.FDesp + "：</dt>");
                    sw.WriteLine("\t" + "\t" + "\t" + "\t" + "<dd>");

                    vObj = FacDataEditCreate.Create(bf.EditType);
                    vObj.Create(sw, bf, Ifc);


                    sw.WriteLine("\t" + "\t" + "\t" + "\t" + "</dd>");
                    sw.WriteLine("\t" + "\t" + "\t" + "</dl>");
                }
            }
        }

        public virtual void CreateDataInIndex(ISysFunConf ifc, StreamWriter sw)
        {
           
        }
 

    }

}
