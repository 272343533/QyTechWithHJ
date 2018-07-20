using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
//using QyTech.HDGprs;



namespace QyTech.Core.Refection
{
    public class BaseFunc
    {
        //public static void Load(string dllPath)
        //{
        //    Assembly ass = Assembly.LoadFrom(dllPath); 
        //    //2、加载dll后,需要使用dll中某类.
        //    Type type = ass.GetType("HjProtocal2.ReadData");//用类型的命名空间和名称获得类型
 
        //    //3、需要实例化类型,才可以使用,参数可以人为的指定,也可以无参数,静态实例可以省略
        //    Object obj = Activator.CreateInstance(type);//利用指定的参数实例话类型
 
        //    //4、调用类型中的某个方法:
        //    //需要首先得到此方法
        //    MethodInfo mi=type.GetMethod("Parse");//通过方法名称获得方法
 
        //    //5、然后对方法进行调用,多态性利用参数进行控制
        //    mi.Invoke(obj,GPRS_DATA_RECORD recdPtr);//根据参数直线方法,返回值就是原方法的返回值


        //}

       
        /// <summary>
        /// 类型匹配
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static bool IsType(Type type, string typeName)
        {
            if (type.ToString().Contains(typeName))
                return true;
            if (type.ToString() == "System.Object")
                return false;

            return IsType(type.BaseType, typeName);
        }
        public static T SetValue<T>(T obj, PropertyInfo propertyInfo, object val)
        {

            if (IsType(propertyInfo.PropertyType, "System.Decimal"))
            {
                if (val != null && val.ToString() != "")
                    propertyInfo.SetValue(obj, Decimal.Parse(val.ToString()), null);
                else
                    propertyInfo.SetValue(obj, new Decimal(0), null);

            }
            else if (IsType(propertyInfo.PropertyType, "System.Int16"))
            {
                if (val != null && val.ToString() != "")
                    propertyInfo.SetValue(obj, System.Int16.Parse(val.ToString()), null);
                else
                    propertyInfo.SetValue(obj,0, null);

            }
            else if (IsType(propertyInfo.PropertyType, "System.Int32"))
            {
                if (val != null && val.ToString() != "")
                    propertyInfo.SetValue(obj, int.Parse(val.ToString()), null);
                else
                    propertyInfo.SetValue(obj, 0, null);

            }
            else if (IsType(propertyInfo.PropertyType, "System.Int64"))
            {
                if (val != null && val.ToString() != "")
                    propertyInfo.SetValue(obj, System.Int64.Parse(val.ToString()), null);
                else
                    propertyInfo.SetValue(obj, 0, null);

            }
            else if (IsType(propertyInfo.PropertyType, "System.String"))
            {
                propertyInfo.SetValue(obj, val.ToString(), null); 
            }
            else
                propertyInfo.SetValue(obj, val, null); //给对应属性赋值 

            return obj;

        }
    }
}
