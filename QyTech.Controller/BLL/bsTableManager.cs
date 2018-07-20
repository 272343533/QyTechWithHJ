using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunMvcExpress.Dao;
using SunMvcExpress.Core.BLL;
using SunMvcExpress.Core.Models;
using System.Data.Objects.DataClasses;
using System.Data.Objects;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using SunMvcExpress.Core.Common;
using System.IO;
using QyTech.Core.Common;
using QyTech.Core.Helpers;
using QyTech.Core.BLL;

namespace SunMvcExpress.BLL
{

    /// <summary>
    /// 基本表bsTable数据访问
    /// </summary>
    public partial class bsTableManager //: CreateManagerParent
    {
        /// <summary>
        /// 获取字段对象
        /// </summary>
        /// <returns></returns>
        public static bsField GetFieldObject(string tablename,string fieldname)
        {
            bsField obj=null;
            using (var db = new WSExpressEntities())
            {
                bsTable obj1=db.bsTable.Where(t=>t.TName==tablename).FirstOrDefault<bsTable>();
                if (obj1!=null)
                {
                    obj=db.bsField.Where(f=>f.bsT_Id==obj1.Id && f.Name==fieldname).FirstOrDefault<bsField>();
                }
                return obj;
            }
        }
        //public List<T> Paging<T>(Guid funid, Guid userID, string Name, string orderField, OrderMethod dire, int pageNum, int numPerPage, out int totalCount) where T : class,new()
        //{
        //    string sql = " TType=0 and ";
        //    List<string> usingfield = new List<string>();
        //    if (Name != null && !Name.Equals(""))
        //    {
        //        sql += "Name like '%" + Name + "%' and ";
        //    }
        //    String idset = Toolkit<bsOrganize>.ToSymbolString(bsUserManager.GetEmpDataRighs(userID), "id", ",");

        //    if (idset.Length>0)
        //    {
        //        idset = idset.Equals("") ? "'" + Guid.Empty.ToString() + "'" : idset + ",'" + Guid.Empty.ToString() + "'";
        //        sql += "id in (" + idset + ")";
        //    }
        //    else
        //    {
        //        if (sql.Length > 4)
        //            sql = sql.Substring(0, sql.Length - 4);
        //    }
        //    List<T> ret = GetGirdListwithPaging<T>(sql, orderField, dire, pageNum, numPerPage, out totalCount);
        //    return ret;
        //}
      
        public static bool LDeleteDetailByPid(Guid id)
        {
            try
            {
                using (var db = new WSExpressEntities())
                {
                    db.ExecuteStoreCommand("update bsField set datastatus=1 where bst_Id='" + id.ToString() + "' and datastatus=0");
                    return true;
                }
            }
            catch { return false; }
        }
        public static bool PDeleteDetailByPid(Guid id)
        {
            try
            {
                using (var db = new WSExpressEntities())
                {
                    db.ExecuteStoreCommand("delete from bsField where bst_Id='" + id.ToString() + "' and datastatus=1");
                    return true;
                }
            }
            catch { return false; }
        }
        public static bool LRestoreDetailByPid(Guid id)
        {
            try
            {
                using (var db = new WSExpressEntities())
                {
                    db.ExecuteStoreCommand("delete from bsField where  bst_Id='" + id.ToString() + "' and datastatus=0");
                    db.ExecuteStoreCommand("update bsField set datastatus=0 where  bst_Id='" + id.ToString() + "' and datastatus=1");
                    return true;
                }
            }
            catch { return false; }
        }
    }
}
