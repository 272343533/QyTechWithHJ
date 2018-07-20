using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunMvcExpress.Core.Models
{
    /// <summary>
    /// 常用工具
    /// </summary>
    public class Toolkit<T>
    {
        /// <summary>
        /// 将一个List对象转化为指定对象一个属性用符号链接的字符串
        /// 作用：可以用在SQL字符串的生成中
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <param name="propertyName">对象特定属性名</param>
        /// <param name="symbol">链接符号</param>
        /// <returns></returns>
        public static String ToSymbolString(List<T> list, String propertyName, String symbol)
        {
            String result = "";
            try
            {
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        result += "'" + item.GetType().GetProperty(propertyName).GetValue(item,null) + "'" + symbol;
                    }
                }
            }
            catch
            {
            }

            return result.Length == 0 ? result : result.Substring(0, result.Length - symbol.Length);
        }
        public static String ToSymbolString(List<T> list, String propertyName, String symbol,bool ToGuid)
        {
            String result = "";
            try
            {
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        if (ToGuid)
                            result += "convert(uniqueidentifier,'" + item.GetType().GetProperty(propertyName).GetValue(item, null) + "')" + symbol;
                        else
                            result += "'" + item.GetType().GetProperty(propertyName).GetValue(item, null) + "'" + symbol;
                    }
                }
            }
            catch
            {
            }

            return result.Length == 0 ? result : result.Substring(0, result.Length - symbol.Length);
        }
        /// <summary>
        /// 将一个List对象转化为指定对象一个属性用符号链接的字符串
        /// 作用：可以用在SQL字符串的生成中(没有单引号)
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <param name="propertyName">对象特定属性名</param>
        /// <param name="symbol">链接符号</param>
        /// <returns></returns>
        public static String ToSymbolStringWithNoQuo(List<T> list, String propertyName, String symbol)
        {
            String result = "";
            try
            {
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        result += "" + item.GetType().GetProperty(propertyName).GetValue(item, null) + symbol;
                    }
                }
            }
            catch
            {
            }

            return result.Length == 0 ? result : result.Substring(0, result.Length - symbol.Length);
        }

        /// <summary>
        /// 作用：返回对象集合的中某个字段的List集合
        /// </summary>
        /// <param name="list"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static List<TR> ToArray<TR>(List<T> list, String propertyName)
        { 
            List<TR> target = new List<TR>();
            try 
	        {
                if (list != null && list.Count >0)
                {
                    foreach (var item in list)
                    {
                        target.Add((TR)(item.GetType().GetProperty(propertyName).GetValue(item, null)));
                    }
                }
	        }
	        catch 
	        {
	        }

            return target;
        }

        /// <summary>
        /// 字符串劈成指定类型的List集合
        /// </summary>
        /// <param name="str"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static List<int> ToArray(string str, char[] symbol)
        {
            string[] strlist = str.Split(symbol);

            List<int> list = new List<int>();
            if (strlist != null && strlist.Length > 0)
            {
                foreach (var item in strlist)
                {
                    list.Add(Convert.ToInt32(item));
                }
            }

            return list;
        }
    }
}