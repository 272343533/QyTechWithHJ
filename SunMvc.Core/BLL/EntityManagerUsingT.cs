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
using QyTech.Core;


namespace SunMvcExpress.Core.BLL
{


 
    /// <summary>
    /// 实体操作类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class EntityManagerUsingT
    {
        private static ILog log = LogManager.GetLogger("EntityManager");
        static String entityName = "";


        public static int GetBySql(string sql)
        {
            try
            {
                using (var db = new WSExpressEntities())
                {

                    List<retObject> objs = db.ExecuteStoreQuery<retObject>(sql).ToList<retObject>();
                    return objs[0].intData;
                }
            }
            catch (Exception ex)
            {
                string errmsg =  ExceptionMessage.Parse(ex);
                LogHelper.Error(ex);
                return -9999;
            }
        }

        public static void SaveToDb()
        {
            using (var db= new WSExpressEntities())
            {
                db.SaveChanges();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="paramvalues"> 都好分割的参数值</param>
        public static int ExecuteSql(string sql)
        {
            try
            {
                using (var db = new WSExpressEntities())
                {
                    return db.ExecuteStoreCommand(sql);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return -1;
            }
        }

        public static List<T> GetAllByStorProcedure<T>(string spName, string paramvalues)
        {
            try
            {
                using (var db = new WSExpressEntities())
                {
                    List<T> obj = db.ExecuteStoreQuery<T>("exec " + spName + " " + paramvalues).ToList<T>();
                    return obj;
                }
            }
            catch (Exception ex)
            {
                string err = entityName + ":" + ex.Message;
                if (ex.InnerException != null)
                    err += " detail:" + ex.InnerException.Message;
                LogHelper.Error(ex);
                return new List<T>();
            }
        }
        public static List<T> GetPagingByStorProcedure<T>(string spName, string paramvalues, int pageNum, int numPerPage, out int totalCount)
        {
            totalCount = 0;
            try
            {
                using (var db = new WSExpressEntities())
                {
                    List<T> list = db.ExecuteStoreQuery<T>("exec " + spName + " " + paramvalues).ToList<T>();
                    totalCount = list.Count;

                    return list.Skip((pageNum - 1) * numPerPage).Take(numPerPage).ToList<T>();
                }
            }
            catch (Exception ex)
            {
                string err = entityName + ":" + ex.Message;
                if (ex.InnerException != null)
                    err += " detail:" + ex.InnerException.Message;
                LogHelper.Error(ex);
                return new List<T>();
            }
        }
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="t">要添加的实体对象</param>
        /// <returns></returns>
        public static string Add<T>(T t)
        {
            try
            {
                using (var db = new WSExpressEntities())
                {
                    entityName = db.GetType().GetProperties().Where(p => p.Name == t.GetType().Name || p.Name == t.GetType().Name + "s" || p.Name == t.GetType().Name + "es").Single().Name;
                    db.AddObject(entityName, t);
                    db.SaveChanges();
                    return "";
                }
            }
            catch (Exception ex)
            {
                string errmsg =  ExceptionMessage.Parse(ex);
                LogHelper.Error(ex);
                return errmsg;
            }
        }

      
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="t">要添加的实体对象</param>
        /// <returns></returns>
        public static T AddReturnEntity<T>( T t, out string errmsg)
        {
            errmsg = "";
            try
            {
                using (var db = new WSExpressEntities())
                {
                    entityName = db.GetType().GetProperties().Where(p => p.Name == t.GetType().Name || p.Name == t.GetType().Name + "s" || p.Name == t.GetType().Name + "es").Single().Name;
                    db.AddObject(entityName, t);
                    db.SaveChanges();
                    return t;
                }
            }
            catch (Exception ex)
            {
                errmsg =  ExceptionMessage.Parse(ex);
                LogHelper.Error(ex);

                return default(T);
            }
        }

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string Modify<T>( T t) where T : EntityObject
        {
            try
            {
                using (var db = new WSExpressEntities())
                {
                    entityName = db.GetType().GetProperties().Where(p => p.Name == t.GetType().Name || p.Name == t.GetType().Name + "s" || p.Name == t.GetType().Name + "es").Single().Name;
                    int i=db.UpdateEntity(t, entityName); //自定更新扩展方法
                    db.SaveChanges();

                    return "";
                }
            }
            catch (Exception ex)
            {
                string errmsg =  ExceptionMessage.Parse(ex);
                LogHelper.Error(ex);
                return errmsg;
            }
        }

    
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string Delete<T>( T t)
        {
            try
            {
                using (var db = new WSExpressEntities())
                {
                    db.DeleteObject(t);
                    db.SaveChanges();
                    return "";
                }
            }
            catch (Exception ex)
            {
                string errmsg =  ExceptionMessage.Parse(ex);
                LogHelper.Error(ex);
                return errmsg;
            }
        }
        public static string Delete<T>(string PkName, Guid PkValue) where T : EntityObject
        {
            try
            {
                string tablename = typeof(T).Name;
                string CommandText = "delete  from " + tablename + " where " + PkName + "='" + PkValue.ToString() + "'";

                int i=EntityManager<T>.ExecuteSql(CommandText);

                return "";
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return ex.Message;
            }
        }

        public static string Delete<T>(string PkName, int PkValue) where T : EntityObject
        {
            try
            {
                string tablename = typeof(T).Name;
                string CommandText = "delete  from " + tablename + " where " + PkName + "=" + PkValue.ToString();

                int i = EntityManager<T>.ExecuteSql(CommandText);

                return "";
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return ex.Message;
            }
        }
       
        /// <summary>
        /// 一般的查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userID"></param>
        /// <param name="query_types"></param>
        /// <param name="query_keys"></param>
        /// <param name="query_values"></param>
        /// <param name="query_operators"></param>
        /// <param name="conditions"></param>
        /// <param name="orderbys"></param>
        /// <param name="orderField"></param>
        /// <param name="dire"></param>
        /// <param name="pageNum"></param>
        /// <param name="numPerPage"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        private static List<T> Paging<T>(Guid userID, string filter,string query_types, string query_keys, string query_values, string query_operators, string conditions,string orderbys, string orderField, OrderMethod dire, int pageNum, int numPerPage, out int totalCount) where T : class,new()
        {
            try
            {
                string sql = " ";
                if (conditions == null)
                {
                    if (query_keys != null)
                    {
                        string[] keys = query_keys.Split(new char[] { ',' });
                        string[] values = query_values.Split(new char[] { ',' });
                        string[] operators = query_operators.Split(new char[] { ',' });
                        for (int i = 0; i < keys.Length; i++)
                        {
                            if (!values[i].Equals(""))
                            {
                                try
                                {
                                    if (operators[i].ToLower() == "like")
                                    {
                                        sql += keys[i] + " like '%" + values[i] + "%'  and";
                                    }
                                    else
                                    {
                                        sql += keys[i] + operators[i] + values[i] + " and ";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogHelper.Error("Entity Manager Pageing:", ex.Message);
                                    sql += keys[i] + " like '%" + values[i] + "%'  and";
                                }
                            }

                        }
                    }
                }
                else
                {
                    sql = conditions + " and ";
                }
                //String idset = Toolkit<bsOrganize>.ToSymbolString(bsUserManager.GetEmpDataRighs(userID), "id", ",");

                //if (idset != "")
                //{
                //    idset = idset.Equals("") ? "'" + Guid.Empty.ToString() + "'" : idset + ",'" + Guid.Empty.ToString() + "'";
                //    sql += "OrgId in (" + idset + ")";
                //}
                //else
                //{
                //    if (sql.Length > 4)
                //        sql = sql.Substring(0, sql.Length - 4);
                //}

                if (orderbys == null || orderbys.Equals(""))
                {
                    if (orderField != null && !orderField.Equals(""))
                    {
                        orderbys += orderField;
                        if (dire == OrderMethod.DESC)
                        {
                            orderbys += " desc";
                        }
                        else
                        {
                            orderbys += " asc";
                        }
                    }
                }
                List<T> ret = GetGirdListwithPaging<T>(sql, orderbys, pageNum, numPerPage, out totalCount);
                return ret;
            }
            catch (Exception ex)
            {
                string errmsg =  ExceptionMessage.Parse(ex);
                LogHelper.Error(ex);
                totalCount=0;
                return new List<T>();
            }
        }
        /// <summary>
        /// 包含extend查询，需要代入外键
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userID"></param>
        /// <param name="fkcondtion">外键,格式"key='value'"</param>
        /// <param name="filter"></param>
        /// <param name="query_types"></param>
        /// <param name="query_keys"></param>
        /// <param name="query_values"></param>
        /// <param name="query_operators"></param>
        /// <param name="conditions"></param>
        /// <param name="orderbys"></param>
        /// <param name="orderField"></param>
        /// <param name="dire"></param>
        /// <param name="pageNum"></param>
        /// <param name="numPerPage"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static List<T> Paging<T>(Guid userID, string fkcondtion,string filter, string query_types, string query_keys, string query_values, string query_operators, string conditions, string orderbys, string orderField, OrderMethod dire, int pageNum, int numPerPage, out int totalCount) where T : class,new()
        {
            try
            {
                string sql = " ";
                if (conditions == null)
                {
                    if (query_keys != null)
                    {
                        string[] keys = query_keys.Split(new char[] { ',' });
                        string[] values = query_values.Split(new char[] { ',' });
                        string[] operators = query_operators.Split(new char[] { ',' });
                        for (int i = 0; i < keys.Length; i++)
                        {
                            if (!values[i].Equals(""))
                            {
                                try
                                {
                                    if (operators[i].ToLower() == "like")
                                    {
                                        sql += keys[i] + " like '%" + values[i] + "%'  and";
                                    }
                                    else
                                    {
                                        sql += keys[i] + operators[i] + values[i] + " and ";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogHelper.Error(ex);
                                    sql += keys[i] + " like '%" + values[i] + "%'  and";
                                }
                            }

                        }
                    }
                }
                else
                {
                    if (!conditions.Equals(""))
                        sql = conditions + " and ";
                }
                if ((filter != null) && (!filter.Equals("")))
                    sql += filter + " and ";
                if (fkcondtion != null && !fkcondtion.Equals(""))
                    sql += fkcondtion + " and ";
                //String idset = Toolkit<bsOrganize>.ToSymbolString(bsUserManager.GetEmpDataRighs(userID), "id", ",");

                //if (idset != "")
                //{
                //    idset = idset.Equals("") ? "'" + Guid.Empty.ToString() + "'" : idset + ",'" + Guid.Empty.ToString() + "'";
                //    sql += "OrgId in (" + idset + ")";
                //}
                //else
                //{
                   if (sql.Length > 4)
                       sql = sql.Substring(0, sql.Length - 4);
               // }

                if (orderbys == null || orderbys.Equals(""))
                {
                    if (orderField != null && !orderField.Equals(""))
                    {
                        orderbys += orderField;
                        if (dire == OrderMethod.DESC)
                        {
                            orderbys += " desc";
                        }
                        else
                        {
                            orderbys += " asc";
                        }
                    }
                }
                List<T> ret = GetGirdListwithPaging<T>(sql, orderbys, pageNum, numPerPage, out totalCount);
                return ret;
            }
            catch (Exception ex)
            {
                string errmsg =  ExceptionMessage.Parse(ex);
                LogHelper.Error(ex);
                totalCount = 0;
                return new List<T>();
            }
        }

        public static List<T> Paging<T>(Guid? userID,string conditions, string orderbys, int pageNum, int numPerPage, out int totalCount) where T : class,new()
        {
            try
            {
                string sql = " ";
                sql = conditions + " and ";

                String idset = "";
                //if (userID != null)
                //{
                //    idset = Toolkit<bsOrganize>.ToSymbolString(bsUserManager.GetEmpDataRighs((Guid)userID), "id", ",");
                //}
                //if (idset != "")
                //{
                //    idset = idset.Equals("") ? "'" + Guid.Empty.ToString() + "'" : idset + ",'" + Guid.Empty.ToString() + "'";
                //    sql += "OrgId in (" + idset + ")";
                //}
                //else
                //{
                if (sql.Length > 4)
                    sql = sql.Substring(0, sql.Length - 4);
                //}

                List<T> ret = GetGirdListwithPaging<T>(sql, orderbys, pageNum, numPerPage, out totalCount);
                return ret;
            }
            catch (Exception ex)
            {
                string errmsg =  ExceptionMessage.Parse(ex);
                LogHelper.Error(ex);
                totalCount = 0;
                return null;
            }
        }


        public static List<T> Paging<T>(string conditions, string orderbys, int pageNum, int numPerPage, out int totalCount) where T : class,new()
        {
            try
            {
                List<T> ret = GetGirdListwithPaging<T>(conditions, orderbys, pageNum, numPerPage, out totalCount);
                return ret;
            }
            catch (Exception ex)
            {
                string errmsg =  ExceptionMessage.Parse(ex);
                LogHelper.Error(ex);
                totalCount = 0;
                return null;
            }
        }

        public static T GetByGuidPk<T>(string PkName, Guid PkValue)
        {
            try
            {
                string tablename = typeof(T).Name;
                string CommandText = "select * from " + tablename + " where " + PkName + "='" + PkValue.ToString() + "'";

                using (var db = new WSExpressEntities())
                {

                    String entityName = db.GetType().GetProperties().Where(p => p.Name == tablename || p.Name == tablename + "s" || p.Name == tablename + "es").Single().Name;
                    var obj = db.ExecuteStoreQuery<T>(CommandText, entityName, System.Data.Objects.MergeOption.AppendOnly, null)
                           .SingleOrDefault<T>();
                    return obj;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return default(T);
            }
        }

        public static T GetByGuidPkWithMaxDt<T>(string PkName, Guid PkValue,string orderby)
        {
            try
            {
                string tablename = typeof(T).Name;
                string CommandText = "select top 1 * from " + tablename + " where " + PkName + "='" + PkValue.ToString() + "' order by "+ orderby;

                using (var db = new WSExpressEntities())
                {

                    String entityName = db.GetType().GetProperties().Where(p => p.Name == tablename || p.Name == tablename + "s" || p.Name == tablename + "es").Single().Name;
                    var obj = db.ExecuteStoreQuery<T>(CommandText, entityName, System.Data.Objects.MergeOption.AppendOnly, null)
                           .SingleOrDefault<T>();
                    return obj;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return default(T);
            }
        }

        public static T GetByPk<T>( string PkName, Guid PkValue) where T:EntityObject
        {
            try
            {
                string tablename = typeof(T).Name;
                string CommandText = "select * from " + tablename + " where " + PkName + "='" + PkValue.ToString() + "'";

                using (var db = new WSExpressEntities())
                {
                  
                    String entityName = db.GetType().GetProperties().Where(p => p.Name == tablename || p.Name == tablename + "s" || p.Name == tablename + "es").Single().Name;
                    var obj = db.ExecuteStoreQuery<T>(CommandText, entityName, System.Data.Objects.MergeOption.AppendOnly, null)
                           .SingleOrDefault<T>();
                    return obj;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return default(T);
            }
        }
        public static T GetByPk<T>( string PkName, string PkValue)
        {
            try
            {
                string tablename = typeof(T).Name;
                string CommandText = "select * from " + tablename + " where " + PkName + "='" + PkValue.ToString() + "'";

                using (var db = new WSExpressEntities())
                {
                    String entityName = db.GetType().GetProperties().Where(p => p.Name == tablename || p.Name == tablename + "s" || p.Name == tablename + "es").Single().Name;

               
                    var obj = db.ExecuteStoreQuery<T>(CommandText, entityName, System.Data.Objects.MergeOption.AppendOnly, null)
                           .SingleOrDefault<T>();
                    return obj;
                }
            }
            catch (Exception ex)
            {
                string errmsg =  ExceptionMessage.Parse(ex);
                LogHelper.Error(ex);
                return default(T);
            }
        }
        public static T GetByPk<T>( string PkName, int PkValue)
        {
            try
            {
                string tablename = typeof(T).Name;
                string CommandText = "select * from " + tablename + " where " + PkName + "=" + PkValue.ToString();

                using (var db = new WSExpressEntities())
                {
                    String entityName = db.GetType().GetProperties().Where(p => p.Name == tablename || p.Name == tablename + "s" || p.Name == tablename + "es").Single().Name;

                    var obj = db.ExecuteStoreQuery<T>(CommandText, entityName, System.Data.Objects.MergeOption.AppendOnly, null)
                           .SingleOrDefault<T>();
                    return obj;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return default(T);
            }
        }

        public static T GetByStringFieldName<T>( string FName, string FValue)
        {
            try
            {
                string tablename = typeof(T).Name;
                string CommandText = "select * from " + tablename + " where " + FName + "='" + FValue.ToString() + "'";

                using (var db = new WSExpressEntities())
                {
                    String entityName = db.GetType().GetProperties().Where(p => p.Name == tablename || p.Name == tablename + "s" || p.Name == tablename + "es").Single().Name;

                    var obj = db.ExecuteStoreQuery<T>(CommandText, entityName, System.Data.Objects.MergeOption.AppendOnly, null)
                           .SingleOrDefault<T>();
                    return obj;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return default(T);
            }
        }

        public static T GetBySql<T>( string conditionSql) where T:EntityObject 
        {
            try
            {
                string tablename = typeof(T).Name;
                string CommandText = "select * from [" + tablename + "] where " + conditionSql;

                using (var db = new WSExpressEntities())
                {
                    String entityName = db.GetType().GetProperties().Where(p => p.Name == tablename || p.Name == tablename + "s" || p.Name == tablename + "es").Single().Name;

                    var obj = db.ExecuteStoreQuery<T>(CommandText, entityName, System.Data.Objects.MergeOption.AppendOnly, null)
                           .SingleOrDefault<T>();
                    return obj;
                }
            }
            catch (Exception ex)
            {
                string errmsg =  ExceptionMessage.Parse(ex);
                LogHelper.Error(ex);
                return default(T);
            }
        }

        public static T GetBySql<T>(string TName, string conditionSql) where T:EntityObject 
        {
            int posindex = 100;
            try
            {
                string CommandText = "select * from [" + TName + "] where " + conditionSql;
                posindex = 200;
                using (var db = new WSExpressEntities())
                {
                    //String entityName = db.GetType().GetProperties().Where(p => p.Name == TName || p.Name == TName + "s" || p.Name == TName + "es").Single().Name;
                    posindex=300;
                    var obj = db.ExecuteStoreQuery<T>(CommandText, TName, System.Data.Objects.MergeOption.AppendOnly, null)
                           .SingleOrDefault<T>();
                    posindex=500;
                    return obj;
                }
            }
            catch (Exception ex)
            {
                string errmsg =  ExceptionMessage.Parse(ex);
                LogHelper.Info("GetBySql", posindex.ToString());
                LogHelper.Error(ex);
                return default(T);
            }
        }

        public static List<T> GetGirdListwithPaging<T>( string conditions, string orderbys, int pageNum, int numPerPage, out int totalCount) where T : class,new()
        {
            totalCount = 0;
                
            try
            {
                using (var db = new WSExpressEntities())
                {
                    var blist = Pagination<T>.PagingWithCount(db, conditions, orderbys, numPerPage, pageNum, out totalCount);
                    if (blist != null)
                    {
                        return blist.ToList<T>();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                string errmsg =  ExceptionMessage.Parse(ex);
                LogHelper.Error(ex);
                
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">条件</param>
        /// <param name="orderbys">排序</param>
        /// <returns></returns>
        public static List<T> GetListNoPaging<T>( string sql, string orderbys) where T : EntityObject //class,new()
        {
            try
            {
                using (var db = new WSExpressEntities())
                {
                    var blist = Pagination<T>.SelectAll(db, sql, orderbys);
                    if (blist != null)
                    {
                        return blist.ToList<T>();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                string errmsg =  ExceptionMessage.Parse(ex);
                LogHelper.Error(ex);
                return null;
            }
        }
       
    }
}