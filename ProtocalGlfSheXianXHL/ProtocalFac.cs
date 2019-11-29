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
namespace QyTech.ProtocalHrzSheXianXHL
{
    public class ProtocalFac:IProtocalFac
    {
        public ProtocalFac(int bsP_Id)
            : base(bsP_Id)
        {
        }

        public override IProtocal Create(byte[] packet)
        {
            IProtocal pobj = new IProtocal();
            int Flag = GetFlagWithPacketLength(packet);
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
     

        public override string[] GetNeedNormalReadClassMethodName()
        {
            return new string[] { "ReadData.CreateReadCommand", "ReadData1.CreateReadCommand" };
        }


        public override string[] GetNeedNormalWriteClassMethodName()
        {
            return new string[] { "WritePart1.CreateWeatherDownCommand" };
        }
    }
}
