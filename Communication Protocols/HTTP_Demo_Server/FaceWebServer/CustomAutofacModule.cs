using Autofac;
using FaceWebServer.DB.Table;
using FaceWebServer.DTO;
using FaceWebServer.DTO.AlarmClock;
using FaceWebServer.DTO.People;
using FaceWebServer.DTO.Record;
using FaceWebServer.DTO.TimeGroup;
using FaceWebServer.Interface;
using FaceWebServer.Service;

namespace DeviceProtocolServer
{
    public class CustomAutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            //var assembly = this.GetType().GetTypeInfo().Assembly;
            //var builder = containerBuilder;
            //var manager = new ApplicationPartManager();
            //manager.ApplicationParts.Add(new AssemblyPart(assembly));
            //manager.FeatureProviders.Add(new ControllerFeatureProvider());
            //var feature = new ControllerFeature();
            //manager.PopulateFeature(feature);
            //builder.RegisterType<ApplicationPartManager>().AsSelf().SingleInstance();
            //builder.RegisterTypes(feature.Controllers.Select(ti => ti.AsType()).ToArray()).PropertiesAutowired();



            containerBuilder.RegisterType<UserDetail>().As<UserDetail>().InstancePerLifetimeScope();
            //containerBuilder.RegisterType<FaceDBContext>().As<DbContext>();
            containerBuilder.RegisterType<RecordService>().As<IRecordService>().PropertiesAutowired();
            containerBuilder.RegisterType<SystemMenuService>().As<ISystemMenuService>().PropertiesAutowired();
            containerBuilder.RegisterType<UserService>().As<IUserService>().PropertiesAutowired();
            containerBuilder.RegisterType<FaceDriveService>().As<IFaceDriveService>().PropertiesAutowired();
            containerBuilder.RegisterType<PeopleService>().As<IPeopleService>().PropertiesAutowired();
            containerBuilder.RegisterType<TimeGroupService>().As<ITimeGroupService>().PropertiesAutowired();
            containerBuilder.RegisterType<DeviceRemoteService>().As<IDeviceRemoteService>().PropertiesAutowired();
            containerBuilder.RegisterType<ConnectIOLogService>().As<IConnectIOLogService>().PropertiesAutowired();
            containerBuilder.RegisterType<DeviceAccessService>().As<IDeviceAccessService>().PropertiesAutowired();
            containerBuilder.RegisterType<TimeGroupService>().As<ITimeGroupService>().PropertiesAutowired();
            containerBuilder.RegisterType<CacheService>().As<ICacheService>().PropertiesAutowired();
            containerBuilder.RegisterType<HolidayService>().As<IHolidayService>().PropertiesAutowired();
            containerBuilder.RegisterType<AlarmClockService>().As<IAlarmClockService>().PropertiesAutowired();
            

            PeopleMapster.ConfigMapster();
            TimeGroupMapster.ConfigMapster();
            AlarmClockMapster.ConfigMapster();
            RecordMapster.ConfigMapster();
            HolidayMapster.ConfigMapster() ;

        }
    }
}