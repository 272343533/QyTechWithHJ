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

    public class Write138008 : ZdFT_Packet
    {
        /// <summary>
        /// 设置阀门角度
        /// </summary>
        public Write138008()
            : base()
        {
        }

        public override byte[] Create(string data)
        {
            string[] dataarr = data.Split(new char[] { ',', ' ' });
            dataarr[1] = (Convert.ToDecimal(dataarr[1])).ToString();
            //dataarr[2] = (Convert.ToDecimal(dataarr[2]) * 100).ToString();

            RawData = new byte[20 +7];
            int ipos = 0;
            RawData[0] = 0x68;
            // 长度
            RawData[1] = (byte)(12 +7);// 0x3C;
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
            // for (int i = 0; i < data.Length; i += 2)
            //{
            //    RawData[ipos++] = (byte)Convert.ToInt16(data.Substring(i, 2), 16);
            // }
            for (int p = 0; p < dataarr.Length; p++)
            {
                string item = dataarr[p];

                if (p == 0)
                {
                    for (int i = item.Length - 2; i >= 0; i -= 2) //倒序
                    {
                        RawData[ipos++] = (byte)Convert.ToInt16(item.Substring(i, 2), 16);
                        StrRawData += item.Substring(i, 2);
                    }
                }
                else
                {
                    string tmp = Convert.ToInt16(item).ToString("X").PadLeft(4, '0');
                    for (int i =2; i >= 0; i -= 2)
                    {
                        RawData[ipos++] = (byte)Convert.ToInt16(tmp.Substring(i, 2), 16);
                        StrRawData += tmp.Substring(i, 2);
                    }
                }
            }

            //校验码
            RawData[ipos++] = GetCheckValue(RawData);// 0xCC;
            //结束符
            RawData[ipos++] = 0x16;


            return RawData;
        }

        public override int Parse(byte[] bytes)
        {
           
            return Parse004000WithRoomId(bytes);

        }
    }
}
