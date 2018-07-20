using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunMvcExpress.Core.Common
{

    public enum PageLayType
        {
            STCommEdit, STTreeEdit, STGridEdit, PCGridPtList, LListRList, LTreeRList
        }
  

    public class PreDefPageLayout
    {
        public static SortedList<string, string> PageLayoutType = new SortedList<string, string>();

        static PreDefPageLayout()
        {
            PageLayoutType.Add("单表普通编辑","STCommEdit");
            PageLayoutType.Add("单表左树右编辑","STTreeEdit");
            PageLayoutType.Add("单表表格编辑", "STGridEdit");
            PageLayoutType.Add("主从网格列表", "PCGridPtList");
            //PageLayoutType.Add("左列表过滤右列表", "LListRList");
            //PageLayoutType.Add("左树状过滤右列表", "LTreeRList");


            PageLayoutType.Add("左右结构", "LeftRight");
            PageLayoutType.Add("左中右结构", "LeftCenterRight");
            PageLayoutType.Add("上下结构", "TopBotton");
            PageLayoutType.Add("上中下结构", "TopCenterBottom");

        }
    }
}
