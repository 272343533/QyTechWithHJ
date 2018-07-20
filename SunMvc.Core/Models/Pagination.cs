using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunMvcExpress.Dao;
using SunMvcExpress.Core.Models;
using System.Data.Objects.DataClasses;
using System.Data.Objects;
using System.Data.Entity;
using log4net;



namespace SunMvcExpress.Core.Models
{
    /// <summary>
    /// 分页类
    /// </summary>
    internal class Pagination<T>
    {
        public  static ILog log = log4net.LogManager.GetLogger("Pagination");
    
        /// <summary>
        /// 获取对象集合的分页数据
        /// </summary>
        /// <param name="condition">查询条件，格式： "name='zhangsan' and age=22"</param>
        /// <param name="orderby">排序字段，格式： "pub_date"</param>
        /// <param name="order">排序方式，OrderMothed枚举</param>
        /// <param name="total">总页数</param>
        /// <returns>特定页的数据</returns>
        public static List<T> Paging(ObjectContext db, string condition, string orderby, OrderMethod order, int PageSize, int PageIndex, out int total)
        {
            //获取真实类型名
            string tablename = typeof(T).Name;
            
            //查询串拼接
            string CommandText = "select * from " + tablename;
            if (condition != null && !condition.Equals(""))
            {
                CommandText += " where " + condition;
            }
            if (orderby != null && !orderby.Equals(""))
            {
                CommandText += " order by " + orderby;
                if (order == OrderMethod.DESC)
                {
                    CommandText += " desc";
                }
                else
                {
                    CommandText += " asc";
                }
            }


            try
            {

                String entityName = db.GetType().GetProperties().Where(p => p.Name == tablename || p.Name == tablename + "s" || p.Name == tablename + "es").Single().Name;

                var list = db.ExecuteStoreQuery<T>(CommandText, entityName, System.Data.Objects.MergeOption.AppendOnly, null)
                    .ToList();

                total = list.Count % PageSize == 0 ? (list.Count / PageSize) : (list.Count / PageSize + 1);

                return list.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList<T>();
            }
            catch
            {
                total = 0;
                return null;
            }
        }

        /// <summary>
        /// 获取对象集合的分页数据
        /// </summary>
        /// <param name="condition">查询条件，格式： "name='zhangsan' and age=22"</param>
        /// <param name="orderby">排序字段，格式： "pub_date"</param>
        /// <param name="order">排序方式，OrderMothed枚举</param>
        /// <returns>特定页的数据</returns>
        public static List<T> Paging(ObjectContext db, string condition, string orderby, OrderMethod order, int PageSize, int PageIndex)
        {
            //获取真实类型名
            string tablename = typeof(T).Name;

            //查询串拼接
            string CommandText = "select * from " + tablename;
            if (condition != null && !condition.Equals(""))
            {
                CommandText += " where " + condition;
            }
            if (orderby != null && !orderby.Equals(""))
            {
                CommandText += " order by " + orderby;
                if (order == OrderMethod.DESC)
                {
                    CommandText += " desc";
                }
                else
                {
                    CommandText += " asc";
                }
            }

            try
            {

                String entityName = db.GetType().GetProperties().Where(p => p.Name == tablename || p.Name == tablename + "s" || p.Name == tablename + "es").Single().Name;
                var list = db.ExecuteStoreQuery<T>(CommandText, entityName, System.Data.Objects.MergeOption.AppendOnly, null)
                    .ToList();

                return list.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList<T>();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取对象集合的分页数据
        /// </summary>
        /// <param name="condition">查询条件，格式： "name='zhangsan' and age=22"</param>
        /// <param name="orderby">排序字段，格式： "pub_date"</param>
        /// <param name="order">排序方式，OrderMothed枚举</param>
        /// <param name="total">z总共的条目</param>
        /// <returns>特定页的数据</returns>
        public static List<T> PagingWithCount(ObjectContext db,string condition, string orderby, OrderMethod order, int PageSize, int PageIndex, out int totalCount)
        {
            //获取真实类型名
            string tablename = typeof(T).Name;

            //查询串拼接
            string CommandText = "select * from " + tablename;
            if (condition != null && !condition.Equals(""))
            {
                CommandText += " where " + condition;
            }
            if (orderby != null && !orderby.Equals(""))
            {
                CommandText += " order by " + orderby;
                if (order == OrderMethod.DESC)
                {
                    CommandText += " desc";
                }
                else
                {
                    CommandText += " asc";
                }
            }


            try
            {

                String entityName = db.GetType().GetProperties().Where(p => p.Name == tablename || p.Name == tablename + "s" || p.Name == tablename + "es" || p.Name == tablename.Substring(0, tablename.Length - 1) + "ies").Single().Name;

                var list = db.ExecuteStoreQuery<T>(CommandText, entityName, System.Data.Objects.MergeOption.AppendOnly, null)
                    .ToList();

                totalCount = list.Count;

                return list.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList<T>();
            }
            catch
            {
                totalCount = 0;
                return null;
            }
        }

        /// <summary>
        /// 获取对象集合的分页数据
        /// </summary>
        /// <param name="condition">查询条件，格式： "name='zhangsan' and age=22"</param>
        /// <param name="orderby">排序字段，格式： "pub_date"</param>
        /// <param name="order">排序方式，OrderMothed枚举</param>
        /// <param name="total">z总共的条目</param>
        /// <returns>特定页的数据</returns>
        public static List<T> PagingWithCount(ObjectContext db, string condition, string orderbys, int PageSize, int PageIndex, out int totalCount)
        {
            //获取真实类型名
            string tablename = typeof(T).Name;

            //查询串拼接
            string CommandText = "select * from " + tablename;;
            if (condition != null && !condition.Trim().Equals(""))
            {
                CommandText +=  " where "+condition;//+" and ";
            }
            
            //CommandText += " isnull(datastatus,0) <> 1 ";
            if (orderbys != null && !orderbys.Equals(""))
            {
                CommandText += " order by " + orderbys;
            }

            try
            {
                String entityName = db.GetType().GetProperties().Where(p => p.Name == tablename || p.Name == tablename + "s" || p.Name == tablename + "es" || p.Name==tablename.Substring(0,tablename.Length-1)+"ies").Single().Name;

                var list = db.ExecuteStoreQuery<T>(CommandText, entityName, System.Data.Objects.MergeOption.AppendOnly,null)
                       .ToList();

                totalCount = list.Count;
                
                return list.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList<T>();
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                log.Error(ex.Message);
                totalCount = 0;
                return null;
            }
        }


        public static List<T> SelectAll(ObjectContext db, string condition, string orderbys)
        {
            //获取真实类型名
            string tablename = typeof(T).Name;

            //查询串拼接
            string CommandText = "select * from " + tablename; ;
            if (condition != null && !condition.Trim().Equals(""))
            {
                CommandText += " where " + condition;//+" and ";
            }

            //CommandText += " isnull(datastatus,0) <> 1 ";
            if (orderbys != null && !orderbys.Equals(""))
            {
                CommandText += " order by " + orderbys;
            }


            try
            {

                String entityName = db.GetType().GetProperties().Where(p => p.Name == tablename || p.Name == tablename + "s" || p.Name == tablename + "es"|| p.Name==tablename.Substring(0,tablename.Length-1)+"ies").Single().Name;

                var list = db.ExecuteStoreQuery<T>(CommandText, entityName, System.Data.Objects.MergeOption.AppendOnly, null)
                       .ToList();
                return list.ToList<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public enum OrderMethod
    {
        ASC,
        DESC
    }
}