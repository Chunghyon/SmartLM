using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.Interface
{
    /// <summary>
    /// 分页数据返回结构
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageResult<T>
    {
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<T> DataList { get; set; }
    }
}
