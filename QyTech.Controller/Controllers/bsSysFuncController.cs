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
using SunMvcExpress.Core;
using System.Text;
using SunMvcExpress.BLL;

using QyTech.Core.Common;
using QyTech.Core.Helpers;
using QyTech.Core.BLL;
using QyTech.Express;
using QyTech.Express.CodeCreate.IDataView;

namespace SunMvcExpress.Controllers
{
    public class bsSysFuncController : WSExpressController
    {


        public Guid Login_User_ID = LoginHelper.GetLoginUserId();
        public string Login_Nick_Name = LoginHelper.GetLoginNickName();


        //public ActionResult InitFunField(Guid id)
        //{
        //    int ret = 10;
        //    //CreateCode.InitFunField(id);
        //    ret=EntityManager<bsField>.ExecuteSql("exec bsAddFunDateToFunField '" + id.ToString() + "'");
                
        //    JsonResult json = new JsonResult();

        //    AjaxJsonResult rs = null;
        //    if (ret==-1)
        //    {
        //        rs = DwzReturn.SuccessAjaxJsonResultNotClosed("");
        //    }
        //    else
        //    {
        //        rs = DwzReturn.FailAjaxJsonResult();
        //    }
        //    json.Data = rs;

        //    return json;
        //}

        public JsonResult CopyFunSetRow(Guid id)
        {
            JsonResult json = new JsonResult();
            using (var db = new WSExpressEntities())
            {

                AjaxJsonResult rs = null;
                string sql = "exec bscopyTableRow '" + id.ToString() + "','bsSysFunc'";
                int i = db.ExecuteStoreCommand(sql);
                if (i == -1)
                {
                    rs = DwzReturn.SuccessAjaxJsonResultNotClosed("");
                }
                else
                {
                    rs = DwzReturn.FailAjaxJsonResult();
                }
                json.Data = rs;
            }
            return json;
        }


        public ActionResult CreateViewFunCode(Guid id)
        {
            JsonResult json = new JsonResult();

            AjaxJsonResult rs = null;

            bsSysFunc obj = bsSysFunManager.SelectByid(id);
            if (obj.FunType.ToLower() == FuncType.Sub.ToString().ToLower())
            {
                rs = new AjaxJsonResult
                {
                    statusCode = "300",
                    message = "sub类型不需生成",
                    navTabId = "",
                    rel = "",
                    callbackType = "",
                    forwardUrl = ""
                };
            }
            else
            {
                bsSysFunc fc = EntityManager<bsSysFunc>.GetByPk<bsSysFunc>("FcId", id);
                bool ret = CreateCode.CreateView(fc);

                if (ret)
                {
                    rs = DwzReturn.SuccessAjaxJsonResultNotClosed("");
                }
                else
                {
                    rs = DwzReturn.FailAjaxJsonResult();
                }
            }
            json.Data = rs;

            return json;
        }

        //public ActionResult CreateFunCode(Guid id)
        //{
        //    JsonResult json = new JsonResult();

        //    AjaxJsonResult rs = null;

        //    bsSysFunc obj = EntityManager<bsSysFunc>.GetByPk<bsSysFunc>("FcId", id);
        //    if (obj.FunType.ToLower() == FuncType.Sub.ToString().ToLower())
        //    {
        //        rs = new AjaxJsonResult
        //        {
        //            statusCode = "300",
        //            message = "sub类型不需生成",
        //            navTabId = "",
        //            rel = "",
        //            callbackType = "",
        //            forwardUrl = ""
        //        };
        //    }
        //    else
        //    {
        //        bool ret = CreateCode.Create(obj);

        //        if (ret)
        //        {
        //            rs = DwzReturn.SuccessAjaxJsonResultNotClosed("");
        //        }
        //        else
        //        {
        //            rs = DwzReturn.FailAjaxJsonResult();
        //        }
        //    }
        //    json.Data = rs;

        //    return json;
        //}
        public ActionResult bsSysFuncson()
        {
            return View();
        }
        public ActionResult bsSysFunc()
        {
            return View();
           
        }

        public ActionResult bsSysFuncLookup(string query_FuncName, int pageNum = 1, int numPerPage = 50)
        {
            if (!SunMvcApp.isValid())
            {
                return null;
            }
            int totalCount;
            string orderField = "FuncCode";
            List<bsSysFunc> objs = new List<bsSysFunc>();
            string conditions = "";
            if (query_FuncName != null && query_FuncName != "")
                conditions += " FuncName like '%" + query_FuncName + "%'";
            objs = EntityManager<bsSysFunc>.Paging<bsSysFunc>(Login_User_ID, conditions, orderField, pageNum, numPerPage, out totalCount);
            ViewBag.numPerPage = numPerPage;
            ViewBag.pageNum = pageNum;
            ViewBag.totalCount = totalCount;
            ViewBag.FuncName = query_FuncName;
            return View(objs);
        }
       
        public ActionResult FunField(Guid id)
        {
            return View();
        }

        public ActionResult bsSysFuncRight(string FunId, string conditions, string orderbys, string query_FuncName, string orderField, string funid = "04932a29-6046-4882-9ed5-b9a8cebd2bba", int pageNum = 1, int numPerPage = 50)
        {
            if (!SunMvcApp.isValid())
            {
                return null;
            }
            int totalCount;
            orderField = "FuncCode";
            List<bsSysFunc> objs = new List<bsSysFunc>();
            if (query_FuncName != null && query_FuncName != "")
            {
                conditions += "FuncName like '%" + query_FuncName + "%' ";
            }
            if (conditions != null && conditions != "")
                conditions += " and dbo.[JudgeFunI2IsInFunI1]('" + FunId + "',FunId)=1";
            else

                conditions = " dbo.[JudgeFunI2IsInFunI1]('" + FunId + "',FunId)=1";
            objs = EntityManager<bsSysFunc>.Paging<bsSysFunc>(Login_User_ID, conditions, orderbys, pageNum, numPerPage, out totalCount);
            ViewBag.numPerPage = numPerPage;
            ViewBag.pageNum = pageNum;
            ViewBag.totalCount = totalCount;
            ViewBag.FuncName = query_FuncName;
            ViewBag.FunId = FunId;

            return View(objs);
            
        }

        public ActionResult bsSysFuncAdd(Guid? id, Guid? FunId, FormCollection fc)
        {
            JsonResult json = new JsonResult();
            ViewBag.PredefPageDataStyle = PredefPageDataStyle.PageLayoutType;
            bsSysFunc obj = new bsSysFunc();
            if (id == null && fc != null && fc["bsSysFunc_FuncName"] != null && !fc["bsSysFunc_FuncName"].Equals(""))
            {
                AjaxJsonResult rs = null;
                string errMsg = bsSysFuncFc(fc, 1);
                if (errMsg.Equals(""))
                {
                    rs = DwzReturn.SuccessAjaxJsonResultNotClosed("");
                }
                else
                {
                    rs = DwzReturn.FailAjaxJsonResult(errMsg);
                }
                json.Data = rs;
                return json;
            }
            else
            {
                if (id != null)
                {
                    using (var db = new WSExpressEntities())
                    {
                        var info = db.bsSysFunc.Single(t => t.FcId == id);
                        ViewBag.FunId = info.FunId;
                        return View(info);
                    }
                }
                ViewBag.FunId = FunId;
                obj.FunLayout = null;
                return View(obj);
            }
        }


        public JsonResult bsSysFuncDel(Guid id)
        {
            JsonResult json = new JsonResult();
            using (var db = new WSExpressEntities())
            {
                var info = db.bsSysFunc.SingleOrDefault(o => o.FcId == id);
                List<bsSysFuncQuery> infoqer = EntityManager<bsSysFuncQuery>.GetListNoPaging<bsSysFuncQuery>("FcId='" + id.ToString() + "'", "");
                List<bsSysFuncOper> infoopr = EntityManager<bsSysFuncOper>.GetListNoPaging<bsSysFuncOper>("FcId='" + id.ToString() + "'", "");
                if (infoqer.Count != 0)
                {
                    foreach (bsSysFuncQuery opr in infoqer)
                    {
                        FuncQueryCondiftionDel(opr.Id);

                    }
                }
                if (infoopr.Count != 0)
                {
                    foreach (bsSysFuncOper opr in infoopr)
                    {
                        FuncOperDel(opr.OperId);
                    }
                }
                if (info != null)
                {
                    AjaxJsonResult rs = null;
                    string errMsg=EntityManager<bsSysFunc>.Delete(db, info);
                    if (errMsg.Equals(""))
                    {
                        rs = DwzReturn.SuccessAjaxJsonResultNotClosed("Func");
                    }
                    else
                    {
                        rs = DwzReturn.FailAjaxJsonResult(errMsg);
                    }
                    json.Data = rs;
                }
            }
            return json;
        }


        public JsonResult bsSysFuncEdit(FormCollection fc)
        {
            JsonResult json = new JsonResult();
            AjaxJsonResult rs = null;
            Guid newguid = Guid.NewGuid();
            string errMsg = "";
            if (!fc["bsSysFunc_FcId"].ToString().Equals("00000000-0000-0000-0000-000000000000"))
            {

                errMsg = bsSysFuncFc(fc, 2);
            }
            else
            {
                errMsg = bsSysFuncFc(fc, 1);
            }

            if (errMsg.Equals(""))
            {
                rs = DwzReturn.SuccessAjaxJsonResultNotClosed("");
            }
            else
            {
                rs = DwzReturn.FailAjaxJsonResult(errMsg);
            }
            json.Data = rs;
            return json;
        }


        public ActionResult ListField(string orderField, string funid = "ebf74e0d-434f-4da8-a4ad-d5fcfeec7402", int pageNum = 1, int numPerPage = 20)
        {
            int totalCount;
            List<bsFuncField> objs = EntityManager<bsFuncField>.Paging<bsFuncField>(Login_User_ID, "","", pageNum, numPerPage, out totalCount);
            ViewBag.numPerPage = numPerPage;
            ViewBag.pageNum = pageNum;
            ViewBag.totalCount = totalCount;
            return View(objs);
        }



        public ActionResult ListFieldAdd(bsFuncField obj, Guid? id)
        {
            JsonResult json = new JsonResult();
            if (id == null && obj != null && obj.FName != null && !obj.FName.Equals(""))
            {
                AjaxJsonResult rs = null;
                obj.Id = Guid.NewGuid();
                string errMsg=EntityManager<bsFuncField>.Add(obj);
                if (errMsg.Equals(""))
                {
                    rs = DwzReturn.SuccessAjaxJsonResult("ListField");
                }
                else
                {
                    rs = DwzReturn.FailAjaxJsonResult(errMsg);
                }
                json.Data = rs;
                return json;
            }
            else
            {
                if (id != null)
                {
                    using (var db = new WSExpressEntities())
                    {
                        var info = db.bsFuncField.Single(t => t.Id == id);
                        return View(info);
                    }
                }
                return View();
            }
        }

        public ActionResult FuncQueryCondition(Guid fcid)
        {
            //List<bsFuncField> obj = EntityManager<bsFuncField>.GetListNoPaging("FcId='" + fcid.ToString() + "'", "");

            List<bsSysFuncQuery> objs = EntityManager<bsSysFuncQuery>.GetListNoPaging<bsSysFuncQuery>("FcId='" + fcid.ToString() + "'", "");
            ViewBag.FcId = fcid;
            ViewBag.totalCount = 50;
            ViewBag.numPerPage = 50;
            ViewBag.pageNum = 1;
            return View(objs);
        }

        public ActionResult FuncAction(Guid fcid)
        {

            List<bsSysFuncInterface> objs = EntityManager<bsSysFuncInterface>.GetListNoPaging<bsSysFuncInterface>("FcId='" + fcid.ToString() + "'", "");
            ViewBag.FcId = fcid;

            return View(objs);
        }
        public JsonResult FuncActionAdd(Guid id)
        {
            JsonResult json = new JsonResult();
            AjaxJsonResult rs = null;
            using (var db = new WSExpressEntities())
            {
                bsSysFuncInterface obj = new bsSysFuncInterface();
                obj.InterfId = Guid.NewGuid();
                obj.FcId = id;
                obj.InterfNo = 10;
                obj.InterfDesp = "描述";
                obj.RetType = "add";
                obj.RetObject = "Object";
                obj.InterfName = "新增";
                obj.InterfParams = "param";
                string errMsg = EntityManager<bsSysFuncInterface>.Add<bsSysFuncInterface>(obj);
                if (errMsg.Equals(""))
                {
                    rs = DwzReturn.SuccessAjaxJsonResultNotClosed("");
                }
                else
                {
                    rs = DwzReturn.FailAjaxJsonResult(errMsg);
                }
            }
                json.Data=rs;
                return json;
        }
        public JsonResult FuncActionEdit(FormCollection fc)
        {
            JsonResult json = new JsonResult();
            AjaxJsonResult rs = null;
            if (SaveActionGirdForm(fc) == "")
            {
                rs = DwzReturn.SuccessAjaxJsonResultNotClosed("");
            }
            else
            {
                rs = DwzReturn.FailAjaxJsonResult();
            }
            json.Data = rs;
            return json;
        }
        private string SaveActionGirdForm(FormCollection fc)
        {
            try
            {
                if (fc.AllKeys[fc.Count - 1].Substring(0, 4) == "item")
                {
                    for (int i = 0; i < Convert.ToInt32(fc.AllKeys[fc.Count - 1].Substring(6, fc.AllKeys[fc.Count - 1].IndexOf('.') - 7)) + 1; i++)
                    {
                        Guid Id = Guid.Parse(fc["items[" + i.ToString() + "].InterfId"].ToString());
                        bsSysFuncInterface dbobj = EntityManager<bsSysFuncInterface>.GetByPk<bsSysFuncInterface>("InterfId", Id);
                        dbobj.InterfNo = Convert.ToInt32(fc["items[" + i.ToString() + "].InterfNo"]);
                        dbobj.FcId = Guid.Parse(fc["items[" + i.ToString() + "].FcId"]);
                        dbobj.InterfName = fc["items[" + i.ToString() + "].InterfName"].ToString();
                        dbobj.InterfDesp = fc["items[" + i.ToString() + "].InterfDesp"].ToString();
                        dbobj.RetObject = fc["items[" + i.ToString() + "].RetObject"].ToString();
                        dbobj.RetType = fc["items[" + i.ToString() + "].RetType"].ToString();
                        dbobj.InterfParams = fc["items[" + i.ToString() + "].InterfParams"].ToString();
                        errMsg = EntityManager<bsSysFuncInterface>.Modify(dbobj);
                        if (errMsg != "")
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = ReturnErrMessage(ex);
            }

            return errMsg;
        }
        public JsonResult FuncActionDel(Guid id)
        {
            JsonResult json = new JsonResult();
            AjaxJsonResult rs = null;
            using (var db = new WSExpressEntities())
            {
                var info = db.bsSysFuncInterface.SingleOrDefault(o => o.InterfId == id);
                if (info != null)
                {
                    string errMsg = EntityManager<bsSysFuncInterface>.Delete(db, info);
                    if (errMsg.Equals(""))
                    {
                        rs = DwzReturn.SuccessAjaxJsonResultNotClosed("");
                    }
                    else
                    {
                        rs = DwzReturn.FailAjaxJsonResult(errMsg);
                    }
                }
            }
            json.Data = rs;
            return json;
        }
        public ActionResult FuncQueryCondiftionAdd(Guid fcid, int pageNum = 1, int numPerPage = 50)
        {
            int totalCount;


            List<bsFuncField> objj = EntityManager<bsFuncField>.GetGirdListwithPaging<bsFuncField>("FcId='" + fcid.ToString() + "'", "FleldNo asc", pageNum, numPerPage, out totalCount);
            ViewBag.pageNum = pageNum;
            ViewBag.numPerPage = numPerPage;
            ViewBag.totalCount = totalCount;
            ViewBag.FcId = fcid;
            return View(objj);
        }
        public JsonResult FuncQueryCondiftionAdd1(string FId)
        {
            JsonResult json = new JsonResult();
            AjaxJsonResult rs = null;
            string[] s = FId.Split(new char[] { ',' });
            int i;
            using (var db = new WSExpressEntities())
            {
                for (i = 0; i < s.Length; i++)
                {
                    Guid guid = new Guid(s[i]);
                    bsFuncField obj = EntityManager<bsFuncField>.GetByPk<bsFuncField>("Id", guid);
                    db.ExecuteStoreCommand("exec bsAddQueryDateToFuncQuery '" + obj.Id + "'");
                }
                if (i == s.Length)
                {
                    rs = DwzReturn.SuccessAjaxJsonResultNotClosed("");
                }
                else
                {
                    rs = DwzReturn.FailAjaxJsonResult();
                }

            }
            json.Data = rs;
            return json;
        }
        public JsonResult FuncQueryCondiftionDel(Guid id)
        {
            JsonResult json = new JsonResult();
            AjaxJsonResult rs = null;

            var info = EntityManager<bsSysFuncQuery>.GetByPk<bsSysFuncQuery>("Id", id);
            if (info != null)
            {
                string errMsg = EntityManager<bsSysFuncQuery>.Delete(info);
                if (errMsg.Equals(""))
                {
                    rs = DwzReturn.SuccessAjaxJsonResultNotClosed("");
                }
                else
                {
                    rs = DwzReturn.FailAjaxJsonResult(errMsg);
                }
            }
            json.Data = rs;
            return json;
        }
        public JsonResult FuncQueryCondiftionEdit(FormCollection fc)
        {
            JsonResult json = new JsonResult();
            AjaxJsonResult rs = null;
            if (SaveNoDel(fc))
            {
                rs = DwzReturn.SuccessAjaxJsonResultNotClosed("FunField");
            }
            else
            {
                rs = DwzReturn.FailAjaxJsonResult();
            }
            json.Data = rs;
            return json;
        }
        private static bool SaveNoDel(FormCollection fc)
        {
            bool saveflag = true;
            try
            {
                if (fc.AllKeys[fc.Count - 1].Substring(0, 4) == "item")
                {
                    for (int i = 0; i < Convert.ToInt32(fc.AllKeys[fc.Count - 1].Substring(6, fc.AllKeys[fc.Count - 1].IndexOf('.') - 7)) + 1; i++)
                    {
                        bsSysFuncQuery cdobj = new bsSysFuncQuery();
                        cdobj.Id = Guid.Parse(fc["items[" + i.ToString() + "].Id"].ToString());
                        cdobj.FcId = Guid.Parse(fc["items[" + i.ToString() + "].FcId"].ToString());
                        cdobj.FFQueryNo = Convert.ToInt32(fc["items[" + i.ToString() + "].FFQueryNo"]);
                        cdobj.FFName = fc["items[" + i.ToString() + "].FFName"].ToString();
                        cdobj.FFDesp = fc["items[" + i.ToString() + "].FFDesp"].ToString();
                        cdobj.FFType = fc["items[" + i.ToString() + "].FFType"].ToString();
                        cdobj.QueryType = fc["items[" + i.ToString() + "].QueryType"].ToString();
                        cdobj.FFPositoin = fc["items[" + i.ToString() + "].FFPosition"].ToString();
                        cdobj.Url = fc["items[" + i.ToString() + "].Url"].ToString();
                        if (fc["items[" + i.ToString() + "].Height"] != null && !fc["items[" + i.ToString() + "].Height"].Equals(""))
                            cdobj.Height = Convert.ToInt32(fc["items[" + i.ToString() + "].Height"].ToString());
                        if (EntityManager<bsSysFuncQuery>.Modify(cdobj) != "")
                        {
                            saveflag = false; break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                saveflag = false;
            }

            return saveflag;
        }
        public ActionResult FunOpertion(Guid fcid)
        {
            ViewBag.FcId = fcid;
            List<bsSysFuncOper> objs = EntityManager<bsSysFuncOper>.GetListNoPaging<bsSysFuncOper>("FcId='" + fcid.ToString() + "'", "");
            return View(objs);
        }

        public JsonResult FuncOperAdd(Guid Id)
        {
            JsonResult json = new JsonResult();
            AjaxJsonResult rs = null;
            using (var db = new WSExpressEntities())
            {
                bsSysFuncOper obj = new bsSysFuncOper();
                obj.OperId = Guid.NewGuid();
                obj.FcId = Id;
                obj.OperName = "新增操作";
                obj.IconClass = "add";
                obj.OperStatus = true;
                string errMsg = EntityManager<bsSysFuncOper>.Add<bsSysFuncOper>(obj);
                if (errMsg.Equals(""))
                {
                    rs = DwzReturn.SuccessAjaxJsonResultNotClosed("FuncOper");
                }
                else
                {
                    rs = DwzReturn.FailAjaxJsonResult(errMsg);
                }
            }
            json.Data = rs;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }

        public JsonResult FuncOperDel(Guid id)
        {
            JsonResult json = new JsonResult();
            AjaxJsonResult rs = null;
            using (var db = new WSExpressEntities())
            {
                var info = db.bsSysFuncOper.SingleOrDefault(o => o.OperId == id);
                if (info != null)
                {
                    string errMsg = EntityManager<bsSysFuncOper>.Delete(db, info);
                    if (errMsg.Equals(""))
                    {
                        rs = DwzReturn.SuccessAjaxJsonResultNotClosed("FuncOper");
                    }
                    else
                    {
                        rs = DwzReturn.FailAjaxJsonResult(errMsg);
                    }
                }
            }
            json.Data = rs;
            return json;
        }

        public JsonResult FuncOperEdit(FormCollection fc)
        {
            JsonResult json = new JsonResult();
            AjaxJsonResult rs = null;
            rs = DwzReturn.SuccessAjaxJsonResultNotClosed("");
           
            if (SaveGirdForm(fc) != "")
            //{
            //    try
            //    {
            //        Guid Id = Guid.Parse(fc["items[0].FcId"].ToString());
            //        System.Data.Objects.ObjectContext db = SingletonLinkDb.GetCommon;
            //        db.ExecuteStoreCommand("exec spbsAddFuncLinkAndField2Role '" + Id.ToString() + "'");
            //        db = null;
            //    }
            //    catch(Exception ex) 
            //    {
            //        rs = DwzReturn.FailAjaxJsonResult(ex.Message);
            //    }
            //}
            //else
            {
                rs = DwzReturn.FailAjaxJsonResult();
            }
            json.Data = rs;
            return json;
        }
        private string SaveGirdForm(FormCollection fc)
        {
            try
            {
                if (fc.AllKeys[fc.Count - 1].Substring(0, 4) == "item")
                {
                    for (int i = 0; i < Convert.ToInt32(fc.AllKeys[fc.Count - 1].Substring(6, fc.AllKeys[fc.Count - 1].IndexOf('.') - 7)) + 1; i++)
                    {
                        Guid Id = Guid.Parse(fc["items[" + i.ToString() + "].OperId"].ToString());
                        bsSysFuncOper dbobj = EntityManager<bsSysFuncOper>.GetByPk<bsSysFuncOper>("OperId", Id);

                        dbobj.OperNo = Convert.ToInt32(fc["items[" + i.ToString() + "].OperNo"]);
                        dbobj.FcId = Guid.Parse(fc["items[" + i.ToString() + "].FcId"]);
                        dbobj.OperName = fc["items[" + i.ToString() + "].OperName"].ToString();


                        dbobj.LinkAddr = fc["items[" + i.ToString() + "].LinkAddr"].ToString();

                        dbobj.IconClass = fc["items[" + i.ToString() + "].IconClass"].ToString();



                        dbobj.Target = fc["items[" + i.ToString() + "].Target"].ToString();


                        dbobj.Rel = fc["items[" + i.ToString() + "].Rel"].ToString();
                        if (fc["items[" + i.ToString() + "].Height"].ToString() != null && !fc["items[" + i.ToString() + "].Height"].ToString().Equals(""))
                        {
                            dbobj.Height = Convert.ToInt32(fc["items[" + i.ToString() + "].Height"].ToString());
                        }
                        if (fc["items[" + i.ToString() + "].Width"].ToString() != null && !fc["items[" + i.ToString() + "].Width"].ToString().Equals(""))
                        {
                            dbobj.Width = Convert.ToInt32(fc["items[" + i.ToString() + "].Width"].ToString());
                        }
                        dbobj.AjaxTodoTitle = fc["items[" + i.ToString() + "].AjaxTodoTitle"].ToString();
                        if (fc["items[" + i.ToString() + "].OperStatus"].ToString() == "on")
                        {
                            dbobj.OperStatus = bool.Parse("True");
                        }
                        else
                        {
                            dbobj.OperStatus = bool.Parse("False");
                        }

                        errMsg = EntityManager<bsSysFuncQuery>.Modify(dbobj);
                        if (errMsg != "")
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = ReturnErrMessage(ex);
            }

            return errMsg;
        }
        private string SaveGirdFormAction(FormCollection fc)
        {
            try
            {
                if (fc.AllKeys[fc.Count - 1].Substring(0, 4) == "item")
                {
                    for (int i = 0; i < Convert.ToInt32(fc.AllKeys[fc.Count - 1].Substring(6, fc.AllKeys[fc.Count - 1].IndexOf('.') - 7)) + 1; i++)
                    {
                        Guid Id = Guid.Parse(fc["items[" + i.ToString() + "].ActionId"].ToString());
                        bsSysFuncInterface dbobj = EntityManager<bsSysFuncInterface>.GetByPk<bsSysFuncInterface>("InterfId", Id);

                        dbobj.InterfNo = Convert.ToInt32(fc["items[" + i.ToString() + "].ActionNo"]);
                        dbobj.FcId = Guid.Parse(fc["items[" + i.ToString() + "].FcId"]);
                        dbobj.InterfName = fc["items[" + i.ToString() + "].InterfName"].ToString();
                        dbobj.RetObject = fc["items[" + i.ToString() + "].RetObject"].ToString();
                        dbobj.RetType = fc["items[" + i.ToString() + "].RetType"].ToString();
                        dbobj.InterfDesp = fc["items[" + i.ToString() + "].InterfDesp"].ToString();
                        dbobj.InterfParams = fc["items[" + i.ToString() + "].InterfParams"].ToString();
                        errMsg = EntityManager<bsSysFuncQuery>.Modify(dbobj);
                        if (errMsg != "")
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = ReturnErrMessage(ex);
            }

            return errMsg;
        }

        public JsonResult AfterCommOpertionSelected(string MulSelFilter, string fcid, string Ids)
        {
            JsonResult json = new JsonResult();
            AjaxJsonResult rs = null;
            try
            {
                using (WSExpressEntities db = new WSExpressEntities())
                {
                    string sql = "insert into bsSysFuncOper ";
                    sql += " select NEWID(),'" + fcid + "',OperNo,OperName,LinkAddr,IconClass,[Target],[Height],[Width],[Rel],[AjaxTodoTitle],[OperStatus] from bsSysFuncOper where FcId is null and  OperNo in (" + Ids + ")";
	
                    db.ExecuteStoreCommand(sql);
                }
                rs = DwzReturn.SuccessAjaxJsonResult("FunOpertion");
            }
            catch(Exception ex)
            {
                rs = DwzReturn.FailAjaxJsonResult(ex.Message);
            }
            json.Data = rs;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }




        public ActionResult CreateViewCode(Guid id)
        {
            JsonResult json = new JsonResult();
            AjaxJsonResult rs = null;

            bsSysFunc fc = bsSysFunManager.SelectByid(id);
            if (fc.FunType.ToLower() == FuncType.Sub.ToString().ToLower())
            {
                rs = new AjaxJsonResult
                {
                    statusCode = "300",
                    message = "sub类型不需生成",
                    navTabId = "",
                    rel = "",
                    callbackType = "",
                    forwardUrl = ""
                };
            }
            else
            {
                bool ret = CreateCode.CreateView(fc);

                if (ret)
                {
                    rs = DwzReturn.SuccessAjaxJsonResultNotClosed("");
                }
                else
                {
                    rs = DwzReturn.FailAjaxJsonResult();
                }
            }
            json.Data = rs;

            return json;
        }

        public ActionResult CreateContrCode(Guid id)
        {
            JsonResult json = new JsonResult();

            AjaxJsonResult rs = null;

            bsSysFunc fc = bsSysFunManager.SelectByid(id);
            if (fc.FunType.ToLower() == FuncType.Sub.ToString().ToLower())
            {
                rs = new AjaxJsonResult
                {
                    statusCode = "300",
                    message = "sub类型不需生成",
                    navTabId = "",
                    rel = "",
                    callbackType = "",
                    forwardUrl = ""
                };
            }
            else
            {
                bool ret = CreateCode.CreateContr(fc);

                if (ret)
                {
                    rs = DwzReturn.SuccessAjaxJsonResultNotClosed("");
                }
                else
                {
                    rs = DwzReturn.FailAjaxJsonResult();
                }
            }
            json.Data = rs;

            return json;
        }
        public string bsSysFuncFc(FormCollection fc, int BroseFlag)
        {
            bsSysFunc obj = new bsSysFunc();
            if (BroseFlag == 2)
            {
                Guid guid = new Guid(fc["bsSysFunc_FcId"]);
                obj = EntityManager<bsSysFunc>.GetByPk<bsSysFunc>("FcId", guid);
            }
            else if (BroseFlag == 1)
            {
                if (fc["bsSysFunc_FunId"] != null && !fc["bsSysFunc_FunId"].Equals(""))
                {
                    Guid guid = new Guid(fc["bsSysFunc_FunId"]);
                    obj.FunId = guid;
                }
                obj.FcId = Guid.NewGuid();
            }
            if (fc != null)
            {
                if (BroseFlag == 2)
                {
                    obj=EntityManager<bsSysFunc>.GetByPk<bsSysFunc>("FcId", Guid.Parse(fc["bsSysFunc_FcId"].ToString()));

                }
                if (!(fc["bsSysFunc_FunLayout"].ToString().Equals("") || fc["bsSysFunc_FunLayout"].ToString() == null))
                {
                    obj.FunLayout = fc["bsSysFunc_FunLayout"].ToString();
                }
                if (!(fc["bsSysFunc_FuncCode"].ToString().Equals("") || fc["bsSysFunc_FuncCode"].ToString() == null))
                {
                    obj.FuncCode = fc["bsSysFunc_FuncCode"];
                }
                if (!(fc["bsSysFunc_FuncName"].ToString().Equals("") || fc["bsSysFunc_FuncName"].ToString() == null))
                {
                    obj.FuncName = fc["bsSysFunc_FuncName"];
                }

                //if (!(fc["bsSysFunc_MainAjaxUrl"].ToString().Equals("") || fc["bsSysFunc_MainAjaxUrl"].ToString() == null))
                //{
                //    obj.PartsPixel = fc["bsSysFunc_PartsPixel"].ToString();
                //}
                if (!(fc["bsSysFunc_FunType"].ToString().Equals("") || fc["bsSysFunc_FunType"].ToString() == null))
                {
                    obj.FunType = fc["bsSysFunc_FunType"].ToString();
                }
                if (!(fc["bsSysFunc_PFuncCode"].ToString().Equals("") || fc["bsSysFunc_PFuncCode"].ToString() == null))
                {
                    obj.PFuncCode = fc["bsSysFunc_PFuncCode"];
                }
                if (!(fc["bsSysFunc_FunContr"].ToString().Equals("") || fc["bsSysFunc_FunContr"].ToString() == null))
                {
                    obj.FunContr = fc["bsSysFunc_FunContr"].ToString();
                }
                if (!(fc["bsSysFunc_ListDbObj"].ToString().Equals("") || fc["bsSysFunc_ListDbObj"].ToString() == null))
                {
                    obj.ListDbObj = fc["bsSysFunc_ListDbObj"].ToString();
                }
                if (!(fc["bsSysFunc_TName"].ToString().Equals("") || fc["bsSysFunc_TName"].ToString() == null))
                {
                    obj.TName = fc["bsSysFunc_TName"].ToString();
                }
                if (!(fc["bsSysFunc_TPK"].ToString().Equals("") || fc["bsSysFunc_TPK"].ToString() == null))
                {
                    obj.TPK = fc["bsSysFunc_TPK"].ToString();
                }
                if (!(fc["bsSysFunc_NotNullField"].ToString().Equals("") || fc["bsSysFunc_NotNullField"].ToString() == null))
                {
                    obj.NotNullField = fc["bsSysFunc_NotNullField"].ToString();
                }


                //if (!(fc["bsSysFunc_Queryfields"].ToString().Equals("") || fc["bsSysFunc_Queryfields"].ToString() == null))
                //{
                //    obj.Queryfields = fc["bsSysFunc_Queryfields"].ToString();
                //}


                if (!(fc["bsSysFunc_Orderbys"].ToString().Equals("") || fc["bsSysFunc_Orderbys"].ToString() == null))
                {
                    obj.Orderbys = fc["bsSysFunc_Orderbys"].ToString();
                }
                if (!(fc["bsSysFunc_Filterfield"].ToString().Equals("") || fc["bsSysFunc_Filterfield"].ToString() == null))
                {
                    obj.Filterfield = fc["bsSysFunc_Filterfield"].ToString();
                }
                //if (!(fc["bsSysFunc_Queryfields"].ToString().Equals("") || fc["bsSysFunc_Queryfields"].ToString() == null))
                //{
                //    obj.Queryfields = fc["bsSysFunc_Queryfields"].ToString();
                //}
                if (!(fc["bsSysFunc_Filtervalue"].ToString().Equals("") || fc["bsSysFunc_Filtervalue"].ToString() == null))
                {
                    obj.Filtervalue = fc["bsSysFunc_Filtervalue"].ToString();
                }
                if (!(fc["bsSysFunc_PFK"].ToString().Equals("") || fc["bsSysFunc_PFK"].ToString() == null))
                {
                    obj.PFK = fc["bsSysFunc_PFK"].ToString();
                }
                if (!(fc["bsSysFunc_SaveOperator"] == null))
                {
                    obj.SaveOperator = bool.Parse(fc["bsSysFunc_SaveOperator"]);

                }


                if (!(fc["bsSysFunc_CreateLookup"] == null))
                {
                    obj.CreateLookup = bool.Parse(fc["bsSysFunc_CreateLookup"]);
                }
                if (!(fc["bsSysFunc_LookupFields"].ToString().Equals("") || fc["bsSysFunc_LookupFields"].ToString() == null))
                {
                    obj.LookupFields = fc["bsSysFunc_LookupFields"].ToString();
                }
                if (!((fc["bsSysFunc_CreateJson"] == null) || fc["bsSysFunc_CreateJson"].Equals("")))
                {
                    obj.CreateJson = bool.Parse(fc["bsSysFunc_CreateJson"]);
                }
                if (!(fc["bsSysFunc_JsonFields"].ToString().Equals("") || fc["bsSysFunc_JsonFields"].ToString() == null))
                {
                    obj.JsonFields = fc["bsSysFunc_JsonFields"].ToString();
                }
                if (!(fc["bsSysFunc_SaveAuditer"] == null))
                {
                    obj.SaveAuditer = bool.Parse(fc["bsSysFunc_SaveAuditer"]);
                }
                if (!(fc["bsSysFunc_UseStrategy"] == null))
                {
                    obj.UseStrategy = bool.Parse(fc["bsSysFunc_UseStrategy"]);
                }

                if (!(fc["bsSysFunc_Usestatus"].ToString().Equals("") || fc["bsSysFunc_Usestatus"].ToString() == null))
                {
                    obj.Usestatus = int.Parse(fc["bsSysFunc_Usestatus"]);
                }

            }
            obj.Operartor = Login_Nick_Name;
            obj.Updatedt = DateTime.Now;
            string errMsg = "error";
            if (BroseFlag == 2)
            {
                errMsg = EntityManager<bsSysFunc>.Modify(obj);
            }
            else if (BroseFlag == 1)
            {
                errMsg = EntityManager<bsSysFunc>.Add(obj);

            }
            return errMsg;
        }
        public bsSysFuncQuery bsSysFuncQueryFc(bsSysFuncQuery obj,FormCollection fc)
        {
            if (fc["bsSysFuncQuery_FFQueryNo"] != null && !fc["bsSysFuncQuery_FFQueryNo"].Equals(""))
            {
                obj.FFQueryNo = int.Parse(fc["bsSysFuncQuery_FFQueryNo"]);
            }
            if (fc["bsSysFuncQuery_FFName"] != null && !fc["bsSysFuncQuery_FFName"].Equals(""))
            {
                obj.FFName = fc["bsSysFuncQuery_FFName"];
            }
            if (fc["bsSysFuncQuery_QueryType"] != null &&! fc["bsSysFuncQuery_QueryType"].Equals(""))
            {
                obj.QueryType = fc["bsSysFuncQuery_QueryType"];
            }
            if (fc["bsSysFuncQuery_FFPosition"] != null && !fc["bsSysFuncQuery_FFPosition"].Equals(""))
            {
                obj.FFPositoin = fc["bsSysFuncQuery_FFPosition"];
            }
            if (fc["bsSysFuncQuery_Url"] != null && !fc["bsSysFuncQuery_Url"].Equals(""))
            {
                obj.Url = fc["bsSysFuncQuery_Url"];
            }
            if (fc["bsSysFuncQuery_Height"] != null && !fc["bsSysFuncQuery_Height"].Equals(""))
            {
                obj.Height = int.Parse(fc["bsSysFuncQuery_Height"]);
            }
            return obj;
        }
    }
}
