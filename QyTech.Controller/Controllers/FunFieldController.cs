using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SunMvcExpress.Dao;
using SunMvcExpress.Core.Models;
using SunMvcExpress.Core.BLL;
using SunMvcExpress.Core.Helpers;
using SunMvcExpress.BLL;
using SunMvcExpress.Core.Common;

using QyTech.Core.Common;
using QyTech.Core.Helpers;
using QyTech.Core.BLL;

namespace SunMvcExpress.Controllers
{
	//[LoginFilter]
	public class FunFieldController : Controller
	{
		public Guid Login_User_ID = LoginHelper.GetLoginUserId();
		public string Login_Nick_Name = LoginHelper.GetLoginNickName();
		public Guid Login_UserOrgId = LoginHelper.GetLoginCompanyId();
        public bsOrganize Login_UserOrgName = LoginHelper.GetLoginCompany();




		public ActionResult FunField(string fcid,string types,string keys,string values, string operators,string conditions,string orderbys,string query_FDesp,string query_FName,string orderField,string orderMethod,string funid="a97024e6-b5d3-44af-af4e-e57a67d49dfc",int pageNum=1, int numPerPage=20)
		{
			int totalCount;
			orderField="FleldNo";
			List<bsFuncField> objs=new List<bsFuncField>();
            if (conditions == null)
                conditions = "";
            conditions+="fcid='"+fcid+"'";
			objs = EntityManager<bsFuncField>.Paging<bsFuncField>(Login_User_ID, "","",types, keys, values, operators,conditions, orderbys, orderField, OrderMethod.ASC, pageNum, numPerPage, out totalCount);

			ViewBag.numPerPage = numPerPage;
			ViewBag.pageNum = pageNum;
			ViewBag.totalCount = totalCount;
			ViewBag.FDesp=query_FDesp;
			ViewBag.FName=query_FName;
            ViewBag.fkcondtion = fcid;
            ViewBag.FcId = fcid;
			
			return View(objs);
		}



        public ActionResult FunFieldAdd(Guid? FcId, int pageNum = 1, int numPerPage = 50)
        {

            bsSysFunc obj = EntityManager<bsSysFunc>.GetByPk<bsSysFunc>("FcId", (Guid)FcId);
            // obj = EntityManager<bsSysFunc>.GetByPk<bsSysFunc>("FcId", id);
            bsTable bstobj = EntityManager<bsTable>.GetByStringFieldName<bsTable>("TName", obj.ListDbObj);
            List<bsField> objs = EntityManager<bsField>.GetListNoPaging<bsField>("bsT_Id='" + bstobj.Id.ToString() + "'", "FleldNo asc");

            ViewBag.FcId = FcId;
            ViewBag.bsF_Id = bstobj.Id;
            return View(objs);

        }

        public JsonResult FunFieldAddData(string Id, Guid FcId)
        {
            JsonResult json = new JsonResult();
            AjaxJsonResult rs = null;

            int ret = EntityManager<object>.ExecuteSql("exec bsAddFunDateToFuncField '" + FcId + "','" + Id + "'");
           
            //string[] s = Id.Split(new char[] { ',' });
            //int i;

            //for (i = 0; i < s.Length; i++)
            //{
            //    Guid bsfid = new Guid(s[i]);
            //    EntityManager<object>.ExecuteSql("exec bsAddFunDateToFuncField '" + FcId + "','" + bsfid + "'");
            //}
            if (ret!=-1)
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

		public JsonResult FunFieldDel(Guid id)
		{
			JsonResult json = new JsonResult();
			AjaxJsonResult rs = null;
			using (var db = new WSExpressEntities())
			{
			    var info = db.bsFuncField.SingleOrDefault(o => o.Id == id);
				    if (info != null)
				    {
                        string errMsg=EntityManager<bsSysFunc>.Delete(db, info);
                        if (errMsg.Equals(""))
				        {
                            rs = DwzReturn.SuccessAjaxJsonResultNotClosed("FunField");
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

        public JsonResult FunFieldEdit(FormCollection fc)
        {
            JsonResult json = new JsonResult();
            AjaxJsonResult rs = null;
            if (SaveNoDel(fc))
            {
                try
                {
                    Guid Id = Guid.Parse(fc["items[0].FcId"].ToString());
                    SingletonLinkDb.GetCommon.ExecuteStoreCommand("exec spbsAddFuncLinkAndField2Role '" + Id.ToString() + "'");
                }
                catch (Exception ex)
                {
                    rs = DwzReturn.FailAjaxJsonResult(ex);
                }
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
                        bsFuncField cdobj = new bsFuncField();
                        
                        cdobj.Id = Guid.Parse(fc["items[" + i.ToString() + "].Id"].ToString());
                        bsFuncField dbobj = EntityManager<bsFuncField>.GetByPk<bsFuncField>("Id", cdobj.Id);
                        dbobj.FcId = Guid.Parse(fc["items[" + i.ToString() + "].FcId"].ToString());
                        dbobj.FleldNo = Convert.ToInt32(fc["items[" + i.ToString() + "].FleldNo"]);
                        dbobj.TName = fc["items[" + i.ToString() + "].TName"].ToString();
                        dbobj.FName = dbobj.FName;
                        dbobj.FDesp = fc["items[" + i.ToString() + "].FDesp"].ToString();
                        dbobj.IsFieldRight = fc["items[" + i.ToString() + "].IsFieldRight"] == null ? false : true;
                        dbobj.TypeGroup = dbobj.TypeGroup;
                        dbobj.Type = dbobj.Type;
                        dbobj.EditType = fc["items[" + i.ToString() + "].EditType"].ToString();
                        dbobj.LookupLink = fc["items[" + i.ToString() + "].LookupLink"].ToString();
                        dbobj.DefalutValue = fc["items[" + i.ToString() + "].DefalutValue"].ToString();
                        dbobj.ListNo = Convert.ToInt32(fc["items[" + i.ToString() + "].ListNo"]);
                        dbobj.ListFWidth = Convert.ToInt32(fc["items[" + i.ToString() + "].ListFWidth"]);

                        dbobj.ListVisible = fc["items[" + i.ToString() + "].ListVisible"] == null ? false : true;
                        dbobj.ListEditable = fc["items[" + i.ToString() + "].ListEditable"] == null ? false : true;
                        dbobj.FormNo = Convert.ToInt32(fc["items[" + i.ToString() + "].FormNo"]);
                        dbobj.FormFWidth = Convert.ToInt32(fc["items[" + i.ToString() + "].FormFWidth"]);
                        dbobj.FormVisible = fc["items[" + i.ToString() + "].FormVisible"] == null ? false : true;
                        dbobj.FormEditable = fc["items[" + i.ToString() + "].FormEditable"] == null ? false : true;

                        if (EntityManager<bsFuncField>.Modify(dbobj) != "")
                        {
                            saveflag = false; break;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                saveflag = false;
                //throw new Exception(ex.Message);
            }

            return saveflag;
        }
   
		public JsonResult FunFieldEdit_1(bsFuncField obj,FormCollection fc)
		{
			JsonResult json = new JsonResult();
			AjaxJsonResult rs = null;
			bsFuncField objdb = EntityManager<bsFuncField>.GetByPk<bsFuncField>("Id",obj.Id);
			objdb.Id=obj.Id;
            //objdb.TName=obj.TName;
            //objdb.FleldNo=obj.FleldNo;
            //objdb.FName=obj.FName;
			objdb.FDesp=obj.FDesp;
			objdb.TypeGroup=obj.TypeGroup;
            //objdb.Type=obj.Type;
			objdb.EditType=obj.EditType;
			objdb.LookupLink=obj.LookupLink;
			objdb.DefalutValue=obj.DefalutValue;
			objdb.ListNo=obj.ListNo;
			objdb.ListFWidth=obj.ListFWidth;
			objdb.ListVisible=obj.ListVisible;
			objdb.ListEditable=obj.ListEditable;
			objdb.FormNo=obj.FormNo;
			objdb.FormFWidth=obj.FormFWidth;
			objdb.FormVisible=obj.FormVisible;
			objdb.FormEditable=obj.FormEditable;
            string errMsg=EntityManager<bsFuncField>.Modify(objdb);
            if (errMsg.Equals(""))
			{
                rs = DwzReturn.SuccessAjaxJsonResult("editpage");
			 }
			 else
			 {
                 rs = DwzReturn.FailAjaxJsonResult(errMsg);
			}
			json.Data = rs;
			return json;
		}
	}
}
