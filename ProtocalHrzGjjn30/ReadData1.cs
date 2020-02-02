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

namespace QyTech.ProtocalHrzGjjn30
{
    public class ReadData1 : GLFCommonReadHrz_DetNo
    {
        public ReadData1(int bsP_Id):base(bsP_Id)
        {
            DetailDevNo =0;
        }
        
    }
}
