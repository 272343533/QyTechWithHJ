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
using QyTech.ProtocalGlfCommon;

namespace QyTech.ProtocalGlfXinyangCN
{
    public class WritePart1 : GLFCommonWritePart
    {
        //采集数据
     
        public WritePart1(int bsP_Id)
            : base(bsP_Id)
        {
            
        }
    }
}


