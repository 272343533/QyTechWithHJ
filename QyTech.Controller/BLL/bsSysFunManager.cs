using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SunMvcExpress.Dao;
using QyTech.Core.Common;
using QyTech.Core.Helpers;
using QyTech.Core.BLL;

namespace SunMvcExpress.BLL
{
    public class bsSysFunManager
    {

        public static bsSysFunc SelectByid(Guid fcid)
        {
            using (var db = new WSExpressEntities())
            {
                bsSysFunc obj = db.bsSysFunc.Where(o=>o.FcId==fcid).First<bsSysFunc>();

                return obj;
            }
        }
        public static bsSysFunc SelectByActionName(string controllerName)
        {
            using (var db = new WSExpressEntities())
            {
                bsSysFunc obj = db.bsSysFunc.Where(o => o.FunContr == controllerName).First<bsSysFunc>();

                return obj;
            }
        }
        /// <summary>
        /// 获取字段对象
        /// </summary>
        /// <returns></returns>
        public static List<bsSysFunc> SelectAll()
        {
            using (var db = new WSExpressEntities())
            {
                List<bsSysFunc> objs = db.bsSysFunc.ToList<bsSysFunc>();
               
                return objs;
            }
        }


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

        //userright，sortedlist<string,string[]>(Name,按钮功能)
        /// <summary>
        /// 根据用户id获取操作权限，页面、页面
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns>(页面,页面按钮功能)，如：采购计划，[保存Save，查看Browse，审核Audit]</returns>
        public static SortedList<string, List<string>> GetEmpOperateRights(Guid id)
        {
            SortedList<string, List<string>> rights = new SortedList<string, List<string>>();

            //try
            //{
                ////页面时funid，按钮功能是bsRolFuncRel
                ////获取emp的所有角色

                //using (var db = new WSExpressEntities())
                //{
                //    var r1 = db.bsUser.Single(t => t.UserID == id);
                //    List<bsRole> objrs = r1.bsRole.OrderBy(s => s.RoleCode).ToList();
                //    //还需要得到这些部门的子部门，这样才能够处理的更好。
                //    foreach (bsRole o in objrs)
                //    {
                //        #region 得到角色功能操作权限
                //        List<bsRolFuncRel> rfobjs = db.bsRolFuncRels.Where(f => f.RoleID == o.RoleID).ToList<bsRolFuncRel>();
                //        foreach (bsRolFuncRel r in rfobjs)
                //        {
                //            List<string> ops;
                //            if (!rights.ContainsKey(r.bsSysFunc.FunContr))
                //            {
                //                //获取操作权限
                //                ops = new List<string>();
                //                if ((bool)r.rQuery)
                //                    ops.Add("Query");
                //                if ((bool)r.rAdd)
                //                    ops.Add("Add");
                //                if ((bool)r.rDelete)
                //                    ops.Add("Delete");
                //                if ((bool)r.rUpdate)
                //                    ops.Add("Edit");

                //                if ((bool)r.rAudit)
                //                    ops.Add("Audit");
                              

                //                rights.Add(r.bsSysFunc.FunContr, ops);
                //            }
                //            else
                //            {
                //                ops = rights[r.bsSysFunc.FunContr];

                //                if ((bool)r.rQuery && !ops.Contains("Query"))
                //                    ops.Add("Query");
                //                if ((bool)r.rAdd && !ops.Contains("Add"))
                //                    ops.Add("Add");
                //                if ((bool)r.rDelete && !ops.Contains("Delete"))
                //                    ops.Add("Delete");
                //                if ((bool)r.rUpdate && !ops.Contains("Update"))
                //                    ops.Add("Edit");
                //                if ((bool)r.Excel && !ops.Contains("Update"))
                //                    ops.Add("Excel");
                //                if ((bool)r.rAudit && !ops.Contains("Audit"))
                //                    ops.Add("Audit");
                      

                //                rights[r.bsSysFunc.FunContr] = ops;
                //            }

                //        }
                //        #endregion
                //    }
                    return rights;
            //    }

            //}
            //catch { return rights; }

        }
       

    }
}
