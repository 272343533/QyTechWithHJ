using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunMvcExpress.Dao;
using System.Data.Objects.DataClasses;
using System.Data.Objects;
using System.Data.Entity;


namespace SunMvcExpress.Core.BLL
{
    
    public class SingletonLinkDb
    {
        private static WSExpressEntities CommonDb_ = new WSExpressEntities();

   
        private SingletonLinkDb() { }  
        public static ObjectContext GetCommon{
            get
            {
                return CommonDb_;
            }
        }

        private static HjHeatingHisEntities HisDb_ = new HjHeatingHisEntities();
        public static ObjectContext GetHis { 
            get { 
                return HisDb_; 
            } 
        }
    }
}
