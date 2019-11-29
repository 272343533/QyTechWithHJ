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
namespace QyTech.HjCenterBSZ2
{
    public class ProtocalFac : IProtocalFac
    {
        public ProtocalFac(int bsP_Id)
            : base(bsP_Id)
        {
        }

        public override IProtocal Create(byte[] packet)
        {
            //IProtocal pobj = new QyTech.HjCenterBSZ.ReadData(168);
            //return pobj;
            IProtocal pobj = new IProtocal();
            int Flag = GetFlagRegHexValue(packet); //长度+5

            foreach (bsProtocal p in childProtocals_)
            {
                if (packet.Length == (p.PacketLength*2) + 5)
                {

                    type = ass.GetType("QyTech." + Protocal.Code + "." + p.Code);
                    pobj = (IProtocal)Activator.CreateInstance(type, p.Id);
                    break;
                }

            }
            return pobj;
        }

        public override IProtocal Create(int Addr)
        {
            IProtocal pobj = new QyTech.HjCenterBSZ2.ReadData(Addr);
            return pobj;
        }

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
            return new string[] { "" };
        }

    }
}
