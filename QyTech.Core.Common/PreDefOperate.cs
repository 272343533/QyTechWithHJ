using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QyTech.Core.Common
{
    public enum ActionType
    {
        Index = 0, Add = 1, Del, Edit, Audit, Import, Export, Browse, Lookup, Suggest,
        ztreeDis = 100, ztreeAdd, ztreeDel, ztreeEdit, ztreeDrag,ztreeChooseNode,GridAdd,GridDel,GridEdit,PCEdit
    }
    public class PreDefOperate
    {
        private static SortedList<string, string> _LinkAddress = new SortedList<string, string>();

       
        public string this[int i]
        {
            get { return "a"; }
        }
        static PreDefOperate()
        {
            _LinkAddress.Add("Index", "Index");
            _LinkAddress.Add("Add", "Add");
            _LinkAddress.Add("Del", "Del?id={oid}");
            _LinkAddress.Add("Edit", "Add?id={oid}");
            _LinkAddress.Add("Audit", "Audit?id={oid}");
            _LinkAddress.Add("Import", "Import");//?弹出dialog，选择文件，上传，处理
            _LinkAddress.Add("Export", "Export");//?没有条件，应为动态给予条件http://blog.csdn.net/winter13292/article/details/7950491
            _LinkAddress.Add("Browse", "Add?id={oid}&BrowseFlag=true");
            _LinkAddress.Add("Lookup", "Lookup");
            _LinkAddress.Add("Suggest", "Suggest");
        }

        public static string SqlTypeMap2CSType(string sqlType)
        {
            if (sqlType == "int")
                return "int";
            else if (sqlType == "decimal")
                return "decimal";
            else if (sqlType == "datetime")
                return "datetime";
            else if (sqlType == "uniqueidentifier")
                return "Guid";
            return "string";
        }

        public static string LinkAddr(string key)
        {
            try
            {
                if (key.Substring(0, 1) == "{" && key.Substring(key.Length - 1, 1) == "}")
                {
                    return _LinkAddress[key.Substring(1, key.Length - 2)];
                }
                else
                {
                    return key;
                }
            }
            catch
            {
                return key;
            }
        }
        public static string LinkAddr(ActionType key)
        {
            return _LinkAddress[key.ToString()];
        }

    }

}
