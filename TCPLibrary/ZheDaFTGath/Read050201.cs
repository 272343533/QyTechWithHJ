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

    public class Read050201: ZdFT_Packet
    {
        /// <summary>
        /// 读终端配置
        /// </summary>
        public Read050201()
            : base()
        {
        }


        public override int Parse(byte[] bytes)
        {
          
            return 10;

        }
    }
}
