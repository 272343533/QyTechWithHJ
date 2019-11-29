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

namespace QyTech.ProtocalHrzCommon_DetNo
{
    public class HRZCommonAlarm : IProtocal
    {

        public HRZCommonAlarm(int bsP_Id) : base(bsP_Id)
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

                        obj = (HrzGathAlarm)SetValueByReflectionFromBytes<HrzGathAlarm>(obj, pi, propertyInfo, buff);
                    }
                    catch (Exception ex)
                    {
                        log.Error("ReadData.Parse(" + pi.FieldName + "):" + ex.Message);
                    }
                }

                using (WSExpressEntities db = new WSExpressEntities())
                {
                    db.AddToHrzGathAlarm(obj);
                    db.SaveChanges();
                }
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


  

        public override DeviceCmd CreateModbusRtuWriteCommandByConf(string simno, Object obj)
        {
            HrzRangeAlarmConf tmpobj = (HrzRangeAlarmConf)obj;
            return base.CreateModbusRtuWriteCommand<HrzRangeAlarmConf>(simno, tmpobj, bsProtItems);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objs">为下发的换热站orgid</param>
        /// <returns></returns>
        public override DeviceCmd CreateModbusRtuWriteCommand(string simno, byte slaveaddr, int address, params object[] args)
        {
            object[] willargs = new object[args.Length - 2];
            for (int i = 0; i < willargs.Length; i++)
            {
                willargs[i] = args[i];
            }
            return base.CreateModbusRtuWriteCommand(simno, slaveaddr, address, willargs);
        }


    }
}



