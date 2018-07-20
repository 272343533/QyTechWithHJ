using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCPLibrary.Abstracts;
using System.IO;
using DSC_FTHj1;
namespace TCPLibrary.DefaultImplements
{
    /// <summary>
    /// ZMessage的默认实现
    /// </summary>
    public class BaseMessage:ZMessage
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public int MsgType
        {
            get;
            set;
        }
        public ZdFT_Packet PackContent
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgType">1注册  2写数据回应 10 读取数据</param>
        /// <param name="packContent"></param>
        public BaseMessage(int msgType, ZdFT_Packet packContent)
        {
            MsgType = msgType;
            PackContent = packContent;
        }
        /// <summary>
        /// 按照规定协议，重写RawData属性
        /// </summary>
        public override byte[] RawData
        {
            get
            {
                //byte[] rawdata = PackContent.Create("1111");// new byte[MsgContent.Length]; //4 + 4 + MsgContent.Length];  //消息类型 + 消息长度 + 消息内容
                //using (MemoryStream ms = new MemoryStream(rawdata))
                //{
                //    BinaryWriter bw = new BinaryWriter(ms);
                  
                //    bw.Write(PackContent.Data); //最后写入消息内容
                //    return rawdata;
                //}
                return PackContent.RawData;
            }
        }
    }
}
