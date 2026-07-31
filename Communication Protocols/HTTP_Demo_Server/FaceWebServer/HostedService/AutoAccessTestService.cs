
using Microsoft.Extensions.Configuration;
using FaceWebServer.DTO.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using FaceWebServer.Interface;
using System;
using System.Linq;
using FaceWebServer.Service;
using System.Collections.Generic;
using FaceWebServer.DB;
using FaceWebServer.DB.Table;
using FaceWebServer.DTO.Access;

namespace DeviceProtocolServer.HostedService
{

    public static class AutoAccessTestServiceExtensions
    {
        public static IServiceCollection AddAutoAccessTestService(this IServiceCollection services,
            IConfiguration config)
        {
            services.Configure<AutoAccessTestOptions>(config);
            services.AddHostedService<AutoAccessTestService>();
            return services;
        }
    }



    /// <summary>
    /// 自动权限测试服务
    /// </summary>
    public class AutoAccessTestService : BackgroundService
    {
        private readonly ILogger<AutoAccessTestService> _logger;
        private IOptionsMonitor<AutoAccessTestOptions> _MonitorAutoAccessTestOptions;
        private IFaceDriveService _DeviceDB;
        private ICacheService _CacheService;
        private IDeviceAccessService _AccessServic;

        public AutoAccessTestService(
            ILogger<AutoAccessTestService> log,
            IOptionsMonitor<AutoAccessTestOptions> monAutoOptions,
            IFaceDriveService deviceDB,
            IDeviceAccessService accessService,
            ICacheService cacheService
            )
        {
            _logger = log;
            _MonitorAutoAccessTestOptions = monAutoOptions;
            _DeviceDB = deviceDB;
            _CacheService = cacheService;
            _AccessServic = accessService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //throw new System.NotImplementedException();
            _logger.LogInformation("自动权限测试服务开启运行！");
            var oRunTime = DateTime.Now.AddHours(-1);
            bool bIsDelete = false;
            bool bRun = false;
            DateTime DevicePassEnd = DateTime.Now.AddYears(5);

            DeviceAccessAddDTO addDto = new()
            {
                AccessType = 0,
                ExpirationDate = DevicePassEnd,
                OpenTimes = 65535,
                KeepOpen = 0,
                Timegroup = 1,
            };


            do
            {
                bRun = false;
                var peopleDic = _CacheService.GetPeopleDictionary();
                var options = _MonitorAutoAccessTestOptions.CurrentValue;
                if (options.SNList.Count > 0 && peopleDic != null && peopleDic.Count > 0)
                {
                    var SNList = options.SNList;
                    var oDiff = (DateTime.Now - oRunTime).TotalMinutes;
                    if (oDiff >= 3)//10分钟
                    {
                        var onlineStatus = _DeviceDB.GetDeviceOnlineStatus(SNList.ToList());

                        if (onlineStatus.Count > 0)
                        {
                            oRunTime = DateTime.Now;


                            foreach (var device in onlineStatus)
                            {
                                if (device.LastKeepaliveTime != DateTime.MinValue)
                                {
                                    var iOnlinetime = (DateTime.Now - device.LastKeepaliveTime).TotalSeconds;
                                    if (iOnlinetime < 60)//保活包在2分钟内，可判定为在线
                                    {
                                        List<int> peopleIDs = new List<int>();
                                        if (options.PCodes != null)
                                        {
                                            peopleIDs = new List<int>();
                                            foreach (var pCode in options.PCodes)
                                            {
                                                var pDto = _CacheService.GetPeopleCache(pCode);
                                                if (pDto != null)
                                                    peopleIDs.Add(pDto.ID);
                                            }
                                        }

                                        if (peopleIDs.Count == 0)
                                            peopleIDs = peopleDic.Select(x => x.Value.ID).Take(20).ToList();

                                        var deviceIDs = new List<int>() { device.ID };

                                        bRun = true;
                                        if (bIsDelete)
                                        {
                                            _logger.LogInformation($" 开始自动删除权限，SN：{device.SN} ,人员数量： {peopleIDs.Count}");
                                            await _AccessServic.DeleteAccess(new DeviceAccessDeleteDTO()
                                            {
                                                DeviceIDs = deviceIDs,
                                                PeopleIDs = peopleIDs
                                            }
                                            );
                                        }
                                        else
                                        {
                                            addDto.DeviceIDs = deviceIDs;
                                            addDto.PeopleIDs = peopleIDs;
                                            _logger.LogInformation($" 开始自动授权，SN：{device.SN} ,人员数量： {peopleIDs.Count}");
                                            await _AccessServic.AddAccess(addDto);
                                        }
                                    }

                                }
                            }
                        }






                    }

                    if (bRun) bIsDelete = !bIsDelete;
                }

                peopleDic = null;
                await Task.Delay(100);
            } while (!stoppingToken.IsCancellationRequested);

            _logger.LogInformation("自动权限测试服务已取消！");
        }

    }
}
