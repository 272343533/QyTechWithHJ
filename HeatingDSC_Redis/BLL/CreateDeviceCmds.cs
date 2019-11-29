using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QyTech.Communication;
using SunMvcExpress.Dao;

using HeatingDSC.Models;
using log4net;
using QyTech.Protocal;

namespace HeatingDSC.BLL
{
    public class CreateDeviceCmds:IProduct
    {

        public DeviceCmd CreateJinLaiWeather(DTUProduct d)
        {
            ParseHrzDownWeather pdw = new ParseHrzDownWeather();

            byte[] commdarr = new byte[9];
            DeviceCmd cmd2send;
            cmd2send = new DeviceCmd();
            try
            {
               
                cmd2send.CommNo =d.CommNo;

                string[] sets = devda.SelectCommProtocalByid((Guid)d.DeviceType.ParseProtocalId).CPDesp.Split(new char[] { ',', ';' });
                ModbusCommand.VerifyMode = ModbusCommand.RTU;
                ModbusCommand.Slaveaddr = Convert.ToByte(sets[0]);
                ModbusCommand.OperatMode = 0x10;
                ModbusCommand.RegStartAddr = 0xF0;//首地址16进制
                ModbusCommand.RegOpNum = 6;

                byte[] sendData = pdw.Create(d.AZWZ);
                ModbusCommand.byteArr = sendData;

                if (ModbusCommand.byteArr != null)//如果有下发的数据
                {
                    commdarr = ModbusCommand.Command();
                    cmd2send.SendCmd = commdarr;
                    //生成命令数组
                    if (commdarr != null)
                    {
                        StringBuilder strbuilder = new StringBuilder();
                        foreach (byte cmdbyte in commdarr)
                        {
                            strbuilder.Append(cmdbyte.ToString("X2"));
                        }
                        cmd2send.Command = strbuilder.ToString();
                    }

                }

            }
            catch (Exception ex)
            {
                WriteError("createcmdForDownKD", ex);
            }
            return cmd2send;
        }

        public DeviceCmd CreateSanFengWeather(DTUProduct d)
        {
            ParseHrzDownWeather pdw = new ParseHrzDownWeather();

            byte[] commdarr = new byte[9];
            DeviceCmd cmd2send;
            CommNumber siminfo;
            cmd2send = new DeviceCmd();
            try
            {
              
                cmd2send.CommNo =d.CommNo;

                string[] sets = devda.SelectCommProtocalByCode(d.CPCode.ToString()).CPDesp.Split(new char[] { ',', ';' });
                ModbusCommand.VerifyMode = ModbusCommand.RTU;
                ModbusCommand.Slaveaddr = Convert.ToByte(sets[0]);
                ModbusCommand.OperatMode = 0x10;
                ModbusCommand.RegStartAddr = 0x00;//首地址16进制
                ModbusCommand.RegOpNum = 6;

                byte[] sendData = pdw.Create(d.AZWZ);
                ModbusCommand.byteArr = sendData;

                if (ModbusCommand.byteArr != null)//如果有下发的数据
                {
                    commdarr = ModbusCommand.Command();
                    cmd2send.SendCmd = commdarr;
                    //生成命令数组
                    if (commdarr != null)
                    {
                        StringBuilder strbuilder = new StringBuilder();
                        foreach (byte cmdbyte in commdarr)
                        {
                            strbuilder.Append(cmdbyte.ToString("X2"));
                        }
                        cmd2send.Command = strbuilder.ToString();
                    }

                }

            }
            catch (Exception ex)
            {
                WriteError("createcmdForDownKD", ex);
            }
            return cmd2send;
        }
    }
}
