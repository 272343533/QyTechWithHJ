using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QyTech.Core.BLL;
using SunMvcExpress.Core.BLL;
using System.Reflection;
using SunMvcExpress.Dao;
using QyTech.Communication;
using QyTech.Protocal.Modbus;
using QyTech.HDGprs;
using QyTech.Protocal;
using HjCommDA;
using QyTech.Core.BLL;
using log4net;

namespace QyTech.ProtocalGlfJinLai
{
    public class ReadData1 : IProtocal
    {

        public HisJLAlarmGatherData HisJLAlarm = new HisJLAlarmGatherData();
   
             public ReadData1(int bsP_Id):base(bsP_Id)
        {
         
        }
        public override int Parse(HDGprs.GPRS_DATA_RECORD recdPtr)
        {
            base.Parse(recdPtr);


            ONTimeGLCommNet ggdyobj = new ONTimeGLCommNet();
            OTGLUnitData ldyobj = new OTGLUnitData();

            Dev = BaseDABll.GetDtuProduct(recdPtr.m_userid);
            Org = BaseDABll.GetOrganize((Guid)Dev.AZWZ);

            List<DetailDevice> devs = BaseDABll.GetDetailDevice(Dev.Id);


            int resualt = 1;
            int byteIndex = 0;

            ggdyobj.Det_Id = devs[0].Id;
            ggdyobj.bsO_Id = Org.Id;

            ggdyobj.GathDt = DateTime.Parse(recdPtr.m_recv_date);    //数据接收时间
            try
            {
                #region 解析到最新公共部分数据对象

                try
                {
                    buff = new byte[4];
                    Buffer.BlockCopy(GetData, byteIndex, buff, 0, 4); CrossHiLow(ref buff);
                    ggdyobj.OTGLCN1 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                    ggdyobj.OTGLCN2 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                    ggdyobj.OTGLCN3 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                    ggdyobj.OTGLCN4 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                    ggdyobj.OTGLCN5 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                    ggdyobj.OTGLCN9 = (decimal)BitConverter.ToSingle(buff, 0);

                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                    ggdyobj.OTGLCN28 = (decimal)BitConverter.ToUInt32(buff, 0);

                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                    ggdyobj.JLOTGLCN36 = (long)BitConverter.ToUInt32(buff, 0);

                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                    ggdyobj.OTGLCN25 = (decimal)BitConverter.ToUInt32(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                    ggdyobj.OTGLCN26 = (decimal)BitConverter.ToUInt32(buff, 0);
                    //Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4);
                    //ggdyobj.OTGLCN27 = (decimal)BitConverter.ToSingle(buff, 0);
                    byteIndex = 0x001C * 2;//补水泵
                    Buffer.BlockCopy(GetData, byteIndex, buff, 0, 2); CrossHiLow(ref buff);
                    ggdyobj.JLOTGLCN37 = (int)(buff[0] & (byte)128) == 0 ? 0 : 1;
                    ggdyobj.JLOTGLCN38 = (int)(buff[0] & (byte)64) == 0 ? 0 : 1;
                    ggdyobj.JLOTGLCN39 = (int)(buff[0] & (byte)32) == 0 ? 0 : 1;
                    ggdyobj.JLOTGLCN40 = (int)(buff[0] & (byte)16) == 0 ? 0 : 1;
                    ggdyobj.JLOTGLCN41 = (int)(buff[0] & (byte)8) == 0 ? 0 : 1;
                    ggdyobj.JLOTGLCN42 = (int)(buff[0] & (byte)4) == 0 ? 0 : 1;
                    ggdyobj.JLOTGLCN43 = (int)(buff[0] & (byte)2) == 0 ? 0 : 1;
                    ggdyobj.JLOTGLCN44 = (int)(buff[0] & (byte)1) == 0 ? 0 : 1;
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                    ggdyobj.JLOTGLCN45 = (int)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                    ggdyobj.JLOTGLCN46 = (decimal)Math.Round(BitConverter.ToSingle(buff, 0), 2);

                    //循环泵1
                    byteIndex = 0x0021 * 2;
                    Buffer.BlockCopy(GetData, byteIndex, buff, 0, 2); CrossHiLow(ref buff);
                    ggdyobj.JLOTGLCN47 = (int)(buff[0] & (byte)128) == 0 ? 0 : 1;
                    ggdyobj.JLOTGLCN48 = (int)(buff[0] & (byte)64) == 0 ? 0 : 1;
                    ggdyobj.JLOTGLCN49 = (int)(buff[0] & (byte)32) == 0 ? 0 : 1;
                    ggdyobj.JLOTGLCN50 = (int)(buff[0] & (byte)16) == 0 ? 0 : 1;
                    Buffer.BlockCopy(GetData, byteIndex += 2, buff, 0, 4); CrossHiLow(ref buff);
                    ggdyobj.JLOTGLCN51 = (decimal)Math.Round(BitConverter.ToSingle(buff, 0), 2);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                    ggdyobj.JLOTGLCN52 = (decimal)Math.Round(BitConverter.ToSingle(buff, 0), 2);

                    byteIndex = 0x0026 * 2;
                    Buffer.BlockCopy(GetData, byteIndex, buff, 0, 2); CrossHiLow(ref buff);
                    ggdyobj.JLOTGLCN53 = (int)(buff[0] & (byte)128) == 0 ? 0 : 1;
                    ggdyobj.JLOTGLCN54 = (int)(buff[0] & (byte)64) == 0 ? 0 : 1;
                    ggdyobj.JLOTGLCN55 = (int)(buff[0] & (byte)32) == 0 ? 0 : 1;
                    ggdyobj.JLOTGLCN56 = (int)(buff[0] & (byte)16) == 0 ? 0 : 1;
                    Buffer.BlockCopy(GetData, byteIndex += 2, buff, 0, 4); CrossHiLow(ref buff);
                    ggdyobj.JLOTGLCN57 = (decimal)Math.Round(BitConverter.ToSingle(buff, 0), 2);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                    ggdyobj.JLOTGLCN58 = (decimal)Math.Round(BitConverter.ToSingle(buff, 0), 2);

                    byteIndex = 0x002B * 2;
                    Buffer.BlockCopy(GetData, byteIndex, buff, 0, 2);
                    ggdyobj.JLOTGLCN59 = (buff[0] & (byte)128) == 0 ? 0 : 1;
                    ggdyobj.JLOTGLCN60 = (buff[0] & (byte)64) == 0 ? 0 : 1;
                    ggdyobj.JLOTGLCN61 = (buff[0] & (byte)32) == 0 ? 0 : 1;
                    ggdyobj.JLOTGLCN62 = (buff[0] & (byte)16) == 0 ? 0 : 1;
                    Buffer.BlockCopy(GetData, byteIndex += 2, buff, 0, 4); CrossHiLow(ref buff);
                    ggdyobj.JLOTGLCN63 = (decimal)Math.Round(BitConverter.ToSingle(buff, 0), 2);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                    ggdyobj.JLOTGLCN64 = (decimal)Math.Round(BitConverter.ToSingle(buff, 0), 2);

                    
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }

                //新增数据
                //气象数据
                //byteIndex = 0X00F0 * 2;
                //Buffer.BlockCopy(GetData, byteIndex, buff, 0, 4); CrossHiLow(ref buff);
                //ggdyobj.OTGLCN30 = (decimal)Math.Round(BitConverter.ToSingle(buff, 0), 2);
                //Buffer.BlockCopy(GetData, byteIndex+=4, buff, 0, 4); CrossHiLow(ref buff);
                //ggdyobj.OTGLCN31 = (decimal)Math.Round(BitConverter.ToSingle(buff, 0), 2);
                //Buffer.BlockCopy(GetData, byteIndex+=4, buff, 0, 4); CrossHiLow(ref buff);
                //ggdyobj.OTGLCN32 = (decimal)Math.Round(BitConverter.ToSingle(buff, 0), 2);
                EntityManager<ONTimeGLCommNet>.Add<ONTimeGLCommNet>(ggdyobj);
                NewGLCommNet newobj = EntityManager<NewGLCommNet>.GetBySql<NewGLCommNet>("Det_Id='" + ggdyobj.Det_Id.ToString() + "'");
                if (newobj == null)
                {
                    EntityOperate.Copy<NewGLCommNet>(ggdyobj, newobj, "Det_Id");
                    EntityManager<NewGLCommNet>.Add<NewGLCommNet>(newobj);
                }
                else
                {
                    EntityOperate.Copy<NewGLCommNet>(ggdyobj, newobj, "Det_Id");
                    EntityManager<NewGLCommNet>.Modify<NewGLCommNet>(newobj);
                }


                #endregion
              

            }
            catch (Exception ex)
            {
                log.Error("JinLaiUpdata:Parse1" + ex.InnerException + "-" + ex.Message);
                resualt = -1;
            }
            try
            {
                
                #region 解析报警信息数据对象
                buff = new byte[2];
                byteIndex = 0x0030 * 2;
                for (int i = 0; i < 9; i++)
                {
                    HisJLAlarm = new HisJLAlarmGatherData();
                    Buffer.BlockCopy(GetData, byteIndex, buff, 0, 2); CrossHiLow(ref buff);
                    HisJLAlarm.JLAlarmWGathDTHis =Convert.ToDateTime(recdPtr.m_recv_date);
                    HisJLAlarm.JLAlarmPosNoHis = i + 1;
                    HisJLAlarm.bsO_Id = Org.Id;
                    HisJLAlarm.Id = ggdyobj.Det_Id;
                    HisJLAlarm.JLAlarmHBit7His = (buff[0] & 128) == 128 ? true : false;
                    HisJLAlarm.JLAlarmHBit4His = (buff[0] & 16) == 16 ? true : false;
                    HisJLAlarm.JLAlarmHBit3His = (buff[0] & 8) == 8 ? true : false;
                    HisJLAlarm.JLAlarmHBit1His = (buff[0] & 2) == 2 ? true : false;
                    HisJLAlarm.JLAlarmHBit0His = (buff[0] & 1) == 1 ? true : false;
                    if ((bool)HisJLAlarm.JLAlarmHBit7His)
                    {
                        HisJLAlarm.JLAlarmLByteHis = (decimal)buff[1];
                    }

                    //保存数据库
                    EntityManager<HisJLAlarmGatherData>.Add<HisJLAlarmGatherData>(HisJLAlarm);
                    
                    byteIndex++;
                }
                #endregion

            }
            catch (Exception ex)
            {
                log.Error("JinLaiUpdata:Parse2" + ex.InnerException + "-" + ex.Message);
                resualt = -2;
            }
            try
            {
                #region 解析最新锅炉数据对象
                buff = new byte[4];
                byteIndex = 0X0039 * 2;
                for (int i = 0; i < 4; i++)
                {
                    ldyobj = new OTGLUnitData();
                    ldyobj.GathDt = Convert.ToDateTime(recdPtr.m_recv_date);
                    ldyobj.Det_Id = devs[i+1].Id;
                    ldyobj.bsO_Id = Org.Id;
                    switch (i)
                    {
                        case 0:
                            byteIndex = 0x0039 * 2;
                            break;
                        case 1:
                            byteIndex = 0x0046 * 2;
                            break;
                        case 2:
                            byteIndex = 0x0053 * 2;
                            break;
                        case 3:
                            byteIndex = 0x0060 * 2;
                            break;
                    }
                    Buffer.BlockCopy(GetData, byteIndex, buff, 0, 4); CrossHiLow(ref buff);
                    ldyobj.JLOTLDYF33 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                    ldyobj.JLOTLDYF34 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                    ldyobj.JLOTLDYF35 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                    ldyobj.JLOTLDYF36 = (decimal)BitConverter.ToSingle(buff, 0);

                    //锅炉状态
                    byteIndex += 6 * 2;
                    Buffer.BlockCopy(GetData, byteIndex, buff, 0, 2); CrossHiLow(ref buff);
                    ldyobj.JLOTLDYF38 = (int)(buff[0] & (byte)128) == 0 ? 0 : 1;
                    ldyobj.JLOTLDYF39 = (int)(buff[0] & (byte)64) == 0 ? 0 : 1;
                    ldyobj.JLOTLDYF40 = (int)(buff[0] & (byte)32) == 0 ? 0 : 1;
                    ldyobj.JLOTLDYF41 = (int)(buff[0] & (byte)16) == 0 ? 0 : 1;
                    ldyobj.JLOTLDYF42 = (int)(buff[0] & (byte)8) == 0 ? 0 : 1;
                    ldyobj.JLOTLDYF43 = (int)(buff[0] & (byte)4) == 0 ? 0 : 1;
                    ldyobj.JLOTLDYF44 = (int)(buff[0] & (byte)2) == 0 ? 0 : 1;
                    ldyobj.JLOTLDYF45 = (int)(buff[0] & (byte)1) == 0 ? 0 : 1;

                    ldyobj.JLOTLDYF46 = (int)(buff[1] & (byte)128) == 0 ? 0 : 1;
                    ldyobj.JLOTLDYF47 = (int)(buff[1] & (byte)64) == 0 ? 0 : 1;
                    ldyobj.JLOTLDYF48 = (int)(buff[1] & (byte)32) == 0 ? 0 : 1;
                    ldyobj.JLOTLDYF49 = (int)(buff[1] & (byte)16) == 0 ? 0 : 1;
                    //保存数据库
                    //glfda.heatingdb.AddToLastGLUnitData(newJLLdy);
                    //glfda.heatingdb.SaveChanges();

                  //保存数据库
                EntityManager<OTGLUnitData>.Add<OTGLUnitData>(ldyobj);
                NewGLUnitData newobj = EntityManager<NewGLUnitData>.GetByPk<NewGLUnitData>("Det_Id", ldyobj.Det_Id);
                if (newobj == null)
                {
                    EntityOperate.Copy<NewGLUnitData>(ldyobj, newobj, "Det_Id");
                    EntityManager<NewGLUnitData>.Add<NewGLUnitData>(newobj);
                }
                else
                {
                    EntityOperate.Copy<NewGLUnitData>(ldyobj, newobj, "Det_Id");
                    EntityManager<NewGLUnitData>.Modify<NewGLUnitData>(newobj);
                }
                }
                #endregion
            }
            catch (Exception ex)
            {
                log.Error("JinLaiUpdata:Parse3" + ex.InnerException + "-" + ex.Message);
                resualt = -3;
            }
            return resualt;
        }

        public override DeviceCmd CreateReadCommand(string simno)
        {
            return base.CreateModbusRtuReadCommand(simno, 0x01, 0x03, 0x0000, 0x006C - 0x0000 + 1);

        }
    }
}
