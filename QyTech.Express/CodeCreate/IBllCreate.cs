using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QyTech.Express.CodeCreate;
using SunMvcExpress.Dao;
using QyTech.Core.Common;
using System.IO;


namespace QyTech.Express.CodeCreate
{
    public class IBllCreate:ICSharpMvcCode
    {
        //对每个类的常用操作接口放在Bll中吧
        public IBllCreate()
            : base()
        {
            FILEPATHHEAD = FILEPATHHEAD + @"BLL\";
            if (!Directory.Exists(FILEPATHHEAD))
            {
                Directory.CreateDirectory(FILEPATHHEAD);
            }
        }

        private void CreateFileHead(ISysFunConf ifc,string CreateFileName)
        {
            if (ifc.fc == null)
                throw new Exception("请首先对功能赋值！");
            
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


            sw.WriteLine("namespace SunMvcExpress.BLL");
            sw.WriteLine("{");
            sw.WriteLine("\t" + "public class " + ifc.fc.FunContr + "Bll");
            sw.WriteLine("\t" + "{");
            sw.WriteLine("");
        }

        private void Create(ISysFunConf ifc, StreamWriter sw)
        {
            foreach (bsSysFuncInterface fif in ifc.FuncInterfs)
            {
                if (fif.RetType == "Json对象")
                    continue;


                string[] fparams = fif.InterfParams.Split(new char[] { ',', ' ' });

                if (fif.RetType == "唯一对象")
                    sw.WriteLine("\t" + "\t" + "public static " + fif.RetObject + " " + fif.InterfName + "(" + fif.InterfParams.Trim() + ")");
                else
                    sw.WriteLine("\t" + "\t" + "public static List<" + fif.RetObject + "> " + fif.InterfName + "(" + fif.InterfParams.Trim() + ")");

                sw.WriteLine("\t" + "\t" + "{");

                if (fparams.Length > 0)
                {
                    if (fif.RetType == "唯一对象")
                        sw.WriteLine("\t" + "\t" + "    return EntityManager<" + fif.RetObject + ">.GetListNoPaging<" + fif.RetObject + ">(\"bsO_id='\" + bsO_Id.ToString() + \"'\", \"\");");
                    else
                    {
                        //应该判断类型，此处暂时都按照guid处理了
                        sw.WriteLine("\t" + "\t" + "    return EntityManager<" + fif.RetObject + ">.GetListNoPaging<" + fif.RetObject + ">(\"" + fparams[1] + "='\" + " + fparams[1] + ".ToString() + \"'\", \"\");");
                    }
                }
                else
                {
                    sw.WriteLine("\t" + "\t" + "    return EntityManager<" + fif.RetObject + ">.GetListNoPaging<" + fif.RetObject + ">(\"\", \"\");");

                }
                sw.WriteLine("\t" + "\t" + "}");
            }
            sw.WriteLine("\t" + "}");
            sw.WriteLine("}");
        }
    
        
        public void Create(ISysFunConf ifc)
        {
            if (ifc.FuncInterfs.Count > 0)
            {
                CreateFileName = FILEPATHHEAD + ifc.fc.FunContr + @"Bll.cs";
                CreateFileHead(ifc, CreateFileName);

                Create(ifc, sw);

                base.CreateFileEnd();
            }
        }        


    }
}
