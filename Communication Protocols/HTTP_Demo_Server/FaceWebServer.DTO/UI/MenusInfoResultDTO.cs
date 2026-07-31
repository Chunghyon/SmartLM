using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.UI
{
    /// <summary>
    /// 菜单结果对象
    /// </summary>
    public class MenusInfoResultDTO
    {
        /// <summary>
        /// 权限菜单树
        /// </summary>
        public List<SystemMenu> menuInfo { get; set; }

        /// <summary>
        /// logo
        /// </summary>
        public LogoInfo logoInfo { get; set; }

        /// <summary>
        /// Home
        /// </summary>
        public HomeInfo homeInfo { get; set; }
    }
}
