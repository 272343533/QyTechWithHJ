using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QyTech.BLL
{
    public class GetJson
    {
        public static string GetJsonByObject(Object o = null) {
            return JsonHelper.SerializeObject(o);
        }
        public static string GetJsonByObjectFormatting(Object o = null)
        {
            return JsonHelper.SerializeObject1(o);
        }
    }
    
}

