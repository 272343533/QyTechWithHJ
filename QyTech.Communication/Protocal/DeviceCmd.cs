using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QyTech.Protocal
{
    public enum CmdStatus { Prepare = 1, Sent, Success, TimeOut }
  
    public class DeviceCmd
    {
        public string DefFlag; //为命令的判断自定义标志
        public Guid bsO_id;
        public int bsP_Id;
        public string CommNo;
        public string CommandDesp;//描述
        public string Command;
        public byte[] SendCmd;
        public string Response;
       // public CmdStatus Status;   //准备下发，已发送，成功，超时

        public DateTime NeedSendTime; //需要下发的时间
        public DateTime ExpiredTime;//过期时间 不过期，则为DateTime.MaxValue;

        public DateTime SendTime;     //下发的时间
        public DateTime ResponseTime; //回应包到的时间

        public DateTime SetDownTime;  //设定洗发的时间

        public DeviceCmd()
        {
            DefFlag  = "";
            NeedSendTime = DateTime.Now;
        }
    }

}
