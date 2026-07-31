using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaceWebServer.DTO.HTTPv2_Protocol;
using Mapster;


namespace FaceWebServer.DTO
{

    /// <summary>
    /// 节假日的Mapster配置
    /// </summary>
    public class HolidayMapster
    {
        public static void ConfigMapster() {
            TypeAdapterConfig<FaceWebServer.DB.Table.Holiday, HTTPDeviceHolidayDay>
                .NewConfig()
                .Map(dest => dest.Type,
                    src => src.HolidayType);

            TypeAdapterConfig<HTTPDeviceHolidayDay, FaceWebServer.DB.Table.Holiday>
                .NewConfig()
                .Map(dest => dest.HolidayType,
                    src => src.Type);

        }
    }
}
