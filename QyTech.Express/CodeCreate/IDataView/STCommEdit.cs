using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QyTech.Express.CodeCreate;
using SunMvcExpress.Dao;
using QyTech.Core.Common;

using System.IO;

namespace QyTech.Express.CodeCreate.IDataView
{
    public class STCommEdit : IDataViewCreate
    {
      

        //protected void CreateIndex()
        //{
        //    string CreateFileName = FILEPATHHEAD + Ifc.fc.FunContr + @"\" + ActionType.Index.ToString() + "Data.cshtml";
        //    BackupFile(CreateFileName);

        //   // string includefilename = FILEPATHHEAD + fc.FunContr + @"\" + fc.FunContr + ".vqu";

        //    FileStream fs = new FileStream(CreateFileName, FileMode.Create, FileAccess.Write);
        //    sw = new StreamWriter(fs, utf8WithBom);
        //    //使用StreamWriter来往文件中写入内容
        //    sw.BaseStream.Seek(0, SeekOrigin.Begin);

        //    base.CreatePagerForm(Ifc.fc, sw);
        //    //base.CreatePagerHeaderHead(fc);
        //    //base.CreatePagerHeaderEnd(fc);
        //    base.CreatePageContentDivHead(Ifc.fc, sw);

        //    Create(Ifc.fc, sw);
        //    base.CreatePageContentDivEnd(sw);
        //    base.CreateJs(sw);


        //    ////把richTextBox1中的内容写入文件
        //    //sw.WriteLine("@model List<" + DAONAMESPACE + "." + fc.TName + ">");
        //    //sw.WriteLine("<form id=\"pagerForm\" method=\"post\" action=\"@Url.Action(" + fullUrl[ActionType.Index] + ")\">  onsubmit=\"return divSearch(this, '"+DatajsbxName+"'");

        //    //if (fc.FunLayout == PageDataStyle.STCommEdit.ToString() || fc.FunLayout == PageDataStyle.STCommEdit.ToString())
        //    //    sw.WriteLine("\t" + "<input type=\"hidden\" name=\"fkcondtion\" id=\"fkcondtion\" value=\"@ViewBag.fkconditon\" />");
        //    //if (fc.Filterfield != "")
        //    //    sw.WriteLine("\t" + "<input type=\"hidden\" name=\"filter\" id=\"filter\" value=\"@ViewBag.filter\" />");

        //    //sw.WriteLine("\t" + "<input type=\"hidden\" name=\"pageNum\" value=\"1\" />");
        //    //sw.WriteLine("\t" + "<input type=\"hidden\" name=\"numPerPage\" value=\"@ViewBag.numPerPage\" />");
        //    //sw.WriteLine("\t" + "<input type=\"hidden\" name=\"orderbys\" value=\"@ViewBag.orderbys\" />");
        //    //sw.WriteLine("</form>");


        
        //    //sw.WriteLine("<div class=\"pageContent\">");
        //    //sw.WriteLine("\t" + "<div class=\"panelBar\">");
        //    //sw.WriteLine("\t" + "\t" + "<ul class=\"toolBar\">");

        //    //sw.WriteLine("\t" + "\t" + "\t" + " @Html.Raw(@ViewBag.OperRights)");

        //    //sw.WriteLine("\t" + "\t" + "</ul>");
        //    //sw.WriteLine("\t" + "</div>");
        //    //sw.WriteLine("");
        //    //sw.WriteLine("\t" + "<table class=\"table\" width=\"" + fc.ListTotalWidth.ToString() + "\"  layoutH=\"138\">");
        //    //sw.WriteLine("\t" + "\t" + "<thead>");
        //    //sw.WriteLine("\t" + "\t" + "    <tr>");
        //    //sw.WriteLine("\t" + "\t" + "        <th width=\"50\">序号</th>");
        //    //foreach (bsFuncField dbffield in dbFunfields)
        //    //{
        //    //    if ((bool)dbffield.ListVisible)
        //    //    {
        //    //        sw.WriteLine("\t" + "\t" + "        <th width=\"" + dbffield.ListFWidth.ToString() + "\">" + dbffield.FDesp + "</th>");

        //    //    }
        //    //}

        //    //sw.WriteLine("\t" + "\t" + "    </tr>");
        //    //sw.WriteLine("\t" + "\t" + "</thead>");
        //    //sw.WriteLine("\t" + "\t" + "<tbody>");
        //    //sw.WriteLine("\t" + "\t" + "@{");
        //    //sw.WriteLine("\t" + "\t" + "\t" + "if (Model != null && Model.Count > 0)");
        //    //sw.WriteLine("\t" + "\t" + "\t" + "{");
        //    //sw.WriteLine("\t" + "\t" + "\t" + "\t" + "int i = 1;");
        //    //sw.WriteLine("\t" + "\t" + "\t" + "\t" + "foreach (var m in Model)");
        //    //sw.WriteLine("\t" + "\t" + "\t" + "\t" + "{");
        //    //sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<tr target=\"oid\" rel=\"@m." + fc.TPK + "\" > ");
        //    ////循环处理每一个字段
        //    //sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "<td>@(i++)</td>");

        //    //foreach (bsFuncField dbffield in dbFunfields)
        //    //{
        //    //    if ((bool)dbffield.ListVisible)
        //    //    {

        //    //        if (dbffield.Type.ToLower() == FieldType.Datetime.ToString().ToLower())
        //    //            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "<td>@(m." + dbffield.FName + ".HasValue ? m." + dbffield.FName + ".Value.ToString(\"yyyy-MM-dd\"): \"\")</td>");
        //    //        else
        //    //            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "<td>@m." + dbffield.FName + "</td>");


        //    //    }
        //    //}

        //    //sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "</tr>");
        //    //sw.WriteLine("\t" + "\t" + "\t" + "\t" + "}");
        //    //sw.WriteLine("\t" + "\t" + "\t" + "}");
        //    //sw.WriteLine("\t" + "\t" + "}");
        //    //sw.WriteLine("\t" + "\t" + "</tbody>");
        //    //sw.WriteLine("\t" + "</table>");
        //    //sw.WriteLine("");
        //    //sw.WriteLine("\t" + "<div class=\"panelBar\">");
        //    //sw.WriteLine("\t" + "\t" + "<div class=\"pages\">");
        //    //sw.WriteLine("\t" + "\t" + "\t" + "<span>显示</span>");
        //    //sw.WriteLine("\t" + "\t" + "\t" + "\t" + "<select class=\"combox\" name=\"numPerPage\" onchange=\"navTabPageBreak({numPerPage:this.value})\", '"+DatajsbxName+"')\">");
        //    //sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<option value=\"10\">10</option>");
        //    //sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<option value=\"20\">20</option>");
        //    //sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<option value=\"50\">50</option>");
        //    //sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<option value=\"100\">100</option>");
        //    //sw.WriteLine("\t" + "\t" + "\t" + "\t" + "</select>");
        //    //sw.WriteLine("\t" + "\t" + "\t" + "<span>条，共 @ViewBag.totalCount 条</span>");
        //    //sw.WriteLine("\t" + "\t" + "</div>");
        //    //sw.WriteLine("\t" + "\t" + "<div class=\"pagination\" targetType=\"navTab\" totalCount=\"@ViewBag.totalCount\" numPerPage=\"@ViewBag.numPerPage\" pageNumShown=\"20\" currentPage=\"@ViewBag.pageNum\"></div>");
        //    //sw.WriteLine("\t" + "</div>");
        //    //sw.WriteLine("");
        //    //sw.WriteLine("</div>");
        //    //sw.WriteLine("");
        //    //sw.WriteLine("<script type=\"text/javascript\">");
        //    //sw.WriteLine("\t" + "$('[name=numPerPage]').last().val('@ViewBag.numPerPage');");
        //    //sw.WriteLine("</script>");

        //    //sw.Flush();
        //    ////关闭此文件
        //    //sw.Close();

        //}

        public override void Create(ISysFunConf ifc, StreamWriter sw)
        {
            sw.WriteLine("\t" + "<div class=\"pageContent\" style=\"border:solid 1px #CCC\">");

            sw.WriteLine("\t" + "\t" + "<div class=\"panelBar\">");
            sw.WriteLine("\t" + "\t" + "\t" + "<ul class=\"toolBar\">");

            sw.WriteLine("\t" + "\t" + "\t" + "\t" + " @Html.Raw(@ViewBag.OperRights)");

            sw.WriteLine("\t" + "\t" + "\t" + "</ul>");
            sw.WriteLine("\t" + "\t" + "</div>");
            sw.WriteLine("");
            sw.WriteLine("\t" + "\t" + "<table class=\"table\" width=\"" + ifc.fc.ListTotalWidth.ToString() + "\"  layoutH=\"138\">");
            sw.WriteLine("\t" + "\t" + "\t" + "<thead>");
            sw.WriteLine("\t" + "\t" + "\t" + "    <tr>");
            sw.WriteLine("\t" + "\t" + "\t" + "        <th width=\"50\">序号</th>");
            foreach (bsFuncField dbffield in ifc.dbFunfields)
            {
                if ((bool)dbffield.ListVisible)
                {
                    sw.WriteLine("\t" + "\t" + "\t" + "        <th width=\"" + dbffield.ListFWidth.ToString() + "\">" + dbffield.FDesp + "</th>");

                }
            }

            sw.WriteLine("\t" + "\t" + "\t" + "    </tr>");
            sw.WriteLine("\t" + "\t" + "\t" + "</thead>");
            sw.WriteLine("\t" + "\t" + "\t" + "<tbody>");
            sw.WriteLine("\t" + "\t" + "\t" + "@{");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "if (Model != null && Model.Count > 0)");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "{");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "int i = 1;");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "foreach (var m in Model)");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "{");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "<tr target=\"oid\" rel=\"@m." + ifc.fc.TPK + "\" > ");
            //循环处理每一个字段
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "<td>@(i++)</td>");

            foreach (bsFuncField dbffield in ifc.dbFunfields)
            {
                if ((bool)dbffield.ListVisible)
                {

                    if (dbffield.Type.ToLower() == FieldType.Datetime.ToString().ToLower())
                        sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "<td>@(m." + dbffield.FName + ".HasValue ? m." + dbffield.FName + ".Value.ToString(\"yyyy-MM-dd\"): \"\")</td>");
                    else
                        sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "<td>@m." + dbffield.FName + "</td>");
                }
            }

            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "</tr>");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "}");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "}");
            sw.WriteLine("\t" + "\t" + "\t" + "}");
            sw.WriteLine("\t" + "\t" + "\t" + "</tbody>");
            sw.WriteLine("\t" + "\t" + "</table>");
            sw.WriteLine("");
            sw.WriteLine("\t" + "\t" + "<div class=\"panelBar\">");
            sw.WriteLine("\t" + "\t" + "\t" + "<div class=\"pages\">");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "<span>显示</span>");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<select class=\"combox\" name=\"numPerPage\" onchange=\"navTabPageBreak({numPerPage:this.value})\", '" + DatajsbxName + "')\">");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "<option value=\"10\">10</option>");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "<option value=\"20\">20</option>");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "<option value=\"50\">50</option>");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "<option value=\"100\">100</option>");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "</select>");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "<span>条，共 @ViewBag.totalCount 条</span>");
            sw.WriteLine("\t" + "\t" + "\t" + "</div>");
            sw.WriteLine("\t" + "\t" + "\t" + "<div class=\"pagination\" targetType=\"navTab\" totalCount=\"@ViewBag.totalCount\" numPerPage=\"@ViewBag.numPerPage\" pageNumShown=\"20\" currentPage=\"@ViewBag.pageNum\"></div>");
            sw.WriteLine("\t" + "\t" + "</div>");
            sw.WriteLine("\t" + "</div>");
        }
        public override void Create(ISysFunConf ifc)
        {
            base.Create(ifc);
           
            CreateFileName = FILEPATHHEAD +ifc. fc.FunContr + @"\" + ActionType.Index.ToString() + "Data.cshtml";
            base.CreateFileHead(ifc,CreateFileName);

            base.CreatePagerForm(ifc, sw);

            Create(ifc, sw);

            base.CreateJs(sw);

            base.CreateFileEnd();
        }

        public override void CreateDataInIndex(ISysFunConf ifc,StreamWriter  sw)
        {
            Create(ifc, sw);

            base.CreateJs(sw);

        }

    }
}
