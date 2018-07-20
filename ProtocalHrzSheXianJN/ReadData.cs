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
using log4net;

namespace QyTech.ProtocalHrzSheXianJN
{
    public class ReadData : IProtocal
    {
        //采集数据
      
        public static DateTime LastUpDate = DateTime.MaxValue;
        private int hrzNo = 0;

        public ReadData(int bsP_Id)
            : base(bsP_Id)
        {

        }


        public override DeviceCmd CreateReadCommand(string simno)
        {
            ////////return base.CreateReadCommand(simno, UseProtocal);
            ////////return base.CreateReadCommand(simno);
            return base.CreateModbusRtuReadCommand(simno, 0x01, 0x03, 0x000, 0x031 - 0x000 + 1);

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
            OTStandDataHrz otobj = new OTStandDataHrz();
            NewStandDataHrz newobj;

            try
            {
                #region 解析hrz数据对象
                int bufflen;
                Type type = otobj.GetType(); //获取类型
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

                        otobj = (OTStandDataHrz)SetValueByReflectionFromBytes<OTStandDataHrz>(otobj, pi, propertyInfo, buff);
                    }
                    catch (Exception ex)
                    {
                        log.Error("ReadData.Parse(" + pi.FieldName + "):" + ex.Message);
                    }
                }

                otobj.GathDt = Convert.ToDateTime(recdPtr.m_recv_date);
                otobj.Det_Id = hrzjzs[hrzNo].Id;
                otobj.bsO_Id = Org.Id;
                //obj.HJGathF2C_2F = Convert.ToDateTime("20" + obj.HJGathF2C.Substring(0, 2) + "-" + obj.HJGathF2C.Substring(2, 2) + "-" + obj.HJGathF2D.Substring(0, 2) + " " + obj.HJGathF2D.Substring(2, 2) + ":" + obj.HJGathF2E.Substring(0, 2) + ":" + obj.HJGathF2E.Substring(2, 2));
                #endregion


                EntityManager<OTStandDataHrz>.Add<OTStandDataHrz>(otobj);


                newobj = EntityManager<NewStandDataHrz>.GetBySql<NewStandDataHrz>("Det_Id='" + otobj.Det_Id.ToString() + "'");

                if (newobj == null)
                {
                    newobj = EntityOperate.Copy<NewStandDataHrz>(otobj);
                    EntityManager<NewStandDataHrz>.Add<NewStandDataHrz>(newobj);
                }
                else
                {
                    EntityOperate.Copy<NewStandDataHrz>(otobj, newobj, "Det_Id");
                    EntityManager<NewStandDataHrz>.Modify<NewStandDataHrz>(newobj);
                }

                OnProtocalDataReceivedProgress(recdPtr.m_userid, otobj);

                #region 后面三个的解码
                buff = new byte[4];
                for (int i = 1; i < hrzjzs.Count; i++)
                {
                    otobj = new OTStandDataHrz();
                    Buffer.BlockCopy(GetData, (0x10 + i * 8) * 2, buff, 0, 4); CrossHiLow(ref buff);
                    otobj.F1 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, (0x12  + i * 8) * 2, buff, 0, 4); CrossHiLow(ref buff);
                    otobj.F2 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, (0x14  + i * 8) * 2, buff, 0, 4); CrossHiLow(ref buff);
                    otobj.F3 = (decimal)BitConverter.ToSingle(buff, 0);
                    otobj.GathDt = Convert.ToDateTime(recdPtr.m_recv_date);
                    otobj.Det_Id = hrzjzs[i].Id;
                    otobj.bsO_Id = Org.Id;


                    EntityManager<OTStandDataHrz>.Add<OTStandDataHrz>(otobj);


                    newobj = EntityManager<NewStandDataHrz>.GetBySql<NewStandDataHrz>("Det_Id='" + otobj.Det_Id.ToString() + "'");

                    if (newobj == null)
                    {
                        newobj = EntityOperate.Copy<NewStandDataHrz>(otobj);
                        EntityManager<NewStandDataHrz>.Add<NewStandDataHrz>(newobj);
                    }
                    else
                    {
                        EntityOperate.Copy<NewStandDataHrz>(otobj, newobj, "Det_Id");
                        EntityManager<NewStandDataHrz>.Modify<NewStandDataHrz>(newobj);
                    }

                    OnProtocalDataReceivedProgress(recdPtr.m_userid, otobj);

                }
                #endregion
            }
            catch (Exception ex)
            {
                log.Error("hrzupunit parse:simno: New Object" + recdPtr.m_userid + ":" + ex.InnerException + "-" + ex.Message);
                resualt = -1;
            }
            finally
            {
                //log.Info("Parse:70");
                otobj = null;
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
