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

    public class Write130109: ZdFT_Packet
    {
        /// <summary>
        /// 广播设置阀门角度
        /// </summary>
        public Write130109()
            : base()
        {
        }

        public override byte[] Create(string data)
        {
            RawData = new byte[20 + 1];
            int ipos = 0;
            RawData[0] = 0x68;
            // 长度
            RawData[1] = (byte)(12 + 1);// 0x3C;
            RawData[2] = 0x00;
            RawData[3] = RawData[1];
            RawData[4] = RawData[2];

            ipos = 5;
            RawData[ipos++] = 0x68;

            RawData[ipos++] = 0x41;

            RawData[ipos++] = 0x02;
            RawData[ipos++] = 0x37;
            RawData[ipos++] = 0x01;
            RawData[ipos++] = 0x00;
            RawData[ipos++] = 0x00;

            RawData[ipos++] = (byte)Convert.ToInt16(CommFlag.Substring(0, 2), 16);// 0x0C;
            RawData[ipos++] = 0xE0;
            RawData[ipos++] = 0x00;
            RawData[ipos++] = 0x00;

            RawData[ipos++] = (byte)Convert.ToInt16(CommFlag.Substring(2, 2), 16);// 0x40;
            RawData[ipos++] = (byte)Convert.ToInt16(CommFlag.Substring(4, 2), 16);// 0x03;

            //数据域
            StrRawData = "";



            RawData[ipos++] = (byte)Convert.ToInt16(data);
            StrRawData += Convert.ToInt16(data).ToString("X").PadLeft(2, '0');


            //校验码
            RawData[ipos++] = GetCheckValue(RawData);// 0xCC;
            //结束符
            RawData[ipos++] = 0x16;


            return RawData;
        }
        public override int Parse(byte[] bytes)
        {
           
            return 2;

        }
    }
}
