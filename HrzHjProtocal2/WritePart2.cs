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
using SunMvcExpress.Core.BLL;
using HjCommDA;

namespace QyTech.HjProtocal2
{
    public class WritePart2 : IProtocal
    {
        //采集数据
        //public static bsOrganize ORG=new bsOrganize();
        HrzCommGath hcg;

        public WritePart2(int bsP_Id)
            : base(bsP_Id)
        {
        }

        //public override void OnHrzCommProgress(HrzCommGath hcg, int[,] CurvePoints)
        //{
        //    base.OnHrzCommProgress(hcg, CurvePoints);
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
            try
            {
                HJHrzControlData2Part2 obj = new HJHrzControlData2Part2();
                obj.GathDt = Convert.ToDateTime(recdPtr.m_recv_date);
                obj.Det_Id = (Guid)hrzjzs[0].Id;
                obj.bsO_Id = Org.Id;
                try
                {
                    NewHJGathData2 gathobj = EntityManager<NewHJGathData2>.GetByPk<NewHJGathData2>("Det_Id", obj.Det_Id);
                    if ((Convert.ToInt32(gathobj.HJGathF62) & 0x03) == 3)
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

                        obj = (HJHrzControlData2Part2)SetValueByReflectionFromBytes<HJHrzControlData2Part2>(obj, pi, propertyInfo, buff);


                    }
                    catch (Exception ex)
                    {
                        log.Error("ReadData.Parse(" + pi.FieldName + "):" + ex.Message);
                    }
                }

                EntityManager<HJHrzControlData2Part2>.Add<HJHrzControlData2Part2>(obj);
              
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
            return base.CreateModbusRtuReadCommand(simno, 0x01, 0x03, 0x152, 0x01BA + 1 - 0x152);
            // //01030152006925C9
        }



        public override DeviceCmd CreateModbusRtuWriteCommand(string simno, byte slaveaddr, int address, params object[] args)
        {
            return base.CreateModbusRtuWriteCommand(simno, slaveaddr, address, args);
        }

        public override DeviceCmd CreateModbusRtuWriteCommandByGath(string simno, object obj)
        {
            HJHrzControlData2Part2 tmpobj = (HJHrzControlData2Part2)obj;
            return base.CreateModbusRtuWriteCommand<HJHrzControlData2Part2>(simno, tmpobj, bsProtItems);
        }
        public override DeviceCmd CreateModbusRtuWriteCommandBySelfDefine(string simno)
        {
            Dev = BaseDABll.GetDtuProduct(simno);
            //Org = BaseDABll.GetOrganize((Guid)Dev.AZWZ);

            //HJHrzControlData2Part2 obj = EntityManager<HJHrzControlData2Part2>.GetByPk<HJHrzControlData2Part2>("bsO_Id", Guid.Parse(Org.BllOrgID));

            //return base.CreateModbusRtuWriteCommand<HJHrzControlData2Part2>(simno, obj, bsProtItems);
            //// return new DeviceCmd();


            List<HJHrzControlData2Part2> objs = EntityManager<HJHrzControlData2Part2>.GetAllByStorProcedure<HJHrzControlData2Part2>("splyGetClouldStorageData '", Dev.AZWZ.ToString() + "',5");
            return base.CreateModbusRtuWriteCommand<HJHrzControlData2Part2>(simno, objs[0], bsProtItems);
        }
    }
}


