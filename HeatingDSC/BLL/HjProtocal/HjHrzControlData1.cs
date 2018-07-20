using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunMvcExpress.Dao;
using QyTech.HDGprs;
using QyTech.Protocal;


using QyTech.Communication;

namespace HeatingDSC.BLL
{
     public class HjHrzControlData1 : IProduct
    {
        //采集数据
        //public static bsOrganize ORG=new bsOrganize();
        HrzCommGath hcg;

        public HjHrzControlData1()
        {
            Producttype = ProductType.HrzUpCommData;
        }

        //public override void OnHrzCommProgress(HrzCommGath hcg, int[,] CurvePoints)
        //{
        //    base.OnHrzCommProgress(hcg, CurvePoints);
        //}


        public override int Parse(GPRS_DATA_RECORD recdPtr)
        {
            base.Parse(recdPtr);
            List<DetailDevice> hrzjzs = devda.SelectHrzJzDeviceBySim(recdPtr.m_userid);
            int resualt = 0;
            try
            {
                #region 解析数据对象
                HJPlcParaControl1 obj = new HJPlcParaControl1();
                obj.GathDT = Convert.ToDateTime(recdPtr.m_recv_date);
                obj.Det_Id = (Guid)hrzjzs[0].Id;
                obj.bsO_Id = Org.Id;
                obj.GathFlag = 1;

                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0100 - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF1 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0101 - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF2 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0102 - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF3 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0103 - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF4 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0104 - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF5 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0105 - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF6 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0106 - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF7 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0107 - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF8 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0108 - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF9 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x010A - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF10 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x010B - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF11 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x010C - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF12 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0110 - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF16 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0112 - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF17 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0114 - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF18 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0116 - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF19 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0118 - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF20 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x011A - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF21 = (decimal)BitConverter.ToSingle(buff, 0);

                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x011C - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF22 = (decimal)BitConverter.ToUInt16(buff, 0);
                obj.HJhrzPCGathF22 = Convert.ToDecimal((Convert.ToInt16(obj.HJhrzPCGathF22)).ToString("X2"));
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x011D - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF23 = (decimal)BitConverter.ToUInt16(buff, 0);
                obj.HJhrzPCGathF23 = Convert.ToDecimal((Convert.ToInt16(obj.HJhrzPCGathF23)).ToString("X2"));
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x011E - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF24 = (decimal)BitConverter.ToUInt16(buff, 0);
                obj.HJhrzPCGathF24 = Convert.ToDecimal((Convert.ToInt16(obj.HJhrzPCGathF24)).ToString("X2"));
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x011F - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF25 = (decimal)BitConverter.ToUInt16(buff, 0);
                obj.HJhrzPCGathF25 = Convert.ToDecimal((Convert.ToInt16(obj.HJhrzPCGathF25)).ToString("X2"));


                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0120 - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF26 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0121 - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF27 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0122 - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF28 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0124 - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF29 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0126 - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF30 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0128 - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF31 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x012A - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF33 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x012C - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF34 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x012E - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF35 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0130 - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF36 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0140 - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF40 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0142 - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF41 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0144 - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF42 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0146 - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF43 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0148 - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF45 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0149 - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF46 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x014A - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF47 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x014B - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF48 = (decimal)BitConverter.ToUInt16(buff, 0);
                #region modified on 2015-06-01
                //buff = new byte[2];
                //Buffer.BlockCopy(GetData, (0x014C - 0x0100) * 2, buff, 0, 2);
                //CrossHiLow(ref buff);
                //obj.HJhrzPCGathF49 = (decimal)BitConverter.ToUInt16(buff, 0);
                #endregion
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x014D - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF50 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x014E - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF51 = (decimal)BitConverter.ToUInt16(buff, 0);

                #region begin 修改 06-01
                //buff = new byte[4];
                //Buffer.BlockCopy(GetData, (0x0150 - 0x0100) * 2, buff, 0, 4);
                //CrossHiLow(ref buff);
                //obj.HJhrzPCGathF54 = (decimal)BitConverter.ToSingle(buff, 0);
                //buff = new byte[4];
                //Buffer.BlockCopy(GetData, (0x0152 - 0x0100) * 2, buff, 0, 4);
                //CrossHiLow(ref buff);
                //obj.HJhrzPCGathF56 = (decimal)BitConverter.ToSingle(buff, 0);
                //buff = new byte[4];
                //Buffer.BlockCopy(GetData, (0x0154 - 0x0100) * 2, buff, 0, 4);
                //CrossHiLow(ref buff);
                //obj.HJhrzPCGathF58 = (decimal)BitConverter.ToSingle(buff, 0);
                //buff = new byte[4];
                //Buffer.BlockCopy(GetData, (0x0156 - 0x0100) * 2, buff, 0, 4);
                //CrossHiLow(ref buff);
                //obj.HJhrzPCGathF59 = (decimal)BitConverter.ToSingle(buff, 0);
                //buff = new byte[4];
                //Buffer.BlockCopy(GetData, (0x0158 - 0x0100) * 2, buff, 0, 4);
                //CrossHiLow(ref buff);
                //obj.HJhrzPCGathF60 = (decimal)BitConverter.ToSingle(buff, 0);
                //buff = new byte[4];
                //Buffer.BlockCopy(GetData, (0x015A - 0x0100) * 2, buff, 0, 4);
                //CrossHiLow(ref buff);
                //obj.HJhrzPCGathF61 = (decimal)BitConverter.ToSingle(buff, 0);
                
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0150 - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF54 = (decimal)BitConverter.ToUInt16(buff, 0);

                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0151 - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF55 = (decimal)BitConverter.ToUInt16(buff, 0);


                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0152 - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF56 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0154 - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF57 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0155 - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF58 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0156 - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF59 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0158 - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF60 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x015A - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF61 = (decimal)BitConverter.ToSingle(buff, 0);
                #endregion
                
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x015C - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF62 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0160 - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF65 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0161 - 0x0100) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF66 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0162 - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF67 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0164 - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF68 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0166 - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF69 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0168 - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF70 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x016A - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF71 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x016C - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF72 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x016E - 0x0100) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF73 = (decimal)BitConverter.ToSingle(buff, 0);

                using (WSExpressEntities db = new WSExpressEntities())
                {
                    db.AddToHJPlcParaControl1(obj);
                    db.SaveChanges();
                }
                #endregion

                OnProtocalDataReceivedProgress(recdPtr.m_userid,obj);
                
                obj = null;
            }
            catch (Exception ex)
            {
                log.Error("hrzupunit parse:simno:" + recdPtr.m_userid + ":" + ex.InnerException + "-" + ex.Message);
                resualt = -1;
            }
            return resualt;
        }
        public override DeviceCmd CreateReadCommand(string simno)
        {
            return base.CreateModbusRtuReadCommand(simno, 0x01, 0x03, 0x100, 0x16F+1-0x100);
        }

        public override DeviceCmd CreateWriteOneCommand(string simno,string writeflag,decimal sendvalue)
        {
            DeviceCmd cmd = new DeviceCmd();
            cmd.CommNo = simno;
            return cmd;
        }

        public static byte[] PackZipBCD(byte b)
        {
            byte[] bytes = new byte[2];
            //高四位
            bytes[0] = (byte)(b / 10);

            //低四位
            bytes[1] = (byte)(b % 10);

            return bytes;
        }
        public static byte PackBCD(byte b)
        {
            //高四位
            byte b1 = (byte)(b / 10);
            //低四位
            byte b2 = (byte)(b % 10);

            return (byte)((b1 << 4) | b2);
        }
    }
}


