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
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using SunMvcExpress.Core;
using SunMvcExpress.BLL;

using QyTech.Core.BLL;
using QyTech.Core.Common;
using QyTech.Core.Helpers;

namespace SunMvcExpress.Controllers
{
	
	public class bsTableController : WSExpressController
	{
		public string Login_Nick_Name = LoginHelper.GetLoginNickName();
		public ActionResult bsTable(string query_Name,string query_Desp,string query_TType, string orderField,string funid="6d42eeee-4bd6-40a0-ab03-e5179cc28951",int pageNum=1, int numPerPage=50)
		{
            if (!SunMvcApp.isValid())
            {
                return null;
            }
			int totalCount;
            orderField = "No";
			List<bsTable> objs=new List<bsTable>();
			bsTableManager objm=new bsTableManager();
            string sqls=" ";
            if (query_Name != null && !query_Name.Equals(""))
            {
                sqls += "TName like '%" + query_Name + "%' and ";
            }
            if (query_Desp != null && !query_Desp.Equals(""))
            {
                sqls += "Desp like '%" + query_Desp + "%' and ";
            }
            if (query_TType != null && !query_TType.Equals(""))
            {
                sqls += "TType like '%" + query_TType + "%' and ";
            }
            if (sqls.Length>1)
                sqls=sqls.Substring(0,sqls.Length-4);
			objs= EntityManager<bsTable>.Paging<bsTable>(Login_User_ID,sqls,orderField,pageNum, numPerPage, out totalCount);
			
            ViewBag.numPerPage = numPerPage;
			ViewBag.pageNum = pageNum;
			ViewBag.totalCount = totalCount;
            ViewBag.Name = query_Name;
            ViewBag.Desp = query_Desp;
            ViewBag.TType = query_TType;
			return View(objs);
		}
        public ActionResult bsTableLookup(string query_Name, string orderField, int pageNum = 1, int numPerPage = 50)
		{
            if (!SunMvcApp.isValid())
            {
                return null;
            }
			int totalCount;
            orderField = "No";
			List<bsTable> objs=EntityManager<bsTable>.GetGirdListwithPaging<bsTable>("","No asc",pageNum, numPerPage, out totalCount);
			ViewBag.numPerPage = numPerPage;
			ViewBag.pageNum = pageNum;
			ViewBag.totalCount = totalCount;
			ViewBag.Name=query_Name;
            
            return View(objs);
		}
        
		public ActionResult bsTableBrowse(Guid id)
		{
		    bsTable obj;
		    using (var db = new WSExpressEntities())
		    {
		        obj = db.bsTable.Single(t => t.Id == id);
		        ViewBag.DetailObj = db.bsField.Where(d => d.bsT_Id == obj.Id).ToList<bsField>();
		        return View(obj);
		    }
		}
		public ActionResult bsTableAdd(bsTable obj,Guid? id, FormCollection fc,string BrowseFlag="false")
		{
			JsonResult json = new JsonResult();
            ViewBag.BrowseFlag = BrowseFlag;
            AjaxJsonResult rs = null;
			    
			if (id == null && obj!=null && obj.TName!=null && !obj.TName.Equals(""))
			{
			    obj.Id = Guid.NewGuid();
                obj.TType = "表";
                if (Save(AddorModify.Add, obj, fc))
                {
                    rs = DwzReturn.SuccessAjaxJsonResult("editpage");
                }
                else
                {
                    rs = DwzReturn.FailAjaxJsonResult();
                }
			    json.Data = rs;
                ViewBag.BrowseFlag = BrowseFlag;
			    return json;
			}
			else
			{
			    if (id != null)
			    {
			        using (var db = new WSExpressEntities())
			        {
			            var objdb = db.bsTable.Single(t => t.Id == id);
			            ViewBag.DetailObj = db.bsField.OrderBy(p=>p.FleldNo).Where(d => d.bsT_Id == objdb.Id).ToList<bsField>();
			            return View(objdb);
			        }
			    }
                bsTable addobj = new bsTable();
                addobj.TName = "NewTable";
                addobj.Id = Guid.NewGuid();
                obj.TType = "表";
                
                errMsg=EntityManager<bsTable>.Add<bsTable>(addobj);
                if (errMsg != "")
                {
                    rs = DwzReturn.FailAjaxJsonResult();
                    json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    json.Data = rs;
                    return json;
                }
                else
                {
                    ViewBag.DetailObj = new List<bsField>();
                    rs = DwzReturn.SuccessAjaxJsonResultNotClosed("bsTable");
                    json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    ViewBag.BrowseFlag = BrowseFlag;
                    json.Data = rs;
                    return json;
                    //return View(addobj);
                }
			}
		}

        public JsonResult bsTableDel(Guid id)
        {
            JsonResult json = new JsonResult();
            using (var db = new WSExpressEntities())
            {
                var info = db.bsTable.SingleOrDefault(o => o.Id == id);

                    if (info != null)
                    {
                        AjaxJsonResult rs = null;
                        db.ExecuteStoreCommand("exec bsDeletetable '"+id.ToString()+"'");
                        string errMsg = EntityManager<bsTable>.Delete<bsTable>(db, info);
                        if (errMsg.Equals(""))
                        {
                            rs = DwzReturn.SuccessAjaxJsonResultNotClosed("bsTable");
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
        public JsonResult bsFieldAdd(Guid id, string BrowseFlag)
        {
            JsonResult json = new JsonResult();
            AjaxJsonResult rs = null;
            if (BrowseFlag != "true")
            {
                bsField obj = new bsField();
                obj.Id = Guid.NewGuid();
                obj.bsT_Id = id;
                obj.Name = "新增";
                obj.Lenght = 10;
                obj.DeciDigits = 2;
                errMsg = EntityManager<bsField>.Add<bsField>(obj);
                if (errMsg.Equals(""))
                {
                    rs = DwzReturn.SuccessAjaxJsonResultNotClosed("add");
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
                errMsg = "当前不能添加字段！";
                rs = DwzReturn.FailAjaxJsonResult(errMsg);
                json.Data = rs;
                return json;

            }
        }
        public JsonResult bsFieldDel(Guid id)
        {
            JsonResult json = new JsonResult();
            AjaxJsonResult rs = null;
               
            using (var db = new WSExpressEntities())
            {
                var info = db.bsField.SingleOrDefault(o => o.Id == id);
                if (info != null)
                {
                    string errMsg=EntityManager<bsField>.Delete(db, info);
                    if (errMsg.Equals(""))
                    {
                        rs = DwzReturn.SuccessAjaxJsonResultNotClosed("add");
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
        public JsonResult bsTableEdit(FormCollection fc)
		{
            bsTable obj = EntityManager<bsTable>.GetByPk<bsTable>("Id",new Guid(fc["bsTable_Id"]));
            
			JsonResult json = new JsonResult();
			AjaxJsonResult rs = null;
            if (SaveNoDel(AddorModify.Modify, obj, fc))
            {
                rs = DwzReturn.SuccessAjaxJsonResultNotClosed("editpage");
            }
            else
            {
                rs = DwzReturn.FailAjaxJsonResult();
            }
			json.Data = rs;
			return json;
		}
		private static bool Save(AddorModify amflag, bsTable obj, FormCollection fc)
		{
		   bool saveflag = true;
		   if (amflag == AddorModify.Add)
		   {
		       if (EntityManager<bsTable>.Add(obj)!="")
		       {
		           saveflag = false;
		       }
		   }
		   else
		   {
		       if (EntityManager<bsTable>.Modify(obj)!="")
		       {
		           saveflag = false;
		       }
		       else
		       {
		           bsTableManager.LDeleteDetailByPid(obj.Id);
		       }
		   }
		   if (saveflag)
		   {
		       try
		       {
		           if (fc.AllKeys[fc.Count - 1].Substring(0, 11) == "item")
		           {
			           for (int i = 0; i < Convert.ToInt32(fc.AllKeys[fc.Count - 1].Substring(6, fc.AllKeys[fc.Count - 1].IndexOf('.') - 7))+1; i++)
			           {
			                bsField cdobj = new bsField();
			               cdobj.Id= Guid.NewGuid();
			               cdobj.bsT_Id=obj.Id;
                           cdobj.FleldNo = Convert.ToInt32(fc["items[" + i.ToString() + "].FleldNo"]);
                           cdobj.Name = fc["items[" + i.ToString() + "].Name"].ToString();
                           cdobj.Desp = fc["items[" + i.ToString() + "].Desp"].ToString();
                           cdobj.Type = fc["items[" + i.ToString() + "].Type"].ToString();
                           if (fc["items[" + i.ToString() + "].Lenght"].ToString() == "")
			                    cdobj.Lenght = null;
                           else
                               cdobj.Lenght = Convert.ToInt32(fc["items[" + i.ToString() + "].Lenght"]);

                           if (fc["items[" + i.ToString() + "].DeciDigits"].ToString() == "")
                               cdobj.DeciDigits = null;
                           else
                               cdobj.DeciDigits = fc["items[" + i.ToString() + "].DeciDigits"].ToString() == "" ? 0 : Convert.ToInt32(fc["items[" + i.ToString() + "].DeciDigits"]);
			
			               if (EntityManager<bsField>.Add(cdobj)!="")
			               {
			                   saveflag = false; break;
			               }
			           }
		           }
		           bsTableManager.PDeleteDetailByPid(obj.Id);
		       }
		       catch
		       {
		           saveflag = false;
		           bsTableManager.LRestoreDetailByPid(obj.Id);
		       }
		   }
		  return saveflag;
		}

        private static bool SaveNoDel(AddorModify amflag, bsTable obj, FormCollection fc)
        {
            bool saveflag = true;
            AddorModify childflag = AddorModify.Add;
            obj.TName = fc["bsTable_TName"];
            obj.TPk = fc["bsTable_TPK"];
            obj.No = Convert.ToInt32(fc["bsTable_No"]);
            obj.Desp = fc["bsTable_Desp"];
            if (amflag == AddorModify.Add)
            {
                if (EntityManager<bsTable>.Add(obj)!="")
                    saveflag = false;
            }
            else
            {
                if (EntityManager<bsTable>.Modify(obj)!="")
                    saveflag = false;
            }
            if (saveflag)
            {
                try
                {
                    if (fc.AllKeys[fc.Count - 1].Substring(0, 4) == "item")
                    {
                        for (int i = 0; i < Convert.ToInt32(fc.AllKeys[fc.Count - 1].Substring(6, fc.AllKeys[fc.Count - 1].IndexOf('.') - 7)) + 1; i++)
                        {
                            bsField cdobj = new bsField();
                            if (fc["items[" + i.ToString() + "].Id"].ToString() == "")
                            {
                                childflag = AddorModify.Add;
                           
                                cdobj.Id = Guid.NewGuid();
                                cdobj.bsT_Id = obj.Id;
                            }
                            else
                            {
                                childflag = AddorModify.Modify;
                                cdobj = EntityManager<bsField>.GetByPk<bsField>("Id", Guid.Parse(fc["items[" + i.ToString() + "].Id"].ToString()));
                            }
                            cdobj.FleldNo = Convert.ToInt32(fc["items[" + i.ToString() + "].FleldNo"]);
                            cdobj.Name = fc["items[" + i.ToString() + "].Name"].ToString();
                            cdobj.Desp = fc["items[" + i.ToString() + "].Desp"].ToString();
                            cdobj.Type = fc["items[" + i.ToString() + "].Type"].ToString();
                            if (fc["items[" + i.ToString() + "].Lenght"].ToString() == "")//长度没有传进去
                                cdobj.Lenght = null;
                            else
                                cdobj.Lenght = Convert.ToInt32(fc["items[" + i.ToString() + "].Lenght"]);

                            if (fc["items[" + i.ToString() + "].DeciDigits"].ToString() == "")
                                cdobj.DeciDigits = null;
                            else
                                cdobj.DeciDigits = fc["items[" + i.ToString() + "].DeciDigits"].ToString() == "" ? 0 : Convert.ToInt32(fc["items[" + i.ToString() + "].DeciDigits"]);

                            cdobj.NotNull = fc["items[" + i.ToString() + "].NotNull"] == null ? false : true;

                            if (childflag == AddorModify.Add)
                            {
                                if (EntityManager<bsField>.Add(cdobj) != "")
                                {
                                    saveflag = false; break;
                                }
                            }
                            else
                            {
                                if (EntityManager<bsField>.Modify(cdobj) != "")
                                {
                                    saveflag = false; break;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    saveflag = false;
                }
            }
            return saveflag;
        }


        public JsonResult bsTableCreate(Guid id)
        {
            JsonResult json = new JsonResult();
            AjaxJsonResult rs = null;
              try
            {
                using (WSExpressEntities db = new WSExpressEntities())
                {

                    if (!db.bsField.Where(p => p.Id == id).Equals(""))
                    {
                        db.ExecuteStoreCommand("exec bsCreateTable '" + id.ToString() + "'");
                        rs = DwzReturn.SuccessAjaxJsonResultNotClosed("");
                    }
                    else
                        rs = DwzReturn.FailAjaxJsonResult("表中字段数为零，不能创建！");
                }
            }
            catch (Exception ex)
            {
                rs = DwzReturn.FailAjaxJsonResult(ex.Message);
            }
            json.Data = rs;
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }
        //补充表的字段信息
        public ActionResult AppendCreatedTableInfoTobsTable(Guid id)
        {
            bool ret = true;
            try
            {
                using (WSExpressEntities db = new WSExpressEntities())
                {
                    bsTable obj = new bsTable();
                    obj = db.bsTable.Single(t => t.Id == id);
                    db.ExecuteStoreCommand("exec bsAppendCreatedTableInfoToBsTable '" + obj.TName + "','" + obj.Desp + "','0','0','" + obj.TPk + "'");
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }
            JsonResult json = new JsonResult();
            AjaxJsonResult rs = null;

           if (ret)
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

        public ActionResult AppendCreatedTableInfo()
        {
            bool ret = true;
            try
            {
                using (WSExpressEntities db = new WSExpressEntities())
                {
                    db.ExecuteStoreCommand("exec bsCreateBsTableData");
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }
            JsonResult json = new JsonResult();
            AjaxJsonResult rs = null;
          if (ret)
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
    

        public ActionResult AddInputerInfo(Guid id)
        {
            JsonResult json = new JsonResult();
            AjaxJsonResult rs = null;
            List<bsField> info = new List<bsField>();
            info = EntityManager<bsField>.GetListNoPaging<bsField>(Login_User_ID, "bsT_Id='" + id.ToString() + "'", "", "", "");
            for (int i = 0; i < info.Count(); i++)
            {
                if (info[i].Desp == "操作人")
                {
                    rs = DwzReturn.FailAjaxJsonResult("  操作人已存在！");
                    json.Data = rs;
                    return json;
                }
            }
            bool ret = true;
            try
            {
                using (WSExpressEntities db = new WSExpressEntities())
                {
                    bsTable obj = new bsTable();
                    obj = db.bsTable.Single(t => t.Id == id);
                    db.ExecuteStoreCommand("exec bsAddOperAndAuditFieldsToTable '" + obj.TName + "','1'");
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }
            //bool ret = false;// CreateCode.AppendSomeFieldData(id, 1);
            if (ret)
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

        public ActionResult AddAuditerInfo(Guid id)
        {
             JsonResult json = new JsonResult();
            AjaxJsonResult rs = null;

            List<bsField> info = new List<bsField>();
            info = EntityManager<bsField>.GetListNoPaging<bsField>(Login_User_ID, "bsT_Id='" + id.ToString() + "'", "", "", "");
            for (int i = 0; i < info.Count(); i++)
            {
                if (info[i].Desp == "审核人")
                {
                    rs = DwzReturn.FailAjaxJsonResult("  审核人字段已存在！");
                    json.Data = rs;
                    return json;
                }
            }

             bool ret = true;
            try
            {
                using (WSExpressEntities db = new WSExpressEntities())
                {
                    bsTable obj = new bsTable();
                    obj = db.bsTable.Single(t => t.Id == id);
                    db.ExecuteStoreCommand("exec bsAddOperAndAuditFieldsToTable '" + obj.TName + "','2'");
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }
            if (ret)
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

        public ActionResult AddDataStatusInfo(Guid id)
        {
            JsonResult json = new JsonResult();
            AjaxJsonResult rs = null;
            List<bsField> info = new List<bsField>();

            info = EntityManager<bsField>.GetListNoPaging<bsField>(Login_User_ID, "bsT_Id='" + id.ToString() + "'", "", "", "");
            for (int i = 0; i < info.Count(); i++)
            {
                if (info[i].Desp == "逻辑删除")
                {
                    rs = DwzReturn.FailAjaxJsonResult("  逻辑删除字段已存在！");
                    json.Data = rs;
                    return json;
                }
            }
            bool ret = true;
            try
            {
                using (WSExpressEntities db = new WSExpressEntities())
                {
                    bsTable obj = new bsTable();
                    obj = db.bsTable.Single(t => t.Id == id);
                    db.ExecuteStoreCommand("exec bsAddOperAndAuditFieldsToTable '" + obj.TName + "','3'");
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }

            if (ret)
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

        public ActionResult AddOrgInfo(Guid id)
        {
            JsonResult json = new JsonResult();
            AjaxJsonResult rs = null;

            List<bsField> info = new List<bsField>();
            info = EntityManager<bsField>.GetListNoPaging<bsField>(Login_User_ID, "bsT_Id='" + id.ToString() + "'", "", "", "");
            for (int i = 0; i < info.Count(); i++)
            {
                if (info[i].Desp == "单位id")
                {
                    rs = DwzReturn.FailAjaxJsonResult("  部门字段已存在！");
                    json.Data = rs;
                    return json;
                }
            }

            bool ret = true;
            try
            {
                using (WSExpressEntities db = new WSExpressEntities())
                {
                    bsTable obj = new bsTable();
                    obj = db.bsTable.Single(t => t.Id == id);
                    db.ExecuteStoreCommand("exec bsAddOperAndAuditFieldsToTable '" + obj.TName + "','4'");
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }

           // bool ret = false;// CreateCode.AppendSomeFieldData(id, 4);
            if(ret)
            {
                rs = DwzReturn.SuccessAjaxJsonResultNotClosed("");
            }
            else
            {
                rs = DwzReturn.FailAjaxJsonResult();
            }
            json.Data =rs;
            return json;
        }


        public JsonResult SelectTableSuggest()
        {
            List<bsTable> objs = new List<bsTable>();
            objs = EntityManager<bsTable>.GetListNoPaging<bsTable>(Login_User_ID, "", "", "substring(name,1,2)!='bs'", "Name asc");
            List<bsTable> data = new List<bsTable>();
            bsTable obj;
            foreach (bsTable bt in objs)
            {
                obj = new bsTable();
                obj.TName = bt.TName;
                obj.TPk = bt.TPk;
                data.Add(obj);
            }
            var source = from c in data select c;
            return Json(source, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SelectField(Guid fkid)
        {
            List<bsField> objs = new List<bsField>();
            objs = EntityManager<bsField>.GetListNoPaging<bsField>(Login_User_ID, "bsT_Id='"+fkid.ToString()+"'", "", "", "FNo asc");
            var source = from c in objs select c;
            return Json(source, JsonRequestBehavior.AllowGet);
        }

        public ActionResult bsFieldLookup(string tableName, string query_Name, string query_Desp, int pageNum = 1, int numPerPage = 50)
        {
            int totalCount;
            bsTable bstobj = EntityManager<bsTable>.GetByStringFieldName<bsTable>("TName", tableName);
            List<bsField> objs = EntityManager<bsField>.GetGirdListwithPaging<bsField>("bsT_Id='" + bstobj.Id.ToString() + "'", "FleldNo asc", pageNum, numPerPage, out totalCount);
            ViewBag.numPerPage = numPerPage;
            ViewBag.pageNum = pageNum;
            ViewBag.totalCount = totalCount;
            ViewBag.Name = query_Name;

            return View(objs);
        }
        public ActionResult SelectMField(string tableName, string query_Name,string query_Desp, int pageNum = 1, int numPerPage = 50)
        {
            int totalCount;
            bsTable bstobj = EntityManager<bsTable>.GetByStringFieldName<bsTable>("TName", tableName);
            List<bsField> objs = EntityManager<bsField>.GetGirdListwithPaging<bsField>("bsT_Id='" + bstobj.Id.ToString() + "'", "FleldNo asc", pageNum, numPerPage, out totalCount);
            ViewBag.numPerPage = numPerPage;
            ViewBag.pageNum = pageNum;
            ViewBag.totalCount = totalCount;
            ViewBag.Name = query_Name;
            
            return View(objs);
        }
        public ActionResult AddDataDictionary()
        {
            return View();
        }
        

    }
}
