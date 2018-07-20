﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QyTech.Core.BLL;
using SunMvcExpress.Core.BLL;
using System.Reflection;
using SunMvcExpress.Dao;
using QyTech.Communication;
using QyTech.Protocal.Modbus;
using QyTech.HDGprs;
using QyTech.Protocal;
using HjCommDA;
using QyTech.Core.BLL;
using log4net;

namespace QyTech.HrzCity6th
{
    public class ReadData : IProtocal
    {
        //采集数据
        //public Hrz1stNetOpen hisHrz1stOpen = new Hrz1stNetOpen();
        //public Hrz1stNetOpen newHrz1stOpen=new Hrz1stNetOpen();

        //public LastHRZCollectData net1data = new LastHRZCollectData();
        //public List<LastHRZCollectData> net2datas=new List<LastHRZCollectData>();
        public OnTimeHRZCollectData hisHrzGatherdata = new OnTimeHRZCollectData();


        public static DateTime LastUpDate = DateTime.MaxValue;

   
        public ReadData(int bsP_Id)
            : base(bsP_Id)
        {
        }

        //public override DeviceCmd CreateReadCommand(string simno)
        //{
        //    if (ReadPacketCommand != null && ReadPacketCommand.CommNo != null && !ReadPacketCommand.CommNo.Equals(""))
        //    {
        //        return ReadPacketCommand;
        //    }
        //    else
        //        return base.CreateModbusRtuReadCommand(simno, 0x01, 0x03, 0x0, 0x46 - 0x0 + 1);
        //    //01030000006585E1
        //}

        public override int Parse(GPRS_DATA_RECORD recdPtr)
        {
            if (base.Parse(recdPtr) == 1)
                return -1;
            //log.Info("Parse:10");
            Dev = BaseDABll.GetDtuProduct(recdPtr.m_userid);
            Org = BaseDABll.GetOrganize((Guid)Dev.AZWZ);

            List<DetailDevice> hrzjzs = BaseDABll.GetDetailDevice(Dev.Id);
         

            int resualt = 1;
            OTStandDataHrz obj = new OTStandDataHrz();

            try
            {
                #region 解析hrz数据对象
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

                        obj = (OTStandDataHrz)SetValueByReflectionFromBytes<OTStandDataHrz>(obj, pi, propertyInfo, buff);
                    }
                    catch (Exception ex)
                    {
                        log.Error("ReadData.Parse(" + pi.FieldName + "):" + ex.Message);
                    }
                }

                obj.GathDt = Convert.ToDateTime(recdPtr.m_recv_date);
                obj.Det_Id = hrzjzs[0].Id;
                obj.bsO_Id = Org.Id;
                obj.Det_Name = hrzjzs[0].DeviceName;
                obj.bsO_Name = Org.Name;
                //obj.HJGathF2C_2F = Convert.ToDateTime("20" + obj.HJGathF2C.Substring(0, 2) + "-" + obj.HJGathF2C.Substring(2, 2) + "-" + obj.HJGathF2D.Substring(0, 2) + " " + obj.HJGathF2D.Substring(2, 2) + ":" + obj.HJGathF2E.Substring(0, 2) + ":" + obj.HJGathF2E.Substring(2, 2));
                #endregion


                EntityManager<OTStandDataHrz>.Add<OTStandDataHrz>(obj);

                
                NewStandDataHrz newobj = EntityManager<NewStandDataHrz>.GetBySql<NewStandDataHrz>("Det_Id='" + obj.Det_Id.ToString() + "'");
                if (newobj == null)
                {
                    newobj = EntityOperate.Copy<NewStandDataHrz>(obj);
                    EntityManager<NewStandDataHrz>.Add<NewStandDataHrz>(newobj);
                }
                else
                {
                    //newobj = EntityOperate.Copy<NewStandDataHrz>(obj);
                    EntityOperate.Copy<NewStandDataHrz>(obj, newobj, "Det_Id");
                    EntityManager<NewStandDataHrz>.Modify<NewStandDataHrz>(newobj);
                }

                OnProtocalDataReceivedProgress(recdPtr.m_userid, obj);



            }
            catch (Exception ex)
            {
                log.Error("hrzupunit parse:simno: New Object" + recdPtr.m_userid + ":" + ex.InnerException + "-" + ex.Message);
                resualt = -1;
            }
            finally
            {
                //log.Info("Parse:70");
                obj = null;
                //hisobj = null;
                hrzjzs.Clear();
                hrzjzs = null;
            }

            return resualt;
        }

    }
}
