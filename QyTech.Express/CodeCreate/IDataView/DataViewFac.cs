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
     public class DataViewFac : IFuncFac
    {
        public static IDataViewCreate Create(string typeName)
        {
            Type type = Type.GetType("QyTech.Express.CodeCreate.IDataView." + typeName);
            ConstructorInfo ci = type.GetConstructor(System.Type.EmptyTypes); ;
            return (IDataViewCreate)ci.Invoke(null);
        }
    }
}
