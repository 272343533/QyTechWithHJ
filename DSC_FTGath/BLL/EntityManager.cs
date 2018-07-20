using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace HeatingDSC2.BLL
{
    public partial class  EntityManager
    {
        /// <summary>
        /// 实体类复制
        /// </summary>
        /// <param name="objold"></param>
        /// <param name="objnew"></param>
        public static void EntityCopyExact(object objold, object objnew)
        {
            Type myType = objold.GetType();
            Type myType2 = objnew.GetType();
            PropertyInfo currobj = null;
            if (myType == myType2)
            {
                PropertyInfo[] myProperties = myType.GetProperties();
                for (int i = 0; i < myProperties.Length; i++)
                {
                    currobj = objold.GetType().GetProperties()[i];
                    currobj.SetValue(objnew, currobj.GetValue(objold, null), null);
                }
            }
        }
        public static void EntityCopyNotExact(object objold, object objnew)
        {
            Type myType = objold.GetType();
            Type myType2 = objnew.GetType();
            PropertyInfo currobj = null;
            PropertyInfo currobj2 = null;

            PropertyInfo[] myProperties = myType.GetProperties();
            for (int i = 0; i < myProperties.Length; i++)
            {
                try
                {
                    currobj = objold.GetType().GetProperties()[i];
                    currobj2 = objnew.GetType().GetProperties()[i];
                    object v=currobj.GetValue(objold, null);
                    currobj2.SetValue(objnew,v , null);
                }
                catch (Exception ex)
                {
                    
                    continue;
                }
            }
        }
    }
}
