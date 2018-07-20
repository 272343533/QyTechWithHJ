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

    public class Read050402 : ZdFT_Packet
    {
        /// <summary>
        /// 读某批数据
        /// </summary>
        public Read050402()
            : base()
        {
        }


        public override int Parse(byte[] bytes)
        {
           
            return 10;

        }
    }
}
