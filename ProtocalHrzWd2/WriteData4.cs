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

namespace QyTech.ProtocalHrzWd2
{
    public class WriteData4 : IProtocal
    {
        public WriteData4(int bsP_Id)
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
                OTStandHrzControlData obj = new OTStandHrzControlData();
                obj.GathDt = Convert.ToDateTime(recdPtr.m_recv_date);
                obj.Det_Id = (Guid)hrzjzs[0].Id;
                obj.bsO_Id = Org.Id;
                obj.bsP_Id = base.bsProtocalObj.Id;
                try
                {
                    NewStandDataStr gathobj = EntityManager<NewStandDataStr>.GetByPk<NewStandDataStr>("Det_Id", obj.Det_Id);
                    if ((Convert.ToInt32(gathobj.F53) & 0x03) == 3)
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

                        obj = (OTStandHrzControlData)SetValueByReflectionFromBytes<OTStandHrzControlData>(obj, pi, propertyInfo, buff);


                    }
                    catch (Exception ex)
                    {
                        log.Error("ReadData.Parse(" + pi.FieldName + "):" + ex.Message);
                    }
                }




                EntityManager<OTStandHrzControlData>.Add<OTStandHrzControlData>(obj);


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

        public override DeviceCmd CreateModbusRtuWriteCommand(string simno, byte slaveaddr, int address, params object[] args)
        {
            return base.CreateModbusRtuWriteCommand(simno, slaveaddr, address, args);
        }

        public override DeviceCmd CreateModbusRtuWriteCommandByGath(string simno, object obj)
        {
            HJHrzControlData2Part1 tmpobj = (HJHrzControlData2Part1)obj;
            return base.CreateModbusRtuWriteCommand<HJHrzControlData2Part1>(simno, tmpobj, bsProtItems);
        }

        public override DeviceCmd CreateModbusRtuWriteCommandBySelfDefine(string simno)
        {
            Dev = BaseDABll.GetDtuProduct(simno);

            List<HJHrzControlData2Part1> objs = EntityManager<HJHrzControlData2Part1>.GetAllByStorProcedure<HJHrzControlData2Part1>("splyGetClouldStorageData '", Dev.AZWZ.ToString() + "',4");
            return base.CreateModbusRtuWriteCommand<HJHrzControlData2Part1>(simno, objs[0], bsProtItems);
        }


    }
}


