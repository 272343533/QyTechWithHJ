using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Runtime.InteropServices;

using QyProtocal.Protocal;
using QyTech.HDGprs;

namespace QyTech.DtuDll
{

    public enum DtuWmCode { Hd = 100, Inhand = 105, Jx = 200, }
    public abstract class IDtuDllProduct
    {
        public static ILog log = log4net.LogManager.GetLogger("DtuDll");

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);


        protected static IDtuDllProduct _instance;
        protected bool _started = false;
        public bool Started
        {
            set
            {
                _started = value;
            }
            get
            {
                return _started;
            }
        }


        /// <summary>
        /// 转换为标准用户数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract GPRS_USER_INFO TransToUser(object obj);
       
        /// <summary>
        /// 转换为标准消息数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract GPRS_DATA_RECORD TransToRecord(object obj);
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="Hwnd"></param>
        /// <param name="inhand_wm_dtu"></param>
        /// <param name="port"></param>
        /// <param name="mess"></param>
        /// <returns></returns>
        public abstract bool StartService(IntPtr Hwnd, int port, StringBuilder mess);

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="mess"></param>
        /// <returns></returns>
        public abstract bool StopService(StringBuilder mess);
     
        /// <summary>
        /// 获取连接用户数据
        /// </summary>
        /// <param name="dtuList"></param>
        /// <returns></returns>
        public abstract bool GetUserList(ref Dictionary<string, GPRS_USER_INFO> dtuList);


        public abstract bool GetUserInfo(string userid, ref GPRS_USER_INFO gui);
      

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="data"></param>
        /// <param name="mess"></param>
        /// <returns></returns>
        public abstract bool SendData(string userid, byte[] data, StringBuilder mess);
        
        /// <summary>
        ///读取数据 
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="data"></param>
        /// <param name="mess"></param>
        /// <returns></returns>
        public abstract bool ReadData(ref GPRS_DATA_RECORD gdr,StringBuilder mess);

    }
}
