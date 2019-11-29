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
using QyTech.ProtocalGlfCommon_DetNo;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace QyTech.ProtocalGlfLiaoHeLy
{
    public class ProtocalFac : GLFCommonProtocalFac
    {
         public ProtocalFac(int bsP_Id)
            : base(bsP_Id)
        {
        }

        /// <summary>
        /// 需要常规读的类及方法名
        /// </summary>
        /// <returns></returns>
        public override string[] GetNeedNormalReadClassMethodName()
        {
            return new string[] {
                "ReadCommon.CreateReadCommand"
                , "ReadData1.CreateReadCommand"
                , "ReadData2.CreateReadCommand"
                , "ReadData3.CreateReadCommand"
                , "ReadData4.CreateReadCommand"
                , "ReadData5.CreateReadCommand"
                , "ReadData6.CreateReadCommand"
                , "ReadData7.CreateReadCommand"
                , "ReadData8.CreateReadCommand"
                , "ReadData9.CreateReadCommand"
                , "ReadData10.CreateReadCommand"
                , "ReadData11.CreateReadCommand"
                , "ReadData12.CreateReadCommand"
                , "ReadData13.CreateReadCommand"
                , "ReadData14.CreateReadCommand"
                , "ReadData15.CreateReadCommand"
                , "ReadData16.CreateReadCommand"
                , "ReadData17.CreateReadCommand"
                , "ReadData18.CreateReadCommand"
                , "ReadData19.CreateReadCommand"
                , "ReadData20.CreateReadCommand"
                , "ReadData21.CreateReadCommand"
            };
        }

        /// <summary>
        /// 需要常规写的类及方法名
        /// </summary>
        /// <returns></returns>
        public override string[] GetNeedNormalWriteClassMethodName()
        {
            return new string[] { };// { "WritePart1.CreateWeatherDownCommand" };
        }

    }
}
