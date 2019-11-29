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
using QyTech.ProtocalHrzCommon;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace QyTech.ProtocalHrzHnHxhy
{
    public class ProtocalFac : HRZCommonProtocalFac
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
                "ReadData1.CreateReadCommand"
                , "ReadData2.CreateReadCommand"
                , "ReadData3.CreateReadCommand"
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
