using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QyTech.Core.BLL
{

    public class ResultInfo
    {
        /// <summary>
        /// 调用结果是否成功
        /// </summary>
        public bool Result { set; get; }
        /// <summary>
        /// 调用结果返回的数据
        /// </summary>
        public string Data { set; get; }
        /// <summary>
        /// 调用结果返回的相关提示信息
        /// </summary>
        public string Msg { set; get; }


    }

    public class ResultInfo_Form<T> where T : new()
    {
        /// <summary>
        /// 调用结果是否成功
        /// </summary>
        public bool Result { set; get; }
        /// <summary>
        /// 调用结果返回的数据
        /// </summary>
        public T Data { set; get; }
        /// <summary>
        /// 调用结果返回的相关提示信息
        /// </summary>
        public string Msg { set; get; }


    }
    public class ResultInfo_Page<T> where T : new()
    {
        /// <summary>
        /// 调用结果是否成功
        /// </summary>
        public bool Result { set; get; }
        /// <summary>
        /// 调用结果返回的数据
        /// </summary>
        public List<T> Data { set; get; }
        /// <summary>
        /// 调用结果返回的相关提示信息
        /// </summary>
        public string Msg { set; get; }

        /// <summary>
        /// 获取记录的总数量
        /// </summary>
        public int totalCount { set; get; }
        /// <summary>
        /// 第几页
        /// </summary>
        public int pageNo { set; get; }

         /// <summary>
        /// 每页的行数
        /// </summary>
        public int numPerPage { set; get; }
    }
}
