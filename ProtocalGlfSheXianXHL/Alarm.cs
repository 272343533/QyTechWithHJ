using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using SunMvcExpress.Dao;
using QyTech.Communication;
using QyTech.Protocal.Modbus;
using QyTech.HDGprs;
using QyTech.Protocal;
using HjCommDA;
using SunMvcExpress.Core.BLL;
using log4net;

namespace QyTech.ProtocalHrzSheXianXHL
{
    public class Alarm : IProtocal
    {
     
        public Alarm(int bsP_Id):base(bsP_Id)
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
                HrzGathAlarm obj = new HrzGathAlarm();
                obj.GathDt = Convert.ToDateTime(recdPtr.m_recv_date);
                obj.Det_Id = (Guid)hrzjzs[0].Id;
                obj.bsO_Id = Org.Id;

                int bufflen;
                Type type = obj.GetType(); //获取类型
                int InitAddr = Convert.ToInt32(bsProtocalObj.FromAddr.Substring(2), 16);
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

                        obj = (HrzGathAlarm)SetValueByReflectionFromBytes<HrzGathAlarm>(obj, pi, propertyInfo, buff);
                    }
                    catch (Exception ex)
                    {
                        log.Error("ReadData.Parse(" + pi.FieldName + "):" + ex.Message);
                    }
                }

                EntityManager<HrzGathAlarm>.Add<HrzGathAlarm>(obj);
               
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
            return base.CreateModbusRtuReadCommand(simno, 0x01, 0x03, 0x280, 0x2C8 - 0x280 + 1);
            //010302800049846C

        }

        public override DeviceCmd CreateModbusRtuWriteCommandByConf(string simno, object obj)
        {
            HrzRangeAlarmConf tmpobj = (HrzRangeAlarmConf)obj;
            return base.CreateModbusRtuWriteCommand<HrzRangeAlarmConf>(simno, tmpobj, bsProtItems);
        }

        public override DeviceCmd CreateModbusRtuWriteCommandByGath(string simno, object obj)
        {
            HrzGathAlarm tmpobj = (HrzGathAlarm)obj;
            return base.CreateModbusRtuWriteCommand<HrzGathAlarm>(simno, tmpobj, bsProtItems);
        }

        public override DeviceCmd CreateModbusRtuWriteCommandBySelfDefine(string simno)
        {
            Dev = BaseDABll.GetDtuProduct(simno);
            Org = BaseDABll.GetOrganize((Guid)Dev.AZWZ);
            HrzGathAlarm obj = EntityManager<HrzGathAlarm>.GetByPk<HrzGathAlarm>("bsO_Id", Org.PId);

            return base.CreateModbusRtuWriteCommand<HrzGathAlarm>(simno, obj, bsProtItems);
            // return new DeviceCmd();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objs">为下发的换热站orgid</param>
        /// <returns></returns>
        public override DeviceCmd CreateModbusRtuWriteCommand(string simno, byte slaveaddr, int address, params object[] args)
        {
            return base.CreateModbusRtuWriteCommand(simno, slaveaddr, address, args);
        }



        //public byte[] Create(Guid orgid)
        //{
        //    DeviceCmd cmd2send = new DeviceCmd();

        //    int len = (0x2BE - 0x280 + 1) * 2;
        //    byte[] data = new byte[len];
        //    int byteindex = 0;
        //    byte[] buff = new byte[4];


        //    using (var db = new WSExpressEntities())
        //    {
        //        vwlyGprsDevice simobj = db.vwlyGprsDevice.Where(u => u.bsO_Id == orgid).FirstOrDefault<vwlyGprsDevice>();
        //        if (simobj != null)
        //        {
        //            cmd2send.CommNo = simobj.CommNo;
        //            List<vwlyHjSetAlarm> dbobjs = db.vwlyHjSetAlarm.Where(u => u.bsO_Id == orgid).OrderBy(o => o.RangeSetNo).ToList<vwlyHjSetAlarm>();
        //            #region 都是先下后上
        //            //0x0280一次网供水温度
        //            //0x0284一次网回水温度报警上限 
        //            //288一次网供水压力 
        //            //28C一次网回水压力
        //            //290一次网瞬时流量
        //            //294二次网供水温度
        //            //298二次网回水温度
        //            //29C二次网供水压力
        //            //2A0二次网回水压力
        //            //2A4二次网瞬时流量
        //            //2A8  1#循环泵工作电流
        //            //2AC 2#循环泵工作电流
        //            //2B0 1#补水泵工作电流
        //            //2B4 2#补水泵工作电流
        //            //2B8 补水泵出口压力
        //            //2BC 循环泵出口压
        //            #endregion
        //            //或者是按照协议地址进行存储，就是担心有些项没有数据

        //            for (int i = 0; i < dbobjs.Count; i++)// (AlarmRangeSet ars in dbobjs)
        //            {
        //                vwlyHjSetAlarm ars = dbobjs[i];

        //                //buff = BitConverter.GetBytes((float)ars.RangeUpAlarm1);
        //                //CrossHiLow(ref buff);
        //                //Buffer.BlockCopy(buff, 0, data, byteindex += 4, 4);

        //                //buff = BitConverter.GetBytes((float)ars.RangeDownAlarm1);
        //                //CrossHiLow(ref buff);
        //                //Buffer.BlockCopy(buff, 0, data, byteindex += 4, 4);


        //                buff = BitConverter.GetBytes((float)ars.RangeUp);
        //                CrossHiLow(ref buff);
        //                byteindex = Convert.ToInt16(ars.RangeSetNo.ToString(), 16) - 0x280;
        //                Buffer.BlockCopy(buff, 0, data, byteindex, 4);
        //            }
        //        }
        //        return data;
        //    }

        //}


    }
}



