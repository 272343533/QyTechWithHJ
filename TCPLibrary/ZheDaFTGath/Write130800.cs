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

    public class Write130800 : ZdFT_Packet
    {
        /// <summary>
        /// 强开单个阀门
        /// </summary>
        public Write130800()
            : base()
        {
        }


        public override int Parse(byte[] bytes)
        {
            return Parse004000WithRoomId(bytes);

        }
    }
}
