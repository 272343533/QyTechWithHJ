using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunMvcExpress.Dao;
using QyTech.Core.Common;
using System.Reflection;
using QyTech.Express.CodeCreate.Action;
using QyTech.Express;
using QyTech.Express.CodeCreate.IQueryView;
using System.IO;


namespace QyTech.Express.IQueryView
{
    public class Index :IQueryViewCreate
    {
        public void Create(IQueryDecorator iqd)
        {
            try
            {
                Ifc = iqd;
                 //bsSysFunc fc = bsSysfunc;
                //string CreateFileName = FILEPATHHEAD + bsSysfunc.FunContr + @"\" + ActionType.Index.ToString() + ".cshtml";
                //BackupFile(CreateFileName);

                // string includefilename = FILEPATHHEAD + fc.FunContr + @"\" + fc.FunContr + ".vqu";
                CreateFileName = FILEPATHHEAD + Ifc.fc.FunContr + @"\" + ActionType.Index.ToString() + ".cshtml";
                base.CreateFileHead(Ifc, CreateFileName);

                CreateQueryLeftConditions(Ifc);
                CreateQueryRightConditions(Ifc);

                sw.WriteLine("<div layoutH=\"0\" style=\"background:#fff\">");
                CreateQueryTopConditions(Ifc);
                base.CreatePageContentDivHead(Ifc.fc, sw);
                //iqd.DataPartDisp.CreateView(sw);
                base.CreatePageContentDivEnd(sw);
                sw.WriteLine("</div>");
          
 
                if (Ifc.FuncQueryLeft.Count > 0 || Ifc.FuncQueryRight.Count>0)
                {
                    IQueryViewCreate ViewObj = new IQueryViewCreate();
                    ViewObj.CreateJsForTreeRef(sw);
                    ViewObj.CreateJs(Ifc,sw);
                    
                }
                base.CreateIndexJsForDefautClick(sw);
            }
            catch (Exception ex)
            {
                log.Error("QyTech.Express.IQueryView.Index.Create:"+ex.Message);
            }
            finally
            {
                CreateFileEnd();
            }
        }

        private void CreateQueryLeftConditions(ISysFunConf ifc)
        {
            IQueryViewCreate ViewObj = new IQueryViewCreate();

            ViewObj.Ifc = ifc;
            ViewObj.sw = sw;

            if (ifc.FuncQueryLeft.Count > 0)
            {
                sw.WriteLine("<div layoutH=\"0\" style=\"float:left; display:block; overflow:auto; width:240px; border:solid 1px #CCC; line-height:21px; background:#fff\">");

                foreach (bsSysFuncQuery fq in Ifc.FuncQueryLeft)
                {
                    ViewObj = FacQueryView.Create(fq.QueryType);
                    ViewObj.Create(ifc, fq, sw);
                }
                sw.WriteLine("</div>");

            }
        }

        private void CreateQueryTopConditions(ISysFunConf ifc)
        {
            IQueryViewCreate ViewObj = new IQueryViewCreate();

            ViewObj.Ifc = ifc;
            ViewObj.sw = sw;

          
            if (ifc.FuncQueryTop.Count > 0)
            {
                base.CreatePagerHeaderHead(Ifc.fc, sw);
              
                ViewObj = FacQueryView.Create("Top");
                foreach (bsSysFuncQuery fq in Ifc.FuncQueryTop)
                {
                    ViewObj.Create(fq, sw);
                  
                }
                base.CreatePagerHeaderEnd(Ifc.fc, sw);

             }

          


        }

        private void CreateQueryRightConditions(ISysFunConf ifc)
        {
            IQueryViewCreate ViewObj = new IQueryViewCreate();

            ViewObj.Ifc = ifc;
            ViewObj.sw = sw;

            if (ifc.FuncQueryRight.Count > 0)
            {
                sw.WriteLine("<div layoutH=\"0\" style=\"float:right; display:block; overflow:auto; width:240px; border:solid 1px #CCC; line-height:21px; background:#fff\">");
                foreach (bsSysFuncQuery fq in Ifc.FuncQueryRight)
                {
                    ViewObj = FacQueryView.Create(fq.QueryType);
                    ViewObj.Create(ifc, fq, sw);
                }
                sw.WriteLine("</div>");

            }


        }

    }
}
