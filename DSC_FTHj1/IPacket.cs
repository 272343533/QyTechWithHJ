using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSC_FTHj1
{
    public class IPacket
    {

        public int pLength = 0;//数据长度
        public byte[] Addr=new byte[5]; //地址域
        public byte AFN;
        public byte SEQ;
        public byte[] InfoDesp=new byte[2];//信息点
        public byte[] InfoType=new byte[2];//信息类
        public byte[] Data;//数据
        public byte CheckByte;//校验位


        public string CommFlag = "";




        public virtual byte[] Create(int Plength, string data)
        {
            byte[] bytes = new byte[2];

            return bytes;
        }

        //
        public virtual void Parse(byte[] bytes)
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


    }
}
