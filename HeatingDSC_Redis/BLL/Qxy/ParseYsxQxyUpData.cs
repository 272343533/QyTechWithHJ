using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QyTech.Communication;
using SunMvcExpress.Dao;
using QyTech.HDGprs;


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
using QyTech.Core;
using SunMvcExpress.Core;

namespace HeatingDSC.BLL
{
    public class ParseYsxQxyUpData : IProtocal
    {
        //采集数据
        public WeaDataHistory otobj = new WeaDataHistory();


        public ParseYsxQxyUpData()
        {
            //Producttype = ProductType.QxyUpData;
        }
        public override int Parse(GPRS_DATA_RECORD recdPtr)
        {
            byte[] buff;
            int resualt = 0;
            try
            {
                //log.Info("qxy0:"+recdPtr.m_userid);
                if (base.Parse(recdPtr) == 1)
                    return -1;
                DTUProduct qxyobj = EntityManager<DTUProduct>.GetBySql<DTUProduct>("CommNo='"+recdPtr.m_userid+"'");
                //log.Info("qxy1:"+qxyobj.Id.ToString()+":"+qxyobj.DeviceID.ToString());
                otobj.DTU_Id = qxyobj.Id;
                otobj.bsO_Id = qxyobj.bsO_Id;

                #region 解析hrz数据对象
                
                otobj.GathDt = DateTime.Parse(recdPtr.m_recv_date);//


                buff = new byte[2];
                Buffer.BlockCopy(GetData, 0x07*2, buff, 0, 2); CrossHiLow(ref buff);
                otobj.WDHis = (decimal)(BitConverter.ToInt16(buff, 0) / 10.0);

                Buffer.BlockCopy(GetData, 0x10*2, buff, 0, 2); CrossHiLow(ref buff);
                otobj.RZHis = BitConverter.ToUInt16(buff, 0);

                Buffer.BlockCopy(GetData, 0x0C*2, buff, 0, 2); CrossHiLow(ref buff);
                otobj.FSHis = (decimal)(BitConverter.ToUInt16(buff, 0) / 10.0);

                #endregion
                //存入历史数据表
                EntityManager<WeaDataHistory>.Add<WeaDataHistory>(otobj);
                NewWeaData newobj = EntityManager<NewWeaData>.GetBySql<NewWeaData>("DTU_Id='" + otobj.DTU_Id.ToString() + "'");
            
                if (newobj == null)
                {
                    newobj=EntityOperate.Copy<NewWeaData>(otobj);
                    EntityManager<NewWeaData>.Add<NewWeaData>(newobj);
                }
                else
                {
                    EntityOperate.Copy<NewWeaData>(otobj, newobj, "DTU_Id");
                    EntityManager<NewWeaData>.Modify<NewWeaData>(newobj);
                }
            }
            catch (Exception ex)
            {
                log.Error("qxy:" + ex.Message);
                resualt = -1;
            }
            return resualt;
        }

        public List<DeviceCmd> CreateProtQxy(List<DTUProduct> gprsDevs)
        {
            List<DeviceCmd> listcmd = new List<DeviceCmd>();
            byte[] commdarr = new byte[9];
            DeviceCmd cmd2send;
            foreach (DTUProduct d in gprsDevs)
            {
                try
                {
                    cmd2send = new DeviceCmd();
                    cmd2send.CommNo = d.CommNo;

                    ModbusCommand.VerifyMode = ModbusCommand.RTU;
                    ModbusCommand.Slaveaddr =0x01;
                    ModbusCommand.OperatMode = 0x03;
                    ModbusCommand.RegStartAddr = 0x00;//首地址16进制
                    ModbusCommand.RegOpNum =0x001A;//, 16);// Convert.ToInt32(sets[2], 16);//字节数量10进制

                    commdarr = ModbusCommand.Command();

                    //生成命令数组
                    if (commdarr != null)
                    {
                        cmd2send.SendCmd = commdarr;
                        StringBuilder strbuilder = new StringBuilder();
                        foreach (byte cmdbyte in commdarr)
                        {
                            strbuilder.Append(cmdbyte.ToString("X2"));
                        }
                        cmd2send.Command = strbuilder.ToString();
                    }
                    listcmd.Add(cmd2send);
                }
                catch (Exception ex)
                {
                    log.Error("气象数据创建:" + d.Id.ToString() + "-" + ex.Message);
                }
            }
            return listcmd;
        }
    }
}
