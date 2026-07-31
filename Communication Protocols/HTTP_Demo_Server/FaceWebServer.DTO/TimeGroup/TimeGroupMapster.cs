using FaceWebServer.DTO.HTTPv1_Protocol;
using FaceWebServer.DTO.HTTPv2_Protocol;
using FaceWebServer.DTO.People;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.TimeGroup
{
    /// <summary>
    /// 开门时段的Mapster配置
    /// </summary>
    public class TimeGroupMapster
    {
        public static void ConfigMapster()
        {
            TypeAdapterConfig<FaceWebServer.DB.Table.TimeGroupDetail, HTTPOpenDoorTimegroupV2>
                .NewConfig()
                .Map(dest => dest.Num,
                    src => src.GroupNum);

            TypeAdapterConfig<HTTPOpenDoorTimegroupV2, FaceWebServer.DB.Table.TimeGroupDetail>
                .NewConfig()
                .Map(dest => dest.GroupNum,
                    src => src.Num);

            TypeAdapterConfig<FaceWebServer.DB.Table.TimeGroupDetail, HTTPOpenDoorTimegroupV1>
                .NewConfig()
                .Map(dest => dest.num,
                    src => src.GroupNum)
                .Map(dest => dest.week1,
                    src => src.Week1.Replace("-","--"),
                    src => !string.IsNullOrEmpty(src.Week1))
                .Map(dest => dest.week2,
                    src => src.Week2.Replace("-", "--"),
                    src => !string.IsNullOrEmpty(src.Week2))
                .Map(dest => dest.week3,
                    src => src.Week3.Replace("-", "--"),
                    src => !string.IsNullOrEmpty(src.Week3))
                .Map(dest => dest.week4,
                    src => src.Week4.Replace("-", "--"),
                    src => !string.IsNullOrEmpty(src.Week4))
                .Map(dest => dest.week5,
                    src => src.Week5.Replace("-", "--"),
                    src => !string.IsNullOrEmpty(src.Week5))
                .Map(dest => dest.week6,
                    src => src.Week6.Replace("-", "--"),
                    src => !string.IsNullOrEmpty(src.Week6))
                .Map(dest => dest.week7,
                    src => src.Week7.Replace("-", "--"),
                    src => !string.IsNullOrEmpty(src.Week7))


                .Map(dest => dest.week1,
                    src => (string)null,
                    src => string.IsNullOrEmpty(src.Week1))
                .Map(dest => dest.week2,
                    src => (string)null,
                    src => string.IsNullOrEmpty(src.Week2))
                .Map(dest => dest.week3,
                    src => (string)null,
                    src => string.IsNullOrEmpty(src.Week3))
                .Map(dest => dest.week4,
                    src => (string)null,
                    src => string.IsNullOrEmpty(src.Week4))
                .Map(dest => dest.week5,
                    src => (string)null,
                    src => string.IsNullOrEmpty(src.Week5))
                .Map(dest => dest.week6,
                    src => (string)null,
                    src => string.IsNullOrEmpty(src.Week6))
                .Map(dest => dest.week7,
                    src => (string)null,
                    src => string.IsNullOrEmpty(src.Week7))
                ;

            TypeAdapterConfig< HTTPOpenDoorTimegroupV1, FaceWebServer.DB.Table.TimeGroupDetail>
                .NewConfig()
                .Map(dest => dest.GroupNum,
                    src => src.num)
                .Map(dest => dest.Week1,
                    src => src.week1.Replace("--", "-"),
                    src => !string.IsNullOrEmpty(src.week1))
                .Map(dest => dest.Week2,
                    src => src.week2.Replace("--", "-"),
                    src => !string.IsNullOrEmpty(src.week2))
                .Map(dest => dest.Week3,
                    src => src.week3.Replace("--", "-"),
                    src => !string.IsNullOrEmpty(src.week3))
                .Map(dest => dest.Week4,
                    src => src.week4.Replace("--", "-"),
                    src => !string.IsNullOrEmpty(src.week4))
                .Map(dest => dest.Week5,
                    src => src.week5.Replace("--", "-"),
                    src => !string.IsNullOrEmpty(src.week5))
                .Map(dest => dest.Week6,
                    src => src.week6.Replace("--", "-"),
                    src => !string.IsNullOrEmpty(src.week6))
                .Map(dest => dest.Week7,
                    src => src.week7.Replace("--", "-"),
                    src => !string.IsNullOrEmpty(src.week7))

                ;
        }
    }
}
