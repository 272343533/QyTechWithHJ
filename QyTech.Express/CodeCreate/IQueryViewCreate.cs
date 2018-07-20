using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using log4net;
using System.Reflection;
using SunMvcExpress.Dao;
using SunMvcExpress.Core.Common;

using QyTech.Core;
using QyTech.Core.Common;


namespace QyTech.Express
{
    public class IQueryViewCreate: ICSharpMvcCode
    {
        public IQueryViewCreate()
        {
            FILEPATHHEAD = FILEPATHHEAD + @"Views\";
        }

        private void CreateDirectory()
        {
            //生成view目录
            if (!System.IO.Directory.Exists(FILEPATHHEAD + Ifc.fc.FunContr))
            {
                System.IO.Directory.CreateDirectory(FILEPATHHEAD + Ifc.fc.FunContr);
            }
        }

        public virtual void Create(ISysFunConf ifc)
        {
            Ifc = ifc;
            CreateDirectory();
        }
        public virtual void Create(ISysFunConf ifc,bsSysFuncQuery fq)
        {

        }
        public virtual void Create(ISysFunConf ifc, bsSysFuncQuery fq, StreamWriter sw)
        {

        }
        public virtual void Create(bsSysFuncQuery fq, StreamWriter sw)
        {

        }
        public virtual void Create(bsSysFuncQuery fq)
        {

        }
        protected virtual void CreateIndex()
        {

            string CreateFileName = FILEPATHHEAD + Ifc.fc.FunContr + @"\" + ActionType.Index.ToString() + ".cshtml";
            BackupFile(CreateFileName);

            string includefilename = FILEPATHHEAD + Ifc.fc.FunContr + @"\" + Ifc.fc.FunContr + ".vqu";

            FileStream fs = new FileStream(CreateFileName, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, utf8WithBom);
            //使用StreamWriter来往文件中写入内容
            sw.BaseStream.Seek(0, SeekOrigin.Begin);
            //把richTextBox1中的内容写入文件
            sw.WriteLine("@model List<" + DAONAMESPACE + "." + Ifc.fc.TName + ">");
            sw.WriteLine("<form id=\"pagerForm\" method=\"post\" action=\"@Url.Action(" + fullUrl[ActionType.Index] + ")\"> ");

            if (Ifc.fc.FunLayout == PageDataStyle.STCommEdit.ToString() || Ifc.fc.FunLayout == PageDataStyle.STCommEdit.ToString())
                sw.WriteLine("\t" + "\t" + "<input type=\"hidden\" name=\"fkcondtion\" id=\"fkcondtion\" value=\"@ViewBag.fkconditon\" />");
            if (Ifc.fc.Filterfield != "")
                sw.WriteLine("\t" + "\t" + "<input type=\"hidden\" name=\"filter\" id=\"filter\" value=\"@ViewBag.filter\" />");

            //查询条件需要调整
            if (Ifc.FuncQuerys != null)
            {
                foreach (bsSysFuncQuery bf in Ifc_Child.FuncQuerys)
                {
                    if (bf == null)
                        continue;
                    //日期型数据的处理。
                    //////if (bf.Type.ToLower() == FieldType.Varchar.ToString().ToLower())
                    //////{
                    //////    sw.WriteLine("\t" + " <input type=\"hidden\" name=\"query_" + bf.Name + "\" value=\"@ViewBag." + bf.Name + "\" />");
                    //////}
                    //////else if (bf.Type.ToLower() == FieldType.Datetime.ToString().ToLower())
                    //////{
                    //////    if (!RangeFields.Contains(bf.Name))
                    //////        sw.WriteLine("\t" + " <input type=\"hidden\" name=\"query_" + bf.Name + "\" value=\"@ViewBag." + bf.Name + "\" />");

                    //////    else
                    //////    {
                    //////        sw.WriteLine("\t" + " <input type=\"hidden\" name=\"query_" + bf.Name + "_start\" value=\"@ViewBag." + bf.Name + "_start\" />");
                    //////        sw.WriteLine("\t" + " <input type=\"hidden\" name=\"query_" + bf.Name + "_end\" value=\"@ViewBag." + bf.Name + "_end\" />");
                    //////    }
                    //////}
                }
            }

            sw.WriteLine("\t" + "<input type=\"hidden\" name=\"pageNum\" value=\"1\" />");
            sw.WriteLine("\t" + "<input type=\"hidden\" name=\"numPerPage\" value=\"@ViewBag.numPerPage\" />");
            sw.WriteLine("\t" + "<input type=\"hidden\" name=\"orderbys\" value=\"@ViewBag.orderbys\" />");
            sw.WriteLine("</form>");


            sw.WriteLine("<div class=\"pageHeader\">");
            sw.WriteLine("\t" + "<form onsubmit=\"return navTabSearch(this);\" action=\"@Url.Action(" + fullUrl[ActionType.Index] + ")\" method=\"post\">");
            sw.WriteLine("\t" + "\t" + "<div class=\"searchBar\">");


            if (Ifc.fc.FunLayout == PageDataStyle.STCommEdit.ToString() || Ifc.fc.FunLayout == PageDataStyle.STCommEdit.ToString())
                sw.WriteLine("\t" + "\t" + "<input type=\"hidden\" name=\"fkcondtion\" id=\"fkcondtion\" value=\"@ViewBag.fkconditon\" />");
            if (Ifc.fc.Filterfield != "")
                sw.WriteLine("\t" + "\t" + "<input type=\"hidden\" name=\"filter\" id=\"filter\" value=\"@ViewBag.filter\" />");


            sw.WriteLine("\t" + "\t" + "<input type=\"hidden\" name=\"conditions\" id=\"conditions\" value=\"@ViewBag.conditions\" />");
            sw.WriteLine("\t" + "\t" + "<input type=\"hidden\" name=\"orderbys\" id=\"orderbys\" value=\"@ViewBag.orderbys\" />");
            sw.WriteLine("\t" + "\t" + "<input type=\"hidden\" name=\"pageNum\" value=\"@ViewBag.pageNum\" />");
            sw.WriteLine("\t" + "\t" + "<input type=\"hidden\" name=\"numPerPage\" value=\"@ViewBag.numPerPage\" />");
            sw.WriteLine("\t" + "\t" + "\t" + "<table class=\"searchContent\">");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "<tr>");

            //foreach (bsField bf in dbQueryfields)
            //{
            //    if (bf == null)
            //        continue;
            //    sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<td>");
            //    if (bf.Type.ToLower() == FieldType.Datetime.ToString().ToLower())
            //    {
            //        if (!RangeFields.Contains(bf.Name))
            //            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + bf.Desp + "：<input type=\"text\" readonly=\"true\" name=\"query_" + bf.Name + "\" value=\"@ViewBag." + bf.Name + "\" class=\"date\" dateFmt=\"yyyy-MM-dd\"  size=\"30\" class=\"required\"/>");
            //        else
            //        {
            //            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + bf.Desp + "：<input type=\"text\" readonly=\"true\" name=\"query_" + bf.Name + "_start\" value=\"@ViewBag." + bf.Name + "_start\" class=\"date\" dateFmt=\"yyyy-MM-dd\"  size=\"30\" class=\"required\"/>");
            //            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + bf.Desp + "-<input type=\"text\" readonly=\"true\" name=\"query_" + bf.Name + "_end\" value=\"@ViewBag." + bf.Name + "_end\" class=\"date\" dateFmt=\"yyyy-MM-dd\"  size=\"30\" class=\"required\"/>");
            //        }
            //    }
            //    else
            //        sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + bf.Desp + "：<input type=\"text\" name=\"query_" + bf.Name + "\" value=\"@ViewBag." + bf.Name + "\"/>");
            //    sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "</td>");
            //}
            //sw.WriteLine("\t" + "\t" + "\t" + "\t" + "</tr>");
            //sw.WriteLine("\t" + "\t" + "\t" + "</table>");
            //sw.WriteLine("\t" + "\t" + "\t" + "<div class=\"subBar\">");
            //sw.WriteLine("\t" + "\t" + "\t" + "\t" + "<ul>");
            //sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<li><div class=\"buttonActive\"><div class=\"buttonContent\"><button type=\"submit\" onclick=\"beforeSubmit()\">检索</button></div></div></li>");
            //sw.WriteLine("\t" + "\t" + "\t" + "\t" + "</ul>");
            //sw.WriteLine("\t" + "\t" + "\t" + "</div>");
            //sw.WriteLine("\t" + "\t" + "</div>");
            //sw.WriteLine("\t" + "</form>");
            //sw.WriteLine("</div>");


            sw.WriteLine("<div class=\"pageContent\">");
            sw.WriteLine("\t" + "<div class=\"panelBar\">");
            sw.WriteLine("\t" + "\t" + "<ul class=\"toolBar\">");

            sw.WriteLine("\t" + "\t" + "\t" + " @Html.Raw(@ViewBag.OperRights)");

            sw.WriteLine("\t" + "\t" + "</ul>");
            sw.WriteLine("\t" + "</div>");
            sw.WriteLine("");
            sw.WriteLine("\t" + "<table class=\"table\" width=\"" + Ifc.fc.ListTotalWidth.ToString() + "\"  layoutH=\"138\">");
            sw.WriteLine("\t" + "\t" + "<thead>");
            sw.WriteLine("\t" + "\t" + "<tr>");
            sw.WriteLine("\t" + "\t" + "<th width=\"50\">序号</th>");
            foreach (bsFuncField dbffield in Ifc.dbFunfields)
            {
                if ((bool)dbffield.ListVisible)
                {
                    sw.WriteLine("\t" + "\t" + "<th width=\"" + dbffield.ListFWidth.ToString() + "\">" + dbffield.FDesp + "</th>");

                }
            }

            sw.WriteLine("\t" + "\t" + "</tr>");
            sw.WriteLine("\t" + "\t" + "</thead>");
            sw.WriteLine("\t" + "\t" + "<tbody>");
            sw.WriteLine("\t" + "\t" + "@{");
            sw.WriteLine("\t" + "\t" + "\t" + "if (Model != null && Model.Count > 0)");
            sw.WriteLine("\t" + "\t" + "\t" + "{");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "int i = 1;");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "foreach (var m in Model)");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "{");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<tr target=\"oid\" rel=\"@m." + Ifc.fc.TPK + "\" > ");
            //循环处理每一个字段
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "<td>@(i++)</td>");

            foreach (bsFuncField dbffield in Ifc.dbFunfields)
            {
                if ((bool)dbffield.ListVisible)
                {

                    if (dbffield.Type.ToLower() == FieldType.Datetime.ToString().ToLower())
                        sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "<td>@(m." + dbffield.FName + ".HasValue ? m." + dbffield.FName + ".Value.ToString(\"yyyy-MM-dd\"): \"\")</td>");
                    else
                        sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "<td>@m." + dbffield.FName + "</td>");


                }
            }

            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "</tr>");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "}");
            sw.WriteLine("\t" + "\t" + "\t" + "}");
            sw.WriteLine("\t" + "\t" + "}");
            sw.WriteLine("\t" + "\t" + "</tbody>");
            sw.WriteLine("\t" + "</table>");
            sw.WriteLine("");
            sw.WriteLine("\t" + "<div class=\"panelBar\">");
            sw.WriteLine("\t" + "\t" + "<div class=\"pages\">");
            sw.WriteLine("\t" + "\t" + "\t" + "<span>显示</span>");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "<select class=\"combox\" name=\"numPerPage\" onchange=\"navTabPageBreak({numPerPage:this.value})\">");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<option value=\"10\">10</option>");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<option value=\"20\">20</option>");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<option value=\"50\">50</option>");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<option value=\"100\">100</option>");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "</select>");
            sw.WriteLine("\t" + "\t" + "\t" + "<span>条，共 @ViewBag.totalCount 条</span>");
            sw.WriteLine("\t" + "\t" + "</div>");
            sw.WriteLine("\t" + "\t" + "<div class=\"pagination\" targetType=\"navTab\" totalCount=\"@ViewBag.totalCount\" numPerPage=\"@ViewBag.numPerPage\" pageNumShown=\"10\" currentPage=\"@ViewBag.pageNum\"></div>");
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

        protected virtual void CreateLookup()
        {
            string strline;
            string CreateFileName = FILEPATHHEAD + Ifc.fc.FunContr + @"\" + ActionType.Lookup.ToString() + ".cshtml";
            BackupFile(CreateFileName);

            FileStream fs = new FileStream(CreateFileName, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, utf8WithBom);
            //使用StreamWriter来往文件中写入内容
            sw.BaseStream.Seek(0, SeekOrigin.Begin);
            sw.WriteLine("@model List<" + DAONAMESPACE + "." + Ifc.fc.TName + ">");
            sw.WriteLine("<form id=\"pagerForm\" method=\"post\" action=\"@Url.Action(\"@Url.Action(" + fullUrl[ActionType.Lookup] + ")\"> ");
            sw.WriteLine("\t" + "<input type=\"hidden\" name=\"funid\" value=\"" + Ifc.fc.FunId.ToString() + "\" />");
            if (Ifc.FuncQuerys != null)
            {
                foreach (bsSysFuncQuery bf in Ifc_Child.FuncQuerys)
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
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "<tr target=\"oid\" rel=\"@m." + Ifc.fc.TPK + "\" > ");
            //循环处理每一个字段
            foreach (bsFuncField dbffield in Ifc_Child.dbFunfields)
            {
                if ((bool)dbffield.ListVisible)
                {
                    sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "<td>@m." + dbffield.FName + "</td>");
                }
            }
            strline = "\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "<td><a class=\"btnSelect\" href=\"javascript:$.bringBack({ ";
            //首先带回主键
            strline += Ifc.fc.TPK + ":'@m." + Ifc.fc.TPK + "',";

            if (Ifc.fc.LookupFields != null && !Ifc.fc.LookupFields.Equals(""))
            {
                string[] backfields = Ifc.fc.LookupFields.Split(new char[] { ',' });
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

        public void CreateJsForTreeRef(StreamWriter sw)
        {

            sw.WriteLine("");
            sw.WriteLine("");

            sw.WriteLine("");
            
            //@*引用ztree的样式表*@
            sw.WriteLine("<link rel=\"stylesheet\" href=\"@Url.Content(\"~/themes/ztree/css/zTreeStyle/zTreeStyle.css\")\" type=\"text/css\"/>");
            //@*引用ztree核心文件*@
            sw.WriteLine("<script type=\"text/javascript\" src=\"@Url.Content(\"~/themes/ztree/js/jquery.ztree.core-3.5.js\")\"></script>");
            //@*引用ztree复选框扩展文件*@
            sw.WriteLine("<script type=\"text/javascript\" src=\"@Url.Content(\"~/themes/ztree/js/jquery.ztree.excheck-3.5.js\")\"></script>");
            //@*引用ztree编辑扩展文件*@
            sw.WriteLine("<script type=\"text/javascript\" src=\"@Url.Content(\"~/themes/ztree/js/jquery.ztree.exedit-3.5.js\")\"></script>");
            //@*引用ztree配置文件*@
            sw.WriteLine("<script type=\"text/javascript\" src=\"@Url.Content(\"~/themes/ztree/js/ztreeDisplay.js\")\"></script>");
            sw.WriteLine("");
            sw.WriteLine("");
        }

        public void CreateJs(ISysFunConf ifc, StreamWriter sw)
        {
            try
            {
                QueryButtonName = ifc.fc.FunContr + "Indexsub";
                sw.WriteLine("");
                sw.WriteLine("");

                //@*后台数据库字段说明：Url:点击之后要打开的路径，相当于a的href； target:连接在什么地方打开，相当于a的target；open:是否展开节点。数据的加载可以在Action里看*@
                sw.WriteLine("<script type=\"text/javascript\">");

                //按下鼠标左键后先执行的事件，需要有返回值，true才可以执行onClick事件，false不执行onClick事件
                sw.WriteLine("\t" + "function beforeClick(treeId, treeNode) {");
                foreach (bsSysFuncQuery fq in ifc.FuncQuerys)
                {
                    if (fq.QueryType == "ChooseSimpTree")
                        sw.WriteLine("\t" + "   $(\"#query_" + ifc.fc.TName + "_" + fq.FFName + "\").val(treeNode.Id);");
                }

                sw.WriteLine("\t" + "    return true;");
                sw.WriteLine("\t" + " }");

                //鼠标点击事件，需要beforeClick返回为true，这两个函数必须写在showTree上面
                sw.WriteLine("\t" + "function onClick(e, treeId, treeNode)");
                sw.WriteLine("\t" + "{");
                sw.WriteLine("\t" + "    //var treeObj = $.fn.zTree.getZTreeObj(\"treeFor" + ifc.fc.FunContr + ifc.fc.PFK + "\");");
                sw.WriteLine("\t" + "    //var nodes = treeObj.getNodes();");
                sw.WriteLine("\t" + "    //if (nodes.length > 0) {");
                sw.WriteLine("\t" + "    //}");
                sw.WriteLine("\t" + "}");

                sw.WriteLine("\t" + "function beforeCheck(treeId, treeNode) {");
                sw.WriteLine("\t" + "    return true;");
                sw.WriteLine("\t" + "}");

                sw.WriteLine("\t" + "function onCheck(e, treeId, treeNode) {");
                sw.WriteLine("\t" + "}");


                //显示树函数，para1是要显示树的元素的ID值；para2为加载树数据的action路径，格式：/Controllers/Actions；para3为是否显示check标志位，check的函数根据需求我再写
                foreach (bsSysFuncQuery fq in ifc.FuncQuerys)
                {
                    if (fq.QueryType == "ChooseSimpTree")
                    {
                        string Url = fq.Url;
                        if (Url.Substring(0, 1) != "/")
                            Url = "/" + fq.Url;
                        sw.WriteLine("\t" + "showTree(\"treeFor" + ifc.fc.FunContr + fq.FFName.Trim() + "\", \"" + Url.Trim() + "/\", false, false, false,false);");
                    }
                    

                }
                sw.WriteLine("</script>");
                sw.WriteLine("");
                sw.WriteLine("");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            
        }
    }

}
