using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Reflection;
using SunMvcExpress.Dao;
using QyTech.Communication;
using QyTech.Protocal.Modbus;
using QyTech.HDGprs;
using QyTech.Protocal;
using HjCommDA;
using SunMvcExpress.Core.BLL;
using QyTech.Core.BLL;

namespace QyTech.HjProtocal1
{
    public class WritePart1 : IProtocal
    {
        //采集数据
        //public static bsOrganize ORG=new bsOrganize();
        HrzCommGath hcg;

        public WritePart1(int bsP_Id):base(bsP_Id)
        {
        }

        public override int Parse(GPRS_DATA_RECORD recdPtr)
        {
            if (base.Parse(recdPtr) == 1)
                return -1;
            //log.Info("Parse:10");
            Dev = BaseDABll.GetDtuProduct(recdPtr.m_userid);
            Org = BaseDABll.GetOrganize((Guid)Dev.AZWZ);

            List<DetailDevice> hrzjzs = BaseDABll.GetDetailDevice(Dev.Id);
            
            int resualt = 1;
            try
            {
                HJPlcParaControl1 obj = new HJPlcParaControl1();
                obj.GathDt = Convert.ToDateTime(recdPtr.m_recv_date);
                obj.Det_Id = (Guid)hrzjzs[0].Id;
                obj.bsO_Id = Org.Id;

                int bufflen;
                Type type = obj.GetType(); //获取类型
                int InitAddr = Convert.ToInt32(bsProtItems[0].StartRegAddress.Substring(2), 16);
                int ItemAddr;

                foreach (bsProtItem pi in bsProtItems)
                {
                    try
                    {
                        bufflen = (int)pi.RegCount * 2;
                        buff = new byte[bufflen];
                        ItemAddr = Convert.ToInt32(pi.StartRegAddress.Substring(2), 16);
                        System.Reflection.PropertyInfo propertyInfo = type.GetProperty(pi.FieldName); //获取指定名称的属性

                        Buffer.BlockCopy(GetData, (ItemAddr - InitAddr) * 2, buff, 0, bufflen);
                        CrossHiLow(ref buff);

                        obj = (HJPlcParaControl1)SetValueByReflectionFromBytes<HJPlcParaControl1>(obj, pi, propertyInfo, buff);


                    }
                    catch (Exception ex)
                    {
                        log.Error("ReadData.Parse(" + pi.FieldName + "):" + ex.Message);
                    }
                }

                try
                {
                    obj.HJhrzPCGathF22to25 = Convert.ToDateTime("20" + obj.HJhrzPCGathF22.ToString().Substring(0, 2) + "-" + obj.HJhrzPCGathF22.ToString().Substring(2, 2) +
                        "-" + obj.HJhrzPCGathF23.ToString().Substring(0, 2) + " " + obj.HJhrzPCGathF23.ToString().Substring(2, 2) + ":" + obj.HJhrzPCGathF24.ToString().Substring(0, 2) +
                        ":" + obj.HJhrzPCGathF24.ToString().Substring(2, 2));

                }
                catch { }


                #region 解析数据对象 nouse

                //buff = new byte[2];
                //Buffer.BlockCopy(GetData, (0x0100 - 0x0100) * 2, buff, 0, 2);
                //CrossHiLow(ref buff);
                //obj.F100 = (decimal)BitConverter.ToUInt16(buff, 0);
                //buff = new byte[2];
                //Buffer.BlockCopy(GetData, (0x0101 - 0x0100) * 2, buff, 0, 2);
                //CrossHiLow(ref buff);
                //obj.F101 = (decimal)BitConverter.ToUInt16(buff, 0);
                //buff = new byte[2];
                //Buffer.BlockCopy(GetData, (0x0102 - 0x0100) * 2, buff, 0, 2);
                //CrossHiLow(ref buff);
                //obj.F102 = (decimal)BitConverter.ToUInt16(buff, 0);
                //buff = new byte[2];
                //Buffer.BlockCopy(GetData, (0x0103 - 0x0100) * 2, buff, 0, 2);
                //CrossHiLow(ref buff);
                //obj.F103 = (decimal)BitConverter.ToUInt16(buff, 0);
                //buff = new byte[2];
                //Buffer.BlockCopy(GetData, (0x0104 - 0x0100) * 2, buff, 0, 2);
                //CrossHiLow(ref buff);
                //obj.F104 = (decimal)BitConverter.ToUInt16(buff, 0);
                //buff = new byte[2];
                //Buffer.BlockCopy(GetData, (0x0105 - 0x0100) * 2, buff, 0, 2);
                //CrossHiLow(ref buff);
                //obj.F105 = (decimal)BitConverter.ToUInt16(buff, 0);
                //buff = new byte[2];
                //Buffer.BlockCopy(GetData, (0x0106 - 0x0100) * 2, buff, 0, 2);
                //CrossHiLow(ref buff);
                //obj.F106 = (decimal)BitConverter.ToUInt16(buff, 0);
                //buff = new byte[2];
                //Buffer.BlockCopy(GetData, (0x0107 - 0x0100) * 2, buff, 0, 2);
                //CrossHiLow(ref buff);
                //obj.F107 = (decimal)BitConverter.ToUInt16(buff, 0);
                //buff = new byte[4];
                //Buffer.BlockCopy(GetData, (0x0108 - 0x0100) * 2, buff, 0, 4);
                //CrossHiLow(ref buff);
                //obj.F108 = (decimal)BitConverter.ToSingle(buff, 0);
                //buff = new byte[2];
                //Buffer.BlockCopy(GetData, (0x010A - 0x0100) * 2, buff, 0, 2);
                //CrossHiLow(ref buff);
                //obj.F10A = (decimal)BitConverter.ToUInt16(buff, 0);
                //buff = new byte[2];
                //Buffer.BlockCopy(GetData, (0x010B - 0x0100) * 2, buff, 0, 2);
                //CrossHiLow(ref buff);
                //obj.F10B = (decimal)BitConverter.ToUInt16(buff, 0);
                //buff = new byte[2];
                //Buffer.BlockCopy(GetData, (0x010C - 0x0100) * 2, buff, 0, 2);
                //CrossHiLow(ref buff);
                //obj.F10C = (decimal)BitConverter.ToUInt16(buff, 0);
                //buff = new byte[4];
                //Buffer.BlockCopy(GetData, (0x0110 - 0x0100) * 2, buff, 0, 4);
                //CrossHiLow(ref buff);
                //obj.F110 = (decimal)BitConverter.ToSingle(buff, 0);
                //buff = new byte[4];
                //Buffer.BlockCopy(GetData, (0x0112 - 0x0100) * 2, buff, 0, 4);
                //CrossHiLow(ref buff);
                //obj.F112 = (decimal)BitConverter.ToSingle(buff, 0);
                //buff = new byte[4];
                //Buffer.BlockCopy(GetData, (0x0114 - 0x0100) * 2, buff, 0, 4);
                //CrossHiLow(ref buff);
                //obj.F114 = (decimal)BitConverter.ToSingle(buff, 0);
                //buff = new byte[4];
                //Buffer.BlockCopy(GetData, (0x0116 - 0x0100) * 2, buff, 0, 4);
                //CrossHiLow(ref buff);
                //obj.F116 = (decimal)BitConverter.ToSingle(buff, 0);
                //buff = new byte[4];
                //Buffer.BlockCopy(GetData, (0x0118 - 0x0100) * 2, buff, 0, 4);
                //CrossHiLow(ref buff);
                //obj.F118 = (decimal)BitConverter.ToSingle(buff, 0);
                //buff = new byte[4];
                //Buffer.BlockCopy(GetData, (0x011A - 0x0100) * 2, buff, 0, 4);
                //CrossHiLow(ref buff);
                //obj.F11A = (decimal)BitConverter.ToSingle(buff, 0);

                //buff = new byte[2];
                //Buffer.BlockCopy(GetData, (0x011C - 0x0100) * 2, buff, 0, 2);
                //CrossHiLow(ref buff);
                //obj.F11C = BitConverter.ToUInt16(buff, 0).ToString("X2");
                //buff = new byte[2];
                //Buffer.BlockCopy(GetData, (0x011D - 0x0100) * 2, buff, 0, 2);
                //CrossHiLow(ref buff);
                //obj.F11D = BitConverter.ToUInt16(buff, 0).ToString("X2");
                //buff = new byte[2];
                //Buffer.BlockCopy(GetData, (0x011E - 0x0100) * 2, buff, 0, 2);
                //CrossHiLow(ref buff);
                //obj.F11E = BitConverter.ToUInt16(buff, 0).ToString("X2");
                ////obj.F11E = Convert.ToDecimal((Convert.ToInt16(obj.F11E)).ToString("X2"));
                //buff = new byte[2];
                //Buffer.BlockCopy(GetData, (0x011F - 0x0100) * 2, buff, 0, 2);
                //CrossHiLow(ref buff);
                //obj.F11F = BitConverter.ToUInt16(buff, 0).ToString("X2");
                //obj.F11C_11F =Convert.ToDateTime("20"+obj.F11C.Substring(0,2)+"-"+obj.F11C.Substring(2,2)+
                //    "-"+obj.F11D.Substring(0,2)+" "+obj.F11D.Substring(2,2) +":"+obj.F11E.Substring(0,2)+
                //    ":"+obj.F11E.Substring(2,2));
                

                //buff = new byte[2];
                //Buffer.BlockCopy(GetData, (0x0120 - 0x0100) * 2, buff, 0, 2);
                //CrossHiLow(ref buff);
                //obj.F120 = (decimal)BitConverter.ToUInt16(buff, 0);
                //buff = new byte[2];
                //Buffer.BlockCopy(GetData, (0x0121 - 0x0100) * 2, buff, 0, 2);
                //CrossHiLow(ref buff);
                //obj.F121 = (decimal)BitConverter.ToUInt16(buff, 0);
                //buff = new byte[4];
                //Buffer.BlockCopy(GetData, (0x0122 - 0x0100) * 2, buff, 0, 4);
                //CrossHiLow(ref buff);
                //obj.F122 = (decimal)BitConverter.ToSingle(buff, 0);
                //buff = new byte[4];
                //Buffer.BlockCopy(GetData, (0x0124 - 0x0100) * 2, buff, 0, 4);
                //CrossHiLow(ref buff);
                //obj.F124 = (decimal)BitConverter.ToSingle(buff, 0);
                //buff = new byte[4];
                //Buffer.BlockCopy(GetData, (0x0126 - 0x0100) * 2, buff, 0, 4);
                //CrossHiLow(ref buff);
                //obj.F126 = (decimal)BitConverter.ToSingle(buff, 0);
                //buff = new byte[2];
                //Buffer.BlockCopy(GetData, (0x0128 - 0x0100) * 2, buff, 0, 2);
                //CrossHiLow(ref buff);
                //obj.F128 = (decimal)BitConverter.ToUInt16(buff, 0);
                //buff = new byte[4];
                //Buffer.BlockCopy(GetData, (0x012A - 0x0100) * 2, buff, 0, 4);
                //CrossHiLow(ref buff);
                //obj.F12A = (decimal)BitConverter.ToSingle(buff, 0);
                //buff = new byte[4];
                //Buffer.BlockCopy(GetData, (0x012C - 0x0100) * 2, buff, 0, 4);
                //CrossHiLow(ref buff);
                //obj.F12C = (decimal)BitConverter.ToSingle(buff, 0);
                //buff = new byte[4];
                //Buffer.BlockCopy(GetData, (0x012E - 0x0100) * 2, buff, 0, 4);
                //CrossHiLow(ref buff);
                //obj.F12E = (decimal)BitConverter.ToSingle(buff, 0);
                //buff = new byte[4];
                //Buffer.BlockCopy(GetData, (0x0130 - 0x0100) * 2, buff, 0, 4);
                //CrossHiLow(ref buff);
                //obj.F130 = (decimal)BitConverter.ToSingle(buff, 0);
                //buff = new byte[4];
                //Buffer.BlockCopy(GetData, (0x0140 - 0x0100) * 2, buff, 0, 4);
                //CrossHiLow(ref buff);
                //obj.F140 = (decimal)BitConverter.ToSingle(buff, 0);
                //buff = new byte[4];
                //Buffer.BlockCopy(GetData, (0x0142 - 0x0100) * 2, buff, 0, 4);
                //CrossHiLow(ref buff);
                //obj.F142 = (decimal)BitConverter.ToSingle(buff, 0);
                //buff = new byte[4];
                //Buffer.BlockCopy(GetData, (0x0144 - 0x0100) * 2, buff, 0, 4);
                //CrossHiLow(ref buff);
                //obj.F144 = (decimal)BitConverter.ToSingle(buff, 0);
                //buff = new byte[2];
                //Buffer.BlockCopy(GetData, (0x0146 - 0x0100) * 2, buff, 0, 2);
                //CrossHiLow(ref buff);
                //obj.F146 = (decimal)BitConverter.ToUInt16(buff, 0);
                //buff = new byte[2];
                //Buffer.BlockCopy(GetData, (0x0148 - 0x0100) * 2, buff, 0, 2);
                //CrossHiLow(ref buff);
                //obj.F148 = (decimal)BitConverter.ToUInt16(buff, 0);
                //////buff = new byte[2];
                //////Buffer.BlockCopy(GetData, (0x0149 - 0x0100) * 2, buff, 0, 2);
                //////CrossHiLow(ref buff);
                //////obj.F149 = (decimal)BitConverter.ToUInt16(buff, 0);
                //buff = new byte[2];
                //Buffer.BlockCopy(GetData, (0x014A - 0x0100) * 2, buff, 0, 2);
                //CrossHiLow(ref buff);
                //obj.F14A = (decimal)BitConverter.ToUInt16(buff, 0);

                #endregion

                ////buff = new byte[2];
                ////Buffer.BlockCopy(GetData, (0x014B - 0x0100) * 2, buff, 0, 2);
                ////CrossHiLow(ref buff);
                ////obj.F14B = (decimal)BitConverter.ToUInt16(buff, 0);
                #region modified on 2015-06-01
                //buff = new byte[2];
                //Buffer.BlockCopy(GetData, (0x014C - 0x0100) * 2, buff, 0, 2);
                //CrossHiLow(ref buff);
                //obj.F14C = (decimal)BitConverter.ToUInt16(buff, 0);
                #endregion
                //////buff = new byte[2];
                //////Buffer.BlockCopy(GetData, (0x014D - 0x0100) * 2, buff, 0, 2);
                //////CrossHiLow(ref buff);
                //////obj.F14D = (decimal)BitConverter.ToUInt16(buff, 0);
                //////buff = new byte[2];
                //////Buffer.BlockCopy(GetData, (0x014E - 0x0100) * 2, buff, 0, 2);
                //////CrossHiLow(ref buff);
                //////obj.F14E = (decimal)BitConverter.ToUInt16(buff, 0);
                //////#endregion

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

                //buff = new byte[2];
                //Buffer.BlockCopy(GetData, (0x0150 - 0x0100) * 2, buff, 0, 2);
                //CrossHiLow(ref buff);
                //obj.F150 = (decimal)BitConverter.ToUInt16(buff, 0);

                //buff = new byte[2];
                //Buffer.BlockCopy(GetData, (0x0151 - 0x0100) * 2, buff, 0, 2);
                //CrossHiLow(ref buff);
                //obj.HJhrzPCGathF55 = (decimal)BitConverter.ToUInt16(buff, 0);


              
                using (WSExpressEntities db = new WSExpressEntities())
                {
                    db.AddToHJPlcParaControl1(obj);
                    db.SaveChanges();
                }
                #endregion

                EntityManager<HJPlcParaControl1>.Add<HJPlcParaControl1>(obj);
               
                OnProtocalDataReceivedProgress(recdPtr.m_userid, obj);

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
            if (ReadPacketCommand.CommNo != null && !ReadPacketCommand.CommNo.Equals(""))
                return ReadPacketCommand;
            else
                return base.CreateModbusRtuReadCommand(simno, 0x01, 0x03, 0x100, 0x016F + 1 - 0x100);
            //010301000052C5CB
        }

        public override DeviceCmd CreateWeatherDownCommand(string simno, float wd, float rz, float fs)
        {
            return base.CreateModbusRtuWriteCommand(simno, 0x01,0x110,wd, rz, fs);
        }

    }
}


