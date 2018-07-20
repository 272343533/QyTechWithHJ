using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QyTech.Protocal;
using SunMvcExpress.Dao;
using QyTech.HDGprs;

using HeatingDSC.Models;
using QyTech.Communication;
namespace HeatingDSC.BLL
{
    public class HjHrzReadOnlyData : IProduct
    {
       //采集数据
        public Hrz1stNetOpen hisHrz1stOpen = new Hrz1stNetOpen();
        //public Hrz1stNetOpen newHrz1stOpen=new Hrz1stNetOpen();

        //public LastHRZCollectData net1data = new LastHRZCollectData();
        //public List<LastHRZCollectData> net2datas=new List<LastHRZCollectData>();
        public OnTimeHRZCollectData hisHrzGatherdata=new OnTimeHRZCollectData();
            
      
        public static DateTime LastUpDate = DateTime.MaxValue;

        public HjHrzReadOnlyData()
        {
            Producttype = ProductType.HJGathData1;
        }
        public override int Parse(GPRS_DATA_RECORD recdPtr)
        {
            base.Parse(recdPtr);
            //log.Info("Parse:10");
            List<DetailDevice> hrzjzs = devda.SelectHrzJzDeviceBySim(recdPtr.m_userid);
            int resualt = 0;
            HJGathData1 hisobj = new HJGathData1();
            NewHJGathData1 obj = new NewHJGathData1();

            try
            {
                #region 解析hrz数据对象
                obj.GathDt = Convert.ToDateTime(recdPtr.m_recv_date);
                obj.Det_Id = hrzjzs[0].Id;
                obj.Id = Org.Id;
                //log.Info("Parse:30");

                //解码开始
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0000 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF1 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0002 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF2 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0004 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF3 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0006 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF4 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0008 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF5 = (decimal)BitConverter.ToUInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x000A - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF6 = (decimal)BitConverter.ToUInt32(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x000C - 0x0000) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJGathF7 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x000D - 0x0000) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJGathF8 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x000E - 0x0000) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJGathF9 = (decimal)BitConverter.ToUInt16(buff, 0);


                //5-30修改
                Buffer.BlockCopy(GetData, (0x000F - 0x0000) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJGathF10 = (decimal)BitConverter.ToUInt16(buff, 0);
                //5-30修改

                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0010 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF11 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0012 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF12 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0014 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF13 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0016 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF14 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0018 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF15 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x001A - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF16 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x001C - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF17 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x001E - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF18 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0020 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF19 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0022 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF20 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0024 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF21 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0026 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF22 = (decimal)BitConverter.ToSingle(buff, 0);

                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0028 - 0x0000) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJGathFX28 = (decimal)BitConverter.ToUInt16(buff, 0);
                Buffer.BlockCopy(GetData, (0x0029 - 0x0000) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJGathFX29 = (decimal)BitConverter.ToUInt16(buff, 0);
                Buffer.BlockCopy(GetData, (0x002A - 0x0000) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJGathFX2A = (decimal)BitConverter.ToUInt16(buff, 0);
                Buffer.BlockCopy(GetData, (0x002B - 0x0000) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJGathFX2B = (decimal)BitConverter.ToUInt16(buff, 0);




                //4-21修改时间 
                try
                {
                    buff = new byte[2];
                    Buffer.BlockCopy(GetData, (0x002C - 0x0000) * 2, buff, 0, 2);
                    CrossHiLow(ref buff);
                    obj.HJGathF67 = (decimal)BitConverter.ToUInt16(buff, 0);
                    obj.HJGathF67 = Convert.ToDecimal((Convert.ToInt16(obj.HJGathF67)).ToString("X2"));

                    Buffer.BlockCopy(GetData, (0x002D - 0x0000) * 2, buff, 0, 2);
                    CrossHiLow(ref buff);
                    obj.HJGathF68 = (decimal)BitConverter.ToUInt16(buff, 0);
                    obj.HJGathF68 = Convert.ToDecimal((Convert.ToInt16(obj.HJGathF68)).ToString("X2"));

                    Buffer.BlockCopy(GetData, (0x002E - 0x0000) * 2, buff, 0, 2);
                    CrossHiLow(ref buff);
                    obj.HJGathF69 = (decimal)BitConverter.ToUInt16(buff, 0);
                    obj.HJGathF69 = Convert.ToDecimal((Convert.ToInt16(obj.HJGathF69)).ToString("X2"));

                    Buffer.BlockCopy(GetData, (0x002F - 0x0000) * 2, buff, 0, 2);
                    CrossHiLow(ref buff);
                    obj.HJGathF70 = (decimal)BitConverter.ToUInt16(buff, 0);
                    obj.HJGathF70 = Convert.ToDecimal((Convert.ToInt16(obj.HJGathF70)).ToString("X2"));
                }
                catch (Exception ex)
                {
                    log.Error("hrzupunit parse:simno: 年月部分解码 New Object" + recdPtr.m_userid + ":" + ex.InnerException + "-" + ex.Message);
                
                }
                //4-21修改时间 end


                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0030 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF26 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0032 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF27 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0034 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF28 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0036 - 0x0000) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJGathF29 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0038 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF31 = (decimal)BitConverter.ToUInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0040 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF35 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0042 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF36 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0044 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF37 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0046 - 0x0000) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJGathF38 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0048 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF40 = (decimal)BitConverter.ToUInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0050 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF44 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0052 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF45 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0054 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF46 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0056 - 0x0000) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJGathF47 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0058 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF49 = (decimal)BitConverter.ToUInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0060 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF54 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0062 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF56 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0064 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF58 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, (0x0066 - 0x0000) * 2, buff, 0, 2);
                CrossHiLow(ref buff);
                obj.HJGathF59 = (decimal)BitConverter.ToUInt16(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0068 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF61 = (decimal)BitConverter.ToUInt32(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0070 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF65 = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0072 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF66 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, (0x0074 - 0x0000) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.HJGathF1000 = (decimal)BitConverter.ToSingle(buff, 0);


                //保存数据库
                //OnTimeHRZCollectData othcd = ChangeNewToHisHrzObj(net2data);
                //glfda.heatingdb = new WSExpressEntities();

                //glfda.heatingdb.AddToHJGathData1(obj);//net2datas[i]));
                //glfda.heatingdb.SaveChanges();
                //log.Info("Parse:40");

           
                //log.Info("Parse:50");
                #endregion
                using (WSExpressEntities db = new WSExpressEntities())
                {
                    db.AddToNewHJGathData1(obj);
                    db.SaveChanges();
                }
                HeatingDSC2.BLL.EntityManager.EntityCopyNotExact(obj, hisobj);
            }
            catch (Exception ex)
            {
                log.Error("hrzupunit parse:simno: New Object" + recdPtr.m_userid + ":" + ex.InnerException + "-" + ex.Message);
                resualt = -1;
            }
               
            try{

                OnProtocalDataReceivedProgress(recdPtr.m_userid, hisobj);

                //using (WSExpressEntities db = new WSExpressEntities())
                //{
                //    db.AddToHJGathData1(hisobj);
                //    db.SaveChanges();
                //}
            }
            catch (Exception ex)
            {
                log.Error("hrzupunit parse:simno: Object" + recdPtr.m_userid + ":" + ex.InnerException + "-" + ex.Message);
                resualt = -1;
            }
            finally
            {
                //log.Info("Parse:70");
                obj = null;
                hisobj = null;
                hrzjzs.Clear();
                hrzjzs = null;
            }
        
            return resualt;
        }

        public override DeviceCmd CreateReadCommand(string simno)
        {
            return base.CreateModbusRtuReadCommand(simno, 0x01, 0x03, 0x0, 0x74+1+1-0x0);
        }

        /// <summary>
        /// 无效，该协议部分不可写
        /// </summary>
        /// <param name="simno"></param>
        /// <param name="writeflag"></param>
        /// <returns></returns>
        //public override DeviceCmd CreateWriteCommand(string simno, string writeflag)
        //{
        //    return new DeviceCmd();
        //}
    }
}
