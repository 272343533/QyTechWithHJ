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
using SunMvcExpress.Core.BLL;
using HjCommDA;
using QyTech.ProtocalGlfCommon;

namespace QyTech.ProtocalGlfJhkh
{
    /// <summary>
    /// 目前该函数已经不在使用，改为DownhrzControl class
    /// </summary>
    public class WritePartCurve : GLFCommonWritePartCurve
    {
        public WritePartCurve(int bsP_Id)
            : base(bsP_Id)
        {
        }

    }
}
