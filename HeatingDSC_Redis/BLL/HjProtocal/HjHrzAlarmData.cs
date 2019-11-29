using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunMvcExpress.Dao;
using QyTech.HDGprs;
using QyTech.Protocal;
using QyTech.Communication;
using HeatingDSC.Models;

namespace HeatingDSC.BLL
{
    public class HjHrzAlarmData : IProduct
    {
        //采集数据
        //public static bsOrganize ORG=new bsOrganize();
        HrzCommGath hcg;
        int _orgId = 1;
        public AlarmRangeSet rangset = new AlarmRangeSet();
     
        public HjHrzAlarmData()
        {
            Producttype = ProductType.HJGathData1;
        }

        public override void OnHrzRangeProgress(HrzGathRange hrg)
        {
            base.OnHrzRangeProgress(hrg);
        }


        public override int Parse(GPRS_DATA_RECORD recdPtr)
        {
            base.Parse(recdPtr);
            List<DetailDevice> hrzjzs = devda.SelectHrzJzDeviceBySim(recdPtr.m_userid);
            int resualt = 0;
            try
            {
                #region 解析数据对象
                HrzGathAlarm obj = new HrzGathAlarm();
                obj.GathDt= Convert.ToDateTime(recdPtr.m_recv_date);
                obj.Det_Id = (Guid)hrzjzs[0].Id;
                obj.bsO_Id = Org.Id;
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0280-0x0280) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F1Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0282-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F1Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0284-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F2Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0286-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F2Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0288-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F3Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x028A-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F3Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x028C-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F4Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x028E-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F4own = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0290-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F5Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0292-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F5Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0294-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F6Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0296-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F6Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0298-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F7Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x029A-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F7Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x029C-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F8Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x029E-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F8Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x02A0-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F9Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x02A2-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F9Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x02A4-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F10Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x02A6-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F10Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x02A8-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F11Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x02AA-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F11Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x02AC-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F12Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x02AE-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F12Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x02B0-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F13Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x02B2-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F13Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x02B4-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F14Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x02B6-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F14Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x02B8-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F15Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x02BA-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F15Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x02BC-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F16Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x02BE-0x0280)*2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F16Down = (decimal)BitConverter.ToSingle(buff, 0);
                try
                {
                    buff = new byte[4];
                    Buffer.BlockCopy(GetData, (0x02C0 - 0x0280) * 2, buff, 0, 4);
                    CrossHiLow(ref buff);
                    obj.F17Up = (decimal)BitConverter.ToSingle(buff, 0);
                    buff = new byte[4];
                    Buffer.BlockCopy(GetData, (0x02C2 - 0x0280) * 2, buff, 0, 4);
                    CrossHiLow(ref buff);
                    obj.F17Down = (decimal)BitConverter.ToSingle(buff, 0);
                    buff = new byte[4];
                    Buffer.BlockCopy(GetData, (0x02C4 - 0x0280) * 2, buff, 0, 4);
                    CrossHiLow(ref buff);
                    obj.F18Up = (decimal)BitConverter.ToSingle(buff, 0);
                    buff = new byte[4];
                    Buffer.BlockCopy(GetData, (0x02C6 - 0x0280) * 2, buff, 0, 4);
                    CrossHiLow(ref buff);
                    obj.F18Down = (decimal)BitConverter.ToSingle(buff, 0);
                }
                catch { }

                using (WSExpressEntities db = new WSExpressEntities())
                {
                    db.AddToHrzGathAlarm(obj);
                    db.SaveChanges();
                }
               
                #endregion
                //OnHrzAlarmRangeGathProgress(obj);
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
            return base.CreateModbusRtuReadCommand(simno, 0x01, 0x03, 0x280, 0x2BE - 0x280 + 1 + 1);
            //return base.CreateModbusRtuReadCommand(simno, 0x01, 0x03, 0x280, 0x2C6 - 0x280 + 1 + 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objs">为下发的换热站orgid</param>
        /// <returns></returns>
        public override DeviceCmd CreateWriteCommand(object objs)
        {
            return base.CreateWriteCommand(objs);
        }
        public byte[] Create(Guid orgid)
        {
            DeviceCmd cmd2send = new DeviceCmd();
           
            int len = (0x2BE - 0x280 + 1) * 2;
            byte[] data = new byte[len];
            int byteindex = 0;
            byte[] buff = new byte[4];

           
            using (var db = new WSExpressEntities())
            {
                vwlyGprsDevice simobj=db.vwlyGprsDevice.Where(u=>u.bsO_Id==orgid).FirstOrDefault<vwlyGprsDevice>();
                if (simobj != null)
                {
                    cmd2send.CommNo = simobj.CommNo;
                    List<vwlyHjSetAlarm> dbobjs = db.vwlyHjSetAlarm.Where(u => u.bsO_Id == orgid).OrderBy(o => o.RangeSetNo).ToList<vwlyHjSetAlarm>();
                    #region 都是先下后上
                    //0x0280一次网供水温度
                    //0x0284一次网回水温度报警上限 
                    //288一次网供水压力 
                    //28C一次网回水压力
                    //290一次网瞬时流量
                    //294二次网供水温度
                    //298二次网回水温度
                    //29C二次网供水压力
                    //2A0二次网回水压力
                    //2A4二次网瞬时流量
                    //2A8  1#循环泵工作电流
                    //2AC 2#循环泵工作电流
                    //2B0 1#补水泵工作电流
                    //2B4 2#补水泵工作电流
                    //2B8 补水泵出口压力
                    //2BC 循环泵出口压
                    #endregion
                    //或者是按照协议地址进行存储，就是担心有些项没有数据

                    for (int i = 0; i < dbobjs.Count; i++)// (AlarmRangeSet ars in dbobjs)
                    {
                        vwlyHjSetAlarm ars = dbobjs[i];

                        //buff = BitConverter.GetBytes((float)ars.RangeUpAlarm1);
                        //CrossHiLow(ref buff);
                        //Buffer.BlockCopy(buff, 0, data, byteindex += 4, 4);

                        //buff = BitConverter.GetBytes((float)ars.RangeDownAlarm1);
                        //CrossHiLow(ref buff);
                        //Buffer.BlockCopy(buff, 0, data, byteindex += 4, 4);


                        buff = BitConverter.GetBytes((float)ars.RangeUp);
                        CrossHiLow(ref buff);
                        byteindex = Convert.ToInt16(ars.RangeSetNo.ToString(), 16) - 0x280;
                        Buffer.BlockCopy(buff, 0, data, byteindex, 4);
                    }
                }
                return data;
            }

        }


    }
}



