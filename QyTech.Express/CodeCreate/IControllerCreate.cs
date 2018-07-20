using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using SunMvcExpress.Dao;
//using SunMvcExpress.Core.Models;
using System.Data.Objects.DataClasses;
using System.Web;
//using SunMvcExpress.Core.BLL;
using SunMvcExpress.Core.Common;

using QyTech.Core;
using QyTech.Core.Common;



namespace QyTech.Express
{
    public class IControllerCreate:ICSharpMvcCode
    {
        public IControllerCreate()
            : base()
        {
            FILEPATHHEAD = FILEPATHHEAD + @"Controllers\";
        }

        public virtual void Create() { }
        public virtual void Create(bsSysFunc fc) { Ifc.fc = fc; }


        /// <summary>
        /// 到的定义结束
        /// </summary>
        public virtual void CreateFileHead()
        {
            if (Ifc.fc == null)
                throw new Exception("请首先对功能赋值！");
            string ControllerFilePath = FILEPATHHEAD + Ifc.fc.FunContr;
            //if (!Directory.Exists(ControllerFilePath))
            //{
            //    Directory.CreateDirectory(ControllerFilePath);
            //}
            BackupFile(CreateFileName);
           
            FileStream fs = new FileStream(CreateFileName, FileMode.Create, FileAccess.Write);
            sw = new StreamWriter(fs, utf8WithBom);

            //使用StreamWriter来往文件中写入内容
            sw.BaseStream.Seek(0, SeekOrigin.Begin);
            sw.WriteLine("using System;");
            sw.WriteLine("using System.Collections.Generic;");
            sw.WriteLine("using System.Linq;");
            sw.WriteLine("using System.Web;");
            sw.WriteLine("using System.Web.Mvc;");

            sw.WriteLine("using SunMvcExpress.Dao;");
            sw.WriteLine("using SunMvcExpress.Core.Models;");
            sw.WriteLine("using SunMvcExpress.Core.BLL;");
            sw.WriteLine("using SunMvcExpress.Core.Helpers;");
            sw.WriteLine("using SunMvcExpress.BLL;");
            sw.WriteLine("using SunMvcExpress.Core.Common;");
            sw.WriteLine("using SunMvcExpress.Core;");
            sw.WriteLine("using QyTech.Core.Common;");
            sw.WriteLine("using QyTech.Core.Helpers;");


            sw.WriteLine("namespace SunMvcExpress.Controllers");
            sw.WriteLine("{");
            sw.WriteLine("\t" + "//[LoginFilter]");
            sw.WriteLine("\t" + "public class " + Ifc.fc.FunContr + "Controller : WSExpressController");
            sw.WriteLine("\t" + "{");
            sw.WriteLine("");
        }
        //从类的最后的）开始
        public override void CreateFileEnd()
        {
            sw.WriteLine("\t" + "}");
            sw.WriteLine("}");


            sw.Flush();
            sw.Close();
        }


        public virtual void CreateSaveST()
        {
            sw.WriteLine("");
            //把richTextBox1中的内容写入文件
            sw.WriteLine("\t" + "\t" + "public string SaveForAddOrEdit(FormCollection fc,AddorModify addmodiflag)");
            sw.WriteLine("\t" + "\t" + "{");

            sw.WriteLine("\t" + "\t" + "    " + Ifc.fc.TName + " obj = new " + Ifc.fc.TName + "();");
            sw.WriteLine("\t" + "\t" + "    if (addmodiflag==AddorModify.Modify)");
            sw.WriteLine("\t" + "\t" + "    {");
            sw.WriteLine("\t" + "\t" + "        obj=EntityManager<" + Ifc.fc.TName + ">.GetByPk<" + Ifc.fc.TName + ">(\"" + Ifc.fc.TPK + "\",fc[\"" + Ifc.fc.TName + "_" + Ifc.fc.TPK + "\"].ToString());");
            sw.WriteLine("\t" + "\t" + "    }");
            sw.WriteLine("\t" + "\t" + "    if (fc != null)");
            sw.WriteLine("\t" + "\t" + "    {");
            foreach( bsFuncField ff in Ifc.dbFunfields)
            {
                if (ff.FName == Ifc.fc.TPK)
                    continue;

                if (ff.FName == SpeicalFields.Operator.ToString() || ff.FName == SpeicalFields.Auditor.ToString())
                {
                    sw.WriteLine("\t" + "\t" + "\t" + "    obj." + ff.FName + " = Login_User_Name;");
                }
                else if (ff.FName == SpeicalFields.OperateDt.ToString() || ff.FName == SpeicalFields.AuditDt.ToString())
                {
                    sw.WriteLine("\t" + "\t" + "\t" + "    obj." + ff.FName + " = DateTime.Now;");
                }
                else if (ff.FName == SpeicalFields.DelStatus.ToString())
                {
                    sw.WriteLine("\t" + "\t" + "\t" + "    obj." + ff.FName + " = false;");
                }
                else if ((bool)ff.FormVisible && !(bool)ff.FormEditable)//可编辑的才需要
                    continue;
                else
                {

                    if (ff.Type == "varchar")
                    {
                        sw.WriteLine("\t" + "\t" + "        if (!(fc[\"" + Ifc.fc.TName + "_" + ff.FName + "\"].ToString().Equals(\"\") || fc[\"" + Ifc.fc.TName + "_" + ff.FName + "\"].ToString() == null))");
                        sw.WriteLine("\t" + "\t" + "        {");
                        sw.WriteLine("\t" + "\t" + "            obj." + ff.FName + " = fc[\"" + Ifc.fc.TName + "_" + ff.FName + "\"].ToString();");
                        sw.WriteLine("\t" + "\t" + "        }");
                    }
                    else
                    {
                        sw.WriteLine("\t" + "\t" + "        if (!(fc[\"" + Ifc.fc.TName + "_" + ff.FName + "\"].ToString() == null))");
                        sw.WriteLine("\t" + "\t" + "        {");
                        if (ff.Type == "date" || ff.Type == "datetime")
                        {
                            sw.WriteLine("\t" + "\t" + "            obj." + ff.FName + " = Convert.ToDateTime(fc[\"" + Ifc.fc.TName + "_" + ff.FName + "\"]);");
                        }
                        else if (ff.Type == "decimal")
                        {
                            sw.WriteLine("\t" + "\t" + "            obj." + ff.FName + " = Convert.ToDecimal(fc[\"" + Ifc.fc.TName + "_" + ff.FName + "\"]);");
                        }
                        else if (ff.Type.IndexOf("int") >= 0)
                        {
                            sw.WriteLine("\t" + "\t" + "            obj." + ff.FName + " = int.Parse(fc[\"" + Ifc.fc.TName + "_" + ff.FName + "\"]);");
                        }
                        else if (ff.Type == "bit")
                        {
                            sw.WriteLine("\t" + "\t" + "            obj." + ff.FName + " = bool.Parse(fc[\"" + Ifc.fc.TName + "_" + ff.FName + "\"]);");
                        }
                        else if (ff.Type == "uniqueidentifier")
                        {
                            sw.WriteLine("\t" + "\t" + "            obj." + ff.FName + " = new Guid(fc[\"" + Ifc.fc.TName + "_" + ff.FName + "\"].ToString());");
                        }
                        sw.WriteLine("\t" + "\t" + "        }");
                    }
                }
            }
            sw.WriteLine("\t" + "\t" + "    }");

            //如果有分类属性，则在此赋值
            if (Ifc.fc.Filterfield != null && Ifc.fc.Filterfield.Trim().Length > 0)
            {
                sw.WriteLine("\t" + "\t" + "\t" + "    obj." + Ifc.fc.Filterfield + " = \"" + Ifc.fc.Filtervalue + "\";");
            }

            sw.WriteLine("\t" + "\t" + "    string errMsg=\"\";");
            sw.WriteLine("\t" + "\t" + "    if (addmodiflag==AddorModify.Modify)");
            sw.WriteLine("\t" + "\t" + "    {");
            sw.WriteLine("\t" + "\t" + "        errMsg = EntityManager<bsSysFunc>.Modify(obj);");
            sw.WriteLine("\t" + "\t" + "    }");
            sw.WriteLine("\t" + "\t" + "    else");
            sw.WriteLine("\t" + "\t" + "    {");
            sw.WriteLine("\t" + "\t" + "        obj." + Ifc.fc.TPK + " = Guid.NewGuid();");
            sw.WriteLine("\t" + "\t" + "        errMsg = EntityManager<" + Ifc.fc.TName + ">.Add(obj);");
                
            sw.WriteLine("\t" + "\t" + "    }");
            sw.WriteLine("\t" + "\t" + "    return errMsg;");
            sw.WriteLine("\t" + "\t" + "}");
        
        }
 
        //fkcondtion,string filter
        public virtual void SetQueryConditionsWithFk()
        {
            sw.WriteLine("\t" + "\t" + "\t" + "if (fkcondtion != null && fkcondtion.Equals(\"\"))");
            sw.WriteLine("\t" + "\t" + "\t" + "{");
            sw.WriteLine("\t" + "\t" + "\t" + "    conditions += fkcondtion;");
            sw.WriteLine("\t" + "\t" + "\t" + "}");
                        //是否一个表存储多类信息
            //功能的过滤字段
            sw.WriteLine("\t" + "\t" + "\t" + "bsSysFuncId = funid;");
            sw.WriteLine("\t" + "\t" + "\t" + "if (bsSysfunc.Filterfield != null && (bsSysfunc.Filterfield.ToString().Length > 0))");
            sw.WriteLine("\t" + "\t" + "\t" + "{");
            sw.WriteLine("\t" + "\t" + "\t" + "    conditions += \"" + Ifc.fc.Filterfield + "=\'" + Ifc.fc.Filtervalue + "\' and \";");
            sw.WriteLine("\t" + "\t" + "\t" + "}");
            //得到他的类型，以便不同的类型生成不同的字符串e
            sw.WriteLine("\t" + "\t" + "\t" + "string conditions = \" \";");
            sw.WriteLine("\t" + "\t" + "\t" + "List<string> usingfield = new List<string>();");
            foreach (bsSysFuncQuery bf in Ifc.FuncQuerys)
            {
                if (bf == null)
                    continue;
                ////if (bf.Type.ToLower() == TypeGroup.Varchar.ToString().ToLower())
                ////{
                ////    sw.WriteLine("\t" + "\t" + "\t" + "if (" + bf.Name + " != null && !" + bf.Name + ".Equals(\"\"))");
                ////    sw.WriteLine("\t" + "\t" + "\t" + "{");
                ////    sw.WriteLine("\t" + "\t" + "\t" + "\t" + "conditions += \"" + bf.Name + " like '%\"+" + bf.Name + "+\"%\' and \";");
                ////    sw.WriteLine("\t" + "\t" + "\t" + " }");
                ////}
                ////else if (bf.Type.ToLower() == TypeGroup.Datetime.ToString().ToLower())
                ////{
                ////    if (!RangeFields.Contains(bf.Name))
                ////    {
                ////        sw.WriteLine("\t" + "\t" + "\t" + "if (" + bf.Name + " != null && !" + bf.Name + ".Equals(\"\"))");
                ////        sw.WriteLine("\t" + "\t" + "\t" + "{");
                ////        sw.WriteLine("\t" + "\t" + "\t" + "\t" + "conditions += \"" + bf.Name + " >=\'\"+" + bf.Name + "+\"\' and \";");
                ////        sw.WriteLine("\t" + "\t" + "\t" + " }");
                ////    }
                ////    else
                ////    {
                ////        sw.WriteLine("\t" + "\t" + "\t" + "if (" + bf.Name + "_start != null && !" + bf.Name + "_start.Equals(\"\"))");
                ////        sw.WriteLine("\t" + "\t" + "\t" + "{");
                ////        sw.WriteLine("\t" + "\t" + "\t" + "\t" + "conditions += \"" + bf.Name + "_start >=\'\"+" + bf.Name + "_start+\"\' and \";");
                ////        sw.WriteLine("\t" + "\t" + "\t" + " }");
                ////        sw.WriteLine("\t" + "\t" + "\t" + "if (" + bf.Name + "_end != null && !" + bf.Name + "_end.Equals(\"\"))");
                ////        sw.WriteLine("\t" + "\t" + "\t" + "{");
                ////        sw.WriteLine("\t" + "\t" + "\t" + "\t" + "conditions += \"" + bf.Name + "_end   <=\'\"+" + bf.Name + "_end +\"\' and \";");
                ////        sw.WriteLine("\t" + "\t" + "\t" + " }");
                ////    }

                ////}
                ////else if ("conditions" + bf.Type.ToLower() == TypeGroup.Decimal.ToString().ToLower())
                ////{
                ////    sw.WriteLine("\t" + "\t" + "\t" + "if (" + bf.Name + " != null && !" + bf.Name + ".Equals(\"\"))");
                ////    sw.WriteLine("\t" + "\t" + "\t" + "{");
                ////    sw.WriteLine("\t" + "\t" + "\t" + "\t" + "conditions += \"" + bf.Name + "=" + bf.Name + " and \";");
                ////    sw.WriteLine("\t" + "\t" + "\t" + " }");
                ////}

            }

            //需要判断是否需要增加orgid字段过滤数据，根据funid
            sw.WriteLine("\t" + "\t" + "\t" + "if (HaveOrgid)");
            sw.WriteLine("\t" + "\t" + "\t" + "{");
            sw.WriteLine("\t" + "\t" + "\t" + "    String idset = Toolkit<bsOrganize>.ToSymbolString(bsUserManager.GetEmpDataRighs(userID), \"id\", \",\");");
            sw.WriteLine("\t" + "\t" + "\t" + "    idset = idset.Equals(\"\") ? \"'\" + Guid.Empty.ToString() + \"'\" : idset + \",'\" + Guid.Empty.ToString() + \"'\";");

            sw.WriteLine("\t" + "\t" + "\t" + "    conditions += \"OrgId in (\" + idset + \")\";");
            sw.WriteLine("\t" + "\t" + "\t" + "}");
            sw.WriteLine("\t" + "\t" + "\t" + "else");
            sw.WriteLine("\t" + "\t" + "\t" + "{");
            sw.WriteLine("\t" + "\t" + "\t" + "    if (conditions.Length > 4)");
            sw.WriteLine("\t" + "\t" + "\t" + "        conditions = conditions.Substring(0, conditions.Length - 4);");
            sw.WriteLine("\t" + "\t" + "\t" + "}");
        }

        public void SetQueryConditions()
        {
            //功能的过滤字段
            if (Ifc.fc.Filterfield != null && (Ifc.fc.Filterfield.ToString().Length > 0))
            {
                sw.WriteLine("\t" + "\t" + "\t" + "    conditions += \"" + Ifc.fc.Filterfield + "=\'" + Ifc.fc.Filtervalue + "\' and \";");
            }
            foreach (bsSysFuncQuery bf in Ifc.FuncQuerys)
            {
                string queryVar = "query_" + bf.FFName;
                if (bf == null)
                    continue;
  
                if (bf.FFType.ToLower() == TypeGroup.Varchar.ToString().ToLower())
                {
                    sw.WriteLine("\t" + "\t" + "\t" + "if (" + queryVar + " != null && !" + queryVar + ".Equals(\"\"))");
                    sw.WriteLine("\t" + "\t" + "\t" + "{");
                    sw.WriteLine("\t" + "\t" + "\t" + "\t" + "conditions += \"" + bf.FFName + " like '%\"+" + queryVar + "+\"%\' and \";");
                    sw.WriteLine("\t" + "\t" + "\t" + " }");
                }
                else if (bf.FFType.ToLower() == TypeGroup.Datetime.ToString().ToLower())
                {
                    if (bf.NeedRange==null || !(bool)bf.NeedRange)
                    {
                        sw.WriteLine("\t" + "\t" + "\t" + "if (" + queryVar + " != null && !" + queryVar + ".Equals(\"\"))");
                        sw.WriteLine("\t" + "\t" + "\t" + "{");
                        sw.WriteLine("\t" + "\t" + "\t" + "\t" + "conditions += \"" + bf.FFName + " ==\'\"+" + queryVar + "+\"\' and \";");
                        sw.WriteLine("\t" + "\t" + "\t" + " }");
                    }
                    else
                    {
                        sw.WriteLine("\t" + "\t" + "\t" + "if (" + queryVar + "_start != null && !" + queryVar + "_start.Equals(\"\"))");
                        sw.WriteLine("\t" + "\t" + "\t" + "{");
                        sw.WriteLine("\t" + "\t" + "\t" + "\t" + "conditions += \"" + bf.FFName + "_start >=\'\"+" + queryVar + "_start+\"\' and \";");
                        sw.WriteLine("\t" + "\t" + "\t" + " }");
                        sw.WriteLine("\t" + "\t" + "\t" + "if (" + queryVar + "_end != null && !" + queryVar + "_end.Equals(\"\"))");
                        sw.WriteLine("\t" + "\t" + "\t" + "{");
                        sw.WriteLine("\t" + "\t" + "\t" + "\t" + "conditions += \"" + bf.FFName + "_end   <=\'\"+" + queryVar + "_end +\"\' and \";");
                        sw.WriteLine("\t" + "\t" + "\t" + " }");
                    }
                }
                else if (bf.FFType.ToLower() == TypeGroup.Decimal.ToString().ToLower() || bf.FFType.ToLower() == TypeGroup.Integer.ToString().ToLower())
                {
                    sw.WriteLine("\t" + "\t" + "\t" + "if (" + queryVar + " != null && !" + queryVar + ".Equals(\"\"))");
                    sw.WriteLine("\t" + "\t" + "\t" + "{");
                    sw.WriteLine("\t" + "\t" + "\t" + "\t" + "conditions += \"" + bf.FFName + "=" + queryVar + " and \";");
                    sw.WriteLine("\t" + "\t" + "\t" + " }");
                }
                else if (bf.FFType.ToLower() == TypeGroup.Guid.ToString().ToLower() || bf.FFType.ToLower() == "uniqueidentifier")
                {
                    sw.WriteLine("\t" + "\t" + "\t" + "if (" + queryVar + " != null && !" + queryVar + ".Equals(\"\"))");
                    sw.WriteLine("\t" + "\t" + "\t" + "{");
                    sw.WriteLine("\t" + "\t" + "\t" + "\t" + "conditions += \"" + bf.FFName + " ='\" + " + queryVar + "+\"' and \";");
                    sw.WriteLine("\t" + "\t" + "\t" + " }");
                }
            }

            //需要判断是否需要增加orgid字段过滤数据，根据funid
            if (HaveOrgid)
            {
                sw.WriteLine("\t" + "\t" + "\t" + "    String idset = Toolkit<bsOrganize>.ToSymbolString(bsUserManager.GetEmpDataRighs(userID), \"id\", \",\");");
                sw.WriteLine("\t" + "\t" + "\t" + "    idset = idset.Equals(\"\") ? \"'\" + Guid.Empty.ToString() + \"'\" : idset + \",'\" + Guid.Empty.ToString() + \"'\";");
                sw.WriteLine("\t" + "\t" + "\t" + "    conditions += \"bsO_Id in (\" + idset + \")\";");
            }
            sw.WriteLine("\t" + "\t" +  "    if (conditions.Length > 4)");
            sw.WriteLine("\t" + "\t" +  "        conditions = conditions.Substring(0, conditions.Length - 4);");
        }
    }
}

   
