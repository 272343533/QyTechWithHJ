using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPLibrary.Abstracts;

using log4net;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace DSC_FTHj1
{
    public class ZdFT_Packet 
    {
        public static ILog log = log4net.LogManager.GetLogger("ZdFT_Packet");


        public DateTime TryDownDt;//下发时间


        public DateTime SetDownDt;//命令设定时间
        public string CommFlag = "";
        protected string FunDesp = "";
        public string DtuNo;//dtu标识id
       

        public string StrRawData = "";
        public byte[] RawData;

        public int pLength = 0;//数据长度
        public byte[] Addr=new byte[5]; //地址域
        public byte AFN;
        public byte SEQ;
        public byte[] InfoDesp=new byte[2];//信息点
        public byte[] InfoType=new byte[2];//信息类
        public byte[] Data;//数据
        public string strData;//字符串格式的数据
        public byte CheckByte;//校验位


        public string ErrorFlag;//004000的错误码
        
        
    
        private SortedDictionary<string, string> _ProductsFlag2Desp = new SortedDictionary<string, string>();

        public SortedDictionary<string, string> ProductFlag2Desp
        {
            get
            {
                return _ProductsFlag2Desp;
            }
        }


        public ZdFT_Packet()
        {
            _ProductsFlag2Desp.Add("0C4003", "热表数据读取");//1
            _ProductsFlag2Desp.Add("0C2003", "阀门数据读取");//2
            _ProductsFlag2Desp.Add("0C0404", "测温器数据读取");//3 不使用

            _ProductsFlag2Desp.Add("048001", "采集器校时");//4

            _ProductsFlag2Desp.Add("130200", "普通开单个阀门");//5
            _ProductsFlag2Desp.Add("130400", "普通关单个阀门");//6
            _ProductsFlag2Desp.Add("130800", "强开单个阀门");//7
            _ProductsFlag2Desp.Add("131000", "强关单个阀门");//7_1
            _ProductsFlag2Desp.Add("134000", "单个阀门自由模式");//8

            _ProductsFlag2Desp.Add("132000", "远程设置温度");//9
            _ProductsFlag2Desp.Add("130101", "开启/关闭温控器热量显示");//10
            _ProductsFlag2Desp.Add("138002", "解锁/锁定温控面板设置温度");//11
            _ProductsFlag2Desp.Add("130203", "温度设置上下限");//12
            _ProductsFlag2Desp.Add("134001", "广播强开阀门");//13
            _ProductsFlag2Desp.Add("130202", "广播强关阀门");//14
            _ProductsFlag2Desp.Add("130402", "广播阀门自由模式");//15

            _ProductsFlag2Desp.Add("138009", "阀门进入远程控制模式");//16
           
            _ProductsFlag2Desp.Add("13010A", "广播阀门进入远程控制模式");//17
            _ProductsFlag2Desp.Add("138000", "广播远程设置温度");//18

            _ProductsFlag2Desp.Add("130801", "广播温度设置上下限");//19
            _ProductsFlag2Desp.Add("132001", "广播开启/关闭面板热量显示");//20
            _ProductsFlag2Desp.Add("130103", "广播解锁或锁定面板设置温度");//21
            _ProductsFlag2Desp.Add("132008", "阀门进入调试模式");//22
            _ProductsFlag2Desp.Add("134008", "广播阀门进入调试");//23
            _ProductsFlag2Desp.Add("138008", "设置阀门角度");//24
            _ProductsFlag2Desp.Add("130109", "广播设置阀门角度");//25
            _ProductsFlag2Desp.Add("130209", "设置调试模式控制逻辑");//26
            _ProductsFlag2Desp.Add("130409", "广播设置调试模式控制逻辑");//27
            _ProductsFlag2Desp.Add("130809", "设置调试模式设定温度");//28
            _ProductsFlag2Desp.Add("131009", "广播设置调试模式设定温度");//29
            _ProductsFlag2Desp.Add("132009", "设置调试模式控制逻辑及温度");//30
            _ProductsFlag2Desp.Add("134009", "广播设置调试模式控制逻辑及温度");//31

            _ProductsFlag2Desp.Add("050402", "读某批数据");//32
            _ProductsFlag2Desp.Add("050201", "读终端配置");//33
            _ProductsFlag2Desp.Add("050202", "读采集器属性配置");//34

            _ProductsFlag2Desp.Add("011000", "重启采集器");//35

            _ProductsFlag2Desp.Add("040202", "配置文件分割下发");//36
            _ProductsFlag2Desp.Add("550100", "透传传输调试命令");//38
            _ProductsFlag2Desp.Add("000400", "确认帧反馈");//37



            //以下没有说明具体的参数和格式，
            //_ProductsFlag2Desp.Add("138000", "广播强制设温");
            //_ProductsFlag2Desp.Add("132002", "进入强制单个阀门");//没有说明
           // _ProductsFlag2Desp.Add("132009", "设置调试模式控制逻辑及温度");//没有说明

            string classname=this.GetType().Name;
            if (classname != "ZdFT_Packet")
            {
                CommFlag = classname.Substring(classname.Length - 6);
                FunDesp = ProductFlag2Desp[CommFlag];
            }
        }


        /// <summary>
        /// 倒叙创建包，返回包的字节流
        /// </summary>
        /// <param name="data"> 数据域</param>
        /// <returns></returns>
        public virtual byte[] Create(string data)
        {
            RawData = new byte[20 + data.Length / 2];
            int ipos = 0;
            RawData[0] = 0x68;
            // 长度
            RawData[1] = (byte)(12 + data.Length / 2);// 0x3C;
            RawData[2] = 0x00;
            RawData[3] = RawData[1];
            RawData[4] = RawData[2];

            ipos = 5;
            RawData[ipos++] = 0x68;

            RawData[ipos++] = 0x41;

            RawData[ipos++] = 0x02;
            RawData[ipos++] = 0x37;
            RawData[ipos++] = 0x01;
            RawData[ipos++] = 0x00;
            RawData[ipos++] = 0x00;

            RawData[ipos++] = (byte)Convert.ToInt16(CommFlag.Substring(0, 2), 16);// 0x0C;
            RawData[ipos++] = 0xE0;
            RawData[ipos++] = 0x00;
            RawData[ipos++] = 0x00;

            RawData[ipos++] = (byte)Convert.ToInt16(CommFlag.Substring(2, 2), 16);// 0x40;
            RawData[ipos++] = (byte)Convert.ToInt16(CommFlag.Substring(4, 2), 16);// 0x03;

            //数据域
            StrRawData = "";
           // for (int i = 0; i < data.Length; i += 2)
            //{
            //    RawData[ipos++] = (byte)Convert.ToInt16(data.Substring(i, 2), 16);
           // }
             for (int i =  data.Length-2; i >=0; i -= 2) //倒序
            {
                RawData[ipos++] = (byte)Convert.ToInt16(data.Substring(i, 2), 16);
                StrRawData += data.Substring(i, 2);
            }
            //bytes[ipos] = 0x12;
            //bytes[ipos] = 0x00;
            //bytes[ipos]=0x00;
            //bytes[ipos]=0x00;
            //bytes[ipos]=0x00;

            //校验码
            RawData[ipos++] = GetCheckValue(RawData);// 0xCC;
            //结束符
            RawData[ipos++] = 0x16;

           
            return RawData;
        }

        /// <summary>
        /// 倒叙创建包，返回包的字节流
        /// </summary>
        /// <param name="data"> 数据域</param>
        /// <returns></returns>
        public virtual byte[] CreateDesc(string data)
        {
            return Create(data);
        }
        public virtual byte[] CreateAsc(string data)
        {
            RawData = new byte[20 + data.Length / 2];
            int ipos = 0;
            RawData[0] = 0x68;
            // 长度
            RawData[1] = (byte)(12 + data.Length / 2);// 0x3C;
            RawData[2] = 0x00;
            RawData[3] = RawData[1];
            RawData[4] = RawData[2];

            ipos = 5;
            RawData[ipos++] = 0x68;

            RawData[ipos++] = 0x41;

            RawData[ipos++] = 0x02;
            RawData[ipos++] = 0x37;
            RawData[ipos++] = 0x01;
            RawData[ipos++] = 0x00;
            RawData[ipos++] = 0x00;

            RawData[ipos++] = (byte)Convert.ToInt16(CommFlag.Substring(0, 2), 16);// 0x0C;
            RawData[ipos++] = 0xE0;
            RawData[ipos++] = 0x00;
            RawData[ipos++] = 0x00;

            RawData[ipos++] = (byte)Convert.ToInt16(CommFlag.Substring(2, 2), 16);// 0x40;
            RawData[ipos++] = (byte)Convert.ToInt16(CommFlag.Substring(4, 2), 16);// 0x03;

            //数据域
            StrRawData = "";
            for (int i = 0; i < data.Length; i += 2)
            {
                RawData[ipos++] = (byte)Convert.ToInt16(data.Substring(i, 2), 16);
            }
            
            //bytes[ipos] = 0x12;
            //bytes[ipos] = 0x00;
            //bytes[ipos]=0x00;
            //bytes[ipos]=0x00;
            //bytes[ipos]=0x00;

            //校验码
            RawData[ipos++] = GetCheckValue(RawData);// 0xCC;
            //结束符
            RawData[ipos++] = 0x16;


            return RawData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns>返回0表示正确，1:否则校验错误;2:广播，不一定返回，所以不再处理；6位数字，表示需要进一步解析的内容</returns>
        public virtual int Parse(byte[] bytes)
        {
            //数据长度
            byte[] lenbytes = new byte[2];
            lenbytes[0] = bytes[1]; lenbytes[1] = bytes[2];
            pLength = Convert.ToInt16(lenbytes);


            AFN = bytes[12];
            SEQ = bytes[13];

            InfoDesp[0] = bytes[14];
            InfoDesp[1] = bytes[15];
            InfoType[0] = bytes[16];
            InfoType[1] = bytes[17];


            Data = new byte[pLength - 12];
            for (int i = 0; i < Data.Length; i++)
            {
                Data[i] = bytes[i + 17];
            }
            CheckByte = bytes[bytes.Length - 2];

            if (CheckByte == GetCheckValue(bytes))
            {
                return 1;
            }
            return 0;
        }

        //第7个字节（1开始）累加到校验位前
        public byte GetCheckValue(byte[] bytes)
        {
            byte cvalue=0;
            for (int i = 7-1; i < bytes.Length - 2;i++ )
            {
                cvalue += bytes[i];
            }
            return cvalue;
        }


        /// <summary>
        /// bcd码转byte[]，可以直接使用bitconvert.tostring(bytes)
        /// </summary>
        /// <param name="strTemp"></param>
        /// <returns></returns>
        private static Byte[] ConvertFrom(string strTemp)
        {
            try
            {
                if (Convert.ToBoolean(strTemp.Length & 1))//数字的二进制码最后1位是1则为奇数  
                {
                    strTemp = "0" + strTemp;//数位为奇数时前面补0  
                }
                Byte[] aryTemp = new Byte[strTemp.Length / 2];
                for (int i = 0; i < (strTemp.Length / 2); i++)
                {
                    aryTemp[i] = (Byte)(((strTemp[i * 2] - '0') << 4) | (strTemp[i * 2 + 1] - '0'));
                }
                return aryTemp;//高位在前  
            }
            catch(Exception ex)
            {
                log.Error("ConvertFrom:" + ex.Message);
                return null; 
            }
        }

        public static string Reverse(string data,int count=2)
        {
            string hexstr = "";
            string[] strs = new string[data.Length / count];
            for (int i = 0; i < strs.Length; i++)
            {
                strs[i] = data.Substring(i * count, count);
            }
            Array.Reverse(strs);

            for (int i = 0; i < strs.Length; i++)
            {
                hexstr += strs[i];
            }

            return hexstr;
        }


        protected int Parse004000(byte[] bytes)
        {
            try
            {
                byte[] buff = new byte[5]; Buffer.BlockCopy(bytes, 0, buff, 0, 5);
                string commflag = BitConverter.ToString(buff, 0).Replace("-", "");
                CommFlag = commflag.Substring(0, 2) + commflag.Substring(6, 4);

              
                
                buff = new byte[1]; Buffer.BlockCopy(bytes, bytes.Length-2, buff, 0, 1);
                ErrorFlag = buff[0].ToString();

            }
            catch (Exception ex)
            {
                log.Error("Parse004000" + ex.Message);
                return 0;
            }
            return 1;
       
        }
        protected int Parse004000WithRoomId(byte[] bytes)
        {
            try
            {
                byte[] buff = new byte[5]; Buffer.BlockCopy(bytes, 0, buff, 0, 5);
                string commflag = BitConverter.ToString(buff, 0).Replace("-", "");
                CommFlag = commflag.Substring(0, 2) + commflag.Substring(6, 4);

                Buffer.BlockCopy(bytes, 5, buff, 0, 5); Array.Reverse(buff);
                strData = BitConverter.ToString(buff, 0).Replace("-", "");
                //strData = Reverse(strData, 2);

                buff = new byte[1]; Buffer.BlockCopy(bytes, bytes.Length - 2, buff, 0, 1);
                ErrorFlag = buff[0].ToString();

            }
            catch (Exception ex)
            {
                log.Error("Parse004000" + ex.Message);
                return -1;
            }
            log.Info("Parse004000WithRoomId-------------------:" + CommFlag);
            return Convert.ToInt32(CommFlag);

        }


    }
}
