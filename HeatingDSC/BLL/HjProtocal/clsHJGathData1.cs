using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAHeating;
using DAHeating.BLL;
using HeatingDSC.DLL;

namespace HeatingDSC.BLL
{
    public class clsHJGathData1 : IProduct
    {
        public clsHJGathData1 obj = new clsHJGathData1();
        private byte[] buff;
        public clsHJGathData1()
        {
            Producttype = ProductType.HJGathData1;
        }
        public override int Parse(GPRS_DATA_RECORD recdPtr)
        {
            base.Parse(recdPtr);
            List<DeviceDetail> hrzjzs = devda.SelectHrzJzDeviceBySim(recdPtr.m_userid);
            int resualt = 0;
            try
            {
                #region 解析数据对象
                HJGathData1 obj = new HJGathData1();
                obj.GathDT = Convert.ToDateTime(recdPtr.m_recv_date);
                obj.DeviDetaID = (int)hrzjzs[0].DeviceTypeID;
                obj.OrganizeID = Org.OrganizeID;
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0000 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF1 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0002 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF2 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0004 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF3 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0006 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF4 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0008 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF5 = (decimal)BitConverter.ToUInt64(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x000A * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF6 = (decimal)BitConverter.ToUInt64(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, 0x000C * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJGathF7 = (decimal)BitConverter.ToUInt32(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, 0x000D * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJGathF8 = (decimal)BitConverter.ToUInt32(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, 0x000E * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJGathF9 = (decimal)BitConverter.ToUInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0010 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF11 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0012 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF12 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0014 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF13 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0016 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF14 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0018 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF15 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x001A * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF16 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x001C * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF17 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x001E * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF18 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0020 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF19 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0022 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF20 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0024 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF21 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0026 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF22 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0030 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF26 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0032 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF27 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0034 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF28 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, 0x0036 * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJGathF29 = (decimal)BitConverter.ToUInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0038 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF31 = (decimal)BitConverter.ToUInt64(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0040 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF35 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0042 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF36 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0044 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF37 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, 0x0046 * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJGathF38 = (decimal)BitConverter.ToUInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0048 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF40 = (decimal)BitConverter.ToUInt64(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0050 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF44 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0052 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF45 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0054 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF46 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, 0x0056 * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJGathF47 = (decimal)BitConverter.ToUInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0058 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF49 = (decimal)BitConverter.ToUInt64(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0060 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF54 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0062 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF56 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0064 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF58 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, 0x0066 * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJGathF59 = (decimal)BitConverter.ToUInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0068 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF61 = (decimal)BitConverter.ToUInt64(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0070 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF65 = (decimal)BitConverter.ToInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0072 * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF66 = (decimal)BitConverter.ToInt32(buff, 0);
                glfda.heatingdb = new HeatingDbEntities();
                glfda.heatingdb.AddToHJGathData1(obj);
                glfda.heatingdb.SaveChanges();
                #endregion
                //OnHJGathData1Progress(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                log.Error("hrzupunit parse:simno:" + recdPtr.m_userid + ":" + ex.InnerException + "-" + ex.Message);
                resualt = -1;
            }
            return resualt;
        }
    }
}
