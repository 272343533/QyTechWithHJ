using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunMvcExpress.Dao;

using HeatingDSC.Models;
using HeatingDSC.BLL;
using DAHeating.DataAccess;
using QyTech.Protocal;

namespace HeatingDSC.BLL
{

    public enum ProtocalDataType
    {
        integer = 1, Single_Float, Unsigned_int, Unsigned_integer, Unsigned_long_integer, 压缩BCD码
    }
    /// <summary>
    /// 根据通讯号获取相关内容
    /// </summary>
    public class HjDeviceCommandForReadWrite
    {
        IProduct product=new IProduct();

        //固定
        public DeviceCmd ReadForReadOnly
        {
            get {
                product = new HjHrzReadOnlyData();
                return product.CreateReadCommand("");
            }
        }
        public DeviceCmd ReadForControl1
        {
            get
            {
                product = new HjHrzControlData1();
                return product.CreateReadCommand("");
            }
        }
        public DeviceCmd ReadForControl2
        {
            get
            {
                product = new HjHrzControlData2();
                return product.CreateReadCommand("");
            }
        }
        public DeviceCmd ReadForCurve
        {
            get
            {
                product = new HjHrzControlCurve();
                return product.CreateReadCommand("");
            }
        }
        public DeviceCmd ReadForRange
        {
            get
            {
                product = new HjHrzRangeData();
                return product.CreateReadCommand("");
            }
        }
        public DeviceCmd ReadForAlarm
        {
            get
            {
                product = new  HjHrzAlarmData();
                return product.CreateReadCommand("");
            }
        }


        //根据数据,以下内容：子卿数据库使用

        //根据downset数据，获取相应的id，曲线id
        public DeviceCmd WriteForCurve(object objs)
        {
            product = new HjHrzControlCurve();
            return product.CreateWriteCommand(objs);
        }

        //根据downset数据，获取相应的id，orgid获取
        public DeviceCmd WriteForRange(List<AlarmRangeSet> objs)
        {

            product = new HjHrzRangeData();
            return product.CreateWriteCommand(objs);
        }

        //根据downset数据，获取相应的id，orgid获取
        public DeviceCmd WriteForAlarm(List<AlarmRangeSet> objs)
        {
                product = new HjHrzAlarmData();
                return product.CreateWriteCommand(objs);
        }


        //一个一发
        public DeviceCmd GetCommand(string sendflag,decimal value)
        {
            //构造command
            DeviceCmd cmd = new DeviceCmd();
            
            return cmd;
        }
        public DeviceCmd CreateHandDownCmd(string simno,string Addr,decimal value)
        {
            DeviceCmd cmd2send = new DeviceCmd();
            #region 根据选择生成数据
            try
            {
                //构造 地址和类型的关系
                DabsProtocal dbobj = new DabsProtocal();
                bsProtItem pitem = dbobj.GetHjDataTypeByAddr("0x"+Addr);
                //根据Addr获取value的数据类型，然后向该地址写入相应类型的数据
              
                DeviceDa devda = new DeviceDa();
                List<DetailDevice> devdetails;
                ParseHrzDownUnitRangeData phdurd;

                byte[] sendData = new byte[2];
                byte[] buff;
                byte[] commdarr=new byte[4];
                cmd2send = new DeviceCmd();
                cmd2send.CommNo = simno;

                ModbusCommand.VerifyMode = ModbusCommand.RTU;
                ModbusCommand.Slaveaddr = Convert.ToByte("01");
                ModbusCommand.OperatMode = 0x10;
                ModbusCommand.RegStartAddr =Convert.ToInt32(Addr, 16);//首地址16进制
                ModbusCommand.RegOpNum = (int)pitem.RegCount;
                sendData = new byte[ModbusCommand.RegOpNum * 2];

                if (pitem.DataType==ProtocalDataType.integer.ToString().Replace('_', ' '))
                        sendData = BitConverter.GetBytes(Convert.ToInt16(value));
                else if (pitem.DataType == ProtocalDataType.Single_Float.ToString().Replace('_', ' '))
                {
                     sendData = BitConverter.GetBytes(Convert.ToSingle(value));
                }
                else if (pitem.DataType == ProtocalDataType.Unsigned_int.ToString().Replace('_', ' ') || pitem.DataType == ProtocalDataType.Unsigned_integer.ToString().Replace('_', ' '))
                {
                    sendData = BitConverter.GetBytes(Convert.ToUInt16(value));
                }
                else if (pitem.DataType == ProtocalDataType.Unsigned_long_integer.ToString().Replace('_', ' '))
                {
                    sendData = BitConverter.GetBytes(Convert.ToUInt32(value));
                
                }
                else if (pitem.DataType == ProtocalDataType.压缩BCD码.ToString())
                {
                    sendData=str2Bcd(value.ToString());
                }
                else
                {
                    throw new Exception("未知的数据类型");
                }
                product.CrossHiLowForThread(ref sendData);
                #endregion
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
                        cmd2send.Command = strbuilder.ToString();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("不能生成下发数据("+ex.Message+")");
                }
                return cmd2send;
            }
            catch (Exception ex)
            {
                throw new Exception("不能处理该标志命令:0x" + Addr + "；问题描述:" + ex.Message);
            }
            return cmd2send;
        }

        public byte[] str2Bcd(String asc)
        {
            int len = asc.Length;
            int mod = len % 2;

            if (mod != 0)
            {
                asc = "0" + asc;
                len = asc.Length;
            }

            byte[] abt = new byte[len];
            if (len >= 2)
            {
                len = len / 2;
            }

            byte[] bbt = new byte[len];
            abt = System.Text.Encoding.ASCII.GetBytes(asc);
            int j, k;

            for (int p = 0; p < asc.Length / 2; p++)
            {
                if ((abt[2 * p] >= '0') && (abt[2 * p] <= '9'))
                {
                    j = abt[2 * p] - '0';
                }
                else if ((abt[2 * p] >= 'a') && (abt[2 * p] <= 'z'))
                {
                    j = abt[2 * p] - 'a' + 0x0a;
                }
                else
                {
                    j = abt[2 * p] - 'A' + 0x0a;
                }

                if ((abt[2 * p + 1] >= '0') && (abt[2 * p + 1] <= '9'))
                {
                    k = abt[2 * p + 1] - '0';
                }
                else if ((abt[2 * p + 1] >= 'a') && (abt[2 * p + 1] <= 'z'))
                {
                    k = abt[2 * p + 1] - 'a' + 0x0a;
                }
                else
                {
                    k = abt[2 * p + 1] - 'A' + 0x0a;
                }

                int a = (j << 4) + k;
                byte b = (byte)a;
                bbt[p] = b;
            }
            return bbt;
        }
        
    }
}
