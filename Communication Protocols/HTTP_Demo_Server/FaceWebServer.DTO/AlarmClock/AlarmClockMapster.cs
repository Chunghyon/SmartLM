using FaceWebServer.DTO.HTTPv2_Protocol;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.AlarmClock
{
    /// <summary>
    /// 闹铃表的Mapster配置
    /// </summary>
    public class AlarmClockMapster
    {
        public static void ConfigMapster()
        {
            TypeAdapterConfig<FaceWebServer.DB.Table.AlarmClock, HTTPAlarmClockTime>
                .NewConfig()
                .Map(dest => dest.Clock,
                    src => src.Date.ToString("HH:mm"));

        }
    }
}
