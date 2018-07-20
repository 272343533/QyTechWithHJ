﻿using System;
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

namespace QyTech.GlfYuanshixian
{
    public class ReadData: IProtocal
    {
        //采集数据
        //public static bsOrganize ORG=new bsOrganize();
        OTStandDataGlf hcg;

        public ReadData(int bsP_Id)
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
                OTStandDataGlf obj = new OTStandDataGlf();
                obj.GathDt = Convert.ToDateTime(recdPtr.m_recv_date);
                obj.Det_Id = (Guid)hrzjzs[0].Id;
                obj.Det_Name = hrzjzs[0].DeviceName;
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

                        obj = (OTStandDataGlf)SetValueByReflectionFromBytes<OTStandDataGlf>(obj, pi, propertyInfo, buff);


                    }
                    catch (Exception ex)
                    {
                        log.Error(recdPtr.m_userid + " " + base.bsProtocalObj.Id.ToString() + " Parse(" + pi.FieldName + "):" + ex.Message);
                    }
                }

                EntityManager<OTStandDataGlf>.Add<OTStandDataGlf>(obj);

                NewStandDataGlf newobj = EntityManager<NewStandDataGlf>.GetBySql<NewStandDataGlf>("Det_Id='" + obj.Det_Id.ToString() + "'");

                if (newobj == null)
                {
                    newobj = EntityOperate.Copy<NewStandDataGlf>(obj);
                    EntityManager<NewStandDataGlf>.Add<NewStandDataGlf>(newobj);
                }
                else
                {
                    EntityOperate.Copy<NewStandDataGlf>(obj, newobj, "Det_Id");
                    EntityManager<NewStandDataGlf>.Modify<NewStandDataGlf>(newobj);
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

        //public override DeviceCmd CreateReadCommand(string simno)
        //{
        //    if (ReadPacketCommand.CommNo != null && !ReadPacketCommand.CommNo.Equals(""))
        //        return ReadPacketCommand;
        //    else
        //        return base.CreateModbusRtuReadCommand(simno, 0x01, 0x03, 0x00, 0x003D + 1 - 0x00);
        //    //010301000052C5CB
        //}

        //public override DeviceCmd CreateWeatherDownCommand(string simno, float wd, float rz, float fs)
        //{
        //    return base.CreateModbusRtuWriteCommand(simno,0x01,0x001A,wd, rz, fs);
        //}

        public override DeviceCmd CreateModbusRtuWriteCommand(string simno, byte slaveaddr, int address, params object[] args)
        {
            return base.CreateModbusRtuWriteCommand(simno, slaveaddr, address, args);
        }
    }
}


