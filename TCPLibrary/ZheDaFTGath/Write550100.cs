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

    public class Write550100 : ZdFT_Packet
    {
        /// <summary>
        /// 透传传输调试命令
        /// </summary>
        public Write550100()
            : base()
        {
        }

        public override byte[] Create(string data)
        {
            return base.CreateAsc(data);
        }

        public override int Parse(byte[] bytes)
        {
            int ret = 0;
            log.Info("550100 确认回包");

            try
            {
                string flag = BitConverter.ToString(bytes, 0).Replace("-", "");
                byte[] buff = new byte[5]; Buffer.BlockCopy(bytes, 0, buff, 0, 5); Array.Reverse(buff);

                log.Info("550100 确认回包  20:" + flag);

                string commflag = flag.Substring(0, 2) + flag.Substring(6, 4);

                PackParseFac fac = new PackParseFac();
                string paras = flag.Substring(10, flag.Length - 12);
                ZdFT_Packet pack = fac.Create(commflag);
                ret = pack.Parse(bytes);
                CommFlag = pack.CommFlag;
                strData = pack.strData;
                StrRawData = pack.strData;

                log.Info("004000 确认回包  30:" + flag + "--" + CommFlag + "-----" + strData);

           

            }
            catch (Exception ex)
            {
                log.Error("000400:" + this.GetType().ToString() + ".Parse" + ex.Message);
                return ret;
            }
            return ret;

            //int ret=base.Parse(bytes);
            //return ret;

        }
    }
}
