using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.InteropServices;

namespace QyTech.HDGprs
{
    //数据结构，GPRS数据记录
    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)] 
	public struct GPRS_DATA_RECORD
	{		
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
		public string	m_userid;		
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
		public string	m_recv_date;                            //收到数据的时间
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]	//这里做了修改，转换时由ByValTStr变为ByValArray类型，
		public byte[]	m_data_buf;					            //定位由string改为byte[]
		public ushort	m_data_len;                             //数据长度
		public byte     m_data_type;                            //数据类型    
		public void Initialize()					            //初始化byte[]的字段
		{			
			m_data_buf = new byte[1024];
		}
		
	}

    //数据结构，GPRS用户信息
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct GPRS_USER_INFO
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string m_userid;
        public uint m_sin_addr;
        public ushort m_sin_port;
        public uint m_local_addr;
        public ushort m_local_port;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string m_logon_date;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string m_update_time;
        public byte m_status;
    }


    public class Gprs
    {
        //定义接口函数		
		[DllImport(".\\wcomm_dll.dll")]
		public static extern int start_gprs_server(
							IntPtr hWnd,
							int wMsg,
							int nServerPort,
							[MarshalAs(UnmanagedType.LPStr)]
							StringBuilder mess);
		[DllImport(".\\wcomm_dll.dll")]
		public static extern int start_net_service(
			IntPtr hWnd,
			int wMsg,
			int nServerPort,
			[MarshalAs(UnmanagedType.LPStr)]
			StringBuilder mess);

		[DllImport(".\\wcomm_dll.dll")]
		public static extern int stop_gprs_server(
							[MarshalAs(UnmanagedType.LPStr)]
							StringBuilder mess);
		[DllImport(".\\wcomm_dll.dll")]
		public static extern int stop_net_service(
			[MarshalAs(UnmanagedType.LPStr)]
			StringBuilder mess);

        [DllImport(".\\wcomm_dll.dll")]
		public static extern int get_max_user_amount();


        [DllImport(".\\wcomm_dll.dll")]
        public static extern int get_online_user_amount();
        
        [DllImport(".\\wcomm_dll.dll")]
        public static extern int get_user_at(int index,ref GPRS_USER_INFO ui);

        [DllImport(".\\wcomm_dll.dll")]
        public static extern int do_close_one_user(
                            [MarshalAs(UnmanagedType.LPStr)]
							string userid,
                            [MarshalAs(UnmanagedType.LPStr)]
							StringBuilder mess);
        [DllImport(".\\wcomm_dll.dll")]
        public static extern int do_close_all_user2(
                            [MarshalAs(UnmanagedType.LPStr)]
							StringBuilder mess);

        [DllImport(".\\wcomm_dll.dll")]
        public static extern int do_close_one_user2(
                            [MarshalAs(UnmanagedType.LPStr)]
							string userid,
                            [MarshalAs(UnmanagedType.LPStr)]
							StringBuilder mess);


		[DllImport(".\\wcomm_dll.dll")]
		public static extern int do_read_proc(
							ref GPRS_DATA_RECORD recdPtr,
							[MarshalAs(UnmanagedType.LPStr)]
							StringBuilder mess,
							bool reply);		

		[DllImport(".\\wcomm_dll.dll")]
		public static extern int do_send_user_data(
							[MarshalAs(UnmanagedType.LPStr)]
							string userid,											
							[MarshalAs(UnmanagedType.LPStr)]
							string data,																		
							int len,
							[MarshalAs(UnmanagedType.LPStr)]
							StringBuilder mess);
        //byte[]
        [DllImport(".\\wcomm_dll.dll")]
        public static extern int do_send_user_data(
                            [MarshalAs(UnmanagedType.LPStr)]
							string userid,
                            
							byte[] data,
                            int len,
                            [MarshalAs(UnmanagedType.LPStr)]
							StringBuilder mess);
        ///DTU数据结构
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

		[DllImport(".\\wcomm_dll.dll")]
		public static extern int get_user_info(
							[MarshalAs(UnmanagedType.LPStr)]
							string userid,
							ref GPRS_USER_INFO infoPtr);



        //设置do_read_proc的读取方式。
        //当SetWorkMode设置为0时，do_read_proc处于阻塞读取方式，当程序执行do_read_proc时，直到读取到数据时才会返回。
        //这种方式下的程序应该设计为多线程方式，当要关闭服务时，需要调用cancel_read_block。
        //当SetWorkMode设置为1时，是非阻塞方式，当程序执行do_read_proc时，无论是否读取到数据，都将马上返回。这种方式
        //下的程序可以采用多线程或定时器方式进行实现。
        //当SetWorkMode设置为2时，start_net_service函数的hWnd参数和wMsg参数必须设置成有意义的值，当DTU有数据到达DSC
        //时，将触发wMsg消息到hWnd，此时程序应该立即执行do_read_proc。
        //本函数必须在start_net_service函数之前执行，否则无效。当不执行SetWorkMode，开发包将使用默认的模式，默认的模
        //式是2，即消息模式。
        //参数
        //nWorkMode	读取方式。0阻塞，1非阻塞，2消息。

        //返回值
        //返回参数nWorkMode的值。
        [DllImport(".\\wcomm_dll.dll")]
        public static extern int SetWorkMode(int nWorkMode);
        [DllImport(".\\wcomm_dll.dll")]
        public static extern int  SelectProtocol(int nProtocol);
        //
		//定义一些SOCKET API函数
		[DllImport("Ws2_32.dll")]
		public  static  extern  Int32  inet_addr(string  ip);
		[DllImport("Ws2_32.dll")]
        public static extern string inet_ntoa(uint ip);
		[DllImport("Ws2_32.dll")]
        public static extern uint htonl(uint ip);
		[DllImport("Ws2_32.dll")]
        public static extern uint ntohl(uint ip);
		[DllImport("Ws2_32.dll")]
        public static extern ushort htons(ushort ip);
		[DllImport("Ws2_32.dll")]
        public static extern ushort ntohs(ushort ip);



        //启动服务
        static public void StartGprsServer(IntPtr hwnd,int gprsport,int msg,ref StringBuilder mess)
        {
            //do_read_proc的读取方式设定为消息方式。当DTU有数据到达DSC时，将触发wMsg消息到hWnd
            SetWorkMode(2);
            SelectProtocol(1);
			//启动服务，hwnd-窗体 msg-消息 gprsport-端口 medd-返回的信息 
			start_net_service(hwnd, msg, gprsport, mess);
        }

        static public void CloseUser(string userid, ref StringBuilder mess)
        {
            do_close_one_user(userid, mess);
        }

        static public void ReSetUser(string userid, ref StringBuilder mess)
        {
            do_close_one_user2(userid, mess);
        }

        static public void ReSetAllUser(ref StringBuilder mess)
        {
            do_close_all_user2( mess);
        }

    }
}
		




/*


namespace gprstest
{	
	//结构定义
	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)] 
	public struct GPRS_DATA_RECORD
	{		
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
		public string	m_userid;		
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
		public string	m_recv_date;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]	//这里做了修改，转换时由ByValTStr变为ByValArray类型，
		public byte[]	m_data_buf;					//定位由string改为byte[]
		public ushort	m_data_len;
		public byte     m_data_type;
		public void Initialize()					//初始化byte[]的字段
		{			
			m_data_buf = new byte[1024];
		}
		//UnmanagedType.LPStr
	}	

	[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)] 
	public struct GPRS_USER_INFO
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
		public string	m_userid;					
		public uint     m_sin_addr;     
		public ushort   m_sin_port;     
		public uint     m_local_addr;   
		public ushort   m_local_port; 
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
		public string   m_logon_date;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
		public string	m_update_time;
		public byte		m_status;		
	}

	/// <summary>
	/// Form1 的摘要说明。
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{		
		//定义接口函数		
		[DllImport(".\\wcomm_dll.dll")]
		public static extern int start_gprs_server(
							IntPtr hWnd,
							int wMsg,
							int nServerPort,
							[MarshalAs(UnmanagedType.LPStr)]
							StringBuilder mess);
		[DllImport(".\\wcomm_dll.dll")]
		public static extern int start_net_service(
			IntPtr hWnd,
			int wMsg,
			int nServerPort,
			[MarshalAs(UnmanagedType.LPStr)]
			StringBuilder mess);

		[DllImport(".\\wcomm_dll.dll")]
		public static extern int stop_gprs_server(
							[MarshalAs(UnmanagedType.LPStr)]
							StringBuilder mess);
		[DllImport(".\\wcomm_dll.dll")]
		public static extern int stop_net_service(
			[MarshalAs(UnmanagedType.LPStr)]
			StringBuilder mess);

		[DllImport(".\\wcomm_dll.dll")]
		public static extern int do_read_proc(
							ref GPRS_DATA_RECORD recdPtr,
							[MarshalAs(UnmanagedType.LPStr)]
							StringBuilder mess,
							bool reply);		

		[DllImport(".\\wcomm_dll.dll")]
		public static extern int do_send_user_data(
							[MarshalAs(UnmanagedType.LPStr)]
							string userid,											
							[MarshalAs(UnmanagedType.LPStr)]
							string data,																		
							int len,
							[MarshalAs(UnmanagedType.LPStr)]
							StringBuilder mess);

		[DllImport(".\\wcomm_dll.dll")]
		public static extern int get_user_info(
							[MarshalAs(UnmanagedType.LPStr)]
							string userid,
							ref GPRS_USER_INFO infoPtr);

		[DllImport(".\\wcomm_dll.dll")]
		public static extern int SetWorkMode(int nWorkMode);
		//
		//定义一些SOCKET API函数
		[DllImport("Ws2_32.dll")]
		private  static  extern  Int32  inet_addr(string  ip);
		[DllImport("Ws2_32.dll")]
		private  static  extern  string inet_ntoa(uint  ip);
		[DllImport("Ws2_32.dll")]
		private  static  extern  uint htonl(uint  ip);
		[DllImport("Ws2_32.dll")]
		private  static  extern  uint ntohl(uint  ip);
		[DllImport("Ws2_32.dll")]
		private  static  extern  ushort htons(ushort  ip);
		[DllImport("Ws2_32.dll")]
		private  static  extern  ushort ntohs(ushort  ip);

		//

		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		/// <summary>
		/// 必需的设计器变量。
		/// 
		//定义窗口消息，用来响应DTU的消息
		public const int WM_DTU = 0x400 + 100;

		private System.Windows.Forms.TextBox tbPort;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbSend;
		private System.Windows.Forms.CheckBox checkBox1;
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();

			//
			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			//
		}

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.tbSend = new System.Windows.Forms.TextBox();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.tbPort = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(8, 16);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox1.Size = new System.Drawing.Size(504, 208);
			this.textBox1.TabIndex = 0;
			this.textBox1.Text = "";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(528, 72);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(88, 32);
			this.button1.TabIndex = 1;
			this.button1.Text = "开始服务";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(528, 120);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(88, 32);
			this.button2.TabIndex = 2;
			this.button2.Text = "停止服务";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// listView1
			// 
			this.listView1.CheckBoxes = true;
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.columnHeader1,
																						this.columnHeader2,
																						this.columnHeader3,
																						this.columnHeader4,
																						this.columnHeader5,
																						this.columnHeader6});
			this.listView1.FullRowSelect = true;
			this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listView1.Location = new System.Drawing.Point(8, 272);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(608, 128);
			this.listView1.TabIndex = 3;
			this.listView1.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "终端登录号码";
			this.columnHeader1.Width = 100;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "移动网内IP地址";
			this.columnHeader2.Width = 100;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "移动网内IP端口";
			this.columnHeader3.Width = 100;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "登录时间";
			this.columnHeader4.Width = 100;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "终端出口IP地址";
			this.columnHeader5.Width = 100;
			// 
			// columnHeader6
			// 
			this.columnHeader6.Text = "终端出口IP端口";
			this.columnHeader6.Width = 100;
			// 
			// tbSend
			// 
			this.tbSend.Location = new System.Drawing.Point(8, 240);
			this.tbSend.Name = "tbSend";
			this.tbSend.Size = new System.Drawing.Size(544, 21);
			this.tbSend.TabIndex = 4;
			this.tbSend.Text = "";
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(560, 240);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(56, 24);
			this.button3.TabIndex = 5;
			this.button3.Text = "发送";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(528, 192);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(88, 32);
			this.button4.TabIndex = 6;
			this.button4.Text = "清除显示";
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// tbPort
			// 
			this.tbPort.Location = new System.Drawing.Point(528, 40);
			this.tbPort.Name = "tbPort";
			this.tbPort.Size = new System.Drawing.Size(88, 21);
			this.tbPort.TabIndex = 7;
			this.tbPort.Text = "5002";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(528, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 16);
			this.label1.TabIndex = 8;
			this.label1.Text = "端口:";
			// 
			// checkBox1
			// 
			this.checkBox1.Location = new System.Drawing.Point(536, 160);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(56, 24);
			this.checkBox1.TabIndex = 9;
			this.checkBox1.Text = "回应";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(624, 413);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tbPort);
			this.Controls.Add(this.tbSend);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.listView1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Form1";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void button4_Click(object sender, System.EventArgs e)
		{
			textBox1.Text = "";
		}

		private void button1_Click(object sender, System.EventArgs e)
		{	
			SetWorkMode(2);

			//启动服务
			StringBuilder mess = new StringBuilder(100);
			//start_gprs_server(this.Handle, WM_DTU, int.Parse(tbPort.Text), mess);			
			start_net_service(this.Handle, WM_DTU, int.Parse(tbPort.Text), mess);						
			textBox1.AppendText(mess.ToString() + "\n");
		}			
	
		protected override void WndProc(ref Message m)
		{
			// TODO:  添加 Form1.WndProc 实现
			int i;

			//响应DTU消息
			if (m.Msg == WM_DTU)
			{
				GPRS_DATA_RECORD recdPtr = new GPRS_DATA_RECORD();
				StringBuilder mess = new StringBuilder(100);					

				//读取DTU数据
				//do_read_proc(dr, mess, checkBox1.Checked);				
				do_read_proc(ref recdPtr, mess, checkBox1.Checked);

				byte a = recdPtr.m_data_type;
				
				switch (recdPtr.m_data_type)
				{
					case 0x01:	//注册包												
						GPRS_USER_INFO user_info = new GPRS_USER_INFO();
						ushort usPort;
						if (get_user_info(recdPtr.m_userid, ref user_info) == 0)
						{	
							//已经注册过													
							for (i = 0; i <	listView1.Items.Count; i++)
								if (listView1.Items[i].Text == user_info.m_userid)
								{																											
									listView1.Items[i].SubItems.Add(inet_ntoa(ntohl(user_info.m_local_addr)));
									
									usPort = user_info.m_local_port;
									listView1.Items[i].SubItems.Add(usPort.ToString());		
							
									listView1.Items[i].SubItems.Add(user_info.m_logon_date);
									
									listView1.Items[i].SubItems.Add(inet_ntoa(ntohl(user_info.m_sin_addr)));
									
									usPort = user_info.m_sin_port;
									listView1.Items[i].SubItems.Add(usPort.ToString());									
									return;
								}

							//没有注册过
							ListViewItem lvi = listView1.Items.Add(user_info.m_userid);														
							lvi.SubItems.Add(inet_ntoa(ntohl(user_info.m_local_addr)));
									
							usPort = user_info.m_local_port;
							lvi.SubItems.Add(usPort.ToString());		
							
							lvi.SubItems.Add(user_info.m_logon_date);
									
							lvi.SubItems.Add(inet_ntoa(ntohl(user_info.m_sin_addr)));
									
							usPort = user_info.m_sin_port;
							lvi.SubItems.Add(usPort.ToString());	
						}
						break;
					case 0x02:	//注销包											
						for (i = 0; i <	listView1.Items.Count; i++)
							if (listView1.Items[i].Text == recdPtr.m_userid)
							{								
								listView1.Items[i].Remove();
								break;
							}
						break;
					case 0x04:	//无效包
						break;
					case 0x09:	//数据包								 
						//int d=3;
						//string xx=recdPtr.m_data_buf;
						//string bb=recdPtr.m_data_buf;
						//byte[] ss = System.Text.Encoding.Default.GetBytes("a");
						//int b = int.Parse(recdPtr.m_data_buf, System.Globalization.NumberStyles.HexNumber);  
						//System.Text.Encoding.ASCII.GetString(bs);
						//string ss = System.Text.Encoding.ASCII.GetString(bc);
						//textBox1.AppendText(b + "\n");
						//SqlConnection conn=new SqlConnection("Data Source=192.168.1.188;UID=sa;PWD=1113;DATABASE=weiz");
						//conn.Open();						
						
  
                        textBox1.AppendText(System.Text.Encoding.Default.GetString(recdPtr.m_data_buf) + "\r\n");
						string name="";
						//int connection=0;
						string lc="";
						int  x=0;
						double zhib=0;
						byte ch, ch1, ch2;		//不能使用char类型进行计算，否则会产生溢出
						//int count = System.Text.Encoding..GetByteCount(recdPtr.m_data_buf);						
						//byte[] bc = System.Text.Encoding.Default.GetBytes(recdPtr.m_data_buf);						
						ch = recdPtr.m_data_buf[0];
						ch1 = recdPtr.m_data_buf[1];
						ch2 = recdPtr.m_data_buf[2];
						//if(recdPtr.m_data_buf.Substring(0,1)==0x31)
						x = ch1*256 + ch2;
						//ch = recdPtr.m_data_buf[0];
						//if(recdPtr.m_data_buf.Substring(0,1)==0x31)
						
						if(ch==0x31)
						{
							//connection=int.Parse(recdPtr.m_data_buf[1].ToString())<<8|int.Parse(recdPtr.m_data_buf[2].ToString());
							//x=connection;
							zhib=(x-855)*5/3241 +0.5;
							name="(TP)总磷";
							lc="5 mg/l";
						  
						}
						if(ch==0x32)
						{
							//connection=int.Parse(recdPtr.m_data_buf[1].ToString())<<8|int.Parse(recdPtr.m_data_buf[2].ToString());
							//x=connection;
							zhib=(x-855)*10/3241 +0.5;
							name="(TN)总氮";
							lc="10 mg/l";
						}
							
						if(ch==0x33)
						{
							//connection=int.Parse(recdPtr.m_data_buf[1].ToString())<<8|int.Parse(recdPtr.m_data_buf[2].ToString());
							//x=connection;
							zhib=(x-855)*50/3241 +0.5;
							name="(COD)化学需氧量";
							lc="50 mg/l";
						}
							
						if(ch==0x34)
						{
							//connection=int.Parse(recdPtr.m_data_buf[1].ToString())<<8|int.Parse(recdPtr.m_data_buf[2].ToString());
							//x=connection;
							zhib=(x-855)*2/3241 +0.5;
							name="(UV)紫外光吸光度";
							lc="2 Abs";
						}
							
						if(ch==0x35)
						{
							//connection=int.Parse(recdPtr.m_data_buf[1].ToString())<<8|int.Parse(recdPtr.m_data_buf[2].ToString());
							//x=connection;
							zhib=(x-855)*10/3241 +0.5;
							name="(TURB)浊度";
							lc="10 mg/l";
						}
							
						if(ch==0x36)
						{
							//connection=int.Parse(recdPtr.m_data_buf[1].ToString())<<8|int.Parse(recdPtr.m_data_buf[2].ToString());
							//x=connection;
							zhib=(x-855)*10/3241 +0.5;
							name="(NH4-N)氨氮";
							lc="10 mg/l";
						}
							
						// string ccc=recdPtr.m_data_buf.Length.ToString();
						
						//	int liangc=0;
						//int liangc=int.Parse(recdPtr.m_data_buf.Substring(2,4).ToString())-31;
						string sql="insert into weiz (name,zhib,liancheng) values ('"+name+"','"+zhib+"','"+lc+"')";
						
						//SqlCommand cm4=new SqlCommand (sql,conn);
						//cm4.ExecuteNonQuery();
						//conn.Close();
						//label2.Text=recdPtr.m_data_buf;
						break;
					default:
						break;
				}
			}
			else
			{	
				//缺省消息处理
				base.WndProc (ref m);
			}
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			StringBuilder mess = new StringBuilder(100);
			//停止服务
			//stop_gprs_server(mess);			
			stop_net_service(mess);
			textBox1.AppendText(mess.ToString());
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			StringBuilder mess = new StringBuilder(100);			

			int i;														
			
			//这段代码是发送文本框的内容，即ASCII字符
			for (i = 0; i < listView1.Items.Count; i++)
				if (listView1.Items[i].Checked)
				{								
					//注意对中文的处理，TextBox取得的字符串会把中文当一个字符，导致发送的字节数减少。我这里没有处理这个	
					//使用System.Text.Encoding.Default.GetByteCount可以获得字节数
					if (do_send_user_data(listView1.Items[i].Text, tbSend.Text, System.Text.Encoding.Default.GetByteCount(tbSend.Text), mess) == -1)
						textBox1.AppendText(mess.ToString());
				}					

			//////////////////////////////////////////////////////////////////////////

			/*
			 * 这段代码是字节数组的传送例子，数组的值自己填，我这里采用固定的值
			 * 经测试，传送的数据没有问题。
			 */
			/*
			string s = new string((char)0, 300);					//分配300字节长度的字符串数组
			byte[] bc = System.Text.Encoding.Default.GetBytes(s);	//转换到字节数组
			for (i = 0; i < 300; i++)								//通过字节数组进行赋值				
				bc[i] = (byte)(i * 2);
				
			string ss = System.Text.Encoding.Default.GetString(bc, 0, 300);	//将该字节数组转换到字符串进行传送
			*/
			 /*
			 * 注意：
			 * 字符串s的作用只是分配空间，
			 * 字节数组bc的作用是赋值，赋值时要注意类型转换
			 * 字符串ss的作用是作为do_send_user_data的参数
			 */
			/*
			for (i = 0; i < listView1.Items.Count; i++)
				if (listView1.Items[i].Checked)
				{													
					//发送全部300字节
					if (do_send_user_data(listView1.Items[i].Text, ss, 300, mess) == -1)
						textBox1.AppendText(mess.ToString());
				}	
			*/
			/* 
			* 调试的时候要注意char 和 byte类型的区别，char 显示大于127的值时会显示负数
			* 下面是DTU收到的数据，显示为16进制
			* 00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F 10 11 12 13 14 15 16 17 18 19 
			* 1A 1B 1C 1D 1E 1F 20 21 22 23 24 25 26 27 28 29 2A 2B 2C 2D 2E 2F 30 31 32 33 
			* 34 35 36 37 38 39 3A 3B 3C 3D 3E 3F 40 41 42 43 44 45 46 47 48 49 4A 4B 4C 4D 
			* 4E 4F 50 51 52 53 54 55 56 57 58 59 5A 5B 5C 5D 5E 5F 60 61 62 63 64 65 66 67 
			* 68 69 6A 6B 6C 6D 6E 6F 70 71 72 73 74 75 76 77 78 79 7A 7B 7C 7D 7E 7F 80 81 
			* 82 83 84 85 86 87 88 89 8A 8B 8C 8D 8E 8F 90 91 92 93 94 95 96 97 98 99 9A 9B 
			* 9C 9D 9E 9F A0 A1 A2 A3 A4 A5 A6 A7 A8 A9 AA AB AC AD AE AF B0 B1 B2 B3 B4 B5 
			* B6 B7 B8 B9 BA BB BC BD BE BF C0 C1 C2 C3 C4 C5 C6 C7 C8 C9 CA CB CC CD CE CF 
			* D0 D1 D2 D3 D4 D5 D6 D7 D8 D9 DA DB DC DD DE DF E0 E1 E2 E3 E4 E5 E6 E7 E8 E9 
			* EA EB EC ED EE EF F0 F1 F2 F3 F4 F5 F6 F7 F8 F9 FA FB FC FD FE FF 00 01 02 03 
			* 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F 10 11 12 13 14 15 16 17 18 19 1A 1B 1C 1D 
			* 1E 1F 20 21 22 23 24 25 26 27 28 29 2A 2B 
			*/						

