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

    public class Write138009 : ZdFT_Packet
    {
        /// <summary>
        /// 阀门进入远程控制模式
        /// </summary>
        public Write138009()
            : base()
        {
        }

        public override int Parse(byte[] bytes)
        {
           
            return Parse004000WithRoomId(bytes);

        }
    }
}
