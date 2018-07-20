using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.Serialization;


namespace QyTech.WebFormCommuWcf
{
    [DataContract]
    public class MessageEntity
    {
        [DataMember]
        public string JsonObject { get; set; }
    }
}
