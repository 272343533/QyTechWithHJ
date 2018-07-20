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



namespace SunMvcExpress.Core.BLL
{
    /// <summary>
    /// 实体操作类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class EntityManager<T>
    {
      
        public static ObjectContext db = new WSExpressEntities();


        public static List<T> GetAllByStorProcedure(string spName, string paramvalues)
        {
            try
            {
                var obj = db.ExecuteStoreQuery<T>("exec " + spName + " " + paramvalues).ToList<T>();
                return obj;
            }
            catch (Exception ex)
            {
                string err = entityName + ":" + ex.Message;
                if (ex.InnerException != null)
                    err += " detail:" + ex.InnerException.Message;
                log.Error("ExecStorProcedure:" + err);
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

                entityName = db.GetType().GetProperties().Where(p => p.Name == t.GetType().Name || p.Name == t.GetType().Name + "s" || p.Name == t.GetType().Name + "es").Single().Name;
                db.AddObject(entityName, t);
                db.SaveChanges();
                return "";

            }
            catch (Exception ex)
            {
                string errmsg = ReturnErrMessage(ex);
                log.Error(errmsg);
                return errmsg;
            }
        }

      
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="t">要添加的实体对象</param>
        /// <returns></returns>
        public static object AddReturnEntity<T>( T t, out string errmsg)
        {
            errmsg = "";
            try
            {

                entityName = db.GetType().GetProperties().Where(p => p.Name == t.GetType().Name || p.Name == t.GetType().Name + "s" || p.Name == t.GetType().Name + "es").Single().Name;
                db.AddObject(entityName, t);
                db.SaveChanges();
                return t;
            }
            catch (Exception ex)
            {
                errmsg = ReturnErrMessage(ex);
                log.Error(errmsg);

                return null;
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

                entityName = db.GetType().GetProperties().Where(p => p.Name == t.GetType().Name || p.Name == t.GetType().Name + "s" || p.Name == t.GetType().Name + "es").Single().Name;
                db.UpdateEntity(t, entityName); //自定更新扩展方法
                db.SaveChanges();
                return "";
            }
            catch (Exception ex)
            {
                string errmsg = ReturnErrMessage(ex);
                log.Error(errmsg);
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
                db.DeleteObject(t);
                db.SaveChanges();
                return "";
            }
            catch (Exception ex)
            {
                string errmsg = ReturnErrMessage(ex);
                log.Error(errmsg);
                return errmsg;
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
                                    log.Error("Entity Manager Pageing:" + ex.Message);
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
                string errmsg = ReturnErrMessage(ex);
                log.Error(errmsg);
                throw new Exception(errmsg);
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
                                    log.Error("Entity Manager Pageing:" + ex.Message);
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
                string errmsg = ReturnErrMessage(ex);
                log.Error(errmsg);
                throw new Exception(errmsg);
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
                string errmsg = ReturnErrMessage(ex);
                log.Error(errmsg);
                throw new Exception(errmsg);
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
                string errmsg = ReturnErrMessage(ex);
                log.Error(errmsg);
                throw new Exception(errmsg);
            }
        }


        public static T GetByPk<T>( string PkName, Guid PkValue)
        {
            try
            {
                string tablename = typeof(T).Name;
                string CommandText = "select * from " + tablename + " where " + PkName + "='" + PkValue.ToString() + "'";


                String entityName = db.GetType().GetProperties().Where(p => p.Name == tablename || p.Name == tablename + "s" || p.Name == tablename + "es").Single().Name;

                var obj = db.ExecuteStoreQuery<T>(CommandText, entityName, System.Data.Objects.MergeOption.AppendOnly, null)
                       .SingleOrDefault<T>();
                return obj;
            }
            catch (Exception ex)
            {
                string errmsg = ReturnErrMessage(ex);
                log.Error(errmsg);
                throw new Exception(errmsg);
            }
        }
        public static T GetByPk<T>( string PkName, string PkValue)
        {
            try
            {
                string tablename = typeof(T).Name;
                string CommandText = "select * from " + tablename + " where " + PkName + "='" + PkValue.ToString() + "'";


                String entityName = db.GetType().GetProperties().Where(p => p.Name == tablename || p.Name == tablename + "s" || p.Name == tablename + "es").Single().Name;

                var obj = db.ExecuteStoreQuery<T>(CommandText, entityName, System.Data.Objects.MergeOption.AppendOnly, null)
                       .SingleOrDefault<T>();
                return obj;
            }
            catch (Exception ex)
            {
                string errmsg = ReturnErrMessage(ex);
                log.Error(errmsg);
                throw new Exception(errmsg);
            }
        }
        public static T GetByPk<T>( string PkName, int PkValue)
        {
            try
            {
                string tablename = typeof(T).Name;
                string CommandText = "select * from " + tablename + " where " + PkName + "=" + PkValue.ToString();


                String entityName = db.GetType().GetProperties().Where(p => p.Name == tablename || p.Name == tablename + "s" || p.Name == tablename + "es").Single().Name;

                var obj = db.ExecuteStoreQuery<T>(CommandText, entityName, System.Data.Objects.MergeOption.AppendOnly, null)
                       .SingleOrDefault<T>();
                return obj;
            }
            catch (Exception ex)
            {
                string errmsg = ReturnErrMessage(ex);
                log.Error(errmsg);
                throw new Exception(errmsg);
            }
        }

        public static T GetByStringFieldName<T>( string FName, string FValue)
        {
            try
            {
                string tablename = typeof(T).Name;
                string CommandText = "select * from " + tablename + " where " + FName + "='" + FValue.ToString() + "'";


                String entityName = db.GetType().GetProperties().Where(p => p.Name == tablename || p.Name == tablename + "s" || p.Name == tablename + "es").Single().Name;

                var obj = db.ExecuteStoreQuery<T>(CommandText, entityName, System.Data.Objects.MergeOption.AppendOnly, null)
                       .SingleOrDefault<T>();
                return obj;
            }
            catch (Exception ex)
            {
                string errmsg = ReturnErrMessage(ex);
                log.Error(errmsg);
                throw new Exception(errmsg);
            }
        }

        public static T GetBySql<T>( string conditionSql)
        {
            try
            {
                string tablename = typeof(T).Name;
                string CommandText = "select * from " + tablename + " where " + conditionSql;


                String entityName = db.GetType().GetProperties().Where(p => p.Name == tablename || p.Name == tablename + "s" || p.Name == tablename + "es").Single().Name;

                var obj = db.ExecuteStoreQuery<T>(CommandText, entityName, System.Data.Objects.MergeOption.AppendOnly, null)
                       .SingleOrDefault<T>();
                return obj;
            }
            catch (Exception ex)
            {
                string errmsg = ReturnErrMessage(ex);
                log.Error(errmsg);
                throw new Exception(errmsg); //return default(T);
            }
        }

        public static List<T> GetGirdListwithPaging<T>( string conditions, string orderbys, int pageNum, int numPerPage, out int totalCount) where T : class,new()
        {
            try
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
            catch (Exception ex)
            {
                string errmsg = ReturnErrMessage(ex);
                log.Error(errmsg);
                throw new Exception(errmsg);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">条件</param>
        /// <param name="orderbys">排序</param>
        /// <returns></returns>
        public static List<T> GetListNoPaging<T>( string sql, string orderbys) where T : class,new()
        {
            try
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
            catch (Exception ex)
            {
                string errmsg = ReturnErrMessage(ex);
                log.Error(errmsg);
                throw new Exception(errmsg);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userID"></param>
        /// <param name="fkcondtion"></param>
        /// <param name="filter"></param>
        /// <param name="conditions"</param>
        /// <param name="orderbys"></param>
        /// <returns></returns>
        public static List<T> GetListNoPaging<T>(Guid userID, string fkcondtion, string filter, string conditions, string orderbys) where T : class,new()
        {
            try
            {
                string sql = " ";
                if (conditions != null && conditions != "")
                    sql = conditions + " and ";

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
                //}

                List<T> ret = GetListNoPaging<T>(db,sql, orderbys);
                return ret;
            }
            catch (Exception ex)
            {
                string errmsg = ReturnErrMessage(ex);
                log.Error(errmsg);
                throw new Exception(errmsg);
            }
        }

        
    }
}