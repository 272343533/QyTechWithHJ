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

    public class Write130200 : ZdFT_Packet
    {


        /// <summary>
        /// 远程设置温度
        /// </summary>
        public Write130200()
            : base()
        {
        }

        public override byte[] Create(string data)
        {
            //转为16禁止
           // string hexstr = Reverse(data);

            RawData = base.Create(data);//
            return RawData;
        }

        //
        public override int Parse(byte[] bytes)
        {

            return Parse004000WithRoomId(bytes);
        }
    }
}
