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

namespace QyTech.ProtocalGlfGaoDianChang
{
    public class ReadData:IProtocal
    {
             public ReadData(int bsP_Id):base(bsP_Id)
        {
         
        }

        public override DeviceCmd CreateReadCommand(string simno)
        {
            return base.CreateModbusRtuReadCommand(simno, 0x20, 0x03, 0x0000, 0x0069 - 0x0000 + 1);

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

            ggdyobj.Det_Id = devs[0].Id;
            ggdyobj.bsO_Id = Org.Id;

            ggdyobj.GathDt = DateTime.Parse(recdPtr.m_recv_date);    //数据接收时间
            try
            {
                #region 解析到最新公共部分数据对象
                buff = new byte[8];
                Buffer.BlockCopy(GetData, 0x4 * 2, buff, 0, 8); //CrossHiLow(ref buff);
                ggdyobj.OTGLCN25 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0xA * 2, buff, 0, 8); //CrossHiLow(ref buff);
                ggdyobj.OTGLCN26 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x14 * 2, buff, 0, 8); //CrossHiLow(ref buff);
                ggdyobj.OTGLCN27 = (decimal)BitConverter.ToDouble(buff, 0);
                Buffer.BlockCopy(GetData, 0x1A * 2, buff, 0, 8); //CrossHiLow(ref buff);
                ggdyobj.OTGLCN28 = (decimal)BitConverter.ToDouble(buff, 0);



                buff = new byte[4];
                Buffer.BlockCopy(GetData, 0x0 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                ggdyobj.OTGLCN1 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x2 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                ggdyobj.OTGLCN2 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x8 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                ggdyobj.OTGLCN3 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0xE * 2, buff, 0, 4); //CrossHiLow(ref buff);
                ggdyobj.OTGLCN4 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x10 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                ggdyobj.OTGLCN5 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x12 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                ggdyobj.OTGLCN6 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x18 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                ggdyobj.OTGLCN7 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x1E * 2, buff, 0, 4); //CrossHiLow(ref buff);
                ggdyobj.OTGLCN8 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x20 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                ggdyobj.OTGLCN9 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x22 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                ggdyobj.OTGLCN10 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x28 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                ggdyobj.OTGLCN11 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x2A * 2, buff, 0, 4); //CrossHiLow(ref buff);
                ggdyobj.OTGLCN12 = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x2C * 2, buff, 0, 4); //CrossHiLow(ref buff);
                ggdyobj.OTGLCN13 = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x2E * 2, buff, 0, 4); //CrossHiLow(ref buff);
                ggdyobj.OTGLCN14 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x30 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                ggdyobj.OTGLCN15 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x32 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                ggdyobj.OTGLCN19 = (decimal)BitConverter.ToSingle(buff, 0);

                Buffer.BlockCopy(GetData, 0x34 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                ggdyobj.OTGLCN20 = (decimal)BitConverter.ToSingle(buff, 0);
                Buffer.BlockCopy(GetData, 0x36 * 2, buff, 0, 4); //CrossHiLow(ref buff);
                ggdyobj.OTGLCN24 = (decimal)BitConverter.ToSingle(buff, 0);

                #endregion

                EntityManager<ONTimeGLCommNet>.Add<ONTimeGLCommNet>(ggdyobj);
                NewGLCommNet newobj = EntityManager<NewGLCommNet>.GetBySql<NewGLCommNet>("Det_Id='" + ggdyobj.Det_Id.ToString() + "'");
                if (newobj == null)
                {
                    newobj=EntityOperate.Copy<NewGLCommNet>(ggdyobj);
                    EntityManager<NewGLCommNet>.Add<NewGLCommNet>(newobj);
                }
                else
                {
                    EntityOperate.Copy<NewGLCommNet>(ggdyobj, newobj, "Det_Id");
                    EntityManager<NewGLCommNet>.Modify<NewGLCommNet>(newobj);
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
                for (int i = 0; i < devs.Count-1; i++)
                {
                    ldyobj = new OTGLUnitData();
                    ldyobj.GathDt = DateTime.Parse(recdPtr.m_recv_date);
                    ldyobj.Det_Id = devs[i+1].Id;
                    ldyobj.bsO_Id = Org.Id;
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
                    ldyobj.OTLDYF1 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4);// CrossHiLow(ref buff);
                    ldyobj.OTLDYF2 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    ldyobj.OTLDYF3 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    ldyobj.OTLDYF4 = (decimal)BitConverter.ToSingle(buff, 0);
                    Buffer.BlockCopy(GetData, byteIndex += 4, buff, 0, 4); //CrossHiLow(ref buff);
                    ldyobj.OTLDYF5 = (decimal)BitConverter.ToSingle(buff, 0);


                    //保存数据库
                    EntityManager<OTGLUnitData>.Add<OTGLUnitData>(ldyobj);
                    NewGLUnitData newobj = EntityManager<NewGLUnitData>.GetByPk<NewGLUnitData>("Det_Id", ldyobj.Det_Id);
                    if (newobj == null)
                    {
                        newobj = EntityOperate.Copy<NewGLUnitData>(ldyobj);
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
                log.Error("GaoDianChang Updata:Parse3" + ex.InnerException + "-" + ex.Message);
                resualt = -3;
            }
            return resualt;
        }
    }
}
