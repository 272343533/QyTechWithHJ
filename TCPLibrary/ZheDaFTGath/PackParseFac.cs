using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Reflection;

using QyTech.Communication;

namespace DSC_FTHj1
{
   
    public class PackParseFac
    {
        //用位数据来表示采集的数据项


        private Dictionary<string, string> CommControlWord = new Dictionary<string, string>();


        public PackParseFac() { }

        /// <summary>
        /// 创建对应的产品
        /// </summary>
        /// <param name="flag">分别控制命令的3个字节表示的字符串序列</param>
        /// <returns></returns>
        public ZdFT_Packet Create(string flag)
        {
            Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 
            object obj;
            if (flag == "000400")
            {

                obj = assembly.CreateInstance("DSC_FTHj1.Resp" + flag);//Resp000400
            }
            else if (flag.Substring(0, 2) == "0C" || flag.Substring(0, 2) == "05")
                obj = assembly.CreateInstance("DSC_FTHj1.Read" + flag);
            else
                obj = assembly.CreateInstance("DSC_FTHj1.Write" + flag);

            ZdFT_Packet retobj=(obj as ZdFT_Packet);
            return retobj;
        }
    
     
        /// <summary>
        /// 需要常规读的类及方法名
        /// </summary>
        /// <returns></returns>
        public string[] GetNeedNormalReadClassMethodName()
        {
            return new string[] { "Read0C2003.Create", "Read0C4003.Create" };
            //return new string[] { "ReadSBRB31.Create"};
        }

        /// <summary>
        /// 需要常规写的类及方法名
        /// </summary>
        /// <returns></returns>
        public string[] GetNeedNormalWriteClassMethodName()
        {
            return new string[] { "" };
        }
    }
}
