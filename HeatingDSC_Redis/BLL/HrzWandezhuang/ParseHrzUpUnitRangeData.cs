using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunMvcExpress.Dao;
using QyTech.Communication;
using HeatingDSC.Models;
using QyTech.HDGprs;
using QyTech.Protocal;

namespace HeatingDSC.BLL
{
     public class ParseHrzUpUnitRangeData : IProduct
    {
        //采集数据
        //public static bsOrganize ORG=new bsOrganize();
        HrzCommGath hcg;
        public ParseHrzUpUnitRangeData()
        {
            Producttype = ProductType.HrzUpCommData;
        }

        public override void OnHrzRangeProgress(HrzGathRange hrg)
        {
            base.OnHrzRangeProgress(hrg);
        }


        public override int Parse(GPRS_DATA_RECORD recdPtr)
        {
            int resualt = 0;
            try
            {
                HrzGathRange hcg = new HrzGathRange();
                //ORG = Org;
                base.Parse(recdPtr);
                hcg.GathDt = Convert.ToDateTime(recdPtr.m_recv_date);
                hcg.bsO_Id = base.Org.Id;

                List<DetailDevice> hrzjzs = devda.SelectHrzJzDeviceBySim(recdPtr.m_userid);

                hcg.Det_Id = hrzjzs[frmStart.slRangeSetSimnoAndUni[recdPtr.m_userid] - 1].Id ;


                byte[] buff = new byte[4];
                Buffer.BlockCopy(GetData, 0, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F1Up = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x2 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F1Down = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x4 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F2Up = BitConverter.ToInt16(buff, 0);
                Buffer.BlockCopy(GetData, 0x6 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F2Down = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x8 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F3Up = BitConverter.ToInt16(buff, 0);
                Buffer.BlockCopy(GetData, 0xA * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F3Down = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0xC * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F4Up = BitConverter.ToInt16(buff, 0);
                Buffer.BlockCopy(GetData, 0xE * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F4own = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x10 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F5Up = BitConverter.ToInt16(buff, 0);
                Buffer.BlockCopy(GetData, 0x12 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F5Down = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x14 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F6Up = BitConverter.ToInt16(buff, 0);
                Buffer.BlockCopy(GetData, 0x16 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F6Down = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x18, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F7Up = BitConverter.ToInt16(buff, 0);
                Buffer.BlockCopy(GetData, 0x1A * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F7Down = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x1C * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F8Up = BitConverter.ToInt16(buff, 0);
                Buffer.BlockCopy(GetData, 0x1E * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F8Down = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x20 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F9Up = BitConverter.ToInt16(buff, 0);
                Buffer.BlockCopy(GetData, 0x22 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F9Down = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x24 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F10Up = BitConverter.ToInt16(buff, 0);
                Buffer.BlockCopy(GetData, 0x26 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F10Down = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x28 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F11Up = BitConverter.ToInt16(buff, 0);
                Buffer.BlockCopy(GetData, 0x2A * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F11Down = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x2C * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F12Up = BitConverter.ToInt16(buff, 0);
                Buffer.BlockCopy(GetData, 0x2E * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F12Down = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x30 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F13Up = BitConverter.ToInt16(buff, 0);
                Buffer.BlockCopy(GetData, 0x32 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F13Down = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x34 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F14Up = BitConverter.ToInt16(buff, 0);
                Buffer.BlockCopy(GetData, 0x36 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F14Down = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x38 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F15Up = BitConverter.ToInt16(buff, 0);
                Buffer.BlockCopy(GetData, 0x3A * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F15Down = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x3C * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F16Up = BitConverter.ToInt16(buff, 0);
                Buffer.BlockCopy(GetData, 0x3E * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F16Down = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x40 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F17Up = BitConverter.ToInt16(buff, 0);
                Buffer.BlockCopy(GetData, 0x42 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F17Down = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x44 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F18Up = BitConverter.ToInt16(buff, 0);
                Buffer.BlockCopy(GetData, 0x46 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F18Down = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x48 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F19Up = BitConverter.ToInt16(buff, 0);
                Buffer.BlockCopy(GetData, 0x4A * 2, buff, 0, 4); //CrossHiLow(ref buff);
                hcg.F19Down = (decimal)BitConverter.ToSingle(buff, 0);


                //写入数据库
                try
                {
                    log.Info("frmstart save range date is:date" + hcg.GathDt.ToString("yyyy-MM-dd HH:mm:ss.fff") + "-" + hcg.bsO_Id.ToString());
                    hrzda.SaveHrzRangeGath(hcg);
                }
                catch (Exception ex)
                {
                    log.Error("保存HrzRange数据" + ex.InnerException + "-" + ex.Message);
                }


                //if (hcg.TempScale != 0)//CurvePoints[0, 0] != 0 && 
                //{
                    log.Info("parse:date" + recdPtr.m_recv_date + "-" + recdPtr.m_userid + "-" + Org.Id.ToString() + " is " + Org.Name);
                    OnHrzRangeProgress(hcg);
                //}
                //else
                //{
                //    log.Error(" Wrror parse hrzCommGath:date" + recdPtr.m_recv_date + "-" + recdPtr.m_userid + "-" + Org.Id.ToString() + " is " + Org.Name);
                //}

            }
            catch (Exception ex)
            {
                log.Error("hrzupcommdatparse:" + ex.InnerException + "-" + ex.Message);
                return 0;
            }
            return resualt;
        }



         /// <summary>
         /// 
         /// </summary>
         /// <param name="orgid"></param>
         /// <param name="jzno">机组号，从1开始</param>
         /// <returns></returns>
        public DeviceCmd CreateCommand(Guid orgid, int jzno)
        {
            byte[] commdarr = new byte[9];
            DeviceCmd cmd2send=new DeviceCmd();
          
            try
            {
                cmd2send = new DeviceCmd();
                cmd2send.CommNo = hrzda.SelectHrzSimnoById(orgid).CommNo;

                ModbusCommand.VerifyMode = ModbusCommand.RTU;
                ModbusCommand.Slaveaddr = 0x01;
                ModbusCommand.OperatMode = 0x03;
                ModbusCommand.RegStartAddr = Convert.ToInt32("300", 16) + (jzno - 1) * Convert.ToInt32("80", 16);//首地址16进制
                ModbusCommand.RegOpNum = Convert.ToInt32("4C", 16);//, 16);// Convert.ToInt32(sets[2], 16);//字节数量10进制

                commdarr = ModbusCommand.Command();

                //生成命令数组
                if (commdarr != null)
                {
                    cmd2send.SendCmd = commdarr;
                    StringBuilder strbuilder = new StringBuilder();
                    foreach (byte cmdbyte in commdarr)
                    {
                        strbuilder.Append(cmdbyte.ToString("X2"));
                    }
                    cmd2send.Command = strbuilder.ToString();
                }

            }
            catch (Exception ex)
            {
                log.Error("buildcommandsforgather:" + jzno + "-" + ex.Message);
            }

            return cmd2send;
        }
    }
}



