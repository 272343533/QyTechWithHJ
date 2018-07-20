using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeatingDSC.BLL
{
    public class bllCommNo2PacketListNo
    {
        //取协议4位，电话6位（2-3位，7-10） 从1开始计数的
        public static int GetPacketListNo(int bsp_id,string commno)
        {
            int flag=Convert.ToInt16(commno.Substring(9,2))/50;
            string ret = bsp_id.ToString() + commno.Substring(1, 2) + commno.Substring(6, 3) + flag.ToString();
            return Convert.ToInt32(ret);
        }
    }
}
