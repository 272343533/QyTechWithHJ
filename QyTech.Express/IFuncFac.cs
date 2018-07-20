using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using SunMvcExpress.Common.Xml;
using SunMvcExpress.Core.Common;

using SunMvcExpress.Dao;

namespace QyTech.Express
{
   
    public class IFuncFac
    {

        public virtual ISysFunConf CreateFun(bsSysFunc fc)
        {
            ISysFunConf view = ReflectFactory.CreateDataPart(fc.FunLayout);
            view.fc = fc;
            return (ISysFunConf)view;
        }

         public virtual ISysFunConf CreateContr(string pagelayout, bsSysFunc fc)
        {
            ISysFunConf contr = ReflectFactory.CreateDataPart(pagelayout);
            contr.fc= fc;
            return (ISysFunConf)contr;
        }

        public virtual IQueryDecorator CreateQueryView(bsSysFunc fc )
        {
            IQueryDecorator view = ReflectFactory.CreateQueryView("QueryIndex");
            view.fc = fc; 
            return (IQueryDecorator)view;
        }

        public virtual ISysFunConf CreateDataView(bsSysFunc fc)
        {
            ISysFunConf view = ReflectFactory.CreateDataPart(fc.FunLayout);
            view.fc = fc;
            return (ISysFunConf)view;
        }

        protected class ReflectFactory
        {
            public static ISysFunConf CreateDataPart(string typeName)
            {
                //Type type = Type.GetType(XmlConfig.GetValue(typeName)+"Contr");
                Type type = Type.GetType("QyTech.Express.DataPart."+typeName);
                ConstructorInfo ci = type.GetConstructor(System.Type.EmptyTypes); ;
                return (ISysFunConf)ci.Invoke(null);
            }
            public static IQueryDecorator CreateQueryView(string typeName)
            {

                Type type = Type.GetType("QyTech.Express.QueryDecorator." + typeName);
                ConstructorInfo ci = type.GetConstructor(System.Type.EmptyTypes); ;
                return (IQueryDecorator)ci.Invoke(null);
            }

            public static IControllerCreate Create(string typeName)
            {
                //Type type = Type.GetType(XmlConfig.GetValue(typeName)+"Contr");
                Type type = Type.GetType("QyTech.Express.CodeCreate.Action." + typeName);
                ConstructorInfo ci = type.GetConstructor(System.Type.EmptyTypes); ;
                return (IControllerCreate)ci.Invoke(null);
            }
         
        }
       
    }
}
