using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QyTech.Communication;
using SunMvcExpress.Dao;

using HeatingDSC.Models;
using log4net;
using DAHeating.DataAccess;
using SunMvcExpress.Dao;
using QyTech.Protocal;

namespace HeatingDSC.BLL
{
    public class DownHrzControl:IProduct
    {
        public DeviceCmd CreateHandDownCmd(Guid orgid,string Addr,params object[] args)
        {

            DeviceDa devda = new DeviceDa();
            List<DetailDevice> devdetails;
             ParseHrzDownUnitRangeData phdurd;

            byte[] sendData = new byte[2];
            byte[] buff;
            DTUProduct d = hrzda.SelectHrzGprsDevice(orgid);
            byte[] commdarr = new byte[9];
            DeviceCmd cmd2send;
            CommNumber siminfo;

            cmd2send = new DeviceCmd();
            cmd2send.CommNo =d.CommNo;

            ModbusCommand.VerifyMode = ModbusCommand.RTU;
            ModbusCommand.Slaveaddr = Convert.ToByte("01");
            ModbusCommand.OperatMode = 0x10;
                    
          
            if (d.DeviTypeId.ToString() != "26" && d.DeviTypeId.ToString()!="25")
            {
                int intaddr = Convert.ToInt32(Addr, 16);
          
                #region 判断地址范围
                if (intaddr == 0x66 || intaddr == 0x68)
                {
                    intaddr = 0x64;
                }
                else if ((intaddr >= 0x180 && intaddr < 0x200) || (intaddr >= 0x380 && intaddr < 0x400))
                {
                    intaddr -= Convert.ToInt32("80", 16);
                }
                else if ((intaddr >= 0x200 && intaddr < 0x280) || (intaddr >= 0x400 && intaddr < 0x480))
                {
                    intaddr -= Convert.ToInt32("80", 16) * 2;
                }
                #endregion

                #region 根据选择生成数据
                try
                {
                    ModbusCommand.RegStartAddr = Convert.ToInt32(Addr, 16);//首地址16进制

                    switch (intaddr.ToString("X"))//0x后面的地址
                    {
                        #region 换热站公共部分
                        case "0":
                            ModbusCommand.RegOpNum = 1;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            sendData = BitConverter.GetBytes(Convert.ToUInt16(args[0])); CrossHiLow(ref sendData);
                            break;
                        case "12":
                            ModbusCommand.RegOpNum = 2;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            sendData = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref sendData);
                            break;
                        case "6A":
                            ModbusCommand.RegOpNum = 4;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[1])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 4, 4);
                            break;
                        case "26":
                            ModbusCommand.RegOpNum = 1;//字节数量10进制
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            sendData = BitConverter.GetBytes(Convert.ToUInt16(args[0])); CrossHiLow(ref sendData);
                            break;
                        case "8":
                            ModbusCommand.RegOpNum = 1;//字节数量10进制
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            sendData = BitConverter.GetBytes(Convert.ToUInt16(args[0])); CrossHiLow(ref sendData);
                            break;
                        case "9":
                            ModbusCommand.RegOpNum = 1;//字节数量10进制
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            sendData = BitConverter.GetBytes(Convert.ToUInt16(args[0])); CrossHiLow(ref sendData);
                            break;
                        case "A":
                            ModbusCommand.RegOpNum = 4;//字节数量10进制
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[1])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 4, 4);
                            break;
                        case "E":
                            ModbusCommand.RegOpNum = 4;//字节数量10进制
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[1])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 4, 4);
                            break;
                        case "14":
                            ModbusCommand.RegOpNum = 6;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[1])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 4, 4);
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[2])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 8, 4);
                            break;
                        case "20":
                            ModbusCommand.RegOpNum = 2;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            sendData = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref sendData);
                            break;
                        case "22":
                            ModbusCommand.RegOpNum = 2;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            sendData = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref sendData);
                            break;
                        case "24":
                            ModbusCommand.RegOpNum = 63;//字节数量10进制

                            HrzCommGath hcg = hrzda.SelectCommGath(orgid);
                            int autocontrolflag = 0;
                            if (hcg != null)
                                autocontrolflag = (int)hcg.ControlStatus;
                            sendData = DownCurves(Guid.Parse(args[0].ToString()), autocontrolflag);
                            break;
                        case "64":
                            ModbusCommand.RegOpNum = 2;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            sendData = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref sendData);
                            break;
                        #endregion
                        #region 换热站机组命令
                        case "10A"://  0x010A   循环泵停差压
                            ModbusCommand.RegOpNum = 2;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            sendData = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref sendData);
                            break;
                        case "10C"://0x010C    循环泵调节二次网差压目标值
                            ModbusCommand.RegOpNum = 2;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            sendData = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref sendData);
                            break;
                        case "10E"://0x010E    循环泵调节二次网差压精度
                            ModbusCommand.RegOpNum = 2;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            sendData = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref sendData);
                            break;
                        //case "116"://0x0116  补水泵启动压力
                        //    //ModbusCommand.RegOpNum = 2;
                        //    //sendData = BitConverter.GetBytes(Convert.ToSingle(this.txt0x116.Text)); CrossHiLow(ref sendData);
                        //   break;
                        //case "118"://0x0118  补水泵停止压力
                        //    break;
                        case "11A"://0x011A  补水泵启动位
                            ModbusCommand.RegOpNum = 2;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            sendData = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref sendData);
                            break;
                        case "11C"://0x011C  补水泵停止位
                            ModbusCommand.RegOpNum = 2;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            sendData = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref sendData);
                            break;
                        case "156"://0x0156  阀门开启并恢复自动控制循环泵变频器
                            ModbusCommand.RegOpNum = 2;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            sendData = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref sendData);
                            break;
                        case "158"://0x0158  阀门关闭循环泵变频
                            ModbusCommand.RegOpNum = 2;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            sendData = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref sendData);
                            break;
                        #endregion
                        #region 量程数据
                        case "300"://0x0300 换热站本地室外温度
                            ModbusCommand.RegOpNum = 4;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[1])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 4, 4);
                            break;
                        case "304"://0x0304 一次网供水温度
                            ModbusCommand.RegOpNum = 4;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[1])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 4, 4);
                            break;
                        case "308"://0x0308 一次网回水温度
                            ModbusCommand.RegOpNum = 4;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[1])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 4, 4);
                            break;
                        case "30C"://0x030C 一次网供水压力
                            ModbusCommand.RegOpNum = 4;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[1])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 4, 4);
                            break;
                        case "310"://0x0310 一次网回水压力
                            ModbusCommand.RegOpNum = 4;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[1])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 4, 4);
                            break;
                        case "314"://0x0314 二次网供水温度
                            ModbusCommand.RegOpNum = 4;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[1])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 4, 4);
                            break;
                        case "318"://0x0318 二次网回水温度
                            ModbusCommand.RegOpNum = 4;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[1])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 4, 4);
                            break;
                        case "31C":// 二次网供水压力r
                            ModbusCommand.RegOpNum = 4;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[1])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 4, 4);
                            break;
                        case "320":// 二次网回水压力
                            ModbusCommand.RegOpNum = 4;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[1])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 4, 4);
                            break;
                        case "324":// 水箱水位上限
                            ModbusCommand.RegOpNum = 4;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[1])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 4, 4);
                            break;
                        case "328":// 1#循环泵变频器频率
                            ModbusCommand.RegOpNum = 4;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[1])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 4, 4);
                            break;
                        case "32C":// 1#循环泵工作电流
                            ModbusCommand.RegOpNum = 4;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[1])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 4, 4);
                            break;
                        case "330":// 2#循环泵变频器频率
                            ModbusCommand.RegOpNum = 4;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[1])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 4, 4);
                            break;
                        case "334": // 2#循环泵工作电流
                            ModbusCommand.RegOpNum = 4;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[1])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 4, 4);
                            break;
                        case "338":// 1#补水泵变频器频率
                            ModbusCommand.RegOpNum = 4;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[1])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 4, 4);
                            break;
                        case "33C":// 1#补水泵工作电流
                            ModbusCommand.RegOpNum = 4;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[1])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 4, 4);
                            break;
                        case "340":// 2#补水泵变频器频率
                            ModbusCommand.RegOpNum = 4;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[1])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 4, 4);
                            break;
                        case "344":// 2#补水泵工作电流
                            ModbusCommand.RegOpNum = 4;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[1])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 4, 4);
                            break;
                        case "348":// 补水泵出口压力
                            ModbusCommand.RegOpNum = 4;
                            sendData = new byte[ModbusCommand.RegOpNum * 2];
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                            buff = BitConverter.GetBytes(Convert.ToSingle(args[2])); CrossHiLow(ref buff);
                            Buffer.BlockCopy(buff, 0, sendData, 4, 4);
                            break;
                        #endregion

                        #region 量程一次下发
                        case "301"://0x0300 换热站本地室外温度
                            ModbusCommand.RegOpNum = 0x4C;
                            ModbusCommand.RegStartAddr = Convert.ToInt32(Addr, 16) - 1;//首地址16进制

                            devdetails = devda.SelectHrzUnitGprsDeviceByAZWZ(orgid);
                            phdurd = new ParseHrzDownUnitRangeData();
                            //sendData = phdurd.CreateRangeData(devdetails[Convert.ToInt32(args[0])-1].DeviDetaID);
                            sendData = phdurd.CreateRangeData(devdetails[0].Id);
                            break;
                        case "381"://0x0300 换热站本地室外温度
                            ModbusCommand.RegOpNum = 0x4C;

                            devdetails = devda.SelectHrzUnitGprsDeviceByAZWZ(orgid);
                            phdurd = new ParseHrzDownUnitRangeData();
                            sendData = phdurd.CreateRangeData(devdetails[1].Id);
                            break;

                        case "401"://0x0300 换热站本地室外温度
                            ModbusCommand.RegOpNum = 0x4C;
                            devdetails = devda.SelectHrzUnitGprsDeviceByAZWZ(orgid);
                            phdurd = new ParseHrzDownUnitRangeData();
                            sendData = phdurd.CreateRangeData(devdetails[2].Id);
                            break;
                        #endregion
                        default:
                            throw new Exception("没有该地址：0x" + Addr);
                            break;
                    }


                }
                catch (Exception ex)
                {
                    throw new Exception("0x" + Addr + ":(" + ex.InnerException + "-" + ex.Message + ")");
                }
                #endregion
            }
            else
            {
                
                if (Addr == (0x1B4).ToString()) //下发曲线
                {
                    using (WSExpressEntities heatingdb = new WSExpressEntities())
                    {
                        string addr = "0x0" + Convert.ToString(Convert.ToInt32(Addr), 16);
                        bsProtItem pi = (from u in heatingdb.bsProtItem
                                                   where u.StartRegAddress == addr
                                                   select u).FirstOrDefault<bsProtItem>();

                        ModbusCommand.RegStartAddr = 0x1B4;// Convert.ToInt32(Addr);//首地址16进制
                        ModbusCommand.RegOpNum = 0x1EF - 0x1B4 + 1;
                        sendData = new byte[ModbusCommand.RegOpNum * 2];
                        
                        HrzRunCurve hrc=hrzda.SelectRunCurve(Guid.Parse(args[0].ToString()));
                        //需要根据参数构造曲线
                        HjHrzControlCurve hcc = new HjHrzControlCurve();
                        sendData =  hcc.Create(hrc); 
                    }
                }
                else if (Convert.ToInt32(Addr) >= 0x280 && Convert.ToInt32(Addr) <= 0x2C4) //报警
                {
                    ModbusCommand.RegStartAddr = Convert.ToInt32(Addr);

                    if (Convert.ToInt32(Addr) == 0x280 && args[1] == null && args[2] == null) //读 
                    {
                        HjHrzAlarmData BMObj = new HjHrzAlarmData();
                        cmd2send = BMObj.CreateReadCommand(cmd2send.CommNo);
                        BMObj = null;
                        return cmd2send;
                    }
                    else
                    {
                        ModbusCommand.RegOpNum = 4;
                        sendData = new byte[ModbusCommand.RegOpNum * 2];
                        buff = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref buff);
                        Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                        buff = BitConverter.GetBytes(Convert.ToSingle(args[1])); CrossHiLow(ref buff);
                        Buffer.BlockCopy(buff, 0, sendData, 4, 4);
                    }
                        
                }
                else if (Convert.ToInt32(Addr) >= 0x200 && Convert.ToInt32(Addr) < 0x280)  //量程
                {
                    ModbusCommand.RegStartAddr = Convert.ToInt32(Addr);
                    if (Convert.ToInt32(Addr) == 0x200 && args[1] == null && args[2] == null) //读 
                    {
                        HjHrzRangeData BMObj = new HjHrzRangeData();
                        cmd2send = BMObj.CreateReadCommand(cmd2send.CommNo);
                        BMObj = null;
                        return cmd2send;
                    }
                    else
                    {
                        ModbusCommand.RegOpNum = 4;
                        sendData = new byte[ModbusCommand.RegOpNum * 2];
                        buff = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref buff);
                        Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                        buff = BitConverter.GetBytes(Convert.ToSingle(args[1])); CrossHiLow(ref buff);
                        Buffer.BlockCopy(buff, 0, sendData, 4, 4);
                    }

                }
                else //if (Convert.ToInt32(Addr,16) >= 0x200)
                {
                    //control2
                    using (WSExpressEntities heatingdb = new WSExpressEntities())
                    {
                        string addr = "0x0" + Convert.ToString(Convert.ToInt32(Addr), 16);
                        bsProtItem pi = (from u in heatingdb.bsProtItem
                                                   where u.StartRegAddress == addr
                                                   select u).FirstOrDefault<bsProtItem>();

                        ModbusCommand.RegStartAddr = Convert.ToInt32(Addr);//首地址16进制
                        ModbusCommand.RegOpNum = (int)pi.RegCount;
                        sendData = new byte[ModbusCommand.RegOpNum * 2];
                        if (pi.DataType == "Unsigned integer" && pi.RegCount == 1)
                        {
                            sendData = BitConverter.GetBytes(Convert.ToUInt16(args[0])); CrossHiLow(ref sendData);

                        }
                        else if (pi.DataType == "Single Float")
                        {
                            sendData = BitConverter.GetBytes(Convert.ToSingle(args[0])); CrossHiLow(ref sendData);
                        }
                        else
                        {
                            log.Error("协议类型[" + pi.DataType + "]没有处理，请统一格式。");
                        }
                    }
                }
            }

            //生成命令数组
            try
            {
                ModbusCommand.byteArr = sendData;
                if (ModbusCommand.byteArr != null)//如果有下发的数据
                {
                    commdarr = ModbusCommand.Command();
                    cmd2send.SendCmd = commdarr;
                }
                if (commdarr != null)
                {
                    StringBuilder strbuilder = new StringBuilder();
                    foreach (byte cmdbyte in commdarr)
                    {
                        strbuilder.Append(cmdbyte.ToString("X2"));
                    }
                    cmd2send.Command = strbuilder.ToString();//01 10 01 72 00 02 04 43 95 80 00 1C AA
                }
            }
            catch (Exception ex)
            {
                throw new Exception("不能生成下发数据");
            }
            return cmd2send;
        }


        public DeviceCmd CreateCmdForDownKD(Guid orgid, float kd)
        {
            DTUProduct d = hrzda.SelectHrzGprsDevice(orgid);
            byte[] commdarr = new byte[9];
            DeviceCmd cmd2send;
            CommNumber siminfo;
            cmd2send = new DeviceCmd();
            try
            {
                cmd2send.CommNo = d.CommNo;

                string[] sets = devda.SelectCommProtocalByCode(d.CPCode.ToString()).CPDesp.Split(new char[] { ',', ';' });
                ModbusCommand.VerifyMode = ModbusCommand.RTU;
                ModbusCommand.Slaveaddr = Convert.ToByte(sets[0]);
                ModbusCommand.OperatMode = 0x10;
                ModbusCommand.RegStartAddr = 0x12;//首地址16进制
                ModbusCommand.RegOpNum = 2;

                byte[] sendData = BitConverter.GetBytes(kd); CrossHiLow(ref sendData);
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

        public DeviceCmd CreateCmdForDownAutoStatus(Guid orgid, short autostatus)
        {
            DTUProduct d = hrzda.SelectHrzGprsDevice(orgid);
            byte[] commdarr = new byte[9];
            DeviceCmd cmd2send = new DeviceCmd();
            CommNumber siminfo;
            try
            {
                 cmd2send = new DeviceCmd();
                cmd2send.CommNo = d.CommNo;

                string[] sets = devda.SelectCommProtocalByCode(d.CPCode.ToString()).CPDesp.Split(new char[] { ',', ';' });
                ModbusCommand.VerifyMode = ModbusCommand.RTU;
                ModbusCommand.Slaveaddr = Convert.ToByte(sets[0]);
                ModbusCommand.OperatMode = 0x10;
                ModbusCommand.RegStartAddr = 0x26;//首地址16进制
                ModbusCommand.RegOpNum = 1;//字节数量10进制

                byte[] sendData = BitConverter.GetBytes(autostatus); CrossHiLow(ref sendData);
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
        public DeviceCmd CreateCmdForDownParaPI(Guid orgid, float P, float I)
        {
            DTUProduct d = hrzda.SelectHrzGprsDevice(orgid);
            byte[] commdarr = new byte[9];
            DeviceCmd cmd2send = new DeviceCmd();
            CommNumber siminfo;
            try
            {
               
                cmd2send.CommNo = d.CommNo;

                string[] sets = devda.SelectCommProtocalByCode(d.CPCode.ToString()).CPDesp.Split(new char[] { ',', ';' });
                ModbusCommand.VerifyMode = ModbusCommand.RTU;
                ModbusCommand.Slaveaddr = Convert.ToByte(sets[0]);
                ModbusCommand.OperatMode = 0x10;
                ModbusCommand.RegStartAddr = 0x14;//首地址16进制
                ModbusCommand.RegOpNum = 4;//字节数量10进制

                byte[] sendData = new byte[8];
                byte[] buff = BitConverter.GetBytes(P); CrossHiLow(ref buff);
                Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                buff = BitConverter.GetBytes(I); CrossHiLow(ref buff);
                Buffer.BlockCopy(buff, 0, sendData, 4, 4);

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
                WriteError("downParaPI", ex);
            }
            return cmd2send;
        }
        public DeviceCmd CreateCmdForDownCurveScale(Guid orgid, float scale)
        {
            DTUProduct d = hrzda.SelectHrzGprsDevice(orgid);
            byte[] commdarr = new byte[9];
            DeviceCmd cmd2send = new DeviceCmd();
            try
            {
                cmd2send.CommNo = d.CommNo;

                string[] sets = devda.SelectCommProtocalByCode(d.CPCode.ToString()).CPDesp.Split(new char[] { ',', ';' });
                ModbusCommand.VerifyMode = ModbusCommand.RTU;
                ModbusCommand.Slaveaddr = Convert.ToByte(sets[0]);
                ModbusCommand.OperatMode = 0x10;
                ModbusCommand.RegStartAddr = 0x20;//首地址16进制
                ModbusCommand.RegOpNum = 2;//字节数量10进制

                if (scale == 0)
                {
                    log.Error("不对，数据为0了");
                    scale = 1;
                }
                byte[] buff = BitConverter.GetBytes(scale);
                CrossHiLow(ref buff);

                ModbusCommand.byteArr = buff;

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
                WriteError("downcurveScale", ex);
            }
            return cmd2send;
        }
        public DeviceCmd CreateCmdForDownCurveOffset(Guid orgid, float offset)
        {
            DTUProduct d = hrzda.SelectHrzGprsDevice(orgid);
            byte[] commdarr = new byte[9];
            DeviceCmd cmd2send = new DeviceCmd();
            try
            {
               
                cmd2send.CommNo = d.CommNo;

                string[] sets = devda.SelectCommProtocalByCode(d.CPCode.ToString()).CPDesp.Split(new char[] { ',', ';' });
                ModbusCommand.VerifyMode = ModbusCommand.RTU;
                ModbusCommand.Slaveaddr = Convert.ToByte(sets[0]);
                ModbusCommand.OperatMode = 0x10;
                ModbusCommand.RegStartAddr = 0x22;//首地址16进制
                ModbusCommand.RegOpNum = 2;//字节数量10进制

                byte[] buff = BitConverter.GetBytes(offset);
                CrossHiLow(ref buff);

                ModbusCommand.byteArr = buff;

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
                WriteError("downcurveScale", ex);
            }
            return cmd2send;
        }
        public DeviceCmd CreateCmdForDownCurvePoints(Guid orgid, Guid hrzruncureveid)
        {
            //this.ucTvDevice1.OrgId; 
            //curs[cboCurve.SelectedIndex].HrzRunCurveID
            //创建命令
            DTUProduct d = hrzda.SelectHrzGprsDevice(orgid);
            byte[] commdarr = new byte[9];
            DeviceCmd cmd2send = new DeviceCmd();
            HrzCommGath hcg=hrzda.SelectCommGath(orgid);
            int autocontrolflag=0;
            if (hcg!=null)
                autocontrolflag=(int)hcg.ControlStatus;

            try
            {
                cmd2send.CommNo =d.CommNo;

                string[] sets = devda.SelectCommProtocalByCode(d.CPCode.ToString()).CPDesp.Split(new char[] { ',', ';' });
                ModbusCommand.VerifyMode = ModbusCommand.RTU;
                ModbusCommand.Slaveaddr = Convert.ToByte(sets[0]);
                ModbusCommand.OperatMode = 0x10;
                ModbusCommand.RegStartAddr = 0x24;//首地址16进制
                ModbusCommand.RegOpNum = 63;//字节数量10进制

                byte[] sendData = DownCurves(hrzruncureveid, autocontrolflag);
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
                WriteError("downCurvePoint:", ex);
            }
            return cmd2send;
        }
        
        /// <summary>
        /// 用于万德协议，由于不在回读，所以下发曲线中的控制标志不再使用，直接固定为自动控制，标志为1
        /// </summary>
        /// <param name="HrzRunCurveid"></param>
        /// <param name="controlflag"></param>
        /// <returns></returns>
        private byte[] DownCurves(Guid HrzRunCurveid, int controlflag)
        {
            //2015-11-18修改 begin
            controlflag = 1;
            //2015-11-18修改 end
            
            int qxwd;
            byte[] data = new byte[126];
            byte[] buff;
            HrzRunCurve hrc = hrzda.SelectRunCurve(HrzRunCurveid);
            int byteindex = 0;
            buff = new byte[2];
            buff = BitConverter.GetBytes((short)hrc.CurevePointCount);   CrossHiLow(ref buff);
            Buffer.BlockCopy(buff, 0, data, byteindex, 2);

            //预留字节
            //buff = BitConverter.GetBytes((short)HrzRunCurveid); CrossHiLow(ref buff);
            //Buffer.BlockCopy(buff, 0, data, byteindex += 2, 2);
            //电动调节阀自控标志
           
            buff = BitConverter.GetBytes((short)controlflag);
            CrossHiLow(ref buff);
            Buffer.BlockCopy(buff, 0, data, byteindex += 2, 2);

            byteindex = 6;
            string[] wds = hrc.CurveHeatringWDS.Split(new char[] { ',' });
           
            for (int i = 0; i < hrc.CurevePointCount; i++)
            {

                //Buffer.BlockCopy(BitConverter.GetBytes(Convert.ToByte(wd)), 0, data, byteindex, 2); 
                data[byteindex + i * 2] = Convert.ToByte(wds[i]);
                qxwd = (int)(hrc.OutWdMin + hrc.OutWdStep * i);
                if (qxwd >= 0)
                    data[byteindex + i * 2 + 1] = (byte)qxwd;
                else
                {
                    data[byteindex + i * 2 + 1] = Convert.ToByte(256 + qxwd);//128+downHrzcurve_.rc.MInWD + downHrzcurve_.rc.WDStep * i);
                }
                if ((byteindex + i * 2) >= data.Length)//(14 + 120 * 2))
                    break;
            }
            //如果点的数量不足120个，则后续数据补0
            if (hrc.CurevePointCount < 60)
            {
                for (int i = 0; i < (60 - hrc.CurevePointCount)*2; i++)
                {
                    data[data.Length-1 - i] = 0;
                }
            }
            return data;
        }
        public DeviceCmd CreateCmdForDownKDUpDown(Guid orgid, float kdDown, float kdUp)
        {
            DTUProduct d = hrzda.SelectHrzGprsDevice(orgid);
            byte[] commdarr = new byte[9];
            DeviceCmd cmd2send = new DeviceCmd();
          
            try
            {
                cmd2send = new DeviceCmd();
                cmd2send.CommNo = d.CommNo;

                string[] sets = devda.SelectCommProtocalByCode(d.CPCode.ToString()).CPDesp.Split(new char[] { ',', ';' });
                ModbusCommand.VerifyMode = ModbusCommand.RTU;
                ModbusCommand.Slaveaddr = Convert.ToByte(sets[0]);
                ModbusCommand.OperatMode = 0x10;
                ModbusCommand.RegStartAddr = 0x6A;//首地址16进制
                ModbusCommand.RegOpNum = 4;//字节数量10进制

                byte[] sendData = new byte[8];
                byte[] buff = BitConverter.GetBytes(kdUp); CrossHiLow(ref buff);
                Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                buff = BitConverter.GetBytes(kdDown); CrossHiLow(ref buff);
                Buffer.BlockCopy(buff, 0, sendData, 4, 4);

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
                WriteError("downParaPI", ex);
            }
            return cmd2send;

        }


        public DeviceCmd CreateCmdForRangeDown(Guid orgid)
        {
            ParseHrzDownUnitRangeData phdurd = new ParseHrzDownUnitRangeData();
            byte[] senddata=phdurd.CreateRangeData(orgid);

            DTUProduct d = hrzda.SelectHrzGprsDevice(orgid);
            byte[] commdarr = new byte[9];
            DeviceCmd cmd2send = new DeviceCmd();
           
            try
            {
                cmd2send = new DeviceCmd();
                cmd2send.CommNo = d.CommNo;

                string[] sets = devda.SelectCommProtocalByCode(d.CPCode.ToString()).CPDesp.Split(new char[] { ',', ';' });
                ModbusCommand.VerifyMode = ModbusCommand.RTU;
                ModbusCommand.Slaveaddr = Convert.ToByte(sets[0]);
                ModbusCommand.OperatMode = 0x10;
                ModbusCommand.RegStartAddr = 0x6A;//首地址16进制
                ModbusCommand.RegOpNum = 4;//字节数量10进制

           
             
                ModbusCommand.byteArr = senddata;

         
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
                WriteError("downParaPI", ex);
            }
            return cmd2send;
        }
    }
}
