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

namespace QyTech.ProtocalHrzCommon
{
    public class HRZCommonReadDataStr : IProtocal
    {
        protected int DetailDevNo = 0;

        //采集数据
        OTStandDataGlf hcg;
   
        public HRZCommonReadDataStr(int bsP_Id)
            : base(bsP_Id)
        {
        }

        public override int Parse(GPRS_DATA_RECORD recdPtr)
        {
            if (base.Parse(recdPtr) == 1)
                return -1;
            Dev = BaseDABll.GetDtuProduct(recdPtr.m_userid);
            Org = BaseDABll.GetOrganize((Guid)Dev.AZWZ);

            List<DetailDevice> details = BaseDABll.GetDetailDevice(Dev.Id);
            
            int resualt = 1;
            try
            {
                OTStandDataStr obj = new OTStandDataStr();
                obj.GathDt = Convert.ToDateTime(recdPtr.m_recv_date);
                obj.Det_Id = details[DetailDevNo].Id;
                obj.Det_Name = details[DetailDevNo].DeviceName;
                obj.bsO_Id = Org.Id;
                obj.bsO_Name = Org.Name;

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

                        obj = (OTStandDataStr)SetValueByReflectionFromBytes<OTStandDataStr>(obj, pi, propertyInfo, buff);


                    }
                    catch (Exception ex)
                    {
                        log.Error("ReadGGUnit"+DetailDevNo.ToString()+recdPtr.m_userid + " " + base.bsProtocalObj.Id.ToString() + " Parse(" + pi.FieldName + "):" + ex.Message);
                    }
                }

                EntityManager<OTStandDataStr>.Add<OTStandDataStr>(obj);

                NewStandDataStr newobj = EntityManager<NewStandDataStr>.GetBySql<NewStandDataStr>("Det_Id='" + obj.Det_Id.ToString() + "'");

                if (newobj == null)
                {
                    newobj = EntityOperate.Copy<NewStandDataStr>(obj);
                    EntityManager<NewStandDataStr>.Add<NewStandDataStr>(newobj);
                }
                else
                {
                    EntityOperate.Copy<NewStandDataStr>(obj, newobj, "Det_Id");
                    EntityManager<NewStandDataStr>.Modify<NewStandDataStr>(newobj);
                }
                OnProtocalDataReceivedProgress(recdPtr.m_userid, obj);

                obj = null;
            }
            catch (Exception ex)
            {
                log.Error("ReadGGUnit parse:simno:" + recdPtr.m_userid + ":" + ex.InnerException + "-" + ex.Message);
                resualt = -1;
            }
            return resualt;
        }


        public override DeviceCmd CreateWeatherDownCommand(string simno, float wd, float rz, float fs)
        {
            return base.CreateModbusRtuWriteCommand(simno,0x01,0x001A,wd, rz, fs);
        }

        public override DeviceCmd CreateModbusRtuWriteCommand(string simno, byte slaveaddr, int address, params object[] args)
        {
            return base.CreateModbusRtuWriteCommand(simno, slaveaddr, address, args);
        }
    }
}


