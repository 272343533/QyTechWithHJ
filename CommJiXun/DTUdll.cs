using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using log4net;
namespace DTU_JiXun
{
    internal class DTUdll
    {

        public static ILog log = log4net.LogManager.GetLogger("DTUdll");


        #region API
        [DllImport(@"gprsdll.dll")]
        private static extern bool DSStartService(ushort uiListenPort);

        [DllImport(@"gprsdll.dll")]
        private static extern bool DSStopService();

        [DllImport(@"gprsdll.dll")]
        private static extern int DSGetModemCount();

        [DllImport(@"gprsdll.dll")]
        private static extern bool DSGetModemByPosition(uint pos, ref ModemInfoStruct pModemInfo);

        [DllImport(@"gprsdll.dll")]
        private static extern bool DSGetNextData(ref ModemDataStruct pDataStruct, ushort waitseconds);

        [DllImport(@"gprsdll.dll")]
        private static extern bool DSSendData(uint modemId, ushort len, byte[] buf);

        [DllImport(@"gprsdll.dll")]
        private static extern bool DSSendControl(uint modemId, ushort len, byte[] buf);

        [DllImport(@"gprsdll.dll")]
        private static extern void DSGetLastError(System.Text.StringBuilder str, int nMaxBufSize); 
        #endregion

        private DTUdll()
        {
        }

        private static DTUdll _instance;
        public static DTUdll Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DTUdll();

                    log.Error("JX 的新实例");
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

        public string _lastError;
        public string LastError
        {
            private set
            {
                _lastError = value;
            }
            get
            {
                return _lastError;
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

        private void GetLastError()
        {
            try
            {
                StringBuilder sb = new StringBuilder(256);
                DSGetLastError(sb, 256);
                LastError = sb.ToString();
            }
            catch(Exception ee)
            {
                LastError = ee.Message;
            }
        }

        public bool StartService(ushort port)
        {
            try
            {
                if (this.Started) throw new Exception("服务已经启动");
                bool flag = DSStartService(port);
                if (!flag)
                    this.GetLastError();
                else
                    LastError = null;
                this.Started = flag;
                return flag;
            }
            catch (Exception ee)
            {
                LastError = ee.Message;
                return false;
            }
        }

        public bool StopService()
        {
            try
            {
                if (!this.Started) throw new Exception("服务尚未启动");
                bool flag = DSStopService();
                if (!flag)
                    this.GetLastError();
                else
                    LastError = null;
                this.Started = !flag;
                return flag;
            }
            catch (Exception ee)
            {
                LastError = ee.Message;
                return false;
            }
        }

        public bool GetDTUList(out Dictionary<uint,ModemInfoStruct> dtuList)
        {
            try
            {
                int cnt = DSGetModemCount();
                //System.Diagnostics.Debug.WriteLine("Count="+cnt.ToString());
                dtuList = new Dictionary<uint, ModemInfoStruct>();
                for (uint ii = 0; ii < cnt; ii++)
                {
                    ModemInfoStruct dtu = new ModemInfoStruct();
                    bool flag = DSGetModemByPosition(ii, ref dtu);
                    if (!flag)
                    {
                        this.GetLastError();
                        return false;
                    }
                    else
                    {
                        dtuList.Add(dtu.m_modemId,dtu);
                    }
                }
                LastError = null;
                return true;
            }
            catch(Exception ee)
            {
                LastError = ee.Message;
                dtuList = new Dictionary<uint, ModemInfoStruct>();
                return false;
            }
        }

        public bool GetNextData(out ModemDataStruct dat)
        {
            dat = new ModemDataStruct();
            return DSGetNextData(ref dat, 0);
        }

        public bool SendControl(uint id, string text)
        {
            try
            {
                byte[] bts = UnicodeEncoding.Default.GetBytes(text+"\r");
                // 采用普通控制方式, 请屏蔽专用控制方式内容
                //return this.SendHex(id, bts); 
                // 采用专用控制方式, 请屏蔽普通控制方式内容
                if (DSSendControl(id, (ushort)bts.Length, bts))
                {
                    LastError = null;
                    return true;
                }
                else
                {
                    this.GetLastError();
                    return false;
                } 
            }
            catch (Exception ee)
            {
                LastError = ee.Message;
                return false;
            }
        }

        public bool SendText(uint id, string text)
        {
            try
            {
                return this.SendHex(id, UnicodeEncoding.Default.GetBytes(text));
            }
            catch (Exception ee)
            {
                LastError = ee.Message;
                return false;
            }
        }

        public bool SendHex(uint id, byte[] bts)
        {
            try
            {
                return this.SendHex(id, bts, (ushort)bts.Length);
            }
            catch (Exception ee)
            {
                LastError = ee.Message;
                return false;
            }
        }

        public bool SendHex(uint id, byte[] bts, int startIndex, ushort lenth)
        {
            try
            {
                lenth = (ushort)Math.Min(lenth, bts.Length - startIndex);
                byte[] bsnd = new byte[lenth];
                Array.Copy(bts, startIndex, bsnd, 0, lenth);
                return SendHex(id, bsnd, lenth);
            }
            catch (Exception ee)
            {
                LastError = ee.Message;
                return false;
            }
        }

        public bool SendHex(uint id, byte[] bts,ushort lenth)
        {
            try
            {
                if (DSSendData(id, lenth, bts))
                {
                    LastError = null;
                    return true;
                }
                else
                {
                    this.GetLastError();
                    return false;
                }
            }
            catch (Exception ee)
            {
                LastError = ee.Message;
                return false;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Size = 40)]
    //internal struct ModemInfoStruct
    public struct ModemInfoStruct
    {
        public uint m_modemId;              //Modem模块的ID号
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] m_phoneno;             //Modem的11位电话号码，必须以'\0'字符结尾  
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] m_dynip;              //Modem的4位动态ip地址   
        public uint m_conn_time;            //Modem模块最后一次建立TCP连接的时间 
        public uint m_refresh_time;         //Modem模块最后一次收发数据的时间     
    }

    //internal struct ModemDataStruct
    public struct ModemDataStruct
    {
        public uint m_modemId;		     	    // Modem模块的ID号
        public uint m_recv_time;				//接收到数据包的时间
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1451)]
        public byte[] m_data_buf;               //存储接收到的数据
        public ushort m_data_len;				//接收到的数据包长度
        public byte m_data_type;	          	//接收到的数据包类型,
                                                //	0x01：用户数据包 
                                                //	0x02：对控制命令帧的回应
    }
}