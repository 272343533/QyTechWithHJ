using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QyTech.Express.CodeCreate;
using SunMvcExpress.Dao;
using QyTech.Core.Common;
using System.IO;

namespace QyTech.Express.CodeCreate.IQueryView
{
    class ChooseSimpTree : IQueryViewCreate
    {
        public override void Create(ISysFunConf ifc, bsSysFuncQuery fq,StreamWriter sw)
        {
           
            sw.WriteLine("\t" + "<div style=\"float:left;\">");
            sw.WriteLine("\t" + "    <div id=\"divlefttreeFor" + ifc.fc.FunContr + "\" layouth=\"2\" style=\"width: 238px; border: solid 1px #CCC; overflow: auto\">");
            sw.WriteLine("\t" + "        <ul id=\"treeFor" + ifc.fc.FunContr + fq.FFName+ "\" class=\"ztree\"></ul>");
            //sw.WriteLine("\t" + "        <a  id=\"atree" + ifc.fc.FunContr + fq.FFName + "\" href=\"\" target=\"ajax\" rel=\"bsEmployeeMainjsbx\"></a>");
            sw.WriteLine("\t" + "    </div>");
            sw.WriteLine("\t" + "</div>");
        }
    
    }
}
