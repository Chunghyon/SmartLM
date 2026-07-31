using FaceWebServer.DB.UI;
using FaceWebServer.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaceWebServer.Language;
using Microsoft.Extensions.Options;
using FaceWebServer.DTO.UI;

namespace FaceWebServer.Service
{
    public class SystemMenuService : BaseService, ISystemMenuService
    {
        private LanguageHandler _LanguageHandler;
        public SystemMenuService(DbContext context, IOptionsSnapshot<LanguageOption> lngopt) : base(context)
        {
            _LanguageHandler = lngopt.Value.GetCurrentLanguageHandler();
        }



        public MenusInfoResultDTO GetSystemMenu()
        {
            List<SystemMenuEntity> systemMenuEntities = new List<SystemMenuEntity>();

            systemMenuEntities = this.Query<SystemMenuEntity>(s => s.id > 0 && s.status == 1).ToList();

            systemMenuEntities.ForEach(entity => {
                entity.title = _LanguageHandler.GetSystemMenu(entity.LanguageCode);
            });

            SystemMenu rootNode = new SystemMenu()
            {
                Id = 0,
                icon = "",
                href = "",
                title = "根目录",
            };

            GetTreeNodeListByNoLockedDTOArray(systemMenuEntities.ToArray(), rootNode);

            MenusInfoResultDTO menusInfoResultDTO = new MenusInfoResultDTO();
            menusInfoResultDTO.menuInfo = rootNode.child;
            menusInfoResultDTO.logoInfo = new LogoInfo();
            menusInfoResultDTO.homeInfo = new HomeInfo();

            var node = systemMenuEntities.Find(x => x.id == 1);
            menusInfoResultDTO.homeInfo.href = node.href;
            menusInfoResultDTO.homeInfo.title = node.title;

            return menusInfoResultDTO;
        }

        /// <summary>
        /// 递归处理数据
        /// </summary>
        /// <param name="systemMenuEntities"></param>
        /// <param name="rootNode"></param>
        public static void GetTreeNodeListByNoLockedDTOArray(SystemMenuEntity[] systemMenuEntities, SystemMenu rootNode)
        {
            if (systemMenuEntities == null || systemMenuEntities.Count() <= 0)
            {
                return;
            }

            var childreDataList = systemMenuEntities.Where(p => p.pid == rootNode.Id);
            if (childreDataList != null && childreDataList.Count() > 0)
            {
                rootNode.child = new List<SystemMenu>();

                foreach (var item in childreDataList)
                {
                    SystemMenu treeNode = new SystemMenu()
                    {
                        Id = item.id,
                        icon = item.icon,
                        href = item.href,
                        title = item.title,
                        sort = item.sort
                    };
                    rootNode.child.Add(treeNode);
                }
                rootNode.child.Sort((x, y) => x.sort.CompareTo(y.sort));

                foreach (var item in rootNode.child)
                {
                    GetTreeNodeListByNoLockedDTOArray(systemMenuEntities, item);
                }
            }
        }
    }
}
