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
namespace QyTech.ProtocalBeiChen
{
    public class ProtocalFac : IProtocalFac
    {
        public ProtocalFac(int bsP_Id)
            : base(bsP_Id)
        {
        }

        //public override IProtocal Create(byte[] packet)
        //{
        //    IProtocal pobj = new IProtocal();
        //    //int Flag = GetFlagRegHexValue(packet);
        //    foreach (bsProtocal p in childProtocals_)
        //    {
        //        if (p.PacketLength ==GetFlagWithPacketLength(packet))
        //        {
        //            type = ass.GetType("QyTech." + Protocal.Code + "." + p.Code);
        //            pobj = (IProtocal)Activator.CreateInstance(type, p.Id);
        //            return pobj;
        //        }

        //    }
        //    return null;
        //}

        /// <summary>
        /// 需要常规读的类及方法名
        /// </summary>
        /// <returns></returns>
        public override string[] GetNeedNormalReadClassMethodName()
        {
            return new string[] { "ReadData.CreateReadCommand" };
        }

        /// <summary>
        /// 需要常规写的类及方法名
        /// </summary>
        /// <returns></returns>
        public override string[] GetNeedNormalWriteClassMethodName()
        {
            return null;// new string[] { "WirteData.CreateWeatherDownCommand" };
        }
    }
}
