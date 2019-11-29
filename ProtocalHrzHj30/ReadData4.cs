using System;
using System.Collections.Generic;

using QyTech.Core.BLL;
using SunMvcExpress.Core.BLL;

using SunMvcExpress.Dao;
using QyTech.Communication;

using QyTech.HDGprs;
using QyTech.Protocal;
using HjCommDA;

using QyTech.ProtocalGlfCommon_DetNo;

namespace QyTech.ProtocalHrzHj30
{
    public class ReadData4 : GLFCommonReadHrz_DetNo
    {
        public ReadData4(int bsP_Id) : base(bsP_Id)
        {
            DetailDevNo = 3;
        }

    }
}
