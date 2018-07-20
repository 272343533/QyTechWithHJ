using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using log4net;
using QyTech.HDGprs;

namespace QyTech.DtuDll
{

    public class InhandDtuDll : IDtuDllProduct
    {
       

        private InhandDtuDll()
        {
        }
        public static InhandDtuDll Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new InhandDtuDll();
                }
                return (InhandDtuDll)_instance;
            }
        }


        public override GPRS_USER_INFO TransToUser(object obj)
        {
            InHand_USER_INFO iui = (InHand_USER_INFO)obj;
            GPRS_USER_INFO gui = new GPRS_USER_INFO();
            gui.m_userid = iui.m_userid;
            gui.m_update_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            try
            {
                byte[] tmp = new byte[4];
                Buffer.BlockCopy(iui.m_update_time,0,tmp,0,4);
               
                DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime().AddSeconds(System.BitConverter.ToInt32(tmp, 0));
                gui.m_update_time = dt.ToString("yyyy-MM-dd HH:mm:ss"); ;

            }
            catch (Exception ex)
            {
                log.Error("TransToUser:" + ex.Message);
            }
            gui.m_status = iui.m_status;
            gui.m_sin_port = iui.m_sin_port;
            gui.m_sin_addr = iui.m_sin_port;
            gui.m_logon_date = iui.m_logon_date;
            gui.m_local_port = iui.m_local_port;
            gui.m_local_addr = iui.m_local_addr;

            return gui;
        }

        /// <summary>
        /// 秒数转日期
        /// </summary>
        /// <param name="Value">秒数</param>
        /// <returns>日期</returns>
        public static DateTime GetGMTDateTime(int Value)
        {
            //秒数转时间日期
            //GMT时间从2000年1月1日开始，先把它作为赋为初值
            long Year = 2000, Month = 1, Day = 1;
            long Hour = 0, Min = 0, Sec = 0;
            //临时变量
            long iYear = 0, iDay = 0;
            long iHour = 0, iMin = 0, iSec = 0;
            //计算文件创建的年份
            iYear = Value / (365 * 24 * 60 * 60);
            Year = Year + iYear;
            //计算文件除创建整年份以外还有多少天
            iDay = (Value % (365 * 24 * 60 * 60)) / (24 * 60 * 60);
            //把闰年的年份数计算出来
            int RInt = 0;
            for (int i = 2000; i < Year; i++)
            {
                if ((i % 4) == 0)
                    RInt = RInt + 1;
            }
            //计算文件创建的时间(几时)
            iHour = ((Value % (365 * 24 * 60 * 60)) % (24 * 60 * 60)) / (60 * 60);
            Hour = Hour + iHour;
            //计算文件创建的时间(几分)
            iMin = (((Value % (365 * 24 * 60 * 60)) % (24 * 60 * 60)) % (60 * 60)) / 60;
            Min = Min + iMin;
            //计算文件创建的时间(几秒)
            iSec = (((Value % (365 * 24 * 60 * 60)) % (24 * 60 * 60)) % (60 * 60)) % 60;
            Sec = Sec + iSec;
            DateTime t = new DateTime((int)Year, (int)Month, (int)Day, (int)Hour, (int)Min, (int)Sec);
            DateTime t1 = t.AddDays(iDay - RInt);
            return t1;
        }

        public override GPRS_DATA_RECORD TransToRecord(object obj)
        {
            GPRS_DATA_RECORD to = new GPRS_DATA_RECORD();
            try
            {
                InHand_DATA_RECORD from = (InHand_DATA_RECORD)obj;
               to.m_userid = from.m_userid;
                to.m_recv_date = from.m_recv_date;
                to.m_data_type = from.m_data_type;
                to.m_data_len = from.m_data_len;
                to.m_data_buf = new byte[1024];
                for (int i = 0; i < 1024; i++)
                {
                    to.m_data_buf[i] = from.m_data_buf[i];
                }
            }
            catch (Exception ex)
            {
                log.Error("TransToRecord:" + "---" + ex.Message);
            }
            return to;
        }

        public override bool StartService(IntPtr Hwnd, int port, StringBuilder mess)
        {
             bool flag = false;
             try
            {
               string str = "";
                if (this.Started) throw new Exception("服务已经启动");
                int mode = InHandDtu.SetWorkMode(2);

                int iflag = InHandDtu.SelectProtocol(1);
                if (iflag == 0)
                {
                    iflag = InHandDtu.start_gprs_server(Hwnd, (uint)(0x400+DtuDll.DtuWmCode.Inhand), port, mess);

                    flag = iflag == 0 ? true : false;
                    Started = flag;
                   
                    return flag;
                }
                return flag;
            }
            catch (Exception ee)
            {
                mess = new StringBuilder(ee.Message);
                return flag;
            }
        }

     


        public override bool StopService(StringBuilder mess)
        {
            try
            {
                if (!Started)
                    throw new Exception("服务尚未启动");
                bool flag = InHandDtu.stop_gprs_server(mess) == 0 ? true : false;
                Started = !flag;
                return flag;
            }
            catch (Exception ee)
            {
                mess = new StringBuilder(ee.Message.ToString());
                return false;
            }
        }

        public override bool GetUserList(ref Dictionary<string, GPRS_USER_INFO> dtuList)
        {
            try
            {
                int cnt = InHandDtu.get_online_user_amount();
                //System.Diagnostics.Debug.WriteLine("Count="+cnt.ToString());
                Dictionary<string, InHand_USER_INFO>  _dtuList = new Dictionary<string, InHand_USER_INFO>();
                InHand_USER_INFO dtu;
                for (uint ii = 0; ii < cnt; ii++)
                {
                    dtu = new InHand_USER_INFO();
                    bool flag = InHandDtu.get_user_at(ii, ref dtu) == 0 ? true : false;
                    if (flag)
                    {
                        dtuList.Add(dtu.m_userid, TransToUser(dtu));
                    }
                }
                return true;
            }
            catch (Exception ee)
            {
                return false;
            }
        }




        public override bool SendData(string userid, byte[] data, StringBuilder mess)
        {
            try
            {

                //byte[] bts = QyTech.Protocal.CommFunc.HexCmd2Bytes(data);

                byte[] user = UnicodeEncoding.Default.GetBytes(userid);
                // 采用普通控制方式, 请屏蔽专用控制方式内容
                //return this.SendHex(id, bts); 
                // 采用专用控制方式, 请屏蔽普通控制方式内容
                if (InHandDtu.do_send_user_data(user, data, (uint)data.Length, mess) == 0)
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


        public override bool ReadData(ref GPRS_DATA_RECORD gdr,StringBuilder mess)
        {
            InHand_DATA_RECORD idr = new InHand_DATA_RECORD();
            int ret = InHandDtu.do_read_proc(ref idr, mess, false);
            log.Error("映瀚通ReadData1:" +ret.ToString()+"---"+ mess.ToString());
          
         

            try
            {
                if (ret == 0)
                {
                    log.Error("映瀚通ReadData1_1:" + idr.m_userid+"--"+idr.m_data_type.ToString()+"--"+idr.m_data_len.ToString()+"--"+idr.m_data_buf.ToString());
                    gdr = TransToRecord(idr);
                }
            }
            catch (Exception ex)
            {
                log.Error("映瀚通ReadData2:" + ex.Message);
            }
            return ret == 0 ? true : false;
        }

        public override bool GetUserInfo(string userid, ref GPRS_USER_INFO gui)
        {
            InHand_USER_INFO iui = new InHand_USER_INFO();
            byte[] byteuser = Encoding.Default.GetBytes(userid);
            int ret = InHandDtu.get_user_info(byteuser, ref iui);
            gui = TransToUser(iui);
            return true;
        }
    }

}