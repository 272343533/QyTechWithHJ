using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Reflection;
using SunMvcExpress.Dao;
using QyTech.Communication;
using QyTech.Protocal.Modbus;
using QyTech.HDGprs;
using QyTech.Protocal;
using HjCommDA;
using SunMvcExpress.Core.BLL;
using QyTech.Core.BLL;
using QyTech.ProtocalGlfCommon_DetNo;

namespace QyTech.ProtocalHrzHj30
{
    public class WritePart5 : GLFCommonWritePart_DetNo
    {
        //采集数据
     
        public WritePart5(int bsP_Id)
            : base(bsP_Id)
        {
            string typename = this.GetType().Name;
            log = log4net.LogManager.GetLogger(typename);
            DetailDevNo = 8;
        }
    }
}


