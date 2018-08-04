using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.Web;
using log4net;
namespace QyTech.WebFormCommuWcf
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“Service1”。
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SendMessageService : ISendMessageService
    {
        ISendMessageServiceCallBack callback;
        Timer heartTimer;
        Random random = new Random();

        ILog log = LogManager.GetLogger("SendMessageService");

        #region ISendMessageService 成员

        public void GetMessage()
        {
            callback = OperationContext.Current.GetCallbackChannel<ISendMessageServiceCallBack>();
            heartTimer = new Timer(new TimerCallback(heartTimer_Elapsed), null, 10, Timeout.Infinite);
        }

        #endregion

        private void heartTimer_Elapsed(object data)
        {
            List<MessageEntity> messageList = HttpRuntime.Cache["MessageEntityList"] as List<MessageEntity>;

            if (messageList != null && messageList.Count > 0)
            {
                try
                {
                    MessageEntity message = messageList[0];
                    if (messageList.Count > 0)
                    {
                        messageList.Remove(message);
                    }
                    HttpRuntime.Cache["MessageEntityList"] = messageList;
                    log.Info("callback" + DateTime.Now.ToString());
                    callback.ReceiveMessage(message);
                }
                catch (System.TimeoutException ex)
                {
                    //超时等待
                    log.Error(ex.Message);

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }


            }
            int interval = random.Next(5, 15);
            heartTimer.Change(interval, Timeout.Infinite);
        }
    }
}
