using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QyTech.Core.Common
{

    public enum PageDataStyle
        {
            STCommEdit, STTreeEdit, STGridEdit, PCCommEdit
        }
  

    public class PredefPageDataStyle
    {
        public static Dictionary<string, string> PageLayoutType = new Dictionary<string, string>();

        static PredefPageDataStyle()
        {
            PageLayoutType.Add("STCommEdit","新界面编辑");
            PageLayoutType.Add("STTreeEdit","单表树编辑");
            PageLayoutType.Add( "STGridEdit","表格编辑");
            PageLayoutType.Add( "PCGridPtList","主从网格");

        }
    }
}
