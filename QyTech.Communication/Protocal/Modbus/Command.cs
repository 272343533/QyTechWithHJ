using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace QyTech.Protocal.Modbus
{
    public class Command
    {
        static private int protocol_DM;//协议
        static private int addr;//丛机开始地址 1
        static private string commStr;//命令串
        static private int retrunstr;
        static private bool decodeFlag;
        static private int xM_DM;
        static private string dataArr;
        static private int datanum;//

        public static int Protocol_DM
        {
            get { return protocol_DM; }
            set {protocol_DM=value; }
        }
        
        public static int Addr
        {
            get { return addr; }
            set { addr = value; }
        }

        public static string CommStr
        {
            get { return commStr; }
            set { commStr = value; }
        }

        public static int Retrunstr
        {
            get { return retrunstr; }
            set { retrunstr = value; }
        }

        public static bool DecodeFlag
        {
            get { return decodeFlag; }
            set { decodeFlag = value; }
        }

        public static int XM_DM
        {
            get { return xM_DM; }
            set { xM_DM = value; }
        }

        public static string DataArr
        {
            get { return dataArr; }
            set { dataArr = value; }
        }

        public static int Datanum
        {
            get { return datanum; }
            set { datanum = value; }
        }

        /// <summary>
        /// 构建数据轮询指令
        /// </summary>
        /// <returns>返回四种格式命令之一，分别为：新科自定义协议/新科Modbus/盂县Modbus/郑州Modbus</returns>
        /// 注：modbus的寄存器数应该设在数据库里面！
        public static byte[] CommandCode()
        {
            ModbusCommand.Slaveaddr = 1;
            ModbusCommand.RegStartAddr = 1;
            ModbusCommand.RegOpNum = protocol_DM;

            ModbusCommand.OperatMode = 0x03;
            ModbusCommand.VerifyMode = ModbusCommand.RTU;
            ModbusCommand.Slaveaddr = (byte)addr;
            ModbusCommand.RegStartAddr = 0;
            return ModbusCommand.Command();
        }




    }
}
