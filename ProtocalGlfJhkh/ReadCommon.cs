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

namespace QyTech.ProtocalGlfJhkh
{
    public class ReadCommon: GLFCommonReadGGUnit
    {
        public ReadCommon(int bsP_Id)
            : base(bsP_Id)
        {
            DetailDevNo = 0;
        }

    
        public override DeviceCmd CreateWeatherDownCommand(string simno, float wd, float rz, float fs)
        {
            return base.CreateModbusRtuWriteCommand(simno,0x01,0x001A,wd, rz, fs);
        }

        public override DeviceCmd CreateModbusRtuWriteCommand(string simno, byte slaveaddr, int address, params object[] args)
        {
            return base.CreateModbusRtuWriteCommand(simno, slaveaddr, address, args);
        }
    }
}


