using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QyTech.Communication;
using SunMvcExpress.Dao;
using QyTech.HDGprs;

namespace HeatingDSC.BLL
{
    public class ParseGlfSanFengUpData: IProduct
    {
        public ONTimeGLCommNet HisGgdy = new ONTimeGLCommNet();
        public OTGLUnitData[] HisLdy = new OTGLUnitData[2];
        public OnTimeHRZCollectData[] HisHrzJz = new OnTimeHRZCollectData[1];

        //public HisJLAlarmGatherData[] HisJLAlarm = new HisJLAlarmGatherData[9];
        
        public ParseGlfSanFengUpData()
        {
            Producttype = ProductType.GlfSanFengUpData;
        }
        
        public override int Parse(GPRS_DATA_RECORD recdPtr)
        {
            base.Parse(recdPtr);
            //根据电话号码确定是组织结构即可
            //bsOrganize org = orgDa.SelectOrganizationBySimCard(recdPtr.m_userid);
            //津涞只有一组设备

            DetailDevice ggdy = devda.SelectGgdyDeviceBySim(recdPtr.m_userid);
            List<DetailDevice> ldys = devda.SelectLdyDeviceBySim(recdPtr.m_userid);
            int resualt = 0;


            HisGgdy.Det_Id = ggdy.Id;
            HisGgdy.Id = Org.Id;

            string StationID = recdPtr.m_userid;        //DTU身份识别码
            DateTime dtGatherTime = DateTime.Parse(recdPtr.m_recv_date);    //数据接收时间
            HisGgdy.GathDt = dtGatherTime;
            try
            {
                #region 解析到最新公共部分数据对象
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN30 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x2 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN31 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x4 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN32 = (decimal)BitConverter.ToDouble(buff, 0);

                Buffer.BlockCopy(GetData, 0x10 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN6 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x12 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN1 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x14 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN2 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x16 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN7 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x18 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN29 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x1A * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN8 = (decimal)BitConverter.ToDouble(buff, 0);
                //Buffer.BlockCopy(GetData, 0x1C * 2, buff, 0, 4); //CrossHiLow(ref buff);
                //HisGgdy.OTGLCN29 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x1E * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN3 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x20 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN4 = (decimal)BitConverter.ToDouble(buff, 0);
                //Buffer.BlockCopy(GetData, 0x22 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                //HisGgdy.OTGLCN9 = (decimal)BitConverter.ToDouble(buff, 0);
                //Buffer.BlockCopy(GetData, 0x24 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                //HisGgdy.OTGLCN11 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x26 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN10 = (decimal)BitConverter.ToDouble(buff, 0);

                
                //Buffer.BlockCopy(GetData, 0x30 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                //HisGgdy.OTGLCN29 = (decimal)BitConverter.ToDouble(buff, 0);
                //Buffer.BlockCopy(GetData, 0x32 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                //HisGgdy.OTGLCN29 = (decimal)BitConverter.ToDouble(buff, 0);
                //Buffer.BlockCopy(GetData, 0x34 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                //HisGgdy.OTGLCN29 = (decimal)BitConverter.ToDouble(buff, 0);
                //Buffer.BlockCopy(GetData, 0x36 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                //HisGgdy.OTGLCN29 = (decimal)BitConverter.ToDouble(buff, 0);
                //Buffer.BlockCopy(GetData, 0x38 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                //HisGgdy.OTGLCN29 = (decimal)BitConverter.ToDouble(buff, 0);
                //Buffer.BlockCopy(GetData, 0x3A * 2, buff, 0, 4); //CrossHiLow(ref buff);
                //HisGgdy.OTGLCN29 = (decimal)BitConverter.ToDouble(buff, 0);



                Buffer.BlockCopy(GetData, 0x40 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN62 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x42 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN63 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x44 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN64 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x46 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN65 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x48 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN66 = (decimal)BitConverter.ToDouble(buff, 0);

                Buffer.BlockCopy(GetData, 0x50 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN20 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x52 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN59 = (decimal)BitConverter.ToDouble(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, 0x0054 * 2, buff, 0, 2);// CrossHiLow(ref buff);
                HisGgdy.JLOTGLCN47 = (int)(buff[0] & (byte)128) == 0 ? 0 : 1;
                HisGgdy.JLOTGLCN48 = (int)(buff[0] & (byte)64) == 0 ? 0 : 1;
                HisGgdy.JLOTGLCN49 = (int)(buff[0] & (byte)32) == 0 ? 0 : 1;
                HisGgdy.JLOTGLCN51 = (int)(buff[0] & (byte)16) == 0 ? 0 : 1;
                HisGgdy.JLOTGLCN52 = (int)(buff[0] & (byte)8) == 0 ? 0 : 1;
                //HisGgdy.JLLastGLCN42 = (int)(buff[0] & (byte)4) == 0 ? 0 : 1;
                //HisGgdy.JLLastGLCN43 = (int)(buff[0] & (byte)2) == 0 ? 0 : 1;
                //HisGgdy.JLLastGLCN44 = (int)(buff[0] & (byte)1) == 0 ? 0 : 1;

                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x56 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN20 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x58 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN29 = (decimal)BitConverter.ToDouble(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, 0x5A * 2, buff, 0, 2);// CrossHiLow(ref buff);
                HisGgdy.OTGLCN20 = (int)(buff[0] & (byte)128) == 0 ? 0 : 1;
                HisGgdy.OTGLCN21 = (int)(buff[0] & (byte)64) == 0 ? 0 : 1;
                HisGgdy.OTGLCN22 = (int)(buff[0] & (byte)32) == 0 ? 0 : 1;
                HisGgdy.OTGLCN23 = (int)(buff[0] & (byte)16) == 0 ? 0 : 1;
                //HisGgdy.JLLastGLCN41 = (int)(buff[0] & (byte)8) == 0 ? 0 : 1;
                //HisGgdy.JLLastGLCN42 = (int)(buff[0] & (byte)4) == 0 ? 0 : 1;
                //HisGgdy.JLLastGLCN43 = (int)(buff[0] & (byte)2) == 0 ? 0 : 1;
                //HisGgdy.JLLastGLCN44 = (int)(buff[0] & (byte)1) == 0 ? 0 : 1;

                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x5C * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN41 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x5E * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN60 = (decimal)BitConverter.ToDouble(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, 0x60 * 2, buff, 0, 2);// CrossHiLow(ref buff);
                HisGgdy.JLOTGLCN53 = (int)(buff[0] & (byte)128) == 0 ? 0 : 1;
                HisGgdy.JLOTGLCN54 = (int)(buff[0] & (byte)64) == 0 ? 0 : 1;
                HisGgdy.JLOTGLCN55 = (int)(buff[0] & (byte)32) == 0 ? 0 : 1;
                HisGgdy.JLOTGLCN56 = (int)(buff[0] & (byte)16) == 0 ? 0 : 1;
                //HisGgdy.JLOTGLCN57 = (int)(buff[0] & (byte)8) == 0 ? 0 : 1;
                //HisGgdy.JLOTGLCN58 = (int)(buff[0] & (byte)4) == 0 ? 0 : 1;
                //HisGgdy.JLLastGLCN43 = (int)(buff[0] & (byte)2) == 0 ? 0 : 1;
                //HisGgdy.JLLastGLCN44 = (int)(buff[0] & (byte)1) == 0 ? 0 : 1;

                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x70 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN45 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x72 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN61 = (decimal)BitConverter.ToDouble(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, 0x74 * 2, buff, 0, 2);// CrossHiLow(ref buff);
                HisGgdy.JLOTGLCN59 = (int)(buff[0] & (byte)128) == 0 ? 0 : 1;
                HisGgdy.JLOTGLCN60 = (int)(buff[0] & (byte)64) == 0 ? 0 : 1;
                HisGgdy.JLOTGLCN61 = (int)(buff[0] & (byte)32) == 0 ? 0 : 1;
                HisGgdy.JLOTGLCN62 = (int)(buff[0] & (byte)16) == 0 ? 0 : 1;
                //HisGgdy.JLLastGLCN41 = (int)(buff[0] & (byte)8) == 0 ? 0 : 1;
                //HisGgdy.JLLastGLCN42 = (int)(buff[0] & (byte)4) == 0 ? 0 : 1;
                //HisGgdy.JLLastGLCN43 = (int)(buff[0] & (byte)2) == 0 ? 0 : 1;
                //HisGgdy.JLLastGLCN44 = (int)(buff[0] & (byte)1) == 0 ? 0 : 1;


                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x76 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN20 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x78 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN29 = (decimal)BitConverter.ToDouble(buff, 0);
                buff = new byte[2];
                Buffer.BlockCopy(GetData, 0x7A * 2, buff, 0, 2);// CrossHiLow(ref buff);
                HisGgdy.OTGLCN20 = (int)(buff[0] & (byte)128) == 0 ? 0 : 1;
                HisGgdy.OTGLCN21 = (int)(buff[0] & (byte)64) == 0 ? 0 : 1;
                HisGgdy.OTGLCN22 = (int)(buff[0] & (byte)32) == 0 ? 0 : 1;
                HisGgdy.OTGLCN23 = (int)(buff[0] & (byte)16) == 0 ? 0 : 1;
                //HisGgdy.JLLastGLCN41 = (int)(buff[0] & (byte)8) == 0 ? 0 : 1;
                //HisGgdy.JLLastGLCN42 = (int)(buff[0] & (byte)4) == 0 ? 0 : 1;
                //HisGgdy.JLLastGLCN43 = (int)(buff[0] & (byte)2) == 0 ? 0 : 1;
                //HisGgdy.JLLastGLCN44 = (int)(buff[0] & (byte)1) == 0 ? 0 : 1;

                //1#燃气流量计
                buff = new byte[8];
                Buffer.BlockCopy(GetData, 0x80 * 2, buff, 0, buff.Length); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN67 = (decimal)BitConverter.ToDouble(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x84 * 2, buff, 0, buff.Length); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN68 = (decimal)BitConverter.ToDouble(buff, 0);
                buff = new byte[8];
                Buffer.BlockCopy(GetData, 0x86 * 2, buff, 0, buff.Length); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN69 = (decimal)BitConverter.ToDouble(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x8A * 2, buff, 0, buff.Length); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN70 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x8C * 2, buff, 0, buff.Length); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN71 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x8E * 2, buff, 0, buff.Length); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN72 = (decimal)BitConverter.ToDouble(buff, 0);

                //2#燃气流量计
                buff = new byte[8];
                Buffer.BlockCopy(GetData, 0x90 * 2, buff, 0, buff.Length); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN73 = (decimal)BitConverter.ToDouble(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x94 * 2, buff, 0, buff.Length); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN74 = (decimal)BitConverter.ToDouble(buff, 0);
                buff = new byte[8];
                Buffer.BlockCopy(GetData, 0x96 * 2, buff, 0, buff.Length); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN75 = (decimal)BitConverter.ToDouble(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x9A * 2, buff, 0, buff.Length); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN76 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x9C * 2, buff, 0, buff.Length); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN77 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x9E * 2, buff, 0, buff.Length); //CrossHiLow(ref buff);
                HisGgdy.OTGLCN78 = (decimal)BitConverter.ToDouble(buff, 0);                


                #endregion
                //存储数据库
                try
                {
                    glfda.heatingdb.AddToONTimeGLCommNet(HisGgdy);
                    glfda.heatingdb.SaveChanges();

                }
                catch (Exception ex)
                {
                    log.Error("glcn:his-----");
                }
                //try
                //{
                //    glfda.heatingdb.AddToONTimeGLCommNet(ChangeNewToHisGggyObj(HisGgdy));
                //    glfda.heatingdb.SaveChanges();
                //}
                //catch (Exception ex)
                //{
                //    log.Error("glcn:his-----");
                //}
            }
            catch (Exception ex)
            {
                log.Error("SanFeng Updata:Parse1" + ex.InnerException + "-" + ex.Message);
                resualt = -1;
            }
            
            try
            {
                #region 解析最新锅炉数据对象
                int byteIndex = 0X0100 * 2;
                for (int i = 0; i < 2; i++)
                {
                    HisLdy[i] = new OTGLUnitData();
                    HisLdy[i].GathDt = dtGatherTime;
                    HisLdy[i].Id = ldys[i].Id;
                    HisLdy[i].bsO_Id = Org.Id;
                    byteIndex = 0x100 * 2 + i * 0x100;
                    
                    buff = new byte[4];
                    //水温度OTLDYF35
                    Buffer.BlockCopy(GetData, byteIndex, buff, 0, 4);// CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF3 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4);// CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF1 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF1 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF37 = (decimal)BitConverter.ToSingle(buff, 0);
                    //烟气温度
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF4 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF5 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF38 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF6 = (decimal)BitConverter.ToSingle(buff, 0);
                    //水压
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF7 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF36 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF8 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF39 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF9 = (decimal)BitConverter.ToSingle(buff, 0);
                    //燃气压力
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF40 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF41 = (decimal)BitConverter.ToSingle(buff, 0);
                    //风压
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF23 = (decimal)BitConverter.ToSingle(buff, 0);
                    //液位
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF42 = (decimal)BitConverter.ToSingle(buff, 0);
                    //含氮量
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF43 = (decimal)BitConverter.ToSingle(buff, 0);

                    //热量表
                    byteIndex = 0x140 * 2 + i * 0x100;
                    
                    Buffer.BlockCopy(GetData, byteIndex, buff, 0, 4); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF14 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF13 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF12 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF11 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF16 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF15 = (decimal)BitConverter.ToSingle(buff, 0);
                    //燃气流量计
                    byteIndex = 0x150 * 2 + i * 0x100;
                    buff = new byte[8];
                    Buffer.BlockCopy(GetData, byteIndex, buff, 0, buff.Length); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF44 = (decimal)BitConverter.ToSingle(buff, 0);
                    buff = new byte[4];
                    Buffer.BlockCopy(GetData, byteIndex += buff.Length, buff, 0, buff.Length); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF45 = (decimal)BitConverter.ToSingle(buff, 0);
                    buff = new byte[8];
                    Buffer.BlockCopy(GetData, byteIndex += buff.Length, buff, 0, buff.Length); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF46 = (decimal)BitConverter.ToSingle(buff, 0);
                    buff = new byte[4];
                    Buffer.BlockCopy(GetData, byteIndex += buff.Length, buff, 0, buff.Length); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF47 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += buff.Length, buff, 0, buff.Length); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF48= (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += buff.Length, buff, 0, buff.Length); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF49 = (decimal)BitConverter.ToSingle(buff, 0);
                    //循环泵变频器频率
                    Buffer.BlockCopy(GetData, byteIndex += buff.Length, buff, 0, buff.Length); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF5 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += buff.Length, buff, 0, buff.Length); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF5 = (decimal)BitConverter.ToSingle(buff, 0);
                    //循环泵状态
                    buff = new byte[2];
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 2);// CrossHiLow(ref buff);
                    HisGgdy.OTGLCN20 = (int)(buff[0] & (byte)128) == 0 ? 0 : 1;
                    HisGgdy.OTGLCN21 = (int)(buff[0] & (byte)64) == 0 ? 0 : 1;
                    HisGgdy.OTGLCN22 = (int)(buff[0] & (byte)32) == 0 ? 0 : 1;
                    HisGgdy.OTGLCN23 = (int)(buff[0] & (byte)16) == 0 ? 0 : 1;
                    //HisGgdy.JLLastGLCN41 = (int)(buff[0] & (byte)8) == 0 ? 0 : 1;
                    //HisGgdy.JLLastGLCN42 = (int)(buff[0] & (byte)4) == 0 ? 0 : 1;
                    //HisGgdy.JLLastGLCN43 = (int)(buff[0] & (byte)2) == 0 ? 0 : 1;
                    //HisGgdy.JLLastGLCN44 = (int)(buff[0] & (byte)1) == 0 ? 0 : 1;
                    
                    //循环泵用电累计量
                    byteIndex = 0x166 * 2 + i * 0x100;
                    buff = new byte[4];
                    Buffer.BlockCopy(GetData, byteIndex, buff, 0, buff.Length); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF29 = (decimal)BitConverter.ToSingle(buff, 0);
                    //鼓风机
                    Buffer.BlockCopy(GetData, byteIndex += buff.Length, buff, 0, buff.Length); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF21 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += buff.Length, buff, 0, buff.Length); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF50 = (decimal)BitConverter.ToSingle(buff, 0);
                    buff = new byte[2];//not given
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 2);// CrossHiLow(ref buff);
                    HisGgdy.OTGLCN20 = (int)(buff[0] & (byte)128) == 0 ? 0 : 1;
                    HisGgdy.OTGLCN21 = (int)(buff[0] & (byte)64) == 0 ? 0 : 1;
                    HisGgdy.OTGLCN22 = (int)(buff[0] & (byte)32) == 0 ? 0 : 1;
                    HisGgdy.OTGLCN23 = (int)(buff[0] & (byte)16) == 0 ? 0 : 1;
                    //HisGgdy.JLLastGLCN41 = (int)(buff[0] & (byte)8) == 0 ? 0 : 1;
                    //HisGgdy.JLLastGLCN42 = (int)(buff[0] & (byte)4) == 0 ? 0 : 1;
                    //HisGgdy.JLLastGLCN43 = (int)(buff[0] & (byte)2) == 0 ? 0 : 1;
                    //HisGgdy.JLLastGLCN44 = (int)(buff[0] & (byte)1) == 0 ? 0 : 1;
                    
                    //鼓风机用电累计量
                    buff = new byte[4];
                    byteIndex = 0x16B * 2 + i * 0x100;
                    Buffer.BlockCopy(GetData, byteIndex, buff, 0, buff.Length); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF22 = (decimal)BitConverter.ToUInt32(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += buff.Length, buff, 0, buff.Length); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF24 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += buff.Length, buff, 0, buff.Length); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF32 = (decimal)BitConverter.ToSingle(buff, 0);
                    
                    Buffer.BlockCopy(GetData, byteIndex += buff.Length, buff, 0, buff.Length); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF30 = BitConverter.ToInt32(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += buff.Length, buff, 0, buff.Length); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF33 = BitConverter.ToInt32(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += buff.Length, buff, 0, buff.Length); //CrossHiLow(ref buff);
                    HisLdy[i].OTLDYF31 = BitConverter.ToInt32(buff, 0);


                    //保存数据库
                    glfda.heatingdb.AddToOTGLUnitData(HisLdy[i]);
                    glfda.heatingdb.SaveChanges();
                }
                #endregion
            }
            catch (Exception ex)
            {
                log.Error("Sanfeng Updata:Parse3" + ex.InnerException + "-" + ex.Message);
                resualt = -3;
            }

            try
            {
                #region 本地换热站
                for (int i = 0; i < HisHrzJz.Length; i++)
                {
                    HisHrzJz[i] = new OnTimeHRZCollectData();
                    HisHrzJz[i].GathDt = dtGatherTime;
                    HisHrzJz[i].Det_Id = ldys[i].Id;
                    HisHrzJz[i].Id = Org.Id;
                    int byteIndex = 0x500 * 2 + i * 0x100;
                    
                    buff = new byte[4];
                    //本地室外环境温度
                    Buffer.BlockCopy(GetData, byteIndex, buff, 0, 4);// CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT7 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4);// CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT13 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT6 = (decimal)BitConverter.ToSingle(buff, 0);
                
                    //一次网供水温度
                    byteIndex = 0x510 * 2 + i * 0x100;
                    Buffer.BlockCopy(GetData, byteIndex, buff, 0, 4); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT1 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT2 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT3 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT4 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT5 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT8 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT9 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT10= (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT11 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT12 = (decimal)BitConverter.ToSingle(buff, 0);
                    
                    //1#循环泵变频器频率
                    byteIndex = 0x530 * 2 + i * 0x100;
                    //1#循环泵变频器频率
                    Buffer.BlockCopy(GetData, byteIndex, buff, 0, 4); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT14 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex+=4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT15 = (decimal)BitConverter.ToSingle(buff, 0);
                    buff = new byte[2];//状态按位解码
                    Buffer.BlockCopy(GetData, byteIndex+=4, buff, 0, 2); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT36 = BitConverter.ToInt32(buff, 0);
                    byteIndex += 2;
                    //1#循环泵电量
                    buff = new byte[4];
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 2); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT32 = BitConverter.ToInt64(buff, 0);

                    //2#循环泵变频器频率
                    byteIndex = 0x538 * 2 + i * 0x100;
                    //2#循环泵变频器频率
                    Buffer.BlockCopy(GetData, byteIndex, buff, 0, 4); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT142 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex, buff, 0, 4); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT152 = (decimal)BitConverter.ToSingle(buff, 0);
                    buff = new byte[2];//状态按位解码
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 2); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT37 = BitConverter.ToInt32(buff, 0);
                    byteIndex += 2;
                    //2#循环泵电量
                    buff = new byte[4];
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 2); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT33 = BitConverter.ToInt64(buff, 0);

                    //1#补水泵变频器频率
                    byteIndex = 0x540 * 2 + i * 0x100;
                    //1#补水泵变频器频率
                    Buffer.BlockCopy(GetData, byteIndex, buff, 0, 4); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT16 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT17 = (decimal)BitConverter.ToSingle(buff, 0);
                    buff = new byte[2];//状态按位解码
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 2); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT38 = BitConverter.ToInt32(buff, 0);
                    byteIndex += 2;
                    //1#补水泵电量
                    buff = new byte[4];
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 2); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT34 = BitConverter.ToInt64(buff, 0);

                    //2#补水泵变频器频率
                    byteIndex = 0x548 * 2 + i * 0x100;
                    //2#补水泵变频器频率
                    Buffer.BlockCopy(GetData, byteIndex, buff, 0, 4); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT162 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex, buff, 0, 4); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT172 = (decimal)BitConverter.ToSingle(buff, 0);
                    buff = new byte[2];//状态按位解码
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 2); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT39 = BitConverter.ToInt32(buff, 0);
                    byteIndex += 2;
                    //2#补水泵电量
                    buff = new byte[4];
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 2); //CrossHiLow(ref buff);
                    HisHrzJz[i].HRZCollOT35 = BitConverter.ToInt64(buff, 0);
       
                     //保存数据库
                    glfda.heatingdb.AddToOnTimeHRZCollectData(HisHrzJz[i]);
                    glfda.heatingdb.SaveChanges();
                }

                #endregion
            }
            catch (Exception ex)
            {
                log.Error("Sanfeng Updata:Parse3" + ex.InnerException + "-" + ex.Message);
                resualt = -3;
            }

            return resualt;
        }

        

        private int GetbitValue(byte input, int index)
        {
            if (index > sizeof(byte))
            {
                return -1;
            }
            //左移到最高位
            int value = input << (sizeof(byte) - 1 - index);
            //右移到最低位
            value = value >> (sizeof(byte) - 1);
            return value;
        }

        #region 不需要

        //private ONTimeGLCommNet ChangeNewToHisGggyObj(LastGLCommNet newobj)
        //{
        //    ONTimeGLCommNet hisobj = new ONTimeGLCommNet();
        //    hisobj.DeviDetaID = newobj.DeviDetaID;
        //    hisobj.Id = newobj.Id;
        //    //hisobj.AlarmInfoHisCN = newobj.LastGLCNAlarmInfo;
        //    hisobj.OTGLCN0 = newobj.LastGLCN0;
        //    hisobj.OTGLCN1 = newobj.LastGLCN1;
        //    hisobj.OTGLCN2 = newobj.LastGLCN2;
        //    hisobj.OTGLCN3 = newobj.LastGLCN3;
        //    hisobj.OTGLCN4 = newobj.LastGLCN4;
        //    hisobj.OTGLCN5 = newobj.LastGLCN5;
        //    hisobj.OTGLCN6 = newobj.LastGLCN6;
        //    hisobj.OTGLCN7 = newobj.LastGLCN7;
        //    hisobj.OTGLCN8 = newobj.LastGLCN8;
        //    hisobj.OTGLCN9 = newobj.LastGLCN9;
        //    hisobj.OTGLCN10 = newobj.LastGLCN10;
        //    hisobj.OTGLCN11 = newobj.LastGLCN11;
        //    hisobj.OTGLCN12 = newobj.LastGLCN12;
        //    hisobj.OTGLCN13 = newobj.LastGLCN13;
        //    hisobj.OTGLCN14 = newobj.LastGLCN14;
        //    hisobj.OTGLCN15 = newobj.LastGLCN15;
        //    hisobj.OTGLCN16 = newobj.LastGLCN16;
        //    hisobj.OTGLCN17 = newobj.LastGLCN17;
        //    hisobj.OTGLCN18 = newobj.LastGLCN18;
        //    hisobj.OTGLCN19 = newobj.LastGLCN19;
        //    hisobj.OTGLCN20 = newobj.LastGLCN20;
        //    hisobj.OTGLCN21 = newobj.LastGLCN21;
        //    hisobj.OTGLCN22 = newobj.LastGLCN22;
        //    hisobj.OTGLCN23 = newobj.LastGLCN23;
        //    hisobj.OTGLCN24 = newobj.LastGLCN24;
        //    hisobj.OTGLCN25 = newobj.LastGLCN25;
        //    hisobj.OTGLCN26 = newobj.LastGLCN26;
        //    hisobj.OTGLCN27 = newobj.LastGLCN27;
        //    hisobj.OTGLCN28 = newobj.LastGLCN28;
        //    hisobj.OTGLCN29 = newobj.LastGLCN29;
        //    hisobj.OTGLCN30 = newobj.LastGLCN30;
        //    hisobj.OTGLCN31 = newobj.LastGLCN31;
        //    hisobj.OTGLCN32 = newobj.LastGLCN32;
        //    hisobj.OTGLCN33 = newobj.LastGLCN33;
        //    hisobj.OTGLCN34 = newobj.LastGLCN34;
        //    hisobj.OTGLCN35 = newobj.LastGLCN35;
        //    hisobj.JLOTGLCN36 = (decimal)newobj.JLLastGLCN36;
        //    hisobj.JLOTGLCN37 = newobj.JLLastGLCN37;
        //    hisobj.JLOTGLCN38 = newobj.JLLastGLCN38;
        //    hisobj.JLOTGLCN39 = newobj.JLLastGLCN39;
        //    hisobj.JLOTGLCN40 = newobj.JLLastGLCN40;
        //    hisobj.JLOTGLCN41 = newobj.JLLastGLCN41;
        //    hisobj.JLOTGLCN42 = newobj.JLLastGLCN42;
        //    hisobj.JLOTGLCN43 = newobj.JLLastGLCN43;
        //    hisobj.JLOTGLCN44 = newobj.JLLastGLCN44;
        //    hisobj.JLOTGLCN45 = newobj.JLLastGLCN45;
        //    hisobj.JLOTGLCN46 = newobj.JLLastGLCN46;
        //    hisobj.JLOTGLCN47 = newobj.JLLastGLCN47;
        //    hisobj.JLOTGLCN48 = newobj.JLLastGLCN48;
        //    hisobj.JLOTGLCN49 = newobj.JLLastGLCN49;
        //    hisobj.JLOTGLCN50 = newobj.JLLastGLCN50;
        //    hisobj.JLOTGLCN51 = newobj.JLLastGLCN51;
        //    hisobj.JLOTGLCN52 = newobj.JLLastGLCN52;
        //    hisobj.JLOTGLCN53 = newobj.JLLastGLCN53;
        //    hisobj.JLOTGLCN54 = newobj.JLLastGLCN54;
        //    hisobj.JLOTGLCN55 = newobj.JLLastGLCN55;
        //    hisobj.JLOTGLCN56 = newobj.JLLastGLCN56;
        //    hisobj.JLOTGLCN57 = newobj.JLLastGLCN57;
        //    hisobj.JLOTGLCN58 = newobj.JLLastGLCN58;
        //    hisobj.JLOTGLCN59 = newobj.JLLastGLCN59;
        //    hisobj.JLOTGLCN60 = newobj.JLLastGLCN60;
        //    hisobj.JLOTGLCN61 = newobj.JLLastGLCN61;
        //    hisobj.JLOTGLCN62 = newobj.JLLastGLCN62;
        //    hisobj.JLOTGLCN63 = newobj.JLLastGLCN63;
        //    hisobj.JLOTGLCN64 = newobj.JLLastGLCN64;
        //    hisobj.JLOTGLCN65 = newobj.JLLastGLCN65;
        //    hisobj.JLOTGLCN66 = newobj.JLLastGLCN66;
        //    hisobj.JLOTGLCN67 = newobj.JLLastGLCN67;
        //    hisobj.JLOTGLCN68 = newobj.JLLastGLCN68;
        //    hisobj.JLOTGLCN69 = newobj.JLLastGLCN69;
        //    hisobj.JLOTGLCN70 = newobj.JLLastGLCN70;
        //    hisobj.JLOTGLCN71 = newobj.JLLastGLCN71;
        //    hisobj.JLOTGLCN72 = newobj.JLLastGLCN72;
        //    hisobj.JLOTGLCN73 = newobj.JLLastGLCN73;
        //    hisobj.JLOTGLCN74 = newobj.JLLastGLCN74;
        //    hisobj.JLOTGLCN75 = newobj.JLLastGLCN75;
        //    hisobj.JLOTGLCN76 = newobj.JLLastGLCN76;
        //    hisobj.JLOTGLCN77 = newobj.JLLastGLCN77;
        //    hisobj.JLOTGLCN78 = newobj.JLLastGLCN78;
        //    hisobj.JLOTGLCN79 = newobj.JLLastGLCN79;
        //    hisobj.JLOTGLCN80 = newobj.JLLastGLCN80;
        //    hisobj.JLOTGLCN81 = newobj.JLLastGLCN81;
        //    hisobj.JLOTGLCN82 = newobj.JLLastGLCN82;
        //    hisobj.JLOTGLCN83 = newobj.JLLastGLCN83;
        //    hisobj.JLOTGLCN84 = newobj.JLLastGLCN84;

        //    return hisobj;


              
        //}
        //private OTGLUnitData ChangeNEwToHisLdyObj(LastGLUnitData newobj)
        //{
        //    OTGLUnitData hisobj = new OTGLUnitData();
        //    hisobj.DeviDetaID = newobj.DeviDetaID;
        //    hisobj.Id = newobj.Id;
        //    hisobj.OTLDYF0 = newobj.LastLDYF0;
        //    hisobj.OTLDYF2 = newobj.LastLDYF2;
        //    hisobj.OTLDYF3 = newobj.LastLDYF3;
        //    hisobj.OTLDYF4 = newobj.LastLDYF4;
        //    hisobj.OTLDYF5 = newobj.LastLDYF5;
        //    hisobj.OTLDYF6 = newobj.LastLDYF6;
        //    hisobj.OTLDYF7 = newobj.LastLDYF7;
        //    hisobj.OTLDYF8 = newobj.LastLDYF8;
        //    hisobj.OTLDYF9 = newobj.LastLDYF9;
        //    hisobj.OTLDYF10 = newobj.LastLDYF10;
        //    hisobj.OTLDYF11 = newobj.LastLDYF11;
        //    hisobj.OTLDYF12 = newobj.LastLDYF12;
        //    hisobj.OTLDYF13 = newobj.LastLDYF13;
        //    hisobj.OTLDYF14 = newobj.LastLDYF14;
        //    hisobj.OTLDYF15 = newobj.LastLDYF15;
        //    hisobj.OTLDYF16 = newobj.LastLDYF16;
        //    hisobj.OTLDYF17 = newobj.LastLDYF17;
        //    hisobj.OTLDYF18 = newobj.LastLDYF18;
        //    hisobj.OTLDYF19 = newobj.LastLDYF19;
        //    hisobj.OTLDYF20 = newobj.LastLDYF20;
        //    hisobj.OTLDYF21 = newobj.LastLDYF21;
        //    hisobj.OTLDYF22 = newobj.LastLDYF22;
        //    hisobj.OTLDYF23 = newobj.LastLDYF23;
        //    hisobj.OTLDYF24 = newobj.LastLDYF24;
        //    hisobj.OTLDYF25 = newobj.LastLDYF25;
        //    hisobj.OTLDYF26 = newobj.LastLDYF26;
        //    hisobj.OTLDYF27 = newobj.LastLDYF27;
        //    hisobj.OTLDYF28 = newobj.LastLDYF28;
        //    hisobj.OTLDYF29 = newobj.LastLDYF29;
        //    hisobj.OTLDYF30 = newobj.LastLDYF30;
        //    hisobj.OTLDYF31 = newobj.LastLDYF31;
        //    hisobj.OTLDYF32 = newobj.LastLDYF32;
        //    hisobj.JLOTLDYF33 = newobj.JLLastLDYF33;
        //    hisobj.JLOTLDYF34 = newobj.JLLastLDYF34;
        //    hisobj.JLOTLDYF35 = newobj.JLLastLDYF35;
        //    hisobj.JLOTLDYF36 = newobj.JLLastLDYF36;
        //    hisobj.JLOTLDYF37 = newobj.JLLastLDYF37;
        //    hisobj.JLOTLDYF38 = newobj.JLLastLDYF38;
        //    hisobj.JLOTLDYF39 = newobj.JLLastLDYF39;
        //    hisobj.JLOTLDYF40 = newobj.JLLastLDYF40;
        //    hisobj.JLOTLDYF41 = newobj.JLLastLDYF41;
        //    hisobj.JLOTLDYF42 = newobj.JLLastLDYF42;
        //    hisobj.JLOTLDYF43 = newobj.JLLastLDYF43;
        //    hisobj.JLOTLDYF44 = newobj.JLLastLDYF44;
        //    hisobj.JLOTLDYF45 = newobj.JLLastLDYF45;
        //    hisobj.JLOTLDYF46 = newobj.JLLastLDYF46;
        //    hisobj.JLOTLDYF47 = newobj.JLLastLDYF47;
        //    hisobj.JLOTLDYF48 = newobj.JLLastLDYF48;
        //    hisobj.JLOTLDYF49 = newobj.JLLastLDYF49;

        //    //hisobj.OTAlarmInfoLDY = newobj.LastLDYAlarmInfo;
        //    return hisobj;

        //}

        #endregion

    }
}
