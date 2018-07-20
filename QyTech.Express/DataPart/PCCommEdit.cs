using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QyTech.Express;
using SunMvcExpress.Dao;
using QyTech.Express.CodeCreate.Action;

namespace QyTech.Express.DataPart
{
    public class PCCommEdit : ISysFunConf
    {
        public override void CreateController()
        {
            //CreateContrObj = new IControllerCreate();
            //CreateContrObj.Ifc = ifc;
            try
            {
                CreateContrObj.CreateFileHead();

                foreach (bsSysFuncOper fo in FuncOpers)
                {
                    ActionFac.Create(fo.OperName);
                }


                //创建json对象
               // base.CreateJsonResult(sw);

              
            }
            finally
            {
                CreateContrObj.CreateFileEnd();
            }
        }
        public override void CreateView()
        {
            CreateViewObj = new IDataViewCreate();
            CreateViewObj.Create(this);
        }

        public override void CreateView(System.IO.StreamWriter sw)
        { 
            throw new NotImplementedException(); 
        }
        public override void CreateDataInIndex(ISysFunConf ifc, System.IO.StreamWriter sw)
        {
            throw new NotImplementedException();
        }
    }
}
