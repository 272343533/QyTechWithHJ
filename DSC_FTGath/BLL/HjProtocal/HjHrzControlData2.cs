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
     public class HjHrzControlData2 : IProduct
    {
        //采集数据
        //public static bsOrganize ORG=new bsOrganize();
        HrzCommGath hcg;

        public HjHrzControlData2()
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
                obj.Det_Id = hrzjzs[0].Id ;
                obj.bsO_Id = Org.Id;

                obj.GathFlag = 2;

                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0170 - 0x0170) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF74 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0172 - 0x0170) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF75 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0174 - 0x0170) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF76 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0176 - 0x0170) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF77 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0178 - 0x0170) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF78 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0179 - 0x0170) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF79 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x017A - 0x0170) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF80 = (decimal)BitConverter.ToUInt16(buff, 0);
                //deleted on 2015-06-01
                //buff = new byte[2];
                //Buffer.BlockCopy(GetData, (0x017B - 0x0170) * 2, buff, 0, 2);
                //CrossHiLow(ref buff);
                //obj.HJhrzPCGathF81 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x017C - 0x0170) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF82 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x017D - 0x0170) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF83 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x017E - 0x0170) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF84 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0180 - 0x0170) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF86 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0181 - 0x0170) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF87 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0182 - 0x0170) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF88 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0184 - 0x0170) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF89 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0186 - 0x0170) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF90 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0188 - 0x0170) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF91 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x018A - 0x0170) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF92 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x018C - 0x0170) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF93 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x018E - 0x0170) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF94 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0190 - 0x0170) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF95 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0191 - 0x0170) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF96 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0192 - 0x0170) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF97 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0194 - 0x0170) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF98 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0196 - 0x0170) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF99 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0198 - 0x0170) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF100 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x019A - 0x0170) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF101 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x019C - 0x0170) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF102 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x019E - 0x0170) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF103 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x01A0 - 0x0170) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF104 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x01A1 - 0x0170) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF105 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x01A2 - 0x0170) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF106 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x01A4 - 0x0170) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF107 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x01A6 - 0x0170) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF108 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x01A8 - 0x0170) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF109 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x01AA - 0x0170) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF110 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x01AC - 0x0170) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF111 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x01B0 - 0x0170) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF114 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x01B2 - 0x0170) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJhrzPCGathF115 = (decimal)BitConverter.ToSingle(buff, 0);

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
            return base.CreateModbusRtuReadCommand(simno, 0x01, 0x03, 0x170, 0x1B3+1-0x170);
        }

        public override DeviceCmd CreateWriteOneCommand(string simno, string writeflag, decimal value)
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


