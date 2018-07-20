using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QyTech.Core.Common
{
    public enum InnerAccount { devAdmin,sysAdmin}

    public class devAdmin
    {
        public static Guid CustId = new Guid("E1C99D09-04A6-4F98-A270-9655DD71FEE9");
        public static Guid Id = new Guid("DD406509-FFF8-4515-9EA2-ECAAB435ADA6");
        //public static Guid Role = new Guid("76407F9A-6926-478E-8A85-25208FCED94A");


        //
        //4A7D1ED414474E4033AC29CCB8653D9B=12211438123
        //F379EAF3C831B04DE153469D1BEC345E=666666
        public static String Pwd = "F379EAF3C831B04DE153469D1BEC345E";
        public static string Name = "devadmin";
        public static string NickName = "开发人员";
    }
    public class sysAdmin
    {
        public static Guid CustId = new Guid("E1C99D09-04A6-4F98-A270-9655DD71FEE9");
        public static Guid Id = new Guid("1D406509-FFF8-4515-9EA2-ECAAB435ADA6");
        //public static Guid Role = new Guid("76407F9A-6926-478E-8A85-25208FCED94A");


        //
        //4A7D1ED414474E4033AC29CCB8653D9B=12211438123
        //F379EAF3C831B04DE153469D1BEC345E=666666
        public static String Pwd = "F379EAF3C831B04DE153469D1BEC345E";
        public static string Name = "sysadmin";
        public static string NickName = "系统管理员";
    }
}
