using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QyProtocal.Protocal.TypeChange
{

    /// <summary>
    /// 自定义转换1-温度编码转换
    /// </summary>
    public class exHEATTEMP
    {

        public static string FromBytes(byte[] bytes)
        {
            int pnt = bytes[0];
            if (pnt > 127)
                pnt -= 256;

            return pnt + "," + bytes[1].ToString();
        }

        public static byte[] ToBytes(string ht)
        {
            byte[] bytes = new byte[2];

            string[] strs = ht.Split(new char[] { ',' });
            sbyte outwd = Convert.ToSByte(strs[0]);

            if (outwd >= 0)
                bytes[0] = (byte)outwd;
            else

                bytes[0] = Convert.ToByte(256 + outwd);

            bytes[1] = Convert.ToByte(strs[1]);

            return bytes;
        }

        public static short ToUnifType(string ht)
        {
            byte[] bytes = ToBytes(ht);
            return BitConverter.ToInt16(bytes, 0);//说明高字节为无符号，但数据类型给的整形，不正确

        }

       
    }
}
