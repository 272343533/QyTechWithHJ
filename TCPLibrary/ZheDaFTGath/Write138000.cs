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

    public class Write138000 : ZdFT_Packet
    {
        /// <summary>
        /// 广播远程设置温度
        /// </summary>
        public Write138000()
            : base()
        {
        }


        public override int Parse(byte[] bytes)
        {
            
            return 2;

        }
    }
}
