using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunMvcExpress.Dao;
using QyTech.Core.Common;

namespace QyTech.Express.CodeCreate.Action
{
    public class ztreeDis : IControllerCreate
    {
        public override void Create()
        {
            sw.WriteLine("\t" + "\t" + "public ActionResult Index()");
            sw.WriteLine("\t" + "\t" + "{");
            sw.WriteLine("\t" + "\t" + "    return View();");
            sw.WriteLine("\t" + "\t" + "}");
            sw.WriteLine("");
            sw.WriteLine("\t" + "\t" + "public ActionResult " + ActionType.ztreeDis.ToString() + "()");
            sw.WriteLine("\t" + "\t" + "{");

            sw.WriteLine("\t" + "\t" + "    List<" + Ifc.fc.ListDbObj + "> objs =EntityManager<" + Ifc.fc.ListDbObj + ">.GetListNoPaging<" + Ifc.fc.ListDbObj + ">(\"\",\"" + Ifc.fc.Orderbys + "\");");
            sw.WriteLine("\t" + "\t" + "    var treelist = new List<ZTreeNode>();");

            sw.WriteLine("\t" + "\t" + "    foreach (var cc in objs)");
            sw.WriteLine("\t" + "\t" + "    {");
            sw.WriteLine("\t" + "\t" + "        try");
            sw.WriteLine("\t" + "\t" + "        {");
            sw.WriteLine("\t" + "\t" + "            var treenode = new ZTreeNode();");
            sw.WriteLine("\t" + "\t" + "            treenode.Id = cc.Id;");
            sw.WriteLine("\t" + "\t" + "            treenode.Name = cc.Name;");
            sw.WriteLine("\t" + "\t" + "            treenode.PId =(Guid) cc.PId;");
            //sw.WriteLine("\t" + "\t" + "            treenode." + Ifc.fc.TPK + " = cc." + Ifc.fc.TPK + ";");
            //sw.WriteLine("\t" + "\t" + "            treenode.Name = cc." + Ifc.fc.NotNullField + ";");
            //sw.WriteLine("\t" + "\t" + "            treenode.P" + Ifc.fc.TPK + " =(Guid) cc.P" + Ifc.fc.TPK + ";");

            sw.WriteLine("\t" + "\t" + "            treenode.noAddBtn = false;");
            sw.WriteLine("\t" + "\t" + "            treenode.noEditBtn = false;");
            sw.WriteLine("\t" + "\t" + "            treenode.noRemoveBtn = false;");
           
            sw.WriteLine("\t" + "\t" + "            if (objs.Count < 50)");
            sw.WriteLine("\t" + "\t" + "            {");
            sw.WriteLine("\t" + "\t" + "                treenode.open = true;");
            sw.WriteLine("\t" + "\t" + "            }");
            
            sw.WriteLine("\t" + "\t" + "            treenode.Url = \"\";");
            sw.WriteLine("\t" + "\t" + "            treenode.target = \"\";");
            sw.WriteLine("\t" + "\t" + "            treenode.Checked = true;");
            sw.WriteLine("\t" + "\t" + "            treelist.Add(treenode);");
            sw.WriteLine("\t" + "\t" + "        }");
            sw.WriteLine("\t" + "\t" + "        catch { }");
            sw.WriteLine("\t" + "\t" + "    }");
            sw.WriteLine("\t" + "\t" + "    var source = from c in treelist select c;");
            sw.WriteLine("\t" + "\t" + "    return Json(source, JsonRequestBehavior.AllowGet);");
            sw.WriteLine("\t" + "\t" + "}");
            sw.WriteLine("");


            sw.WriteLine("\t" + "\t" + "public JsonResult " + ActionType.ztreeChooseNode.ToString() + "(Guid " + Ifc.fc.TPK + ")");
            sw.WriteLine("\t" + "\t" + "{");
            sw.WriteLine("\t" + "\t" + "    var info = EntityManager<" + Ifc.fc.TName + ">.GetByPk<" + Ifc.fc.TName + ">(\"" + Ifc.fc.TPK + "\", " + Ifc.fc.TPK + ");");
          
            sw.WriteLine("\t" + "\t" + "    JsonResult json = new JsonResult();");
            sw.WriteLine("\t" + "\t" + "    json.Data = new");
            sw.WriteLine("\t" + "\t" + "    {");
            //循环每个字段
            foreach (bsFuncField ff in Ifc.dbFunfields)
            {
                if (ff.FName==Ifc.dbFunfields[Ifc.dbFunfields.Count-1].FName)
                    sw.WriteLine("\t" + "\t" + "        " + ff.FName + " = info." + ff.FName );
                else
                    sw.WriteLine("\t" + "\t" + "        " + ff.FName + " = info." + ff.FName + ",");
            
            }//循环结束

            sw.WriteLine("\t" + "\t" + "    };");

            sw.WriteLine("\t" + "\t" + "    return Json(json, JsonRequestBehavior.AllowGet);");
           sw.WriteLine("\t" + "\t" + "}");

        }

        public override void CreateSaveST()
        {
            Edit obj = new Edit();
            obj.sw = sw;
            obj.Create();
            base.CreateSaveST();

            //sw.WriteLine("");
            ////把richTextBox1中的内容写入文件
            //sw.WriteLine("\t" + "\t" + "public string SaveForAddOrEdit(FormCollection fc,AddorModify addmodiflag)");
            //sw.WriteLine("\t" + "\t" + "{");

            //sw.WriteLine("\t" + "\t" + "    " + Ifc.fc.TName + " obj = new " + Ifc.fc.TName + "();");
            //sw.WriteLine("\t" + "\t" + "    obj=EntityManager<" + Ifc.fc.TName + ">.GetByPk<" + Ifc.fc.TName + ">(\"" + Ifc.fc.TPK + "\",fc[\"" + Ifc.fc.TName + "_" + Ifc.fc.TPK + "\"].ToString());");
            //sw.WriteLine("\t" + "\t" + "    if (fc != null)");
            //sw.WriteLine("\t" + "\t" + "    {");
            //foreach (bsFuncField ff in Ifc.dbFunfields)
            //{
            //    if (ff.FName == Ifc.fc.TPK)
            //        continue;

            //    if ((bool)ff.FormVisible && !(bool)ff.FormEditable)//可编辑的才需要
            //        continue;

            //    if (ff.Type == "varchar")
            //    {
            //        sw.WriteLine("\t" + "\t" + "        if (!(fc[\"" + Ifc.fc.TName + "_" + ff.FName + "\"].ToString().Equals(\"\") || fc[\"" + Ifc.fc.TName + "_" + ff.FName + "\"].ToString() == null))");
            //        sw.WriteLine("\t" + "\t" + "        {");
            //        sw.WriteLine("\t" + "\t" + "            obj." + ff.FName + " = fc[\"" + Ifc.fc.TName + "_" + ff.FName + "\"].ToString();");
            //        sw.WriteLine("\t" + "\t" + "        }");
            //    }
            //    else
            //    {
            //        sw.WriteLine("\t" + "\t" + "        if (!(fc[\"" + Ifc.fc.TName + "_" + ff.FName + "\"].ToString() == null))");
            //        sw.WriteLine("\t" + "\t" + "        {");
            //        if (ff.Type == "date" || ff.Type == "datetime")
            //        {
            //            sw.WriteLine("\t" + "\t" + "            obj." + ff.FName + " = Convert.ToDateTime(fc[\"" + Ifc.fc.TName + "_" + ff.FName + "\"]);");
            //        }
            //        else if (ff.Type == "decimal")
            //        {
            //            sw.WriteLine("\t" + "\t" + "            obj." + ff.FName + " = Convert.ToDateTime(fc[\"" + Ifc.fc.TName + "_" + ff.FName + "\"]);");
            //        }
            //        else if (ff.Type.IndexOf("int") >= 0)
            //        {
            //            sw.WriteLine("\t" + "\t" + "            obj." + ff.FName + " = int.Parse(fc[\"" + Ifc.fc.TName + "_" + ff.FName + "\"]);");
            //        }
            //        else if (ff.Type == "bit")
            //        {
            //            sw.WriteLine("\t" + "\t" + "            obj." + ff.FName + " = bool.Parse(fc[\"" + Ifc.fc.TName + "_" + ff.FName + "\"]);");
            //        }
            //        else if (ff.Type == "uniqueidentifier")
            //        {
            //            sw.WriteLine("\t" + "\t" + "            obj." + ff.FName + " = new Guid(fc[\"" + Ifc.fc.TName + "_" + ff.FName + "\"].ToString());");
            //        }
            //        sw.WriteLine("\t" + "\t" + "        }");
            //    }
            //}
            //sw.WriteLine("\t" + "\t" + "    }");
            //if (HaveSpecialFields(SpeicalFields.DelStatus))
            //{
            //    sw.WriteLine("\t" + "\t" + "\t" + "    obj.delStatus = false;");
            //}
            ////如果有分类属性，则在此赋值
            //if (Ifc.fc.Filterfield != null && Ifc.fc.Filterfield.Trim().Length > 0)
            //{
            //    sw.WriteLine("\t" + "\t" + "\t" + "    obj." + Ifc.fc.Filterfield + " = \"" + Ifc.fc.Filtervalue + "\";");
            //}


            //if (Ifc.fc.SaveOperator != null && (bool)Ifc.fc.SaveOperator)
            //{
            //    sw.WriteLine("\t" + "\t" + "obj.Operater = Login_Nick_Name;");
            //    sw.WriteLine("\t" + "\t" + "obj.Updatedt = DateTime.Now;");
            //}

            //sw.WriteLine("\t" + "\t" + "    string errMsg=EntityManager<" + Ifc.fc.TName + ">.Modify(obj);");
            //sw.WriteLine("\t" + "\t" + "    return errMsg;");
            //sw.WriteLine("\t" + "\t" + "}");

        }
    }
}
