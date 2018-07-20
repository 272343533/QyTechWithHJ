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

namespace QyTech.HjProtocal2
{
    public class Range : IProtocal
    {
        //public clsHJGathData11 obj = new clsHJGathData11();
        public Range(int bsP_Id)
            : base(bsP_Id)
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
                HrzGathRange obj = new HrzGathRange();
                obj.GathDt = Convert.ToDateTime(recdPtr.m_recv_date);
                obj.Det_Id = (Guid)hrzjzs[0].Id;
                obj.bsO_Id = Org.Id;
                try
                {
                    NewHJGathData2 gathobj = EntityManager<NewHJGathData2>.GetByPk<NewHJGathData2>("Det_Id", obj.Det_Id);
                    if ((Convert.ToInt32(gathobj.HJGathF62) & 0x03) == 3)
                    {
                        obj.bsO_Id = Guid.Parse(Org.BllOrgID);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("云存储判断：" + ex.Message);
                }
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

                        obj = (HrzGathRange)SetValueByReflectionFromBytes<HrzGathRange>(obj, pi, propertyInfo, buff);
                    }
                    catch (Exception ex)
                    {
                        log.Error("ReadData.Parse(" + pi.FieldName + "):" + ex.Message);
                    }
                }

                EntityManager<HrzGathRange>.Add<HrzGathRange>(obj);
              
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
            return base.CreateModbusRtuReadCommand(simno, 0x01, 0x03, 0x200, 0x26C + 1 - 0x200);
            //01030200006D859F
        }
        public override DeviceCmd CreateModbusRtuWriteCommandByConf(string simno, object obj)
        {
            HrzRangeAlarmConf tmpobj = (HrzRangeAlarmConf)obj;
            return base.CreateModbusRtuWriteCommand<HrzRangeAlarmConf>(simno, tmpobj, bsProtItems);
        }

        public override DeviceCmd CreateModbusRtuWriteCommandByGath(string simno, object obj)
        {
            HrzGathRange tmpobj = (HrzGathRange)obj;
            return base.CreateModbusRtuWriteCommand<HrzGathRange>(simno, tmpobj, bsProtItems);
        }

        public override DeviceCmd CreateModbusRtuWriteCommandBySelfDefine(string simno)
        {
            Dev = BaseDABll.GetDtuProduct(simno);
            //Org = BaseDABll.GetOrganize((Guid)Dev.AZWZ);
            //HrzGathRange obj = EntityManager<HrzGathRange>.GetByPk<HrzGathRange>("bsO_Id", Guid.Parse(Org.BllOrgID));
            //return base.CreateModbusRtuWriteCommand<HrzGathRange>(simno, obj, bsProtItems);

            List<HrzGathRange> objs = EntityManager<HrzGathRange>.GetAllByStorProcedure<HrzGathRange>("splyGetClouldStorageData '", Dev.AZWZ.ToString() + "',1");
            return base.CreateModbusRtuWriteCommand<HrzGathRange>(simno, objs[0], bsProtItems);
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
        //            List<vwlyHjSetRange> dbobjs = db.vwlyHjSetRange.Where(u => u.Id == orgid).OrderBy(o => o.RangeSetNo).ToList<vwlyHjSetRange>();
        //            //或者是按照协议地址进行存储

        //            for (int i = 0; i < dbobjs.Count; i++)// (AlarmRangeSet ars in dbobjs)
        //            {
        //                vwlyHjSetRange ars = dbobjs[i];
        //                buff = BitConverter.GetBytes((float)ars.RangeUp);
        //                CrossHiLow(ref buff);
        //                byteindex = Convert.ToInt16(ars.RangeSetNo.ToString(), 16) - 0x200;
        //                Buffer.BlockCopy(buff, 0, data, byteindex, 4);
        //            }
        //        }
        //        return data;
        //    }

        //}

    }
}
