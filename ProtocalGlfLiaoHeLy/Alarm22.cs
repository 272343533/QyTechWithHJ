using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using SunMvcExpress.Dao;
using QyTech.Communication;
using QyTech.Protocal.Modbus;
using QyTech.HDGprs;
using QyTech.Protocal;
using HjCommDA;
using SunMvcExpress.Core.BLL;
using log4net;

using QyTech.ProtocalGlfCommon_DetNo;


namespace QyTech.ProtocalGlfLiaoHeLy
{
    public class Alarm22 : GLFCommonAlarm_DetNo
    {

        public Alarm22(int bsP_Id)
            : base(bsP_Id)
        {
            string typename = this.GetType().Name;
            log = log4net.LogManager.GetLogger(typename);
            DetailDevNo = 24;
        }


       

    }
}



