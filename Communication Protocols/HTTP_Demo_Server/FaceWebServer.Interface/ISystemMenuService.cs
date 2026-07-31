using FaceWebServer.DTO.UI;

namespace FaceWebServer.Interface
{
    /// <summary>
    /// 界面菜单服务
    /// </summary>
    public interface ISystemMenuService : IBaseService
    {
        public MenusInfoResultDTO GetSystemMenu();
    }
}
