using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using SunMvcExpress.Dao;
using log4net;
using QyTech.HDGprs;
using QyTech.Protocal;
using QyTech.Protocal.Modbus;
using HjCommDA;
using QyTech.Core.BLL;
using SunMvcExpress.Core.BLL;
using QyTech.Core.Refection;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace QyTech.Communication
{
  
    /// <summary>
    /// 数据到达
    /// </summary>
    /// <param name="commno"></param>
    /// <param name="obj"></param>
    public delegate void delProtocalDataReceived(string commno, object obj);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="commno"></param>
    /// <param name="cmds"></param>
    public delegate void delProtocalDeviceCommand(string commno, List<DeviceCmd> cmds);

    //public delegate void delHrzCommData(HrzCommGath hcg, int[,] CurvePoints);
    //public delegate void delHrzJzUnitData(OnTimeHRZCollectData othcd);
    //public delegate void delHrzJzUnitRangeData(HrzGathRange hgr);
    //public delegate void delHrzJzUnitAlarmData(HrzGathAlarm hga);


    public struct DownHrzCurve
    {
        public vwlyHrzAutoDownCurveSetting dcsetting;
        public HrzRunCurve rc;
        public bool IsDown;
    }



    //////public enum GathDataStarAddr
    //////{
    //////    Hrz = 0x00,
    //////    HrzJzUnit1 = 0x100,
    //////    HrzJzUnit2 = 0x180,
    //////    HrzJzUnit3 = 0x200,
    //////    HrzJzRange1 = 0x200,
    //////    HrzJzRange2 = 0x380,
    //////    HrzJzRange3 = 0x400,
    //////}
    //////public enum GathDataRegNum
    //////{
    //////    Hrz = 0x6E,
    //////    HrzJz = 0x60,
    //////    HrzRange = 0x4C
    //////}

    //////public enum AcceptPacketLength_Hj
    //////{
    //////    Response = 8,
    //////    HjCureve = 125,
    //////    HjAlarmGath = 133,
    //////    HjReadWriteData2 = 141,
    //////    HjRangeGath = 221,
    //////    HjReadWriteData1 = 229,
    //////    HjReadGath = 241
    //////    //HjReadWriteGath = 245 //最大值，西门子的最大值
    //////}

    //////public enum AcceptPacketLength
    //////{
    //////    Error = 5,
    //////    Response = 8,
    //////    Qxy = 19,
    //////    HrzUnitRange = 157, //范围 
    //////    HrzUnit = 197,//机组
    //////    HrzCommData = 225,//原215，现加入开度上下限,地址从0开始+2
    //////    GlfGaoDianChang = 229,
    //////    GlfJinLai = 309,

    //////    MixHrzUintAndResponse = 205,
    //////    MixHrzUnitTwo = 394,
    //////    MixHrzCommUnit = 424,
    //////    MixHrzUnitThree = 591


    //////}

    //////public enum CommProtocal
    //////{
    //////    ModBusRTU = 2,
    //////    GprsGlf = 3,
    //////    GprsHrz = 4,
    //////    GprsQxy = 5,
    //////    GprsWdj,
    //////    GprsHrzUnit1,
    //////    GprsHrzUnit2,
    //////    GprsHrzUnit3,
    //////    GprsHrzUnit4,
    //////    GprsHjHrz = 29
    //////}

    //////public enum ProductType
    //////{

    //////    GlfofJinLaiUpData = 31,//说明自己是谁，数据部分名称，需要与数据库中的名称一致，或者是36位字符串
    //////    GlfOfJinLaiDownRange,
    //////    GlfofGaodianchangUpData,

    //////    HrzUpCommData = 41,//1;00,35;20,34;1A,6 获取所有，下发曲线，下发气象数据
    //////    HrzDownCurveAll,
    //////    HrzDownCurveScale,
    //////    HrzDownCurveOffset,//还没设置
    //////    hrzDownCurveOnly,
    //////    HrzDownWeather,
    //////    HrzUpRunCurveData,

    //////    QxyUpData = 51,//1;00,07
    //////    WdjUpData = 61,//4;00,4

    //////    HrzUpUnitData = 69,
    //////    HrzDownUnitRangeData,
    //////    HrzUpUnit1Data = 71,//1;120,50
    //////    HrzDownUnit1RangeData,
    //////    HrzUpUnit2Data = 81,///1;1A0,50//1;220,50
    //////    HrzDownUnit2RangeData,
    //////    HrzUpUnit3Data = 91,////1;220,50
    //////    HrzDownUnit3RangeData,

    //////    ResponseData = 100,

    //////    GlfSanFengUpData = 101,
    //////    GlYiJunLiUpData = 102,

    //////    HrzUpUnitRangeData = 110,


    //////    HJGathData1 = 20031,
    //////    HJPlcParaControl1,
    //////    HJPlcParaControl2,
    //////    HJPlcParaControlCurve,
    //////    GathRangeData_Hj,
    //////    GathAlarmData_Hj

    //////}

    public class IProtocal
    {
        public static ILog log = log4net.LogManager.GetLogger("IProtocal");


        public event delProtocalDataReceived delProtocalDataReceivedhandler;
        public event delProtocalDeviceCommand delProtocalDeviceCommanddhandler;
        
        protected string Simno;//sim卡号
        protected bsOrganize Org;
        protected DTUProduct Dev;

        public bsProtocal bsProtocalObj;
        public List<bsProtItem> bsProtItems;
        public Dictionary<string,bsProtocal> subProtocals;//code,协议
        
        public string Producttype;//的类型

        protected DateTime PacketTime;
        protected byte SlaveAddress;
        protected byte[] GetData;//实际的数据

        protected int RegStartAddr;
        protected int RegCount;

        //包的总长度，用于读命令的判断，或者更改读命令。
        protected int PacketLength;

        //每个包都有一个读命令，如果已经建立，则直接使用,写不需要，因为需要从db中读取数据。
        protected DeviceCmd ReadPacketCommand=new DeviceCmd();

        /// <summary>
        /// 常用临时变量
        /// </summary>
        protected byte[] buff;

        public IProtocal()
        {
            bsProtocalObj = null;
            bsProtItems = null;
            subProtocals = null;
       }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CodingOrDeCoding">0：编码 1：解码</param>
        /// <param name="bsP_Id">编码时的协议族bsP_id，解码时细节bsP_Id</param>
        public IProtocal(int CodingOrDeCoding,int bsP_Id)
        {
            string classname = this.GetType().Name;
            log = log4net.LogManager.GetLogger(classname);
            if (CodingOrDeCoding == 0)
            {
                GetSubProtocalInfo(bsP_Id);
                bsProtocalObj = subProtocals[classname];
            }
            else
            {
                bsProtocalObj = EntityManager<bsProtocal>.GetByPk<bsProtocal>("Id" , bsP_Id);
            }
            bsProtItems = EntityManager<bsProtItem>.GetListNoPaging<bsProtItem>("bsP_Id='" + bsProtocalObj.Id.ToString() + "' and FieldName is not null and DataMemo=1", "StartRegAddress");
           
         }

        public IProtocal(int bsP_Id)
        {
            string classname = this.GetType().Name;
            log = log4net.LogManager.GetLogger(classname);
            try
            {
                //按照协议族处理，编码时
                GetSubProtocalInfo(bsP_Id);
                if (subProtocals.Count>0)
                    bsProtocalObj = subProtocals[classname];
                else
                    //按照协议处理，解码时
                    bsProtocalObj = EntityManager<bsProtocal>.GetByPk<bsProtocal>("Id", bsP_Id);
            }
            catch
            {
                //按照协议处理，解码时
                bsProtocalObj = EntityManager<bsProtocal>.GetByPk<bsProtocal>("Id", bsP_Id);
          
            }
            bsProtItems = EntityManager<bsProtItem>.GetListNoPaging<bsProtItem>("bsP_Id='" + bsProtocalObj.Id.ToString() + "' and FieldName is not null and DataMemo=1", "StartRegAddress");

        }
      
        private void GetSubProtocalInfo(int bsP_Id)
        {
            List<bsProtocal> bsProtocals = EntityManager<bsProtocal>.GetListNoPaging<bsProtocal>("PId='" + bsP_Id + "'", "");
            subProtocals = new Dictionary<string, bsProtocal>();
            foreach (bsProtocal p in bsProtocals)
            {
                try
                {
                    subProtocals.Add(p.Code, p);
                }
                catch { }
            }
        }

        public virtual void OnProtocalDataReceivedProgress(string commnno, Object obj)
        {
            if (this.delProtocalDataReceivedhandler != null)
                this.delProtocalDataReceivedhandler(commnno, obj);
        }
        public virtual void OndelProtocalDeviceCommandProgress(string commnno, List<DeviceCmd> cmds)
        {
            if (this.delProtocalDeviceCommanddhandler != null)
                this.delProtocalDeviceCommanddhandler(commnno, cmds);
        }


       /// <summary>
        ///  把转换为字节数组
       /// </summary>
       /// <param name="obj">要转换的类对象</param>
       /// <returns></returns>
        public virtual byte[] Create(object obj) 
        {
            //依赖于配置数据标准类型的正确，目前提供了把对象转换为单个对象的方式，这里暂时不使用，需要可自行实现
            byte[] bytes = new byte[255];
            try
            {
                //获取数据
             
                int InitAddr = Convert.ToInt32(bsProtocalObj.FromAddr.Substring(2), 16);
                Type type = obj.GetType(); //获取类型
                int arrIndex = 0;
                foreach (bsProtItem pi in bsProtItems)
                {
                    object val;
                    try
                    {
                        System.Reflection.PropertyInfo propertyInfo = type.GetProperty(pi.FieldName); //获取指定名称的属性
                        val = GetValueByReflectionFromDb(obj, pi, propertyInfo);
                        buff = IBytesConverter.ToBytes(val, pi.UnifType);

                        buff.CopyTo(bytes, arrIndex);
                        arrIndex += buff.Length;
                    }
                    catch (Exception ex)
                    {
                        log.Error("IProtocal.Create(" + pi.FieldName + "):" + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("IProtocal.Create:" + ex.Message);
            }
            return bytes;
        }
        public virtual int SentData(GPRS_DATA_RECORD recdPtr)
        {
            return 0;
        }
        public virtual int Parse(GPRS_DATA_RECORD recdPtr)
        {
            ChangeOnLineStatus(recdPtr.m_userid);
            PacketTime = DateTime.Parse(recdPtr.m_recv_date);

            byte[] ReceivedData = new byte[recdPtr.m_data_len];
            Buffer.BlockCopy(recdPtr.m_data_buf, 0, ReceivedData, 0, recdPtr.m_data_len);
            ModbusCommandForThread mct = new ModbusCommandForThread();

            if (mct.Parse(ReceivedData) == 1)
            {
                GetData = mct.byteArr;
                return 1;
            }
            RegStartAddr = mct.RegStartAddr;
            RegCount = mct.RegOpNum;
            GetData = mct.byteArr;
            return 0;
        }

        protected void ChangeOnLineStatus(string simno)
        {
            try
            {

                DTUProduct dtu = EntityManager<DTUProduct>.GetByPk<DTUProduct>("CommNo", simno);
                dtu.OnLine = true;
                dtu.OnLineDt = DateTime.Now;
                EntityManager<DTUProduct>.Modify<DTUProduct>(dtu);
            }
            catch (Exception ex)
            {
                log.Error(simno + "更新在线状态错误ChangeOnLineStatus：" + ex.Message);
            }
        }
        //public override int Parse<T>(GPRS_DATA_RECORD recdPtr)
        //{
        //    if (this.Parse(recdPtr) == 1)
        //        return -1;
        //    //log.Info("Parse:10");
        //    Dev = BaseDABll.GetDtuProduct(recdPtr.m_userid);
        //    Org = BaseDABll.GetOrganize((Guid)Dev.AZWZ);

        //    List<DetailDevice> hrzjzs = BaseDABll.GetDetailDevice(Dev.Id);
        //    bsProtItems = BaseDABll.GetProtocalItem(42);//(int)Dev.bsP_Id);

        //    int resualt = 1;
        //    try
        //    {
        //        HrzGathAlarm obj = new HrzGathAlarm();
        //        obj.GathDt = Convert.ToDateTime(recdPtr.m_recv_date);
        //        obj.Det_Id = (Guid)hrzjzs[0].Id;
        //        obj.bsO_Id = Org.Id;

        //        int bufflen;
        //        Type type = obj.GetType(); //获取类型
        //        int InitAddr = Convert.ToInt32(bsProtItems[0].StartRegAddress.Substring(2), 16);
        //        int ItemAddr;

        //        foreach (bsProtItem pi in bsProtItems)
        //        {
        //            try
        //            {
        //                bufflen = (int)pi.RegCount * 2;
        //                buff = new byte[bufflen];
        //                ItemAddr = Convert.ToInt32(pi.StartRegAddress.Substring(2), 16);
        //                System.Reflection.PropertyInfo propertyInfo = type.GetProperty(pi.FieldName); //获取指定名称的属性

        //                Buffer.BlockCopy(GetData, (ItemAddr - InitAddr) * 2, buff, 0, bufflen);
        //                CrossHiLow(ref buff);

        //                obj = (HrzGathAlarm)GetValueByReflection<HrzGathAlarm>(obj, pi, propertyInfo, buff);
        //            }
        //            catch (Exception ex)
        //            {
        //                log.Error("ReadData.Parse(" + pi.FieldName + "):" + ex.Message);
        //            }
        //        }

        //        using (WSExpressEntities db = new WSExpressEntities())
        //        {
        //            db.AddToHrzGathAlarm(obj);
        //            db.SaveChanges();
        //        }
        //        OnProtocalDataReceivedProgress(recdPtr.m_userid, obj);
        //        obj = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("hrzupunit parse:simno:" + recdPtr.m_userid + ":" + ex.InnerException + "-" + ex.Message);
        //        resualt = -1;
        //    }
        //    return resualt;
        //}








        public void CrossHiLowForThread(ref byte[] b)
        {
            byte tmp;
            for (int i = 0; i < b.Length / 2; i++)
            {
                tmp = b[i];
                b[i] = b[b.Length - 1 - i];
                b[b.Length - 1 - i] = tmp;
            }
        }


        public static void CrossHiLow(ref byte[] b)
        {
            byte tmp;
            for (int i = 0; i < b.Length / 2; i++)
            {
                tmp = b[i];
                b[i] = b[b.Length - 1 - i];
                b[b.Length - 1 - i] = tmp;
            }
        }


        public static string bytes2Hex(byte[] b)
        {
            string str = "";
            for (int i = 0; i < b.Length; i++)
            {
                str += b[i].ToString("X2");
            }
            return str;
        }
        public static string bytes2Hex(byte[] b,int len)
        {
            string str = "";
            for (int i = 0; i < (b.Length > len?len:b.Length); i++)
            {
                str += b[i].ToString("X2");
            }
            return str;
        }

        public string bytes2HexForThread(byte[] b)
        {
            string str = "";
            for (int i = 0; i < b.Length; i++)
            {
                str += b[i].ToString("X2");
            }
            return str;
        }

        protected void WriteError(string desp, Exception ex)
        {
            log.Error(desp + ":" + ex.InnerException + "-" + ex.Message);
        }




        public virtual DeviceCmd CreateReadCommand(string simno)
        {
            if (ReadPacketCommand != null && ReadPacketCommand.CommNo != null && !ReadPacketCommand.CommNo.Equals(""))
            {
                return ReadPacketCommand;
            }
            else
            {
                int fromaddr = Convert.ToInt32(bsProtocalObj.FromAddr.Substring(2, 4), 16);
                int toaddr = Convert.ToInt32(bsProtocalObj.ToAddr.Substring(2, 4), 16);

                int operflag = 0x03;
                if (bsProtocalObj.ParseType == "ModBus_RTU_05")
                    operflag = 0x01;

                return CreateModbusRtuReadCommand(simno, 0x01, operflag, fromaddr, toaddr - fromaddr + 1);
            }
        }

        public DeviceCmd CreateReadCommand(string simno,bsProtocal bsp)
        {
            if (ReadPacketCommand != null && ReadPacketCommand.CommNo != null && !ReadPacketCommand.CommNo.Equals(""))
            {
                return ReadPacketCommand;
            }
            else
            {
                int fromaddr = Convert.ToInt32(bsp.FromAddr.Substring(2,4),16);
                int toaddr = Convert.ToInt32(bsp.ToAddr.Substring(2, 4), 16);
                return CreateModbusRtuReadCommand(simno, 0x01, 0x03, fromaddr, toaddr - fromaddr + 1);
            }
        }

        /// <summary>
        /// 读命令
        /// </summary>
        /// <param name="simno"></param>
        /// <param name="slaveaddr"></param>
        /// <param name="operatmode"></param>
        /// <param name="regstartaddr"></param>
        /// <param name="regopnum"></param>
        /// <returns></returns>
        public DeviceCmd CreateModbusRtuReadCommand(string simno, byte slaveaddr, int operatmode, int regstartaddr, int regopnum)
        {
            byte[] commdarr = new byte[9];
            DeviceCmd cmd2send = new DeviceCmd();
            try
            {

                cmd2send.CommNo = simno;

                ModbusCommand.VerifyMode = ModbusCommand.RTU;
                ModbusCommand.Slaveaddr = slaveaddr;
                ModbusCommand.OperatMode = operatmode;
                ModbusCommand.RegStartAddr = regstartaddr;//首地址16进制
                ModbusCommand.RegOpNum = regopnum;//72+1(最后两个寄存器)+1（从0开始）


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

            }
            catch (Exception ex)
            {
                log.Error("buildcommandsforgather: 卡号为" + simno + "-" + ex.Message);
            }
            cmd2send.NeedSendTime = DateTime.Now;
            if (cmd2send.Command.Substring(0, 4) == "0101"){
                cmd2send.Response = "0101"+ (Convert.ToInt32(cmd2send.Command.Substring(10, 2), 16) /8+1).ToString("X2");
            }
            else if (Convert.ToInt32(cmd2send.Command.Substring(10,2),16)*2>=10)
                cmd2send.Response = "0103"+(Convert.ToInt32(cmd2send.Command.Substring(10,2),16)*2).ToString("X2");
            else
                cmd2send.Response = "01030" + (Convert.ToInt32(cmd2send.Command.Substring(10, 2), 16) * 2).ToString("X");
            return cmd2send;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="simno"></param>
        /// <param name="slaveaddr"></param>
        /// <param name="address"></param>
        /// <param name="buff"></param>
        /// <returns></returns>
        private DeviceCmd CreateModbusRtuWriteCommandByBytes(string simno, byte slaveaddr, int address, byte[] buff)
        {
            byte[] sendData = new byte[2];
            byte[] sendDataTmp = new byte[255];
            byte[] commdarr = new byte[9];


            DeviceCmd cmd2send = new DeviceCmd();
            cmd2send.CommNo = simno;

            ModbusCommand.VerifyMode = ModbusCommand.RTU;
            ModbusCommand.Slaveaddr = slaveaddr;
            ModbusCommand.OperatMode = 0x10;

            ModbusCommand.RegStartAddr = address;//首地址16进制

            ModbusCommand.RegOpNum = buff.Length/2;
         
            try
            {
                ModbusCommand.byteArr = buff;
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
                throw new Exception("不能生成下发数据");
            }
            cmd2send.NeedSendTime = DateTime.Now;
            cmd2send.Response = cmd2send.Command.Substring(0, 12);
            return cmd2send;
        }

        /// <summary>
        ///  最基本的接口，要求数据类型必须完全准确
        /// </summary>
        /// <param name="simno"></param>
        /// <param name="address"></param>
        /// <param name="slaveaddr"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual DeviceCmd CreateModbusRtuWriteCommand(string simno, byte slaveaddr,int address, params object[] args)
        {
            byte[] sendData = new byte[2];
            byte[] sendDataTmp = new byte[255];
            byte[] commdarr = new byte[9];


            int regnum = 0;
            foreach (object para in args)
            {
                if (para == null)
                    break;
                if (para is decimal || para is float)
                {
                    regnum += 2;
                }
                else if (para is short)
                {
                    regnum += 1;
                }
                else if (para is int)
                {
                    regnum += 2;
                }
                else if (para is UInt16)
                {
                    regnum += 1;
                }
                else if (para is UInt32)
                {
                    regnum += 2;
                }
                else
                {
                    regnum += 0;
                }

            }

            ModbusCommand.RegOpNum = regnum;
            sendData = new byte[ModbusCommand.RegOpNum * 2];

            int regpos = 0;
            foreach (object para in args)
            {
                if (para == null)
                    break;
                if (para is decimal || para is float)
                {
                    sendDataTmp = new byte[4];
                    sendDataTmp = BitConverter.GetBytes(Convert.ToSingle(para));
                    CrossHiLow(ref sendDataTmp);
                    sendDataTmp.CopyTo(sendData, regpos * 2);

                    regpos += 2;
                }
                else if (para is short)
                {
                    sendDataTmp = new byte[2];
                    sendDataTmp = BitConverter.GetBytes(Convert.ToInt16(para));
                    CrossHiLow(ref sendDataTmp);
                    sendDataTmp.CopyTo(sendData, regpos * 2);

                    regpos += 1;
                }
                else if (para is UInt16)
                {
                    sendDataTmp = new byte[2];
                    sendDataTmp = BitConverter.GetBytes(Convert.ToUInt16(para));
                    CrossHiLow(ref sendDataTmp);
                    sendDataTmp.CopyTo(sendData, regpos * 2);
                    regpos += 1;
                }
                else if (para is int)
                {
                    sendDataTmp = new byte[4];
                    sendDataTmp = BitConverter.GetBytes(Convert.ToInt32(para));
                    CrossHiLow(ref sendDataTmp);
                    sendDataTmp.CopyTo(sendData, regpos * 2);

                    regpos += 2;
                }
                else if (para is UInt32)
                {
                    sendDataTmp = new byte[4];
                    sendDataTmp = BitConverter.GetBytes(Convert.ToUInt32(para));
                    CrossHiLow(ref sendDataTmp);
                    sendDataTmp.CopyTo(sendData, regpos * 2);
                    regpos += 2;
                }
                else
                {
                    regpos += 2;
                }
            }

            return this.CreateModbusRtuWriteCommandByBytes(simno, slaveaddr, address, sendData);
        }

        /// <summary>
        /// 单独给出的气象数据接口
        /// </summary>
        /// <param name="simno"></param>
        /// <param name="wd"></param>
        /// <param name="rz"></param>
        /// <param name="fs"></param>
        /// <returns></returns>
        public virtual DeviceCmd CreateWeatherDownCommand(string simno, float wd, float rz, float fs)
        {
            return new DeviceCmd();// CreateModbusRtuWriteCommand(simno, InitAddr, 0x01, wd, rz, fs);
        }

        /// <summary>
        /// 根据采集数据直接下发接口
        /// </summary>
        /// <param name="simno"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual DeviceCmd CreateModbusRtuWriteCommandByGath(string simno, object obj)
        {
            return new DeviceCmd();
        }


        /// <summary>
        /// 根据配置数据下发接口
        /// </summary>
        /// <param name="simno"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual DeviceCmd CreateModbusRtuWriteCommandByConf(string simno, object obj)
        {
            return new DeviceCmd();
        }


        /// <summary>
        /// 其它自定义接口扩展方式
        /// </summary>
        /// <param name="simno"></param>
        /// <returns></returns>
        public virtual DeviceCmd CreateModbusRtuWriteCommandBySelfDefine(string simno)
        {
            return new DeviceCmd();
        }
 
        /// <summary>
        /// 标准下发转换接口,依赖于配置数据标准类型的正确
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="simno"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected virtual DeviceCmd CreateModbusRtuWriteCommand<T>(string simno, T obj, List<bsProtItem> bsProtItems)
        {
            try
            {
                //获取数据
                DTUProduct Dev = EntityManager<DTUProduct>.GetBySql<DTUProduct>("CommNo='" + simno + "'");
                List<DetailDevice> hrzjzs = BaseDABll.GetDetailDevice(Dev.Id);

                int InitAddr = Convert.ToInt32(bsProtocalObj.FromAddr, 16);
                int ToAddr = Convert.ToInt32(bsProtocalObj.ToAddr, 16);
               
                //获取曲线数据中的最大地址和最小地址
                List<object> lstargs = new List<object>();

                Type type = obj.GetType(); //获取类型

                for(int i=0;i<bsProtItems.Count;i++)
                {
                    bsProtItem pi = bsProtItems[i];
                    object val;
                    try
                    {
                        System.Reflection.PropertyInfo propertyInfo = type.GetProperty(pi.FieldName); //获取指定名称的属性
                        val = GetValueByReflectionFromDb(obj, pi, propertyInfo);
                        
                        lstargs.Add(val==null?0:val);

                        int CurAddr=Convert.ToInt16(pi.StartRegAddress.Substring(2),16);
                        if (i < bsProtItems.Count-1)
                        {
                            int NextAddr = Convert.ToInt16(bsProtItems[i + 1].StartRegAddress.Substring(2), 16);
                            if (NextAddr - CurAddr != pi.RegCount)
                            {
                                for (int p = CurAddr + (int)pi.RegCount; p < NextAddr; p++)
                                {
                                    lstargs.Add((short)0);
                                }
                            }
                        }
                     
                    }
                    catch (Exception ex)
                    {
                        log.Error("IProtocal.CreateModbusRtuWriteCommand(" + pi.FieldName + "):" + ex.Message);
                    }
                }
                object[] args = lstargs.ToArray();
                
                return CreateModbusRtuWriteCommand(simno, 0x01,InitAddr, args);
 
            }
            catch (Exception ex)
            {
                log.Error("CreateModbusRutWriteCommand:" + ex.Message);

            }
            return new DeviceCmd();
        }
 

        public static object SetValueByReflectionFromBytes<T>(T obj, bsProtItem pi, PropertyInfo propertyInfo, byte[] buff)
        {

            try
            {
                object val = IBytesConverter.FromBytes(buff, pi.UnifType);
                val = GetFactVal(val, pi.Memo);
                obj = (T)BaseFunc.SetValue<T>(obj, propertyInfo, val);
                 
                //if (BaseFunc.IsType(propertyInfo.PropertyType, "System.Decimal"))
                //{
                //    if (val.ToString() != "")
                //        propertyInfo.SetValue(obj, Decimal.Parse(val.ToString()), null);
                //    else
                //        propertyInfo.SetValue(obj, new Decimal(0), null);

                //}
                //else
                //    propertyInfo.SetValue(obj, val, null); //给对应属性赋值
            }
            catch (Exception ex)
            {
                log.Error("SetValueByReflection:" + ex.Message + "(" + propertyInfo .Name+ ")");
                throw ex;
            }

            return obj;
         
        }

        /// <summary>
        /// 有的参数解码后要乘以或除一个系数，才是真正的值，也就是用整形代替浮点型，或更大范围的数据
        /// </summary>
        /// <param name="val"></param>
        /// <param name="subOrMul"></param>
        /// <returns></returns>
        private static object GetFactVal(object val, string subOrMul)
        {
            try
            {
                if (subOrMul != null && subOrMul.Length > 1)
                {
                    string mulOrDiv = subOrMul.Substring(0, 1);
                    Decimal sub = Convert.ToDecimal(subOrMul.Substring(1));
                    if (mulOrDiv == "/")
                        val = Convert.ToUInt16(val) / sub;
                    else
                        val = Convert.ToUInt16(val) * sub;

                }
            }
            catch { }
            return val;
        }


        public static object GetValueByReflectionFromDb(object obj, bsProtItem pi, PropertyInfo propertyInfo)
        {
            try
            {
                object val = propertyInfo.GetValue(obj);
                return  IBytesConverter.ToRightType(val, pi.UnifType);
            }
            catch (Exception ex)
            {
                log.Error("GetValueByReflection:" + ex.Message);
                return null;
            }
        }
    }
}
