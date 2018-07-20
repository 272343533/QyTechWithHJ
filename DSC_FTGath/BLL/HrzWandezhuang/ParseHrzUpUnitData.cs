using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SunMvcExpress.Dao;
using QyTech.HDGprs;



using QyTech.Communication;
namespace HeatingDSC.BLL
{
    public class ParseHrzUpUnitData : IProduct
    {
       //采集数据
        public Hrz1stNetOpen hisHrz1stOpen = new Hrz1stNetOpen();
        //public Hrz1stNetOpen newHrz1stOpen=new Hrz1stNetOpen();

        //public LastHRZCollectData net1data = new LastHRZCollectData();
        //public List<LastHRZCollectData> net2datas=new List<LastHRZCollectData>();
        public OnTimeHRZCollectData hisHrzGatherdata=new OnTimeHRZCollectData();
            
      
        public static DateTime LastUpDate = DateTime.MaxValue;

        public ParseHrzUpUnitData()
        {
            Producttype = ProductType.HrzUpUnitData;
        }
        public override int Parse(GPRS_DATA_RECORD recdPtr)
        {
            base.Parse(recdPtr);

            List<DetailDevice> hrzjzs = devda.SelectHrzJzDeviceBySim(recdPtr.m_userid);
            
            int resualt = 0;
            try
            {
                #region 解析hrz数据对象
                buff = new byte[4];

                NewHRZCollectData net2data = new NewHRZCollectData();
                net2data.Id = Org.Id;
                net2data.GathDt = DateTime.Parse(recdPtr.m_recv_date);//

                LastUpDate = DateTime.Parse(recdPtr.m_recv_date);//

                //写的是0，实际机组部分从100，200，300开始的，所以要注意
                Buffer.BlockCopy(GetData, 0x0 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT23 = (decimal)BitConverter.ToSingle(buff, 0);
                
                float[] qxdata = orgDa.SelectOrgWeather(Org.Id);
                net2data.HRZCollOT23 = (decimal)qxdata[0];
                net2data.HRZCollOT24 = (decimal)qxdata[2];
                net2data.HRZCollOT25= (decimal)qxdata[1];

                //一次网调节阀开度控制
                Buffer.BlockCopy(GetData, 0x2 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOTF102 = (decimal)BitConverter.ToSingle(buff, 0);
                //P
                Buffer.BlockCopy(GetData, 0x4 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT26 = (decimal)BitConverter.ToSingle(buff, 0);
                //I
                Buffer.BlockCopy(GetData, 0x6 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT27 = (decimal)BitConverter.ToSingle(buff, 0);
                //D
                Buffer.BlockCopy(GetData, 0x8 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOTF108 = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0xA * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOTF10A = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0xC * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOTF10C = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0xE * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOTF10E = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x10 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT28 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x12 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT29 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x14 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOTF114 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x16 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOTF116 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x18 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOTF118 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x1A * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOTF11A = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x1C * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOTF11C = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x1E * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOTF11E = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x20 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT1 = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x22 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT2 = (decimal)BitConverter.ToSingle(buff, 0);
 
                Buffer.BlockCopy(GetData, 0x24 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT3 = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x26 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT4 = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x28 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT5 = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x2A * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT6 = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x2C * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT7 = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x2E * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT8 = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x30 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT9 = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x32 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT10 = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x34 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT11 = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x36 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT12 = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x38 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT13 = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x3A * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT14 = (decimal)BitConverter.ToSingle(buff, 0);
                net2data.HRZCollOT14 = net2data.HRZCollOT14 <= frmStart.HzCutDownValue ? 0 : net2data.HRZCollOT14;

                Buffer.BlockCopy(GetData, 0x3C * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT15 = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x3E * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT142 = (decimal)BitConverter.ToSingle(buff, 0);
                net2data.HRZCollOT142 = net2data.HRZCollOT142 <= frmStart.HzCutDownValue ? 0 : net2data.HRZCollOT142;
              
                Buffer.BlockCopy(GetData, 0x40 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT152 = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x42 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT16 = (decimal)BitConverter.ToSingle(buff, 0);
                net2data.HRZCollOT16 = net2data.HRZCollOT16 <= frmStart.HzCutDownValue ? 0 : net2data.HRZCollOT16;
              

                Buffer.BlockCopy(GetData, 0x44 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT17 = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x46 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT162 = (decimal)BitConverter.ToSingle(buff, 0);
                net2data.HRZCollOT162 = net2data.HRZCollOT162 <= frmStart.HzCutDownValue ? 0 : net2data.HRZCollOT162;

                Buffer.BlockCopy(GetData, 0x48 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT172 = (decimal)BitConverter.ToSingle(buff, 0);

            
                Buffer.BlockCopy(GetData, 0x4A * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT18 = (long)BitConverter.ToUInt32(buff, 0);

                Buffer.BlockCopy(GetData, 0x4C * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT19 = (long)BitConverter.ToUInt32(buff, 0);

                net2data.HRZCollOT20 = net2data.HRZCollOT12 - net2data.HRZCollOT11;// (decimal)BitConverter.ToSingle(buff, 0);
         
                Buffer.BlockCopy(GetData, 0x4E * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT21 = (decimal)BitConverter.ToSingle(buff, 0);
     
                net2data.HRZCollOT22 = (int)(net2data.HRZCollOT11 - net2data.HRZCollOT12);
                
                buff = new byte[2];
                Buffer.BlockCopy(GetData, 0x50 * 2, buff, 0, 2); CrossHiLow(ref buff);
                net2data.HRZCollOTF150 = (int)BitConverter.ToInt16(buff, 0); ;//补水泵状态

                Buffer.BlockCopy(GetData, 0x51 * 2, buff, 0, 2); CrossHiLow(ref buff);
                net2data.Det_Id = hrzjzs[BitConverter.ToInt16(buff, 0) - 1].Id;


                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x52 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOTF152 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x54 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOTF154 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x56 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOTF156 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x58 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOTF158 = (decimal)BitConverter.ToSingle(buff, 0);


                OnTimeHRZCollectData hisObj = new OnTimeHRZCollectData();
                HeatingDSC2.BLL.EntityManager.EntityCopyNotExact(net2data, hisObj);
               
                //保存数据库
                //OnTimeHRZCollectData othcd = ChangeNewToHisHrzObj(net2data);
                using (glfda.heatingdb = new WSExpressEntities())
                {

                    glfda.heatingdb.AddToNewHRZCollectData(net2data);//net2datas[i]));
                    glfda.heatingdb.SaveChanges();

                }
                 //hisObj.EntityKey.EntitySetName="
                //using (glfda.heatingdb = new WSExpressEntities())
                //{
                //    glfda.heatingdb.AddToOnTimeHRZCollectData(hisObj);
                //    glfda.heatingdb.SaveChanges();
                //}
                #endregion

                OnHrzJzUnitProgress(hisObj);
                net2data = null;
            }
            catch (Exception ex)
            {
                log.Error("hrzupunit parse:simno:" + recdPtr.m_userid +":"+ ex.InnerException + "-" + ex.Message);
                resualt = -1;
           }
            return resualt;
        }

  
    }
}
