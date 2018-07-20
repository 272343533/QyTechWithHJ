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
using QyTech.Core.BLL;
namespace QyTech.HjProtocal12
{
    /// <summary>
    /// 目前该函数已经不在使用，改为DownhrzControl class
    /// </summary>
    public class WritePartCurve : IProtocal
    {
 
        public WritePartCurve(int bsP_Id)
            : base(bsP_Id)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">DownHrzCurve</param>
        /// <returns>要下发的字节数组</returns>
        public override byte[] Create(object obj)
        {
            // base.Create(obj);
            //26	0x0100	曲线比例系数	Single Float	2	Read/Write
            //27	0x0102	曲线调节增量	Single Float	2	Read/Write
            //28	0x0104	曲线中坐标点数	Single int 	1	Read/Write
            //29	0x0105	室外温度最低值	Single int	1	Read/Write
            //30	0x0106	室外温度变化步长	Single int 	1	Read/Write
            //31	0x0107	1#点二次网温度	Single int 	1	Read/Write
            //32	0x0108	2#点二次网温度	Single int	1	Read/Write
            //33	0x0109	…	…	…	…
            //34	0x017E	120#点二次网温度	Single int 	1	Read/Write
            //判断该组织结构是否有曲线需要下发
            byte qxwd;//曲线温度 0-255
            sbyte outwd;//室外温度 -127-128
            HrzRunCurve hrc_ = (HrzRunCurve)obj;

            //2:系数 2：增量 1：坐标点 1 最低值 1步长 120个点/2

            byte[] data;// = new byte[134];//[134];  //(2+2+3+60)*2             //old:(120+3+2*3)*2
            int byteindex = 0;
            int len = (0x1EF - 0x1B4 + 1) * 2;
            data = new byte[len];

            //0x1B4
            byte[] buff = new byte[2];
            buff = BitConverter.GetBytes((short)hrc_.CurevePointCount);
            CrossHiLow(ref buff);
            Buffer.BlockCopy(buff, 0, data, byteindex, 2);

            //0x1B5-0x1BB
            for (int i = 0x1B5; i <= 0x1BB; i++)
            {
                buff = BitConverter.GetBytes((short)0);
                CrossHiLow(ref buff);
                Buffer.BlockCopy(buff, 0, data, byteindex += 2, 2);
            }
            //0x1BC
            buff = new byte[4];
            buff = BitConverter.GetBytes((float)hrc_.CurveScale);
            CrossHiLow(ref buff);
            Buffer.BlockCopy(buff, 0, data, byteindex += 2, 4);

            //0x1BE
            buff = BitConverter.GetBytes((float)hrc_.CurveOffset);
            CrossHiLow(ref buff);
            Buffer.BlockCopy(buff, 0, data, byteindex += 4, 4);

            byteindex += 4;
            //0x1C0
            string[] wds = hrc_.CurveHeatringWDS.Split(new char[] { ',' });

            for (int i = 0; i < hrc_.CurevePointCount; i++)
            {
                //曲线温度
                qxwd = Convert.ToByte(wds[i]);
                data[byteindex + i * 2] = qxwd;

                //室外温度
                outwd = Convert.ToSByte(hrc_.OutWdMin + i * hrc_.OutWdStep);
                if (outwd >= 0)
                    data[byteindex + i * 2 + 1] = (byte)outwd;
                else
                {
                    data[byteindex + i * 2 + 1] = Convert.ToByte(256 + outwd);//128+downHrzcurve_.rc.MInWD + downHrzcurve_.rc.WDStep * i);
                }



                if ((byteindex + i * 2) >= data.Length)  //(14 + 120 * 2))
                    break;
            }
            //如果点的数量不足48个，则后续数据补0
            if (hrc_.CurevePointCount < 48)
            {
                for (int i = 0; i < (48 - hrc_.CurevePointCount) * 2; i++)
                {
                    data[119 - i] = 0;
                }
            }

            return data;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="simno"></param>
        /// <param name="obj">HrzRunCurve</param>
        /// <returns></returns>
        public override DeviceCmd CreateModbusRtuWriteCommandByConf(string simno,object obj)
        {
            HrzRunCurve obj_ = (HrzRunCurve)obj;
           
            
            HjPlcParaControlCurve objtarget = new HjPlcParaControlCurve();

            //转换数据
            objtarget.TempCount = obj_.CurevePointCount;
            objtarget.TempScale = obj_.CurveScale;
            objtarget.TempOffset = obj_.CurveOffset;
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

                hwd = Convert.ToInt16(wds[wds.Length - 1]);

                val = outwd.ToString() + "," + hwd.ToString();

                System.Reflection.PropertyInfo propertyInfo = type.GetProperty("Temp" + (i + 1).ToString()); //获取指定名称的属性
                propertyInfo.SetValue(objtarget, val, null); //给对应属性赋值

            }
            
            return base.CreateModbusRtuWriteCommand<HjPlcParaControlCurve>(simno, objtarget, bsProtItems);
        }

        public override DeviceCmd CreateModbusRtuWriteCommandByGath(string simno,object obj)
        {
            HjPlcParaControlCurve obj_ = (HjPlcParaControlCurve)obj;
            return base.CreateModbusRtuWriteCommand<HjPlcParaControlCurve>(simno, obj_, bsProtItems);
        }

        public override DeviceCmd CreateModbusRtuWriteCommandBySelfDefine(string simno)
        {
            Dev = BaseDABll.GetDtuProduct(simno);
            //Org = BaseDABll.GetOrganize((Guid)Dev.AZWZ);

            //HjPlcParaControlCurve obj_ = EntityManager<HjPlcParaControlCurve>.GetByPk<HjPlcParaControlCurve>("bsO_Id", Guid.Parse(Org.BllOrgID));
            //return base.CreateModbusRtuWriteCommand<HjPlcParaControlCurve>(simno, obj_, bsProtItems);


            List<HjPlcParaControlCurve> objs = EntityManager<HjPlcParaControlCurve>.GetAllByStorProcedure<HjPlcParaControlCurve>("splyGetClouldStorageData '", Dev.AZWZ.ToString() + "',3");
            return base.CreateModbusRtuWriteCommand<HjPlcParaControlCurve>(simno, objs[0], bsProtItems);
        }


        public override DeviceCmd CreateReadCommand(string simno)
        {
            return base.CreateModbusRtuReadCommand(simno, 0x01, 0x03, 0x1BB, 0x1F0 - 0x1BB + 1);
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
                try
                {
                    NewStandDataHrz gathobj = EntityManager<NewStandDataHrz>.GetByPk<NewStandDataHrz>("Det_Id", hrzjzs[0].Id);
                    if ((Convert.ToInt32(gathobj.F118) & 0x03) == 3)
                    {
                        obj.bsO_Id = Guid.Parse(Org.BllOrgID);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("云存储判断："+ex.Message);
                }
                   

                int bufflen;
                Type type = obj.GetType(); //获取类型
                int InitAddr = Convert.ToInt32(bsProtocalObj.FromAddr.Substring(2), 16);
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
                EntityManager<HjPlcParaControlCurve>.Add<HjPlcParaControlCurve>(obj);

                NewHjPlcParaControlCurve newobj = EntityManager<NewHjPlcParaControlCurve>.GetBySql<NewHjPlcParaControlCurve>("bsO_Id='" + obj.bsO_Id.ToString() + "'");
                if (newobj == null)
                {
                    newobj = EntityOperate.Copy<NewHjPlcParaControlCurve>(obj);
                    EntityManager<NewHjPlcParaControlCurve>.Add<NewHjPlcParaControlCurve>(newobj);
                }
                else
                {
                    EntityOperate.Copy<NewHjPlcParaControlCurve>(obj, newobj, "bsO_Id");
                    EntityManager<NewHjPlcParaControlCurve>.Modify<NewHjPlcParaControlCurve>(newobj);
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
