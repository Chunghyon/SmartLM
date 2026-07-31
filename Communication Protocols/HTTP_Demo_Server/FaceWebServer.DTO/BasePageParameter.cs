using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO
{
    /// <summary>
    /// 分页请求的基础参数
    /// </summary>
    public class BasePageParameter
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; } = 1;
        /// <summary>
        /// 页面最大行数
        /// </summary>
        public int PageSize { get; set; } = 50;
        /// <summary>
        /// 是否正序
        /// </summary>
        public bool IsAsc { get; set; } = false;
    }
}
