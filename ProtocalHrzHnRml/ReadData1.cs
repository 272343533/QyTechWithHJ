using System;
using System.Collections.Generic;

using QyTech.Core.BLL;
using SunMvcExpress.Core.BLL;

using SunMvcExpress.Dao;
using QyTech.Communication;

using QyTech.HDGprs;
using QyTech.Protocal;
using HjCommDA;

using QyTech.ProtocalHrzCommon;

namespace QyTech.ProtocalHrzHnRml
{
    public class ReadData1 : HRZCommonReadData
    {
        public ReadData1(int bsP_Id):base(bsP_Id)
        {
            DetailDevNo = 0;
        }
        
    }
}
