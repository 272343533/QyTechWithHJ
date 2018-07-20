using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeatingDSC.Models
{
    class MeterInfo
    {
            //MainDM: TMainDM;
            //FDataSet:TClientDataSet;
            string  customerID;         //客户id
            string  stationID;          //站点id
            string  flowMeterID;        //流量计id
            string  gasLineID;          //管线id
            string  flowMeterSN;        //流量计编号
            string  flowMeterName;      //流量计名称
            int     flowMeterAddr;      //流量计安装地址
            string  protocol_DM;        //协议代码
            int     numInProtocol;      //协议中的编号
            string  signalType_dm;      //信号类型
            bool    zT_DM;              //是否在用
            string  manufacture_DM;     //生产厂家代码
            string  flowMeterType_DM;   //流量计类型代码
            string  fLowMeterModel_DM;  //流量计型号代码
            DateTime jyrq;              //检验日期
            DateTime yxrq;              //有效日期
            string  remark;             //备注


            
            public string CustomerID{ get; set; }         //客户id
            public string StationID{ get; set; }          //站点id
            public string FlowMeterID{ get; set; }        //流量计id
            public string GasLineID{ get; set; }          //管线id
            public string FlowMeterSN{ get; set; }        //流量计编号
            public string FlowMeterName{ get; set; }      //流量计名称
            public int FlowMeterAddr{ get; set; }      //流量计安装地址
            public string Protocol_DM{ get; set; }        //协议代码
            public int NumInProtocol{ get; set; }      //协议中的编号
            public string SignalType_dm{ get; set; }      //信号类型
            public bool ZT_DM{ get; set; }              //是否在用
            public string Manufacture_DM{ get; set; }     //生产厂家代码
            public string FlowMeterType_DM{ get; set; }   //流量计类型代码
            public string FLowMeterModel_DM{ get; set; }  //流量计型号代码
            public DateTime Jyrq{ get; set; }              //检验日期
            public DateTime Yxrq{ get; set; }              //有效日期
            public string Remark{ get; set; }             //备注

    }
}
