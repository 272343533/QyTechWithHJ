using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunMvcExpress.Dao;
using QyTech.HDGprs;


using QyTech.Communication;
namespace HeatingDSC.BLL
{
     public class ParseHrzUpCommData : IProduct
    {
        //采集数据
        //public static bsOrganize ORG=new bsOrganize();
        HrzCommGath hcg;

        public ParseHrzUpCommData()
        {
            Producttype = ProductType.HrzUpCommData;
        }

        public override void OnHrzCommProgress(HrzCommGath hcg, int[,] CurvePoints)
        {
            base.OnHrzCommProgress(hcg, CurvePoints);
        }


        public override int Parse(GPRS_DATA_RECORD recdPtr)
        {
            int resualt = 0;
            try
            {
                HrzCommGath hcg = new HrzCommGath();
                //ORG = Org;
                base.Parse(recdPtr);
                byte[] buff = new byte[2];
                hcg.GathDt = Convert.ToDateTime(recdPtr.m_recv_date);
                hcg.bsO_Id = base.Org.Id;
                Buffer.BlockCopy(GetData, 0, buff, 0, 2); CrossHiLow(ref buff);
                hcg.HrzCGF0 = BitConverter.ToInt16(buff, 0);
                
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x2 * 2, buff, 0, 4); CrossHiLow(ref buff);
                hcg.HrzCGF2 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x4 * 2, buff, 0, 4); CrossHiLow(ref buff);
                hcg.HrzCGF4 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x6 * 2, buff, 0, 4); CrossHiLow(ref buff);
                hcg.HrzCGF6 = (decimal)BitConverter.ToSingle(buff, 0);
                
                buff = new byte[2];
                Buffer.BlockCopy(GetData, 0x8 * 2, buff, 0, 2); CrossHiLow(ref buff);
                hcg.HrzCGF8 = BitConverter.ToInt16(buff, 0);
                Buffer.BlockCopy(GetData, 0x9 * 2, buff, 0, 2); CrossHiLow(ref buff);
                hcg.HrzCGF9 = BitConverter.ToInt16(buff, 0);
                
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0xA * 2, buff, 0, 4); CrossHiLow(ref buff);
                hcg.HrzCGFA = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0xC * 2, buff, 0, 4); CrossHiLow(ref buff);
                hcg.HrzCGFC = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0xE * 2, buff, 0, 4); CrossHiLow(ref buff);
                hcg.HrzCGFE = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x10 * 2, buff, 0, 4); CrossHiLow(ref buff);
                hcg.HrzCGF10 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x12 * 2, buff, 0, 4); CrossHiLow(ref buff);
                hcg.KD = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x14 * 2, buff, 0, 4); CrossHiLow(ref buff);
                hcg.ParaP = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x16 * 2, buff, 0, 4); CrossHiLow(ref buff);
                hcg.ParaI = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x18 * 2, buff, 0, 4); CrossHiLow(ref buff);
                hcg.HrzCGF18 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x1A * 2, buff, 0, 4); CrossHiLow(ref buff);
                hcg.QxyWd = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x1C * 2, buff, 0, 4); CrossHiLow(ref buff);
                hcg.QxyRz = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x1E * 2, buff, 0, 4); CrossHiLow(ref buff);
                hcg.QxyFs = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x20 * 2, buff, 0, 4); CrossHiLow(ref buff);
                hcg.TempScale = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x22 * 2, buff, 0, 4); CrossHiLow(ref buff);
                hcg.TempOffset = (decimal)BitConverter.ToSingle(buff, 0);
                
                buff = new byte[2];
                Buffer.BlockCopy(GetData, 0x24 * 2, buff, 0, 2); CrossHiLow(ref buff);
                hcg.HrzCGF24 = BitConverter.ToInt16(buff, 0);
                //0x25预留
                Buffer.BlockCopy(GetData, 0x25 * 2, buff, 0, 2); CrossHiLow(ref buff);
                hcg.RunCurveID = BitConverter.ToInt16(buff, 0);

                Buffer.BlockCopy(GetData, 0x26 * 2, buff, 0, 2); CrossHiLow(ref buff);
                hcg.ControlStatus = BitConverter.ToInt16(buff, 0);
                //曲线点坐标
                int[,] CurvePoints = new int[60, 2];
                for (int i = 0; i <60;i++)// hcg.HrzCGF24; i++)
                {
                    CurvePoints[i, 0] = GetData[0x27 * 2 + i * 2];
                    CurvePoints[i, 1] = GetData[0x27 * 2 + 1 + i * 2];
                    if (CurvePoints[i, 1] > 127)
                        CurvePoints[i, 1] -= 256;
                }

                //获取开度上下限
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x64 * 2, buff, 0, 4); CrossHiLow(ref buff);
                hcg.HrzCGF64 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x66 * 2, buff, 0, 4); CrossHiLow(ref buff);
                hcg.HrzCGF66 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x68 * 2, buff, 0, 4); CrossHiLow(ref buff);
                hcg.HrzCGF68 = (decimal)BitConverter.ToSingle(buff, 0);
                 Buffer.BlockCopy(GetData, 0x6A * 2, buff, 0, 4); CrossHiLow(ref buff);
                hcg.KDUp = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x6C * 2, buff, 0, 4); CrossHiLow(ref buff);
                hcg.KDDown = (decimal)BitConverter.ToSingle(buff, 0);

                //写入数据库
                try
                {
                    log.Info("frmstart save date is:date" + hcg.GathDt.ToString("yyyy-MM-dd HH:mm:ss.fff") + "-" + hcg.bsO_Id.ToString());
                    hrzda.SaveHrzCommGath(hcg, CurvePoints);
                }
                catch (Exception ex)
                {
                    log.Error("保存HrzComm数据" + ex.InnerException + "-" + ex.Message);
                }


                //if (hcg.TempScale != 0)//CurvePoints[0, 0] != 0 && 
                //{
                    log.Info("parse:date" + recdPtr.m_recv_date + "-" + recdPtr.m_userid + "-" + Org.Id.ToString() + " is " + Org.Name);
                    OnHrzCommProgress(hcg, CurvePoints);
                //}
                //else
                //{
                //    log.Error(" Wrror parse hrzCommGath:date" + recdPtr.m_recv_date + "-" + recdPtr.m_userid + "-" + Org.Id.ToString() + " is " + Org.Name);
                //}

            }
            catch (Exception ex)
            {
                log.Error("hrzupcommdatparse:" + ex.InnerException + "-" + ex.Message);
                return 0;
            }
            return resualt;
        }


      
    }
}


