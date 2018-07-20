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

namespace QyTech.ProtocalHrzWd2
{
    public class ReadData2 : IProtocal
    {
        //采集数据
        //public Hrz1stNetOpen hisHrz1stOpen = new Hrz1stNetOpen();
        //public Hrz1stNetOpen newHrz1stOpen=new Hrz1stNetOpen();

        //public LastHRZCollectData net1data = new LastHRZCollectData();
        //public List<LastHRZCollectData> net2datas=new List<LastHRZCollectData>();
        public OnTimeHRZCollectData hisHrzGatherdata = new OnTimeHRZCollectData();


        public static DateTime LastUpDate = DateTime.MaxValue;

        public ReadData2(int bsP_Id)
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
                obj.Det_Id = hrzjzs[1].Id;
                obj.bsO_Id = Org.Id;
                obj.Det_Name = hrzjzs[1].DeviceName;
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
                        log.Error("ReadData2.Parse(" + recdPtr.m_userid + "---" + pi.FieldName + "):" + ex.Message);
                    }
                }
                #endregion



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
                //数据已经准备好，准备采集，需要加入读取指令
                if ((Convert.ToUInt16(obj.F52) & 0x01) == 0x01)
                {
                    OnProtocalDataReceivedProgress(recdPtr.m_userid, obj);

                    if ((Convert.ToUInt16(obj.F52) & 0x01) == 0x01)
                    {
                        #region 对其它5个包的数据，判断是否需要采集做云采集存储
                        prot = new WriteData1(281);
                        cmd = prot.CreateReadCommand(recdPtr.m_userid);
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmd.CommandDesp = "控制参数1数据采集";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(0);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmds.Add(cmd);

                        prot = new WriteData2(282);
                        cmd = prot.CreateReadCommand(recdPtr.m_userid);
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmd.CommandDesp = "控制参数2数据采集";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(1);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmds.Add(cmd);
                        prot = new WriteData3(288);
                        cmd = prot.CreateReadCommand(recdPtr.m_userid);
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmd.CommandDesp = "控制参数3数据采集";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(1);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmds.Add(cmd);
                        prot = new WriteData4(289);
                        cmd = prot.CreateReadCommand(recdPtr.m_userid);
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmd.CommandDesp = "控制参数4数据采集";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(1);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmds.Add(cmd);


                        prot = new WriteCurve(283);
                        cmd = prot.CreateReadCommand(recdPtr.m_userid);
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmd.CommandDesp = "曲线配置数据采集";
                        cmd.SetDownTime =DateTime.Now.AddSeconds(2);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmds.Add(cmd);


                        prot = new Range1(286);
                        cmd = prot.CreateReadCommand(recdPtr.m_userid);
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmd.CommandDesp = "量程1数据采集";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(3); ;
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmds.Add(cmd);
                        prot = new Range2(287);
                        cmd = prot.CreateReadCommand(recdPtr.m_userid);
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmd.CommandDesp = "量程2数据采集";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(3); ;
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmds.Add(cmd);


                        prot = new Alarm1(284);
                        cmd = prot.CreateReadCommand(recdPtr.m_userid);
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmd.CommandDesp = "报警1数据采集";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(4);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmds.Add(cmd);
                        prot = new Alarm2(285);
                        cmd = prot.CreateReadCommand(recdPtr.m_userid);
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmd.CommandDesp = "报警2数据采集";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(4);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmds.Add(cmd);

                        #endregion

                        if (cmds != null && cmds.Count > 0)
                        {
                            cmd = CreateModbusRtuWriteCommand(recdPtr.m_userid, 0x01, 0xDD, Convert.ToUInt16(Convert.ToUInt16(obj.F52) & 0xFFF0));
                            cmd.NeedSendTime = DateTime.Now;
                            cmd.bsO_id = Guid.Parse(obj.bsO_Id.ToString());
                            cmd.bsP_Id = (int)Dev.bsP_Id;
                            cmd.CommandDesp = "读数据恢复写入";
                            cmd.SetDownTime = DateTime.Now.AddSeconds(5);
                            cmd.NeedSendTime = cmd.SetDownTime;
                            cmds.Add(cmd);

                            OndelProtocalDeviceCommandProgress(recdPtr.m_userid, cmds);
                        }
                    }
                }

                if ((Convert.ToUInt16(obj.F53) & 0x01) == 0x01)
                {
                    //需要把到云存储（供热站级别）中的配置数据，下发到该换热站
                    if ((Convert.ToUInt16(obj.F53) & 0x02) == 0x02)
                    {
                        #region 获取云端配置 生成命令，

                        prot = new WriteData1(281);
                        cmd = prot.CreateModbusRtuWriteCommandBySelfDefine(recdPtr.m_userid);
                        cmd.CommandDesp = "控制参数1数据云通用下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(1);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);

                        prot = new WriteData2(282);
                        cmd = prot.CreateModbusRtuWriteCommandBySelfDefine(recdPtr.m_userid);
                        cmd.CommandDesp = "控制参数2数据云通用下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(2);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);
                        prot = new WriteData3(288);
                        cmd = prot.CreateModbusRtuWriteCommandBySelfDefine(recdPtr.m_userid);
                        cmd.CommandDesp = "控制参数3数据云通用下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(2);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);
                        prot = new WriteData4(289);
                        cmd = prot.CreateModbusRtuWriteCommandBySelfDefine(recdPtr.m_userid);
                        cmd.CommandDesp = "控制参数4数据云通用下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(2);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);


                        prot = new WriteCurve(283);
                        cmd = prot.CreateModbusRtuWriteCommandBySelfDefine(recdPtr.m_userid);
                        cmd.CommandDesp = "曲线数据云通用下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(3);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);


                        prot = new Range1(286);
                        cmd = prot.CreateModbusRtuWriteCommandBySelfDefine(recdPtr.m_userid);
                        cmd.CommandDesp = "量程1数据云通用下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(4);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);
                        prot = new Range2(287);
                        cmd = prot.CreateModbusRtuWriteCommandBySelfDefine(recdPtr.m_userid);
                        cmd.CommandDesp = "量程2数据云通用下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(4);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);


                        prot = new Alarm1(285);
                        cmd = prot.CreateModbusRtuWriteCommandBySelfDefine(recdPtr.m_userid);
                        cmd.CommandDesp = "报警1数据云通用下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(5);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);
                        prot = new Alarm2(286);
                        cmd = prot.CreateModbusRtuWriteCommandBySelfDefine(recdPtr.m_userid);
                        cmd.CommandDesp = "报警2数据云通用下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(5);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);
                        #endregion
                    }
                    else
                    {
                        #region 获取本地配置,获得最后一次采集的数据，下发。没有配置
                        cmds = new List<DeviceCmd>();
                        prot = new WriteData1(281);
                        cmd = prot.CreateModbusRtuWriteCommandBySelfDefine(recdPtr.m_userid);
                        cmd.CommandDesp = "控制参数1数据云下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(1);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);

                        prot = new WriteData2(282);
                        cmd = prot.CreateModbusRtuWriteCommandBySelfDefine(recdPtr.m_userid);
                        cmd.CommandDesp = "控制参数2数据云下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(2);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);
                        prot = new WriteData3(288);
                        cmd = prot.CreateModbusRtuWriteCommandBySelfDefine(recdPtr.m_userid);
                        cmd.CommandDesp = "控制参数3数据云下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(2);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);
                        prot = new WriteData4(289);
                        cmd = prot.CreateModbusRtuWriteCommandBySelfDefine(recdPtr.m_userid);
                        cmd.CommandDesp = "控制参数4数据云下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(2);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);


                        prot = new WriteCurve(283);
                        cmd = prot.CreateModbusRtuWriteCommandBySelfDefine(recdPtr.m_userid);
                        cmd.CommandDesp = "曲线数据云下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(3);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);


                        prot = new Range1(286);
                        cmd = prot.CreateModbusRtuWriteCommandBySelfDefine(recdPtr.m_userid);
                        cmd.CommandDesp = "量程1数据云下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(4);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);
                        prot = new Range2(287);
                        cmd = prot.CreateModbusRtuWriteCommandBySelfDefine(recdPtr.m_userid);
                        cmd.CommandDesp = "量程2数据云下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(4);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);


                        prot = new Alarm1(285);
                        cmd = prot.CreateModbusRtuWriteCommandBySelfDefine(recdPtr.m_userid);
                        cmd.CommandDesp = "报警1数据云下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(5);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);
                        prot = new Alarm2(286);
                        cmd = prot.CreateModbusRtuWriteCommandBySelfDefine(recdPtr.m_userid);
                        cmd.CommandDesp = "报警2数据云下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(5);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);

                        #endregion
                    }
                    if (cmds != null && cmds.Count > 0)
                    {
                        cmd = CreateModbusRtuWriteCommand(recdPtr.m_userid, 0x01, 0xDE, Convert.ToUInt16(Convert.ToUInt16(obj.F53) & 0xFFF0));
                        cmd.NeedSendTime = DateTime.Now;
                        cmd.bsO_id =(Guid)obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmd.CommandDesp = "写数据恢复";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(6);
                        cmds.Add(cmd);

                        OndelProtocalDeviceCommandProgress(recdPtr.m_userid, cmds);
                    }
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
