using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using log4net;

namespace QyTech.Core.BLL
{
    public class EntityOperate
    {
        public static ILog log = log4net.LogManager.GetLogger("EntityOperate");
  
        public static T Copy<T>(object obj)
        {
            Type oldType = obj.GetType();
            Type newType = typeof(T);
            PropertyInfo currnew = null;
            object objT = newType.Assembly.CreateInstance(newType.FullName);
            
            PropertyInfo[] myProperties = newType.GetProperties();
            for (int i = 0; i < myProperties.Length; i++)
            {
                try
                {
                    currnew = objT.GetType().GetProperties()[i];
                    Type newobjtype = currnew.PropertyType;
                    if (newobjtype.ToString().Contains(typeof(int).FullName)
                        || newobjtype.ToString().Contains(typeof(decimal).FullName)
                        || newobjtype.ToString().Contains(typeof(long).FullName)
                        || newobjtype.ToString().Contains(typeof(String).FullName)
                        || newobjtype.ToString().Contains(typeof(Guid).FullName)
                        || newobjtype.ToString().Contains(typeof(DateTime).FullName)
                        || newobjtype.ToString().Contains(typeof(float).FullName)
                        || newobjtype.ToString().Contains(typeof(double).FullName)
                        || newobjtype.ToString().Contains(typeof(short).FullName)
                        || newobjtype.ToString().Contains(typeof(ushort).FullName)
                        || newobjtype.ToString().Contains(typeof(uint).FullName)
                        || newobjtype.ToString().Contains(typeof(ulong).FullName)
                        || newobjtype.ToString().Contains(typeof(char).FullName)
                        || newobjtype.ToString().Contains(typeof(byte).FullName)
                       )
                    {
                        foreach (PropertyInfo pi in obj.GetType().GetProperties())
                        {
                            if (pi.Name == currnew.Name)
                            {
                                object v = pi.GetValue(obj, null);
                                currnew.SetValue(objT, v, null);
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    continue;
                }
            }

            return (T)objT;
        }

        public static void Copy<T>(object obj,T objT,String key)
        {
            Type oldType = obj.GetType();
            Type newType = typeof(T);
            PropertyInfo currnew = null;
            //object objT = newType.Assembly.CreateInstance(newType.FullName);

            PropertyInfo[] myProperties = newType.GetProperties();
            for (int i = 0; i < myProperties.Length; i++)
            {
                try
                {
                    currnew = objT.GetType().GetProperties()[i];
                    if (currnew.Name == key)
                        continue;

                    Type newobjtype = currnew.PropertyType;
                    if (newobjtype.ToString().Contains(typeof(int).FullName)
                        || newobjtype.ToString().Contains(typeof(decimal).FullName)
                        || newobjtype.ToString().Contains(typeof(long).FullName)
                        || newobjtype.ToString().Contains(typeof(String).FullName)
                        || newobjtype.ToString().Contains(typeof(Guid).FullName)
                        || newobjtype.ToString().Contains(typeof(DateTime).FullName)
                        || newobjtype.ToString().Contains(typeof(float).FullName)
                        || newobjtype.ToString().Contains(typeof(double).FullName)
                        || newobjtype.ToString().Contains(typeof(short).FullName)
                        || newobjtype.ToString().Contains(typeof(ushort).FullName)
                        || newobjtype.ToString().Contains(typeof(uint).FullName)
                        || newobjtype.ToString().Contains(typeof(ulong).FullName)
                        || newobjtype.ToString().Contains(typeof(char).FullName)
                        || newobjtype.ToString().Contains(typeof(byte).FullName)
                       )
                    {
                        foreach (PropertyInfo pi in obj.GetType().GetProperties())
                        {
                            if (pi.Name == currnew.Name)
                            {
                                object v = pi.GetValue(obj, null);
                                currnew.SetValue(objT, v, null);
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    continue;
                }
            }

        }
    }
}
