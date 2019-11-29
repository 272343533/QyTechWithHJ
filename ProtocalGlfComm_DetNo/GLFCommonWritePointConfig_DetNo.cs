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

namespace QyTech.ProtocalGlfCommon_DetNo
{
    public class GLFCommonWritePointConfig_DetNo : IProtocal
    {
        protected int DetailDevIndex = -1;
        protected int DetailDevNo = 0;
        protected HrzPointConfig obj;

        //采集数据
        public GLFCommonWritePointConfig_DetNo(int bsP_Id)
            : base(bsP_Id)
        {
        }

        public override int Parse(GPRS_DATA_RECORD recdPtr)
        {
            if (base.Parse(recdPtr) == 1)
                return -1;

            Dev = BaseDABll.GetDtuProduct(recdPtr.m_userid);
            Org = BaseDABll.GetOrganize((Guid)Dev.AZWZ);

            List<DetailDevice> detailDevs = BaseDABll.GetDetailDevice(Dev.Id);
            DetailDevIndex = GLFCommonProtocalFac.GetIndexByDevNo(detailDevs, DetailDevNo.ToString());
            if (DetailDevIndex == -1)
                return -2;

            int resualt = 1;
            try
            {
                obj= new HrzPointConfig();
                obj.GathDt = Convert.ToDateTime(recdPtr.m_recv_date);
                obj.Det_Id = (Guid)detailDevs[DetailDevNo].Id;
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

                        obj = (HrzPointConfig)SetValueByReflectionFromBytes<HrzPointConfig>(obj, pi, propertyInfo, buff);


                    }
                    catch (Exception ex)
                    {
                        log.Error("ReadLUnit.Parse(" + pi.FieldName + "):" + ex.Message);
                    }
                }

                EntityManager<HrzPointConfig>.Add<HrzPointConfig>(obj);
                OnProtocalDataReceivedProgress(recdPtr.m_userid, obj);

                //是否需要作为集中存储,考虑在子类中处理，思路是可以在子类中访问这个obj对象
                //NewHJGathData2 Hj2Read = EntityManager<NewHJGathData2>.GetBySql<NewHJGathData2>("bsO_Id='" + obj.bsO_Id.ToString() + "'");
                //if ((Convert.ToUInt16(Hj2Read.HJGathF62) & 0x02) == 0x02)
                //{

                //    HJHrzControlData2Part1Cloud cloudobj = new HJHrzControlData2Part1Cloud();
                //    EntityOperate.Copy<HJHrzControlData2Part1Cloud>(obj, cloudobj, "Det_Id");

                //    EntityManager<HJHrzControlData2Part1Cloud>.Add<HJHrzControlData2Part1Cloud>(cloudobj);
                //}
                obj = null;
            }
            catch (Exception ex)
            {
                log.Error("hrzupunit parse:simno:" + recdPtr.m_userid + ":" + ex.InnerException + "-" + ex.Message);
                resualt = -1;
            }
            return resualt;
        }

    }
}


