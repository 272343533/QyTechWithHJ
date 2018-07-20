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

    public class Write011000 : ZdFT_Packet
    {

        public Write011000()
            : base()
        {
        }

        /// <summary>
        /// 重启采集器
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public override int Parse(byte[] bytes)
        {
            return 3;

        }
    }
}
