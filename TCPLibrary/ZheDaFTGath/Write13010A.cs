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

    public class Write13010A: ZdFT_Packet
    {
        /// <summary>
        /// 广播阀门进入远程控制模式
        /// </summary>
        public Write13010A()
            : base()
        {
        }


        public override int Parse(byte[] bytes)
        {
            
            return 2;

        }
    }
}
