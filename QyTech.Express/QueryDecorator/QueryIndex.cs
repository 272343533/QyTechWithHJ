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

namespace QyTech.Express.QueryDecorator
{
    public class QueryIndex:IQueryDecorator
    {
       
        public override void CreateView()
        {
            QyTech.Express.IQueryView.Index obj = new QyTech.Express.IQueryView.Index();
            obj.Create(this);
         }

        public override void CreateController()
        {
            throw new NotImplementedException();
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
