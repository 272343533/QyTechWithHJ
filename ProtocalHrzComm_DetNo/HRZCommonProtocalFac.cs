using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QyTech.Communication;
using log4net;
using SunMvcExpress.Dao;
using HjCommDA;
using SunMvcExpress.Core.BLL;
using QyTech.Core;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace QyTech.ProtocalHrzCommon_DetNo
{
    public class HRZCommonProtocalFac: IProtocalFac
    {
        public HRZCommonProtocalFac(int bsP_Id)
            : base(bsP_Id)
        {
            log = log4net.LogManager.GetLogger(this.GetType().Name);
        }


        public override IProtocal Create(byte[] packet)
        {
            IProtocal pobj = new IProtocal();
            int Flag;

            Flag = GetFlagWithPacketLength(packet);
            foreach (bsProtocal p in childProtocals_)
            {
                if (p.PacketLength == Flag)
                {
                    type = ass.GetType("QyTech." + Protocal.Code + "." + p.Code);
                    pobj = (IProtocal)Activator.CreateInstance(type, p.Id);
                    return pobj;
                }

            }
            return null;
        }

        public override IProtocal Create(int Addr)
        {
            IProtocal pobj = new IProtocal();
            try
            {
                foreach (bsProtocal p in childProtocals_)
                {
                    if (Convert.ToInt16(p.ToAddr.Substring(2, 4), 16) >= Addr)
                    {
                        type = ass.GetType("QyTech." + Protocal.Code + "." + p.Code);

                        pobj = (IProtocal)Activator.CreateInstance(type, Addr);// + p.Code));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return pobj;
        }
        /// <summary>
        /// 需要常规读的类及方法名
        /// </summary>
        /// <returns></returns>
        public override string[] GetNeedNormalReadClassMethodName()
        {
            return new string[] {
                "ReadData1.CreateReadCommand"
                , "ReadData2.CreateReadCommand"
                , "ReadData3.CreateReadCommand"
                , "ReadData4.CreateReadCommand"
                , "ReadData5.CreateReadCommand"
                , "ReadData6.CreateReadCommand"
                , "ReadData7.CreateReadCommand"
                , "ReadData8.CreateReadCommand"
                , "ReadData9.CreateReadCommand"};
        }

        /// <summary>
        /// 需要常规写的类及方法名
        /// </summary>
        /// <returns></returns>
        public override string[] GetNeedNormalWriteClassMethodName()
        {
            return new string[] { "WritePart1.CreateWeatherDownCommand" };
        }

    }
}
