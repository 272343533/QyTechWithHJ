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
    public class Range1 : GLFCommonRange_DetNo
    {
         public Range1(int bsP_Id):base(bsP_Id)
        {
            DetailDevNo =27;
        }

       

    }
}
