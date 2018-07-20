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

namespace QyTech.ProtocalGlfJinLai
{
    public class WritePart : IProtocal
    {
        //采集数据
        //public static bsOrganize ORG=new bsOrganize();
        HrzCommGath hcg;

        public WritePart(int bsP_Id)
            : base(bsP_Id)
        {
        }

        public override int Parse(GPRS_DATA_RECORD recdPtr)
        {
            if (base.Parse(recdPtr) == 1)
                return -1;
           
            return 0;
        }

        public override DeviceCmd CreateReadCommand(string simno)
        {
            if (ReadPacketCommand.CommNo != null && !ReadPacketCommand.CommNo.Equals(""))
                return ReadPacketCommand;
            else
                return base.CreateModbusRtuReadCommand(simno, 0x01, 0x03, 0xF0, 0xF5 + 1 - 0xF0);
            //010301000052C5CB
        }

        public override DeviceCmd CreateWeatherDownCommand(string simno, float wd, float rz, float fs)
        {
            return base.CreateModbusRtuWriteCommand(simno, 0x01, 0xF0, wd, rz, fs);
        }

    }
}


