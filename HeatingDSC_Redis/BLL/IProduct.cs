using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QyTech.Communication;
using SunMvcExpress.Dao;
using DAHeating.DataAccess;
using HeatingDSC.Models;
using log4net;
using QyTech.HDGprs;
using QyTech.Protocal;

namespace HeatingDSC.BLL
{

    public delegate void delProtocalDataReceived(string simcardno,object obj);

    public delegate void delHrzCommData(HrzCommGath hcg, int[,] CurvePoints);
    public delegate void delHrzJzUnitData(OnTimeHRZCollectData othcd);
    public delegate void delHrzJzUnitRangeData(HrzGathRange hrg);
 
    public enum GathDataStarAddr
    {
        Hrz = 0x00,
        HrzJzUnit1 = 0x100,
        HrzJzUnit2 = 0x180,
        HrzJzUnit3 = 0x200,
        HrzJzRange1 = 0x200,
        HrzJzRange2 = 0x380,
        HrzJzRange3 = 0x400,
    }
    public enum GathDataRegNum
    {
        Hrz = 0x6E,
        HrzJz = 0x60,
        HrzRange = 0x4C
    }

    public enum AcceptPacketLength_Hj
    {
        Response=8,
        HjCureve = 125,
        HjAlarmGath = 133,
        HjReadWriteData2 = 141,
        HjRangeGath = 221,
        HjReadWriteData1 = 229,
        HjReadGath = 241
        //HjReadWriteGath = 245 //最大值，西门子的最大值
    }

    public enum AcceptPacketLength
    {
        Error=5,
        Response=8,
        Qxy=19,
        HrzUnitRange = 157, //范围 
        HrzUnit=197,//机组
        HrzCommData=225,//原215，现加入开度上下限,地址从0开始+2
        GlfGaoDianChang = 229,
        GlfJinLai=309,
       
        MixHrzUintAndResponse=205,
        MixHrzUnitTwo = 394,
        MixHrzCommUnit = 424,
        MixHrzUnitThree = 591

        
    }

    public enum CommProtocal
    {
        ModBusRTU = 2,
        GprsGlf=3,
        GprsHrz=4,
        GprsQxy=5,
        GprsWdj,
        GprsHrzUnit1,
        GprsHrzUnit2,
        GprsHrzUnit3,
        GprsHrzUnit4,
        GprsHjHrz=29
    }

    public enum ProductType
    {
        
        GlfofJinLaiUpData = 31,//说明自己是谁，数据部分名称，需要与数据库中的名称一致，或者是36位字符串
        GlfOfJinLaiDownRange,
        GlfofGaodianchangUpData,

        HrzUpCommData = 41,//1;00,35;20,34;1A,6 获取所有，下发曲线，下发气象数据
        HrzDownCurveAll,
        HrzDownCurveScale,
        HrzDownCurveOffset,//还没设置
        hrzDownCurveOnly,
        HrzDownWeather,
        HrzUpRunCurveData,
        
        QxyUpData = 51,//1;00,07
        WdjUpData = 61,//4;00,4

        HrzUpUnitData=69,
        HrzDownUnitRangeData,
        HrzUpUnit1Data = 71,//1;120,50
        HrzDownUnit1RangeData,
        HrzUpUnit2Data = 81,///1;1A0,50//1;220,50
        HrzDownUnit2RangeData,
        HrzUpUnit3Data = 91,////1;220,50
        HrzDownUnit3RangeData,

        ResponseData=100,

        GlfSanFengUpData=101,
        GlYiJunLiUpData=102,

        HrzUpUnitRangeData=110,


        HJGathData1 = 20031,
        HJPlcParaControl1,
        HJPlcParaControl2,
        HJPlcParaControlCurve,
        GathRangeData_Hj,
        GathAlarmData_Hj

    }

    public class IProduct
    {
        public static ILog log = log4net.LogManager.GetLogger("IProduct");


        public event delProtocalDataReceived delProtocalDataReceivedhandler;

        public event delHrzCommData delHrzCommonDataHandler;
        public event delHrzJzUnitData delHrzJzUnitDataHandler;
        public event delHrzJzUnitRangeData delHrzJzUnitRangeDataHandler;
 
  
        protected string Simno;//sim卡号
        protected bsOrganize Org;

        protected OrganizeDa orgDa = new OrganizeDa();
        protected GLFDa glfda = new GLFDa();
        protected HrzDa hrzda = new HrzDa();
        protected DeviceDa devda = new DeviceDa();
        protected SimcardDa simda = new SimcardDa();
        //////protected RangeSettingDa rangeDa = new RangeSettingDa();
        public ProductType Producttype;//的类型

        protected DateTime PacketTime;
        protected byte SlaveAddress;
        public byte[] GetData;//实际的数据

        public int RegStartAddr;
        public int RegCount;

        //包的总长度，用于读命令的判断，或者更改读命令。
        public int PacketLength;

        /// <summary>
        /// 常用临时变量
        /// </summary>
        public byte[] buff;

        public virtual void OnHrzCommProgress(HrzCommGath hcg, int[,] CurvePoints)
        {
            if (this.delHrzCommonDataHandler != null)
                this.delHrzCommonDataHandler(hcg, CurvePoints);
        }
        public virtual void OnHrzJzUnitProgress(OnTimeHRZCollectData othcd)
        {
            if (this.delHrzJzUnitDataHandler != null)
                this.delHrzJzUnitDataHandler(othcd);
        }
        public virtual void OnHrzRangeProgress(HrzGathRange hrg)
        {
            if (this.delHrzJzUnitRangeDataHandler != null)
                this.delHrzJzUnitRangeDataHandler(hrg);
        }

        public virtual void OnProtocalDataReceivedProgress(string simcardno,Object obj)
        {
            if (this.delProtocalDataReceivedhandler != null)
                this.delProtocalDataReceivedhandler(simcardno,obj);
        }

        public virtual byte[] Create(object obj)
        {
            return null;
        }
        public virtual int SentData(GPRS_DATA_RECORD recdPtr)
        {
            return 0;
        }
        public virtual int Parse(GPRS_DATA_RECORD recdPtr)
        {
            //根据电话号码确定是组织结构即可
             Org= orgDa.SelectOrganizationBySimCard(recdPtr.m_userid);
             PacketTime = DateTime.Parse(recdPtr.m_recv_date);

             byte[] ReceivedData = new byte[recdPtr.m_data_len];
             Buffer.BlockCopy(recdPtr.m_data_buf, 0, ReceivedData, 0, recdPtr.m_data_len);
            
            //2015-10-28  for thread
             //if (ModbusCommand.Parse(ReceivedData) == 1)
             //{
             //    GetData = ModbusCommand.byteArr;
             //    return 1;
             //}
             //RegStartAddr = ModbusCommand.RegStartAddr;
             //RegCount = ModbusCommand.RegOpNum;
             //GetData = ModbusCommand.byteArr;

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
        public void CrossHiLowForThread(ref byte[] b)
        {
            byte tmp;
            for (int i = 0; i < b.Length/2; i++)
            {
                tmp = b[i];
                b[i] = b[b.Length -1- i];
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
            string str="";
            for (int i = 0; i < b.Length; i++)
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
            return new DeviceCmd();
        }
    
        public DeviceCmd CreateModbusRtuReadCommand(string simno,byte slaveaddr,int operatmode,int regstartaddr,int regopnum)
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
                log.Error("buildcommandsforgather: 卡号为" +simno + "-" + ex.Message);
            }

            return cmd2send;
        }

         /// <summary>
        /// 按标志下发单独下发某些寄存器
        /// </summary>
        /// <param name="simno"></param>
        /// <param name="?"></param>
        /// <param name="writeflag"></param>
        /// <returns></returns>
        public virtual DeviceCmd CreateWriteCommand(object objs)
        {
            return new DeviceCmd();
        }

        public virtual DeviceCmd CreateWriteOneCommand(string simno, string flag, decimal value)
        {
            return new DeviceCmd();
        }
    }
}
