using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QyTech.Core.Helpers
{
    public class AjaxJsonResult
    { 
        public String statusCode {set; get;}
        public String message {set; get;}
        public String navTabId{set;get;}
        public String rel {set;get;}
        public String callbackType {set; get;}
        public String forwardUrl {set;get;}
        public String confirmMsg {get; set;}
    }
}