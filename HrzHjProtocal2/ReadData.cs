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

namespace QyTech.HjProtocal2
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
        

        public override DeviceCmd CreateReadCommand(string simno)
        {
            if (ReadPacketCommand != null && ReadPacketCommand.CommNo != null && !ReadPacketCommand.CommNo.Equals(""))
            {
                return ReadPacketCommand;
            }
            else
                return base.CreateModbusRtuReadCommand(simno, 0x01, 0x03, 0x0, 0x64 - 0x0 + 1);
            //01030000006585E1
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
            HJGathData2 obj = new HJGathData2();

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

                        obj = (HJGathData2)SetValueByReflectionFromBytes<HJGathData2>(obj, pi, propertyInfo, buff);
                    }
                    catch (Exception ex)
                    {
                        log.Error("ReadData.Parse(" + recdPtr.m_userid + "---" + pi.FieldName + "):" + ex.Message);
                    }
                }

                obj.GathDt = Convert.ToDateTime(recdPtr.m_recv_date);
                obj.Det_Id = hrzjzs[0].Id;
                obj.bsO_Id = Org.Id;
                try
                {
                    obj.HJGathF2C_2F = Convert.ToDateTime("20" + obj.HJGathF2C.Substring(0, 2) + "-" + obj.HJGathF2C.Substring(2, 2) + "-" + obj.HJGathF2D.Substring(0, 2) + " " + obj.HJGathF2D.Substring(2, 2) + ":" + obj.HJGathF2E.Substring(0, 2) + ":" + obj.HJGathF2E.Substring(2, 2));
                }
                catch (Exception ex)
                {
                    log.Error("ReadData.Parse(" + recdPtr.m_userid + "---HJGathF2C_2F):" + ex.Message);
                }
                #endregion



                EntityManager<HJGathData2>.Add<HJGathData2>(obj);

                NewHJGathData2 newobj = EntityManager<NewHJGathData2>.GetBySql<NewHJGathData2>("Det_Id='" + obj.Det_Id.ToString() + "'");
                if (newobj == null)
                {
                    newobj = EntityOperate.Copy<NewHJGathData2>(obj);
                    EntityManager<NewHJGathData2>.Add<NewHJGathData2>(newobj);
                }
                else
                {
                    EntityOperate.Copy<NewHJGathData2>(obj, newobj, "Det_Id");
                    EntityManager<NewHJGathData2>.Modify<NewHJGathData2>(newobj);
                }
                //数据已经准备好，准备采集，需要加入读取指令
                if ((Convert.ToUInt16(obj.HJGathF62) & 0x01) == 0x01)
                {
                    //EntityManager<HJGathData2>.Add<HJGathData2>(obj);

                    //NewHJGathData2 newobj = EntityManager<NewHJGathData2>.GetBySql<NewHJGathData2>("Det_Id='" + obj.Det_Id.ToString() + "'");
                    //if (newobj == null)
                    //{
                    //    newobj = EntityOperate.Copy<NewHJGathData2>(obj);
                    //    EntityManager<NewHJGathData2>.Add<NewHJGathData2>(newobj);
                    //}
                    //else
                    //{
                    //    EntityOperate.Copy<NewHJGathData2>(obj, newobj, "Det_Id");
                    //    EntityManager<NewHJGathData2>.Modify<NewHJGathData2>(newobj);
                    //}
                    OnProtocalDataReceivedProgress(recdPtr.m_userid, obj);

                    if ((Convert.ToUInt16(obj.HJGathF62) & 0x01) == 0x01 || (Convert.ToUInt16(obj.HJGathF62) & 0x02)==0x02)
                    {
                        string flag="";
                        if ((Convert.ToUInt16(obj.HJGathF62) & 0x02) == 0x02){
                            flag="公共配置";
                        }
                        #region 对其它5个包的数据，判断是否需要采集做云采集存储
                        prot = new WritePart1(42);
                        cmd = prot.CreateReadCommand(recdPtr.m_userid);
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmd.CommandDesp =flag+ "控制参数1数据采集";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(0);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmds.Add(cmd);

                        prot = new WritePart2(43);
                        cmd = prot.CreateReadCommand(recdPtr.m_userid);
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmd.CommandDesp = flag + "控制参数2数据采集";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(1);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmds.Add(cmd);


                        prot = new WritePartCurve(44);
                        cmd = prot.CreateReadCommand(recdPtr.m_userid);
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmd.CommandDesp = flag + "曲线配置数据采集";
                        cmd.SetDownTime =DateTime.Now.AddSeconds(2);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmds.Add(cmd);


                        prot = new Range(46);
                        cmd = prot.CreateReadCommand(recdPtr.m_userid);
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmd.CommandDesp = flag + "量程数据采集";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(3);;
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmds.Add(cmd);


                        prot = new Alarm(45);
                        cmd = prot.CreateReadCommand(recdPtr.m_userid);
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmd.CommandDesp = flag + "报警数据采集";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(4);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmds.Add(cmd);

                        #endregion

                        if (cmds != null && cmds.Count > 0)
                        {
                            cmd = CreateModbusRtuWriteCommand(recdPtr.m_userid, 0x01, 0x62, Convert.ToUInt16((int)obj.HJGathF62&0xFFF0));
                            cmd.NeedSendTime = DateTime.Now;
                            cmd.bsO_id = obj.bsO_Id;
                            cmd.bsP_Id = (int)Dev.bsP_Id;
                            cmd.CommandDesp = flag + "读数据恢复写入";
                            cmd.SetDownTime = DateTime.Now.AddSeconds(5);
                            cmd.NeedSendTime = cmd.SetDownTime;
                            cmds.Add(cmd);

                            OndelProtocalDeviceCommandProgress(recdPtr.m_userid, cmds);
                        }
                    }
                }

                if ((Convert.ToUInt16(obj.HJGathF63) & 0x01) == 0x01)
                {
                    //需要把到云存储（供热站级别）中的配置数据，下发到该换热站
                    if ((Convert.ToUInt16(obj.HJGathF63) & 0x02) == 0x02)
                    {
                        #region 获取云端配置 生成命令，

                        prot = new WritePart1(42);
                        cmd = prot.CreateModbusRtuWriteCommandBySelfDefine(recdPtr.m_userid);
                        cmd.CommandDesp = "控制参数1数据云下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(1);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);

                        prot = new WritePart2(43);
                        cmd = prot.CreateModbusRtuWriteCommandBySelfDefine(recdPtr.m_userid);
                        cmd.CommandDesp = "控制参数2数据云下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(2);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);


                        prot = new WritePartCurve(44);
                        cmd = prot.CreateModbusRtuWriteCommandBySelfDefine(recdPtr.m_userid);
                        cmd.CommandDesp = "曲线数据云下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(3);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);


                        prot = new Range(46);
                        cmd = prot.CreateModbusRtuWriteCommandBySelfDefine(recdPtr.m_userid);
                        cmd.CommandDesp = "量程数据云下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(4);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);


                        prot = new Alarm(45);
                        cmd = prot.CreateModbusRtuWriteCommandBySelfDefine(recdPtr.m_userid);
                        cmd.CommandDesp = "报警数据云下发";
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
                        //控制参数1
                        HJHrzControlData2Part1 kc1 = EntityManager<HJHrzControlData2Part1>.GetBySql<HJHrzControlData2Part1>("GathDt= (select max(GathDt) from  HJHrzControlData2Part1 where bsO_Id='" + obj.bsO_Id.ToString() + "') and bsO_Id='" + obj.bsO_Id + "'");

                        prot = new WritePart1(42);
                        cmd = prot.CreateModbusRtuWriteCommandByGath(recdPtr.m_userid, kc1);
                        cmd.CommandDesp = "控制参数1下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(1);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);

                        //控制参数2
                        HJHrzControlData2Part2 kc2 = EntityManager<HJHrzControlData2Part2>.GetBySql<HJHrzControlData2Part2>("GathDt= (select max(GathDt) from  HJHrzControlData2Part2 where bsO_Id='" + obj.bsO_Id.ToString() + "') and bsO_Id='" + obj.bsO_Id + "'");
                        prot = new WritePart2(43);
                        cmd = prot.CreateModbusRtuWriteCommandByGath(recdPtr.m_userid, kc2);
                        cmd.CommandDesp = "控制参数2下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(2);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);

                        //曲线
                        HjPlcParaControlCurve qx = EntityManager<HjPlcParaControlCurve>.GetBySql<HjPlcParaControlCurve>("GathDt= (select max(GathDt) from  HjPlcParaControlCurve where bsO_Id='" + obj.bsO_Id.ToString() + "') and bsO_Id='" + obj.bsO_Id + "'");
                        prot = new WritePartCurve(44);
                        cmd = prot.CreateModbusRtuWriteCommandByGath(recdPtr.m_userid, qx);
                        cmd.CommandDesp = "曲线参数下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(3);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);

                        //量程
                        HrzRangeAlarmConf lg = EntityManager<HrzRangeAlarmConf>.GetBySql<HrzRangeAlarmConf>("bsO_Id='" + obj.bsO_Id + "' and Filter='量程'");
                        prot = new Range(46);
                        cmd = prot.CreateModbusRtuWriteCommandByConf(recdPtr.m_userid, lg);
                        cmd.CommandDesp = "量程参数下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(4);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);

                        //报警
                        HrzRangeAlarmConf bj = EntityManager<HrzRangeAlarmConf>.GetBySql<HrzRangeAlarmConf>("bsO_Id='" + obj.bsO_Id + "' and Filter='报警'");
                        prot = new Alarm(45);
                        cmd = prot.CreateModbusRtuWriteCommandByConf(recdPtr.m_userid, bj);
                        cmd.CommandDesp = "报警参数下发";
                        cmd.SetDownTime = DateTime.Now.AddSeconds(5);
                        cmd.NeedSendTime = cmd.SetDownTime;
                        cmd.bsO_id = obj.bsO_Id;
                        cmd.bsP_Id = (int)Dev.bsP_Id;
                        cmds.Add(cmd);
                        #endregion
                    }
                    if (cmds != null && cmds.Count > 0)
                    {
                        cmd = CreateModbusRtuWriteCommand(recdPtr.m_userid, 0x01, 0x63, Convert.ToUInt16((int)obj.HJGathF62&0xFFF0));
                        cmd.NeedSendTime = DateTime.Now;
                        cmd.bsO_id = obj.bsO_Id;
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
