using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using SunMvcExpress.Common.Xml;
using SunMvcExpress.Core.Common;

using SunMvcExpress.Dao;
namespace QyTech.Express.CodeCreate.IDataView.IDataEditType
{
    public class FacDataEditCreate : IFuncFac
    {
        public static IDataEditType Create(string typeName)
        {
            Type type = Type.GetType("QyTech.Express.CodeCreate.IDataView.IDataEditType.DataEdit" + typeName);
            ConstructorInfo ci = type.GetConstructor(System.Type.EmptyTypes); ;
            return (IDataEditType)ci.Invoke(null);
        }
    }
}
