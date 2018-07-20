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

    public class Write132008: ZdFT_Packet
    {
        /// <summary>
        /// 阀门进入调试模式
        /// </summary>
        public Write132008()
            : base()
        {
        }


        public override int Parse(byte[] bytes)
        {
            
            return Parse004000WithRoomId(bytes);

        }
    }
}
