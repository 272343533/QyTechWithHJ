using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HeatingDSC.Models;
using HeatingDSC.BLL;
using HeatingDSC.UI;
using HeatingDSC.DLL;

using HeatingDSC.TEST;



namespace HeatingDSC
{
    public partial class frmMain : Form
    {
  
        struct StationCmd
        {
            public string stationid;
            public string command;
        }

        //定义窗口消息，用来响应DTU的消息
        public const int WM_DTU = 0x400 + 100;

        //所有站点的链表
        IList<Stations> listAllstations;
        IList<StationCmd> listCommand = new List<StationCmd>();
       
        int sendComandTimerInterval=0;

        public frmMain()
        {
            InitializeComponent();
        }
       //折叠
        private void 通讯详情ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            通讯详情ToolStripMenuItem.Checked = !通讯详情ToolStripMenuItem.Checked;
            splitContainer3.Panel2Collapsed = !通讯详情ToolStripMenuItem.Checked;
        }

        //加载窗口
        private void Form1_Load(object sender, EventArgs e)
        {
         
            this.Text = Application.ProductName;//主窗口标题
            
            ProgParams.appPath = System.IO.Directory.GetCurrentDirectory();//获取当前目录
            ProgParams.readParamsFromXMLFile();//读取程序参数

            //折叠splitContainer3的panel2---通讯详情窗口
            //splitContainer3.Panel2Collapsed = true;

            //初始化站点树
            loadStaions2TreeView();
            //初始化ListView
            InitListView();

            //启动GPRS服务
            StartGPRSServer();

            //构成指令链表
            buildCommands2Stations();
        }

        //折叠登陆详情
        private void 登陆详情ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            登陆详情ToolStripMenuItem.Checked = !登陆详情ToolStripMenuItem.Checked;
            splitContainer2.Panel2Collapsed = !登陆详情ToolStripMenuItem.Checked;
        }

        //折叠站点树状图
        private void 站点树状图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            站点树状图ToolStripMenuItem.Checked = !站点树状图ToolStripMenuItem.Checked;
            splitContainer1.Panel1Collapsed = !站点树状图ToolStripMenuItem.Checked;
        }

        //打开程序参数设置界面
        private void 程序参数设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProgParamSet frmPPSet = new frmProgParamSet();
            frmPPSet.ShowDialog();
        }
        
        private void loadStaions2TreeView()
        {
            //所有站点的链表
            //IList<Stations> listAllstations;// Getallstations()

            listAllstations=StationManager.Getallstations();
            if (listAllstations == null)
                return;
            
            
            tvStations.LabelEdit=true;
            TreeNode root=new TreeNode();
            root.Text="燃气计量站点";
            root.ImageIndex = 0;
            tvStations.Nodes.Add(root);

            //string stDetail;
            //遍历所有站点
            foreach (Stations st in listAllstations)
            {
                //stDetail = st.StationName + "(" + st.CommID + ")";
                TreeNode node = new TreeNode();
                node.Text = st.StationName + "(" + st.CommID + ")";
                node.ImageIndex = 1;
                root.Nodes.Add(node);
                
                //root.Nodes.Add(stDetail);
            }
             
            tvStations.ExpandAll();
                       
        }


        //站点数据测试
        private void 所有站点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAllStations frmAllSt = new frmAllStations();
            frmAllSt.ShowDialog();
        }


        //启动GPRS服务
        private void StartGPRSServer()
        {
            StringBuilder mess=new StringBuilder(100);
            Gprs.StartGprsServer(this.Handle,ProgParams.GprsComPort,WM_DTU,ref mess);
            //txtComDetail.Text += mess.ToString();
            statusbarPanel1.Text = mess.ToString();
        }


        private void 启动服务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartGPRSServer();
        }

        private void 停止服务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder mess=new StringBuilder(50);
            Gprs.stop_net_service(mess);
            statusbarPanel1.Text = mess.ToString();
        }



        private void InitListView()
        {
           // ListViewItem itemNode=new ListViewItem();
            System.Windows.Forms.ColumnHeader header;
            
            header = new ColumnHeader();
            header.Text = "GPRS_ID";
            header.Width=120;
            this.listView1.Columns.Add(header);

            header = new ColumnHeader();
            header.Text = "注册时间";
            header.Width=300;
            this.listView1.Columns.Add(header);

        }

        //消息处理函数
        protected override void WndProc(ref Message m)
        {
            // TODO:  添加 Form1.WndProc 实现
            int i;

            //响应DTU消息，已经定义WM_DTU为DTU消息
            if (m.Msg == WM_DTU)
            {
                GPRS_DATA_RECORD recdPtr = new GPRS_DATA_RECORD();
                StringBuilder mess = new StringBuilder(100);

                //读取DTU数据
                //底层服务收到DTU发送的数据后，向启动服务的窗口发送消息（此处即为Form1）,本
                //函数即为该消息的响应，功能为读出并处理数据。
                //参数1：recdPtr--存放DTU信息的数据结构
                //参数2：mess---存放度数据过程中的信息，不需要时可设为Null
                //参数3：bAnswer--是否对收到的数据包进行应答，调试时可设为true;发布时要设为false!
                Gprs.do_read_proc(ref recdPtr,mess,false);
                //取出数据的Type
                byte a = recdPtr.m_data_type;

                switch (recdPtr.m_data_type)
                {
                    case 0x01:	//注册包，将新注册的站点加入到列表中											
                        GPRS_USER_INFO user_info = new GPRS_USER_INFO();
                        ushort usPort;
                        if (Gprs.get_user_info(recdPtr.m_userid, ref user_info) == 0)//读取成功
                        {
                            //检查是否注册过
                            for (i = 0; i < listView1.Items.Count; i++)
                              if (listView1.Items[i].Text == user_info.m_userid)
                                {   //已注册，仅修改登录数据，不增加行
                                    listView1.Items[i].SubItems.Add(Gprs.inet_ntoa(Gprs.ntohl(user_info.m_local_addr)));

                                    usPort = user_info.m_local_port;
                                    listView1.Items[i].SubItems.Add(usPort.ToString());

                                    listView1.Items[i].SubItems.Add(user_info.m_logon_date);
                                    listView1.Items[i].SubItems.Add(Gprs.inet_ntoa(Gprs.ntohl(user_info.m_sin_addr)));

                                    usPort = user_info.m_sin_port;
                                    listView1.Items[i].SubItems.Add(usPort.ToString());
                                    return;
                                }
                            //未注册
                            ListViewItem lvi = listView1.Items.Add(user_info.m_userid);             //增加一行，内容是userid
                            lvi.SubItems.Add(Gprs.inet_ntoa(Gprs.ntohl(user_info.m_local_addr)));   //同行加一列，地址

                            usPort = user_info.m_local_port;                                        //端口
                            lvi.SubItems.Add(usPort.ToString());

                            lvi.SubItems.Add(user_info.m_logon_date);                               //注册时间
                            lvi.SubItems.Add(Gprs.inet_ntoa(Gprs.ntohl(user_info.m_sin_addr)));     //地址

                            usPort = user_info.m_sin_port;
                            lvi.SubItems.Add(usPort.ToString());                                    //端口
                        }
                        break;
                    case 0x02:	//注销包											
                        for (i = 0; i < listView1.Items.Count; i++)
                            if (listView1.Items[i].Text == recdPtr.m_userid)
                            {
                                listView1.Items[i].Remove();
                                break;
                            }
                        break;
                    case 0x04:	//无效包
                        break;
                    case 0x09:	//DTU向DSC发送的用户数据包,进入解码程序
                                int iRet = ProcessData(recdPtr);
                                if (iRet == 0)
                                    SaveDatatoDatabase();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                //缺省消息处理
                base.WndProc(ref m);
            }
        }


        /// <summary>
        /// 解码数据
        /// </summary>
        /// <param name="recdPtr">从DTU传回的数据结构</param>
        /// GPRS_DATA_RECORD详情：
        ///typedef struct _USER_DATA_RECORD
        ///{
        ///char		m_userid[12];					//DTU身份识别码
        ///char		m_recv_date[20];				//接收到数据包的时间
        ///char		m_data_buf[MAX_RECEIVE_BUF];	//存储接收到的数据
        ///uint16		m_data_len;					//接收到的数据包长度
        ///uint8		m_data_type;				//接收到的数据包类型
        ///}data_record;

        ///接收到的数据包类型(m_data_type):
        ///1）0x01 DTU请求注册包
        ///2）0x02 DTU请求注销包
        ///3）0x03 忽略（当DTU尚未注册成功而向DSC发送数据时）
        ///3）0x04 无效的数据包
        ///4）0x05 DTU已经接收到DSC发送的用户数据包
        ///5）0x09 DTU向DSC发送的用户数据包
        ///6）0x0d DTU参数设置成功应答包
        ///7）0x0b DTU参数查询应答包
        ///8）0x06 请求断开DTU PPP连接成功应答包
        ///9）0x07 请求DTU停止向DSC发送数据成功应答包
        ///10）0x08 请求DTU开始向DSC发送数据成功应答包
        ///11）0x0A 请求DTU清除缓冲区数据成功应答包
 
        /// <returns>成功返回零，否则返回-1</returns>
        /// 
        private int ProcessData(GPRS_DATA_RECORD recdPtr)
        {
            int resualt = 0;

            string StationID = recdPtr.m_userid;        //DTU身份识别码
            string ReCeivDate = recdPtr.m_recv_date;    //数据接收时间
            
                //Encoding.Default.GetString();
            


            return resualt;
        }


        /// <summary>
        /// 保存数据到数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveDatatoDatabase()
        {
            //需要判断是那个数据，如果有报警，还要写报警数据
        }


        private void listViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestListViewcs frmTest = new TestListViewcs();
            frmTest.ShowDialog();
        }


        /// <summary>
        /// 间隔35秒查询所有站点数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void timer1_Tick(object sender, EventArgs e)
        {
            sendComandTimerInterval += 1;

            if ((sendComandTimerInterval % 5) == 0)
                buildCommands2Stations();

            ///遍历指令链表,发送指令
            foreach (StationCmd st in listCommand)
            {
                StringBuilder mess = new StringBuilder(100);
            
                Gprs.do_send_user_data("a", st.command, st.command.Length, mess);
                //txtComDetail.AppendText(st.stationid + "_" + st.command + Environment.NewLine);
            }


            toolStripStatusLabel2.Text = sendComandTimerInterval.ToString();
            
            
            if (sendComandTimerInterval >= 100)
                sendComandTimerInterval = 0;

        }


        /// <summary>
        /// 构建发往站点的命令链表
        /// </summary>
        private void buildCommands2Stations()
        {
            listAllstations = StationManager.Getallstations();

            listCommand.Clear();
            
            string curStationID;
            StationCmd Command2Send;
            byte[] commdarr=new byte[9];

            foreach (Stations st in listAllstations)
            {
                Command2Send = new StationCmd();
                curStationID = st.CommID;
                Command2Send.stationid = curStationID;

           
                Command.Addr = 0x01;
                Command.Protocol_DM = Convert.ToInt16(st.Protocol_DM);
                commdarr = Command.CommandCode(); 
   
                //生成命令数组
                if (commdarr != null)
                {
                    StringBuilder strbuilder = new StringBuilder();
                    foreach (byte cmdbyte in commdarr)
                    {
                       strbuilder.Append(cmdbyte.ToString("X2"));
                    }
                    Command2Send.command = strbuilder.ToString();
                }
                listCommand.Add(Command2Send);
           }
            
        }


        /// <summary>
        /// 指令生成菜单动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 指令生成ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buildCommands2Stations();
            foreach (StationCmd st in listCommand)
            {
                txtComDetail.AppendText(st.stationid +"_"+st.command+Environment.NewLine);
            }
       }

        private void tvStations_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent == null)
                e.Node.SelectedImageIndex = 0;
            else
                e.Node.SelectedImageIndex = 2;

        }

        private void 信息管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 数据操作ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
            
  
    }
}
