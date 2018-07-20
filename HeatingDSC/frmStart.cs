using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HeatingDSC.Models;
using SunMvcExpress.Dao;
using SunMvcExpress.Core.BLL;
using System.Reflection;
using QyTech.Core;
using System.Diagnostics;
using HeatingDSC.UI;
using log4net;
using QyTech.Protocal.Modbus;
using ChenDong.XmlConfigFile;
using System.Threading;
using SunMvcExpress.Common.Xml;
using HeatingDSC.BLL;
//////using HeatingDSC.CreateProtocalUI;

using QyTech.HDGprs;
using QyTech.Protocal;
using QyTech.Communication;

using DTU_JiXun;

using System.Runtime.InteropServices;

using QyTech.DtuDll;

//using HeatingDSC.SR_DownCommand;

//[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace HeatingDSC
{

    public partial class frmStart : Form//,ISendMessageServiceCallback
    {
        public static ILog log = log4net.LogManager.GetLogger("frmstart");

        private frmProgParamSet frmconf = new frmProgParamSet();

        private bool m_Terminated = false;

        private SortedDictionary<string, string> OpSimNo = new SortedDictionary<string, string>();

          //定义窗口消息，用来响应DTU的消息
        public const int WM_DTU = 0x400 + 100;
  
  
        //CommNo,bsO_Id 
        private SortedList<string, Guid> slbsO_IdWithCommNo = new SortedList<string, Guid>();
        //CommNo，bsP_Id
        private SortedList<string, int> slProtocalCodeBySimno = new SortedList<string, int>();
        //所有dtu设备 气象仪
        private SortedList<string, vwlyGprsDevice> slDtuDevice = new SortedList<string, vwlyGprsDevice>();
        //协议 simno集合
        private SortedList<int, List<string>> slSimnoWithbsP = new SortedList<int, List<string>>();
        private SortedList<int, bsProtocal> slProtocal = new SortedList<int, bsProtocal>();

        //所有detail设备 换热站机组
        private Dictionary<Guid, List<vwlyGprsDetailDevice>> slDtuDetailDevice = new Dictionary<Guid, List<vwlyGprsDetailDevice>>();

        //各设备需要发送的命令，0表示正常的采集数据，1 表示气象数据的下发  2以后表示动态要发送的命令，动态命令优先
        //前两个命令在下发后更改对应的需要下发的时间
        private SortedList<string, List<DeviceCmd>> slDeviceCmdsNoraml = new SortedList<string, List<DeviceCmd>>();
        private SortedList<string, List<DeviceCmd>> slDeviceCmdsUnNoraml = new SortedList<string, List<DeviceCmd>>();


        private List<DeviceCmd> QxyCmds = new List<DeviceCmd>();

 
        //需要解码的包序列,//协议+通讯号组合，包
        private SortedList<int, List<GPRS_DATA_RECORD>> NeedParsePackets = new SortedList<int, List<GPRS_DATA_RECORD>>();
      
        //需要动态增加的命令的获取。
        List<vwlyHrzAutoDownCurveSetting> dhcS = new List<vwlyHrzAutoDownCurveSetting>();
        List<vwlyHrzDownSet> hdss = new List<vwlyHrzDownSet>();//包含commno

        //注册的设备
        SortedList<string, object> slConnectedBySimno = new SortedList<string, object>();

        //存储最新的气象数据
        private Dictionary<Guid, vwlyWeatherPowerCalculate> slOrgWeatherDatas = new Dictionary<Guid, vwlyWeatherPowerCalculate>();


        private bool OnlySendConnectedFlag = false;
        private int ReStartPerMinutes = 0;
        private bool IsTest = false;

        private static readonly object LockObjectSendCmd = new object();
        private static readonly Dictionary<string, object> LockObjectModifyCmds = new Dictionary<string, object>();
        private static readonly Dictionary<string, object> LockObjectCommNoSend = new Dictionary<string, object>();


        //private JXComm commjixun = new JXComm();
        private static Dictionary<string, DtuInfo> dtu_JxInfoFromCommNo = new Dictionary<string, DtuInfo>();
        private static Dictionary<uint, string> dtu_JxInfoFromIdToCommNo = new Dictionary<uint, string>();
        private static Dictionary<string, string> dtu_trantypes = new Dictionary<string, string>();

        DTUdll DTUService = DTUdll.Instance;
        InhandDtuDll inhandServ = InhandDtuDll.Instance;
        //SendMessageServiceClient client;

        //public void ReceiveMessage(MessageEntity messageEntity)
        //{
        //    //MessageBox.Show(messageEntity.Content);
        //    if (true == txtWcf.InvokeRequired)
        //    {
        //        txtWcf.Invoke(new UpdateListBoxDelegate(UpdateListBox), messageEntity.Content);
        //    }
        //    else
        //    {
        //        UpdateListBox(messageEntity.Content);
        //    }

        //}

        private delegate void UpdateListBoxDelegate(string Message);
        private void UpdateListBox(string message)
        {

            this.txtWcf.Text+="\r\n"+message;
            //listbMessage= listbMessage.Items.Count - 1;
        }

        private void JudgeSameWithCommand(string simno, string packet)
        {
            if (packet.Length >= 8)//有效数据包
            {
                try
                {
                    log.Info("--------" + simno + "---待判断数量" + slDeviceCmdsUnNoraml[simno].Count.ToString());
                    if (slDeviceCmdsUnNoraml[simno].Count > 0)
                    {
                    
                        for (int i = 0; i < slDeviceCmdsUnNoraml[simno].Count; i++)
                        {
                           
                            DeviceCmd cmd = slDeviceCmdsUnNoraml[simno][i];
                            log.Info("--------" + simno +cmd.bsO_id.ToString()+ "---待判断命令:" + cmd.Command);
                   
                            //更新数据库
                            string sql = "Convert(varchar(19),SetDt,120)='" + cmd.SetDownTime.ToString("yyyy-MM-dd HH:mm:ss") + "' and bsO_Id='" + cmd.bsO_id.ToString() + "' and SetFlag='0x" + cmd.Command.Substring(4, 4) + "'";
                            log.Info("--------" + simno + cmd.bsO_id.ToString() + "---待判断命令条件:" + sql);
                   
                            HrzDownSet hds = EntityManager<HrzDownSet>.GetBySql<HrzDownSet>(sql);
                            if (hds != null)
                            {
                                if (hds.ValidResponse.Substring(0, 6) == packet.Substring(0, 6))//都只处理前六位
                                {
                                    if (packet.Substring(2, 2) == "01")
                                        hds.SetValues = Convert.ToUInt16(packet.Substring(6, Convert.ToInt16(packet.Substring(4, 2), 16) * 2),16).ToString();
                                  
                                    hds.DownSuccDt = DateTime.Now;
                                  
                                    EntityManager<HrzDownSet>.Modify<HrzDownSet>(hds);
                                }
                                
                            }
                            //更新命令
                            if (cmd.Response.Substring(0, 6) == packet.Substring(0, 6))
                            {
                                lock (LockObjectModifyCmds[simno])
                                {
                                    slDeviceCmdsUnNoraml[simno].Remove(cmd);
                                }
                                break;
                            }
                        }
                    }

                }
                catch(Exception ex)
                {
                    LogHelper.Error(ex);
                }
            }
        }
        private static readonly object JxDtuOperateStatus = new object();//0:未操作，1：读数据 2：读状态 3：其它
    
        /// <summary>
        /// 5s取一次手动下发数据，30s获取一次设备信息
        /// </summary>
        private void InitDataFromDb()
        {
            QyTech.Protocal.DeviceCmd cmd;

            List<DTUProduct> qxyDtus = EntityManager<DTUProduct>.GetListNoPaging<DTUProduct>("CommNo in ('13311112222','13922220002')", "CommNo");
            HeatingDSC.BLL.ParseQxyUpData qud = new HeatingDSC.BLL.ParseQxyUpData();
            QxyCmds = qud.CreateWdzProtQxy(qxyDtus);


            List<DTUProduct> wqqxyDtus = EntityManager<DTUProduct>.GetListNoPaging<DTUProduct>(" bsp_Id =68 and CommNo not in ('13311112222','13922220002')", "CommNo");// in ('13866889999','13866668888','13600000000')", "CommNo");
            HeatingDSC.BLL.ParseWqQxyUpData qud1 = new HeatingDSC.BLL.ParseWqQxyUpData();
            QxyCmds.AddRange(qud1.CreateProtQxy(wqqxyDtus));


            List<bsProtocal> bsps = EntityManager<bsProtocal>.GetListNoPaging<bsProtocal>("", "Id");
            List<vwlyProtocalCodeBySimcardNo> vws;
            foreach (bsProtocal bsp in bsps)
            {
                slProtocal.Add(bsp.Id, bsp);

                vws = EntityManager<vwlyProtocalCodeBySimcardNo>.GetListNoPaging<vwlyProtocalCodeBySimcardNo>("CommNo is not null and CommNo!='' and bsP_Id=" + bsp.Id.ToString(), "CommNo");
                foreach (vwlyProtocalCodeBySimcardNo v in vws)
                {
                    int proAndcommMap = bllCommNo2PacketListNo.GetPacketListNo(bsp.Id, v.CommNo);
                    if (!NeedParsePackets.ContainsKey(proAndcommMap))
                        NeedParsePackets.Add(proAndcommMap, new List<GPRS_DATA_RECORD>());
                }
            }
            bsps = null;
            
            vws = EntityManager<vwlyProtocalCodeBySimcardNo>.GetListNoPaging<vwlyProtocalCodeBySimcardNo>("CommNo is not null and CommNo!=''", "CommNo");
            foreach (vwlyProtocalCodeBySimcardNo v in vws)
            {
                if (!slProtocalCodeBySimno.ContainsKey(v.CommNo))
                {
                    slProtocalCodeBySimno.Add(v.CommNo, (int)v.bsP_Id);
                }
                else
                    this.txtSimNoUn.Text += v.CommNo + "初始化时发现协议重复" + "\r\n";
            }
            vws = null;


            List<vwlyGprsDevice> objs = EntityManager<vwlyGprsDevice>.GetListNoPaging<vwlyGprsDevice>("", "bsP_Id,CommNo");
            foreach (vwlyGprsDevice obj in objs)
            {
                 try
                {
                   if (!LockObjectModifyCmds.ContainsKey(obj.CommNo))
                        LockObjectModifyCmds.Add(obj.CommNo, new object());
                   if (!LockObjectCommNoSend.ContainsKey(obj.CommNo))
                       LockObjectCommNoSend.Add(obj.CommNo, new object());
                   

                   if (!slSimnoWithbsP.ContainsKey((int)obj.bsP_Id))
                    {
                        slSimnoWithbsP.Add((int)obj.bsP_Id, new List<string>());
                    }
                   slSimnoWithbsP[(int)obj.bsP_Id].Add(obj.CommNo);

                   if (!slDtuDevice.ContainsKey(obj.CommNo))
                    {
                        slDtuDevice.Add(obj.CommNo, obj);
                        slbsO_IdWithCommNo.Add(obj.CommNo, (Guid)obj.bsO_Id);
                        
                        slDeviceCmdsNoraml.Add(obj.CommNo, new List<DeviceCmd>());
                        slDeviceCmdsNoraml[obj.CommNo].Add(new DeviceCmd());

                        slDeviceCmdsUnNoraml.Add(obj.CommNo, new List<DeviceCmd>());
                   }
                    
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }


            List<vwlyGprsDetailDevice> detailobjs = EntityManager<vwlyGprsDetailDevice>.GetListNoPaging<vwlyGprsDetailDevice>("", "DTU_Id");
            foreach (vwlyGprsDetailDevice det in detailobjs)
            {
                if (!slDtuDetailDevice.ContainsKey(det.DTU_Id))
                {
                    slDtuDetailDevice.Add(det.DTU_Id, new List<vwlyGprsDetailDevice>());
                }
                slDtuDetailDevice[det.DTU_Id].Add(det);
            }
          

            int pre_bsP_Id = -1;
            vwlyGprsDevice dev = objs[0];

            Assembly ass = Assembly.LoadFrom(dev.bsP_Code + ".dll");
            Type type = ass.GetType("QyTech." + dev.bsP_Code + ".ProtocalFac");
            Object PacketFac = Activator.CreateInstance(type, dev.bsP_Id);

            //采集占据随后的1-n个位置
            MethodInfo PfMethod = type.GetMethod("GetNeedNormalReadClassMethodName");
            string[] Methods = (string[])PfMethod.Invoke(PacketFac, null);
            string[] classmethod;

            for (int d = 0; d < objs.Count; d++)
            {
                if (!dtu_trantypes.ContainsKey(objs[d].CommNo))
                    dtu_trantypes.Add(objs[d].CommNo, objs[d].TranType);
                else if (objs[d].CommNo != "")
                    log.Info("重复通讯好trantype");
                else
                    continue;
                try
                {
                        dev = objs[d];
                //为了测试
                        if (dev.CommNo == "15300010001")
                    {
                        int a = 100;
                        a++;
                    }
                    else
                    {
                        int b = 1;
                        b++;
                    }

                    if (pre_bsP_Id != -1 && pre_bsP_Id != dev.bsP_Id)
                    {
                        ass = Assembly.LoadFrom(dev.bsP_Code + ".dll");
                        type = ass.GetType("QyTech." + dev.bsP_Code + ".ProtocalFac");
                        PacketFac = Activator.CreateInstance(type, dev.bsP_Id);

                        //采集占据随后的1-n个位置
                        PfMethod = type.GetMethod("GetNeedNormalReadClassMethodName");
                        Methods = (string[])PfMethod.Invoke(PacketFac, null);
                    }
                    if (slDtuDetailDevice.ContainsKey(dev.Id))
                    {
                        if (dev.Id == Guid.Parse("BD0E3D46-7BFA-4A76-B874-B2390AFF9A0C"))
                        {
                            if (dev.CommNo == "13300030001")//dev.CommNo == "13399990000" || 糕点厂
                            {
                                string meth = Methods[0];
                                classmethod = meth.Split(new char[] { '.' });
                                type = ass.GetType("QyTech." + dev.bsP_Code + "." + classmethod[0]);
                                PacketFac = Activator.CreateInstance(type, dev.bsP_Id);

                                PfMethod = type.GetMethod(classmethod[1], new Type[] { Type.GetType("System.String") });
                                cmd = (DeviceCmd)PfMethod.Invoke(PacketFac, new object[] { dev.CommNo });
                                cmd.ExpiredTime = DateTime.MaxValue;
                                slDeviceCmdsNoraml[dev.CommNo].Add(cmd);
                            }
                        }
                        else
                        {
                           
                                for (int i = 0; i < Methods.Length && i < slDtuDetailDevice[dev.Id].Count; i++)
                                {
                                    string meth = Methods[i];
                                    classmethod = meth.Split(new char[] { '.' });
                                    type = ass.GetType("QyTech." + dev.bsP_Code + "." + classmethod[0]);
                                    PacketFac = Activator.CreateInstance(type, dev.bsP_Id);

                                    PfMethod = type.GetMethod(classmethod[1], new Type[] { Type.GetType("System.String") });
                                    cmd = (DeviceCmd)PfMethod.Invoke(PacketFac, new object[] { dev.CommNo });
                                    cmd.ExpiredTime = DateTime.MaxValue;
                                    slDeviceCmdsNoraml[dev.CommNo].Add(cmd);
                                }
                            //    if (dev.bsP_Id != 278) //子设备和命令一样，就是每个命令单独的
                            //    { }
                            //else
                            //{
                            //    for (int i = 0; i < Methods.Length; i++)
                            //    {
                            //        string meth = Methods[i];
                            //        classmethod = meth.Split(new char[] { '.' });
                            //        type = ass.GetType("QyTech." + dev.bsP_Code + "." + classmethod[0]);
                            //        PacketFac = Activator.CreateInstance(type, dev.bsP_Id);

                            //        PfMethod = type.GetMethod(classmethod[1], new Type[] { Type.GetType("System.String") });
                            //        cmd = (DeviceCmd)PfMethod.Invoke(PacketFac, new object[] { dev.CommNo });
                            //        cmd.ExpiredTime = DateTime.MaxValue;
                            //        slDeviceCmdsNoraml[dev.CommNo].Add(cmd);
                            //    }
                            //}
                        }
                    }
                    else
                    {
                        log.Info(dev.CommNo+"没有配置细节设备数据");
                    }
                    pre_bsP_Id = (int)dev.bsP_Id;
                }
                catch (Exception ex)
                {
                    log.Error(dev.CommNo + "---" + ex.Message);
                    continue;
                }
            }
        }


        /// <summary>
        /// 云存储采集，下载命令的增加
        /// </summary>
        /// <param name="simno"></param>
        /// <param name="cmds"></param>
        private void protaol_delProtocalDeviceCommandhandler(string simno, List<DeviceCmd> cmds)
        {
            if (cmds.Count == 0)
                return;
            lock (LockObjectModifyCmds[cmds[0].CommNo])
            {
                
                for(int i=0;i<cmds.Count;i++)
                {
                    DeviceCmd cmd = cmds[i];
                    log.Info("protaol_delProtocalDeviceCommandhandler:" + simno + "-" + cmd.Command + ",命令共:" + cmds.Count.ToString());
                    //判断是否已经存在某些命令，有则不需要再增加
                    if (!HaveDeviceCmd(cmd, slDeviceCmdsUnNoraml[simno]))
                    {
                        log.Error(cmd.Command);
                        cmd.ExpiredTime = DateTime.Now.AddMinutes(1);//一分钟周期内不过期
                        slDeviceCmdsUnNoraml[simno].Add(cmd);

                        //存入数据库
                        HrzDownSet hds = new HrzDownSet();
                        hds.SetDt = cmd.SetDownTime;
                        hds.bsO_Id = cmd.bsO_id;// slbsO_IdWithCommNo[simno];
                        hds.bsO_Name = EntityManager<bsOrganize>.GetByPk<bsOrganize>("Id", cmd.bsO_id).Name;
                        hds.bsP_Id = cmd.bsP_Id;
                        hds.OperType = "0x" + cmd.Command.Substring(2, 2);
                        hds.SetFlag = "0x" + cmd.Command.Substring(4, 4);
                        hds.ForAllPacket = true;
                        hds.CommNo = simno;
                        hds.DownCmd = cmd.Command;
                        hds.Downer = "采集程序";
                        hds.FlagDesp = cmd.CommandDesp;
                        hds.ValidResponse = cmd.Response;
                        hds.DownDt = hds.SetDt;

                        EntityManager<HrzDownSet>.Add<HrzDownSet>(hds);
                    }
                }
            }
        }       
    
        private void protaol_delProtocalDataReceivedhandler(string simno, object data)
        {
        }
  
        private bool HaveDeviceCmd(DeviceCmd cmd, List<DeviceCmd> cmds)
        {
            foreach (DeviceCmd c in cmds)
            {
                if (c.Command == cmd.Command)
                    return true;
            }
            return false;
        }




     

        private void CreateWeatDownCommand()
        {

            List<vwlyWeatherPowerCalculate> wpcs = EntityManager<vwlyWeatherPowerCalculate>.GetListNoPaging<vwlyWeatherPowerCalculate>("", "");
            slOrgWeatherDatas.Clear();
            foreach (vwlyWeatherPowerCalculate wpc in wpcs)
            {
                slOrgWeatherDatas.Add(wpc.bsO_Id, wpc);
            }
            foreach (int bsP_Id in slSimnoWithbsP.Keys)
            {
                try
                {
                    List<DeviceCmd> cmds = CreateWeatDownCommand(bsP_Id, slSimnoWithbsP[bsP_Id]);

                    foreach (DeviceCmd cmd in cmds)
                    {

                        if (slDeviceCmdsNoraml[cmd.CommNo].Count == 0)
                            slDeviceCmdsNoraml[cmd.CommNo].Add(cmd);
                        else
                        {
                            slDeviceCmdsNoraml[cmd.CommNo][0] = cmd;
                        }

                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
            }

        }
        private List<DeviceCmd> CreateWeatDownCommand(int bsP_Id, List<string> simnos)
        {
            List<DeviceCmd> cmds = new List<DeviceCmd>();
            DeviceCmd cmd;
            try
            {
                bsProtocal bsp = slProtocal[bsP_Id];

                Assembly ass = Assembly.LoadFrom(bsp.Code + ".dll");
                Type type = ass.GetType("QyTech." + bsp.Code + ".ProtocalFac");
                Object PacketFlag = Activator.CreateInstance(type, bsP_Id);

                MethodInfo PfMethod = type.GetMethod("GetNeedNormalWriteClassMethodName");
                string[] Methods = (string[])PfMethod.Invoke(PacketFlag, null);
                string[] classmethod;

                //只有一个气象位置命令。
                string meth = Methods[0];
                classmethod = meth.Split(new char[] { '.' });
                type = ass.GetType("QyTech." + bsp.Code + "." + classmethod[0]);
                PacketFlag = Activator.CreateInstance(type, bsP_Id);

                PfMethod = type.GetMethod(classmethod[1]);//通过方法名称获得方法

                foreach (string simno in simnos)
                {
                    vwlyGprsDevice dev = slDtuDevice[simno];

                    //获得气象数据
                    float wd = 0, rz = 0, fs = 0;

                    if (slOrgWeatherDatas.ContainsKey((Guid)dev.bsO_Id))
                    {
                        vwlyWeatherPowerCalculate wpc = slOrgWeatherDatas[(Guid)dev.bsO_Id];// EntityManager<vwlyWeatherPowerCalculate>.GetByPk<vwlyWeatherPowerCalculate>("bsO_Id",(Guid)dev.bsO_Id);

                        wd = (float)wpc.WDHis;
                        rz = (float)wpc.RZHis;
                        fs = (float)wpc.FSHis;
                        cmd = (DeviceCmd)PfMethod.Invoke(PacketFlag, new object[] { dev.CommNo, wd, rz, fs });
                        cmd.NeedSendTime = DateTime.Now;
                        cmd.ExpiredTime = cmd.NeedSendTime.AddMinutes(1);
                        cmds.Add(cmd);
                    }
                    else
                    {
                        log.Error("没有配置气象数据:" + simno);
                    }
                  
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return cmds;
        }
        private void GetHandDownCommandLoop()
        {
            while (!m_Terminated)
            {
                GetHandDownCmd();
                Thread.Sleep(5000);
            }
        }

        private void GetHandDownCmd()
        {
            //将过期的命令删除掉
            try
            {
                foreach (string key in slDeviceCmdsUnNoraml.Keys)
                {
                    for (int i = slDeviceCmdsUnNoraml[key].Count - 1; i >= 0; i--)
                    {
                        if (slDeviceCmdsUnNoraml[key][i].SetDownTime < DateTime.Now.AddMinutes(-5))
                        {
                            lock (LockObjectModifyCmds[key])
                            {
                                slDeviceCmdsUnNoraml[key].RemoveAt(i);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("删除过期命令：",ex);
            }
            int ErrFlag = 100;
            try
            {
                DeviceCmd cmd = new DeviceCmd();
                List<vwlyHrzDownSet> hds = EntityManager<vwlyHrzDownSet>.GetListNoPaging<vwlyHrzDownSet>("", "SetDt");
                foreach (vwlyHrzDownSet hd in hds)
                {
                    //if (hd.bsO_Name == "西湖道小学公建") ;
                    //{
                    //    break;
                    //}
                    try
                    {
                        IProtocalFac pf = new IProtocalFac(slProtocalCodeBySimno[hd.CommNo]);
                        int addr = Convert.ToInt32(hd.SetFlag.Substring(2, 4), 16);
                        IProtocal pro = pf.Create(addr);

                        ErrFlag = 90;
                        if (hd.DownCmd != null && !hd.DownCmd.Equals(""))//如果已有命令，表明下发过，直接使用原来的数据。但是不包括05的线圈命令
                        {
                            if (hd.OperType == "0x05")
                            {
                                #region 写线圈
                                cmd = new DeviceCmd();
                                cmd.bsO_id = hd.bsO_Id;
                                cmd.bsP_Id = (int)hd.bsP_Id;
                                cmd.Command = hd.DownCmd;
                                cmd.CommandDesp = hd.FlagDesp;
                                cmd.CommNo = hd.CommNo;
                                cmd.DefFlag = "";
                                cmd.ExpiredTime = DateTime.Now.AddMinutes(5);
                                cmd.NeedSendTime = hd.SetDt;// hd.DownDt.Value;//原来为什么直接写呢？2017-11-09

                                cmd.SendCmd = CommFunc.HexCmd2Bytes(cmd.Command);// System.Text.Encoding.Default.GetBytes(cmd.Command);
                                cmd.SendCmd = ModbusCommand.GetBytesAfterCalculateCrc16(cmd.SendCmd);//增加校验位
                                cmd.Command = CommFunc.Bytes2HexCmd(cmd.SendCmd);
                                cmd.bsO_id = hd.bsO_Id;
                                cmd.Response = hd.ValidResponse;
                                cmd.SetDownTime = hd.SetDt;

                                #endregion
                            }
                            else
                            {
                                cmd = new DeviceCmd();
                                cmd.bsO_id = hd.bsO_Id;
                                cmd.bsP_Id = (int)hd.bsP_Id;
                                cmd.Command = hd.DownCmd;
                                cmd.CommandDesp = hd.FlagDesp;
                                cmd.CommNo = hd.CommNo;
                                cmd.DefFlag = "";
                                cmd.ExpiredTime = hd.DownDt.Value.AddMinutes(5);
                                cmd.NeedSendTime = hd.DownDt.Value;
                                cmd.Response = hd.ValidResponse;
                                cmd.SendCmd = CommFunc.HexCmd2Bytes(cmd.Command);// System.Text.Encoding.Default.GetBytes(cmd.Command);
                                //cmd.SendCmd = ModbusCommand.GetBytesAfterCalculateCrc16(cmd.SendCmd);//增加校验位，已经有了呀？
                                cmd.SetDownTime = hd.SetDt;
                            }
                            
                        }
                        else if (hd.ForAllPacket == null || !(bool)hd.ForAllPacket)//单地址下发
                        {
                            #region 部分包数据设置下发----写
                            if (hd.OperType == "0x05")  //如果没有给出命令，则拼命令
                            {
                                #region 写线圈
                                cmd = new DeviceCmd();
                                cmd.bsO_id = hd.bsO_Id;
                                cmd.bsP_Id = (int)hd.bsP_Id;
                                cmd.Command = "0105" + hd.SetFlag.Substring(2) + hd.SetValues;
                                cmd.CommandDesp = hd.FlagDesp;
                                cmd.CommNo = hd.CommNo;
                                cmd.DefFlag = "";
                                cmd.ExpiredTime = DateTime.Now.AddMinutes(5);
                                cmd.NeedSendTime = hd.SetDt;// hd.DownDt.Value;//原来为什么直接写呢？2017-11-09

                                cmd.SendCmd = CommFunc.HexCmd2Bytes(cmd.Command);// System.Text.Encoding.Default.GetBytes(cmd.Command);
                                cmd.SendCmd = ModbusCommand.GetBytesAfterCalculateCrc16(cmd.SendCmd);
                                //
                                //cmd.Command = CommFunc.Bytes2HexCmd(cmd.SendCmd);
                                cmd.bsO_id = hd.bsO_Id;
                                cmd.Response = cmd.Command;
                                cmd.SetDownTime = hd.SetDt;

                                #endregion
                            }
                            else
                            {
                                ErrFlag = 190;

                                string[] str = hd.SetValues.Split(new char[] { ',' });
                                object[] vals = new object[str.Length];

                                ErrFlag = 290;

                                //需要确定str的实际类型
                                int ItemIndex = -1;
                                for (int i = 0; i < pro.bsProtItems.Count; i++)
                                {
                                    if (hd.SetFlag == pro.bsProtItems[i].StartRegAddress)
                                    {
                                        ErrFlag = 390;

                                        ItemIndex = i;
                                        break;
                                    }
                                }
                                ErrFlag = 490;

                                if (ItemIndex != -1)
                                {
                                    ErrFlag = 590;

                                    for (int s = 0; s < str.Length; s++)
                                    {
                                        vals[s] = IBytesConverter.ToRightType(str[s], pro.bsProtItems[ItemIndex + s].UnifType);
                                    }
                                    cmd = pro.CreateModbusRtuWriteCommand(hd.CommNo, 0x01, addr, vals);
                                    cmd.bsO_id = hd.bsO_Id;
                                    cmd.SetDownTime = hd.SetDt;
                                    ErrFlag = 690;

                                }
                            }
                            #endregion
                        }
                        else  //多地址同时下发
                        {
                            #region 多地址同时下发
                            ErrFlag = 90;
                            if (hd.OperType == "0x03") //全部读取该地址所在包数据----读
                            {
                                ErrFlag = 85;

                                cmd = pro.CreateReadCommand(hd.CommNo);
                            }
                            else //全部写该地址所在包数据----写
                            {
                                ErrFlag = 80;

                                object dbobj = HjCommDA.BaseDABll.GetObjByDownSet(hd);//根据dbobj获取实际的对象
                                ErrFlag = 75;
                                cmd = pro.CreateModbusRtuWriteCommandByConf(hd.CommNo, dbobj);
                            }
                            ErrFlag = 70;

                            cmd.bsO_id = slbsO_IdWithCommNo[cmd.CommNo];
                            cmd.SetDownTime = hd.SetDt;
                            ErrFlag = 60;
                            #endregion

                        }
                        cmd.ExpiredTime = DateTime.MaxValue;
                         
                        ErrFlag = 50;

                        if (cmd.CommNo != "")
                        {
                            ErrFlag = 40;
                            bool HaveSameCmd = false;
                            foreach (DeviceCmd cmd1 in slDeviceCmdsUnNoraml[hd.CommNo])
                            {
                                //ErrFlag = 35;
                                if (cmd1.Command == cmd.Command)
                                {
                                    //ErrFlag = 32;
                                    //LogHelper.Info("GetHandDownCmd", "命令已经存在：" + cmd.CommNo + ":" + cmd.Command);
                                    HaveSameCmd = true;
                                    break;
                                }
                            }
                            if (!HaveSameCmd)
                            {
                                ErrFlag = 30;
                                lock (LockObjectModifyCmds[cmd.CommNo])
                                {
                                    ErrFlag = 20;
                                    slDeviceCmdsUnNoraml[hd.CommNo].Add(cmd);
                                }
                                ErrFlag = 10;
                                HrzDownSet hddb = EntityManager<HrzDownSet>.GetBySql<HrzDownSet>("Convert(varchar(20),Setdt,120)='" + hd.SetDt.ToString("yyyy-MM-dd HH:mm:ss") + "' and bsO_Id='" + hd.bsO_Id + "' and SetFlag='" + hd.SetFlag + "'");
                                hddb.DownCmd = cmd.Command;
                                hddb.DownDt = DateTime.Now;
                                hddb.ValidResponse = cmd.Response;

                                EntityManager<HrzDownSet>.Modify<HrzDownSet>(hddb);
                            }

                       }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(ErrFlag.ToString()+"--"+hd.CommNo, ex);
                    }

               }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ErrFlag.ToString(),ex);
            }
        }

        private bool HaveUnNoramCommand(string simno, DeviceCmd cmd)
        {
            foreach (DeviceCmd cmd1 in slDeviceCmdsUnNoraml[simno])
            {
                if (cmd1.Command==cmd.Command)
                    return true;
            }
            return false;
        }


        /// <summary>
        /// ReStartAtMinute <60 每小时都重启，>60  
        /// </summary>
        private void ReStartApplication()
        {
            int AddMinutes = 0;
            while (!m_Terminated)
            {
                if ((AddMinutes++ > 20) && DateTime.Now.Minute > 10 && DateTime.Now.Minute == ReStartPerMinutes % 60)
                {
                    m_Terminated = true;
                    log.Info("Restart Applcation .....");
                    Application.Restart();
                    Thread.Sleep(5000);
                    log.Info("Restart Applcation");
                }
                AddMinutes = AddMinutes > 1000 ? 0 : AddMinutes;
                for (int i = 0; i < 30; i++)
                {
                    Thread.Sleep(2 * 1000);
                    if (m_Terminated)
                        break;
                }

            }
        }



        private bool HaveCommand(IList<DeviceCmd> cmds, DeviceCmd command)
        {
            foreach (DeviceCmd cmd in cmds)
            {
                if (cmd.Command == command.Command && cmd.CommNo == command.CommNo)
                    return true;
            }
            return false;
        }


        public frmStart()
        {
            InitializeComponent();
        }


        private void RecordDt(string desp)
        {
            log.Info(desp + ":" + DateTime.Now.ToString("mm:ss"));
        }

        public const int USER = 0x0400;//用户自定义消息的开始数值
        [DllImport("user32.dll")]
        public static extern void PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);


        protected override void DefWndProc(ref System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                case USER + 1:
                    //string message = string.Format("收到自己消息的参数:{0},{1}", m.WParam, m.LParam);
                    StartProcess();
                    break;
                default:
                    base.DefWndProc(ref m);//一定要调用基类函数，以便系统处理其它消息。
                    break;
            }
        }
        private void StartProcess()
        {
            MessageBox.Show("具备条件，可以正常运行了！");
        }
        private void frmStart_Load(object sender, EventArgs e)
        {
          
            //PostMessage(this.Handle, USER + 1, 168, 51898);
    

            //Handle是窗口句柄，它是一个IntPtr类型
            // IntPtr hdc = GetWindowDC(this.Handle);
            // List<HrzGathAlarm> objs = EntityManager<HrzGathAlarm>.GetAllByStorProcedure<HrzGathAlarm>("splyGetClouldStorageData", "'D6B5BCCC-ACD3-4FD9-B3B6-471A1D410A7E',2");




            //client = new SendMessageService
            //Client(new System.ServiceModel.InstanceContext(this));
            //QyTech.ProtocalHrzSanFengYongJi.WritePartCurve a = new QyTech.ProtocalHrzSanFengYongJi.WritePartCurve(102);
            //HrzRunCurve hrc = EntityManager<HrzRunCurve>.GetByPk<HrzRunCurve>("Id", "4b6c9e3b-33db-4ede-8e03-7b01b21f1cc9");
            //a.CreateModbusRtuWriteCommandByConf("13300040005",hrc);

            RecordDt("load start");
            Control.CheckForIllegalCrossThreadCalls = false;
            try
            {
                this.notifyIcon1.Text = "程序正在加载配置数据";
                ProgParams.appPath = System.IO.Directory.GetCurrentDirectory();//获取当前目
                notifyIcon1.Icon = new Icon(ProgParams.appPath + @"\dsc.ico");
             
                ProgParams.readParamsFromXMLFile();//读取程序参数
                ReStartPerMinutes = int.Parse(XmlConfig.GetValue("ReStartM"));
                OnlySendConnectedFlag = bool.Parse(XmlConfig.GetValue("OnlySendConnectedFlag"));
            }
            catch (Exception ex)
            {
                log.Error("ico:" + ex.Message);
            }

            try
            {

                this.notifyIcon1.Text = "程序正在从数据库中初始化数据......";//123//
                InitDataFromDb();
            }
            catch (Exception ex)
            {
                log.Error("get device data error：" + ex.InnerException + "-" + ex.Message);
                return;
            }
          
            this.notifyIcon1.Text = "正在处理界面数据";
            this.ucTvDevice1.RefreshTreeAllDevice();
            this.notifyIcon1.Text = "正在启动Gprs服务";
            //trWrite_Tick(null, null);
            启动服务ToolStripMenuItem.Checked = true;

            //client.GetMessage();

            RecordDt("load end");
        }

        private void SendNormalCommand()
        {
            while (!m_Terminated)
            {
                try
                {
                    //先发气象仪
                    foreach(DeviceCmd cmd in QxyCmds)
                    {
                         SendCommand(cmd);
                    }

                    CreateWeatDownCommand();//获取气象数据，然后发送
                    for (int jz = 0; jz < 9 + 1; jz++)
                    {
                        foreach (string key in slDeviceCmdsNoraml.Keys)
                        {
                               
                            if (Monitor.TryEnter(LockObjectCommNoSend[key], 500))
                            {
                                try
                                {
                                    if (slConnectedBySimno.ContainsKey(key.ToString()) || !OnlySendConnectedFlag)
                                    {
                                        if (slDeviceCmdsNoraml[key].Count > jz)
                                        {
                                            DeviceCmd cmd = slDeviceCmdsNoraml[key.ToString()][jz];
                                            lock (LockObjectSendCmd) { SendCommand(cmd); }
                                        }
                                    }
                                    if (m_Terminated) break;
                                }
                                catch (Exception ex)
                                {
                                    LogHelper.Error(key, ex);
                                }
                                Monitor.Exit(LockObjectCommNoSend[key]);
                            }
                            if (key == "14000000001")
                                continue;
                        }
                        for (int t = 0; t < ProgParams.IntervalBetweenCmds; t++)
                        {
                            if (m_Terminated)
                                break;
                            Thread.Sleep(1000);
                        }
                    }
                    for (int t = 0; t < ProgParams.IntervalBetweenGath; t++)
                    {
                        if (m_Terminated)
                            break;
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error("while:", ex);
                }
            }
        }

        private void SendUnNormalCommand()
        {
            int IntervalBetweenCmds = 5;//5秒钟发送一条命令

            while (!m_Terminated)
            {
                try
                {
                    foreach (string key in slDeviceCmdsUnNoraml.Keys)
                    {
                        if (slDeviceCmdsUnNoraml[key].Count > 0)
                        {
                            log.Info("SendUnNormalCommand 试图启动线程发送" + key.ToString());
                            if (Monitor.TryEnter(LockObjectModifyCmds[key.ToString()], 100))
                            {
                                log.Info("SendUnNormalCommand 占用命令修改状态锁" + key.ToString());
                                if (slConnectedBySimno.ContainsKey(key.ToString()) || !OnlySendConnectedFlag)
                                {
                                    log.Info("SendUnNormalCommand 已启动线程发送" + key.ToString());
                                    Thread thrsendCmd = new Thread(new ParameterizedThreadStart(SendUnNormalCommand));
                                    thrsendCmd.Start(key);
                                }

                                Monitor.Exit(LockObjectModifyCmds[key.ToString()]);
                                log.Info("SendUnNormalCommand 已启动线程发送,释放命令修改状态锁" + key.ToString());
                            }
                            else
                                log.Info("SendUnNormalCommand试图发送临时命令,但还处于命令修改状态,需要完成修改，才能发送" + key.ToString());
                        }
                    }
                    for (int i = 0; i < IntervalBetweenCmds; i++)
                    {
                        if (m_Terminated)
                            break;
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error("while:", ex);
                }
            }
        }
        /// <summary>
        /// 每个通讯号一个线程，完成处理
        /// </summary>
        /// <param name="key">通讯号</param>
        private void SendUnNormalCommand(object key)
        {
            try
            {
                log.Info("SendUnNormalCommand 准备发送" + key.ToString() + "命令数量:" + slDeviceCmdsUnNoraml[key.ToString()].Count.ToString());
                string strcmds="";
                for (int i = 0; i < slDeviceCmdsUnNoraml[key.ToString()].Count; i++)
                {
                    strcmds += "," + slDeviceCmdsUnNoraml[key.ToString()][i].Command;
                }
                log.Info(key+":"+strcmds.Substring(1));
                
                lock(LockObjectCommNoSend[key.ToString()])
                {
                    log.Info("SendUnNormalCommand 准备发送,已占据通讯发送锁,试图占据命令修改锁" + key.ToString());
               
                    lock (LockObjectModifyCmds[key.ToString()])
                    {
                        log.Info("SendUnNormalCommand 准备发送,已占据通讯发送锁,已占据命令修改锁,准备发送" + key.ToString());
           
                        for (int i = 0; i < slDeviceCmdsUnNoraml[key.ToString()].Count; i++)
                        {
                            DeviceCmd cmd = slDeviceCmdsUnNoraml[key.ToString()][i];
                           
                            if (cmd.NeedSendTime <= DateTime.Now)
                            {
                                lock (LockObjectSendCmd)
                                {
                                    SendCommand(cmd);
                                    slDeviceCmdsUnNoraml[key.ToString()][i].ExpiredTime = DateTime.Now;
                                }
                                for (int t = 0; t < ProgParams.IntervalBetweenCmds; t++)
                                {
                                    if (m_Terminated)
                                        break;
                                    Thread.Sleep(1000);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        public static byte[] GetBytes(string hexString)
        {
            int byteLength = hexString.Length / 2;
            byte[] bytes = new byte[byteLength];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return bytes;
        }
        private string bytes2Hex(byte[] b)
        {
            string str = "";
            for (int i = 0; i < b.Length; i++)
            {
                str += b[i].ToString("X2");
            }
            return str;
        }
        //private void DoPreWork()
        //{
        //    try
        //    {

        //        //显示设备与组织结构数据
        //        try
        //        {
        //            if (this.InvokeRequired)
        //            {
        //                this.BeginInvoke(new MethodInvoker(this.ucTvDevice1.RefreshTreeAllDevice));
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error("uctvdevice:"+ex.Message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("DoPreWork:" + ex.Message);
        //        return;
        //    }
        //}


        //启动GPRS服务
        private bool StartGPRSServer()
        {
           
            StringBuilder mess = new StringBuilder(100);
            Gprs.StartGprsServer(this.Handle, ProgParams.GprsComPort, WM_DTU, ref mess);
            string strmess= mess.ToString();
            if (strmess.Length > 50)
            {
                this.notifyIcon1.Text += "(" + ProgParams.GprsComPort.ToString() + " 默认服务已启动)";
                return true;
            }
            else
            {
                this.notifyIcon1.Text += "(" + strmess + ")";
                return false;
            }
        }

        //启动GPRS服务
        private bool StartJXGPRSServer()
        {

            StringBuilder mess = new StringBuilder(100);
            Gprs.StartGprsServer(this.Handle, ProgParams.GprsComPort, WM_DTU, ref mess);
            string strmess = mess.ToString();
            if (strmess.Length > 50)
            {
                this.notifyIcon1.Text = "(" + strmess.Substring(strmess.Length - 50, 50) + ")";
                return true;
            }
            else
            {
                this.notifyIcon1.Text = "(" + strmess + ")";
                return false;
            }
        }
       

        private void RefreshRegisters(SortedDictionary<string,string> simnos)
        {
            this.txtSimNo.Text = "";
            int i = 1;
            string strno="";
            foreach (string simno in simnos.Keys)
            {
                strno="    "+i++.ToString();
                strno=strno.Substring(strno.Length-4)+"  ";
                this.txtSimNo.Text +=strno+ simno +":     "+simnos[simno]+ "\r\n";
            }
            Application.DoEvents();
        }
        private void RefreshRegistersForTree(bool RegOrCancel, string simcardno)
        {
            //foreach(TreeNode tn in this.ucTvDevice1
        }


        private void WriteReceiveLog(GPRS_DATA_RECORD recdPtr)
        {
            if (!rbNoReDisp.Checked && txtCommNo.Text == recdPtr.m_userid)
            {

                if (rbOnlyData.Checked && recdPtr.m_data_type == 0x09)
                {
                    this.txtReceiveData.Text = recdPtr.m_userid + " " + recdPtr.m_recv_date + " " + IProtocal.bytes2Hex(recdPtr.m_data_buf, recdPtr.m_data_len) + "\r\n" + this.txtReceiveData.Text;
                    if (this.txtReceiveData.Lines.Length >= 50)
                    {
                        rbNoReDisp.Checked = true;
                    }
                }
                else if (rbAll.Checked)
                {
                    this.txtReceiveData.Text = recdPtr.m_userid + " " + recdPtr.m_recv_date + " " + (recdPtr.m_data_type == 0x01 ? "注册" : (recdPtr.m_data_type == 0x02 ? "注销" : (recdPtr.m_data_type == 0x04 ? "无效" : recdPtr.m_data_type.ToString()))) + " " + IProtocal.bytes2Hex(recdPtr.m_data_buf, recdPtr.m_data_len) + "\r\n" + this.txtReceiveData.Text;
                    if (this.txtReceiveData.Lines.Length >= 100)
                    {
                        rbNoReDisp.Checked = true;
                    }
                }
            }
        }

        private void WriteDisp(GPRS_DATA_RECORD recdPtr)
        {
            if (!rbNoReDisp.Checked && txtCommNo.Text == recdPtr.m_userid)
            {

                if (rbOnlyData.Checked && recdPtr.m_data_type == 0x09)
                {
                    this.txtReceiveData.Text = recdPtr.m_userid + " " + recdPtr.m_recv_date + " " + IProtocal.bytes2Hex(recdPtr.m_data_buf, recdPtr.m_data_len) + "\r\n" + this.txtReceiveData.Text;
                    if (this.txtReceiveData.Lines.Length >= 50)
                    {
                        rbNoReDisp.Checked = true;
                    }
                }
                else if (rbAll.Checked)
                {
                    this.txtReceiveData.Text = recdPtr.m_userid + " " + recdPtr.m_recv_date + " " + (recdPtr.m_data_type == 0x01 ? "注册" : (recdPtr.m_data_type == 0x02 ? "注销" : (recdPtr.m_data_type == 0x04 ? "无效" : recdPtr.m_data_type.ToString()))) + " " + IProtocal.bytes2Hex(recdPtr.m_data_buf, recdPtr.m_data_len) + "\r\n" + this.txtReceiveData.Text;
                    if (this.txtReceiveData.Lines.Length >= 100)
                    {
                        rbNoReDisp.Checked = true;
                    }
                }
            }
        }

        //消息处理函数protected override void WndProc(ref Message m)
        //protected override void WndProc(ref Message m)
        protected override void WndProc(ref Message m)
        {
            // TODO:  添加 Form1.WndProc 实现
            //响应DTU消息，已经定义WM_DTU为DTU消息 1005130810 1005090810
            if (m.Msg == WM_DTU)
            {
                #region 宏电数据包
                try
                {
                    //log.Info("dtu packet arrive");
                    GPRS_DATA_RECORD recdPtr = new GPRS_DATA_RECORD();
                    StringBuilder mess = new StringBuilder(100);
                    //读取DTU数据
                    //底层服务收到DTU发送的数据后，向启动服务的窗口发送消息（此处即为Form1）,本
                    //函数即为该消息的响应，功能为读出并处理数据。
                    //参数1：recdPtr--存放DTU信息的数据结构
                    //参数2：mess---存放度数据过程中的信息，不需要时可设为Null
                    //参数3：bAnswer--是否对收到的数据包进行应答，调试时可设为true;发布时要设为false!
                    Gprs.do_read_proc(ref recdPtr, mess, false);
                    WriteReceiveLog(recdPtr);
                    //try
                    //{
                    //    log.Info("WndProc收到包:" + recdPtr.m_userid + " length:" + recdPtr.m_data_len.ToString() + "  type:" + recdPtr.m_data_type.ToString() + " data:" + IProtocal.bytes2Hex(recdPtr.m_data_buf, recdPtr.m_data_len));
                    //}
                    //catch { }
                    WriteDisp(recdPtr);
                    switch (recdPtr.m_data_type)
                    {
                        case 0x01:
                            #region 注册包，将新注册的站点加入到列表中
                            GPRS_USER_INFO user_info = new GPRS_USER_INFO();
                            if (Gprs.get_user_info(recdPtr.m_userid, ref user_info) == 0)//读取成功
                            {
                                //检查是否注册过
                                if (!slConnectedBySimno.ContainsKey(user_info.m_userid))
                                {
                                    try
                                    {
                                        slConnectedBySimno.Add(user_info.m_userid, slProtocalCodeBySimno[user_info.m_userid].ToString());

                                        if (!this.ucTvDevice1.ChangeColorBySimcardNo(recdPtr.m_userid, Color.Green))
                                        {
                                            this.txtSimNoUn.Text += recdPtr.m_userid + " registered,刷新树不成功\r\n";
                                        }
                                        else
                                        {
                                            if (!OpSimNo.ContainsKey(user_info.m_userid))
                                                OpSimNo.Add(user_info.m_userid, user_info.m_logon_date + "---");// +user_info.m_update_time.ToString("MM-dd HH:mm:ss") + "   ");
                                            else
                                                OpSimNo[user_info.m_userid] = user_info.m_logon_date + "---";// + user_info.ToString("MM-dd HH:mm:ss") + "   ";

                                        }
                                    }
                                    catch
                                    {
                                        slConnectedBySimno.Add(user_info.m_userid + "没有配置或协议重复", "");
                                        this.txtSimNoUn.Text += recdPtr.m_userid + " registered，没有配置或协议重复\r\n";
                                    }
                                    //设置数据库中在线状态
                                    try
                                    {

                                        DTUProduct dtu = EntityManager<DTUProduct>.GetByPk<DTUProduct>("Id", slDtuDevice[recdPtr.m_userid].Id);
                                        dtu.OnLine = true;
                                        dtu.OnLineDt = DateTime.Now;
                                        EntityManager<DTUProduct>.Modify<DTUProduct>(dtu);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(recdPtr.m_userid+"注册包：" + ex.Message);
                                    }
                               }

                            }
                            #endregion
                            break;
                        case 0x02:
                            #region 注销包
                            log.Info("cancel dtu packet:" + recdPtr.m_userid);
                            if (slConnectedBySimno.ContainsKey(recdPtr.m_userid))
                            {
                                try
                                {
                                    slConnectedBySimno.Remove(recdPtr.m_userid);
                                    if (!this.ucTvDevice1.ChangeColorBySimcardNo(recdPtr.m_userid, Color.Gray))
                                    {
                                        this.txtSimNo.Text += recdPtr.m_userid + " Canceled \r\n";
                                    }
                                    DTUProduct dtu = EntityManager<DTUProduct>.GetByPk<DTUProduct>("Id", slDtuDevice[recdPtr.m_userid].Id);
                                    dtu.OnLine = false;
                                    dtu.OnLineDt = DateTime.Now;
                                    EntityManager<DTUProduct>.Modify<DTUProduct>(dtu);
                                }
                                catch (Exception ex)
                                {
                                    log.Error(recdPtr.m_userid+"注销包：" + ex.Message);
                                }
                            }
                            #endregion
                            break;
                        case 0x04:	//无效包
                            break;
                        case 0x09:	//DTU向DSC发送的用户数据包,进入解码程序

                            if ((recdPtr.m_userid == "13311112222") || (recdPtr.m_userid == "13922220002") )
                            {
                                HeatingDSC.BLL.ParseQxyUpData pqu = new HeatingDSC.BLL.ParseQxyUpData();
                                pqu.Parse(recdPtr);
                            }
                            else if ((recdPtr.m_userid == "13866889999") || (recdPtr.m_userid == "13866668888") )
                            {
                                HeatingDSC.BLL.ParseWqQxyUpData pqu = new HeatingDSC.BLL.ParseWqQxyUpData();
                                pqu.Parse(recdPtr);
                            }
                            else if (recdPtr.m_userid == "13600000000" || (recdPtr.m_userid == "18600010000") || (recdPtr.m_userid == "13100000000"))
                            {
                                HeatingDSC.BLL.ParseSheXianQxyUpData pqu = new HeatingDSC.BLL.ParseSheXianQxyUpData();
                                pqu.Parse(recdPtr);
                            }
                            else
                            {
                                int bsP_Id = slProtocalCodeBySimno[recdPtr.m_userid];
                                try
                                {
                                    NeedParsePackets[bllCommNo2PacketListNo.GetPacketListNo(bsP_Id,recdPtr.m_userid)].Add(recdPtr);
                                }
                                catch
                                {
                                    log.Error("NeedParsePackets 数据链不能增加数据：" + recdPtr.m_userid + "该包会被舍弃:");
                                }

                                //if (NeedParsePackets[bsP_Id].Count > 200)
                                //{
                                
                                //    //把数据转移到另外一个结构中，然后单独在开辟线程解码
                                //    List<GPRS_DATA_RECORD> BackupParsePackets = new List<GPRS_DATA_RECORD>();
                                //    BackupParsePackets.AddRange(NeedParsePackets[bsP_Id]);
                                //    NeedParsePackets[bsP_Id].Clear();

                                //    log.Error("数据太多，开辟新线程解码：" + recdPtr.m_userid);
                                //    Thread thr = new Thread(new ParameterizedThreadStart(ProcessDataWithThreadSingle));
                                //    thr.Priority = ThreadPriority.Highest;
                                //    thr.Start(BackupParsePackets);

                                 
                                //    //log.Error("数据太多，舍弃1个：" + recdPtr.m_userid + "再去掉第0个:" + NeedParsePackets[bsP_Id][0].m_userid);
                                //    //NeedParsePackets[bsP_Id].RemoveAt(0);
                                //}
                            }

                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("WndProc:" + ex.Message);
                }

                #endregion
            }
            else if (m.Msg == (0x400 + (int)DtuWmCode.Inhand))
            {
                #region 映瀚通数据包
                try
                {
                    GPRS_DATA_RECORD recdPtr = new GPRS_DATA_RECORD();
                    StringBuilder mess = new StringBuilder(100);
                    bool ihFlag = this.inhandServ.ReadData(ref recdPtr, mess);

                    if (ihFlag)
                    {
                        WriteDisp(recdPtr);

                        //1 收到心跳包， 2收到退出包，3 收到登录包，9 收到终端发上来的数据，
                        switch (recdPtr.m_data_type)
                        {
                            case 3:
                                #region 注册包
                                GPRS_USER_INFO user_info = new GPRS_USER_INFO();
                                if (inhandServ.GetUserInfo(recdPtr.m_userid, ref user_info))//读取成功
                                {
                                    //检查是否注册过
                                    if (!slConnectedBySimno.ContainsKey(user_info.m_userid))
                                    {
                                        try
                                        {
                                            slConnectedBySimno.Add(user_info.m_userid, slProtocalCodeBySimno[user_info.m_userid].ToString());

                                            if (!this.ucTvDevice1.ChangeColorBySimcardNo(recdPtr.m_userid, Color.Green))
                                            {
                                                this.txtSimNoUn.Text += recdPtr.m_userid + " registered,刷新树不成功\r\n";
                                            }
                                            else
                                            {
                                                if (!OpSimNo.ContainsKey(user_info.m_userid))
                                                    OpSimNo.Add(user_info.m_userid, user_info.m_logon_date + "---" +user_info.m_update_time + "   ");
                                                else
                                                    OpSimNo[user_info.m_userid] = user_info.m_logon_date ;// + user_info.ToString("MM-dd HH:mm:ss") + "   ";

                                            }
                                        }
                                        catch
                                        {
                                            slConnectedBySimno.Add(user_info.m_userid + "没有配置或协议重复", "");
                                            this.txtSimNoUn.Text += recdPtr.m_userid + " registered，没有配置或协议重复\r\n";
                                        }
                                        //设置数据库中在线状态
                                        try
                                        {

                                            DTUProduct dtu = EntityManager<DTUProduct>.GetByPk<DTUProduct>("Id", slDtuDevice[recdPtr.m_userid].Id);
                                            dtu.OnLine = true;
                                            dtu.OnLineDt = DateTime.Now;
                                            EntityManager<DTUProduct>.Modify<DTUProduct>(dtu);
                                        }
                                        catch (Exception ex)
                                        {
                                            log.Error(recdPtr.m_userid + "注册包：" + ex.Message);
                                        }
                                    }

                                }
                                #endregion
                                break;
                            case 2:
                                #region 注销包
                                log.Info("cancel dtu packet:" + recdPtr.m_userid);
                                if (slConnectedBySimno.ContainsKey(recdPtr.m_userid))
                                {
                                    try
                                    {
                                        slConnectedBySimno.Remove(recdPtr.m_userid);
                                        if (!this.ucTvDevice1.ChangeColorBySimcardNo(recdPtr.m_userid, Color.Gray))
                                        {
                                            this.txtSimNo.Text += recdPtr.m_userid + " Canceled \r\n";
                                        }
                                        DTUProduct dtu = EntityManager<DTUProduct>.GetByPk<DTUProduct>("Id", slDtuDevice[recdPtr.m_userid].Id);
                                        dtu.OnLine = false;
                                        dtu.OnLineDt = DateTime.Now;
                                        EntityManager<DTUProduct>.Modify<DTUProduct>(dtu);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(recdPtr.m_userid + "注销包：" + ex.Message);
                                    }
                                }
                                #endregion
                                break;
                            case 9:
                                int bsP_Id = slProtocalCodeBySimno[recdPtr.m_userid];
                                try
                                {
                                    NeedParsePackets[bllCommNo2PacketListNo.GetPacketListNo(bsP_Id, recdPtr.m_userid)].Add(recdPtr);
                                }
                                catch
                                {
                                    log.Error("NeedParsePackets 数据链不能增加数据：" + recdPtr.m_userid + "该包会被舍弃:");
                                }

                                break;
                        }

                    }
                }
                catch (Exception ex)
                {
                    log.Error("映瀚通解码出现问题：" + ex.Message);
                             
                }
                #endregion
            }

            else
            {
                //缺省消息处理
                base.WndProc(ref m);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bsP_Id">原来表示协议id，现在表示协议+通讯号的组合</param>
        private void ProcessDataWithThreadPool(object bsP_Id)
        {
            while (!m_Terminated)
            {
                try
                {
                    while (NeedParsePackets[(int)bsP_Id].Count > 0)
                    {
                        try
                        {
                            ProcessData(NeedParsePackets[(int)bsP_Id][0]);
                        }
                        catch (Exception ex)
                        {
                            log.Error("ProcessDataWithThreadPool In While: " + bsP_Id.ToString() + NeedParsePackets[(int)bsP_Id].Count.ToString() + ex.Message);
                        }
                        finally
                        {
                            NeedParsePackets[(int)bsP_Id].RemoveAt(0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("ProcessDataWithThreadPool: " + bsP_Id.ToString() + NeedParsePackets[(int)bsP_Id].Count.ToString() + ex.Message);
                }
                Thread.Sleep(1000);
            }
        }


        private void ProcessDataWithThreadSingle(object listpackets)
        {
            log.Error("数据太多，开辟新线程解码");
                                  
            List<GPRS_DATA_RECORD> packets = (List<GPRS_DATA_RECORD>)listpackets;
            if (packets.Count>0)
                log.Error("数据太多，开辟新线程解码： 解码首包" + packets[0].m_userid);
                                  
            try
            {
                while (packets.Count > 0)
                {
                    try
                    {
                        ProcessData(packets[0]);
                    }
                    catch (Exception ex)
                    {
                        log.Error("ProcessDataWithThreadSingle In While: " + packets[0].m_userid + ":" + ex.Message);
                    }
                    finally
                    {
                        packets.RemoveAt(0);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("ProcessDataWithThreadSingle: " + packets[0].m_userid + ":" + ex.Message);
            }

        }

        private void 服务配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (服务配置ToolStripMenuItem.Checked)
                frmconf.Show();
            else
                frmconf.Hide();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 显示数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = !this.Visible;
            if (this.Visible)
            {
                this.WindowState = FormWindowState.Normal;
            }
            显示数据ToolStripMenuItem.Checked = !显示数据ToolStripMenuItem.Checked;
        
        }





        private void frmStart_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (启动服务ToolStripMenuItem.Checked)
            {
                启动服务ToolStripMenuItem.Checked = false;
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            this.notifyIcon1.Text = "dsc采集程序,正在关闭Gprs服务...";
            //////////m_Terminated = true;
            //////////trRead.Enabled = false;
            //////////trWrite.Enabled = false;
            //////////trGatherCurve.Enabled = false;
            Thread.Sleep(2000);
            StringBuilder mess = new StringBuilder(50);
            Gprs.stop_net_service(mess);
            this.notifyIcon1.Text = "dsc采集程序" + mess.ToString();
        }


      

        public int ProcessData(GPRS_DATA_RECORD recdPtr)
        {
            string packet = "";
            try
            {
                int resualt = 0;

                if (recdPtr.m_data_len == 0)
                    return 0;
                byte[] buff = new byte[recdPtr.m_data_len];
                Buffer.BlockCopy(recdPtr.m_data_buf, 0, buff, 0, recdPtr.m_data_len);

                IProtocal product = new IProtocal();
                packet = product.bytes2HexForThread(buff);

                if (recdPtr.m_data_len == 8 || packet.Substring(2, 2) == "05" || packet.Substring(2, 2) == "01")
                {
                    if (!"011001100006,0110001A0006".Contains(packet.Substring(0, 12)))//鸿觉(两个一样),万德气象//气象数据不解析
                    {
                        log.Info("准备解析包:date:" + recdPtr.m_recv_date + ";src:" + recdPtr.m_userid + ";长度:" + recdPtr.m_data_len.ToString() + "- data:" + packet);
                        JudgeSameWithCommand(recdPtr.m_userid, packet);
                    }
                    return 0;
                }

                log.Info("准备解析包:date:" + recdPtr.m_recv_date + ";src:" + recdPtr.m_userid + ";长度:" + recdPtr.m_data_len.ToString() + "- data:" + packet);
                
                DTUProduct dtup = EntityManager<DTUProduct>.GetBySql<DTUProduct>("commno='" + recdPtr.m_userid + "'");
                bsProtocal bsP = EntityManager<bsProtocal>.GetByPk<bsProtocal>("Id", (int)dtup.bsP_Id);

                Assembly ass = Assembly.LoadFrom(bsP.Code + @".dll");
                Type type = ass.GetType("QyTech." + bsP.Code + ".ProtocalFac");
                Object PacketFlag = Activator.CreateInstance(type, dtup.bsP_Id);
               
                Type[] tp = new Type[1];
                tp[0] = typeof(byte[]);

                MethodInfo PfMethod = type.GetMethod("Create", tp);//通过方法名称获得方法
                QyTech.Communication.IProtocal protocal = (QyTech.Communication.IProtocal)PfMethod.Invoke(PacketFlag, new object[] { buff });
                if (protocal != null)
                {
                    log.Info("protocal:" + protocal.ToString());
                    protocal.delProtocalDeviceCommanddhandler += new QyTech.Communication.delProtocalDeviceCommand(protaol_delProtocalDeviceCommandhandler);
                    protocal.delProtocalDataReceivedhandler += new QyTech.Communication.delProtocalDataReceived(protaol_delProtocalDataReceivedhandler);

                    protocal.Parse(recdPtr);

                    log.Info("will JudgeSameWithCommand simo:" + recdPtr.m_userid + "---" + packet.Substring(0, 6));
                    JudgeSameWithCommand(recdPtr.m_userid, packet);
                }
                else
                {
                    log.Info("parse re:" + "QyTech." + bsP.Code + ".ProtocalFac" + "." + dtup.bsP_Id + tp[0].ToString());
                 
                }
                #region 2016 以前的协议
                //////else if (dtup.CPCode == (int)HeatingDSC.BLL.CommProtocal.GprsHjHrz)
                //////{
                //////    //靠长度解码还是靠匹配解码？
                //////    //答：靠匹配的话下面程序可能有处理不了的包，没有回应包，所以还是采用长度匹配
                //////    #region 新增加的鸿觉协议
                //////    //////////////////ppf = new PacketFac_Hj();
                //////    //////////////////switch ((AcceptPacketLength_Hj)recdPtr.m_data_len)
                //////    //////////////////{
                //////    //////////////////    case AcceptPacketLength_Hj.HjReadGath:
                //////    //////////////////        product = ppf.CreatePacket(ProductType.HJGathData1);
                //////    //////////////////        product.delProtocalDataReceivedhandler += new delProtocalDataReceived(product_delProtocalDataReceivedhandler);
                //////    //////////////////        product.Parse(recdPtr);
                //////    //////////////////        product = null;
                //////    //////////////////        break;
                //////    //////////////////    case AcceptPacketLength_Hj.HjReadWriteData1:
                //////    //////////////////        product = ppf.CreatePacket(ProductType.HJPlcParaControl1);
                //////    //////////////////        product.delProtocalDataReceivedhandler += new delProtocalDataReceived(product_delProtocalDataReceivedhandler);
                //////    //////////////////        product.Parse(recdPtr);
                //////    //////////////////        product = null;
                //////    //////////////////        break;
                //////    //////////////////    case AcceptPacketLength_Hj.HjReadWriteData2:
                //////    //////////////////        product = ppf.CreatePacket(ProductType.HJPlcParaControl2);
                //////    //////////////////        product.delProtocalDataReceivedhandler += new delProtocalDataReceived(product_delProtocalDataReceivedhandler);
                //////    //////////////////        product.Parse(recdPtr);
                //////    //////////////////        product = null;
                //////    //////////////////        break;
                //////    //////////////////    case AcceptPacketLength_Hj.HjCureve:
                //////    //////////////////        product = ppf.CreatePacket(ProductType.HJPlcParaControlCurve);
                //////    //////////////////        product.delProtocalDataReceivedhandler += new delProtocalDataReceived(product_delProtocalDataReceivedhandler);
                //////    //////////////////        product.Parse(recdPtr);
                //////    //////////////////        product = null;
                //////    //////////////////        break;
                //////    //////////////////    case AcceptPacketLength_Hj.HjRangeGath:
                //////    //////////////////        product = ppf.CreatePacket(ProductType.GathRangeData_Hj);
                //////    //////////////////        product.delProtocalDataReceivedhandler += new delProtocalDataReceived(product_delProtocalDataReceivedhandler);
                //////    //////////////////        product.Parse(recdPtr);
                //////    //////////////////        {
                //////    //////////////////            byte[] da1 = new byte[recdPtr.m_data_len];
                //////    //////////////////            Buffer.BlockCopy(recdPtr.m_data_buf, 0, da1, 0, recdPtr.m_data_len);
                //////    //////////////////            IProduct product1 = new IProduct();
                //////    //////////////////            string packet1 = product.bytes2HexForThread(da1);

                //////    //////////////////            JudgeSameWithCommand(recdPtr.m_userid, packet1);
                //////    //////////////////        }
                //////    //////////////////        product = null;
                //////    //////////////////        break;
                //////    //////////////////    case AcceptPacketLength_Hj.HjAlarmGath:
                //////    //////////////////        product = ppf.CreatePacket(ProductType.GathAlarmData_Hj);
                //////    //////////////////        product.delProtocalDataReceivedhandler += new delProtocalDataReceived(product_delProtocalDataReceivedhandler);
                //////    //////////////////        product.Parse(recdPtr);
                //////    //////////////////        {
                //////    //////////////////            byte[] da1 = new byte[recdPtr.m_data_len];
                //////    //////////////////            Buffer.BlockCopy(recdPtr.m_data_buf, 0, da1, 0, recdPtr.m_data_len);
                //////    //////////////////            IProduct product1 = new IProduct();
                //////    //////////////////            string packet1 = product.bytes2HexForThread(da1);

                //////    //////////////////            JudgeSameWithCommand(recdPtr.m_userid, packet1);
                //////    //////////////////        }

                //////    //////////////////        product = null;
                //////    //////////////////        break;
                //////    //////////////////    case AcceptPacketLength_Hj.Response:

                //////    //////////////////        //recdPtrNew.m_data_len = (ushort)AcceptPacketLength_Hj.Response;
                //////    //////////////////        //recdPtrNew.m_data_buf = new byte[recdPtrNew.m_data_len];
                //////    //////////////////        //Buffer.BlockCopy(recdPtr.m_data_buf, 0, recdPtrNew.m_data_buf, 0, (int)recdPtrNew.m_data_len);

                //////    //////////////////        //string strResponse = bytes2Hex(recdPtrNew.m_data_buf);

                //////    //////////////////        //JudgeSameWithCommand(recdPtr.m_userid, strResponse);

                //////    //////////////////        break;
                //////    //////////////////}

                //////    #endregion
                //////}
                //else
                //{
                #region 界面2015年前的协议
                //////////////////if ((AcceptPacketLength)recdPtr.m_data_len == AcceptPacketLength.Qxy | (AcceptPacketLength)recdPtr.m_data_len == AcceptPacketLength.GlfJinLai | (AcceptPacketLength)recdPtr.m_data_len == AcceptPacketLength.GlfGaoDianChang | (AcceptPacketLength)recdPtr.m_data_len == AcceptPacketLength.HrzUnitRange)
                //////////////////{
                //////////////////    #region 单独包解码
                //////////////////    switch ((AcceptPacketLength)recdPtr.m_data_len)
                //////////////////    {
                //////////////////        case AcceptPacketLength.Qxy:
                //////////////////            product = ppf.CreatePacket(ProductType.QxyUpData);
                //////////////////            product.Parse(recdPtr);//需要开个线程去处理吗？或者写队列，从队列里处理输出，应该不用，因为是轮训来的数据，不会突然大批量来
                //////////////////            product = null;
                //////////////////            break;
                //////////////////        case AcceptPacketLength.GlfJinLai:
                //////////////////            product = ppf.CreatePacket(ProductType.GlfofJinLaiUpData);
                //////////////////            product.Parse(recdPtr);//需要开个线程去处理吗？或者写队列，从队列里处理输出，应该不用，因为是轮训来的数据，不会突然大批量来
                //////////////////            product = null;
                //////////////////            break;
                //////////////////        case AcceptPacketLength.GlfGaoDianChang:
                //////////////////            product = ppf.CreatePacket(ProductType.GlfofGaodianchangUpData);
                //////////////////            product.Parse(recdPtr);//需要开个线程去处理吗？或者写队列，从队列里处理输出，应该不用，因为是轮训来的数据，不会突然大批量来
                //////////////////            product = null;
                //////////////////            break;
                //////////////////        case AcceptPacketLength.HrzUnitRange:
                //////////////////            product = ppf.CreatePacket(ProductType.HrzUpUnitRangeData);
                //////////////////            product.delHrzJzUnitRangeDataHandler += new delHrzJzUnitRangeData(product_delHrzJzUnitRangeDataHandler);
                //////////////////            product.Parse(recdPtr);

                //////////////////            product = null;
                //////////////////            break;
                //////////////////    }
                //////////////////    #endregion

                //////////////////    #region Mix Not use
                //////////////////    //else if ((AcceptPacketLength)recdPtr.m_data_len==AcceptPacketLength.MixHrzUintAndResponse)
                //////////////////    //{
                //////////////////    //    recdPtrNew = new GPRS_DATA_RECORD();
                //////////////////    //    if (recdPtr.m_data_buf[1] == 3)
                //////////////////    //    {
                //////////////////    //        recdPtrNew.m_data_len = (ushort)AcceptPacketLength.HrzUnit;// (ushort)(recdPtr.m_data_len / 2);
                //////////////////    //        recdPtrNew.m_data_buf = new byte[recdPtrNew.m_data_len];
                //////////////////    //        recdPtrNew.m_data_type = recdPtr.m_data_type;
                //////////////////    //        recdPtrNew.m_recv_date = recdPtr.m_recv_date;
                //////////////////    //        recdPtrNew.m_userid = recdPtr.m_userid;

                //////////////////    //        Buffer.BlockCopy(recdPtr.m_data_buf, 0, recdPtrNew.m_data_buf, 0, (int)recdPtrNew.m_data_len);
                //////////////////    //        product = ppf.CreatePacket(ProductType.HrzUpUnitData);
                //////////////////    //        log.Info("info:date:" + recdPtr.m_recv_date + ";src:" + recdPtr.m_userid + ";type:" + ((AcceptPacketLength)recdPtrNew.m_data_len).ToString() + "- data:" + IProduct.bytes2Hex(da));
                //////////////////    //        product.Parse(recdPtrNew);//

                //////////////////    //        recdPtrNew.m_data_len = (ushort)AcceptPacketLength.Response;
                //////////////////    //        recdPtrNew.m_data_buf = new byte[recdPtrNew.m_data_len];
                //////////////////    //        Buffer.BlockCopy(recdPtr.m_data_buf, (ushort)AcceptPacketLength.HrzUnit, recdPtrNew.m_data_buf, 0, (int)recdPtrNew.m_data_len);
                //////////////////    //        product = ppf.CreatePacket(ProductType.ResponseData);
                //////////////////    //        log.Info("info:date:" + recdPtrNew.m_recv_date + ";src:" + recdPtr.m_userid + ";type:" + ((AcceptPacketLength)recdPtrNew.m_data_len).ToString() + "- data:" + IProduct.bytes2Hex(da));
                //////////////////    //        product.Parse(recdPtrNew);
                //////////////////    //    }
                //////////////////    //    else
                //////////////////    //    {
                //////////////////    //        recdPtrNew.m_data_len = (ushort)AcceptPacketLength.Response;// (ushort)(recdPtr.m_data_len / 2);
                //////////////////    //        recdPtrNew.m_data_buf = new byte[recdPtrNew.m_data_len];
                //////////////////    //        recdPtrNew.m_data_type = recdPtr.m_data_type;
                //////////////////    //        recdPtrNew.m_recv_date = recdPtr.m_recv_date;
                //////////////////    //        recdPtrNew.m_userid = recdPtr.m_userid;

                //////////////////    //        Buffer.BlockCopy(recdPtr.m_data_buf, 0, recdPtrNew.m_data_buf, 0, (int)recdPtrNew.m_data_len);
                //////////////////    //        product = ppf.CreatePacket(ProductType.ResponseData);
                //////////////////    //        log.Info("info:date:" + recdPtrNew.m_recv_date + ";src:" + recdPtr.m_userid + ";type:" + ((AcceptPacketLength)recdPtrNew.m_data_len).ToString() + "- data:" + IProduct.bytes2Hex(da));
                //////////////////    //        product.Parse(recdPtrNew);


                //////////////////    //        recdPtrNew.m_data_len = (ushort)AcceptPacketLength.HrzUnit;
                //////////////////    //        recdPtrNew.m_data_buf = new byte[recdPtrNew.m_data_len];
                //////////////////    //        Buffer.BlockCopy(recdPtr.m_data_buf, (ushort)AcceptPacketLength.Response, recdPtrNew.m_data_buf, 0, (int)recdPtrNew.m_data_len);
                //////////////////    //        product = ppf.CreatePacket(ProductType.HrzUpUnitData);
                //////////////////    //        log.Info("info:date:" + recdPtr.m_recv_date + ";src:" + recdPtr.m_userid + ";type:" + ((AcceptPacketLength)recdPtrNew.m_data_len).ToString() + "- data:" + IProduct.bytes2Hex(da));
                //////////////////    //        product.Parse(recdPtrNew);//


                //////////////////    //    }
                //////////////////    //}
                //////////////////    //else if ((AcceptPacketLength)recdPtr.m_data_len == AcceptPacketLength.MixHrzUnitTwo)
                //////////////////    //{
                //////////////////    //    recdPtrNew = new GPRS_DATA_RECORD();
                //////////////////    //    recdPtrNew.m_data_len = (ushort)AcceptPacketLength.HrzUnit;
                //////////////////    //    recdPtrNew.m_data_buf = new byte[recdPtrNew.m_data_len];
                //////////////////    //    recdPtrNew.m_data_type = recdPtr.m_data_type;
                //////////////////    //    recdPtrNew.m_recv_date = recdPtr.m_recv_date;
                //////////////////    //    recdPtrNew.m_userid = recdPtr.m_userid;

                //////////////////    //    Buffer.BlockCopy(recdPtr.m_data_buf, 0, recdPtrNew.m_data_buf, 0, (int)recdPtrNew.m_data_len);
                //////////////////    //    product = ppf.CreatePacket(ProductType.HrzUpUnitData);
                //////////////////    //    product.Parse(recdPtrNew);//

                //////////////////    //    Buffer.BlockCopy(recdPtr.m_data_buf, (int)recdPtrNew.m_data_len, recdPtrNew.m_data_buf, 0, (int)recdPtrNew.m_data_len);
                //////////////////    //    product = ppf.CreatePacket(ProductType.HrzUpUnitData);
                //////////////////    //    product.Parse(recdPtrNew);      
                //////////////////    //}
                //////////////////    //else if (recdPtr.m_data_len == 315)//三个换热站一起返回
                //////////////////    //{
                //////////////////    //    recdPtrNew = new GPRS_DATA_RECORD();
                //////////////////    //    recdPtrNew.m_data_len = (ushort)AcceptPacketLength.HrzUnit;
                //////////////////    //    recdPtrNew.m_data_buf = new byte[recdPtrNew.m_data_len];
                //////////////////    //    recdPtrNew.m_data_type = recdPtr.m_data_type;
                //////////////////    //    recdPtrNew.m_recv_date = recdPtr.m_recv_date;
                //////////////////    //    recdPtrNew.m_userid = recdPtr.m_userid;

                //////////////////    //    Buffer.BlockCopy(recdPtr.m_data_buf, 0, recdPtrNew.m_data_buf, 0, (int)recdPtrNew.m_data_len);
                //////////////////    //    product = ppf.CreatePacket(ProductType.HrzUpUnitData);
                //////////////////    //    product.Parse(recdPtrNew);//

                //////////////////    //    Buffer.BlockCopy(recdPtr.m_data_buf, (int)recdPtrNew.m_data_len, recdPtrNew.m_data_buf, 0, (int)recdPtrNew.m_data_len);
                //////////////////    //    product = ppf.CreatePacket(ProductType.HrzUpUnitData);
                //////////////////    //    product.Parse(recdPtrNew);    

                //////////////////    //    Buffer.BlockCopy(recdPtr.m_data_buf, (int)recdPtrNew.m_data_len*2, recdPtrNew.m_data_buf, 0, (int)recdPtrNew.m_data_len);
                //////////////////    //    product = ppf.CreatePacket(ProductType.HrzUpUnitData);
                //////////////////    //    product.Parse(recdPtrNew);      
                //////////////////    //}
                //////////////////    #endregion
                //////////////////}
                //////////////////else
                //////////////////{
                //////////////////    #region 换热站(混合)数据
                //////////////////    //获取包的头部3个字节
                //////////////////    byte[] buff = new byte[3];
                //////////////////    int srcOffset = 0, srcOperateEnd = 0;
                //////////////////    while (srcOperateEnd < recdPtr.m_data_len)
                //////////////////    {
                //////////////////        Buffer.BlockCopy(recdPtr.m_data_buf, srcOffset, buff, 0, buff.Length);
                //////////////////        string head = IProduct.bytes2Hex(buff);
                //////////////////        recdPtrNew = new GPRS_DATA_RECORD();
                //////////////////        recdPtrNew.m_data_type = recdPtr.m_data_type;
                //////////////////        recdPtrNew.m_recv_date = recdPtr.m_recv_date;
                //////////////////        recdPtrNew.m_userid = recdPtr.m_userid;

                //////////////////        if (head == HrzResponsePacketHead[0])//comm
                //////////////////        {
                //////////////////            recdPtrNew.m_data_len = (ushort)AcceptPacketLength.HrzCommData;
                //////////////////            recdPtrNew.m_data_buf = new byte[recdPtrNew.m_data_len];
                //////////////////            if (recdPtr.m_data_buf.Length - srcOffset < recdPtrNew.m_data_len)
                //////////////////                return 0;
                //////////////////            Buffer.BlockCopy(recdPtr.m_data_buf, srcOffset, recdPtrNew.m_data_buf, 0, (int)recdPtrNew.m_data_len);

                //////////////////            product = ppf.CreatePacket(ProductType.HrzUpCommData);
                //////////////////            product.delHrzCommonDataHandler += new delHrzCommData(phucd_delHrzCommonDataHandler);
                //////////////////            product.Parse(recdPtrNew);//

                //////////////////            srcOperateEnd += (int)AcceptPacketLength.HrzCommData;
                //////////////////            srcOffset += (int)AcceptPacketLength.HrzCommData;
                //////////////////        }
                //////////////////        else if (head == HrzResponsePacketHead[1])//unit
                //////////////////        {
                //////////////////            recdPtrNew.m_data_len = (ushort)AcceptPacketLength.HrzUnit;
                //////////////////            recdPtrNew.m_data_buf = new byte[recdPtrNew.m_data_len];
                //////////////////            if (recdPtr.m_data_buf.Length - srcOffset < recdPtrNew.m_data_len)
                //////////////////                return 0;
                //////////////////            Buffer.BlockCopy(recdPtr.m_data_buf, srcOffset, recdPtrNew.m_data_buf, 0, (int)recdPtrNew.m_data_len);

                //////////////////            product = ppf.CreatePacket(ProductType.HrzUpUnitData);
                //////////////////            product.delHrzJzUnitDataHandler += new delHrzJzUnitData(product_delHrzJzUnitDataHandler);
                //////////////////            product.Parse(recdPtrNew);//

                //////////////////            srcOperateEnd += (int)AcceptPacketLength.HrzUnit;
                //////////////////            srcOffset += (int)AcceptPacketLength.HrzUnit;
                //////////////////        }
                //////////////////        else if (head.Substring(0, 4) == HrzResponsePacketHead[2])
                //////////////////        {
                //////////////////            recdPtrNew.m_data_len = (ushort)AcceptPacketLength.HrzUnitRange;
                //////////////////            recdPtrNew.m_data_buf = new byte[recdPtrNew.m_data_len];
                //////////////////            Buffer.BlockCopy(recdPtr.m_data_buf, srcOffset, recdPtrNew.m_data_buf, 0, (int)recdPtrNew.m_data_len);

                //////////////////            product = ppf.CreatePacket(ProductType.HrzUpUnitData);
                //////////////////            product.delHrzJzUnitDataHandler += new delHrzJzUnitData(product_delHrzJzUnitDataHandler);
                //////////////////            product.Parse(recdPtrNew);//

                //////////////////            srcOperateEnd += (int)AcceptPacketLength.HrzUnit;
                //////////////////            srcOffset += (int)AcceptPacketLength.HrzUnit;
                //////////////////        }
                //////////////////        else if (head.Substring(0, 4) == HrzResponsePacketHead[3])//set response
                //////////////////        {

                //////////////////            srcOperateEnd += (int)AcceptPacketLength.Response;
                //////////////////            srcOffset += (int)AcceptPacketLength.Response;
                //////////////////        }
                //////////////////        //else if (head.Substring(0, 4) == HrzResponsePacketHead[3])//range set
                //////////////////        //{
                //////////////////        //    recdPtrNew.m_data_len = (ushort)AcceptPacketLength.HrzRangeData;
                //////////////////        //    recdPtrNew.m_data_buf = new byte[recdPtrNew.m_data_len];
                //////////////////        //    Buffer.BlockCopy(recdPtr.m_data_buf, srcOffset, recdPtrNew.m_data_buf, 0, (int)recdPtrNew.m_data_len);

                //////////////////        //    product = ppf.CreatePacket(ProductType.HrzUpUnitRangeData);
                //////////////////        //    product.delHrzJzUnitRangeDataHandler+=new delHrzJzUnitRangeData(product_delHrzJzUnitRangeDataHandler); 
                //////////////////        //    product.Parse(recdPtrNew);//

                //////////////////        //    srcOperateEnd += (int)AcceptPacketLength.HrzUnit;
                //////////////////        //    srcOffset += (int)AcceptPacketLength.HrzUnit;
                //////////////////        //}                    
                //////////////////        else
                //////////////////        {
                //////////////////            log.Error(recdPtr.m_userid + "数据无法处理，请考虑数据实际, 在万德协议中不是公共，机组，量程范围，回应包");
                //////////////////            break;
                //////////////////        }
                //////////////////    }
                //////////////////    #endregion

                //////////////////}
                #endregion
                //}
                #endregion
                return resualt;
            }
            catch (Exception ex)
            {
                log.Error("ProcessData error::" + recdPtr.m_userid + " length:" + recdPtr.m_data_len.ToString() + " data:" + packet + "---" + ex.Message);
                return -1;
            }
        }

        private void product_delProtocalDataReceivedhandler(string simcardno, object obj)
        {
            //根据obj的不同类型，刷新不同的tabpage

        }

        /// <summary>
        /// 0 正常，-1表示失败
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public int SendCommand(DeviceCmd cmd)
        {
            int ret=-1;
            int errPos = 10000;
            try
            {
                //if (cmd.ExpiredTime < DateTime.Now)
                //{
                //    if (chkSend.Checked && this.txtCommNo.Text == cmd.CommNo)
                //    {
                //        this.txtSendData.Text = "Expired:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + cmd.CommNo + "-" + cmd.Command + "\r\n" + this.txtSendData.Text;

                //    }
                //    return 9999;
                //}
                errPos = 7000;
                if (chkSend.Checked && this.txtCommNo.Text == cmd.CommNo)
                {
                    this.txtSendData.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + cmd.CommNo + "-" + cmd.Command + "\r\n" + this.txtSendData.Text;
                }
                errPos = 8000;
                if (dtu_trantypes.ContainsKey(cmd.CommNo))
                {
                    errPos = 9000;
                    if (dtu_trantypes[cmd.CommNo] == "无线方式")//宏电方式
                    {
                        errPos = 9001;
                        StringBuilder mess = new StringBuilder(100);
                        errPos = 9002;
                        ret = Gprs.do_send_user_data(cmd.CommNo, cmd.SendCmd, cmd.SendCmd.Length, mess);
                        errPos = 9003;
                    }
                    else if (dtu_trantypes[cmd.CommNo] == "无线--计讯")
                    {
                        //计讯方式
                        errPos = 9004;
                        if (dtu_JxInfoFromCommNo.ContainsKey(cmd.CommNo))
                        {
                            ret = this.SendData(dtu_JxInfoFromCommNo[cmd.CommNo].ID, cmd.SendCmd);
                            errPos = 9005;
                        }
                        else
                            errPos = 9050;
                    }
                    else if (dtu_trantypes[cmd.CommNo] == "无线映翰通")//映翰通
                    {
                        errPos = 9001;
                        StringBuilder mess = new StringBuilder(100);
                        errPos = 9002;
                        bool retflag = inhandServ.SendData(cmd.CommNo, cmd.SendCmd, mess);
                        ret = retflag ? 0 : -1;
                        errPos = 9003;
                    }
                    else
                    {
                        errPos = 9006;
                        StringBuilder mess = new StringBuilder(100);
                        ret = Gprs.do_send_user_data(cmd.CommNo, cmd.SendCmd, cmd.SendCmd.Length, mess);
                        errPos = 9007;
                    }
                }
                else
                {
                    errPos = 9010;
                    StringBuilder mess = new StringBuilder(100);
                    ret = Gprs.do_send_user_data(cmd.CommNo, cmd.SendCmd, cmd.SendCmd.Length, mess);
                    errPos = 9020;
                }
            }
            catch (Exception ex)
            {
                log.Error("SendCommand:" + ex.Message + "--errPos:" + errPos.ToString() + cmd.CommNo+"-"+cmd.Command);
                if (chkSend.Checked && this.txtCommNo.Text == cmd.CommNo)
                {
                    this.txtSendData.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + cmd.CommNo + "-" + cmd.Command + "\r\n" + this.txtSendData.Text;
                }
            }
            //string retdesp = ret == -1 ? "Fail" : "Succ";
            //log.Info("SendCommand(" + retdesp + "):" + cmd.CommNo + "-" + cmd.Command);
            log.Debug("SendCommand:" + "--errPos:" + errPos.ToString() + cmd.CommNo + "-" + cmd.Command);
            return ret;
        }

        #region Control Part




        //private void button3_Click(object sender, EventArgs e)
        //{
        //    d = hrzda.SelectHrzGprsDevice(this.ucTvDevice1.OrgId);
        //    byte[] commdarr = new byte[9];
        //    DeviceCmd cmd2send;
        //    SIMCardInfo siminfo;
        //    try
        //    {
        //        siminfo = simda.SelectSIMCardInfo((int)d.simcardid);
        //        cmd2send = new DeviceCmd();
        //        cmd2send.CommNo = siminfo.SIMCardNo;

        //        string[] sets = d.CommProtocal.CPCode.Split(new char[] { ',', ';' });
        //        ModbusCommand.VerifyMode = ModbusCommand.RTU;
        //        ModbusCommand.Slaveaddr = Convert.ToByte(sets[0]);
        //        ModbusCommand.OperatMode = 0x10;
        //        ModbusCommand.RegStartAddr = 0x26;//首地址16进制
        //        ModbusCommand.RegOpNum = 1;//字节数量10进制

        //        byte[] sendData = BitConverter.GetBytes((short)cboAutoOrNot.SelectedIndex); IProduct.CrossHiLow(ref sendData);
        //        ModbusCommand.byteArr = sendData;

        //        if (ModbusCommand.byteArr != null)//如果有下发的数据
        //        {
        //            commdarr = ModbusCommand.Command();
        //            cmd2send.SendCmd = commdarr;
        //            //生成命令数组
        //            if (commdarr != null)
        //            {
        //                StringBuilder strbuilder = new StringBuilder();
        //                foreach (byte cmdbyte in commdarr)
        //                {
        //                    strbuilder.Append(cmdbyte.ToString("X2"));
        //                }
        //                cmd2send.Command = strbuilder.ToString();
        //            }

        //            int ret = SendCommand(cmd2send);
        //            if (ret == 0)
        //            {
        //                MessageBox.Show("发送成功");
        //            }
        //            else
        //                MessageBox.Show("发送失败");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorForNotSelectHrz();
        //    }
        //}

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    d = hrzda.SelectHrzGprsDevice(this.ucTvDevice1.OrgId);
        //    byte[] commdarr = new byte[9];
        //    DeviceCmd cmd2send;
        //    SIMCardInfo siminfo;
        //    try
        //    {
        //        siminfo = simda.SelectSIMCardInfo((int)d.simcardid);
        //        cmd2send = new DeviceCmd();
        //        cmd2send.CommNo = siminfo.SIMCardNo;

        //        string[] sets = d.CommProtocal.CPCode.Split(new char[] { ',', ';' });
        //        ModbusCommand.VerifyMode = ModbusCommand.RTU;
        //        ModbusCommand.Slaveaddr = Convert.ToByte(sets[0]);
        //        ModbusCommand.OperatMode = 0x10;
        //        ModbusCommand.RegStartAddr = 0x14;//首地址16进制
        //        ModbusCommand.RegOpNum = 4;//字节数量10进制

        //        byte[] sendData = new byte[8];
        //        byte[] buff = BitConverter.GetBytes(Convert.ToSingle(this.txtP.Text)); IProduct.CrossHiLow(ref buff);
        //        Buffer.BlockCopy(buff, 0, sendData, 0, 4);
        //        buff = BitConverter.GetBytes(Convert.ToSingle(this.txtI.Text)); IProduct.CrossHiLow(ref buff);
        //        Buffer.BlockCopy(buff, 0, sendData, 4, 4);

        //        ModbusCommand.byteArr = sendData;

        //        if (ModbusCommand.byteArr != null)//如果有下发的数据
        //        {
        //            commdarr = ModbusCommand.Command();
        //            cmd2send.SendCmd = commdarr;
        //            //生成命令数组
        //            if (commdarr != null)
        //            {
        //                StringBuilder strbuilder = new StringBuilder();
        //                foreach (byte cmdbyte in commdarr)
        //                {
        //                    strbuilder.Append(cmdbyte.ToString("X2"));
        //                }
        //                cmd2send.Command = strbuilder.ToString();
        //            }

        //            int ret = SendCommand(cmd2send);
        //            if (ret == 0)
        //            {
        //                MessageBox.Show("发送成功");
        //            }
        //            else
        //                MessageBox.Show("发送失败");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorForNotSelectHrz();
        //    }
        //}

        private void ErrorForNotSelectHrz()
        {
            MessageBox.Show("请首先选择换热站！");
        }

        //private void button4_Click_1(object sender, EventArgs e)
        //{
        //    //this.ucTvDevice1.OrgId; 
        //    //curs[cboCurve.SelectedIndex].HrzRunCurveID
        //    //创建命令
        //    d = hrzda.SelectHrzGprsDevice(this.ucTvDevice1.OrgId);
        //    byte[] commdarr = new byte[9];
        //    DeviceCmd cmd2send;
        //    SIMCardInfo siminfo;
        //    try
        //    {
        //        siminfo = simda.SelectSIMCardInfo((int)d.simcardid);
        //        cmd2send = new DeviceCmd();
        //        cmd2send.CommNo = siminfo.SIMCardNo;

        //        string[] sets = d.CommProtocal.CPCode.Split(new char[] { ',', ';' });
        //        ModbusCommand.VerifyMode = ModbusCommand.RTU;
        //        ModbusCommand.Slaveaddr = Convert.ToByte(sets[0]);
        //        ModbusCommand.OperatMode = 0x10;

        //        //PacketFac ppf = new PacketFac();
        //        //IProduct product = ppf.CreatePacket(ProductType.hrzDownCurveOnly);
        //        byte[] sendData = DownCurves();
        //        ModbusCommand.byteArr = null;
        //        if (sendData != null)
        //        {
        //            if (true)
        //            {
        //                ModbusCommand.RegStartAddr = 0x24;//首地址16进制
        //                ModbusCommand.RegOpNum = 63;//字节数量10进制
        //                byte[] temp = new byte[126];
        //                Buffer.BlockCopy(sendData, 8, temp, 0, 126);
        //                ModbusCommand.byteArr = temp;
        //            }
        //            else
        //            {
        //                ModbusCommand.RegStartAddr = 0x20;//首地址16进制
        //                ModbusCommand.RegOpNum = 67;//字节数量10进制
        //                ModbusCommand.byteArr = sendData;
        //            }
        //            //ModbusCommand.byteArr[7] = 0xFF;
        //            //log.Error("asdfasdfasdf" + d.DeviceName);
        //        }
        //        if (ModbusCommand.byteArr != null)//如果有下发的数据
        //        {
        //            commdarr = ModbusCommand.Command();
        //            cmd2send.SendCmd = commdarr;
        //            //生成命令数组
        //            if (commdarr != null)
        //            {
        //                StringBuilder strbuilder = new StringBuilder();
        //                foreach (byte cmdbyte in commdarr)
        //                {
        //                    strbuilder.Append(cmdbyte.ToString("X2"));
        //                }
        //                cmd2send.Command = strbuilder.ToString();
        //            }

        //            int ret = SendCommand(cmd2send);
        //            if (ret == 0)
        //            {
        //                MessageBox.Show("发送成功");
        //            }
        //            else
        //            {
        //                MessageBox.Show("发送失败");
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("下发曲线错误:(" + ex.InnerException + "-" + ex.Message + ")");
        //    }
        //}

        //private byte[] DownCurves()
        //{
        //    byte[] data = new byte[134];//[134];  //(2+2+3+60)*2             //old:(120+3+2*3)*2
        //    int byteindex = 0;
        //    byte[] buff = new byte[4];

        //    float scale = 1;
        //    float offset = 0;
        //    if (false)
        //    {
        //        scale = 0; //Convert.ToSingle(txtScale.Text);
        //        offset = 0; //Convert.ToSingle(txtOffset.Text);
        //    }
        //    buff = BitConverter.GetBytes(scale);
        //    IProduct.CrossHiLow(ref buff);
        //    Buffer.BlockCopy(buff, 0, data, 0, 4);

        //    buff = BitConverter.GetBytes(offset);
        //    IProduct.CrossHiLow(ref buff);
        //    Buffer.BlockCopy(buff, 0, data, 4, 4);

        //    byteindex = 8;
        //    buff = new byte[2];
        //    buff = BitConverter.GetBytes((short)curs[cboCurve.SelectedIndex].CurevePointCount);
        //    //int a = BitConverter.ToInt16(buff, 0);
        //    IProduct.CrossHiLow(ref buff);
        //    Buffer.BlockCopy(buff, 0, data, byteindex, 2);

        //    //预留字节
        //    buff = BitConverter.GetBytes((short)curs[cboCurve.SelectedIndex].HrzRunCurveID);
        //    IProduct.CrossHiLow(ref buff);
        //    Buffer.BlockCopy(buff, 0, data, byteindex += 2, 2);
        //    //电动调节阀自控标志
        //    int controlflag=0;
        //    HrzCommGath hcg = hrzda.SelectCommGath(CurrentHrzOrgId);
        //    if (hcg != null)
        //        controlflag = (int)hcg.ControlStatus;

        //    buff = BitConverter.GetBytes((short)controlflag);
        //    IProduct.CrossHiLow(ref buff);
        //    Buffer.BlockCopy(buff, 0, data, byteindex += 2, 2);

        //    byteindex = 14;
        //    for (int i = 0; i < 60; i++)
        //    {
        //        string v;
        //        try
        //        {
        //            v = dataGridView1.Rows[1].Cells[i + 1].Value.ToString();
        //        }
        //        catch
        //        {
        //            v = "0";
        //        }
        //        data[byteindex + i * 2] = Convert.ToByte(v);

        //        try
        //        {
        //            v = dataGridView1.Rows[0].Cells[i + 1].Value.ToString();
        //        }
        //        catch
        //        {
        //            v = "0";
        //        }

        //        if (Convert.ToInt32(v) >= 0)
        //            data[byteindex + i * 2 + 1] = Convert.ToByte(v);
        //        else
        //            data[byteindex + i * 2 + 1] = Convert.ToByte(256 + Convert.ToInt32(v));//128+downHrzcurve_.rc.MInWD + downHrzcurve_.rc.WDStep * i);
        //    }
        //    return data;
        //}



        private int GCM(int m, int n)
        {
            int r, t;
            if (m < n)
            { t = m; m = n; n = t; }
            r = m % n;
            while (r != 0)
            {
                m = n;
                n = r;
                r = m % n;
            }
            return n;
        }
        private int LCM(int m, int n)
        {
            return m * n / GCM(m, n);
        }

        #endregion

  
   
        private void RecordLogError(string desp, Exception ex)
        {
            log.Error(desp + ":" + ex.InnerException + "-" + ex.Message);
        }
    
 
        /// <summary>
        /// 判断dtu是否断线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trDtuTimeOut_Tick(object sender, EventArgs e)
        {
            int i, iMaxDTUAmount;
            GPRS_USER_INFO ui = new GPRS_USER_INFO();
            DateTime t_now, t_update;

            //#region 计讯
            //获取开发包支持的最大连接数
            iMaxDTUAmount = Gprs.get_online_user_amount();
            log.Info("在线数量：" + iMaxDTUAmount.ToString());
            //OpSimNo.Clear();
            for (i = 0; i < iMaxDTUAmount; i++)
            {
                try
                {
                    ui.m_status = 0;
                    //获取指定位置的DTU信息
                    Gprs.get_user_at(i, ref ui);
                    if (1 == ui.m_status)
                    {
                        //取当前系统时间
                        t_now = DateTime.Now;
                        //取m_update_time的时间
                        t_update = Convert.ToDateTime(m_update_timeToDT(ui.m_update_time));
                        //用当前时间比较最后一次更新的时间
                        TimeSpan ts = new TimeSpan();
                        ts = t_now - t_update;
                        if (ts.TotalSeconds >= ProgParams.dtutimeout)
                        {
                            if (OpSimNo.ContainsKey(ui.m_userid))
                            {
                                //if (OpSimNo[ui.m_userid].IndexOf("已掉线")==-1)
                                OpSimNo[ui.m_userid] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "超时(" + ProgParams.dtutimeout.ToString() + ")";
                                //else
                                //    OpSimNo[ui.m_userid] = OpSimNo[ui.m_userid] +DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            //OpSimNo.Remove(ui.m_userid);
                            // Gprs.do_close_one_user2(ui.m_userid, null);
                        }
                        else
                        {
                            if (!OpSimNo.ContainsKey(ui.m_userid))
                                OpSimNo.Add(ui.m_userid, ui.m_logon_date + "---" + t_update.ToString("MM-dd HH:mm:ss") + "   ");
                            else
                                OpSimNo[ui.m_userid] = ui.m_logon_date + "---" + t_update.ToString("MM-dd HH:mm:ss") + "   ";

                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("dtutimeout:" + ex.InnerException + "-" + ex.Message);
                }

            }

            //处理计讯服务的dtu状态
            lock (JxDtuOperateStatus)
            {
                List<DtuInfo> jxstatus = this.GetDtuLinkStatus();
                log.Info("计讯联接的dtu数量是：" + jxstatus.Count.ToString() + "个");
                foreach (DtuInfo di in jxstatus)
                {
                    try
                    {
                        log.Error(di.PhoneNO + ":" + di.DynIP + ":" + di.ID.ToString() + ":" + di.LastActTime.ToShortTimeString() + ":" + di.ConnectTime.ToShortTimeString());

                        if (!dtu_JxInfoFromCommNo.ContainsKey(di.PhoneNO))
                        {
                            dtu_JxInfoFromCommNo.Add(di.PhoneNO, di);
                        }
                        if (!dtu_JxInfoFromIdToCommNo.ContainsKey(di.ID))
                        {
                            dtu_JxInfoFromIdToCommNo.Add(di.ID, di.PhoneNO);
                        }

                        t_now = DateTime.Now;
                        //取m_update_time的时间
                        t_update = di.LastActTime;
                        //用当前时间比较最后一次更新的时间
                        TimeSpan ts = new TimeSpan();
                        ts = t_now - t_update;
                        //if (ts.TotalSeconds >= ProgParams.dtutimeout)
                        //{
                        //    if (OpSimNo.ContainsKey(di.PhoneNO))
                        //    {
                        //        //if (OpSimNo[ui.m_userid].IndexOf("已掉线")==-1)
                        //        OpSimNo[di.PhoneNO] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "超时(" + ProgParams.dtutimeout.ToString() + ")";
                        //        //else
                        //        //    OpSimNo[ui.m_userid] = OpSimNo[ui.m_userid] +DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //    }
                        //    //OpSimNo.Remove(ui.m_userid);
                        //    // Gprs.do_close_one_user2(ui.m_userid, null);
                        //}
                        //else
                        //{
                        if (!OpSimNo.ContainsKey(di.PhoneNO))
                            OpSimNo.Add(di.PhoneNO, di.ConnectTime.ToString("yyyy-MM-dd HH:mm:ss") + "---" + t_update.ToString("MM-dd HH:mm:ss") + "   " + di.ID.ToString("X"));
                        else
                            OpSimNo[di.PhoneNO] = di.ConnectTime.ToString("yyyy-MM-dd HH:mm:ss") + "---" + t_update.ToString("MM-dd HH:mm:ss") + "   " + di.ID.ToString("X");

                        //}


                        if (!slConnectedBySimno.ContainsKey(di.PhoneNO))
                        {
                            slConnectedBySimno.Add(di.PhoneNO, slProtocalCodeBySimno[di.PhoneNO]);

                            if (!this.ucTvDevice1.ChangeColorBySimcardNo(di.PhoneNO, Color.Brown))
                            {
                                this.txtSimNoUn.Text += di.PhoneNO + " registered,刷新树不成功\r\n";
                            }
                        }




                    }
                    catch (Exception ex)
                    {
                        log.Error("dtutimeout——Jx:" + ex.InnerException + "-" + ex.Message);
                    }
                }

                foreach (string phoneno in slDtuDevice.Keys)
                {
                    continue;
                    bool findflag = false;
                    DtuInfo di = new DtuInfo() ;
                    for (int d = 0; d < jxstatus.Count;d++ )
                    {
                        if (phoneno == jxstatus[d].PhoneNO)
                        {
                            findflag = true;
                            di = jxstatus[d];
                            break;
                        }
                    }
                    if (findflag)
                    {
                        //设置数据库中在线状态
                        try
                        {

                            DTUProduct dtu = EntityManager<DTUProduct>.GetByPk<DTUProduct>("Id", slDtuDevice[di.PhoneNO].Id);
                            dtu.OnLine = true;
                            dtu.OnLineDt = DateTime.Now;
                            EntityManager<DTUProduct>.Modify<DTUProduct>(dtu);
                        }
                        catch (Exception ex)
                        {
                            log.Error(phoneno + " 2计讯在线：" + ex.Message);
                        }
                    }
                    else
                    {
                        //设置数据库中在线状态
                        try
                        {
                            
                            DTUProduct dtu = EntityManager<DTUProduct>.GetByPk<DTUProduct>("Id", slDtuDevice[phoneno].Id);
                           // if (dtu.TranType == "无线--计讯")
                           // {
                                if (dtu.OnLineDt != null && dtu.OnLineDt.Value.AddMinutes(5) < DateTime.Now)
                                {
                                    dtu.OnLine = false;
                                   // dtu.OnLineDt = DateTime.Now;
                                    EntityManager<DTUProduct>.Modify<DTUProduct>(dtu);
                                }
                           // }
                        }
                        catch (Exception ex)
                        {
                            log.Error(phoneno + " 1计讯在线：" + ex.Message);
                        }
                    }
                }
            }

            RefreshRegisters(OpSimNo);
        }
        private DateTime m_update_timeToDT(string upddt)
        {
            try
            {
                byte[] bytes = System.Text.Encoding.Default.GetBytes(upddt);
                uint d = BitConverter.ToUInt32(bytes, 0);

                DateTime dt1970 = new System.DateTime(1970, 1, 1, 0, 0, 0);
                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(dt1970);
                DateTime time = startTime.AddSeconds(d);

                return time;
            }
            catch { return DateTime.MinValue; }
        }


        //private string JudgeInputRangeSetString(string str)
        //{
        //    string[] strS = str.Split(new char[] { ',' });
        //    int[] setS = new int[strS.Length];
        //    int RightSetCount = 0;
        //    string strRight;
        //    for (int i = 0; i < strS.Length; i++)
        //    {
        //        try
        //        {
        //            setS[i] = Convert.ToInt32(strS[i]);
        //            if (setS[i] > 100)
        //                setS[i] = 100;
        //            else if (setS[i] < 0)
        //                setS[i] = 0;
        //        }
        //        catch
        //        {
        //            setS[i] = 0;
        //        }
        //    }
        //    SortByMaoPao(ref setS);
        //    strRight = "0";
        //    int curSet = 0;
        //    for (int i = 0; i < setS.Length; i++)
        //    {
        //        if ((setS[i] > curSet) && (setS[i] < 100))
        //        {
        //            strRight += "," + setS[i].ToString();
        //            curSet = setS[i];
        //        }
        //    }
        //    strRight += ",100";
        //    return strRight;
        //}
        private void SortByMaoPao(ref int[] arr)
        {
            int temp;
            for (int i = 0; i < arr.Length - 1; i++)
            {
                for (int j = i + 1; j < arr.Length; j++)
                {
                    if (arr[i] > arr[j])
                    {
                        temp = arr[i];
                        arr[i] = arr[j];
                        arr[j] = temp;

                    }
                }
            }
        }



   


        private RadioButton ReturnCheckedRadioButton(TabPage tp)
        {
            foreach (Control c in tp.Controls)
            {
                if (c.GetType().Name.ToString() == "RadioButton")
                {
                    if ((c as RadioButton).Checked)
                        return c as RadioButton;
                }
            }
            return null;
        }

        private void btnResetDtu_Click(object sender, EventArgs e)
        {
            StringBuilder mess = new StringBuilder(100);
            Gprs.ReSetUser(this.ucTvDevice1.CurrentCommNo, ref mess);
            if (mess.ToString() == "")
                MessageBox.Show("请确定dtu已经连接！");
            else
                MessageBox.Show(mess.ToString());
        }

        private void button14_Click(object sender, EventArgs e)
        {
            StringBuilder mess = new StringBuilder(100);
            Gprs.ReSetAllUser(ref mess);
            if (mess.ToString() == "")
                MessageBox.Show("请确定dtu已经连接！");
            else
                MessageBox.Show(mess.ToString());
        }

        private void 启动服务ToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                //启动GPRS服务
                if (启动服务ToolStripMenuItem.Checked)
                {

                    m_Terminated = false;
                    //发送常规命令
                    Thread thrSendNormalCommand = new Thread(new ThreadStart(SendNormalCommand));
                    thrSendNormalCommand.Start();

                    //发送命令
                    Thread thrSendCommand = new Thread(new ThreadStart(SendUnNormalCommand));
                    thrSendCommand.Start();


                    this.notifyIcon1.Text = "正在创建线程";
                    //手动下发
                    Thread thrGetHandDownCmd = new Thread(new ThreadStart(GetHandDownCommandLoop));
                    thrGetHandDownCmd.Start();

                    //命令解析
                    for (int i = 0; i < NeedParsePackets.Count; i++)
                    {
                        Thread thr = new Thread(new ParameterizedThreadStart(ProcessDataWithThreadPool));
                        thr.Priority = ThreadPriority.Highest;
                        thr.Start(NeedParsePackets.Keys[i]);
                    }

                    this.notifyIcon1.Text = "服务正在启动......";
                    #region 启动服务
                    this.notifyIcon1.Text = ""; 
                    //计讯通讯方式();
                    string ret = this.startServer((ushort)(ProgParams.GprsComPort + 10));
                    this.notifyIcon1.Text += ";" + ret;
                    tmrJiXunDtuData.Enabled = true;

                    //映瀚通 通讯方式
                    StringBuilder mess = new StringBuilder(1000);
                    bool flag = inhandServ.StartService(this.Handle, 6022,mess);
                    this.notifyIcon1.Text += ";"+ "6022服务启动";

                    //宏电通讯方式
                    StartGPRSServer();

                    // flag = inhandServ.StopService();

                    #endregion

                    if (ProgParams.ReStart)
                    {
                        Thread thrReStart = new Thread(ReStartApplication);
                        thrReStart.Start();
                    }


                }
                else
                {
                    this.notifyIcon1.Text = "dsc采集程序,正在关闭Gprs服务...";
                    m_Terminated = true;
                    Thread.Sleep(4000);
                    StringBuilder mess = new StringBuilder(50);
                    Gprs.stop_net_service(mess);
                    this.notifyIcon1.Text = "dsc采集程序" + mess.ToString();

                    this.StopServer();
                    tmrJiXunDtuData.Enabled = false;
                  
                    txtSimNo.Text = "";
                }


                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                log.Error("start gprs" + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void ucTvDevice1_delNodeMouseClickHandler(bsOrganize org,string CommNo)
        {
            this.txtCommNo.Text = CommNo;
            try
            {
                this.txtCmd.Text = slDeviceCmdsNoraml[CommNo][1].Command;
                this.chkCRC.Checked = true;
            }
            catch { this.txtCmd.Text = "没有采集命令"; }
        }

        private void btnSendCmd_Click(object sender, EventArgs e)
        {
            int flag = 0;
            try
            {
                byte crchi, crclo;
                string strCmd;
                byte[] bytes;
                byte[] cmdBytes;
                strCmd = this.txtCmd.Text.Trim();
                   
                if (!chkCRC.Checked)
                {
                    bytes = CommFunc.HexCmd2Bytes(strCmd);
                    cmdBytes = ModbusCommand.GetBytesAfterCalculateCrc16(bytes);
                    txtSendCmd.Text = CommFunc.Bytes2HexCmd(cmdBytes);
                }
                else
                {
                    cmdBytes = CommFunc.HexCmd2Bytes(strCmd);
                    txtSendCmd.Text = strCmd ;//= this.txtCmd.Text.Substring(16, 2);
                }

                flag = 10;
                DeviceCmd cmd = new DeviceCmd();
                cmd.CommNo = this.txtCommNo.Text;
                cmd.SendCmd = cmdBytes;
                cmd.Command = IProtocal.bytes2Hex(cmdBytes);
                flag = 20;
                cmd.ExpiredTime = DateTime.Now.AddSeconds(5);
                int ret=SendCommand(cmd);
                //StringBuilder mess = new StringBuilder(100);
                //int ret = Gprs.do_send_user_data(cmd.CommNo, cmd.SendCmd, cmd.SendCmd.Length, mess);
                flag = 30;
                MessageBox.Show(ret == -1 ? "Fail" : "Succ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtReceiveData.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtSendData.Text = "";

        }

        private void tmrJiXunDtuData_Tick(object sender, EventArgs e)
        {
            try
            {
                log.Info("准备接收计讯数据！");
                lock (JxDtuOperateStatus)
                {
                    List<GPRS_DATA_RECORD> gdrs = new List<GPRS_DATA_RECORD>();

                    List<ModemDataStruct> jxdata = this.GetDtuData();
                    log.Info("接收计讯数据！数据有：" + jxdata.Count.ToString());

                    foreach (ModemDataStruct mds in jxdata)
                    {
                        GPRS_DATA_RECORD gdr = new GPRS_DATA_RECORD();
                        gdr.m_userid = dtu_JxInfoFromIdToCommNo[mds.m_modemId];
                        gdr.m_recv_date = DTU_JiXun.Util.ULongToDatetime(mds.m_recv_time).ToString("yyyy-MM-dd HH:mm:ss");
                        gdr.m_data_type = 0x9;
                        gdr.m_data_len = mds.m_data_len;

                        if (chkIgnoreJxLen.Checked)
                        {
                            byte[] tmpbytes = new byte[mds.m_data_len];
                            gdr.m_data_buf = mds.m_data_buf;
                            log.Info("计讯数据转为统一接口:号码：" + gdr.m_userid + "长度:" + mds.m_data_len.ToString() + ",Data:" + IProtocal.bytes2Hex(tmpbytes));

                            gdrs.Add(gdr);
                            WriteReceiveLog(gdr);
                        }
                        else
                        {
                            if (mds.m_data_len > 0)
                            {
                                byte[] tmpbytes = new byte[mds.m_data_len];
                                mds.m_data_buf.CopyTo(tmpbytes, 0);
                                gdr.m_data_buf = tmpbytes;
                                log.Info("计讯数据转为统一接口:号码：" + gdr.m_userid + "长度:" + mds.m_data_len.ToString() + ",Data:" + IProtocal.bytes2Hex(tmpbytes));

                                gdrs.Add(gdr);
                                WriteReceiveLog(gdr);
                            }
                            else
                                log.Info("计讯数据转为统一接口:号码：" + gdr.m_userid + "长度:" + mds.m_data_len.ToString());
                        }
                    }

                    foreach (GPRS_DATA_RECORD recdPtr in gdrs)
                    {
                        int bsP_Id = slProtocalCodeBySimno[recdPtr.m_userid];
                        try
                        {
                            NeedParsePackets[bllCommNo2PacketListNo.GetPacketListNo(bsP_Id, recdPtr.m_userid)].Add(recdPtr);
                        }
                        catch
                        {
                            log.Error("NeedParsePackets 数据链不能增加数据：" + recdPtr.m_userid + "该包会被舍弃:");
                        }

                        //if (NeedParsePackets[bsP_Id].Count > 200)
                        //{

                        //    //把数据转移到另外一个结构中，然后单独在开辟线程解码
                        //    List<GPRS_DATA_RECORD> BackupParsePackets = new List<GPRS_DATA_RECORD>();
                        //    BackupParsePackets.AddRange(NeedParsePackets[bsP_Id]);
                        //    NeedParsePackets[bsP_Id].Clear();

                        //    log.Error("数据太多，开辟新线程解码：" + recdPtr.m_userid);
                        //    Thread thr = new Thread(new ParameterizedThreadStart(ProcessDataWithThreadSingle));
                        //    thr.Priority = ThreadPriority.Highest;
                        //    thr.Start(BackupParsePackets);


                        //    //log.Error("数据太多，舍弃1个：" + recdPtr.m_userid + "再去掉第0个:" + NeedParsePackets[bsP_Id][0].m_userid);
                        //    //NeedParsePackets[bsP_Id].RemoveAt(0);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("tmrJiXunDtuData_Tick:" + ex.Message);
            }
        }

        private void 启动服务ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }





     
        public string startServer(ushort comm_port)
        {
            bool flag = DTUService.StartService((ushort)comm_port);
            string ret = "计讯服务" + comm_port.ToString() + "已启动";
            if (flag)
            {
                log.Info("启动计讯服务(Port=" + comm_port.ToString() + ")");
            }
            else
            {
                log.Warn("启动计讯服务(Port=" + comm_port.ToString() + ")" + DTUService.LastError);
                ret += "计讯6012启动失败:" + DTUService.LastError;
            }
            return ret;
        }

        public string StopServer()
        {
            bool flag = DTUService.StopService();
            string ret = "停止计讯服务";
            if (flag)
            {
                log.Info("停止计讯服务");
            }
            else
            {
                log.Warn("停止计讯服务:" + DTUService.LastError);
                ret += ":" + DTUService.LastError;
            }
            return ret;
        }


        public List<DtuInfo> GetDtuLinkStatus()
        {
            List<DtuInfo> dtuTB = new List<DtuInfo>();

            try
            {
                //维护列表
                Dictionary<uint, ModemInfoStruct> dtuList;
                if (DTUService.GetDTUList(out dtuList))
                {
                    //this.Text = dtuList.Count.ToString();
                    //删除数据集中的无效记录
                    foreach (ModemInfoStruct mis in dtuList.Values)
                    {
                        DtuInfo di = new DtuInfo();
                        di.ID = mis.m_modemId;
                        di.PhoneNO = Util.Byte11ToPhoneNO(mis.m_phoneno, 0);
                        di.DynIP = Util.ByteArrayToHexString(mis.m_dynip);
                        di.ConnectTime = Util.ULongToDatetime(mis.m_conn_time);
                        di.LastActTime = Util.ULongToDatetime(mis.m_refresh_time);
                        di.Visible = true;
                        //Application.DoEvents();
                        if (!DTUService.Started)
                            return dtuTB; ;
                        if (!dtuTB.Contains(di))
                        {
                            dtuTB.Add(di);
                        }
                    }
                    return dtuTB;
                }
                else
                {
                    return dtuTB;
                }
            }
            catch (Exception ee)
            {
                log.Warn("读取计讯连接列表:" + ee.Message);
                return dtuTB;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtuID"></param>
        /// <param name="bts"></param>
        /// <returns>0:发送成功 -1：发送失败</returns>
        public int SendData(uint dtuID, byte[] bts)
        {
            if (DTUService.SendHex(dtuID, bts))
            {
                //log.Info("向用户 " + dtuID.ToString("X").PadLeft(8, '0') + " 发送数据  成功!"+Util.ByteArrayToHexString(bts));
                log.Info("向用户 " + dtuID.ToString() + " 发送数据  成功!" + Util.ByteArrayToHexString(bts));
                return 0;
            }
            else
            {
                log.Error("向用户 " + dtuID.ToString() + " 发送数据  失败!" + this.DTUService.LastError);
                //在获取列表
                List<DtuInfo> dis=GetDtuLinkStatus();
                string a = "";
                foreach (DtuInfo di in dis)
                {
                    a += ":" + di.ID.ToString();

                }
                return -1;
            }
        }

        public List<ModemDataStruct> GetDtuData()
        {
            log.Info("准备获取计讯数据（计讯模块）");
            List<ModemDataStruct> data = new List<ModemDataStruct>();
            try
            {
                //读取数据
                ModemDataStruct dat = new ModemDataStruct();

                log.Info("准备获取计讯数据（计讯模块）...");
                while (DTUService.GetNextData(out dat))
                {
                    log.Info("获取计讯数据（计讯模块）:" + dat.m_modemId.ToString("x"));

                    data.Add(dat);
                    if (!DTUService.Started) return data;
                }
                return data;
            }
            catch (Exception ee)
            {
                log.Error("读取计讯数据出错：", ee);
                return data;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] sendBts = Encoding.Default.GetBytes(this.test2.Text);
            uint idno=Convert.ToUInt32(this.test1.Text);

            int ret=this.SendData(idno, sendBts);

          
            MessageBox.Show(ret == -1 ? "Fail" : "Succ");
              
        }

        private void button3_Click_1(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //client.GetMessage();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            txtWcf.Text = "";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            #region test
            IsTest = true;
            启动服务ToolStripMenuItem.Checked = false;
            if (IsTest)
            {
                try
                {



                    //测试生成采集命令
                    string datatest;
                    GPRS_DATA_RECORD recdPtr = new GPRS_DATA_RECORD();
                    recdPtr.m_recv_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    recdPtr.m_userid = "15300010001";
                    datatest = "0103D6000000000000021000000000000000000005B154441E4C000000000043448D0043A77E0241EC80020000099D3F800D323F21B4E84325AFD0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000097B0";

                    
                    datatest = datatest.Replace(" ", "");
                    int count = datatest.Length;
                    byte[] bb = new byte[datatest.Length / 2];


                    recdPtr.m_data_buf = new byte[bb.Length];
                    recdPtr.m_data_len = (ushort)bb.Length;

                    for (int i = 0; i < bb.Length; i++)
                    {
                        bb[i] = Convert.ToByte(datatest.Substring(i * 2, 2), 16);
                    }
                    recdPtr.m_data_buf = bb;
                    int bsP_Id = slProtocalCodeBySimno[recdPtr.m_userid];
                    try
                    {
                        NeedParsePackets[bllCommNo2PacketListNo.GetPacketListNo(bsP_Id, recdPtr.m_userid)].Add(recdPtr);
                    }
                    catch
                    {
                        log.Error("NeedParsePackets 数据链不能增加数据：" + recdPtr.m_userid + "该包会被舍弃:");
                    }

                    string str = bytes2Hex(recdPtr.m_data_buf);

                    //HeatingDSC.BLL.ParseSheXianQxyUpData obj = new HeatingDSC.BLL.ParseSheXianQxyUpData();
                    //obj.Parse(recdPtr);

                    GetHandDownCmd();

                    ProcessData(recdPtr);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
                return;
            }
            #endregion
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }


    public class LockObject
    {
        public string Name;
        public DateTime dt = DateTime.Now;
        public int LockValue = 0;
    }
}
