using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Web;
using SunMvcExpress.Dao;
using SunMvcExpress.Core.Models;
using System.Data.Objects.DataClasses;
using System.Data.Objects;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using SunMvcExpress.Core.Common;
using SunMvcExpress.Core.BLL;
using System.IO;
//using SunMvcExpress.Core.CreateDataAccess;
using QyTech.Core.Common;
using QyTech.Core.Helpers;
using QyTech.Core.BLL;

namespace SunMvcExpress.BLL
{
    public class bsSysFuncManager //: CreateManagerParent
    {

        /// <summary>
        /// 根据用户id获取数据权限，按照org过滤
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<bsOrganize> GetEmpDataRighs(Guid id)
        {
            List<bsOrganize> objstmp = new List<bsOrganize>();
            try
            {
                List<bsOrganize> subobjs;
                using (var db = new WSExpressEntities())
                {
                    var r1 = db.bsUser.Single(t => t.UserID == id);
                    List<bsOrganize> objs = r1.bsOrganize.OrderBy(s => s.Name).ToList();
                    //还需要得到这些部门的子部门，这样才能够处理的更好。
                    //foreach (bsOrganize o in objs)
                    //{
                    //    subobjs = OrganizeManager.GetSubOrganizeList(o.Id);
                    //    objstmp.AddRange(subobjs);
                    //}
                    objstmp.AddRange(objs);

                    return objstmp;

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return objstmp;
            }
        }

       
        public List<T> Paging<T>(Guid funid, string sel_fungroupid, string FuncName, string orderField, OrderMethod dire, int pageNum, int numPerPage, out int totalCount) where T : class,new()
        {
            //string sql = " substring(FuncCode,1,3)!='sys' and ";
            
            string sql = " GName!='系统' and datastatus=0 and ";
            if (sel_fungroupid != null && sel_fungroupid.Length > 0)
                sql = "  FunId='" + sel_fungroupid + "' and ";
            List<string> usingfield = new List<string>();
            if (FuncName != null && !FuncName.Equals(""))
            {
                sql += "FuncName like '%" + FuncName + "%' and ";
            }
            //bsSysFuncId = funid;
            //if (bsSysfun.Filterfield != null && (bsSysfun.Filterfield.ToString().Length > 0))
            //{
            //   sql += " and ";
            //}
            //sql += "  isnull(datastatus,0) <> 1 ";
            //if (HaveOrgid)
            //{
            //    String idset = Toolkit<bsOrganize>.ToSymbolString(OrganizeManager.GetSubOrganizeList(id), "id", ",");
            //    idset = idset.Equals("") ? "" + id : idset + "," + id;
            //    sql += "id in (" + idset + ")";
            //}
            //else
            //{
            if (sql.Length > 4)
                sql = sql.Substring(0, sql.Length - 4);
            //}
            List<T> ret = EntityManager<T>.GetGirdListwithPaging<T>(sql, orderField, pageNum, numPerPage, out totalCount);
            return ret;
        }
    }
}
