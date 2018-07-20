 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using SunMvcExpress.Common.Xml;
using SunMvcExpress.Core.Common;

using SunMvcExpress.Dao;
namespace QyTech.Express.CodeCreate.IQueryView
{
    public class FacQueryView : IFuncFac
    {
        public static IQueryViewCreate Create(string typeName)
        {
            Type type = Type.GetType("QyTech.Express.CodeCreate.IQueryView." + typeName);
            ConstructorInfo ci = type.GetConstructor(System.Type.EmptyTypes); ;
            return (IQueryViewCreate)ci.Invoke(null);
        }
    }
}

