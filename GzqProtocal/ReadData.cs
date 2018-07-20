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

namespace QyTech.GzqProtocal
{
    public class ReadData: IProtocal
    {
        //采集数据
        //public Hrz1stNetOpen hisHrz1stOpen = new Hrz1stNetOpen();
        //public Hrz1stNetOpen newHrz1stOpen=new Hrz1stNetOpen();

        //public LastHRZCollectData net1data = new LastHRZCollectData();
        //public List<LastHRZCollectData> net2datas=new List<LastHRZCollectData>();
        //public OnTimeHRZCollectData hisHrzGatherdata = new OnTimeHRZCollectData();


        public static DateTime LastUpDate = DateTime.MaxValue;

        public ReadData(int bsP_Id)
            : base(bsP_Id)
        {
           
        }

        public override byte[] Create(object obj)
        {
            return base.Create(obj);
        }
        
        public override int Parse(GPRS_DATA_RECORD recdPtr)
        {
            if (base.Parse(recdPtr) == 1)
                return -1;
            //log.Info("Parse:10");
            Dev = BaseDABll.GetDtuProduct(recdPtr.m_userid);
            Org = BaseDABll.GetOrganize((Guid)Dev.AZWZ);

            List<DetailDevice> hrzjzs = BaseDABll.GetDetailDevice(Dev.Id);

            List<DeviceCmd> cmds = new List<DeviceCmd>();
            //生成命令，
            DeviceCmd cmd;
            IProtocal prot;

            int resualt = 1;
            OTStandDataStr obj = new OTStandDataStr();

            try
            {
                #region 解析hrz数据对象
                int bufflen;
                Type type = obj.GetType(); //获取类型
                int InitAddr = Convert.ToInt32(bsProtItems[0].StartRegAddress.Substring(2), 16);
                int ItemAddr;

                obj.GathDt = Convert.ToDateTime(recdPtr.m_recv_date);
                obj.Det_Id = hrzjzs[0].Id;
                obj.bsO_Id = Org.Id;
                obj.Det_Name = hrzjzs[0].DeviceName;
                obj.bsO_Name = Org.Name;
                obj.bsp_Id = bsProtocalObj.Id;

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
                        log.Error("ReadData.Parse(" + recdPtr.m_userid + "---" + pi.FieldName + "):" + ex.Message);
                    }
                }

         
               
                #endregion



                EntityManager<OTStandDataStr>.Add<OTStandDataStr>(obj);

                NewStandDataStr newobj = EntityManager<NewStandDataStr>.GetBySql<NewStandDataStr>("Det_Id='" + obj.Det_Id.ToString() + "'");
                if (newobj == null)
                {
                    newobj = EntityOperate.Copy<NewStandDataStr>(obj);
                    newobj.F02 = "0";
                    EntityManager<NewStandDataStr>.Add<NewStandDataStr>(newobj);
                }
                else
                {
                    EntityOperate.Copy<NewStandDataStr>(obj, newobj, "Det_Id");
                    EntityManager<NewStandDataStr>.Modify<NewStandDataStr>(newobj);
                }
            }
            catch (Exception ex)
            {
                log.Error("HjProtocal2 parse:simno: New Object" + recdPtr.m_userid + ":" + ex.InnerException + "-" + ex.Message);
                resualt = -1;
            }
            finally
            {
                obj = null;
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
            return base.CreateModbusRtuWriteCommand(simno,  slaveaddr,address, args);
        }

      
    }
}
