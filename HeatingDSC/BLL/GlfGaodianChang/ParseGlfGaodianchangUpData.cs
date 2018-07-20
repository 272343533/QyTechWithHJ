using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QyTech.Communication;
using SunMvcExpress.Dao;
using QyTech.HDGprs;

namespace HeatingDSC.BLL
{
    public class ParseGlfGaodianchangUpData: IProduct
    {
       // public LastGLCommNet newJLGgdy = new LastGLCommNet();
        //public LastGLUnitData[] newJLLdy = new LastGLUnitData[2];//应该从数据库里获取炉单元的行数

        public ONTimeGLCommNet HisJLGgdy = new ONTimeGLCommNet();
        public OTGLUnitData[] HisJLLdy = new OTGLUnitData[2];

        //public HisJLAlarmGatherData[] HisJLAlarm = new HisJLAlarmGatherData[9];
        
         public ParseGlfGaodianchangUpData()
        {
            Producttype = ProductType.GlfofGaodianchangUpData;
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


            HisJLGgdy.Det_Id = ggdy.Id;
            HisJLGgdy.Id = Org.Id;

            string StationID = recdPtr.m_userid;        //DTU身份识别码
            DateTime dtGatherTime = DateTime.Parse(recdPtr.m_recv_date);    //数据接收时间
            HisJLGgdy.GathDt = dtGatherTime;
            try
            {
                #region 解析到最新公共部分数据对象
                buff = new byte[8];
                Buffer.BlockCopy(GetData, 0x4 * 2, buff, 0, 8); //CrossHiLow(ref buff);
                HisJLGgdy.OTGLCN25 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0xA * 2, buff, 0, 8); //CrossHiLow(ref buff);
                HisJLGgdy.OTGLCN26 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x14 * 2, buff, 0, 8); //CrossHiLow(ref buff);
                HisJLGgdy.OTGLCN27 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x1A * 2, buff, 0, 8); //CrossHiLow(ref buff);
                HisJLGgdy.OTGLCN28 = (decimal)BitConverter.ToDouble(buff, 0);



                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisJLGgdy.OTGLCN1 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x2 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisJLGgdy.OTGLCN2 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x8 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisJLGgdy.OTGLCN3 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0xE * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisJLGgdy.OTGLCN4 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x10 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisJLGgdy.OTGLCN5 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x12 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisJLGgdy.OTGLCN6 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x18 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisJLGgdy.OTGLCN7 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x1E * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisJLGgdy.OTGLCN8 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x20 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisJLGgdy.OTGLCN9 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x22 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisJLGgdy.OTGLCN10 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x28 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisJLGgdy.OTGLCN11 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x2A * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisJLGgdy.OTGLCN12 = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x2C * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisJLGgdy.OTGLCN13 = (decimal)BitConverter.ToSingle(buff, 0);
               
                Buffer.BlockCopy(GetData, 0x2E * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisJLGgdy.OTGLCN14 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x30 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisJLGgdy.OTGLCN15 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x32 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisJLGgdy.OTGLCN19 = (decimal)BitConverter.ToSingle(buff, 0);
               
                Buffer.BlockCopy(GetData, 0x34 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisJLGgdy.OTGLCN20 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x36 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                HisJLGgdy.OTGLCN24 = (decimal)BitConverter.ToSingle(buff, 0);

                #endregion
                //存储数据库
                //try
                //{
                //    glfda.heatingdb.AddToLastGLCommNet(newJLGgdy);
                //    glfda.heatingdb.SaveChanges();

                //}
                //catch (Exception ex)
                //{
                //    log.Error("glcn:last-----");
                //}
                try
                {
                    glfda.heatingdb.AddToONTimeGLCommNet(HisJLGgdy);
                    glfda.heatingdb.SaveChanges();
                }
                catch (Exception ex)
                {
                    log.Error("glcn:his-----");
                }
            }
            catch (Exception ex)
            {
                log.Error("GaoDianChang Updata:Parse1" + ex.InnerException + "-" + ex.Message);
                resualt = -1;
            }
            
            try
            {
                #region 解析最新锅炉数据对象
                buff = new byte[4];
                int byteIndex = 0X0039 * 2;
                for (int i = 0; i < 2; i++)
                {
                    HisJLLdy[i] = new OTGLUnitData();
                    HisJLLdy[i].GathDt = dtGatherTime;
                    HisJLLdy[i].Id = ldys[i].Id;
                    HisJLLdy[i].bsO_Id = Org.Id;
                    switch (i)
                    {
                        case 0:
                            byteIndex = 0x0040 * 2;
                            break;
                        case 1:
                            byteIndex = 0x0060 * 2;
                            break;
                    }
                    Buffer.BlockCopy(GetData, byteIndex, buff, 0, 4);// CrossHiLow(ref buff);
                    HisJLLdy[i].OTLDYF1 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4);// CrossHiLow(ref buff);
                    HisJLLdy[i].OTLDYF2 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisJLLdy[i].OTLDYF3 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisJLLdy[i].OTLDYF4 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    HisJLLdy[i].OTLDYF5 = (decimal)BitConverter.ToSingle(buff, 0);

                    
                    //保存数据库
                    glfda.heatingdb.AddToOTGLUnitData(HisJLLdy[i]);
                    glfda.heatingdb.SaveChanges();

                   
                }
                #endregion
            }
            catch (Exception ex)
            {
                log.Error("GaoDianChang Updata:Parse3" + ex.InnerException + "-" + ex.Message);
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
        
    }
}
