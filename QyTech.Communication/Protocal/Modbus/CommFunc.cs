using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QyTech.Protocal
{

    public class CommFunc
    {
        public static byte[] HexCmd2Bytes(string data)
        {
            byte[] bytes = new byte[data.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(data.Substring(i * 2, 2), 16);
            }
            return bytes;
        }

        public static string Bytes2HexCmd(byte[] bytes)
        {
            if (bytes != null)
            {
                
                StringBuilder strbuilder = new StringBuilder();
                foreach (byte cmdbyte in bytes)
                {
                    strbuilder.Append(cmdbyte.ToString("X2"));
                }
                return strbuilder.ToString();
            }
            return null;
        }

        #region 16进制与压缩BCD码转换

        /// <summary>  
        /// BCD码转换16进制(压缩BCD)  
        /// </summary>  
        /// <param name="strTemp"></param>  
        /// <returns></returns>  
        public static Byte[] Hex2PackZipBCD(string strTemp, int IntLen)
        {
            try
            {
                Byte[] Temp = ConvertFrom(strTemp.Trim());
                Byte[] return_Byte = new Byte[IntLen];
                if (IntLen != 0)
                {
                    if (Temp.Length < IntLen)
                    {
                        for (int i = 0; i < IntLen - Temp.Length; i++)
                        {
                            return_Byte[i] = 0x00;
                        }
                    }
                    Array.Copy(Temp, 0, return_Byte, IntLen - Temp.Length, Temp.Length);
                    return return_Byte;
                }
                else
                {
                    return Temp;
                }
            }
            catch
            { return null; }
        }
        /// <summary>  
        /// 16进制转换BCD（解压BCD）  
        /// </summary>  
        /// <param name="AData"></param>  
        /// <returns></returns>  
        public static string PackZipBCD2Hex(Byte[] AData)
        {
            try
            {
                StringBuilder sb = new StringBuilder(AData.Length * 2);
                foreach (Byte b in AData)
                {
                    sb.Append(b >> 4);
                    sb.Append(b & 0x0f);
                }
                return sb.ToString();
            }
            catch { return null; }
        }

        private static Byte[] ConvertFrom(string strTemp)
        {
            try
            {
                if (Convert.ToBoolean(strTemp.Length & 1))//数字的二进制码最后1位是1则为奇数  
                {
                    strTemp = "0" + strTemp;//数位为奇数时前面补0  
                }
                Byte[] aryTemp = new Byte[strTemp.Length / 2];
                for (int i = 0; i < (strTemp.Length / 2); i++)
                {
                    aryTemp[i] = (Byte)(((strTemp[i * 2] - '0') << 4) | (strTemp[i * 2 + 1] - '0'));
                }
                return aryTemp;//高位在前  
            }
            catch
            { return null; }
        }

        #endregion

        public static byte[] PackBCD(ushort b)
        {
            byte[] bytes = new byte[2];
            //高四位
            bytes[0] = (byte)(b / 10);

            //低四位
            bytes[1] = (byte)(b % 10);

            return bytes;
        }
    }
}

