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

    public class Write132001: ZdFT_Packet
    {
        /// <summary>
        /// 广播开启/关闭面板热量显示
        /// </summary>
        public Write132001()
            : base()
        {
        }


        public override int Parse(byte[] bytes)
        {
           
            return 2;

        }
    }
}
