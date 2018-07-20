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

    public class Write048001 : ZdFT_Packet
    {

        /// <summary>
        /// 采集器校时
        /// </summary>
        public Write048001()
            : base()
        {
        }

        public override int Parse(byte[] bytes)
        {
            try
            {
                Parse004000(bytes);
            } 
            catch (Exception ex)
            {
                log.Error("048001.Parse" + ex.Message);
                return 0;
            }
            return 1;

        }
    }
}
