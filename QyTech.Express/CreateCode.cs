using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunMvcExpress.Dao;
using SunMvcExpress.Core;
using log4net;
using System.Reflection;
using QyTech.Core.Common;
using QyTech.Express.DataPart;
using QyTech.Express.CodeCreate;

namespace QyTech.Express
{
    public class CreateCode
    {
      
        public static bool CreateContr(bsSysFunc  fc)
        {
            try
            {
                IFuncFac pf = new IFuncFac();
                if ((fc.FunLayout != null && fc.FunLayout.Length > 0))
                {
                    PageDataStyle lt = (PageDataStyle)Enum.Parse(typeof(PageDataStyle), fc.FunLayout);
                    ISysFunConf ifc = pf.CreateContr(lt.ToString(), fc);
                    ifc.CreateController();

                    IBllCreate bllobj = new IBllCreate();
                    bllobj.Create(ifc);
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }
        public static bool CreateView(bsSysFunc fc)
        {
            try
            {
                IFuncFac pf = new IFuncFac();
                if ((fc.FunLayout != null && fc.FunLayout.Length > 0))
                {
                    PageDataStyle lt = (PageDataStyle)Enum.Parse(typeof(PageDataStyle), fc.FunLayout);
                    ISysFunConf ifc_datadisp = pf.CreateDataView(fc);

                    if (lt!= PageDataStyle.STTreeEdit)
                    {
                        IQueryDecorator ifc = pf.CreateQueryView(fc);
                        ifc.DataPartDisp = ifc_datadisp;
                        ifc.CreateView();
                    }

                    ifc_datadisp.CreateView();

                  
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }


        //private  ISysFunConf CreateDataView(string pagelayout, bsSysFunc fc)
        //{
        //    ISysFunConf view = ReflectFactory.CreateDataPart(pagelayout);
        //    view.fc = fc;
        //    return (ISysFunConf)view;

        //}
    }
}
