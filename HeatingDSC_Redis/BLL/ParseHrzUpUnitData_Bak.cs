using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DAHeating;
using DAHeating.BLL;

using HeatingDSC.DLL;
namespace HeatingDSC.BLL
{
    public class ParseHrzUpUnitData_bak : IProduct
    {
        //采集数据
        public HisHrz1stNetOpen hisHrz1stOpen = new HisHrz1stNetOpen();
        public Hrz1stNetOpen newHrz1stOpen = new Hrz1stNetOpen();

        //public LastHRZCollectData net1data = new LastHRZCollectData();
        //public List<LastHRZCollectData> net2datas=new List<LastHRZCollectData>();
        public OnTimeHRZCollectData hisHrzGatherdata = new OnTimeHRZCollectData();

        private byte[] buff;

        public static DateTime LastUpDate = DateTime.MaxValue;

        public ParseHrzUpUnitData_bak()
        {
            Producttype = ProductType.HrzUpUnitData;
        }
        public override int Parse(GPRS_DATA_RECORD recdPtr)
        {
            base.Parse(recdPtr);

            List<DeviceDetail> hrzjzs = devda.SelectHrzJzDeviceBySim(recdPtr.m_userid);

            int resualt = 0;
            try
            {
                #region 解析hrz数据对象
                buff = new byte[4];

                OnTimeHRZCollectData net2data = new OnTimeHRZCollectData();
                net2data.OrganizeID = Org.OrganizeID;
                net2data.HRZCollOT0 = DateTime.Parse(recdPtr.m_recv_date);//

                LastUpDate = DateTime.Parse(recdPtr.m_recv_date);//


                Buffer.BlockCopy(GetData, 0x0 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT23 = (decimal)BitConverter.ToSingle(buff, 0);
                float[] qxdata = orgDa.SelectOrgWeather(Org.OrganizeID);
                net2data.HRZCollOT23 = (decimal)qxdata[0];
                net2data.HRZCollOT24 = (decimal)qxdata[2];
                net2data.HRZCollOT25 = (decimal)qxdata[1];

                Buffer.BlockCopy(GetData, 0x2 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOTF102 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x4 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT26 = BitConverter.ToInt32(buff, 0);
                Buffer.BlockCopy(GetData, 0x6 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT27 = BitConverter.ToInt32(buff, 0);
                Buffer.BlockCopy(GetData, 0x8 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOTF108 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0xA * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOTF10A = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0xC * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOTF10C = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0xE * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOTF10E = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x10 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT28 = BitConverter.ToInt32(buff, 0);
                Buffer.BlockCopy(GetData, 0x12 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOT29 = BitConverter.ToInt32(buff, 0);
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
                net2data.DeviDetaID = hrzjzs[BitConverter.ToInt16(buff, 0) - 1].DeviDetaID;


                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x52 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOTF152 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x54 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOTF154 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x56 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOTF156 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x58 * 2, buff, 0, 4); CrossHiLow(ref buff);
                net2data.HRZCollOTF158 = (decimal)BitConverter.ToSingle(buff, 0);

                //保存数据库
                //OnTimeHRZCollectData othcd = ChangeNewToHisHrzObj(net2data);
                glfda.heatingdb = new HeatingDbEntities();

                glfda.heatingdb.AddToOnTimeHRZCollectData(net2data);//net2datas[i]));
                glfda.heatingdb.SaveChanges();

                //glfda.heatingdb.AddToLastHRZCollectData(net2data);
                //glfda.heatingdb.SaveChanges();

                #endregion

                OnHrzJzUnitProgress(net2data);
                net2data = null;
            }
            catch (Exception ex)
            {
                log.Error("hrzupunit parse:simno:" + recdPtr.m_userid + ":" + ex.InnerException + "-" + ex.Message);
                resualt = -1;
            }
            return resualt;
        }

        //private OnTimeHRZCollectData ChangeNewToHisHrzObj(LastHRZCollectData newobj)
        //{
        //    OnTimeHRZCollectData hisobj = new OnTimeHRZCollectData();
        //    hisobj.DeviDetaID = newobj.DeviDetaID;
        //    hisobj.OrganizeID = newobj.OrganizeID;
        //    hisobj.HRZCollOT0=newobj.HRZCollectDataLast;
        //    //hisobj.HRZCollOT0 = newobj.HRZColl0Last;
        //    hisobj.HRZCollOT1 = newobj.HRZColl1Last;
        //    hisobj.HRZCollOT2 = newobj.HRZColl2Last;
        //    hisobj.HRZCollOT3 = newobj.HRZColl3Last;
        //    hisobj.HRZCollOT4 = newobj.HRZColl4Last;
        //    hisobj.HRZCollOT5 = newobj.HRZColl5Last;
        //    hisobj.HRZCollOT6 = newobj.HRZColl6Last;
        //    hisobj.HRZCollOT7 = newobj.HRZColl7Last;
        //    hisobj.HRZCollOT8 = newobj.HRZColl8Last;
        //    hisobj.HRZCollOT9 = newobj.HRZColl9Last;
        //    hisobj.HRZCollOT10 = newobj.HRZColl10Last;
        //    hisobj.HRZCollOT11 = newobj.HRZColl11Last;
        //    hisobj.HRZCollOT12 = newobj.HRZColl12Last;
        //    hisobj.HRZCollOT13 = newobj.HRZColl13Last;
        //    hisobj.HRZCollOT14 = newobj.HRZColl14Last;
        //    hisobj.HRZCollOT15 = newobj.HRZColl15Last;
        //    hisobj.HRZCollOT142 = newobj.HRZColl14Last2;
        //    hisobj.HRZCollOT152 = newobj.HRZColl15Last2;
        //    hisobj.HRZCollOT16 = newobj.HRZColl16Last;
        //    //hisobj.OTGLCN18 = newobj.LastGLCN18;
        //    hisobj.HRZCollOT17 = newobj.HRZColl17Last;
        //    hisobj.HRZCollOT162 = newobj.HRZColl16Last2;
        //    hisobj.HRZCollOT172 = newobj.HRZColl17Last2;
        //    hisobj.HRZCollOT18 = newobj.HRZColl18Last;
        //    hisobj.HRZCollOT19 = newobj.HRZColl19Last;
        //    hisobj.HRZCollOT20 = newobj.HRZColl20Last;
        //    hisobj.HRZCollOT21 = newobj.HRZColl21Last;
        //    hisobj.HRZCollOT22 = newobj.HRZColl22Last;
        //    hisobj.HRZCollOT23 = newobj.HRZColl23Last;
        //    hisobj.HRZCollOT24 = newobj.HRZColl24Last;
        //    hisobj.HRZCollOT25 = newobj.HRZColl25Last;
        //    hisobj.HRZCollOT26 = (int)newobj.HRZColl26Last;//should modify the data to much data
        //    hisobj.HRZCollOT27 = (int)newobj.HRZColl27Last;
        //    hisobj.HRZCollOT28 = (int)newobj.HRZColl28Last;
        //    hisobj.HRZCollOT29 = (int)newobj.HRZColl29Last;
        //    //hisobj.HRZCollOT30 = (int)newobj.HRZColl30Last;
        //    //hisobj.HRZCollOT31 = (int)newobj.HRZColl31Last;

        //    hisobj.HRZCollOTF102 = newobj.HRZCollLastF102;
        //    hisobj.HRZCollOTF108 = newobj.HRZCollLastF108;
        //    hisobj.HRZCollOTF10A = newobj.HRZCollLastF10A;
        //    hisobj.HRZCollOTF10C = newobj.HRZCollLastF10C;
        //    hisobj.HRZCollOTF10E = newobj.HRZCollLastF10E;
        //    hisobj.HRZCollOTF114 = newobj.HRZCollLastF114;
        //    hisobj.HRZCollOTF116 = newobj.HRZCollLastF116;
        //    hisobj.HRZCollOTF118 = newobj.HRZCollLastF118;
        //    hisobj.HRZCollOTF11A = newobj.HRZCollLastF11A;
        //    hisobj.HRZCollOTF11C = newobj.HRZCollLastF11C;
        //    hisobj.HRZCollOTF11E = newobj.HRZCollLastF11E;
        //    hisobj.HRZCollOTF150 = newobj.HRZCollLastF150;
        //    hisobj.HRZCollOTF151 = newobj.HRZCollLastF151;
        //    hisobj.HRZCollOTF152 = newobj.HRZCollLastF152;
        //    hisobj.HRZCollOTF154 = newobj.HRZCollLastF154;
        //    hisobj.HRZCollOTF156 = newobj.HRZCollLastF156;
        //    hisobj.HRZCollOTF158 = newobj.HRZCollLastF158;
        //    hisobj.HRZCollOTF1000 = newobj.HRZCollLastF1000;

        //    return hisobj;
        //}

    }
}
