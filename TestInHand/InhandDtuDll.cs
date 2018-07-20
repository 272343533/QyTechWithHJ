using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using log4net;
namespace QyTech.InHandDtu
{
    //public  MAX_RECEIVE_BUF 1024

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct InHand_USER_INFO
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string m_userid; //= new char[12] //DTU 身份识别码
        public uint m_sin_addr; //DTU 进入Internet 的代理主机IP 地址
        public ushort m_sin_port; //DTU 进入Internet 的代理主机IP 端口
        public uint m_local_addr; //DTU 在移动网内IP 地址
        public ushort m_local_port; //DTU 在移动网内IP 端口
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string m_logon_date;// = new char[20]; //DTU 登录时间，字符串格式
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string m_update_time;// = new char[20]; //DTU 包更新时间，DC 接收到该DTU 最
        //近一个包的时间，使用前四字节，time_t 类型，后16 字节未使用
        public byte m_status; //DTU 状态, 1 在线 ，0 不在线
    };//user_info

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct InHand_DATA_RECORD
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string m_userid;//= new char[12]; // DTU 身份识别码
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string m_recv_date;// = new char[20]; //接收到数据包的时间
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]	//这里做了修改，转换时由ByValTStr变为ByValArray类型，
        public byte[] m_data_buf;// = new byte[1024]; //存储接收到的数据
        public ushort m_data_len; //接收到的数据包长度
        public byte m_data_type; //接收到的数据包类型， 1 收到心跳包， 2
        //收到退出包，3 收到登录包，9 收到终端发上来的数据，
        public void Initialize()					            //初始化byte[]的字段
        {
            m_data_buf = new byte[1024];
        }
    };//data_record;


    public class InhandDtuDll
    {

        public static ILog log = log4net.LogManager.GetLogger("DtuDll");

        //定义窗口消息，用来响应DTU的消息
        public const uint WM_DTU =0x0400 + 105;

        #region API

        [DllImport("user32.dll")]
        static extern IntPtr GetWindowDC(IntPtr hWnd);

        //[DllImport(@"gprs_dll.dll", EntryPoint = "SetWorkMode",CallingConvention=CallingConvention.Cdecl)]
        [DllImport(@"gprs_dll.dll", EntryPoint = "SetWorkMode", CharSet = CharSet.Auto, SetLastError = true,CallingConvention = CallingConvention.Cdecl)]
        private static extern int SetWorkMode(int nWorkMode);




        [DllImport(@"gprs_dll.dll", EntryPoint = "do_read_proc", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern int do_read_proc(
            ref InHand_DATA_RECORD pDataStruct,
             [MarshalAs(UnmanagedType.LPStr)] StringBuilder mess
            , bool reply);

        [DllImport(@"gprs_dll.dll", EntryPoint = "do_send_user_data", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern int do_send_user_data(
            byte[] userid,
             byte[] data,
            uint len,
            [MarshalAs(UnmanagedType.LPStr)] StringBuilder mess);

        [DllImport(@"gprs_dll.dll")]
        private static extern int do_close_one_user(string userid, StringBuilder mess);
        [DllImport(@"gprs_dll.dll")]
        public static extern int do_close_one_user2(string userid, StringBuilder mess);
        [DllImport(@"gprs_dll.dll")]
        public static extern int do_close_all_user(StringBuilder mess);
        [DllImport(@"gprs_dll.dll")]
        public static extern int do_close_all_user2(StringBuilder mess);
        [DllImport(@"gprs_dll.dll")]
        public static extern int get_max_user_amount();
        [DllImport(@"gprs_dll.dll")]
        public static extern int set_max_user_amount();
        [DllImport(@"gprs_dll.dll", EntryPoint = "get_user_at", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern int get_user_at(uint pos, ref InHand_USER_INFO userPtr);
        [DllImport(@"gprs_dll.dll", EntryPoint = "get_user_info", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern int get_user_info(
            byte[] userid, 
            ref InHand_USER_INFO userPtr);
        [DllImport(@"gprs_dll.dll")]
        public static extern int get_online_user_amount();
        [DllImport(@"gprs_dll.dll")]
        private static extern int DeleteAllUser();
        [DllImport(@"gprs_dll.dll")]
        private static extern int AddOneUser(InHand_USER_INFO userPtr);
        [DllImport(@"gprs_dll.dll")]
        private static extern int SetCustomIP(UInt64 ulIPAddr);


        [DllImport(@"gprs_dll.dll", EntryPoint = "SelectProtocol", CallingConvention = CallingConvention.Cdecl)]
        private static extern int SelectProtocol(int nProtocol);

        

        //[DllImport(@"gprs_dll.dll", EntryPoint = "start_gprs_server", CharSet = CharSet.Auto, SetLastError = false, 
        //    CallingConvention = CallingConvention.Cdecl)]
       [DllImport(@"gprs_dll.dll", EntryPoint = "start_gprs_server", CallingConvention = CallingConvention.Cdecl)]
        private static extern int start_gprs_server(
            IntPtr HWND,
            uint wMsg,
            int ServerPort,
            [MarshalAs(UnmanagedType.LPStr)]
           StringBuilder mess);
        [DllImport(@"gprs_dll.dll", EntryPoint = "stop_gprs_server", CallingConvention = CallingConvention.Cdecl)]
        private static extern int stop_gprs_server(
             [MarshalAs(UnmanagedType.LPStr)]
             StringBuilder mess);

        
        //BOOL _stdcall DLLCloseModemById(u32t modemId)
        #endregion
        public bool StartService(IntPtr Hwnd, uint inhand_wm_dtu,int port,StringBuilder mess)
        {
            try
            {
                string str = "";
                if (this.Started) throw new Exception("服务已经启动");
                int mode = SetWorkMode(2);

                int iflag = SelectProtocol(1);
                log.Error(mode.ToString());
                iflag = start_gprs_server(Hwnd, WM_DTU, port, mess);
                
                bool flag = iflag == 0 ? true : false;
                if (flag)
                {
                    this.Started = flag;
                }
                return flag;
            }
            catch (Exception ee)
            {
                mess = new StringBuilder(ee.Message);
                return false;
            }
        }

        private InhandDtuDll()
        {
        }

        private static InhandDtuDll _instance;
        public static InhandDtuDll Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new InhandDtuDll();
                }
                return _instance;
            }
        }

        private bool _started = false;
        public bool Started
        {
            private set
            {
                _started = value;
            }
            get
            {
                return _started;
            }
        }

        private ushort _listenPort = 0;
        public ushort ListenPort
        {
            private set
            {
                _listenPort = value;
            }
            get
            {
                return _listenPort;
            }
        }

     

    

        public bool StopService(ref StringBuilder mess)
        {
            try
            {
                if (!this.Started) 
                    throw new Exception("服务尚未启动");
                bool flag = stop_gprs_server(mess)==0?true:false;
                this.Started = !flag;
                return flag;
            }
            catch (Exception ee)
            {
                mess = new StringBuilder(ee.Message.ToString());
                return false;
            }
        }

        public bool GetDTUList(out Dictionary<string, InHand_USER_INFO> dtuList)
        {
            try
            {
                int cnt = get_online_user_amount();
                //System.Diagnostics.Debug.WriteLine("Count="+cnt.ToString());
                dtuList = new Dictionary<string, InHand_USER_INFO>();
                for (uint ii = 0; ii < cnt; ii++)
                {
                    InHand_USER_INFO dtu = new InHand_USER_INFO();
                    bool flag = get_user_at(ii, ref dtu)==0?true:false;
                    if (flag)
                    {
                        dtuList.Add(dtu.m_userid, dtu);
                    }
                }
                return true;
            }
            catch (Exception ee)
            {
                dtuList = new Dictionary<string, InHand_USER_INFO>();
                return false;
            }
        }

        //public bool GetNextData(out InHand_DATA_RECORD dat)
        //{
        //    dat = new InHand_DATA_RECORD();
        //    return DLLGetNextData(ref dat, 0);
        //}
        public static byte[] HexCmd2Bytes(string data)
        {
            byte[] bytes = new byte[data.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(data.Substring(i * 2, 2), 16);
            }
            return bytes;
        }
        public bool SendCommand(string userid, string data,StringBuilder mess)
        {
            try
            {
                
                byte[] bts =HexCmd2Bytes(data);
               
                byte[] user = UnicodeEncoding.Default.GetBytes(userid);
                // 采用普通控制方式, 请屏蔽专用控制方式内容
                //return this.SendHex(id, bts); 
                // 采用专用控制方式, 请屏蔽普通控制方式内容
                if (do_send_user_data(user, bts, (uint)bts.Length, mess) == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ee)
            {
                mess = new StringBuilder(ee.Message);
                return false;
            }
        
        }

        public bool SendText(string id, string text,StringBuilder mess)
        {
            try
            {
                return this.SendHex(id, UnicodeEncoding.Default.GetBytes(text),mess);
            }
            catch (Exception ee)
            {
                mess = new StringBuilder(ee.Message);
                return false;
            }
        }

        public bool SendHex(string id, byte[] bts, StringBuilder mess)
        {
            try
            {
                return this.SendHex(id, bts, (ushort)bts.Length,mess);
            }
            catch (Exception ee)
            {
                mess = new StringBuilder(ee.Message);
                return false;
            }
        }

        public bool SendHex(string id, byte[] bts, int startIndex, ushort lenth, StringBuilder mess)
        {
            try
            {
                lenth = (ushort)Math.Min(lenth, bts.Length - startIndex);
                byte[] bsnd = new byte[lenth];
                Array.Copy(bts, startIndex, bsnd, 0, lenth);
                return SendHex(id, bsnd, lenth,mess);
            }
            catch (Exception ee)
            {
                mess = new StringBuilder(ee.Message);
                return false;
            }
        }

        public bool SendHex(string id, byte[] bts, ushort lenth, StringBuilder mess)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bts.Length; i++)
            {
                sb.Append(bts[i].ToString("X2"));
            }
            string data = sb.ToString();
            try
            {
                if (SendCommand(id, data, mess))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ee)
            {
                mess =new StringBuilder(ee.Message);
                return false;
            }
        }

     
    }

}