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
using System.Reflection;

namespace QyTech.Communication
{
    public class IProtocalFac
    {
        protected static ILog log = log4net.LogManager.GetLogger("IProtocalFac");

        protected bsProtocal Protocal;
        protected List<bsProtocal> childProtocals_;


        protected Assembly ass= null;
        protected Type type = null;         
        public IProtocalFac(int bsP_Id)
        {
            log = log4net.LogManager.GetLogger(this.GetType().Name);
          
            Protocal = EntityManager<bsProtocal>.GetByPk<bsProtocal>("Id", bsP_Id);
            childProtocals_ = EntityManager<bsProtocal>.GetListNoPaging<bsProtocal>("PId=" + bsP_Id.ToString(), "FromAddr desc");
            //load dll
            ass = Assembly.LoadFrom(Protocal.Code+@".dll");
          
        }

        /// <summary>
        /// 数据的最后两个字节表示包的标识
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        protected virtual int GetFlagRegHexValue(byte[] packet)
        {
            byte[] buff = new byte[2];
            Buffer.BlockCopy(packet, packet.Length - 4, buff, 0, 2);
            IProtocal.CrossHiLow(ref buff);
            int intFlag =BitConverter.ToUInt16(buff, 0);
            return intFlag;
        }

        /// <summary>
        /// 包数据部分的实际长度，即（包长度-5）/2
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        protected int GetFlagWithPacketLength(byte[] packet)
        {
            if (packet[1] != 0x01)
                return (packet.Length - 5) / 2;//寄存器长度
            else
                return packet.Length - 5;//字节长度
        }
 
        //解码时知道具体解码项的bsP_Id
        public virtual IProtocal Create(byte[] packet)
        {
            IProtocal pobj = new IProtocal();
            int Flag;
            if (Protocal.JudgeType == 2)
            {
                Flag = GetFlagRegHexValue(packet);
                foreach (bsProtocal p in childProtocals_)
                {
                    if (Convert.ToInt32(p.ToAddr.Substring(2, 4), 16) == Flag)
                    {
                        type = ass.GetType("QyTech." + Protocal.Code + "." + p.Code);
                        pobj = (IProtocal)Activator.CreateInstance(type, p.Id);
                        return pobj;
                    }
                }
            }
            else
            {
                Flag = GetFlagWithPacketLength(packet);
                foreach (bsProtocal p in childProtocals_)
                {
                    int len = Convert.ToInt32(p.ToAddr.Substring(2, 4), 16) - Convert.ToInt32(p.FromAddr.Substring(2, 4), 16) + 1;
                    //if (p.PacketLength == Flag)
                    if (len == Flag)
                    {
                        type = ass.GetType("QyTech." + Protocal.Code + "." + p.Code);
                        pobj = (IProtocal)Activator.CreateInstance(type, p.Id);
                        return pobj;
                    }
                }
            }
            return null;
        }

      

        //编码时知道设备的通讯族，不知道具体bsP_id，构造时
        public virtual IProtocal Create(int Addr)
        {
            IProtocal pobj = new IProtocal();
            try
            {
                //if (Addr == 0x0027) --modified by szw on 2018-01-22  for 宁河闭挂炉的0027下发地址，需要核实以前为什么要特殊处理0027
                //{
                //    foreach (bsProtocal p in childProtocals_)
                //    {
                //        if (Convert.ToInt16(p.FromAddr.Substring(2, 4), 16)== Addr)
                //        {
                //            type = ass.GetType("QyTech." + Protocal.Code + "." + p.Code);

                //            pobj = (IProtocal)Activator.CreateInstance(type, p.Id);// + p.Code));
                //            break;
                //        }
                //    }
                //}
                //else
                //{

                    foreach (bsProtocal p in childProtocals_)
                    {
                        //if (Convert.ToInt16(p.ToAddr.Substring(2, 4), 16) >= Addr)//2017-12-12 修改，子协议修改为了倒序
                        if (Convert.ToInt16(p.FromAddr.Substring(2, 4), 16) <= Addr)
                        {
                            type = ass.GetType("QyTech." + Protocal.Code + "." + p.Code);

                            pobj = (IProtocal)Activator.CreateInstance(type, p.Id);// + p.Code));
                            break;
                        }
                    }
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return pobj;
        }

        /// <summary>
        /// 需要常规读写的类及方法名
        /// </summary>
        /// <returns></returns>
        public virtual string[] GetNeedNormalReadClassMethodName()
        {
            return new string[] {""};
        }

        public virtual string[] GetNeedNormalWriteClassMethodName()
        {
            return new string[] { ""};
        }

    }
}
