using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using SunMvcExpress.Common.Xml;
using SunMvcExpress.Core.Common;
using SunMvcExpress.Dao;

namespace QyTech.Express.CodeCreate.IDataView
{
     public class DataPartFac
    {
        public static ISysFunConf Create(string typeName)
        {
            Type type = Type.GetType(" QyTech.Express.DataPart." + typeName);
            ConstructorInfo ci = type.GetConstructor(System.Type.EmptyTypes); ;
            return (ISysFunConf)ci.Invoke(null);
        }
        public static ISysFunConf Create(bsSysFunc fc)
        {
            Type type = Type.GetType(" QyTech.Express.DataPart." + fc.FunLayout);
            ConstructorInfo ci = type.GetConstructor(System.Type.EmptyTypes); 
            ISysFunConf ifc=(ISysFunConf)ci.Invoke(null);
            ifc.fc = fc;
            return ifc;
        }
    }
}


