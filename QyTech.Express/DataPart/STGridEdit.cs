using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunMvcExpress.Dao;
using QyTech.Express.CodeCreate.Action;
using QyTech.Express;
using QyTech.Express.CodeCreate.IDataView;

namespace QyTech.Express.DataPart
{
    public class STGridEdit : ISysFunConf
    {
        public override void CreateController()
        {
            log.Error("for test useful");
            try
            {
                System.IO.StreamWriter sw;
                //Index action
                CreateContrObj = new GridIndex();
                CreateContrObj.Ifc = this;
                CreateContrObj.CreateFileHead();
                sw = CreateContrObj.sw;

                CreateContrObj.Create();


                //other action only for add delete edit
                //FuncOpers=CreateContrObj.FunOperates;
                bool CreateSaveFlag = false;
                string ActionName = "";
                foreach (bsSysFuncOper fo in FuncOpers)
                {
                    if (fo.LinkAddr.Substring(0, 1) == "{")
                        ActionName = fo.LinkAddr.Substring(1, fo.LinkAddr.Length - 2);
                    else
                        ActionName = fo.LinkAddr;
                    if ((ActionName == "Add") && !CreateSaveFlag)
                    {
                        CreateContrObj.CreateSaveST();
                        CreateSaveFlag = true;
                    }
                    else if ((ActionName == "GridAdd") && !CreateSaveFlag)
                    {
                        CreateContrObj = ActionFac.Create("GridEdit");
                        CreateContrObj.sw = sw;
                        CreateContrObj.Ifc = this;
                        CreateContrObj.Create();
                    }
                    CreateContrObj = ActionFac.Create(ActionName);
                    CreateContrObj.sw = sw;
                    CreateContrObj.Ifc = this;
                    CreateContrObj.Create();
                }

                base.CreateJsonResult(sw);

              
            }
            catch (Exception ex)
            {
                log.Error("CreateController:" + ex.Message);
                throw new Exception(ex.Message);
            }
            finally
            {
                CreateContrObj.CreateFileEnd();
            }
        }
        public override void CreateView()
        {
            log.Error("for test veiew");
            try
            {
                IDataViewCreate vObj = new IDataViewCreate();
                vObj = DataViewFac.Create(fc.FunLayout);
                vObj.Create(this);
            }
            catch (Exception ex)
            {
                log.Error("CreateView:" + ex.Message);
                throw new Exception(ex.Message);
            }
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
