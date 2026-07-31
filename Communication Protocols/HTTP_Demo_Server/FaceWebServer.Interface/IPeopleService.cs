using FaceWebServer.DB.Table;
using FaceWebServer.DTO.People;
using FaceWebServer.Utility.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaceWebServer.Interface
{
    /// <summary>
    /// 人员操作服务
    /// </summary>
    public interface IPeopleService : IBaseService
    {

        /// <summary>
        /// 分页查询人员信息
        /// </summary>
        /// <returns></returns>
        PageResult<PeopleQueryResultDTO> Query(PeopleQueryDTO queryDTO);

        /// <summary>
        /// 获取一个新的自动用户号
        /// </summary>
        /// <returns></returns>
        long GetNewAutoUserID();

        void UpdateAutoUserID(long iNewUserID);

        /// <summary>
        /// 导入人员
        /// </summary>
        /// <param name="peoples"></param>
        Task InputPeople(List<People> peoples);


        /// <summary>
        /// 增加人员
        /// </summary>
        /// <param name="newPeople"></param>
        /// <returns></returns>
        Task<JsonResultModel> AddNew(People newPeople, Func<People, JsonResultModel> imageCallblack);

        /// <summary>
        /// 删除人员
        /// </summary>
        Task DeletePeople(HashSet<int> ids);

        /// <summary>
        /// 更新人员
        /// </summary>
        /// <param name="newPeople"></param>
        /// <returns></returns>
        Task<JsonResultModel> UpdatePeople(People newPeople, Func<People, JsonResultModel> imageCallblack, bool bUpdateAccess = true);

        /// <summary>
        /// 删除所有人员
        /// </summary>
        void ClearPeople();


        /// <summary>
        /// 根据用户号获取用户信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        People GetPeopleByUserID(long userID);


        /// <summary>
        /// 加载特征码
        /// </summary>
        /// <param name="people"></param>
        /// <returns></returns>
        Task LoadFeatureCode(PeopleDTO people);
        /// <summary>
        /// 删除特征码文件
        /// </summary>
        /// <param name="people"></param>
        /// <returns></returns>
        Task DeleteFeatureCode(PeopleDTO people);
        /// <summary>
        /// 保存人员特征码到本地文件
        /// </summary>
        /// <param name="people"></param>
        /// <returns></returns>
        Task SaveFeatureCode(PeopleDTO people);

        /// <summary>
        /// 注册人员识别凭证
        /// </summary>
        /// <param name="enrollMediaData"></param>
        /// <returns></returns>
        Task<JsonResultModel> EnrollUserMediaData(EnrollUserMediaDataDTO enrollMediaData);
    }
}
