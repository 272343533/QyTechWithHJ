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

namespace QyTech.GlfNingHeBgl
{
    /// <summary>
    /// 目前该函数已经不在使用，改为DownhrzControl class
    /// </summary>
    public class WritePartCurve : IProtocal
    {
        public DownHrzCurve downHrzcurve_;

    

        public WritePartCurve(int bsP_Id)
            : base(bsP_Id)
        {
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="simno"></param>
        /// <returns></returns>
        public override DeviceCmd CreateModbusRtuWriteCommandByConf(string simno, object obj)
        {
            HrzRunCurve obj_ = (HrzRunCurve)obj;


            HjPlcParaControlCurve objtarget = new HjPlcParaControlCurve();

            //转换数据
            objtarget.TempCount = obj_.CurevePointCount;
            objtarget.TempScale = obj_.CurveScale;
            objtarget.TempOffset = obj_.CurveOffset;
            string[] wds = obj_.CurveHeatringWDS.Split(new char[] { ',' });

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

                propertyInfo.SetValue(objtarget, val, null); //给对应属性赋值
            }
            for (int i = wds.Length; i < 60; i++)
            {
                outwd = Convert.ToInt16(obj_.OutWdMin + i * obj_.OutWdStep);// Convert.ToInt16(wds[i]);

                hwd = Convert.ToInt16(wds[wds.Length - 1]);

                val = outwd.ToString() + "," + hwd.ToString();

                System.Reflection.PropertyInfo propertyInfo = type.GetProperty("Temp" + (i + 1).ToString()); //获取指定名称的属性
                propertyInfo.SetValue(objtarget, val, null); //给对应属性赋值

            }

            return base.CreateModbusRtuWriteCommand<HjPlcParaControlCurve>(simno, objtarget, bsProtItems);
        }

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
                        log.Error("ReadData.Parse(" + pi.FieldName + "):" + ex.Message);
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
  
            #endregion

        }

    }
}
