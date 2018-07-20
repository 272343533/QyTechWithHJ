using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SunMvcExpress.Dao;
//using QyTech.Core;
//using QyTech.Core.BLL;
using SunMvcExpress.Core;
using SunMvcExpress.Core.BLL;


using log4net;

namespace DSC_FTHj1
{

    public class Resp000400 : ZdFT_Packet
    {

        public Resp000400()
            : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strid">标识字段组id</param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public override int Parse(byte[] bytes)
        {

            int ret = 0;
            log.Info("004000 确认回包");
         
            try
            {
                string flag = BitConverter.ToString(bytes, 0).Replace("-", "");
                byte[] buff = new byte[5]; Buffer.BlockCopy(bytes, 0, buff, 0, 5); Array.Reverse(buff);
    
                log.Info("004000 确认回包  20:" + flag);
               
                string commflag=flag.Substring(0, 2) + flag.Substring(6, 4);

                PackParseFac fac = new PackParseFac();
                string paras = flag.Substring(10, flag.Length - 12);
                ZdFT_Packet pack = fac.Create(commflag);
                ret=pack.Parse(bytes);
                CommFlag = pack.CommFlag;
                strData = pack.strData;
                StrRawData = pack.strData;

                 log.Info("004000 确认回包  30:" + flag+"--"+CommFlag +"-----"+strData);
              
               // HrzDownSet dbobj = EntityManager<HrzDownSet>.GetBySql<HrzDownSet>("setflag='" + flag.Substring(0, 2) + flag.Substring(6, 4) + "'");

               // if (ret == "00")
               // {
               //     dbobj.ValidResponse = BitConverter.ToString(buff, 0).Replace("-", "");
               // }
               // else if (ret=="01")
               // {
               //     dbobj.ValidResponse = "找不到指定的参数，请检查ID标识，上位机是否与下位机配置是否一致";
               // }
               // else if (ret == "02")
               // {
               //     dbobj.ValidResponse = "没有相应的命令，找不到相应的DA以及DT";
               // }
               // else if (ret == "03")
               // {
               //     dbobj.ValidResponse = "找不到相应的厂家代码，请确认厂家是否选择正确";
               // }
               // else
               // {
               //     dbobj.ValidResponse = " 无反馈或者反馈数据错误，请确认MBUS是否接正确或厂家代码以及表地址是否填写选择正确";
               // }
               // //找到最早的下发命令，进行结果判断
               // log.Error("004000 确认回包  30");

               // dbobj.DownSuccDt= DateTime.Now;
               ////并直接保存到数据中
               // EntityManager<HrzDownSet>.Modify<HrzDownSet>(dbobj);

               // log.Error("004000 确认回包  40");
        
            }
            catch (Exception ex)
            {
                log.Error("000400:"+ this.GetType().ToString()+".Parse" + ex.Message);
                return ret;
            }
            return ret;

        }
    }
}
