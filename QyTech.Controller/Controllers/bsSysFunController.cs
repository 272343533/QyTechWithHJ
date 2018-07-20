using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SunMvcExpress.Dao;
using SunMvcExpress.Core.Models;
using SunMvcExpress.Core.BLL;
using SunMvcExpress.Core.Helpers;
using SunMvcExpress.Core.Common;
using System.Text;

using QyTech.Core.Common;
using QyTech.Core.Helpers;

namespace SunMvcExpress.Controllers
{
    public class bsSysFunController : Controller
    {
        public Guid Login_User_ID = LoginHelper.GetLoginUserId();
        public string Login_Nick_Name = LoginHelper.GetLoginNickName();
        public Guid Login_UserOrgId = LoginHelper.GetLoginCompanyId();
	

        public ActionResult bsSysFun()//string sel_bsF_Id, string query_FuncName, string orderField, string funid = "04932a29-6046-4882-9ed5-b9a8cebd2bba", int pageNum = 1, int numPerPage = 50)
        {
            if (!SunMvcApp.isValid())
            {
                return null;
            }
            return View();
        }

     
        
        public JsonResult bsSysFunEdit(FormCollection fc)
        {
            bsSysFun obj = new bsSysFun();
            JsonResult json = new JsonResult();
            Guid guid = new Guid(fc["bsSysFun_FunId"]);
            bsSysFun objdb = EntityManager<bsSysFun>.GetByPk<bsSysFun>("FunId", guid);
            objdb.FNo = int.Parse(fc["bsSysFun_FNo"]);
            objdb.Name = fc["bsSysFun_Name"];
            objdb.Controller = fc["bsSysFun_Controller"];
            //objdb.IsSys = obj.IsSys;
            objdb.rel = fc["bsSysFun_rel"];
            objdb.Type = fc["bsSysFun_Type"];
            objdb.Target = fc["bsSysFun_Target"];
            if (fc["bsSysFun_Height"] != null && !fc["bsSysFun_Height"].Equals(""))
            {
                objdb.Height = int.Parse(fc["bsSysFun_Height"]);
            }
            if (fc["bsSysFun_Width"] != null && !fc["bsSysFun_Width"].Equals(""))
            {
                objdb.Width = int.Parse(fc["bsSysFun_Width"]);
            }
            if (fc["bsSysFun_FunStatus"] != null && !fc["bsSysFun_FunStatus"].Equals(""))
            {
                objdb.FunStatus = bool.Parse(fc["bsSysFun_FunStatus"]);
            }
            else
            {
                objdb.FunStatus = true ;
            }

            if (fc["bsSysFun_UseUserPolicy"] != null && !fc["bsSysFun_UseUserPolicy"].Equals(""))
            {
                objdb.UseUserPolicy = bool.Parse(fc["bsSysFun_UseUserPolicy"]);
            }
            else
            {
                objdb.UseUserPolicy = false;
            }
            if (fc["bsSysFun_UseRoleFields"] != null && !fc["bsSysFun_UseRoleFields"].Equals(""))
            {
                objdb.UseRoleFields = bool.Parse(fc["bsSysFun_UseRoleFields"]);
            }
            else
            {
                objdb.UseRoleFields = false;
            }
            AjaxJsonResult rs = null;
            string errMsg=EntityManager<bsSysFun>.Modify(objdb);
            if (errMsg.Equals(""))
            {
                rs = DwzReturn.SuccessAjaxJsonResultNotClosed("editpage");
            }
            else
            {
                rs = DwzReturn.FailAjaxJsonResult(errMsg);
            }
            json.Data = rs;
            return json;
        }

        public JsonResult TreeFunGroup()
        {
            using (var db = new WSExpressEntities())
            {
                var info = db.bsSysFun.Where(p => p.Type != "SubPage").OrderBy(p => p.FNo);
                var treelist = new List<ZTreeNode>();
                foreach (var cc in info)
                {
                    var treenode = new ZTreeNode();
                    treenode.Id = cc.FunId.ToString();
                    treenode.Name = cc.Name;
                    treenode.PId = cc.PId.ToString();
                    if (cc.Type.ToLower() == "root")
                    {
                        treenode.open = true;
                    }
                    else
                    {
                        treenode.open = false;
                    }
                    //treenode.target = cc.target;
                    treelist.Add(treenode);
                }
                var source = from c in treelist select c;
                return Json(source, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult ztreeDis()
        {

            int totalCount;

            List<bsSysFun> objs = new List<bsSysFun>();
            objs = EntityManager<bsSysFun>.Paging < bsSysFun>(Login_User_ID,"IsSys!=1","FNo", 1, 200, out totalCount);

            var treelist = new List<ZTreeNode>();

            foreach (var cc in objs)
            {
                var treenode = new ZTreeNode();
                treenode.Id = cc.FunId.ToString();
                treenode.Name = cc.Name;

                treenode.PId = cc.PId.ToString();//.HasValue ? (Guid)cc.PId : Guid.Empty;
                if (objs.Count < 150)
                {
                    treenode.open = true;
                }
                else
                {
                    if (cc.Type == "Root")
                        treenode.open = true;
                }
                treenode.Url = "";
                treenode.target = "";
                treelist.Add(treenode);
            }
            var source = from c in treelist select c;
            return Json(source, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ztreeDisAllFun()
        {

            int totalCount;

            List<bsSysFun> objs = new List<bsSysFun>();
            objs = EntityManager<bsSysFun>.Paging<bsSysFun>(Login_User_ID, "", "FNo", 1, 200, out totalCount);

            var treelist = new List<ZTreeNode>();

            foreach (var cc in objs)
            {
                var treenode = new ZTreeNode();
                treenode.Id = cc.FunId.ToString();
                treenode.Name = cc.Name;

                treenode.PId = cc.PId.ToString();//.HasValue ? (Guid)cc.PId : Guid.Empty;
                if (objs.Count < 150)
                {
                    treenode.open = true;
                }
                else
                {
                    if (cc.Type == "Root")
                        treenode.open = true;
                }
                treenode.addBtnFlag = true;
                treenode.editBtnFlag = true;
                treenode.removeBtnFlag = true;
                treenode.Url = "";
                treenode.target = "";
                treelist.Add(treenode);
            }
            var source = from c in treelist select c;
            return Json(source, JsonRequestBehavior.AllowGet);
        }


        //public JsonResult bsSysFunForEdit(Guid Id)
        //{

        //    using (var db = new WSExpressEntities())
        //    {
        //        var info = db.bsSysFun.SingleOrDefault(c => c.FunId == Id);

        //        JsonResult json = new JsonResult();
        //        json.Data = new
        //        {
        //            FunId = info.FunId,
        //            Name = info.Name,
        //            FNo = info.FNo,
        //            Type = info.Type,
        //            Controller = info.Controller,
        //            Action = info.Action,
        //            Target = info.Target,
        //            rel = info.rel,
        //            Height = info.Height,
        //            Width = info.Width,
        //            FunStatus = info.FunStatus
        //        };

        //        return Json(json, JsonRequestBehavior.AllowGet);
        //    }
        //}



        public JsonResult bsSysFunForEdit(Guid Id)
        {


            var info = EntityManager<bsSysFun>.GetByPk<bsSysFun>("FunId", Id);


            JsonResult json = new JsonResult();
            json.Data = new
            {
                FunId = info.FunId,
                Name = info.Name,
                FNo = info.FNo,
                Type = info.Type,
                Controller = info.Controller,
                Action = info.Action,
                Target = info.Target,
                rel = info.rel,
                Height = info.Height,
                Width = info.Width,
                FunStatus = info.FunStatus,
                UseUserPolicy = info.UseUserPolicy,
                UseRoleFields = info.UseRoleFields
            };

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ztreeAdd(Guid id, string name, Guid pId)
        {
            bsSysFun obj = new bsSysFun();
            obj.FunId = id;
            obj.Name = name;
            obj.PId = pId;
            obj.IsSys = 3;
            obj.FNo = 0;
            EntityManager<bsSysFun>.Add(obj);
            return null;
        }

        public JsonResult ztreeDel(Guid Id)
        {

            var info = EntityManager<bsSysFun>.GetByPk<bsSysFun>("FunId", Id);

            EntityManager<bsSysFun>.Delete<bsSysFun>(info);
            return null;
        }

        public JsonResult ztreeEdit(Guid Id, String Name)
        {
            var info = EntityManager<bsSysFun>.GetByPk<bsSysFun>("FunId", Id);
            info.Name = Name;
            EntityManager<bsSysFun>.Modify<bsSysFun>(info);
            return null;
        }

        public JsonResult ztreeDrag(Guid Id, Guid PId)
        {
            bsSysFun objdb = EntityManager<bsSysFun>.GetByPk<bsSysFun>("FunId", Id);
            objdb.PId = PId;
            EntityManager<bsSysFun>.Modify(objdb);

            return null;
        }


        /// <summary>
        /// 为软件用户配置权限
        /// </summary>
        /// <param name="id">软件用户id</param>
        /// <returns></returns>
        public JsonResult ztreeDisForSoftUser(Guid id)
        {

            try
            {
                bsSoftCustInfo obj = EntityManager<bsSoftCustInfo>.GetByPk<bsSoftCustInfo>("CustId", id);
                List<vwTreeFun> objs = EntityManager<vwTreeFun>.GetListNoPaging<vwTreeFun>("", "NodeCode");
                var treelist = new List<ZTreeNode>();
                foreach (var o in objs)
                {
                    var treenode = new ZTreeNode();
                    treenode.Id = o.Id.ToString();
                    treenode.Name = o.Name;
                    treenode.PId = o.PId.ToString();
                    treenode.open = false;
                    if (obj.FunRights != null && obj.FunRights.Contains(o.Id.ToString()))
                        treenode.Checked = true;
                    treelist.Add(treenode);
                }
                var source = from c in treelist select c;
                return Json(source, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
 
    }
}
