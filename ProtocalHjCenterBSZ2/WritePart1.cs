using HjCommDA;
using QyTech.Communication;
using QyTech.HDGprs;
using QyTech.Protocal;
using SunMvcExpress.Core.BLL;
using SunMvcExpress.Dao;
using System;
using System.Collections.Generic;

namespace QyTech.HjCenterBSZ2
{
    public class WritePart1 : IProtocal
    {
        //采集数据
        //public static bsOrganize ORG=new bsOrganize();
        protected int DetailDevIndex = 1;
        public WritePart1(int bsP_Id)
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


                OTStandDataHrzControl1 obj = new OTStandDataHrzControl1();
                obj.GathDt = Convert.ToDateTime(recdPtr.m_recv_date);
                obj.Det_Id = (Guid)hrzjzs[DetailDevIndex].Id;
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

                        obj = (OTStandDataHrzControl1)SetValueByReflectionFromBytes<OTStandDataHrzControl1>(obj, pi, propertyInfo, buff);


                    }
                    catch (Exception ex)
                    {
                        log.Error("ReadData.Parse(" + pi.FieldName + "):" + ex.Message);
                    }
                }

                EntityManager<OTStandDataHrzControl1>.Add<OTStandDataHrzControl1>(obj);

              
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
                return base.CreateModbusRtuReadCommand(simno, 0x01, 0x03, 0x30, 0x005F + 1 - 0x30);
            //010301000052C5CB
        }
    }
}


