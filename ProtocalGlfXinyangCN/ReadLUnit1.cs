using System;
using System.Collections.Generic;

using QyTech.Core.BLL;
using SunMvcExpress.Core.BLL;

using SunMvcExpress.Dao;
using QyTech.Communication;

using QyTech.HDGprs;
using QyTech.Protocal;
using HjCommDA;

using QyTech.ProtocalGlfCommon;

namespace QyTech.ProtocalGlfXinyangCN
{
    public class ReadLUnit1 : GLFCommonReadLUnit
    {
        public ReadLUnit1(int bsP_Id):base(bsP_Id)
        {
            DetailDevNo = 1;
        }
        
    }
}
