using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QyProtocal.Protocal.TypeChange
{
    /// <summary>
    ///10进制与压缩BCD码转换
    /// </summary>
    public class exPackZipBCD
    {
        public static string FromBytes(byte[] bytes)
        {
            if (bytes.Length == 1)
                return bytes[0].ToString("X2");
            else if (bytes.Length == 2)
                return BitConverter.ToUInt16(bytes, 0).ToString("X2");
            else if (bytes.Length == 4)
                return BitConverter.ToUInt32(bytes, 0).ToString("X2");
            else
                return BitConverter.ToUInt64(bytes, 0).ToString("X2");
        }

        /// <summary>
        /// 16个字节转换为压缩bcd码
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static byte[] ToBytes(ushort i)
        {
            byte[] ret = new byte[2];
            //高8位
            byte b1 = (byte)(i / 100);
            //低8位
            byte b2 = (byte)(i % 100);

            ret[1] = ToPackZipBCD(b1);
            ret[0] = ToPackZipBCD(b2);

            return ret;
        }

        public static short ToUnifType(ushort obj)
        {
            byte[] bytes = ToBytes(obj);
            //bytes[1] = (byte)Convert.ToInt32(bytes[1].ToString("X2"));
            return BitConverter.ToInt16(bytes, 0);//说明高字节为无符号，但数据类型给的整形，不正确

        }





        private static byte ToPackZipBCD(byte b)
        {
            //高四位
            byte b1 = (byte)(b / 10);
            //低四位
            byte b2 = (byte)(b % 10);

            return (byte)((b1 << 4) | b2);
        }

    }
}
