using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeatingDSC.BLL
{
    public class ParseHrzDownWeather : IProduct
    {
        public override byte[] Create(object obj)
        {
            Guid orgid=(Guid)obj;
            //0x0180-0186
            //得到数据库中新（最后的）的气象条件数据
            float[] qxdata=orgDa.SelectOrgWeather(orgid);
            if (qxdata == null)
                return null;
            byte[] data=new byte[12];
            
            //数据长度(类型1+数据项*4)
            int byteindex = 0;
            //BitConverter.GetBytes(1.0f).CopyTo(data, 0);
            Byte[] buff = new byte[4];
            buff = BitConverter.GetBytes(qxdata[0]);CrossHiLow(ref buff);
            buff.CopyTo(data, byteindex);
            buff = BitConverter.GetBytes(qxdata[2]);CrossHiLow(ref buff);
            buff.CopyTo(data, byteindex+=4);
            buff = BitConverter.GetBytes(qxdata[1]); CrossHiLow(ref buff);
            buff.CopyTo(data, byteindex+=4);

            return data;
        }
    }
}
