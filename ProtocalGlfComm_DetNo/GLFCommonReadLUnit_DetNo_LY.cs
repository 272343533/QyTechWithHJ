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

namespace QyTech.ProtocalGlfCommon_DetNo
{
    public class GLFCommonReadLUnit_DetNo_LY : IProtocal
    {
        protected int DetailDevIndex = -1;
        protected int DetailDevNo = 0;
        //采集数据

        public static DateTime LastUpDate = DateTime.MaxValue;
       
        public GLFCommonReadLUnit_DetNo_LY(int bsP_Id):base(bsP_Id)
        {
         
        }
        
        public override int Parse(GPRS_DATA_RECORD recdPtr)
        {
            if (base.Parse(recdPtr) == 1)
                return -1;
            //log.Info("Parse:10");
            Dev = BaseDABll.GetDtuProduct(recdPtr.m_userid);
            Org = BaseDABll.GetOrganize((Guid)Dev.AZWZ);

            List<DetailDevice> detailDevs = BaseDABll.GetDetailDevice(Dev.Id);
            DetailDevIndex = GLFCommonProtocalFac.GetIndexByDevNo(detailDevs, DetailDevNo.ToString());
            if (DetailDevIndex == -1)
                return -2;


            int resualt = 1;
            OTStandDataGldy_LY obj = new OTStandDataGldy_LY();

            try
            {
                #region 解析hrz数据对象
                int bufflen;
                Type type = obj.GetType(); //获取类型
                int InitAddr = Convert.ToInt32(base.bsProtocalObj.FromAddr.Substring(2), 16);
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

                        obj = (OTStandDataGldy_LY)SetValueByReflectionFromBytes<OTStandDataGldy_LY>(obj, pi, propertyInfo, buff);
                    }
                    catch (Exception ex)
                    {
                        log.Error(recdPtr.m_userid + " " + base.bsProtocalObj.Id.ToString() + " Parse(" + pi.FieldName + "):" + ex.Message);
                    }
                }

                obj.GathDt = Convert.ToDateTime(recdPtr.m_recv_date);
                obj.Det_Id = detailDevs[DetailDevIndex].Id;
                obj.Det_Name = detailDevs[DetailDevIndex].DeviceName;
                obj.bsO_Id = Org.Id;
                obj.bsO_Name = Org.Name;
                //obj.HJGathF2C_2F = Convert.ToDateTime("20" + obj.HJGathF2C.Substring(0, 2) + "-" + obj.HJGathF2C.Substring(2, 2) + "-" + obj.HJGathF2D.Substring(0, 2) + " " + obj.HJGathF2D.Substring(2, 2) + ":" + obj.HJGathF2E.Substring(0, 2) + ":" + obj.HJGathF2E.Substring(2, 2));
                #endregion


                EntityManager<OTStandDataGldy_LY>.Add<OTStandDataGldy_LY>(obj);


                //NewStandDataGldy newobj = EntityManager<NewStandDataGldy>.GetBySql<NewStandDataGldy>("Det_Id='" + obj.Det_Id.ToString() + "'");
               
                //if (newobj == null)
                //{
                //    newobj = EntityOperate.Copy<NewStandDataGldy>(obj);
                //    EntityManager<NewStandDataGldy>.Add<NewStandDataGldy>(newobj);
                //}
                //else
                //{
                //    EntityOperate.Copy<NewStandDataGldy>(obj, newobj, "Det_Id");
                //    EntityManager<NewStandDataGldy>.Modify<NewStandDataGldy>(newobj);
                //}

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
                detailDevs.Clear();
                detailDevs = null;
            }

            return resualt;
        }



        /// <summary>
        /// 无效，该协议部分不可写,有效，在云处理中使用
        /// </summary>
        /// <param name="simno"></param>
        /// <param name="writeflag"></param>
        /// <returns></returns>
        public override DeviceCmd CreateModbusRtuWriteCommand(string simno, byte slaveaddr, int address, params object[] args)
        {
            return base.CreateModbusRtuWriteCommand(simno, slaveaddr, address, args);
        }


    }
}
