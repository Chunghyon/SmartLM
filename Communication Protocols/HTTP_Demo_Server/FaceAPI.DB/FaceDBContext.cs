using FaceWebServer.DB.Log;
using FaceWebServer.DB.Table;
using FaceWebServer.DB.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata;


namespace FaceWebServer.DB
{
    public class FaceDBContext : DbContext
    {
        private ILogger<FaceDBContext> _logger;
        private IOptionsMonitor<FaceDBOption> _option;
        public FaceDBContext(DbContextOptions<FaceDBContext> Options, 
            ILogger<FaceDBContext> logger,
            IOptionsMonitor<FaceDBOption> option) : base(Options)
        {
            _logger = logger;
            _option = option;
            //_logger.LogInformation("创建一个 FaceDBContext");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(_option.CurrentValue.ShowSQLLog)
            {
                optionsBuilder.UseLoggerFactory(new CustomEFLoggerFactory(_logger));
            }
            
        }

        /// <summary>
        /// 人员
        /// </summary>
        public DbSet<People> People { get; set; }

        /// <summary>
        /// 人员权限
        /// </summary>
        public DbSet<PeopleAccessDetail> PeopleAccessDetail { get; set; }



        /// <summary>
        /// 设备信息
        /// </summary>
        public DbSet<DeviceDetail> Device { get; set; }

        /// <summary>
        /// 闹铃表
        /// </summary>
        public DbSet<AlarmClock> AlarmClock { get; set; }

        /// <summary>
        /// 开门时段
        /// </summary>
        public DbSet<TimeGroupDetail> TimeGroupDetail { get; set; }


        /// <summary>
        /// 节假日
        /// </summary>
        public DbSet<Holiday> Holiday { get; set; }


        /// <summary>
        /// 远程任务
        /// </summary>
        public DbSet<RemoteTaskDetail> RemoteTaskDetail { get; set; }


        /// <summary>
        /// 打卡记录表
        /// </summary>
        public DbSet<IdentifyRecord> IdentifyRecord { get; set; }

        /// <summary>
        /// 系统记录
        /// </summary>
        public DbSet<SystemRecord> SystemRecord { get; set; }

        /// <summary>
        /// 界面UI菜单
        /// </summary>
        public DbSet<SystemMenuEntity> SystemMenus { get; set; }


        /// <summary>
        /// 网站管理员
        /// </summary>
        public DbSet<UserDetail> User { get; set; }

        /// <summary>
        /// 用户日志
        /// </summary>
        public DbSet<UserLogModel> UserLog { get; set; }


        /// <summary>
        /// 系统参数 键值对
        /// </summary>
        public DbSet<SystemKV> SystemKV { get; set; }

        /// <summary>
        /// 人脸机网络连接日志
        /// </summary>
        public DbSet<ConnectIOLog> ConnectIOLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<AlarmClock>()
            //    .Property(b => b.Num)
            //    .IsRequired()
            //    ;
        }
    }
}
