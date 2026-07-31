using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using FaceWebServer.DB.Table;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using FaceWebServer.DTO.HTTPv2_Protocol;
using FaceWebServer.Utility;
using FaceWebServer.DTO.HTTPv1_Protocol;

namespace FaceWebServer.DTO.People
{
    public class PeopleMapster
    {
        public static void ConfigMapster()
        {
            //DB.Table.People 转 PeopleDTO  
            TypeAdapterConfig<FaceWebServer.DB.Table.People, PeopleDTO>
                .NewConfig()
                .Map(dest => dest.FaceFeature,
                    src => (PeopleFeatureCode)null,
                    src => string.IsNullOrEmpty(src.FaceFeature))
                .Map(dest => dest.FaceFeature,
                    src => JsonConvert.DeserializeObject<PeopleFeatureCode>(src.FaceFeature),
                    src => !string.IsNullOrEmpty(src.FaceFeature))

                .Map(dest => dest.Fingerprints,
                    src => (List<PeopleFeatureCode>)null,
                    src => string.IsNullOrEmpty(src.Fingerprints))
                .Map(dest => dest.Fingerprints,
                    src => JsonConvert.DeserializeObject<List<PeopleFeatureCode>>(src.Fingerprints),
                    src => !string.IsNullOrEmpty(src.Fingerprints))

                .Map(dest => dest.Palmveins,
                    src => (List<PeopleFeatureCode>)null,
                    src => string.IsNullOrEmpty(src.Palmveins))
                .Map(dest => dest.Palmveins,
                    src => JsonConvert.DeserializeObject<List<PeopleFeatureCode>>(src.Palmveins),
                    src => !string.IsNullOrEmpty(src.Palmveins));

            //PeopleDTO 转 DB.Table.People
            TypeAdapterConfig<PeopleDTO, FaceWebServer.DB.Table.People>
                .NewConfig()
                .Map(dest => dest.FaceFeature,
                    src => string.Empty,
                    src => src.FaceFeature == null)
                .Map(dest => dest.FaceFeature,
                    src => JsonConvert.SerializeObject(src.FaceFeature),
                    src => src.FaceFeature != null)

                .Map(dest => dest.Fingerprints,
                    src => string.Empty,
                    src => src.Fingerprints == null)
                .Map(dest => dest.Fingerprints,
                    src => JsonConvert.SerializeObject(src.Fingerprints),
                    src => src.Fingerprints != null)

                .Map(dest => dest.Palmveins,
                    src => string.Empty,
                    src => src.Palmveins == null)
                .Map(dest => dest.Palmveins,
                    src => JsonConvert.SerializeObject(src.Palmveins),
                    src => src.Palmveins != null)
                .AfterMapping((src, dest) =>
                {
                    if (src.FaceFeature == null)
                    {
                        dest.FaceNum = 0;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(src.FaceFeature.Data))
                        {
                            dest.FaceFeature = string.Empty;
                            dest.FaceNum = 0;
                        }
                        else
                        {
                            dest.FaceNum = 1;
                        }
                    }

                    if (src.Fingerprints == null)
                    {
                        dest.FingerprintsNum = 0;
                    }
                    else
                    {
                        if (src.Fingerprints.Count == 0)
                        {
                            dest.Fingerprints = string.Empty;
                            dest.FingerprintsNum = 0;
                        }
                        else
                        {
                            dest.FingerprintsNum = src.Fingerprints.Count;
                        }
                    }


                    if (src.Palmveins == null)
                    {
                        dest.PalmveinsNum = 0;
                    }
                    else
                    {
                        if (src.Palmveins.Count == 0)
                        {
                            dest.Palmveins = string.Empty;
                            dest.PalmveinsNum = 0;
                        }
                        else
                        {
                            dest.PalmveinsNum = src.Palmveins.Count;
                        }
                    }

                });


            //DB.Table.People 转 HTTPPeopleV2  
            TypeAdapterConfig<FaceWebServer.DB.Table.People, HTTPPeopleV2>
                .NewConfig()
                .Map(dest => dest.FaceFeature,
                    src => (string)null,
                    src => string.IsNullOrEmpty(src.FaceFeature))
                .Map(dest => dest.FaceFeature,
                    src => JsonConvert.DeserializeObject<PeopleFeatureCode>(src.FaceFeature).Data,
                    src => !string.IsNullOrEmpty(src.FaceFeature))

                 .Map(dest => dest.FaceFeatureMD5,
                    src => (string)null,
                    src => string.IsNullOrEmpty(src.FaceFeature))
                .Map(dest => dest.FaceFeatureMD5,
                    src => JsonConvert.DeserializeObject<PeopleFeatureCode>(src.FaceFeature).MD5,
                    src => !string.IsNullOrEmpty(src.FaceFeature))


                .Map(dest => dest.Fingerprints,
                    src => (List<HTTPPeopleFeatureCode>)null,
                    src => string.IsNullOrEmpty(src.Fingerprints))
                .Map(dest => dest.Fingerprints,
                    src => JsonConvert.DeserializeObject<List<HTTPPeopleFeatureCode>>(src.Fingerprints),
                    src => !string.IsNullOrEmpty(src.Fingerprints))

                .Map(dest => dest.Palmveins,
                    src => (List<HTTPPeopleFeatureCode>)null,
                    src => string.IsNullOrEmpty(src.Palmveins))
                .Map(dest => dest.Palmveins,
                    src => JsonConvert.DeserializeObject<List<HTTPPeopleFeatureCode>>(src.Palmveins),
                    src => !string.IsNullOrEmpty(src.Palmveins));



            //HTTPPeopleV2 转 DB.Table.People
            TypeAdapterConfig<HTTPPeopleV2, FaceWebServer.DB.Table.People>
                .NewConfig()
                .Map(dest => dest.FaceFeature,
                    src => string.Empty,
                    src => src.FaceFeature == null)
                .Map(dest => dest.FaceFeature,
                    src => JsonConvert.SerializeObject(new HTTPPeopleFeatureCode()
                    {
                        Num = 1,
                        Data = src.FaceFeature,
                        MD5 = src.FaceFeatureMD5
                    }),
                    src => !string.IsNullOrEmpty(src.FaceFeature))

                .Map(dest => dest.Fingerprints,
                    src => string.Empty,
                    src => src.Fingerprints == null)
                .Map(dest => dest.Fingerprints,
                    src => JsonConvert.SerializeObject(src.Fingerprints),
                    src => src.Fingerprints != null)

                .Map(dest => dest.Palmveins,
                    src => string.Empty,
                    src => src.Palmveins == null)
                .Map(dest => dest.Palmveins,
                    src => JsonConvert.SerializeObject(src.Palmveins),
                    src => src.Palmveins != null)
                .AfterMapping((src, dest) =>
                {
                    if (src.FaceFeature == null)
                    {
                        dest.FaceNum = 0;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(src.FaceFeature))
                        {
                            dest.FaceFeature = string.Empty;
                            dest.FaceNum = 0;
                        }
                        else
                        {
                            dest.FaceNum = 1;
                        }
                    }

                    if (src.Fingerprints == null)
                    {
                        dest.FingerprintsNum = 0;
                    }
                    else
                    {
                        if (src.Fingerprints.Count == 0)
                        {
                            dest.Fingerprints = string.Empty;
                            dest.FingerprintsNum = 0;
                        }
                        else
                        {
                            dest.FingerprintsNum = src.Fingerprints.Count;
                        }
                    }


                    if (src.Palmveins == null)
                    {
                        dest.PalmveinsNum = 0;
                    }
                    else
                    {
                        if (src.Palmveins.Count == 0)
                        {
                            dest.Palmveins = string.Empty;
                            dest.PalmveinsNum = 0;
                        }
                        else
                        {
                            dest.PalmveinsNum = src.Palmveins.Count;
                        }
                    }

                });


            TypeAdapterConfig<FaceWebServer.DB.Table.PeopleAccessDetail, HTTPPeopleV2>
                .NewConfig()
                .Map(dest => dest.ExpirationDate,
                    src => TimestampUtility.ToUnixTimestampBySeconds(src.ExpirationDate));




            //DB.Table.People 转 HTTPPeopleV2  
            TypeAdapterConfig<FaceWebServer.DB.Table.People, HTTPv1_Protocol.HTTPPeopleV1>
                .NewConfig()
                .Map(dest => dest.EmployeeID, src => src.UserID)
                .Map(dest => dest.EmployeeName, src => src.Name)
                .Map(dest => dest.EmployeeJob, src => src.Job)
                .Map(dest => dest.EmployeeIdentity, src => src.IdentityCard)
                .Map(dest => dest.EmployeePassword, src => src.Password)
                .Map(dest => dest.EmployeeIc, src => src.CardNum)
                .Map(dest => dest.EmployeePhotoWay, src => "path")
                .Map(dest => dest.EmployeePhoto, src => src.Photo)
                .Map(dest => dest.PhotoMD5, src => src.PhotoMD5)
            ;

            //DB.Table.PeopleAccessDetail 转 HTTPPeopleV2  
            TypeAdapterConfig<FaceWebServer.DB.Table.PeopleAccessDetail, HTTPv1_Protocol.HTTPPeopleV1>
                .NewConfig()
                .Map(dest => dest.EmployeeRoot, src => src.AccessType)
                .Map(dest => dest.TimegroupID, src => src.Timegroup)
                .Map(dest => dest.EmployeeShold, src => 0)
                .Map(dest => dest.DevicePassBean, src => new HTTPPeopleV1_AccessDetail()
                {
                    DevicePassStart = DateTime.Now.AddYears(-1),
                    DevicePassEnd = src.ExpirationDate,
                    DevicePassTimeOver = 1,
                    DevicePassNumber = src.OpenTimes == 65535 ? 0 : src.OpenTimes
                })
            ;
        }
    }
}
