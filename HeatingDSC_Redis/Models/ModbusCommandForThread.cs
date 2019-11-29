using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
namespace HeatingDSC.Models
{
    public class ModbusCommandForThread
    {
        public static ILog log = log4net.LogManager.GetLogger("modbus");
        public const int RTU=0;
        public const int ASCII=1;

        private const int CRC_LEN = 2;


        /* CRC16 Table High byte */
        static private readonly byte[] _CRCHi = new byte[]
        {
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
        0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
        0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
        0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
        0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
        0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40
        } ;

        /* CRC16 Table Low byte */
        static private readonly byte[] _CRCLo = new byte[]
        {
        0x00, 0xC0, 0xC1, 0x01, 0xC3, 0x03, 0x02, 0xC2,
        0xC6, 0x06, 0x07, 0xC7, 0x05, 0xC5, 0xC4, 0x04,
        0xCC, 0x0C, 0x0D, 0xCD, 0x0F, 0xCF, 0xCE, 0x0E,
        0x0A, 0xCA, 0xCB, 0x0B, 0xC9, 0x09, 0x08, 0xC8,
        0xD8, 0x18, 0x19, 0xD9, 0x1B, 0xDB, 0xDA, 0x1A,
        0x1E, 0xDE, 0xDF, 0x1F, 0xDD, 0x1D, 0x1C, 0xDC,
        0x14, 0xD4, 0xD5, 0x15, 0xD7, 0x17, 0x16, 0xD6,
        0xD2, 0x12, 0x13, 0xD3, 0x11, 0xD1, 0xD0, 0x10,
        0xF0, 0x30, 0x31, 0xF1, 0x33, 0xF3, 0xF2, 0x32,
        0x36, 0xF6, 0xF7, 0x37, 0xF5, 0x35, 0x34, 0xF4,
        0x3C, 0xFC, 0xFD, 0x3D, 0xFF, 0x3F, 0x3E, 0xFE,
        0xFA, 0x3A, 0x3B, 0xFB, 0x39, 0xF9, 0xF8, 0x38,
        0x28, 0xE8, 0xE9, 0x29, 0xEB, 0x2B, 0x2A, 0xEA,
        0xEE, 0x2E, 0x2F, 0xEF, 0x2D, 0xED, 0xEC, 0x2C,
        0xE4, 0x24, 0x25, 0xE5, 0x27, 0xE7, 0xE6, 0x26,
        0x22, 0xE2, 0xE3, 0x23, 0xE1, 0x21, 0x20, 0xE0,
        0xA0, 0x60, 0x61, 0xA1, 0x63, 0xA3, 0xA2, 0x62,
        0x66, 0xA6, 0xA7, 0x67, 0xA5, 0x65, 0x64, 0xA4,
        0x6C, 0xAC, 0xAD, 0x6D, 0xAF, 0x6F, 0x6E, 0xAE,
        0xAA, 0x6A, 0x6B, 0xAB, 0x69, 0xA9, 0xA8, 0x68,
        0x78, 0xB8, 0xB9, 0x79, 0xBB, 0x7B, 0x7A, 0xBA,
        0xBE, 0x7E, 0x7F, 0xBF, 0x7D, 0xBD, 0xBC, 0x7C,
        0xB4, 0x74, 0x75, 0xB5, 0x77, 0xB7, 0xB6, 0x76,
        0x72, 0xB2, 0xB3, 0x73, 0xB1, 0x71, 0x70, 0xB0,
        0x50, 0x90, 0x91, 0x51, 0x93, 0x53, 0x52, 0x92,
        0x96, 0x56, 0x57, 0x97, 0x55, 0x95, 0x94, 0x54,
        0x9C, 0x5C, 0x5D, 0x9D, 0x5F, 0x9F, 0x9E, 0x5E,
        0x5A, 0x9A, 0x9B, 0x5B, 0x99, 0x59, 0x58, 0x98,
        0x88, 0x48, 0x49, 0x89, 0x4B, 0x8B, 0x8A, 0x4A,
        0x4E, 0x8E, 0x8F, 0x4F, 0x8D, 0x4D, 0x4C, 0x8C,
        0x44, 0x84, 0x85, 0x45, 0x87, 0x47, 0x46, 0x86,
        0x82, 0x42, 0x43, 0x83, 0x41, 0x81, 0x80, 0x40
        };
                
        byte slaveaddr;       //从机地址
        int regStartAddr;     //寄存器起始地址
        int regOpNum;        //寄存器操作数量     
        int byteOpNum;       //操作的字节数
        int verifyMode;      //ModBus通讯方式RTU/ASCII
        int operatMode;      //命令操作模式（仅支持读0x03、写0x10）
        //static char[] dataArr;     //数据数组
        
        public int DataLength;  //解析包时的数据长度
        public byte[] Crc;   //解析包时收到的校验

        byte[] byteArr_;
        public byte[] byteArr
        {
            get { return byteArr_; }
            set { byteArr_ = value; }
        }
        public int ByteOpNum
        {
            get { return byteOpNum; }
            set { byteOpNum = value; }
        }
        public byte Slaveaddr       //从机地址
        {
            get { return slaveaddr; }
            set { slaveaddr = value;}
        }


        public int RegStartAddr
        {
            get { return regStartAddr; }
            set { regStartAddr = value; }
        }

        /// <summary>
        /// 寄存器数
        /// </summary>
        public int RegOpNum
        {
            get { return regOpNum; }
            set { regOpNum = value; }
        }

        public int VerifyMode
        {
            get { return verifyMode; }
            set { verifyMode = value; }
        }

        public int OperatMode
        {
            get { return operatMode; }
            set { operatMode = value; }
        }

        //static public char[] DataArr
        //{
        //    get { return dataArr; }
        //}
                
        /// <summary>
        /// Mudbus读写数据指令
        /// </summary>
        /// <returns></returns>
        public byte[] Command()
        {
            byte[] tmpcomm;////=new byte[20];
            byte[] defult;
            try
            {
                switch (operatMode)
                {
                    case 0x03:  //读数据
                        #region 03读数据
                        if (verifyMode == ASCII)//ASCII方式
                        {

                            tmpcomm = new byte[7];
                            tmpcomm[0] = slaveaddr;
                            tmpcomm[1] = 0x03;
                            tmpcomm[2] = (byte)(regStartAddr >> 8);
                            tmpcomm[3] = (byte)(regStartAddr & 0xFF);
                            tmpcomm[4] = (byte)(regOpNum >> 8);
                            tmpcomm[5] = (byte)(regOpNum & 0xFF);
                            tmpcomm[6] = 0;
                            for (int i = 0; i < 6; i++)
                            {
                                tmpcomm[6] += tmpcomm[i];
                            }
                            tmpcomm[6] = (byte)(tmpcomm[6] % 0xFF);
                            tmpcomm[6] = (byte)(~tmpcomm[6] + 1);
                            return tmpcomm;
                        }
                        else if (verifyMode == RTU)//RTU方式
                        {


                            tmpcomm = new byte[8];
                            tmpcomm[0] = slaveaddr;
                            tmpcomm[1] = 0x03;
                            tmpcomm[2] = (byte)(regStartAddr >> 8);
                            tmpcomm[3] = (byte)(regStartAddr & 0xFF);
                            tmpcomm[4] = (byte)(regOpNum >> 8);
                            tmpcomm[5] = (byte)(regOpNum & 0xFF);


                            byte crcHi = 0xff;  // high crc byte initialized
                            byte crcLo = 0xff;  // low crc byte initialized 

                            for (int i = 0; i < 6; i++)
                            {
                                int crcIndex = crcHi ^ tmpcomm[i]; // calculate the crc lookup index

                                crcHi = (byte)(crcLo ^ _CRCHi[crcIndex]);
                                crcLo = _CRCLo[crcIndex];
                            }

                            tmpcomm[6] = crcHi;
                            tmpcomm[7] = crcLo;//交换高低位

                            //tmpcomm[0] = 1;//01 03 00  00  00 01  0A 84 
                            //tmpcomm[1] = 5;
                            //tmpcomm[2] = 0;
                            //tmpcomm[3] = 0;
                            ////tmpcomm[4] = 0;
                            ////tmpcomm[5] = 1;
                            //int a = Convert.ToInt32("D9", 16);

                            //ushort CRCCode = ModbusCommand.CalculateCrc16(tmpcomm, out tmpcomm[6], out tmpcomm[7]);
                            return tmpcomm;
                        }
                        #endregion
                        break;
                    case 0x10:
                        #region 10写数据
                        if (verifyMode == RTU)//RTU方式
                        {
                            //格式：0 从机地址，1指令类型、2开始地址、2数据数量、1字节数量、数据、2校验
                            tmpcomm = new byte[9 + byteArr_.Length];
                            tmpcomm[0] = slaveaddr;
                            tmpcomm[1] = 0x010;//
                            tmpcomm[2] = (byte)(regStartAddr >> 8);
                            tmpcomm[3] = (byte)(regStartAddr & 0xFF);
                            tmpcomm[4] = (byte)(regOpNum >> 8);
                            tmpcomm[5] = (byte)(regOpNum & 0xFF);
                            tmpcomm[6] = (byte)byteArr_.Length;

                            byteArr_.CopyTo(tmpcomm, 7);

                            byte crcHi = 0xff;  // high crc byte initialized
                            byte crcLo = 0xff;  // low crc byte initialized 

                            for (int i = 0; i < 6 + byteArr_.Length; i++)
                            {
                                int crcIndex = crcHi ^ tmpcomm[i]; // calculate the crc lookup index

                                crcHi = (byte)(crcLo ^ _CRCHi[crcIndex]);
                                crcLo = _CRCLo[crcIndex];
                            }
                            CalculateCRCA001(tmpcomm, tmpcomm.Length - 2, out crcHi, out crcLo);


                            tmpcomm[7 + byteArr_.Length] = crcLo;
                            tmpcomm[8 + byteArr_.Length] = crcHi;

                            //ushort CRCCode = ModbusCommand.CalculateCrc16(tmpcomm, out tmpcomm[6], out tmpcomm[7]);
                            return tmpcomm;
                        }
                        #endregion
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error("command:" + ex.InnerException + "-" + ex.Message);
            }
            defult=new byte[1];
            defult[0] = 0xFF;//错误标志
            return defult;
       }

       

        /// <summary>
        /// 解析包数据
        /// </summary>
        /// <param name="packet">收到的字节数据</param>
        /// <returns>0：正常 1：校验不正确</returns>
        public int Parse(byte[] packet)
        {
            byte crchi,crclo;
            try
            {
                slaveaddr = packet[0];
                operatMode = packet[1];
                Crc = new byte[2];
                if (operatMode == 3)
                {
                    DataLength = packet[2];
                    if (DataLength != packet.Length - 5)
                        DataLength = packet.Length - 5;
                    byteArr_ = new byte[DataLength];
                    char c = (char)packet[3];
                    Buffer.BlockCopy(packet, 3, byteArr_, 0, DataLength);
                    Buffer.BlockCopy(packet, 3 + DataLength, Crc, 0, 2);
                    //byte[] crcsource = new byte[DataLength + 3];
                    //Buffer.BlockCopy(packet, 0, crcsource, 0, crcsource.Length);
                    if (!(Crc[0] == 0 && Crc[1] == 0))//没有校验位
                    {
                        ModbusCommand.CalculateCrc16(packet, out crchi, out crclo);
                        if (crchi == Crc[0] && crclo == Crc[1])
                        {
                            //dataArr = System.Text.Encoding.UTF8.GetChars(byteArr_);
                            return 0;
                        }
                        else
                            return 1;
                    }
                }
                else if (operatMode == 16)
                {
                    byte[] buff = new byte[2];
                    buff[0] = packet[3]; buff[1] = packet[2];
                    RegStartAddr = BitConverter.ToInt16(buff, 0);
                    buff[0] = packet[5]; buff[1] = packet[4];
                    regOpNum = BitConverter.ToInt16(buff, 0);

                    Crc = new byte[2];
                    Crc[1] = packet[6]; Crc[0] = packet[7];
                    if (!(Crc[0] == 0 && Crc[1] == 0))//没有校验位
                    {
                        ModbusCommand.CalculateCrc16(packet, out crchi, out crclo);
                        if (crchi == Crc[1] && crclo == Crc[0])
                        {
                            //dataArr = System.Text.Encoding.UTF8.GetChars(byteArr_);
                            return 0;
                        }
                        else
                            return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("parse:"+ex.Message,ex.InnerException);
            }
            return 0;
        }
        
        //计算CRC校验码
        public ushort GetCRC16(byte[] puchMsg, int DataLen,out ushort CRCHi,out ushort CRCLo)
        {
            int Index;
            CRCHi = 0xFF;
            CRCLo = 0xFF;

            int i=0;
            int k = DataLen;
            while(k>=0)
            {
                Index = CRCHi ^ puchMsg[i]; /* calculate the CRC16 */
                CRCHi = (byte)(CRCLo ^ _CRCHi[Index]);
                CRCLo = _CRCLo[Index];
                i++;
                k--;
            }
            return (ushort)(CRCHi << 8 | CRCLo);
        }



        public  ushort CalculateCrc16(byte[] buffer, out byte crcHi, out byte crcLo)
        {
            crcHi = 0xff;  // high crc byte initialized
            crcLo = 0xff;  // low crc byte initialized 

            for (int i = 0; i < buffer.Length - CRC_LEN; i++)
            {
                int crcIndex = crcHi ^ buffer[i]; // calculate the crc lookup index

                crcHi = (byte)(crcLo ^ _CRCHi[crcIndex]);
                crcLo = _CRCLo[crcIndex];
            }

            return (ushort)(crcHi << 8 | crcLo);
        }

        public void CalculateCRC8005(byte[] pByte, out byte hi, out byte lo)
        {
            ushort sum;
            CalculateCRC8005(pByte, out sum);
            lo = (byte)(sum & 0xFF);
            hi = (byte)((sum & 0xFF00) >> 8);
        }

        private void CalculateCRC8005(byte[] pByte, out ushort pChecksum)
        {
            int nBit;
            ushort nShiftedBit;
            pChecksum = 0xFFFF;

            for (int nByte = 0; nByte < pByte.Length; nByte++)
            {
                pChecksum ^= pByte[nByte];
                for (nBit = 0; nBit < 8; nBit++)
                {
                    if ((pChecksum & 0x1) == 1)
                    {
                        nShiftedBit = 1;
                    }
                    else
                    {
                        nShiftedBit = 0;
                    }
                    pChecksum >>= 1;
                    if (nShiftedBit != 0)
                    {
                        pChecksum ^= 0x8005;
                    }
                }
            }
        }
       
        public void CalculateCRCA001(byte[] pByte,int datalength, out byte hi, out byte lo)
        {
            ushort sum;
            CalculateCRCA001(pByte,datalength, out sum);
            lo = (byte)(sum & 0xFF);
            hi = (byte)((sum & 0xFF00) >> 8);
        }

        private void CalculateCRCA001(byte[] pByte, int datalength,out ushort pChecksum)
        {
            int nBit;
            ushort nShiftedBit;
            pChecksum = 0xFFFF;

            for (int nByte = 0; nByte < datalength; nByte++)
            {
                pChecksum ^= pByte[nByte];
                for (nBit = 0; nBit < 8; nBit++)
                {
                    if ((pChecksum & 0x1) == 1)
                    {
                        nShiftedBit = 1;
                    }
                    else
                    {
                        nShiftedBit = 0;
                    }
                    pChecksum >>= 1;
                    if (nShiftedBit != 0)
                    {
                        pChecksum ^= 0xA001;
                    }
                }
            }
        }



        public string ModBusCRC16(string tmpComStr,string poly)
        {
            int Flag;
            ushort crc, genpoly;//Word
            string s;
            //tmpComStr = "01 03 0000 0001 3388";
            s = tmpComStr.Replace(" ", ""); ;
            crc = 0xFFFF;
            genpoly = Convert.ToUInt16(poly, 16);//.ToString();  

            for (int i = 0; i < s.Length; i++)
            {
                crc = (ushort)(crc ^ (ushort)s[i]);

                for (int j = 0; j <= 7; j++)
                {
                    Flag = crc & 0x0001;//记住最低位
                    crc = (ushort)(crc >> 1);
                    if (Flag == 0)
                        continue;
                    else
                        crc = (ushort)(crc ^ genpoly);
                }
            }
            //for (int i = 0; i < s.Length; i++)
            //{
            //    crc = (ushort)(crc ^ (ushort)s[i]);
            //    for (int j = 0; j <= 7; j++)
            //    {
            //        //最右端位是否为0
            //        if ((crc & 0x0001) != 0)//为1
            //            crc = (ushort)((crc >> 1) ^ genpoly);
            //        else//为0
            //            crc = (ushort)(crc >> 1);
            //    }
            //}
            //高低字节互换
            byte[] bytes = BitConverter.GetBytes(crc);
            byte b = bytes[0];
            bytes[0] = bytes[1];
            bytes[1] = b;
            crc = BitConverter.ToUInt16(bytes, 0);
            string str = crc.ToString("x");
            return str;// IntToHex(crc,4);
        }

    }
}
