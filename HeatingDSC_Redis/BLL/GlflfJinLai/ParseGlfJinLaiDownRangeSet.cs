using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunMvcExpress.Dao;


namespace HeatingDSC.BLL
{
    public class ParseGlfJinLaiDownRangeSet: IProduct
    {
        int _orgIdofJinLai = 1;
        public AlarmRangeSet rangset = new AlarmRangeSet();

        public ParseGlfJinLaiDownRangeSet()
        {
            Producttype = ProductType.GlfOfJinLaiDownRange;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">津涞锅炉房ID</param>
        /// <returns>要下发的字节数组</returns>
        public override byte[] Create(object obj)
        {
            // base.Create(obj);
            _orgIdofJinLai = (int)obj;
            byte[] data = CreateRangeData(_orgIdofJinLai);
            return data;
        }
       
        /// <summary>
        /// 量程设置数据转换，56-73寄存器的量程上下限数据,共18项
        /// </summary>
        /// <returns>字节数组</returns>
        private byte[] CreateRangeData(int glfid)
        {
            //byte[] buff = new byte[18 * 4];
            //try
            //{
            //    List<vwlyHrzRangeSet> rsS = orgDa.SelectOrgRangeSet(glfid);
            //    int byteindex = -4;
            //    for (int i = 0; i < rsS.Count; i++)
            //    {
            //        BitConverter.GetBytes((float)rsS[i].RangeUp).CopyTo(buff, byteindex += 4);
            //        BitConverter.GetBytes((float)rsS[i].RangeDown).CopyTo(buff, byteindex += 4);
            //    }
            //}
            //catch
            //{
                return null;
            //}
            //return buff;
        } 
    }
}
