using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeatingDSC.BLL.HjProtocal
{
    public class HexStringToCruve
    {
        /// <summary>
        /// 对鸿觉协议的下发命令解码
        /// </summary>
        /// <param name="DownCommand">下发命令字符串</param>
        /// <param name="curveScale">比例系数</param>
        /// <param name="curveOffset">偏移量</param>
        /// <param name="pxs">室外温度</param>
        /// <param name="pys">目标温度</param>
        public static void ToHjProtocalCurve(string DownCommand, out float curveScale, out float curveOffset, out int[,] CurvePoints)
        {
            int addr = 0;
            byte[] buff = new byte[2];
            byte[] PacketData = GetBytes(DownCommand);
            byte[] GetData=new byte[PacketData.Length-9];
            Buffer.BlockCopy(PacketData, 7, GetData, 0, GetData.Length);
            buff = new byte[2];
            Buffer.BlockCopy(GetData, addr, buff, 0, 2); CrossHiLow(ref buff);
            int curvePointsNum = BitConverter.ToInt16(buff, 0);
            addr = addr + (0x1BC - 0x1B5) * 2;
            buff = new byte[4];
            //比例系数
            Buffer.BlockCopy(GetData, addr += 2, buff, 0, 4); CrossHiLow(ref buff);
            curveScale = BitConverter.ToSingle(buff, 0);
            //偏移量
            Buffer.BlockCopy(GetData, addr += 4, buff, 0, 4); CrossHiLow(ref buff);
            curveOffset = BitConverter.ToSingle(buff, 0);
            
            //坐标点
           
            addr += 4;
            CurvePoints = new int[curvePointsNum, 2];
            for (int i = 0; i < curvePointsNum; i++)
            {
                CurvePoints[i, 0] = GetData[addr + i * 2];
                CurvePoints[i, 1] = GetData[addr + 1 + i * 2];
                if (CurvePoints[i, 1] > 127)
                    CurvePoints[i, 1] -= 256;

                
            }
        }
        public static void ToWdzProtocalCurve(string DownCommand, out int[,] CurvePoints)
        {
            byte[] buff = new byte[2];
            byte[] PacketData = GetBytes(DownCommand);
            byte[] GetData = new byte[PacketData.Length - 15];
            Buffer.BlockCopy(PacketData, 13, GetData, 0, GetData.Length);
            
            int InitAddr=0x27;

            //Buffer.BlockCopy(GetData, (0x20-InitAddr) * 2, buff, 0, 4); CrossHiLow(ref buff);
            //curveScale = BitConverter.ToSingle(buff, 0);
            //Buffer.BlockCopy(GetData, (0x22-InitAddr) * 2, buff, 0, 4); CrossHiLow(ref buff);
            //curveOffset = BitConverter.ToSingle(buff, 0);

            //buff = new byte[2];
            //Buffer.BlockCopy(GetData, (0x24-InitAddr) * 2, buff, 0, 2); CrossHiLow(ref buff);
            //int CurvePntCount = BitConverter.ToInt16(buff, 0);
            ////0x25预留
            //Buffer.BlockCopy(GetData, (0x25-InitAddr) * 2, buff, 0, 2); CrossHiLow(ref buff);
            ////hcg.RunCurveID = BitConverter.ToInt16(buff, 0);
            
            //Buffer.BlockCopy(GetData, (0x26-InitAddr) * 2, buff, 0, 2); CrossHiLow(ref buff);
            //hcg.ControlStatus = BitConverter.ToInt16(buff, 0);
            //曲线点坐标
            CurvePoints = new int[60, 2];
            for (int i = 0; i < 60; i++)// hcg.HrzCGF24; i++)
            {
                CurvePoints[i, 0] = GetData[(0x27 - InitAddr) * 2 + i * 2];
                CurvePoints[i, 1] = GetData[(0x27 - InitAddr) * 2 + 1 + i * 2];
                if (CurvePoints[i, 1] > 127)
                    CurvePoints[i, 1] -= 256;
            }

        }
        //把下发的16进制字符串转换为byte[]   
        public static byte[] GetBytes(string hexString)
        {
            int byteLength = hexString.Length / 2;
            byte[] bytes = new byte[byteLength];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return bytes;
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
    }
}
