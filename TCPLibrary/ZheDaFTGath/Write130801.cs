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

    public class Write130801 : ZdFT_Packet
    {
        /// <summary>
        /// 广播温度设置上下限
        /// </summary>
        public Write130801()
            : base()
        {
        }

        public override byte[] Create(string data)
        {
            string[] wds = data.Split(new char[] { ',', ' ' });
            string hexDown = Convert.ToInt32(Convert.ToDecimal(wds[0]) * 100).ToString("X4");
            string hexUp = Convert.ToInt32(Convert.ToDecimal(wds[1]) * 100).ToString("X4");

            return base.Create(hexDown + hexUp);
        }

        public override int Parse(byte[] bytes)
        {

            return 2;

        }
    }
}
