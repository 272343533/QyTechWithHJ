using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using SunMvcExpress.Common.Xml;
using SunMvcExpress.Core.Common;

using SunMvcExpress.Dao;

namespace QyTech.Express.CodeCreate.Action
{
    public class ActionFac:IFuncFac
    {
        public static IControllerCreate Create(string typeName)
        {
            //Type type = Type.GetType(XmlConfig.GetValue(typeName)+"Contr");
            Type type = Type.GetType("QyTech.Express.CodeCreate.Action." + typeName);
            ConstructorInfo ci = type.GetConstructor(System.Type.EmptyTypes); ;
            return (IControllerCreate)ci.Invoke(null);
        }
    }
}
