using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;


[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]

namespace DTU_JiXun
{
    public class DtuInfo
    {
            public uint ID;

            public string PhoneNO;

            public string DynIP;

            public DateTime ConnectTime;

            public DateTime LastActTime;

            public bool Visible;
    }

    //public class JXComm
    //{

    // public static ILog log = log4net.LogManager.GetLogger("JXComm");

    //DTUdll DTUService = DTUdll.Instance;

    //public string startServer(ushort comm_port)
    //{
    //    bool flag = DTUService.StartService((ushort)comm_port);
    //    string ret = "计讯服务" + comm_port.ToString() + "已启动";
    //    if (flag)
    //    {
    //        log.Info("启动计讯服务(Port=" + comm_port.ToString() + ")");
    //    }
    //    else
    //    {
    //        log.Warn("启动计讯服务(Port=" + comm_port.ToString() + ")"+DTUService.LastError);
    //        ret += "计讯6012启动失败:" + DTUService.LastError;
    //    }
    //    return ret;
    //}

    //public string StopServer()
    //{
    //    bool flag = DTUService.StopService();
    //    string ret = "停止计讯服务";
    //    if (flag)
    //    {
    //        log.Info("停止计讯服务");
    //    }
    //    else
    //    {
    //        log.Warn("停止计讯服务:" + DTUService.LastError);
    //        ret += ":" + DTUService.LastError;
    //    }
    //    return ret;
    //}


    //public List<DtuInfo> GetDtuLinkStatus()
    //{
    //    List<DtuInfo> dtuTB = new List<DtuInfo>();

    //    try
    //    {
    //        //维护列表
    //        Dictionary<uint, ModemInfoStruct> dtuList;
    //        if (DTUService.GetDTUList(out dtuList))
    //        {
    //            //this.Text = dtuList.Count.ToString();
    //            //删除数据集中的无效记录
    //            foreach (ModemInfoStruct mis in dtuList.Values)
    //            {
    //                DtuInfo di = new DtuInfo();
    //                di.ID =Convert.ToUInt32(Util.DTUIDToString(mis.m_modemId));
    //                di.PhoneNO = Util.Byte11ToPhoneNO(mis.m_phoneno, 0);
    //                di.DynIP = Util.ByteArrayToHexString(mis.m_dynip);
    //                di.ConnectTime =  Util.ULongToDatetime(mis.m_conn_time);
    //                di.LastActTime =  Util.ULongToDatetime(mis.m_refresh_time);
    //                di.Visible = true;
    //                //Application.DoEvents();
    //                if (!DTUService.Started) 
    //                    return dtuTB; ;
    //                if (!dtuTB.Contains(di))
    //                {
    //                    dtuTB.Add(di);
    //                }
    //            }
    //            return dtuTB;
    //        }
    //        else
    //        {
    //            return dtuTB;
    //        }
    //    }
    //    catch (Exception ee)
    //    {
    //        log.Warn("读取计讯连接列表:"+ee.Message);
    //        return dtuTB;
    //    }

    //}

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="dtuID"></param>
    ///// <param name="bts"></param>
    ///// <returns>0:发送成功 -1：发送失败</returns>
    //public int SendData(uint dtuID, byte[] bts)
    //{
    //    if (DTUService.SendHex(dtuID, bts))
    //    {
    //        //log.Info("向用户 " + dtuID.ToString("X").PadLeft(8, '0') + " 发送数据  成功!"+Util.ByteArrayToHexString(bts));
    //        log.Error("向用户 " + dtuID.ToString() + " 发送数据  成功!" + Util.ByteArrayToHexString(bts));
    //        return 0;
    //    }
    //    else
    //    {
    //        log.Error("向用户 " + dtuID.ToString() + " 发送数据  失败!" + this.DTUService.LastError);
    //        return -1;
    //    }
    //}

    //public List<ModemDataStruct> GetDtuData()
    //{
    //    log.Error("准备获取计讯数据（计讯模块）");
    //    List<ModemDataStruct> data = new List<ModemDataStruct>();
    //    try
    //    {
    //         //读取数据
    //        ModemDataStruct dat = new ModemDataStruct();

    //        log.Error("准备获取计讯数据（计讯模块）...");
    //        while (DTUService.GetNextData(out dat))
    //        {
    //            log.Error("获取计讯数据（计讯模块）:" + dat.m_modemId.ToString("x"));

    //            data.Add(dat);
    //            if (!DTUService.Started) return data;
    //        }
    //        return data;
    //    }
    //    catch (Exception ee)
    //    {
    //        log.Error("读取计讯数据", ee);
    //        return data;
    //    }

    //}
    //}
}
