using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunMvcExpress.Dao;
using QyTech.Communication;
using QyTech.HDGprs;
using QyTech.Protocal;

namespace HeatingDSC.BLL
{
        public delegate void delGetRunCurveParas(float curveScale, float curveOffset, int curvepointnum, int[,] curvepoints);
 
     public class ParseHrzUpRunCurve : IProduct
    {
         public event delGetRunCurveParas GetRunCurveParasHandler;

        public ParseHrzUpRunCurve()
        {
            Producttype = ProductType.HrzUpCommData;
        }
        public override int Parse(GPRS_DATA_RECORD recdPtr)
        {
            int resualt = 0;
            try
            {
                base.Parse(recdPtr);
                resualt = 1;
                int addr = 0;
                byte[] buff = new byte[4];

                Buffer.BlockCopy(GetData, addr, buff, 0, 4); CrossHiLow(ref buff);
                float curveScale = BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, addr += 4, buff, 0, 4); CrossHiLow(ref buff);
                float curveOffset = BitConverter.ToSingle(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, addr += 4, buff, 0, 2); CrossHiLow(ref buff);
                int curvePointsNum = BitConverter.ToInt16(buff, 0);
                addr += 2;
                Buffer.BlockCopy(GetData, addr += 2, buff, 0, 2); CrossHiLow(ref buff);
                int DdtjfAutoContFlag = BitConverter.ToInt16(buff, 0);
                //曲线点坐标
                string pxs="", pys="";
                addr += 2;
                int[,] CurvePoints = new int[curvePointsNum, 2];
                for (int i = 0; i < curvePointsNum; i++)
                {
                    CurvePoints[i, 0] = GetData[addr + i * 2];
                    pxs += "," + CurvePoints[i, 0];
                    CurvePoints[i, 1] = GetData[addr + 1 + i * 2];
                    if (CurvePoints[i, 1] > 127)
                        CurvePoints[i, 1] -= 256;

                    pys += "," + CurvePoints[i, 1];
                }
                if (pxs.Length > 0)
                    pxs = pxs.Substring(1);
                if (pys.Length > 0)
                    pys = pys.Substring(1);
                WatchCurve watcur = hrzda.SelectWatchCurveByHrzId(Org.Id);
                watcur.CurvePointNum = curvePointsNum;
                watcur.CurveX = pxs;
                watcur.CurveY = pys;
                watcur.GatDt = Convert.ToDateTime(recdPtr.m_recv_date);
                hrzda.heatingdb.SaveChanges();
                if (GetRunCurveParasHandler!=null)
                    GetRunCurveParasHandler(curveScale, curveOffset, curvePointsNum, CurvePoints);
            }
            catch (Exception ex)
            {
                log.Error("hrzupcommdatparse:" + ex.InnerException + "-" + ex.Message);
                return 0;
            }
            return resualt;
        }


        //public override DeviceCmd CreateReadCommand(string simno)
        //{
        //    DeviceCmd dc = new DeviceCmd();
        //    dc.CommNo = simno;
        //    dc.Command = "0103";
        //    dc.SendCmd=new byte[2];
        //    return dc;
        //}

        public override DeviceCmd CreateReadCommand(string simno)
        {
            return base.CreateModbusRtuReadCommand(simno, 0x01, 0x03, 0x1B4, 0x1EF + 1 - 0x1B4);
        }
    }
}
