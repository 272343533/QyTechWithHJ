using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using log4net;
using System.Reflection;
using SunMvcExpress.Dao;
using QyTech.Core.Common;
using SunMvcExpress.Core;
using SunMvcExpress.Core.BLL;

namespace QyTech.Express
{
    public class ICSharpMvcCode
    {
        public StreamWriter sw;
 
        private ISysFunConf ifc_;
        public ISysFunConf Ifc_Child;


        protected string DatajsbxName;//= "BaseInfoIndexjsbx";

        private bool HaveOrgid_;//表中是否包含orgid
        private string ControllerName_;
        //private List<string> RangeFields_;

        private bool BackupSourceFlag_ = true;//需要从xml配置才好
        private string DAONAMESPACE_ = "SunMvcExpress.Dao";


        protected string FILEPATHHEAD = "";
        protected SortedList<ActionType, string> fullUrl = new SortedList<ActionType, string>();
        protected Encoding utf8WithBom = new System.Text.UTF8Encoding(true);
        public static ILog log = log4net.LogManager.GetLogger("IPage");
        public string CreateFileName { get; set; }

        public string QueryButtonName;


        public ICSharpMvcCode()
        {
            FILEPATHHEAD = SunMvcApp.AppMapPath;
            BackupSourceFlag_ = Convert.ToBoolean(SunMvcExpress.Common.Xml.XmlConfig.GetValue("BackupSourceFlag"));
           
        }

        protected string DAONAMESPACE
        {
            get { return DAONAMESPACE_; }
        }
        protected bool BackupSourceFlag
        {
            get { return BackupSourceFlag_; }
        }

        //protected ICSharpMvcCode ChildFunc
        //{
        //    get { return ChildFunc_; }
        //}

        private void SetAllFullActionUrl()
        {
            if (ControllerName_ != null)
            {
                if (ControllerName_.Length > 0)
                {
                    fullUrl.Clear();
                    for (int i = 0; i < Enum.GetValues(typeof(ActionType)).Length; i++)
                    {
                        ActionType act = (ActionType)i;
                        fullUrl.Add(act,  "\"" + act.ToString() + "\",\"" + ControllerName_ + "\"");
                    }
                }
            }
        }



        protected string SetTabSpace(int count)
        {
            string ret = "";
            for (int i = 0; i < count; i++)
            {
                ret += "\t";
            }
            return ret;
        }

     
        public bool HaveOrgid
        {

            get
            {
                HaveOrgid_ = false;
                foreach (bsFuncField bf in Ifc.dbFunfields)
                {
                    if (bf.FName.ToLower() == "bsO_Id")
                    {
                        HaveOrgid_ = true;
                        break;
                    }
                }
                return HaveOrgid_;
            }
        }

        public ISysFunConf Ifc
        {
            get
            {
                return ifc_;
            }
            set
            {
                ifc_ = value;


                if (ifc_ != null)
                {
                    ControllerName_ = ifc_.fc.FunContr;
                    CreateFileName = FILEPATHHEAD + ifc_.fc.FunContr + @"Controller.cs";
                    SetAllFullActionUrl();
                    QueryButtonName = ifc_.fc.FunContr + "Indexsub";
                }
            }
        }

  
           
        /// <summary>
        /// 得到index view的分页参数的界面
        /// </summary>
        /// <returns></returns>
        public string GetIndexParameters()
        {
            string strPara = "";
            string DataType = "";

            if (ifc_.fc.Filterfield != null && !ifc_.fc.Filterfield.Trim().Equals(""))
                strPara += "string filter,";

            List<string> usingfield = new List<string>();

            foreach (bsSysFuncQuery bf in Ifc.FuncQuerys)
            {
                switch (bf.FFType)
                {
                    case "varchar":
                        DataType = "string";
                        break;
                    case "datetime":
                    case "date":
                        DataType = "DateTime?";
                        break;
                    default:
                        DataType = "string";
                        break;
                }
                if (bf.NeedRange == null)
                {
                    strPara += DataType + " query_" + bf.FFName + ",";
                }
                else
                {
                    strPara += DataType + "? query_" + bf.FFName + "_start,";
                    strPara += DataType + "? query_" + bf.FFName + "_end,";

                }
            }
            strPara += "string orderbys,int pageNum=1, int numPerPage=20,string conditions=\"\"";

            return strPara;
        }
        public string GetIndexParametersNoType()
        {
            string strPara = "";

            if (ifc_.fc.Filterfield != null && !ifc_.fc.Filterfield.Trim().Equals(""))
                strPara += "filter,";

            List<string> usingfield = new List<string>();

            foreach (bsSysFuncQuery bf in Ifc.FuncQuerys)
            {
                if (bf.NeedRange == null)
                {
                    strPara += " query_" + bf.FFName + ",";
                }
                else
                {
                    strPara += " query_" + bf.FFName + "_start,";
                    strPara += " query_" + bf.FFName + "_end,";

                }
            }
            strPara += "orderbys,pageNum, numPerPage,conditions";

            return strPara;
        }

        /// <summary>
        /// 得到获取参数值的字符串
        /// </summary>
        /// <returns></returns>
        public string GetIndexParameterValues()
        {
            string strPara = "new Guid(funid),Login_User_ID,";

            strPara = "fkcondtion,filter,conditions,orderbys,";

            using (var db = new WSExpressEntities())
            {
                foreach (bsSysFuncQuery bf in ifc_.FuncQuerys)
                {
                    if (bf == null)
                        continue;
                    ////if (bf.Type.ToLower() == FieldType.Datetime.ToString().ToLower() || bf.Type.ToLower() == FieldType.Date.ToString().ToLower())
                    ////{
                    ////    if (!RangeFields.Contains(bf.Name))
                    ////        strPara += "query_" + bf.Name + ",";
                    ////    else
                    ////    {
                    ////        strPara += "query_" + bf.Name + "_start,";
                    ////        strPara += "query_" + bf.Name + "_end,";
                    ////    }
                    ////}
                    ////else
                    ////    strPara += "query_" + bf.Name + ",";
                }
                strPara += "pageNum, numPerPage, out totalCount";

                return strPara;
            }
        }

        //得到单词的复数
        public string GetWordss(string word)
        {
            return word;

            string ret;
            string lastone, lasttwo;
            lastone = word.Substring(word.Length - 1, 1);
            lasttwo = word.Substring(word.Length - 2, 2);

            if ((lastone == "o") || (lastone == "s") || (lastone == "x"))
            {
                ret = word + "es";
            }
            else if (lastone == "S")
            {
                ret = word;
            }
            else if (lastone == "f")
            {
                ret = word.Substring(0, word.Length - 1) + "ves";
            }
            else if (lastone == "y")
            {
                ret = word.Substring(0, word.Length - 1) + "ies";
            }
            else if ((lasttwo == "ch") || (lasttwo == "sh"))
            {
                ret = word + "es";
            }
            else if (lasttwo == "fe")
            {
                ret = word.Substring(0, word.Length - 2) + "ves";
            }
            else
                ret = word + "s";

            return ret;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        public bool BackupFile(string source)
        {
            try
            {
                if (File.Exists(source))
                {
                    if (BackupSourceFlag)
                        File.Copy(source, source + DateTime.Now.ToString("yyyyMMddHHmmss"));
                    File.Delete(source);
                }
                return true;
            }
            catch { return false; }
        }

        public bool HaveSpecialFields(SpeicalFields fc)
        {
            bool haveflag = false;
            foreach (bsFuncField bf in Ifc.dbFunfields)
            {
                if (bf.FName == fc.ToString())
                {
                    haveflag = true;
                    break;
                }
            }
            return haveflag;
        }

        /// <summary>
        /// 查看查询条件中是否有范围的查询字段，看是否有连续的重名字段，有则认为是范围
        /// </summary>
        /// <returns></returns>
        public List<string> GetDataRangeQueryFields()
        {
            List<string> rangefields = new List<string>();
            List<string> searchfields = new List<string>();
            //foreach (string f in Queryfields_)
            //{
            //    if (searchfields.Contains(f))
            //    {
            //        rangefields.Add(f);
            //    }
            //    else
            //    {
            //        searchfields.Add(f);
            //    }
            //}
            return rangefields;
        }



        protected void CreateFileHead(ISysFunConf Ifc,string createfilename)
        {
            CreateFileName = createfilename;// FILEPATHHEAD + fc.FunContr + @"\" + ActionType.Index.ToString() + ".cshtml";
            BackupFile(CreateFileName);

            string includefilename = FILEPATHHEAD + Ifc.fc.FunContr + @"\" + Ifc.fc.FunContr + ".vqu";

            FileStream fs = new FileStream(CreateFileName, FileMode.Create, FileAccess.Write);
            sw = new StreamWriter(fs, utf8WithBom);
            //使用StreamWriter来往文件中写入内容
            sw.BaseStream.Seek(0, SeekOrigin.Begin);

        }

        protected virtual void CreatePagerForm(ISysFunConf Ifc,StreamWriter sw)
        {
            this.Ifc = Ifc;
            DatajsbxName = Ifc.fc.FunContr + ActionType.Index.ToString() + "jsbx";
           
            sw.WriteLine("@model List<" + DAONAMESPACE + "." + Ifc.fc.TName + ">");
            sw.WriteLine("<form id=\"pagerForm\" method=\"post\" action=\"@Url.Action(" + fullUrl[ActionType.Index] + ")\"  onsubmit=\"return divSearch(this, '" + DatajsbxName + "');\">");

            sw.WriteLine("\t" + "<input type=\"hidden\" name=\"pageNum\" value=\"1\" />");
            sw.WriteLine("\t" + "<input type=\"hidden\" name=\"numPerPage\" value=\"@ViewBag.numPerPage\" />");
            sw.WriteLine("\t" + "<input type=\"hidden\" name=\"orderbys\" value=\"@ViewBag.orderbys\" />");
            if (Ifc.fc.Filterfield != "")
                sw.WriteLine("\t" + "<input type=\"hidden\" name=\"filter\" id=\"filter\" value=\"@ViewBag.filter\" />");
           
            foreach (bsSysFuncQuery fq in Ifc.FuncQuerys)
            {
                if (Ifc.fc.FunLayout == PageDataStyle.STCommEdit.ToString())        // onsubmit="return divSearch(this, 'DialyReportMainjsbx');" 
                    sw.WriteLine("\t" + "<input type=\"hidden\" name=\"query_" + fq.FFName + "\" id=\"query_" + Ifc.fc.TName + "_" + fq.FFName + "\" value=\"@ViewBag." + fq.FFName + "\" />");
            }

       
            sw.WriteLine("</form>");

        }

        protected virtual void CreatePagerHeaderHead(bsSysFunc fc, StreamWriter sw)
        {
            DatajsbxName = fc.FunContr + ActionType.Index.ToString() + "jsbx";
           
            sw.WriteLine("\t" + "<div class=\"pageHeader\">");
            sw.WriteLine("\t" + "    <form onsubmit=\"return divSearch(this, '" + DatajsbxName + "');\" action=\"@Url.Action(\"IndexData\", \"" + fc.FunContr + "\")\" method=\"post\">");
            sw.WriteLine("\t" + "        <div class=\"searchBar\">");
            sw.WriteLine("\t" + "            <input type=\"hidden\" name=\"pageNum\" value=\"1\" />");
            sw.WriteLine("\t" + "            <input type=\"hidden\" name=\"numPerPage\" value=\"@ViewBag.numPerPage\" />");
            sw.WriteLine("\t" + "            <input type=\"hidden\" name=\"orderbys\" value=\"@ViewBag.orderbys\" />");
            if (Ifc.fc.Filterfield!=null && Ifc.fc.Filterfield.Trim() != "")
                sw.WriteLine("\t" + "            <input type=\"hidden\" name=\"filter\" id=\"filter\" value=\"@ViewBag.filter\" />");

            foreach (bsSysFuncQuery fq in Ifc.FuncQuerys)
            {
                if (fq.FFPositoin !="Top")
                    sw.WriteLine("\t" + "            <input type=\"hidden\" name=\"query_" + fq.FFName + "\" id=\"query_" + Ifc.fc.TName + "_" + fq.FFName + "\" value=\"@ViewBag." + fq.FFName + "\" />");
            }
            sw.WriteLine("\t" + "            <table class=\"searchContent\">");
            sw.WriteLine("\t" + "                <tr>");


        }
        protected void CreatePagerHeaderEnd(bsSysFunc fc, StreamWriter sw)
        {
            QueryButtonName = fc.FunContr + "Indexsub";
            sw.WriteLine("\t" + "                </tr>");
            sw.WriteLine("\t" + "            </table>");
            sw.WriteLine("\t" + "            <div class=\"subBar\">");
            sw.WriteLine("\t" + "                <ul>");
            sw.WriteLine("\t" + "                    <li>");
            sw.WriteLine("\t" + "                        <div class=\"buttonActive\">");
            sw.WriteLine("\t" + "                            <div class=\"buttonContent\">");
            sw.WriteLine("\t" + "                                <button id=\"" + QueryButtonName + "\" type=\"submit\">检索</button></div>");
            sw.WriteLine("\t" + "                        </div>");
            sw.WriteLine("\t" + "                    </li>");
            sw.WriteLine("\t" + "                </ul>");
            sw.WriteLine("\t" + "            </div>");
            sw.WriteLine("\t" + "    </form>");
            sw.WriteLine("\t" + "</div>");
        }


        protected void CreatePageContentDivHead(bsSysFunc fc, StreamWriter sw)
        {
            sw.WriteLine("\t" + "\t" + "<div  id=\"" + fc.FunContr + "Indexjsbx\"  class=\"pageContent\">");
        }
        protected void CreatePageContentDivEnd(StreamWriter sw)
        {
            sw.WriteLine("\t" + "\t" + "</div>");
        }
        protected virtual void CreateJs(StreamWriter sw)
        {
            sw.WriteLine("");
            sw.WriteLine("<script type=\"text/javascript\">");
            sw.WriteLine("\t" + "$('[name=numPerPage]').last().val('@ViewBag.numPerPage');");
            sw.WriteLine("</script>");
        }
        protected virtual void CreateIndexJsForDefautClick(StreamWriter sw)
        {
            sw.WriteLine("");
            sw.WriteLine("<script type=\"text/javascript\">");
            sw.WriteLine("\t" + "function DefaultLoad() {");
            sw.WriteLine("\t" + "    $(\"#" + QueryButtonName + "\").trigger(\"click\");");
            sw.WriteLine("\t" + "}");
            sw.WriteLine("\t" + "setTimeout('DefaultLoad()', 500);");
            sw.WriteLine("</script>");
        }

        public virtual void CreateFileEnd()
        {
            sw.Flush();
            sw.Close();
        }
    }
}
