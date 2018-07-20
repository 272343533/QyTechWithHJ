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

    public class Write134001 : ZdFT_Packet
    {
        /// <summary>
        /// 广播强开阀门
        /// </summary>
        public Write134001()
            : base()
        {
        }
        //create 无参数，不需处理，使用默认即可

        public override int Parse(byte[] bytes)
        {
           
            return 2;

        }
    }
}
