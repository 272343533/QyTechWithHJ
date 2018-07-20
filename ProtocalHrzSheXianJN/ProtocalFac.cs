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
namespace QyTech.ProtocalHrzSheXianJN
{
    public class ProtocalFac : IProtocalFac
    {
        public ProtocalFac(int bsP_Id)
            : base(bsP_Id)
        {
        }
        //0x151
        protected override int GetFlagRegHexValue(byte[] packet)
        {
            byte[] buff = new byte[2];
            if (packet.Length > 0x51)
            {
                Buffer.BlockCopy(packet, 0x51 * 2 + 3, buff, 0, 2);
                IProtocal.CrossHiLow(ref buff);
                int intFlag = BitConverter.ToUInt16(buff, 0);
                return intFlag;
            }
            else
            {
                return ushort.MaxValue;
            }

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


            //IProtocal pobj = new IProtocal();
            //if (packet.Length == 50 * 2 + 5)
            //{
            //    type = ass.GetType("QyTech.ProtocalHrzSheXianJN.ReadData");
            //    pobj = (IProtocal)Activator.CreateInstance(type, 184);
            //}
        
            //return pobj;
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
            return new string[] { "ReadData.CreateReadCommand" };
        }

        /// <summary>
        /// 需要常规写的类及方法名
        /// </summary>
        /// <returns></returns>
        public override string[] GetNeedNormalWriteClassMethodName()
        {
            return new string[] { };
        }

    }
}
