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

namespace QyTech.GlfSheXianHospital
{
    public class WritePart3 : IProtocal
    {
        //采集数据
        //public static bsOrganize ORG=new bsOrganize();
        HrzCommGath hcg;

        public WritePart3(int bsP_Id)
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
                OTStandDataHrzControl2 obj = new OTStandDataHrzControl2();
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

                        obj = (OTStandDataHrzControl2)SetValueByReflectionFromBytes<OTStandDataHrzControl2>(obj, pi, propertyInfo, buff);


                    }
                    catch (Exception ex)
                    {
                        log.Error("ReadData.Parse(" + pi.FieldName + "):" + ex.Message);
                    }
                }

                //try
                //{
                //    obj.F11C_11F = Convert.ToDateTime("20" + obj.F11C.Substring(0, 2) + "-" + obj.F11C.Substring(2, 2) +
                //        "-" + obj.F11D.Substring(0, 2) + " " + obj.F11D.Substring(2, 2) + ":" + obj.F11E.Substring(0, 2) +
                //        ":" + obj.F11E.Substring(2, 2));

                //}
                //catch { }


                EntityManager<OTStandDataHrzControl2>.Add<OTStandDataHrzControl2>(obj);

                ////是否需要作为集中存储
                //NewHJGathData2 Hj2Read = EntityManager<NewHJGathData2>.GetBySql<NewHJGathData2>("bsO_Id='" + obj.bsO_Id.ToString() + "'");
                //if ((Convert.ToUInt16(Hj2Read.HJGathF62) & 0x02) == 0x02)
                //{
                //    HJHrzControlData2Part1Cloud cloudobj = new HJHrzControlData2Part1Cloud();
                //    EntityOperate.Copy<HJHrzControlData2Part1Cloud>(obj, cloudobj, "Det_Id");

                //    EntityManager<HJHrzControlData2Part1Cloud>.Add<HJHrzControlData2Part1Cloud>(cloudobj);
                //}

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
                return base.CreateModbusRtuReadCommand(simno, 0x01, 0x03, 0x01F0, 0x023F + 1 - 0x01F0);
            //010301000052C5CB
        }

        public override DeviceCmd CreateWeatherDownCommand(string simno, float wd, float rz, float fs)
        {
            return base.CreateModbusRtuWriteCommand(simno, 0x01, 0x0200, wd);
        }

        

    }
}


