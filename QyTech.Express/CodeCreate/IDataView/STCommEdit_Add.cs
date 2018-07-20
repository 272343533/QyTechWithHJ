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
    public class STCommEdit_Add:IDataViewCreate
    {
        public override void Create(ISysFunConf ifc)
        {
            base.Create(ifc);
            string CreateFileName = FILEPATHHEAD + ifc.fc.FunContr + @"\" + ActionType.Add.ToString() + ".cshtml";
            BackupFile(CreateFileName);

            FileStream fs = new FileStream(CreateFileName, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, utf8WithBom);
            //使用StreamWriter来往文件中写入内容
            sw.BaseStream.Seek(0, SeekOrigin.Begin);
            //把richTextBox1中的内容写入文件
            sw.WriteLine("@model " + DAONAMESPACE + "." + ifc.fc.TName);
            sw.WriteLine("@{");

            sw.WriteLine("\t" + "String url = \"\";");
            sw.WriteLine("\t" + "if (Model != null)  ");
            sw.WriteLine("\t" + "{");
            sw.WriteLine("\t" + "    url = Url.Action(" + fullUrl[ActionType.Edit] + ");");
            sw.WriteLine("\t" + "}");
            sw.WriteLine("\t" + "else");
            sw.WriteLine("\t" + "{");
            sw.WriteLine("\t" + "    url = Url.Action(" + fullUrl[ActionType.Add] + ");");
            sw.WriteLine("\t" + "}");
            sw.WriteLine("}");

       
            sw.WriteLine("<div class=\"pageContent\">");
            sw.WriteLine("\t" + "<form method=\"post\" action=\"@url\" class=\"pageForm required-validate\" onsubmit=\"return validateCallback(this, navTabAjaxDone);\">");

            sw.WriteLine("\t" + "\t" + "<div class=\"pageFormContent\" layoutH=\"55\">");

            CreateEditForm(sw);

            sw.WriteLine("</div>");
            sw.WriteLine("<div class=\"formBar\">");
            sw.WriteLine("	<ul>");
            sw.WriteLine("		@{ if (ViewBag.BrowseFlag == \"false\")");
            sw.WriteLine("		    {");
            sw.WriteLine("		        <li><div class=\"buttonActive\"><div class=\"buttonContent\"><button type=\"submit\">保存</button></div></div></li>");
            sw.WriteLine("		        <li><div class=\"button\"><div class=\"buttonContent\"><button type=\"button\" class=\"close\">取消</button></div></div></li>");
            sw.WriteLine("		    }");
            sw.WriteLine("		    else");
            sw.WriteLine("		    {");
            sw.WriteLine("		        <li><div class=\"button\"><div class=\"buttonContent\"><button type=\"button\" class=\"close\">退出</button></div></div></li>");
            sw.WriteLine("		    }");
            sw.WriteLine("		}");
            sw.WriteLine("	</ul>");
            sw.WriteLine("</div>");
            sw.WriteLine("</form>");
            sw.WriteLine("</div>");
            sw.WriteLine("@if (Model != null)");
            sw.WriteLine("{");
            sw.WriteLine("    @Html.Raw(@ScriptHelper.Obj2InitScript(Model))    ");
            sw.WriteLine("}");

            sw.WriteLine("<script type=\"text/javascript\">");
            sw.WriteLine("    function customvalidXxx(element) {");
            sw.WriteLine("        if ($(element).val() == \"xxx\") return false;");
            sw.WriteLine("        return true;");
            sw.WriteLine("    }");
            sw.WriteLine("</script>");

            sw.Flush();
            //关闭此文件
            sw.Close();
        }

    }
}
