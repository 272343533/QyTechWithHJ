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

namespace QyTech.Express.CodeCreate.IQueryView
{
    public class EditCombox : IQueryViewCreate
    {
        public override void Create(bsSysFuncQuery fq, StreamWriter sw)
        {
            //sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + fq.FFDesp + "：<input type=\"text\" id=\"query_" + fq.FFName + "\" name=\"query_" + fq.FFName + "\" />");

            //sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "			<td>");
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "\t" + fq.FFDesp + "： <select class=\"required\" name=\"query_" + fq.FFName.Trim() + "\">");
            if (fq.Url.Length > 0)
            {
                if (fq.Url.Substring(0, 1) == "{")
                {
                    string[] splits = fq.Url.Split(new char[] { '{', '}', ';' });
                    foreach (string s1 in splits)
                    {
                        if (s1.Trim()=="")
                            continue;
                        if (s1.IndexOf(',') < 0)
                        {
                            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "			        <option value=\"" + s1 + "\" @(ViewBag." + fq.FFName + " ==\"" + s1 + "\"? \"selected\" : \"\")>" + s1 + "</option>");
                        }
                        else
                        {
                            string[] s2 = s1.Split(new char[] { ',' });
                            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "			        <option value=\"" + s2[0] + "\" @(ViewBag." + fq.FFName + " ==\"" + s2[0] + "\"? \"selected\" : \"\")>" + s2[1] + "</option>");
                        }
                    }
                }
                else
                {
                    //要获取对应的接口对象
                    try
                    {
                        string[] interfs = fq.Url.Split(new char[] { '(', '.' });
                        bsSysFunc fc = EntityManager<bsSysFunc>.GetBySql<bsSysFunc>("FunContr='" + interfs[0].Substring(0, interfs[0].Length - 3) + "'");
                        bsSysFuncInterface fi = EntityManager<bsSysFuncInterface>.GetBySql<bsSysFuncInterface>("FcId='" + fc.FcId.ToString() + "' and InterfName='" + fq.Url.Split(new char[] { '(', '.' })[1] + "'");

                        sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "			    @foreach (SunMvcExpress.Dao." + fi.RetObject + " vbi in ViewBag.vb" + fq.FFName + "s as List<" + fi.RetObject + ">)");
                        sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "			    {");
                        sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "			        <option value=\"@vbi." + fq.FFName + "\" @(detaobj." + fq.FFName + " ==@vbi." + fq.FFName + "? \"selected\" : \"\")>@vbi." + fq.FFName + "</option>");
                        sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "			    }");
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                    }
                }
            }
            sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "			    </select>");
            //sw.WriteLine("\t" + "\t" + "\t" + "\t" + "\t" + "\t" + "			</td>");

        }
    }
}
