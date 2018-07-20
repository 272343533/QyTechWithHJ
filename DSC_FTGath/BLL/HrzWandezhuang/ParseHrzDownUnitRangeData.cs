using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunMvcExpress.Dao;
namespace HeatingDSC.BLL
{
    public class ParseHrzDownUnitRangeData : IProduct
    {
          Guid _orgId;
        public AlarmRangeSet rangset = new AlarmRangeSet();

        public ParseHrzDownUnitRangeData()
        {
            Producttype = ProductType.HrzDownUnitRangeData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">津涞锅炉房ID</param>
        /// <returns>要下发的字节数组</returns>
        public override byte[] Create(object obj)
        {
            // base.Create(obj);
            _orgId = (Guid)obj;
            byte[] data = CreateRangeData(_orgId);
            return data;
        }
       
        /// <summary>
        /// 量程设置数据转换，56-73寄存器的量程上下限数据,共18项
        /// </summary>
        /// <returns>字节数组</returns>
        public byte[] CreateRangeData(Guid devdetailid)
        {
            byte[] buff = new byte[38 * 4];
            try
            {
                List<vwlyHrzRangeSet> rsS = orgDa.SelectOrgRangeSet(devdetailid);
                if (rsS == null)
                    return null;
                int byteindex = -4;
                int index=0;
                for (int i = 0; i < rsS.Count; i++)
                {
                    BitConverter.GetBytes((float)rsS[i].RangeUp).CopyTo(buff, byteindex += 4);
                    BitConverter.GetBytes((float)rsS[i].RangeDown).CopyTo(buff, byteindex += 4);
                    index=i;
                }
                index++;
                if (index < 19)
                {
                    for (int i = index; i < 19; i++)
                    {
                        BitConverter.GetBytes((float)1.00).CopyTo(buff, byteindex += 4);
                        BitConverter.GetBytes((float)0).CopyTo(buff, byteindex += 4);
                    }
                }
            }
            catch
            {
                return null;
            }
            return buff;
        }


        /// <summary>
        /// 量程设置数据转换，56-73寄存器的量程上下限数据,共18项
        /// </summary>
        /// <returns>字节数组</returns>
        public byte[] CreateRangeData(decimal[,] rangedata)
        {
            if (rangedata.GetUpperBound(0) != 18)
                throw new Exception("应该包括19项上下限数据");
            byte[] buff = new byte[38 * 4];
            try
            {
              
                int byteindex = -4;
                for (int i = 0; i <= rangedata.GetUpperBound(0); i++)
                {
                    BitConverter.GetBytes((float)rangedata[i,1]).CopyTo(buff, byteindex += 4);
                    BitConverter.GetBytes((float)rangedata[i,0]).CopyTo(buff, byteindex += 4);
                }
            }
            catch
            {
                return null;
            }
            return buff;
        } 
    }
}
