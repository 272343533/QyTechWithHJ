using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using SunMvcExpress.Dao;
using log4net;
using SunMvcExpress.Core.BLL;
namespace System.Web.Mvc
{
    public static class ScriptHelper
    {
        public static ILog log = log4net.LogManager.GetLogger("ScriptHelper");

    
        /// <summary>
        /// 应该考虑form的显示方式，如checkbox，combox等，
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static String Obj2InitScript<T>(T t, bool WithTableName=true)
        {
            String result = @"<script type='text/javascript'>";
            string viewItemName = "";
            try
            {
                var list = t.GetType().GetProperties().ToList();
                String tablename = t.GetType().Name;
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        if (WithTableName)
                            viewItemName = tablename + "_" + item.Name;
                        else
                            viewItemName = item.Name;
                        if (item.PropertyType.ToString().Contains("System.") && (item.PropertyType.ToString().Contains("Nullable") || item.PropertyType.ToString().Split('.').Length == 2))
                        {
                            if (item.PropertyType.ToString().Contains("DateTime"))
                            {
                                if (item.GetValue(t, null) != null)
                                {
                                    int? systypeid = EntityManager<int>.GetBySql("select convert(int,a.system_type_id) as intData  from sys.columns a inner join sysobjects c on a.object_Id=c.id and c.[name]='" + tablename + "' and a.Name='" + item.Name + "'");
            
                                    if (Convert.ToInt32(systypeid) != 40)
                                    {
                                        result += "$($('[name=" + viewItemName + "]')[0]).val('" + (Convert.ToDateTime(item.GetValue(t, null))).ToString("yyyy-MM-dd HH:mm:ss") + "');";
                                    }
                                    else
                                    {
                                        result += "$($('[name=" + viewItemName + "]')[0]).val('" + (Convert.ToDateTime(item.GetValue(t, null))).ToString("yyyy-MM-dd") + "');";
                                    }
                                }
                            }
                            else if (item.PropertyType.ToString().Contains("Boolean"))
                            {
                                result += "$($(\"input[name='" + viewItemName + "'][value=" + item.GetValue(t, null) + "]\")).attr(\"checked\",true);";
                            }
                            else
                            {
                                result += "$($('[name=" + viewItemName + "]')[0]).val('" + item.GetValue(t, null) + "');";
                            }
                        }

                        //test
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "\\log.txt";
                        //if (!File.Exists(path))
                        //{
                        //    using (File.Create(path)) { }
                        //}
                        //File.AppendAllText(path, item.Name + "\t\r");
                        //File.AppendAllText(path, item.PropertyType.ToString() + "\t\r");

                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Obj2InitScript:" + ex.Message);
            }

            return result + @"</script>";
        }


        ///////// <summary>
        ///////// 用于查看页面的初始化脚本
        ///////// </summary>
        ///////// <typeparam name="T"></typeparam>
        ///////// <param name="t"></param>
        ///////// <param name="IsDetail">是否为查看页面的标志</param>
        ///////// <returns></returns>
        //////public static String Obj2InitScript<T>(T t, bool IsDetail, bool WithTableName = true)
        //////{
        //////    StringBuilder sb = new StringBuilder();
        //////    sb.Append(@"<script type='text/javascript'>");

        //////    try
        //////    {
        //////        var list = t.GetType().GetProperties().ToList();
        //////        String tablename = t.GetType().Name;
        //////        if (list != null && list.Count > 0)
        //////        {
        //////            foreach (var item in list)
        //////            {
        //////                if (item.PropertyType.ToString().Contains("System.") && (item.PropertyType.ToString().Contains("Nullable") || item.PropertyType.ToString().Split('.').Length == 2))
        //////                {
        //////                    if (item.PropertyType.ToString().Contains("DateTime"))
        //////                    {
        //////                        if (item.GetValue(t, null) != null)
        //////                        {
        //////                            sb.Append("$($('[name=" + item.Name + "]')[0]).val('" + (Convert.ToDateTime(item.GetValue(t, null))).ToString("yyyy/MM/dd") + "');");
        //////                        }

        //////                    }
        //////                    else
        //////                    {
        //////                        sb.Append("$($('[name=" + item.Name + "]')[0]).val('" + item.GetValue(t, null) + "');");
        //////                    }

        //////                }
        //////            }
        //////        }
        //////    }
        //////    catch
        //////    {
        //////    }

        //////    if (IsDetail == true)
        //////    {
        //////        //网页的查看初始化脚本
        //////        sb.Append(@"$('dd input,.hide-input, dd textarea').each(function(){if($(this).attr('type') != 'hidden'){var txt = $(this).val();$(this).hide().parent().append($('<div>'+ txt +'</div>').css({'text-align':'left','line-height':'21px'}));}});");

        //////        sb.Append(@"$('dd select').each(function(){var txt = $(this).children('[selected=selected]').text();$(this).hide().parent().append($('<div>'+ txt +'</div>').css({'text-align':'left','line-height':'21px'}));});");
        //////        sb.Append(@"$('dd a').hide();");
        //////    }

        //////    sb.Append(@"</script>");
        //////    return sb.ToString();
        //////}
        /// <summary>
        /// 时间格式化，页面显示
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static String DateTimeFormating(DateTime? date)
        {
            String str = "";
            if (date != null)
            {
                str = date.Value.ToString("yyyy-MM-dd");
            }
            return str;
        }



    }
}