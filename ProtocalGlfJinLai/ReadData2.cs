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
    public class ReadData2 : IProtocal
    {

        public HisJLAlarmGatherData HisJLAlarm = new HisJLAlarmGatherData();
   
        public ReadData2(int bsP_Id):base(bsP_Id)
        {
         
        }
        public override int Parse(HDGprs.GPRS_DATA_RECORD recdPtr)
        {
            base.Parse(recdPtr);


             
            OTGLUnitData ldyobj = new OTGLUnitData();

            Dev = BaseDABll.GetDtuProduct(recdPtr.m_userid);
            Org = BaseDABll.GetOrganize((Guid)Dev.AZWZ);

            List<DetailDevice> devs = BaseDABll.GetDetailDevice(Dev.Id);


            int resualt = 1;
            int byteIndex = 0;
            ONTimeGLCommNet ggdyobj = EntityManager<ONTimeGLCommNet>.GetBySql<ONTimeGLCommNet>("GathDt=(select max(GathDt) from ONTimeGLCommNet where  Det_Id='" + devs[0].Id.ToString() + "'  ) and Det_Id='" + devs[0].Id.ToString() + "'");

            if (ggdyobj != null)
            {
                //ggdyobj.Det_Id = devs[0].Id;
                //ggdyobj.bsO_Id = Org.Id;

                //ggdyobj.GathDt = DateTime.Parse(recdPtr.m_recv_date);    //数据接收时间
                try
                {
                    #region 解析到最新公共部分数据对象

                    try
                    {
                        byteIndex = 0;// 0X0070 * 2;
                        buff = new byte[4];
                        Buffer.BlockCopy(GetData, byteIndex, buff, 0, 4); CrossHiLow(ref buff);
                        ggdyobj.JLOTGLCN65 = (decimal)Math.Round(BitConverter.ToSingle(buff, 0), 2);
                        Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                        ggdyobj.JLOTGLCN66 = (decimal)Math.Round(BitConverter.ToSingle(buff, 0), 2);
                        Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                        ggdyobj.JLOTGLCN67 = (decimal)Math.Round(BitConverter.ToSingle(buff, 0), 2);
                        Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                        ggdyobj.JLOTGLCN68 = BitConverter.ToUInt32(buff, 0);
                        Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                        ggdyobj.JLOTGLCN69 = (decimal)Math.Round(BitConverter.ToSingle(buff, 0), 2);
                        Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                        ggdyobj.JLOTGLCN70 = (decimal)Math.Round(BitConverter.ToSingle(buff, 0), 2);
                        Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                        ggdyobj.JLOTGLCN71 = (decimal)Math.Round(BitConverter.ToSingle(buff, 0), 2);
                        Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                        ggdyobj.JLOTGLCN73 = (decimal)Math.Round(BitConverter.ToSingle(buff, 0), 2);
                        Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                        ggdyobj.JLOTGLCN74 = (decimal)Math.Round(BitConverter.ToSingle(buff, 0), 2);
                        Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                        ggdyobj.JLOTGLCN75 = (decimal)Math.Round(BitConverter.ToSingle(buff, 0), 2);
                        Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                        ggdyobj.JLOTGLCN76 = (decimal)Math.Round(BitConverter.ToSingle(buff, 0), 2);
                        Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                        ggdyobj.JLOTGLCN77 = (decimal)Math.Round(BitConverter.ToSingle(buff, 0), 2);
                        Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                        ggdyobj.JLOTGLCN78 = BitConverter.ToUInt32(buff, 0);
                        Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                        ggdyobj.JLOTGLCN79 = BitConverter.ToUInt32(buff, 0);
                        Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                        ggdyobj.JLOTGLCN80 = BitConverter.ToUInt32(buff, 0);
                        Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                        ggdyobj.JLOTGLCN81 = BitConverter.ToUInt32(buff, 0);
                        Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                        ggdyobj.JLOTGLCN82 = BitConverter.ToUInt32(buff, 0);
                        Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                        ggdyobj.JLOTGLCN83 = BitConverter.ToUInt32(buff, 0);
                        Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); CrossHiLow(ref buff);
                        ggdyobj.JLOTGLCN84 = BitConverter.ToUInt32(buff, 0);
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
                    //EntityManager<ONTimeGLCommNet>.Add<ONTimeGLCommNet>(ggdyobj);
                    EntityManager<ONTimeGLCommNet>.Modify<ONTimeGLCommNet>(ggdyobj);
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
            }
            return resualt;
        }

        public override DeviceCmd CreateReadCommand(string simno)
        {
            return base.CreateModbusRtuReadCommand(simno, 0x01, 0x03, 0x0070, 0x0097 - 0x0070 + 1);

        }
    }
}
