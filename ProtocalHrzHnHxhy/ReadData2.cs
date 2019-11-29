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

namespace QyTech.ProtocalHrzHnHxhy
{
    public class ReadData2 : HRZCommonReadData
    {
        public ReadData2(int bsP_Id):base(bsP_Id)
        {
            DetailDevNo = 1;
        }
        
    }
}
