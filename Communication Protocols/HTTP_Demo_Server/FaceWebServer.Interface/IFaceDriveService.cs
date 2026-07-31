using FaceWebServer.DB.Table;
using FaceWebServer.DTO;
using FaceWebServer.DTO.Device;
using FaceWebServer.Utility.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FaceWebServer.Interface
{
    /// <summary>
    /// 设备操作服务
    /// </summary>
    public interface IFaceDriveService : IBaseService
    {
        DbSet<DeviceDetail> GetDBSet();


        /// <summary>
        /// 分页查询设备信息
        /// </summary>
        /// <returns></returns>
        PageResult<DeviceQueryResultDTO> Query(DeviceQueryDTO queryDTO);

        /// <summary>
        /// 根据SN获取设备在线状态
        /// </summary>
        List<DeviceOnlineStatusQueryResultDTO>  GetDeviceOnlineStatus(List<string> SNList);


        /// <summary>
        /// 根据设备SN获取设备信息
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        DeviceDetail GetDeviceDetail(string SN);


        /// <summary>
        /// 根据Id获取设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DeviceDetail GetDeviceDetail(int id);

        /// <summary>
        /// 添加一个新设备
        /// </summary>
        /// <param name="oDevice"></param>
        /// <returns></returns>
        Task<DeviceDetail> Add(DeviceDetail oDevice);

        /// <summary>
        /// 更新设备信息
        /// </summary>
        /// <param name="oDevice"></param>
        /// <returns></returns>
        JsonResultModel Update(DeviceDetail oDevice);

        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="IDList"></param>
        /// <returns></returns>
        Task<bool> Delete(List<int> IDList);

        




        /// <summary>
        /// 设置设备出厂默认值
        /// </summary>
        void SaveDefaultValue(SetDefaultValueRequestDTO par);

        /// <summary>
        /// 获取设备默认参数的Json
        /// </summary>
        /// <returns></returns>
        string GetDefaultValue(string sProtocol);

        /// <summary>
        /// 执行初始化设备完毕
        /// </summary>
        /// <param name="iDeviceID"></param>
        Task FormatDevice(int iDeviceID);

        /// <summary>
        /// 更新设备固件版本
        /// </summary>
        /// <param name="url"></param>
        /// <param name="ver"></param>
        /// <param name="softMD5"></param>
        /// <param name="deviceID"></param>
        Task UpdateDeviceSoft(string url, string ver, string softMD5, int deviceID);


        /// <summary>
        /// 设备更换SN
        /// </summary>
        /// <param name="oDBDevice"></param>
        void ReplaceDeviceSN(DeviceDetail oDBDevice,string sNewSN);

        /// <summary>
        /// 更新所有设备的上传状态为0，标记为未上传
        /// </summary>
        /// <returns></returns>
        Task UpdateAllDeviceUploadStatus();

        /// <summary>
        /// 对设备进行远程抓拍
        /// </summary>
        Task<JsonResultModel> RemoteSnapshoot(RemoteSnapshootDTO dto);
    }
}
