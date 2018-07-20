using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSC_FTHj1
{

    public enum CommControlType { ReadSBRB=0,ReadFM,CheckDt,CommonOpenFM,CommonCloseFM}



    public class FTHj1
    {
        
        //用位数据来表示采集的数据项
        //0：热表，水表 0C4003
        //读阀门数据    0C2003
        //校时          048001
        //普通开单阀门  130200
        //普通关单个阀门



        private Dictionary<CommControlType, string> CommControlWord = new Dictionary<CommControlType,string>();

        private void FHJj1()
        {
            CommControlWord.Add(CommControlType.ReadSBRB, "0C4003");
            CommControlWord.Add(CommControlType.ReadFM, "0C2003");
            CommControlWord.Add(CommControlType.CheckDt, "048001");
            CommControlWord.Add(CommControlType.CommonOpenFM, "130200");
            CommControlWord.Add(CommControlType.CommonCloseFM, "134000");

        }


       
    }




    public class ReadSBRB : IPacket
    {
        #region 3.1

        public ReadSBRB()
        {
            CommFlag = "0C4003";
        }

        public override byte[] Create(string data)
        {
            byte[] bytes = new byte[8+17+data.Length];
            int ipos = 0;
            bytes[0]=0x68; 
            // 长度
            bytes[1] = (byte)(12 + data.Length / 2);// 0x3C;
            bytes[2]=0x00;
            bytes[3] = bytes[1];
            bytes[4] = bytes[2];

            bytes[ipos]=0x68;

            bytes[ipos]=0x41;

            bytes[ipos]=0x02;
            bytes[ipos]=0x37;
            bytes[ipos]=0x01;
            bytes[ipos]=0x00;
            bytes[ipos]=0x00;

            bytes[ipos] = (byte) Convert.ToInt16(CommFlag.Substring(0, 2),16);// 0x0C;
            bytes[ipos]=0xE0;
            bytes[ipos]=0x00;
            bytes[ipos]=0x00;

            bytes[ipos] = (byte)Convert.ToInt16(CommFlag.Substring(2, 2), 16);// 0x40;
            bytes[ipos] = (byte)Convert.ToInt16(CommFlag.Substring(4, 2), 16);// 0x03;
           
            //数据域
            for (int i = 0; i < data.Length; i += 2)
            {
                bytes[ipos] = (byte)Convert.ToInt16(data.Substring(i,2),16);
            }
            //bytes[ipos] = 0x12;
            //bytes[ipos] = 0x00;
            //bytes[ipos]=0x00;
            //bytes[ipos]=0x00;
            //bytes[ipos]=0x00;

            //校验码
            bytes[ipos] = GetCheckValue(bytes);// 0xCC;
            //结束符
            bytes[ipos] = 0x16;

            return bytes;
        }

        //
        public override void Parse(byte[] bytes)
        {
            base.Parse(bytes);

            //解析为对象
           

            //并直接保存到数据中
        }
        #endregion
    }
}
