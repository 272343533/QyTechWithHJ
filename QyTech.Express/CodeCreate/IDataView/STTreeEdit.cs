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
    public class STTreeEdit: IDataViewCreate
    {
        public override void Create(ISysFunConf ifc, StreamWriter sw)
        {

            sw.WriteLine("\t" + "<link rel=\"stylesheet\" href=\"@Url.Content(\"~/themes/ztree/css/zTreeStyle/zTreeStyle.css\")\" type=\"text/css\">");
            sw.WriteLine("\t" + "<script type=\"text/javascript\" src=\"@Url.Content(\"~/themes/ztree/js/jquery.ztree.core-3.5.js\")\"></script>");
            sw.WriteLine("\t" + "<script type=\"text/javascript\" src=\"@Url.Content(\"~/themes/ztree/js/jquery.ztree.excheck-3.5.js\")\"></script>");
            sw.WriteLine("\t" + "<script type=\"text/javascript\" src=\"@Url.Content(\"~/themes/ztree/js/jquery.ztree.exedit-3.5.js\")\"></script>");
            sw.WriteLine("\t" + "<script type=\"text/javascript\" src=\"@Url.Content(\"~/themes/ztree/js/ztreeDisplay.js\")\"></script>");
            sw.WriteLine("\t" + "<style type=\"text/css\">");
            sw.WriteLine("\t" + "    #lefttree");
            sw.WriteLine("\t" + "    {");
            sw.WriteLine("\t" + "        float: left;");
            sw.WriteLine("\t" + "    }");
            sw.WriteLine("\t" + "    #rightcontent");
            sw.WriteLine("\t" + "    {");
            sw.WriteLine("\t" + "        position: absolute;");
            sw.WriteLine("\t" + "        left: 20%;");
            sw.WriteLine("\t" + "        float: left;");
            sw.WriteLine("\t" + "    }");
            sw.WriteLine("\t" + "    #btn");
            sw.WriteLine("\t" + "    {");
            sw.WriteLine("\t" + "        text-align: center;");
            sw.WriteLine("\t" + "    }");
            sw.WriteLine("\t" + "</style>");
            sw.WriteLine("\t" + "<div>");
            sw.WriteLine("\t" + "    <div id=\"lefttree\" layouth=\"2\" style=\"width: 440px; border: solid 1px #CCC; overflow: auto\">");
            sw.WriteLine("\t" + "        <div class=\"pageContent\">");
            sw.WriteLine("\t" + "            <div class=\"panelBar\">");
            sw.WriteLine("\t" + "                <ul class=\"toolBar\">");
            sw.WriteLine("\t" + "                    <li><a class=\"edit\"　href=\"javascript:\" onclick=\"showTree('" + Ifc.fc.FunContr + "IndexTree', '/" + Ifc.fc.FunContr + "/ztreeDis', false, false, true, true);\"><span>刷新</span></a></li>");
            foreach (bsSysFuncOper fo in ifc.FuncOpers)
            {
                if (fo.LinkAddr.IndexOf("{ztree") == -1)
                {
                    if (fo.Target=="dialog")
                        sw.WriteLine("\t" + "                    <li><a class=\"" + fo.IconClass + "\" id=\"idFor" + ifc.fc.FunContr + fo.OperName + "\" href=\"" + fo.LinkAddr + "\"  target=\"dialog\"  height=\"" + fo.Height + "\" width=\"" + fo.Width + "\"><span>" + fo.OperName + "</span></a></li>");
                    else if (fo.Target=="navTab")
                        sw.WriteLine("\t" + "                    <li><a class=\"" + fo.IconClass + "\" id=\"idFor" + ifc.fc.FunContr + fo.OperName + "\" href=\"" + fo.LinkAddr + "\"  target=\"navTab\"><span>" + fo.OperName + "</span></a></li>");
                    else if (fo.Target=="ajaxTodo")
                        sw.WriteLine("\t" + "                    <li><a class=\"" + fo.IconClass + "\" id=\"idFor" + ifc.fc.FunContr + fo.OperName + "\" href=\"" + fo.LinkAddr + "\"  target=\"ajaxTodo\"  title=\"" + fo.AjaxTodoTitle + "\"><span>" + fo.OperName + "</span></a></li>");
                }
            }
            sw.WriteLine("\t" + "                </ul>");
            sw.WriteLine("\t" + "            </div>");
            sw.WriteLine("\t" + "            <ul id=\""+Ifc.fc.FunContr+"IndexTree\" class=\"ztree\" layoutH=\"40\"></ul>");
            sw.WriteLine("\t" + "        </div>");
            sw.WriteLine("\t" + "    </div>");
            sw.WriteLine("\t" + "");
            sw.WriteLine("\t" + "    <div id=\"sitePolicy\" class=\"unitBox\" style=\"margin-left: 440px;\">");
            sw.WriteLine("\t" + "        <form method=\"post\"  action=\"@Url.Action(\"Edit\", \"" + ifc.fc.FunContr + "\")\" class=\"pageForm required-validate\" onsubmit=\"return validateCallback(this, navTabAjaxDone);\">");
            sw.WriteLine("\t" + "            <div class=\"pageFormContent\" layouth=\"50\">");

            base.CreateEditForm(sw);

            sw.WriteLine("\t" + "            </div>");
            sw.WriteLine("\t" + "            <div class=\"formBar\">");
            sw.WriteLine("\t" + "                <ul>");
            sw.WriteLine("\t" + "                    <li>");
            sw.WriteLine("\t" + "                        <div class=\"buttonActive\">");
            sw.WriteLine("\t" + "                            <div class=\"buttonContent\">");
            sw.WriteLine("\t" + "                                <button type=\"submit\">保存</button>");
            sw.WriteLine("\t" + "                            </div>");
            sw.WriteLine("\t" + "                        </div>");
            sw.WriteLine("\t" + "                    </li>");
            sw.WriteLine("\t" + "                    <li>");
            sw.WriteLine("\t" + "                        <div class=\"button\">");
            sw.WriteLine("\t" + "                            <div class=\"buttonContent\">");
            sw.WriteLine("\t" + "                                <button type=\"button\" class=\"close\">取消</button>");
            sw.WriteLine("\t" + "                            </div>");
            sw.WriteLine("\t" + "                        </div>");
            sw.WriteLine("\t" + "                    </li>");
            sw.WriteLine("\t" + "                </ul>");
            sw.WriteLine("\t" + "            </div>");
            sw.WriteLine("\t" + "        </form>");

            sw.WriteLine("\t" + "    </div>");
            sw.WriteLine("\t" + "</div>");

            sw.WriteLine("\t" + "");
            sw.WriteLine("\t" + "<script type=\"text/javascript\">");

            sw.WriteLine("\t" + "    function onClick(e, treeId, treeNode) {");
            sw.WriteLine("\t" + "        $.post(\"/" + ifc.fc.FunContr + "/ztreeChooseNode\", { Id: treeNode.Id }, function (data) {");

            foreach (bsFuncField ff in Ifc.dbFunfields)
            {
                if (ff.EditType==EditType.radio.ToString())
                {
                   sw.WriteLine("\t" + "             $(\"input[name='"+ifc.fc.TName+"_"+ff.FName+"'][value='\" + data.Data."+ff.FName+" + \"']\").attr(\"checked\", true);");
                }
                else
                    sw.WriteLine("\t" + "            $(\"#" + ifc.fc.TName + "_" + ff.FName + "\").val(data.Data." + ff.FName + ");");
           
            }
            foreach (bsSysFuncOper fo in ifc.FuncOpers)
            {
                if (fo.LinkAddr.Substring(0,6)!="{ztree")
                  sw.WriteLine("\t" + "            $(\"#idFor"+ifc.fc.FunContr+fo.OperName + "\").attr(\"href\", \"" + fo.LinkAddr + "\"+data.Data."+ifc.fc.TPK+");");
            }
           
            sw.WriteLine("\t" + "        });");
            sw.WriteLine("\t" + "    }");
            sw.WriteLine("\t" + "    function beforeClick() { }");
            sw.WriteLine("\t" + "    function beforeCheck() { }");
            sw.WriteLine("\t" + "    function onCheck() { }");
            sw.WriteLine("\t" + "    showTree(\"" + Ifc.fc.FunContr + "IndexTree\", \"/" + Ifc.fc.FunContr + "/ztreeDis\", false, false, true, true);");
            sw.WriteLine("\t" + "</script>");
        }
        
        public override void Create(ISysFunConf ifc)
        {
            base.Create(ifc);
            CreateFileName = FILEPATHHEAD + ifc.fc.FunContr + @"\" + ActionType.Index.ToString() + ".cshtml";
            base.CreateFileHead(ifc, CreateFileName);

            Create(ifc, sw);

            base.CreateFileEnd();
        }

       
    }
}
