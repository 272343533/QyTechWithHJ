using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Web;
using SunMvcExpress.Dao;
using System.Web.Mvc;
using SunMvcExpress.Core.Helpers;

using SunMvcExpress.Core.Models;
using System.Data.Objects.DataClasses;
using System.Data.Objects;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;





namespace SunMvcExpress.Core.Common
{

 
    public enum ActionTypeName
    {
        新增, 删除, 编辑, 审核, Excel导入, Excel导出
    }
    public enum SysFunType
    {
        //根，导航分组，内部树分组，功能页，子功能页
        Root=0,Bar=1,Group=21,Page=3,SubPage=31
    }

    public enum EditType
    {
        //文本   单选      复选    下拉     隐藏     提示     带回    多个带回  从属带回(Exctend)   隐藏从属带回, 文件
        text = 1, radio, checkbox, select, hidden, suggest, lookup, Mlookup, Elookup, HElookup,fileup
    }
    public enum AddorModify
    { 
    Add=1,Modify
    }
    public enum FuncType
    {
        Main = 1, Lookup,Extend, Sub = 101,主功能=1001,子功能,从属功能,查找带回
    }
    public enum LayoutType
    {
        BaseTradation = 1, BasePop, Mainchild_Grid = 11, Mainchild_GridView
    }
    

    public enum FlowStatus
    {
        新增=1,已保存,已审核
    }

    public enum dwzTarget
    {
        navTab=1,dialog
    }
    public enum TypeGroup { Varchar, Decimal, Integer,Datetime, Boolean, Guid }
    public enum FieldType { Uniqueidentifier,Varchar,Datetime,Date,Int,Bigint,Bit,Decimal}

    public enum SpeicalFields { Operator, OperDt, FlowStatus = 1, Auditer, AuditDT, delStatus,bsO_Id }

    //根据配置项，生成linq 查询，返回需要的对象
    public class CommQuery
    {
        public static List<T> ExecuteQuery<T>(Guid funid,string values) where T : class,new()
        {
            using (var db=new WSExpressEntities())
            {
                    
                bsSysFunc sf=db.bsSysFunc.Where(p=>p.FunId==funid).FirstOrDefault<bsSysFunc>();
                //bsTable bt = db.bsTable.Where(t => t.Name == sf.TName).SingleOrDefault<bsTable>();
                string[] queryfields=sf.Queryfields.Split(new char[]{','});

                //得到他的类型，以便不同的类型生成不同的字符串e
                List<bsField> fs = db.bsField.Where(f => f.bsTable.TName==sf.TName).ToList<bsField>();
                SortedList<string,bsField> slfs=ChangeToSorted(fs);

                string[] queryvalues=values.Split(new char[]{','});

                string sql = "select * from " + sf.TName + " where ";//表的多个字段需要全列出来吗,需要核实linq to entity模型
                int index=0;
                foreach (string field in queryfields)
                {
                    bsField bf = slfs[field];
                    if (bf.Type.ToLower() == TypeGroup.Varchar.ToString().ToLower())
                    {
                        sql += field + "='" + queryvalues[index] + "' and ";
                    }
                    index++;
                }

                sql = sql.Substring(0, sql.Length - 4);

                List<T> ret = db.ExecuteStoreQuery<T>(sql).ToList<T>();
                return ret;
            }
        }

        public static SortedList<string, bsField> ChangeToSorted(List<bsField> fs)
        {
            SortedList<string, bsField> slfs = new SortedList<string, bsField>();
            foreach( bsField bf in fs)
            {
                slfs.Add(bf.Name, bf);
            }

            return slfs;
        }
    }


    public class DwzReturn
    {
        public static AjaxJsonResult SuccessAjaxJsonResult(string navtabid)
        {
            AjaxJsonResult rs = null;
            rs = new AjaxJsonResult
            {
                statusCode = "200",
                message = "操作成功",
                navTabId = navtabid,
                rel = "",
                callbackType = "closeCurrent",
                forwardUrl = ""
            };
            return rs;
        }
        public static AjaxJsonResult SuccessAjaxJsonResultNotClosed(string navtabid)
        {
            AjaxJsonResult rs = null;
            rs = new AjaxJsonResult
            {
                statusCode = "200",
                message = "操作成功",
                navTabId = navtabid,
                rel = "",
                callbackType = "",
                forwardUrl = ""
            };
            return rs;
        }

        public static AjaxJsonResult FailAjaxJsonResult()
        {
            AjaxJsonResult rs = null;
            rs = new AjaxJsonResult
            {
                statusCode = "300",
                message = "操作失败",
                navTabId = "",
                rel = "",
                callbackType = "",
                forwardUrl = ""
            };

            return rs;
        }

        public static AjaxJsonResult FailAjaxJsonResult(string reason)
        {
            AjaxJsonResult rs = null;
            rs = new AjaxJsonResult
            {
                statusCode = "300",
                message = reason,
                navTabId = "",
                rel = "",
                callbackType = "",
                forwardUrl = ""
            };

            return rs;
        }
       
    }


    public class sysAdminGuid
    {
        public static Guid User = new Guid("5956B1AA-2A44-4DF0-A1A4-B1C108C5DAE8");
        public static Guid Role = new Guid("76407F9A-6926-478E-8A85-25208FCED94A");

    }
   

    
        
}
