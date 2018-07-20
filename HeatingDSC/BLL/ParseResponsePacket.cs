using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QyTech.Communication;
using SunMvcExpress.Dao;
using QyTech.HDGprs;

namespace HeatingDSC.BLL
{
    public class ParseResponsePacket:IProduct
    {
        public ParseResponsePacket()
        {
            Producttype = ProductType.ResponseData;
        }
        public override int Parse(GPRS_DATA_RECORD recdPtr)
        {
            base.Parse(recdPtr);
            int resualt = 0;
            try
            {
                //log.Info("response0:"+recdPtr.m_userid);
                
                //根据电话-〉设备-〉协议-〉包类型
                List<DTUProduct> devs = devda.SelectDevicesbySimNo(recdPtr.m_userid);
               //气象仪一个设备，换热站多个设备
                string[] cpcode=new string[1];
                int PiPeiflag=0;
                bool findflag=false;
                int CPCode=0;
                foreach (DTUProduct dev in devs)
                {
                    CPCode = (int)dev.CPCode;
                    #region 匹配设备协议和地址
                    switch ((CommProtocal)dev.CPCode)
                    {
                        case CommProtocal.GprsGlf:
                            break;
                        case CommProtocal.GprsHrz://cureve,weather,
                            //cpcode = dev.CommProtocal.CPDesp.Split(new char[] { ',', ';' });
                            break;
                        case CommProtocal.GprsQxy:////
                            break;
                        case CommProtocal.GprsWdj:
                            break;
                        case CommProtocal.GprsHrzUnit1:
                            break;
                        case CommProtocal.GprsHrzUnit2:
                            break;
                        case CommProtocal.GprsHrzUnit3:
                            break;
                    }

                    for (int i = 1; i < cpcode.Length/2; i++)
                    {
                        if ((Convert.ToInt32(cpcode[2 * i - 1],16) == RegStartAddr) && (Convert.ToInt32(cpcode[2 * i]) == RegCount))
                        {
                            PiPeiflag = i;
                            findflag = true;
                            break;
                       }
                    }
                    #endregion
                    if (findflag)
                    {
                        break;
                    }
                   
                }
        
                if (findflag)
                {
                    PacketFac ppf = new PacketFac();
                    IProduct product = ppf.CreatePacket((ProductType)((CPCode * 10) + PiPeiflag));
                    product.SentData(recdPtr);
                }
        
            }
            catch (Exception ex)
            {
                log.Error("qxy:"+ex.Message);
                resualt = -1;
            }
            return resualt;
        }

    }
}
