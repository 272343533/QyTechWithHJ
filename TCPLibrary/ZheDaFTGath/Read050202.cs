﻿using System;
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

    public class Read050202 : ZdFT_Packet
    {
        /// <summary>
        /// 读采集器属性配置
        /// </summary>
        public Read050202()
            : base()
        {
        }

        public override int Parse(byte[] bytes)
        {
           
            return 10;

        }
    }
}
