using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QyTech.Express.CodeCreate;
using SunMvcExpress.Dao;
using QyTech.Core.Common;


namespace QyTech.Express.CodeCreate.Action
{
    public class GridIndex:IControllerCreate
    {
        public override void Create()
        {
            sw.WriteLine("");
            string PFK_type = "Guid";
            foreach (bsFuncField ff in Ifc.dbFunfields)
            {
                if (ff.FName == Ifc.fc.PFK)
                {
                    PFK_type = ff.Type;
                    break;
                }
            }
            PFK_type = PreDefOperate.SqlTypeMap2CSType(PFK_type);
            if (Ifc.fc.PFK != null && Ifc.fc.PFK != "")
            {
                if (PFK_type == "Guid"|| PFK_type=="string")
                {
                    sw.WriteLine("\t" + "\t" + "public ActionResult " + ActionType.Index.ToString() + "(string " + Ifc.fc.PFK + ")");
                }
                else
                {
                    sw.WriteLine("\t" + "\t" + "public ActionResult " + ActionType.Index.ToString() + "(" + PFK_type + " " + Ifc.fc.PFK + ")");
               
                }
            }
            else
                sw.WriteLine("\t" + "\t" + "public ActionResult " + ActionType.Index.ToString() + "()");
            sw.WriteLine("\t" + "\t" + "{");
            foreach(bsFuncField ff in Ifc.dbFunfields)
            {
                if (ff.LookupLink.Length>0 && ff.LookupLink.Substring(0, 1) != "{")
                {
                    string[] splits=ff.LookupLink.Split(new char[] {'(',')',' ','{','}'});
                    if (PFK_type == "Guid")
                    {
                        sw.WriteLine("\t" + "\t" + "\t" + "ViewBag.vb" + ff.FName + "s=" + splits[0] + "(Guid.Parse(" + splits[2] + "));");
                    }
                    else
                    {
                        sw.WriteLine("\t" + "\t" + "\t" + "ViewBag.vb" + ff.FName + "s=" + splits[0] + "(" + splits[3] + ");");
                    }

                }
            }
            sw.WriteLine("\t" + "\t" + "\t" + "List<" + Ifc.fc.TName + "> objs=new List<" + Ifc.fc.TName + ">();");

            sw.WriteLine("\t" + "\t" + "\t" + "ViewBag." + Ifc.fc.PFK + "=" + Ifc.fc.PFK + ";");

            if (PFK_type=="int" )                                                                                                                                      
            {
                sw.WriteLine("\t" + "\t" + "\t" + "objs = EntityManager<" + Ifc.fc.TName + ">.GetListNoPaging<" + Ifc.fc.TName + ">(\"" + Ifc.fc.PFK + "=\"+" + Ifc.fc.PFK + ".ToString(),\"\");");
            }
            else
            {
                sw.WriteLine("\t" + "\t" + "\t" + "objs = EntityManager<" + Ifc.fc.TName + ">.GetListNoPaging<" + Ifc.fc.TName + ">(\"" + Ifc.fc.PFK + "='\"+" + Ifc.fc.PFK + ".ToString()+\"'\",\"\");");
            }

            sw.WriteLine("\t" + "\t" + "\t" + "return View(objs);");
            sw.WriteLine("\t" + "\t" + "}");
        }

       
    }
}
