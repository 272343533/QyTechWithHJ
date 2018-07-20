using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using log4net;

namespace TCPLibrary.Abstracts
{
    /// <summary>
    /// TCP通信服务端
    /// </summary>
    public abstract class ZServerSocket
    {

        public static ILog log = log4net.LogManager.GetLogger("ZServerSocket");

        /// <summary>
        /// 侦听Socket
        /// </summary>
        private Socket _socket;
        /// <summary>
        /// 服务端侦听状态
        /// </summary>
        private bool _run = false;

        /// <summary>
        /// 客户端建立连接时激发该事件
        /// </summary>
        public event ConnectedEventHandler Connected;
        /// <summary>
        /// 客户端断开连接时激发该事件
        /// </summary>
        public event DisConnectedEventHandler DisConnected;
        /// <summary>
        /// 收到客户端消息时激发该事件
        /// </summary>
        public event MessageReceivedEventHandler MessageReceived;

        /// <summary>
        /// 创建代理Socket
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        protected abstract ZProxySocket GetProxy(Socket socket);

        /// <summary>
        /// 开始侦听
        /// </summary>
        /// <param name="port"></param>
        public void StartAccept(byte[] ipbytes, int port)
        {
            IPAddress addr;
            if (ipbytes[0] == 0 || ipbytes[0]==127)
                addr = IPAddress.Any;
            else
                addr = new IPAddress(ipbytes);
            if (!_run)
            {
                if (_socket == null)
                {
                    _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }
                _socket.Bind(new IPEndPoint(addr, port));
                _socket.Listen(250);
                _socket.BeginAccept(new AsyncCallback(OnAccept), null);

                _run = true;
            }
        }

        /// <summary>
        /// 侦听回调方法
        /// </summary>
        /// <param name="ar"></param>
        private void OnAccept(IAsyncResult ar)
        {
            try
            {
                Socket new_accept = _socket.EndAccept(ar);  //新连接socket
                ZProxySocket proxy = GetProxy(new_accept);  //创建代理socket
                proxy.DisConnected += new DisConnectedEventHandler(proxy_DisConnected);
                proxy.MessageReceived += new MessageReceivedEventHandler(proxy_MessageReceived);
                proxy.StartReceive();  //代理socket开始接收数据

                if (Connected != null)
                {
                    Connected.BeginInvoke(proxy, null, null);  //激发Connected事件
                }
                _socket.BeginAccept(new AsyncCallback(OnAccept), null);  //开始下一次异步侦听
            }
            catch(Exception ex)
            {
                log.Error("OnAccept:" + ex.Message);
            }
        }
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="proxySocket"></param>
        /// <param name="message"></param>
        void proxy_MessageReceived(ZProxySocket proxySocket, ZMessage message)
        {
            if (MessageReceived != null)
            {
                MessageReceived(proxySocket, message);  //激发MessageReceived事件
            }
        }
        /// <summary>
        /// 客户端断开
        /// </summary>
        /// <param name="proxySocket"></param>
        void proxy_DisConnected(ZProxySocket proxySocket)
        {
            if (DisConnected != null)
            {
                DisConnected(proxySocket);  //激发DisConnected事件
            }
        }
    }
}
