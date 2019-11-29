﻿using System;
using System.Collections.Generic;

using QyTech.Core.BLL;
using SunMvcExpress.Core.BLL;

using SunMvcExpress.Dao;
using QyTech.Communication;

using QyTech.HDGprs;
using QyTech.Protocal;
using HjCommDA;

using QyTech.ProtocalGlfCommon_DetNo;

namespace QyTech.ProtocalGlfLiaoHeLy
{
    public class ReadData6 : GLFCommonReadLUnit_DetNo_LY
    {
        public ReadData6(int bsP_Id) : base(bsP_Id)
        {
            DetailDevNo = 6;
        }

    }
}
