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
namespace QyTech.WdzProtocal
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
                 Buffer.BlockCopy(packet,0x51*2+3, buff, 0, 2);
                 IProtocal.CrossHiLow(ref buff);
                 int intFlag = BitConverter.ToUInt16(buff, 0);
                 return intFlag;
             }
             else
             {
                 return ushort.MaxValue ;
             }
 
         }
         public override IProtocal Create(byte[] packet)
         {
             IProtocal pobj = new IProtocal();

             
            if  (packet.Length ==60*2+5 )
            {
                type = ass.GetType("QyTech.WdzProtocal.WritePartCurve");
                pobj = (IProtocal)Activator.CreateInstance(type, 165);
            }
            else if (packet.Length==97*2+5)
            {
                int Flag = GetFlagRegHexValue(packet); //长度+5
                type = ass.GetType("QyTech.WdzProtocal.ReadData" + Flag.ToString());
                pobj = (IProtocal)Activator.CreateInstance(type, Flag==1?49:Flag==2?50:51);
              
            }
            else if (packet.Length == 117*2 + 5)//common
            {

                type = ass.GetType("QyTech.WdzProtocal.Common");
                pobj = (IProtocal)Activator.CreateInstance(type, 166);
            }
            else// /alarm没有标志暂不解析
            {
            }
            return pobj;
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
            return new string[] { "ReadData1.CreateReadCommand", "ReadData2.CreateReadCommand", "ReadData3.CreateReadCommand" };
        }

        /// <summary>
        /// 需要常规写的类及方法名
        /// </summary>
        /// <returns></returns>
        public override string[] GetNeedNormalWriteClassMethodName()
        {
            return new string[] { "Common.CreateWeatherDownCommand" };
        }

    }
}
