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

namespace QyTech.ProtocalHrzSheXianJN
{
    public class WritePartCurve:IProtocal
    {

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


            HjParaControlCurve1 objtarget = new HjParaControlCurve1();
            System.Reflection.PropertyInfo propertyInfo;
            //转换数据
            objtarget.TempCount = (int)obj_.CurevePointCount;
            objtarget.TempScale = (decimal)obj_.CurveScale;
            objtarget.TempOffset = (decimal)obj_.CurveOffset;
            string[] wds = obj_.CurveHeatringWDS.Split(new char[] { ',' });

            Type type = objtarget.GetType();
            int outwd, hwd;
            string val;
            for (int i = 1; i <= wds.Length; i += 1)
            {
                hwd = Convert.ToInt16(wds[i-1]);
                propertyInfo = type.GetProperty("Temp" + (i *2-1).ToString()); //获取指定名称的属性
                propertyInfo.SetValue(objtarget, hwd, null); //给对应属性赋值

                outwd = Convert.ToInt16(obj_.OutWdMin + i * obj_.OutWdStep);// Convert.ToInt16(wds[i]);
                propertyInfo = type.GetProperty("Temp" + (i * 2).ToString()); //获取指定名称的属性
                propertyInfo.SetValue(objtarget, outwd, null); //给对应属性赋值

            
            }
            for (int i = wds.Length+1; i <= 48; i++)
            {
                hwd = Convert.ToInt16(wds[wds.Length-1]);
                propertyInfo = type.GetProperty("Temp" + (i * 2 - 1).ToString()); //获取指定名称的属性
                propertyInfo.SetValue(objtarget, hwd, null); //给对应属性赋值

                outwd = Convert.ToInt16(obj_.OutWdMin + i * obj_.OutWdStep);// Convert.ToInt16(wds[i]);
                propertyInfo = type.GetProperty("Temp" + (i * 2).ToString()); //获取指定名称的属性
                propertyInfo.SetValue(objtarget, outwd, null); //给对应属性赋值
            }

            return base.CreateModbusRtuWriteCommand<HjParaControlCurve1>(simno, objtarget, bsProtItems);
        }



        public override DeviceCmd CreateReadCommand(string simno)
        {
            return base.CreateModbusRtuReadCommand(simno, 0x01, 0x03, 0x007A, 0x0DF - 0x07A+ 1);
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
                HjParaControlCurve1 obj = new HjParaControlCurve1();
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

                        obj = (HjParaControlCurve1)SetValueByReflectionFromBytes<HjParaControlCurve1>(obj, pi, propertyInfo, buff);
                    }
                    catch (Exception ex)
                    {
                        log.Error("ReadData.Parse(" + pi.FieldName + "):" + ex.Message);
                    }
                }

                using (WSExpressEntities db = new WSExpressEntities())
                {
                    db.AddToHjParaControlCurve1(obj);
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

     

        }


    }
}
