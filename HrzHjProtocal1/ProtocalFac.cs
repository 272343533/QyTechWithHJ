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
namespace QyTech.HjProtocal1
{
    public class ProtocalFac:IProtocalFac
    {
        public ProtocalFac(int bsP_Id)
            : base(bsP_Id)
        {
            log = log4net.LogManager.GetLogger(this.GetType().Name);
        }

        //解码时知道具体解码项的bsP_Id
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

        /// <summary>
        /// 需要常规读的类及方法名
        /// </summary>
        /// <returns></returns>
        public override string[] GetNeedNormalReadClassMethodName()
        {
            return new string[] { "ReadData.CreateReadCommand"  };
        }

        /// <summary>
        /// 需要常规写的类及方法名
        /// </summary>
        /// <returns></returns>
        public override string[] GetNeedNormalWriteClassMethodName()
        {
            return new string[] { "WritePart1.CreateWeatherDownCommand"};
        }

    }
}
