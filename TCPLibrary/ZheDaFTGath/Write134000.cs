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

    public class Write134000 : ZdFT_Packet
    {
        /// <summary>
        /// 单个阀门自由模式
        /// </summary>
        public Write134000()
            : base()
        {
        }


        public override int Parse(byte[] bytes)
        {
            
            return Parse004000WithRoomId(bytes);

        }
    }
}
