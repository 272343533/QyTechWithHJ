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

    public class Write040202 : ZdFT_Packet
    {
        /// <summary>
        /// 配置文件分割下发
        /// </summary>
        public Write040202()
            : base()
        {
        }


        public override int Parse(byte[] bytes)
        {
            return 0;

        }
    }
}
