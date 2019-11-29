using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QyProtocal.Protocal.TypeChange;

namespace QyTech.Communication
{
    public class IBytesConverter
    {

        /// <summary>
        /// 对象转换为byte[]
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="UnifType">对象的统一类型</param>
        /// <returns></returns>
        public static byte[] ToBytes(object obj,string UnifType)
        {
           byte[] buff= new byte[1];
            if (UnifType == "FLOAT")
                buff = BitConverter.GetBytes(Convert.ToSingle(obj));
            else if (UnifType == "DOUBLE")
                buff = BitConverter.GetBytes(Convert.ToDouble(obj));
            else if (UnifType == "UINT16")
                buff = BitConverter.GetBytes(Convert.ToUInt16(obj));
            else if (UnifType == "UINT32")
                buff =BitConverter.GetBytes(Convert.ToUInt32(obj));
            else if (UnifType == "UINT64")
                buff = BitConverter.GetBytes(Convert.ToUInt64(obj));
            else if (UnifType == "INT16")
                buff = BitConverter.GetBytes(Convert.ToInt16(obj));
            else if (UnifType == "INT32")
                buff = BitConverter.GetBytes(Convert.ToInt32(obj));
            else if (UnifType == "INT64")
                buff = BitConverter.GetBytes(Convert.ToInt64(obj));
            else if (UnifType == "CHAR")
                buff = BitConverter.GetBytes(Convert.ToChar(obj));
            else if (UnifType == "BOOLEAN")
                buff = BitConverter.GetBytes(Convert.ToBoolean(obj));
            else if (UnifType == "WORD")
                buff = BitConverter.GetBytes(Convert.ToUInt16(obj));
            else if (UnifType == "BCDCODE")
                buff = exPackZipBCD.ToBytes(Convert.ToUInt16(obj));
            else if (UnifType == "HEATTEMP")
                buff = exHEATTEMP.ToBytes(obj.ToString());
            else if (UnifType == "BIN_CN4")
                buff = exPackBIN_CN4.ToBytes(Convert.ToSingle(obj));
            else if (UnifType == "BIN_CN8")
                buff = exPackBIN_CN8.ToBytes(Convert.ToDouble(obj));
            else if (UnifType == "Coil05")
                buff = Coil05.ToBytes(Convert.ToBoolean(obj));

            return buff;
        }


        /// <summary>
        /// byte[]转换为对象
        /// </summary>
        /// <param name="buff">字节数组</param>
        /// <param name="UnifType">要转为的类型</param>
        /// <returns></returns>
        public static object FromBytes(byte[] buff, string UnifType)
        {
            object val;
            if (UnifType == "FLOAT")
            {
                val = (decimal)BitConverter.ToSingle(buff, 0);
                val = Math.Round((decimal)val, 5);
            }
            else if (UnifType == "DOUBLE")
            {
                val = (decimal)BitConverter.ToDouble(buff, 0);
                val = Math.Round((decimal)val, 5);
            }
            else if (UnifType == "UINT16")
                val = BitConverter.ToUInt16(buff, 0);
            else if (UnifType == "UINT32")
                val = BitConverter.ToUInt32(buff, 0);
            else if (UnifType == "UINT64")
                val = BitConverter.ToUInt64(buff, 0);
            else if (UnifType == "INT16")
                val = BitConverter.ToInt16(buff, 0);
            else if (UnifType == "INT32")
                val = BitConverter.ToInt32(buff, 0);
            else if (UnifType == "INT64")
                val = BitConverter.ToInt64(buff, 0);
            else if (UnifType == "CHAR")
                val = BitConverter.ToChar(buff, 0);
            else if (UnifType == "BOOLEAN")
                val = BitConverter.ToBoolean(buff, 0);
            else if (UnifType == "WORD")
                val = BitConverter.ToUInt16(buff, 0).ToString();
            else if (UnifType == "BCDCODE")
                val = exPackZipBCD.FromBytes(buff).PadLeft(4, '0');
            else if (UnifType == "HEATTEMP")
                val = exHEATTEMP.FromBytes(buff);
            else if (UnifType == "DWORD")
                val = BitConverter.ToUInt32(buff, 0).ToString();
            else if (UnifType == "BIN_CN4")
                val = exPackBIN_CN4.FromBytes(buff);
            else if (UnifType == "BIN_CN8")
                val = exPackBIN_CN8.FromBytes(buff);
            else if (UnifType == "Coil05")
                val = Coil05.FromBytes(buff);
            else
                val = -9999;

            return val;
        }
     
        /// <summary>
        /// 把object转换为需要的UnifType类型
        /// </summary>
        /// <param name="obj">数据</param>
        /// <param name="UnifType">要转换成的类型</param>
        /// <returns></returns>
        public static object ToRightType(object obj, string UnifType)
        {
            object val;
            if (UnifType == "FLOAT")
                val = Convert.ToSingle(obj);
            else if (UnifType == "DOUBLE")
                val = Convert.ToDouble(obj);
            else if (UnifType == "UINT16")
                val = Convert.ToUInt16(obj);
            else if (UnifType == "UINT32")
                val = Convert.ToUInt32(obj);
            else if (UnifType == "UINT64")
                val = Convert.ToUInt64(obj);
            else if (UnifType == "INT16")
                val = Convert.ToInt16(obj);
            else if (UnifType.ToLower()=="UINT16".ToLower())
                val = Convert.ToUInt16(obj);
            else if (UnifType == "INT32")
                val = Convert.ToInt32(obj);
            else if (UnifType.ToLower() == "UINT32".ToLower())
                val = Convert.ToUInt32(obj);
            else if (UnifType == "INT64")
                val = Convert.ToInt64(obj);
            else if (UnifType.ToLower() == "UINT64".ToLower())
                val = Convert.ToUInt64(obj);
            else if (UnifType == "CHAR")
                val = Convert.ToChar(obj);
            else if (UnifType == "BOOLEAN")
                val = Convert.ToBoolean(obj);
            else if (UnifType == "WORD")
                val = Convert.ToUInt16(obj);
            else if (UnifType == "BCDCODE")
                val = exPackZipBCD.ToUnifType(Convert.ToUInt16(obj));
            else if (UnifType == "HEATTEMP")
                val = exHEATTEMP.ToUnifType(obj.ToString());
            else
                val =-9999;
            return val;
        }

        
    }
}
