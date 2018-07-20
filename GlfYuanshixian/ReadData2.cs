using System;
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

namespace QyTech.GlfYuanshixian
{
    public class ReadData2 : IProtocal
    {
          public OnTimeHRZCollectData hisHrzGatherdata = new OnTimeHRZCollectData();


        public static DateTime LastUpDate = DateTime.MaxValue;
        private int hrzNo =2;

        public ReadData2(int bsP_Id)
            : base(bsP_Id)
        {
        }

        //public override DeviceCmd CreateReadCommand(string simno)
        //{
        //    //return base.CreateReadCommand(simno, UseProtocal);
        //    //return base.CreateReadCommand(simno);

        //    return base.CreateModbusRtuReadCommand(simno, 0x01, 0x03, 0x70, 0x9C - 0x70 + 1);
   
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
            OTStandDataGldy obj = new OTStandDataGldy();

            try
            {
                #region 解析hrz数据对象
                int bufflen;
                Type type = obj.GetType(); //获取类型
                int InitAddr = Convert.ToInt32(base.bsProtocalObj.FromAddr.Substring(2), 16);
                int ItemAddr;//

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

                        obj = (OTStandDataGldy)SetValueByReflectionFromBytes<OTStandDataGldy>(obj, pi, propertyInfo, buff);
                    }
                    catch (Exception ex)
                    {
                        log.Error(recdPtr.m_userid + " " + base.bsProtocalObj.Id.ToString() + " Parse(" + pi.FieldName + "):" + ex.Message);
                    }
                }

                obj.GathDt = Convert.ToDateTime(recdPtr.m_recv_date);
                obj.Det_Id = hrzjzs[hrzNo].Id;
                obj.Det_Name = hrzjzs[hrzNo].DeviceName;
                obj.bsO_Id = Org.Id;
                obj.bsO_Name = Org.Name;
                //obj.HJGathF2C_2F = Convert.ToDateTime("20" + obj.HJGathF2C.Substring(0, 2) + "-" + obj.HJGathF2C.Substring(2, 2) + "-" + obj.HJGathF2D.Substring(0, 2) + " " + obj.HJGathF2D.Substring(2, 2) + ":" + obj.HJGathF2E.Substring(0, 2) + ":" + obj.HJGathF2E.Substring(2, 2));
                #endregion


                EntityManager<OTStandDataGldy>.Add<OTStandDataGldy>(obj);


                NewStandDataGldy newobj = EntityManager<NewStandDataGldy>.GetBySql<NewStandDataGldy>("Det_Id='" + obj.Det_Id.ToString() + "'");
                if (newobj == null)
                {
                    newobj = EntityOperate.Copy<NewStandDataGldy>(obj);
                    EntityManager<NewStandDataGldy>.Add<NewStandDataGldy>(newobj);
                }
                else
                {
                    EntityOperate.Copy<NewStandDataGldy>(obj, newobj, "Det_Id");

                    EntityManager<NewStandDataGldy>.Modify<NewStandDataGldy>(newobj);
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
