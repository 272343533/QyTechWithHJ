using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TCPLibrary;
using TCPLibrary.Abstracts;
using TCPLibrary.DefaultImplements;

using DSC_FTHj1;
using TCPServer.Bll;
using SunMvcExpress.Dao;
using QyTech.Core;
using QyTech.Core.BLL;
using QyTech.Core.Common;

using SunMvcExpress.Core;
using SunMvcExpress.Core.BLL;

using log4net;
 using System.Net;
using System.Net.Sockets;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace TCPServer
{
    public partial class Form1 : Form
    {

        public static ILog log = log4net.LogManager.GetLogger("Form1");
        PackParseFac fac = new PackParseFac();
  

        BaseServerSocket _server;

        Dictionary<string, string> Ip2DtuNo = new Dictionary<string, string>();
        Dictionary<string, ZProxySocket> DtuNo2Socket = new Dictionary<string, ZProxySocket>();
        Dictionary<ZProxySocket,string> Socket2DtuNo = new Dictionary<ZProxySocket,string>();
        List<string> LinkedDtu=new List<string>();

        private Dictionary<string, ZdFT_Packet> needSendList = new Dictionary<string,ZdFT_Packet>();

        Dictionary<string, ZdFtDevice> dtudevs = new Dictionary<string, ZdFtDevice>();

        private static readonly object LockObjectCommNoSend = new object();
      

        public Form1()
        {
            InitializeComponent();
        }
        //通讯号与设备号注册时有：170911001------15995555673
        //设备号与ip port 对应起来

        List<ZProxySocket> lstProxySocket = new List<ZProxySocket>();
        private void Form1_Load(object sender, EventArgs e)
        {

            //ZdFT_Packet test = new Write130801();
            //byte[] bytes=test.Create("19.5,23");
            this.textBox4.Text=GetLocalIP();


            ZdFT_Packet pack = new ZdFT_Packet();
            //this.textBox4.Text =  System.Net.IPAddress.Any.ToString();
            List<DTUProduct> dtus = EntityManager<DTUProduct>.GetListNoPaging<DTUProduct>("dtuprotype='内网采集器设备'", "");
            foreach (DTUProduct dtu in dtus)
            {
                try
                {
                    ZdFtDevice dev = new ZdFtDevice();
                    dev.DtuDevId = dtu.DtuProNo;//dtuid
                    dev.BuildingDevId = ZdFT_Packet.Reverse(dtu.CommNo);//总表id

                    List<DetailDevice> details = EntityManager<DetailDevice>.GetListNoPaging<DetailDevice>("DTU_Id='" + dtu.Id + "'", "");
                    dev.RoomDevIds = new List<string>();
                    foreach (DetailDevice dd in details)
                    {
                        dev.RoomDevIds.Add(ZdFT_Packet.Reverse(dd.SubDevNo));
                    }
                    dtudevs.Add(dev.BuildingDevId,dev);
                }
                catch (Exception ex)
                {
                    log.Error("load device:" + ex.Message);
                }
            }

            this.comboBox1.Items.Clear();
            this.comboBox3.Items.Clear();
            this.cboHB.Items.Clear();
            if (dtudevs.Count > 0)
            {
                //this.comboBox1.Items.Add("01 00 11 09 17");
                foreach (ZdFtDevice zd in dtudevs.Values)
                {
                    this.comboBox3.Items.Add(zd.DtuDevId);
                    this.comboBox1.Items.Add(zd.BuildingDevId);

                }
                if (comboBox1.Items.Count>0)
                    comboBox1.SelectedIndex = 0;

                foreach (string str in dtudevs[this.comboBox1.Text].RoomDevIds)
                {
                    this.cboHB.Items.Add(ZdFT_Packet.Reverse(str));
                }
                if (cboHB.Items.Count>0)
                    cboHB.SelectedIndex = 0;
            }


            this.comboBox2.Items.Clear();
            ZdFT_Packet obj = new ZdFT_Packet();
            foreach (string flag in obj.ProductFlag2Desp.Keys)
            {
                this.comboBox2.Items.Add(flag + obj.ProductFlag2Desp[flag]);
            }

            #region cancel
            //this.comboBox2.Items.Add("0C4003"+"热表数据读取");//1
            //this.comboBox2.Items.Add("0C2003"+"阀门数据读取");//2
            //this.comboBox2.Items.Add("0C0404"+"测温器数据读取");//3 不使用

            //this.comboBox2.Items.Add("048001"+"采集器校时");//4

            //this.comboBox2.Items.Add("130200"+"普通开单个阀门");//5
            //this.comboBox2.Items.Add("130400"+"普通关单个阀门");//6
            //this.comboBox2.Items.Add("130800"+"强开单个阀门");//7
            //this.comboBox2.Items.Add("131000"+"强关单个阀门");//7_1
            //this.comboBox2.Items.Add("134000"+"单个阀门自由模式");//8

            //this.comboBox2.Items.Add("132000"+"远程设置温度");//9
            //this.comboBox2.Items.Add("130101"+"开启/关闭温控器热量显示");//10
            //this.comboBox2.Items.Add("138002"+"解锁/锁定温控面板设置温度");//11
            //this.comboBox2.Items.Add("130203"+"温度设置上下限");//12
            //this.comboBox2.Items.Add("134001"+"广播强开阀门");//13
            //this.comboBox2.Items.Add("130202"+"广播强关阀门");//14
            //this.comboBox2.Items.Add("130402"+"广播阀门自由模式");//15
            //this.comboBox2.Items.Add("138009"+"阀门进入远程控制模式");//16
            //this.comboBox2.Items.Add("13010A"+"广播阀门进入远程控制模式");//17
            //this.comboBox2.Items.Add("134006"+"广播远程设置温度");//18

            //this.comboBox2.Items.Add("130801"+"广播温度设置上下限");//19
            //this.comboBox2.Items.Add("132001"+"广播开启/关闭面板热量显示");//20
            //this.comboBox2.Items.Add("130103"+"广播解锁或锁定面板设置温度");//21
            //this.comboBox2.Items.Add("132008"+"阀门进入调试模式");//22
            //this.comboBox2.Items.Add("134008"+"广播阀门进入调试");//23
            //this.comboBox2.Items.Add("130808"+"设置阀门角度");//24
            //this.comboBox2.Items.Add("130109"+"广播设置阀门角度");//25
            //this.comboBox2.Items.Add("130209"+"设置调试模式控制逻辑");//26
            //this.comboBox2.Items.Add("130409"+"广播设置调试模式控制逻辑");//27
            //this.comboBox2.Items.Add("130809"+"设置调试模式设定温度");//28
            //this.comboBox2.Items.Add("131009"+"广播设置调试模式设定温度");//29
            //this.comboBox2.Items.Add("132009"+"设置调试模式控制逻辑及温度");//30
            //this.comboBox2.Items.Add("134009"+"广播设置调试模式控制逻辑及温度");//31

            //this.comboBox2.Items.Add("050402"+"读某批数据");//32
            //this.comboBox2.Items.Add("050201"+"读终端配置");//33
            //this.comboBox2.Items.Add("050202"+"读采集器属性配置");//34

            //this.comboBox2.Items.Add("011000"+"重启采集器");//35

            //this.comboBox2.Items.Add("040202"+"配置文件分割下发");//36
            //this.comboBox2.Items.Add("550100"+"透传传输调试命令");//38
            //this.comboBox2.Items.Add("000400"+"确认帧反馈");//37
            #endregion

            this.comboBox2.SelectedIndex = this.comboBox2.Items.Count-1;

            this.button3_Click(null, null);

            //System.Threading.Thread thrReStart = new System.Threading.Thread(ReStartApplication);
            //thrReStart.Start();

        }

    /// <summary>
    /// 获取本机IP地址
    /// </summary>
    /// <returns>本机IP地址</returns>
    public static string GetLocalIP()
    {
        try
        {
            string HostName = Dns.GetHostName(); //得到主机名
            IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
            for (int i = 0; i < IpEntry.AddressList.Length; i++)
            {
                //从IP地址列表中筛选出IPv4类型的IP地址
                //AddressFamily.InterNetwork表示此IP为IPv4,
                //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    return IpEntry.AddressList[i].ToString();
                }
            }
            return "";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }

    }
        /// <summary>
        /// 接受消息
        /// </summary>
        /// <param name="proxySocket"></param>
        /// <param name="message"></param>
        void _server_MessageReceived(ZProxySocket proxySocket, ZMessage message)
        {
            int errPos = 1000;
            log.Info(DateTime.Now.ToString("HH:mm:ss") + ":信息来了");
            this.Invoke((Action)(delegate ()
            {
            BaseMessage bmsg = message as BaseMessage;
            ZdFT_Packet msg = (message as BaseMessage).PackContent;

            log.Info(DateTime.Now.ToString("HH:mm:ss") + ":信息来了1");
            if (bmsg.MsgType == 1)//注册
            {
                string DtuNo = msg.StrRawData.Substring(0, 9);
                // IpToCommNo.Add(msg.StrRawData.Substring(9, 11), proxySocket.RemoteIP + ":" + proxySocket.RemotePort);
                Ip2DtuNo.Add(proxySocket.RemoteIP + ":" + proxySocket.RemotePort, DtuNo);// msg.StrRawData.Substring(0, 9));

                    textBox6.AppendText("注册:" + DateTime.Now.ToString("MM-dd HH:mm:ss")  + "---" + proxySocket.RemoteIP + ":" + proxySocket.RemotePort + "->"+msg.StrRawData.Substring(0, 9) + "---" + msg.StrRawData.Substring(9)+ "\r\n");
                    if (DtuNo2Socket.ContainsKey(DtuNo))
                    {
                        DtuNo2Socket.Remove(DtuNo);
                    }
                    DtuNo2Socket.Add(DtuNo, proxySocket);

                    Socket2DtuNo.Add(proxySocket, DtuNo);
                    if (!LinkedDtu.Contains(DtuNo))
                    {
                        LinkedDtu.Add(DtuNo);
                        nudMaxDtuCount.Value=LinkedDtu.Count;
                    }
                }
                else
                {
                    log.Info(DateTime.Now.ToString("HH:mm:ss") + ":信息来了2");
                    msg.DtuNo = Ip2DtuNo[proxySocket.RemoteIP + ":" + proxySocket.RemotePort];
                    if (this.textBox1.Text.Length > 5000)
                        this.textBox1.Text = "";
                    if (chkRecord.Checked)
                    {
                        if (comboBox3.Text == "" || comboBox3.Text == msg.DtuNo)
                        {
                            textBox1.AppendText(DateTime.Now.ToString("MM-dd HH:mm:ss") + " 机器：" + proxySocket.RemoteIP + ":" + proxySocket.RemotePort + " 收到数据：\r\n");
                            textBox1.AppendText(msg.DtuNo + ":   Flag:" + msg.CommFlag + "数据:" + msg.StrRawData + "\r\n");
                        }
                    }
                    PackParseFac fac = new PackParseFac();
                    ZdFT_Packet obj = fac.Create(msg.CommFlag);

                    log.Info(DateTime.Now.ToString("HH:mm:ss") + ":信息来了3----"+bmsg.MsgType.ToString());
                    if (bmsg.MsgType == 2)//数据包
                    {
                        //获取标志
                        obj.Parse(msg.Data);

                        //判断是否存在采集命令
                        try
                        {
                            if (obj.CommFlag == "0C2003")
                            {
                                errPos = 5000;
                                string key = msg.DtuNo + msg.CommFlag.ToString() + HeadTailExChangeWith2Char(msg.StrRawData.Substring(18, 10));//阀门号要倒叙
                            
                                log.Info("判断列表中是否包含该采集命令：" + key);

                                if (needSendList.ContainsKey(key))
                                {
                                    errPos = 6000;
                                    log.Info("结果：包含该命令：" + obj.DtuNo + obj.ToString());

                                    ZdFT_Packet tmp = needSendList[key];
                                    string wheresql = "Convert(varchar(23),SetDt,121)='" + tmp.SetDownDt.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' and CommNo='" + tmp.DtuNo + "' and SetFlag='" + tmp.CommFlag.ToString() + "'";
                                    HrzDownSet dbobj = EntityManager<HrzDownSet>.GetBySql<HrzDownSet>(wheresql);
                                    errPos = 7000;
                                    dbobj.DownSuccDt = DateTime.Now;
                                    dbobj.ValidResponse = obj.ErrorFlag;
                                    //并直接保存到数据中
                                    EntityManager<HrzDownSet>.Modify<HrzDownSet>(dbobj);
                                    errPos = 8000;
                                    try
                                    {
                                        needSendList.Remove(key);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error("receive packet should lock:" + ex.Message);
                                    }
                                    errPos = 9000;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(errPos.ToString()+"解码后判断是否需要处理数据库下发成功：" + ex.Message);
                        }
                    
                    }
                    else if (bmsg.MsgType == 10 || bmsg.MsgType==55) //确认包或者透明包
                    {
                        errPos = 4000;
                        //确认包
                        log.Info("确认包开始解码");
                        int ret = obj.Parse(msg.Data);
                        log.Info("确认包解码完成：" + ret.ToString());
                        if (chkRecord.Checked && (comboBox3.Text == "" || comboBox3.Text == msg.DtuNo))
                        {
                            textBox1.AppendText(obj.DtuNo + ":   Flag:" + obj.CommFlag + "数据:" + obj.StrRawData + "\r\n");
                        }
                        if (ret != 2) //广播不处理
                        {
                            //保存hrzdownset数据库
                            //if (ret > 130000 )//控制命令下发回包
                            {
                                try
                                {
                                    errPos = 5000;
                                    string key = msg.DtuNo + obj.CommFlag.ToString() + obj.strData;
                                    log.Info("判断列表中是否包含该命令：" + key);

                                    //if (needSendList.ContainsKey(obj.DtuNo + obj.CommFlag))
                                    if (needSendList.ContainsKey(key))
                                    {
                                        errPos = 6000;
                                        log.Info("结果：包含该命令：" + obj.DtuNo + obj.ToString());

                                        ZdFT_Packet tmp = needSendList[key];
                                        string wheresql = "Convert(varchar(23),SetDt,121)='" + tmp.SetDownDt.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' and CommNo='" + tmp.DtuNo + "' and SetFlag='" + tmp.CommFlag.ToString() + "'";
                                        HrzDownSet dbobj = EntityManager<HrzDownSet>.GetBySql<HrzDownSet>(wheresql);
                                        errPos = 7000;
                                        dbobj.DownSuccDt = DateTime.Now;
                                        dbobj.ValidResponse = obj.ErrorFlag;
                                        //并直接保存到数据中
                                        EntityManager<HrzDownSet>.Modify<HrzDownSet>(dbobj);
                                        //if (System.Threading.Monitor.TryEnter(LockObjectCommNoSend, 500))
                                        //{
                                        errPos = 8000;
                                        try
                                        {
                                            needSendList.Remove(key);
                                        }
                                        catch (Exception ex)
                                        {
                                            log.Error("receive packet should lock:" + ex.Message);
                                        }
                                        //    System.Threading.Monitor.Exit(LockObjectCommNoSend);
                                        //}
                                        errPos = 9000;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    log.Error("解码后判断是否需要处理数据库下发成功：" + ex.Message);
                                }
                            }
                        }
                    }
                }

            }));
        }

        private string HeadTailExChangeWith2Char(string str)
        {
            string strRet = "";
            if (str.Length % 2 == 0)
            {
                for (int i = 0; i < str.Length/2; i++)
                {
                    strRet = str.Substring(i * 2, 2) + strRet;
                }
                return strRet;
            }
            else
                return str;
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="proxySocket"></param>
        void _server_DisConnected(ZProxySocket proxySocket)
        {
            try
            {
                this.Invoke((Action)(delegate()
                {
                    textBox6.AppendText("断开:" + DateTime.Now.ToString("MM-dd HH:mm:ss") + "---" + proxySocket.RemoteIP + ":" + proxySocket.RemotePort + "\r\n");
                    if (lstProxySocket.Contains(proxySocket))
                    {
                        lstProxySocket.Remove(proxySocket);
                        log.Error("list.Remove(proxySocket);");
                    }

                    if (DtuNo2Socket.ContainsKey(Socket2DtuNo[proxySocket]))
                    {
                        DtuNo2Socket.Remove(Socket2DtuNo[proxySocket]);
                        log.Error("DtuNo2Socket.Remove(Socket2DtuNo[proxySocket]);");
                    }

                    if   (Socket2DtuNo.ContainsKey(proxySocket)){
                        Socket2DtuNo.Remove(proxySocket);
                        log.Error(" Socket2DtuNo.Remove(proxySocket);");
                    }
                    textBox6.AppendText("断开:" + DateTime.Now.ToString("MM-dd HH:mm:ss") + "---" + proxySocket.RemoteIP + ":" + proxySocket.RemotePort + "Recorded \r\n");
                }));
            }
            catch (Exception ex)
            {
                log.Error("_server_DisConnected" + ex.Message);
            }
        }
        /// <summary>
        /// 建立连接
        /// </summary>
        /// <param name="proxySocket"></param>
        void _server_Connected(ZProxySocket proxySocket)
        {
            this.Invoke((Action)(delegate()
            {
                lstProxySocket.Add(proxySocket);
                textBox6.AppendText("连接:" + DateTime.Now.ToString("MM-dd HH:mm:ss") + "---" + proxySocket.RemoteIP + ":" + proxySocket.RemotePort + "\r\n");
            }));
        }
       
        /// <summary>
        /// 发送图片（可序列化对象）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //using (OpenFileDialog ofd = new OpenFileDialog())
            //{
            //    ofd.Filter = "图片文件|*.jpg;*.jpeg";
            //    if (ofd.ShowDialog() == DialogResult.OK)
            //    {
            //        BinaryFormatter bf = new BinaryFormatter();
            //        using (MemoryStream ms = new MemoryStream())
            //        {
            //            bf.Serialize(ms, Image.FromFile(ofd.FileName));
            //            foreach (ZProxySocket proxy in list)
            //            {
            //                proxy.SendMessage(new BaseMessage(2, ms.ToArray()));  //发送BaseMessage消息
            //            }
            //        }
            //    }
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] address = this.textBox4.Text.Replace(" ","").Split(new char[] { '.',',' });
            if (address.Length!=4)
            {
                MessageBox.Show("请核对ip地址！");
                return;
            }
            int port = Convert.ToInt32(this.textBox3.Text);

            byte[] byteaddr = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                byteaddr[i] = Convert.ToByte(address[i]);
            }
             
            _server = new BaseServerSocket();
            _server.Connected += new ConnectedEventHandler(_server_Connected);
            _server.DisConnected += new DisConnectedEventHandler(_server_DisConnected);
            _server.MessageReceived += new MessageReceivedEventHandler(_server_MessageReceived);


            _server.StartAccept(byteaddr, port);

            textBox6.AppendText(DateTime.Now.ToString("MM-dd HH:mm:ss")+"---"+this.textBox4.Text + "服务器启动，监听端口 " + port.ToString() + "...\r\n");

            button3.Enabled = false;
        }

        private void tmrSendData_Tick(object sender, EventArgs e)
        {
            log.Info("tmr start");
            string smess = this.textBox2.Text.Replace(" ", "");

            string[] cmds = fac.GetNeedNormalReadClassMethodName();

            log.Info("tmr 1-------------cmd count" + cmds.Length.ToString());
            #region 测试使用（从控件中获取）
            //for (int i = 0; i < cmds.Length && i != 2; i++)
            //{
            //    string[] needsend = cmds[i].Split(new char[] { '.' });

            //    log.Info("tmr 1-------------cmd count" + cmds.Length.ToString());
            //    ZdFT_Packet obj = fac.Create(needsend[0].Substring(4));
            //    if (needsend[0].Substring(4) == "0C4003")
            //        foreach (string zhb in this.comboBox1.Items)
            //        {
            //            obj.Create(zhb.Replace(" ", ""));
            //            SendCmd(obj);
            //        }
            //    else if (needsend[0].Substring(4) == "0C2003")
            //        foreach (string zib in this.cboHB.Items)
            //        {
            //            obj.Create(zib.Replace(" ", ""));
            //            SendCmd(obj);
            //        }
            //}
            #endregion

            #region 从数据库获取
            foreach (ZdFtDevice zd in dtudevs.Values)
            {
                if (!DtuNo2Socket.ContainsKey(zd.DtuDevId))
                    continue;

                for (int i = 0; i < cmds.Length && i != 2; i++)
                {
                    string[] needsend = cmds[i].Split(new char[] { '.' });

                    log.Info("tmr 1-------------cmd count" + cmds.Length.ToString());
                    ZdFT_Packet obj = fac.Create(needsend[0].Substring(4));
                    if (needsend[0].Substring(4) == "0C4003")
                    {
                        obj.Create(zd.DtuDevId.Replace(" ", ""));
                        SendCmd(DtuNo2Socket[zd.DtuDevId], obj);
                    }
                    else if (needsend[0].Substring(4) == "0C2003")
                    {
                        foreach (string zib in zd.RoomDevIds)
                        {
                            obj.Create(zib.Replace(" ", ""));
                            SendCmd(DtuNo2Socket[zd.DtuDevId], obj);
                        }
                    }
                }
            }
            #endregion

            log.Info("tmr end");
        }

        private void SendOneDtuReadCommand(object dtudevice)
        {
            ZdFtDevice dtu = (ZdFtDevice)dtudevice;


        }

        private void SendCmd(ZdFT_Packet sendpack)
        {
            try
            {
                foreach (ZProxySocket proxy in lstProxySocket)
                {
                    proxy.SendMessage(new BaseMessage(2, sendpack));
                    log.Info("发送：" + sendpack.StrRawData);
                }
            }
            catch (Exception ex)
            {
                log.Error("发送失败：" + ex.Message + "!(" + sendpack.CommFlag + "-" + sendpack.StrRawData + ")");
            }
        }

        private void SendCmd(ZProxySocket proxy, ZdFT_Packet sendpack)
        {
            try
            {
                proxy.SendMessage(new BaseMessage(2, sendpack));
                if (chkRecord.Checked && (comboBox3.Text == "" || comboBox3.Text == sendpack.DtuNo))
                {
                    textBox1.AppendText(sendpack.DtuNo + ":发送数据:" +sendpack.CommFlag +"---"+ sendpack.StrRawData + "\r\n");
                }
                log.Info("发送：" + sendpack.StrRawData);

            }
            catch (Exception ex)
            {
                log.Error("发送失败：" + ex.Message + "!(" + sendpack.CommFlag + "-" + sendpack.StrRawData + ")");
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBox2.Text = comboBox1.Text;

            cboHB.Items.Clear();
            cboHB.Text = "";
            foreach (string str in dtudevs[this.comboBox1.Text].RoomDevIds)
            {
                this.cboHB.Items.Add(ZdFT_Packet.Reverse(str));
            }
            if (cboHB.Items.Count>0)
                cboHB.SelectedIndex = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string smess = this.textBox2.Text.Replace(" ", "");

            string flag = this.comboBox2.Text.Substring(0,6);

            ZdFT_Packet obj = fac.Create(flag);

            obj.Create(smess);
            if (chkSendAllSocket.Checked)
            {
                SendCmd(obj);
            }
            else
            {
                if (DtuNo2Socket.ContainsKey(comboBox3.Text))
                {
                    SendCmd(DtuNo2Socket[comboBox3.Text], obj);//needsendcmd[Convert.ToInt16(flag) - 31]);
                    //SendCmd(obj);
                }
                else
                {
                    MessageBox.Show("还没有上线，不能发送");
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            tmrReadData.Enabled = checkBox1.Checked;
            if (tmrReadData.Enabled)
                tmrSendData_Tick(null, null);
        }

        private void tmrWriteData_Tick(object sender, EventArgs e)
        {
            int errPos = 0;
            try
            {
                for (int i=needSendList.Keys.Count-1;i>=0;i--)
                {
                    string key=needSendList.Keys.ElementAt(i);
                    try
                    {
                        ZdFT_Packet pack = needSendList[key];
                        //已经发过的命令超过一分钟才会再发
                        if (pack.SetDownDt.AddMinutes(5) < DateTime.Now)
                        {
                           // string key = pack.DtuNo.Trim() + pack.CommFlag;
                            //if (pack.strData.Trim().Length >= 10)
                            //    key += pack.strData.Substring(0, 10).Trim();
                            needSendList.Remove(key);
                          
                            log.Info("列表中清除命令:" + key);
                        }
                    }
                    catch(Exception ex)
                    {
                        log.Error("过期命令删除失败:"+ex.Message);
                    }
                }
                List<vwlyHrzDownSet> sends = EntityManager<vwlyHrzDownSet>.GetListNoPaging<vwlyHrzDownSet>("bsp_Id=100000", "setdt");
                PackParseFac fac = new PackParseFac();
                log.Info("tmrGetNeedSend_Tick:" + sends.Count.ToString());

                if (sends.Count > 0)
                {
                    log.Error("队列长度：" + needSendList.Count.ToString() + ",要新增：" + sends.Count.ToString());
                    foreach (vwlyHrzDownSet obj in sends)
                    {
                        try
                        {
                            //string key = obj.SetDt.ToString("yyyy-MM-dd HH:mm:ss") + obj.CommNo.Trim() + obj.SetFlag ;
                            string key = obj.CommNo.Trim() + obj.SetFlag;
                            if (obj.SetValues.Trim().Length >= 10)
                                key += obj.SetValues.Substring(0, 10).Trim();
                            log.Info("要新增:" + key + "-----" + obj.SetDt.ToString("yyyy-MM-dd"));
                            errPos = 1000;
                            if (needSendList.ContainsKey(key))
                            {
                                log.Warn("已包含:" + key + "-----" + needSendList[key].SetDownDt.ToString("yyy-MM-dd HH:mm:ss"));
                            }
                            if (!needSendList.ContainsKey(key))
                            {
                                errPos = 2000;
                                ZdFT_Packet pack = fac.Create(obj.SetFlag);
                                pack.SetDownDt = obj.SetDt;
                                pack.DtuNo = obj.CommNo.Trim();
                                pack.Create(obj.SetValues.Trim());
                                //if (System.Threading.Monitor.TryEnter(LockObjectCommNoSend, 500))
                                //{
                                errPos = 3000;
                                try
                                {
                                    needSendList.Add(key, pack);
                                }
                                catch (Exception ex)
                                {
                                    log.Warn("add to list should lock:新增出错:" + key + "    " + pack.SetDownDt.ToString("yyy-MM-dd HH:mm:ss") + ":" + ex.Message);
                                }
                                //    System.Threading.Monitor.Exit(LockObjectCommNoSend);
                                //}
                                errPos = 4000;
                                log.Debug("增加列表" + key);
                                try
                                {
                                    string wheresql = "Convert(varchar(30),SetDt,121)='" + obj.SetDt.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'and CommNo='" + obj.CommNo + "' and SetFlag='" + obj.SetFlag + "'";
                                    HrzDownSet dbobj = EntityManager<HrzDownSet>.GetBySql<HrzDownSet>(wheresql);

                                    errPos = 5000;
                                    dbobj.DownDt = DateTime.Now;
                                    dbobj.DownCmd = pack.StrRawData;
                                    //并直接保存到数据中
                                    EntityManager<HrzDownSet>.Modify<HrzDownSet>(dbobj);
                                    errPos = 6000;
                                }
                                catch (Exception ex) { log.Error("更新数据库出错:     " + ex.Message); }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("处理命令出错:     " + ex.Message+"时间:"+obj.SetDt.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        }
                      
                    }
                    log.Info("新增后队列长度：" + needSendList.Count.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error("trnWruteData1:"+errPos.ToString()+"--"+ex.Message, ex);
            }

            //发送
            if (needSendList.Count > 0)
            {

                foreach (ZdFT_Packet pack in needSendList.Values)
                {
                    try
                    {
                        //已经发过的命令超过一分钟才会再发
                        if (pack.TryDownDt.AddMinutes(1) < DateTime.Now)
                        {
                            //ZdFT_Packet pack = needSendList.Values.FirstOrDefault<ZdFT_Packet>();
                            if (DtuNo2Socket.ContainsKey(pack.DtuNo))
                            {
                                log.Info("tmrWriteData_Tick send trigger:" + pack.DtuNo);
                                SendCmd(DtuNo2Socket[pack.DtuNo], pack);
                                
                              
                                pack.TryDownDt = DateTime.Now;

                                string wheresql = "Convert(varchar(30),SetDt,120)='" + pack.SetDownDt.ToString("yyyy-MM-dd HH:mm:ss") + "'and CommNo='" + pack.DtuNo + "' and SetFlag='" + pack.CommFlag + "'";
                                HrzDownSet dbobj = EntityManager<HrzDownSet>.GetBySql<HrzDownSet>(wheresql);

                                dbobj.AutoSetDt = DateTime.Now;

                                //并直接保存到数据中
                                EntityManager<HrzDownSet>.Modify<HrzDownSet>(dbobj);

                                //Application.DoEvents();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("trnWruteData2:" + ex.Message, ex);
                    }
                }
            }
        }

        

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            tmrRestart.Enabled = checkBox2.Checked;
            tmrWriteData.Enabled = checkBox2.Checked;
        }

        //测试按钮
        private void btnText_Click(object sender, EventArgs e)
        {
            ZdFT_Packet msg = new ZdFT_Packet();
            msg.CommFlag = "0C2003";
            msg.strData = "0B01000013000811170105091700EC5180410AD77F41000080416666DE416666DE41122F0000780000006400000000000000000000000000000000000000000000000000000042060000000080BF00";
            msg.strData = msg.strData.Replace(" ", "");
            int count = msg.strData.Length;
            byte[] bb = new byte[msg.strData.Length / 2];
            for (int i = 0; i < bb.Length; i++)
            {
                bb[i] = Convert.ToByte(msg.strData.Substring(i * 2, 2), 16);
            }
            msg.Data=bb;
            PackParseFac fac = new PackParseFac();
            ZdFT_Packet obj = fac.Create(msg.CommFlag);
            obj.Parse( msg.Data);
        }

      
        private bool m_Terminated = false;
        private int ReStartPerMinutes = 60;

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
                    System.Threading.Thread.Sleep(5000);
                    log.Info("Restart Applcation");
                }
                AddMinutes = AddMinutes > 1000 ? 0 : AddMinutes;
                for (int i = 0; i < 30; i++)
                {
                    System.Threading.Thread.Sleep(2 * 1000);
                    if (m_Terminated)
                        break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtCurrSocket.Text = "";

            foreach (string key in DtuNo2Socket.Keys)
            {
                try
                {
                    txtCurrSocket.AppendText(key + ":" + DtuNo2Socket[key].RemoteIP + ":" + DtuNo2Socket[key].RemotePort + "\r\n");
                }
                catch (Exception ex)
                {
                    txtCurrSocket.AppendText(":" + DtuNo2Socket[key].RemoteIP + ":" + DtuNo2Socket[key].RemotePort + "\r\n");
                }
            }

            txtCurrSocket.AppendText("lst:\r\n");
            foreach (ZProxySocket item in lstProxySocket)
            {
                try
                {
                    txtCurrSocket.AppendText(Socket2DtuNo[item] + ":" + item.RemoteIP + ":" + item.RemotePort + "\r\n");
                }
                catch(Exception ex)
                {
                    txtCurrSocket.AppendText(":" + item.RemoteIP + ":" + item.RemotePort + "\r\n");
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Sure?","Just You!",MessageBoxButtons.YesNo))
            {
                _server = null;
                System.Threading.Thread.Sleep(5000);
                Application.Restart();
            }
        }

        private static int FullSocketsCount = 5;
        private static DateTime LessFullSocketsDt=DateTime.MaxValue;
        private static bool FirstHappened = false;
        private void tmrRestart_Tick(object sender, EventArgs e)
        {
            if (!chkAutoDetDtu.Checked)
                return;

            FullSocketsCount=Convert.ToInt16(this.nudMaxDtuCount.Value);

            if (lstProxySocket.Count < FullSocketsCount || DtuNo2Socket.Count < FullSocketsCount)
            {
                if (DateTime.Now.AddMinutes(-5) > LessFullSocketsDt)
                {
                    string disp=DateTime.Now.ToString("MM-dd HH:mm:ss") + "----：sockets:" + lstProxySocket.Count.ToString() + "---register:" + DtuNo2Socket.Count.ToString() + "\r\n";
                    log.Info("断开连接，不能重新连接，重新初始化："+disp);
                    txtCurrSocket.Text += disp;
                    _server = null;
                    System.Threading.Thread.Sleep(5000);
                    Application.Restart();

                }
                else
                {
                    if(!FirstHappened )
                    {
                        LessFullSocketsDt = DateTime.Now;
                        txtCurrSocket.Text += DateTime.Now.ToString("MM-dd HH:mm:ss") + "----sockets:" + lstProxySocket.Count.ToString()+"---register:"+DtuNo2Socket.Count.ToString()+"\r\n";
                        FirstHappened=true;
                    }
                }

            }
            else
            {
                if (FirstHappened)
                {
                    LessFullSocketsDt = DateTime.MaxValue;
                    FirstHappened = false;
                    txtCurrSocket.Text += DateTime.Now.ToString("MM-dd HH:mm:ss") + "----sockets:" + lstProxySocket.Count.ToString() + "---register:" + DtuNo2Socket.Count.ToString() + "\r\n";
                }
                      
            }

        }

        private void nudMaxDtuCount_ValueChanged(object sender, EventArgs e)
        {
            FirstHappened = false;

        }
    }
}
