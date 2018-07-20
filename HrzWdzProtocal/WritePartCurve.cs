using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Reflection;
using SunMvcExpress.Dao;
using QyTech.Communication;
using QyTech.Protocal.Modbus;
using QyTech.HDGprs;
using QyTech.Protocal;
using SunMvcExpress.Core.BLL;
using HjCommDA;

namespace QyTech.WdzProtocal
{
    /// <summary>
    /// 目前该函数已经不在使用，改为DownhrzControl class
    /// </summary>
    public class WritePartCurve : IProtocal
    {
        public DownHrzCurve downHrzcurve_;

        public delegate void delGetRunCurveParas(float curveScale, float curveOffset, int curvepointnum, int[,] curvepoints);


        public event delGetRunCurveParas GetRunCurveParasHandler;




        public WritePartCurve(int bsP_Id)
            : base(bsP_Id)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="simno"></param>
        /// <returns></returns>
        public override DeviceCmd CreateModbusRtuWriteCommandByConf(string simno,object obj)
        {
            HrzRunCurve obj_ = (HrzRunCurve)obj;
           
            
            HjPlcParaControlCurve objtarget = new HjPlcParaControlCurve();

            //转换数据
            //objtarget.TempCount = obj_.CurevePointCount;
            //objtarget.TempScale = obj_.CurveScale;
            //objtarget.TempOffset = obj_.CurveOffset;

            string[] wds = obj_.CurveHeatringWDS.Split(new char[] {','});

            Type type = objtarget.GetType();
            int outwd, hwd;
            string val;
            for (int i = 0; i < wds.Length; i += 1)
            {
                outwd = Convert.ToInt16(obj_.OutWdMin + i * obj_.OutWdStep);// Convert.ToInt16(wds[i]);
                hwd = Convert.ToInt16(wds[i]);

                val = outwd.ToString() + "," + hwd.ToString();
                //val = CommFunc.HeatTempToUnifType(outwd.ToString()+","+hwd.ToString());

                System.Reflection.PropertyInfo propertyInfo = type.GetProperty("Temp" + (i + 1).ToString()); //获取指定名称的属性

                propertyInfo.SetValue(objtarget,val, null); //给对应属性赋值
            }
            for (int i = wds.Length; i < 60; i++)
            {
                
                outwd = Convert.ToInt16(obj_.OutWdMin + i * obj_.OutWdStep);// Convert.ToInt16(wds[i]);
              
                hwd = Convert.ToInt16(wds[wds.Length-1]);

                val = outwd.ToString() + "," + hwd.ToString();
            
                System.Reflection.PropertyInfo propertyInfo = type.GetProperty("Temp" + (i + 1).ToString()); //获取指定名称的属性
                propertyInfo.SetValue(objtarget, val, null); //给对应属性赋值

            }
            
            return base.CreateModbusRtuWriteCommand<HjPlcParaControlCurve>(simno, objtarget, bsProtItems);
        }

    

        public override DeviceCmd CreateReadCommand(string simno)
        {
            return base.CreateModbusRtuReadCommand(simno, 0x01, 0x03, 0x027, 0x0062 - 0x0027 + 1);
            //return base.CreateReadCommand(simno);
            //010301BB0036B405
        }
       
        
        ///// <summary>
        ///// 曲线配置下发完毕后更改标识
        ///// </summary>
        ///// <param name="recdPtr"></param>
        ///// <returns></returns>
        //public override int SentData(GPRS_DATA_RECORD recdPtr)
        //{
        //    //根据电话号码，地址，决定做什么
        //    //把对应的数据的1改为0，表示已经发送
        //    try
        //    {
        //        base.Parse(recdPtr);
        //        //hrzda.UpdateTTRunCurveConf(Org.Id, RegStartAddr, RegCount);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("HrzDownCurve-SentData" + ex.InnerException + "-" + ex.Message);
        //        return 1;
        //    }
        //    return 0;
        //}


        public override int Parse(GPRS_DATA_RECORD recdPtr)
        {

            if (base.Parse(recdPtr) == 1)
                return -1;
            //log.Info("Parse:10");
            Dev = BaseDABll.GetDtuProduct(recdPtr.m_userid);
            Org = BaseDABll.GetOrganize((Guid)Dev.AZWZ);

            List<DetailDevice> hrzjzs = BaseDABll.GetDetailDevice(Dev.Id);
            
            int resualt = 1;
            try
            {
                HjPlcParaControlCurve obj = new HjPlcParaControlCurve();
                obj.GathDt = Convert.ToDateTime(recdPtr.m_recv_date);
                obj.bsO_Id = Org.Id;

                int bufflen;
                Type type = obj.GetType(); //获取类型
                int InitAddr = Convert.ToInt32(bsProtItems[0].StartRegAddress.Substring(2), 16);
                int ItemAddr;

                foreach (bsProtItem pi in bsProtItems)
                {
                    try
                    {
                        bufflen = (int)pi.RegCount * 2;
                        buff = new byte[bufflen];
                        ItemAddr = Convert.ToInt32(pi.StartRegAddress.Substring(2), 16);
                        System.Reflection.PropertyInfo propertyInfo = type.GetProperty(pi.FieldName); //获取指定名称的属性

                        Buffer.BlockCopy(GetData, (ItemAddr - InitAddr) * 2, buff, 0, bufflen);
                        CrossHiLow(ref buff);

                        obj = (HjPlcParaControlCurve)SetValueByReflectionFromBytes<HjPlcParaControlCurve>(obj, pi, propertyInfo, buff);


                    }
                    catch (Exception ex)
                    {
                        log.Error("WritePartCurve.Parse(" + pi.FieldName + "):" + ex.Message);
                    }
                }

                using (WSExpressEntities db = new WSExpressEntities())
                {
                    db.AddToHjPlcParaControlCurve(obj);
                    db.SaveChanges();
                }
                OnProtocalDataReceivedProgress(recdPtr.m_userid, obj);
                obj = null;
            }
            catch (Exception ex)
            {
                log.Error("hrzupunit parse:simno:" + recdPtr.m_userid + ":" + ex.InnerException + "-" + ex.Message);
                resualt = -1;
            }
            return resualt;

            #region previous
            //HjPlcParaControlCurve hjcurve = new HjPlcParaControlCurve();// hrzda.SelectWatchCurveByHrzId(Org.Id);
           
            //Dev = BaseDABll.GetDtuProduct(recdPtr.m_userid);
            //Org = BaseDABll.GetOrganize((Guid)Dev.AZWZ);

            //List<DetailDevice> hrzjzs = BaseDABll.GetDetailDevice(Dev.Id);
            //bsProtItems = BaseDABll.GetProtocalItem(43);//(int)Dev.bsP_Id);

            //int resualt = 1; 
            
            //hjcurve.GathDt = Convert.ToDateTime(recdPtr.m_recv_date);
            //hjcurve.bsO_Id = Org.Id;

            //try
            //{
            //    base.Parse(recdPtr);
             
            //    resualt = 1;
            //    byte[] buff = new byte[2];

            //    buff = new byte[2];
            //    Buffer.BlockCopy(GetData, (0x01BB - 0x01BB) * 2, buff, 0, 2); CrossHiLow(ref buff);
            //    hjcurve.TempCount = BitConverter.ToInt16(buff, 0);
  
            //    buff = new byte[4];
            //    Buffer.BlockCopy(GetData, (0x01BC - 0x01BB) * 2, buff, 0, 4); CrossHiLow(ref buff);
            //    hjcurve.TempScale = (decimal)BitConverter.ToSingle(buff, 0);
            //    Buffer.BlockCopy(GetData, (0x01BE - 0x01BB) * 2, buff, 0, 4); CrossHiLow(ref buff);
            //    hjcurve.TempOffset = (decimal)BitConverter.ToSingle(buff, 0);
                
            //    //曲线点坐标
            //    int addr = (0x1C0 - 0x01BB) * 2;
               
            //    string pxs = "", pys = "";
            //    addr += 4;
            //    int[,] CurvePoints = new int[(int)hjcurve.TempCount, 2];
            //    for (int i = 0; i < hjcurve.TempCount; i++)
            //    {
            //        buff[0]=GetData[addr + i * 2];
            //        CurvePoints[i, 0] = GetData[addr + i * 2];
            //        CurvePoints[i, 1] = GetData[addr + 1 + i * 2];
            //        if (CurvePoints[i, 1] > 127)
            //            CurvePoints[i, 1] -= 256;

            //        hjcurve.GetType().GetProperty("Temp" + (i + 1).ToString()).SetValue(hjcurve, CurvePoints[i, 1].ToString() + "," + CurvePoints[i, 0].ToString(), null);//赋值给b对象

            //    }

            //    EntityManager<HjPlcParaControlCurve>.Add<HjPlcParaControlCurve>(hjcurve);

            //    OnProtocalDataReceivedProgress(recdPtr.m_userid, hjcurve);


            //    #region add on 2015-06-01
            //    //List<DetailDevice> hrzjzs = devda.SelectHrzJzDeviceBySim(recdPtr.m_userid);

            //    //HJPlcParaControl1 obj = new HJPlcParaControl1();
            //    //obj.GathDT = Convert.ToDateTime(recdPtr.m_recv_date);
            //    //obj.DeviDetaID = (int)hrzjzs[0].DeviceTypeID;
            //    //obj.Id = Org.Id;
            //    //obj.GathFlag = 1;

            //    //buff = new byte[2];
            //    //Buffer.BlockCopy(GetData, (0x01B4 - 0x01B4) * 2, buff, 0, 2);
            //    //CrossHiLow(ref buff);
            //    //obj.HJhrzPCGathF123 = (decimal)BitConverter.ToInt16(buff, 0);
            //    //buff = new byte[2];
            //    //Buffer.BlockCopy(GetData, (0x01B4 - 0x01B4) * 2, buff, 0, 2);
            //    //CrossHiLow(ref buff);
            //    //obj.HJhrzPCGathF124 = (decimal)BitConverter.ToInt16(buff, 0);
            //    //buff = new byte[2];
            //    //Buffer.BlockCopy(GetData, (0x01F2 - 0x01B4) * 2, buff, 0, 2);
            //    //CrossHiLow(ref buff);
            //    //obj.HJhrzPCGathF125 = (decimal)BitConverter.ToInt16(buff, 0);
            //    //buff = new byte[2];
            //    //Buffer.BlockCopy(GetData, (0x01F3 - 0x01B4) * 2, buff, 0, 2);
            //    //CrossHiLow(ref buff);
            //    //obj.HJhrzPCGathF126 = (decimal)BitConverter.ToInt16(buff, 0);
            //    //buff = new byte[2];
            //    //Buffer.BlockCopy(GetData, (0x01F4 - 0x01B4) * 2, buff, 0, 2);
            //    //CrossHiLow(ref buff);
            //    //obj.HJhrzPCGathF127 = (decimal)BitConverter.ToInt16(buff, 0);
            //    //buff = new byte[2];
            //    //Buffer.BlockCopy(GetData, (0x01F5 - 0x01B4) * 2, buff, 0, 2);
            //    //CrossHiLow(ref buff);
            //    //obj.HJhrzPCGathF128 = (decimal)BitConverter.ToInt16(buff, 0);
            //    //buff = new byte[2];
            //    //Buffer.BlockCopy(GetData, (0x01F6 - 0x01B4) * 2, buff, 0, 2);
            //    //CrossHiLow(ref buff);
            //    //obj.HJhrzPCGathF129 = (decimal)BitConverter.ToInt16(buff, 0);
            //    //buff = new byte[2];
            //    //Buffer.BlockCopy(GetData, (0x01F7 - 0x01B4) * 2, buff, 0, 2);
            //    //CrossHiLow(ref buff);
            //    //obj.HJhrzPCGathF130 = (decimal)BitConverter.ToInt16(buff, 0);
            //    //buff = new byte[2];
            //    //Buffer.BlockCopy(GetData, (0x01F8 - 0x01B4) * 2, buff, 0, 2);
            //    //CrossHiLow(ref buff);
            //    //obj.HJhrzPCGathF131 = (decimal)BitConverter.ToInt16(buff, 0);
            //    //buff = new byte[2];
            //    //Buffer.BlockCopy(GetData, (0x01F9 - 0x01B4) * 2, buff, 0, 2);
            //    //CrossHiLow(ref buff);
            //    //obj.HJhrzPCGathF132 = (decimal)BitConverter.ToInt16(buff, 0);

            //    #endregion
            //}
            //catch (Exception ex)
            //{
            //    log.Error("hrzupcommdatparse:" + ex.InnerException + "-" + ex.Message);
            //    return 0;
            //}
            //return resualt;
            #endregion

        }


    }
}
