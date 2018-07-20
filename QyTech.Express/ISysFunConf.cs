using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QyTech.Core.Common;
using QyTech.Core.Helpers;

using SunMvcExpress.Dao;
using log4net;
using System.IO;
using SunMvcExpress.Core;


[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace QyTech.Express
{
    public abstract class ISysFunConf
    {
        public static ILog log = log4net.LogManager.GetLogger("bsUserManager");

        private bsSysFunc fc_;
        public ISysFunConf Ifc_Child;//父子表关系
        public ISysFunConf Ifc_Parnet;//从属功能与主功能的关系
        private List<bsSysFuncOper> FuncOpers_;
        private List<bsFuncField> dbFunfields_;//功能字段设置
        private List<bsSysFuncInterface> FuncInterfs_;//逻辑接口
        private List<bsSysFuncQuery> FuncQuerys_;
        private List<bsSysFuncQuery> FuncQueryTop_ ;
        private List<bsSysFuncQuery> FuncQueryLeft_ ;
        private List<bsSysFuncQuery> FuncQueryRight_;



        protected IControllerCreate CreateContrObj;
        protected IDataViewCreate CreateViewObj;

        public abstract void CreateView();
        public abstract void CreateView(StreamWriter sw);
        public abstract void CreateDataInIndex(ISysFunConf ifc, StreamWriter sw);
        public abstract void CreateController();

        public ISysFunConf()
        {
        }

        public ISysFunConf(bsSysFunc fc)
        {
            this.fc = fc;
        }

        public bsSysFunc fc
        {
            get { return fc_; }
            set
            {
                this.fc_ = value;
                using (var db = new WSExpressEntities())
                {
                    dbFunfields_ = db.bsFuncField.Where(p => p.FcId == fc_.FcId).OrderBy(p => p.FleldNo).ToList<bsFuncField>();
                    FuncQuerys_ = db.bsSysFuncQuery.Where(p => p.FcId == fc_.FcId).OrderBy(c=>c.FFQueryNo).ToList<bsSysFuncQuery>();

                    FuncQueryTop_ = new List<bsSysFuncQuery>();
                    FuncQueryLeft_ = new List<bsSysFuncQuery>();
                    FuncQueryRight_ = new List<bsSysFuncQuery>();
                    foreach (bsSysFuncQuery fq in FuncQuerys_)
                    {
                        if (fq.FFPositoin.Substring(0, 3).ToLower() == "top")
                            FuncQueryTop_.Add(fq);
                        else if (fq.FFPositoin.Substring(0, 4).ToLower() == "left")
                            FuncQueryLeft_.Add(fq);
                        else if (fq.FFPositoin.Substring(0, 5).ToLower() == "right")
                            FuncQueryRight_.Add(fq);
                    }

                    FuncOpers_ = db.bsSysFuncOper.Where(p => p.FcId == fc_.FcId).ToList<bsSysFuncOper>();
                    FuncInterfs_ = db.bsSysFuncInterface.Where(p => p.FcId == fc_.FcId).ToList<bsSysFuncInterface>();
                    //EntityManager<bsSysFuncOper>.GetListNoPaging<bsSysFuncOper>("FcId='" + fc.FcId.ToString() + "'", "OperNo asc");

                    // RangeFields_ = GetDataRangeQueryFields();
                    //获取孩子func
                    //////if (fc.FunType == null || fc.FunType.ToLower() == FuncType.主功能.ToString().ToLower())
                    //////{
                    //////    if (fc.FunLayout.ToLower() == PageDataStyle.PCCommEdit.ToString().ToLower())
                    //////    {
                    //////        //获取一个sub对象
                    //////        bsSysFunc bsSysFunDetail_ = db.bsSysFunc.Where(p => p.PFuncCode == fc.FuncCode).FirstOrDefault<bsSysFunc>();
                    //////        if (bsSysFunDetail_ != null)
                    //////        {
                    //////            ifc_Child = new ();
                    //////            ifc_Child.bsSysFuncId = bsSysFunDetail_.FcId;
                    //////        }
                    //////    }
                    //////}
                }
            }
        }

        public List<bsSysFuncQuery> FuncQuerys
        {
            get
            {
                return FuncQuerys_;
            }
        }
        public List<bsSysFuncOper> FuncOpers
        {
            get { return FuncOpers_; }
        }

        public List<bsFuncField> dbFunfields
        {
            get { return dbFunfields_; }
        }
        public List<bsSysFuncInterface> FuncInterfs
        {
            get { return FuncInterfs_; }
        }


        protected void CreateJsonResult(StreamWriter sw)
        {
              foreach (bsSysFuncInterface fif in FuncInterfs)
                {
                    if (fif.RetType == "Json对象")
                    {
                        //在此创建Json对象
                        sw.WriteLine("      public JsonResult " + fif.InterfName + "(" + fif.InterfParams.Trim() + ")");
                        sw.WriteLine("     {");
                        sw.WriteLine("         using (var db = new WSExpressEntities())");
                        sw.WriteLine("         {");

                        if (fif.InterfParams.Length > 0)
                        {
                            string[] splits = fif.InterfParams.Split(new char[] { ' ' });
                            sw.WriteLine("             var objs = db." + fif.RetObject + ".Where(c => c." + splits[1] + " == " + splits[1] + ");");
                        
                        }
                        else
                        {
                            sw.WriteLine("             var objs = db." + fif.RetObject + ";");
                        }
                        sw.WriteLine("             List<" + fif.RetObject + "> list = new List<" + fif.RetObject + ">();");
                        sw.WriteLine("             foreach (var it in objs)");
                        sw.WriteLine("             {");

                        sw.WriteLine("                  " + fif.RetObject + " item = new " + fif.RetObject + "();");
                        //对每个字段进行处理
                        bsTable tobj = SunMvcExpress.Core.BLL.EntityManager<bsTable>.GetByStringFieldName<bsTable>("TName", fif.RetObject);
                        List<bsField> fs = SunMvcExpress.Core.BLL.EntityManager<bsField>.GetListNoPaging<bsField>("bsT_Id='" + tobj.Id + "'", "");
                        foreach (bsField ff in fs)
                        {
                            sw.WriteLine("                 item." + ff.Name + "=it." + ff.Name + ";");
                        }
                        sw.WriteLine("                 list.Add(item);");
                        sw.WriteLine("             }");
                        sw.WriteLine("             var source = from c in list select c;");
                        sw.WriteLine("             return Json(source, JsonRequestBehavior.AllowGet);");
                        sw.WriteLine("         }");
                        sw.WriteLine("     }");
                    }
                }
        }

        public List<bsSysFuncQuery> FuncQueryTop { get { return FuncQueryTop_; } }
        public List<bsSysFuncQuery> FuncQueryLeft { get { return FuncQueryLeft_; } }
        public List<bsSysFuncQuery> FuncQueryRight { get { return FuncQueryRight_; } }

    }
}
