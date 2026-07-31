using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaceWebServer.DB.Table;
using FaceWebServer.DTO.HTTPv1_Protocol;
using FaceWebServer.DTO.HTTPv2_Protocol;
using FaceWebServer.DTO.People;
using FaceWebServer.Utility;
using Mapster;
using Newtonsoft.Json;


namespace FaceWebServer.DTO.Record
{
    public static class RecordMapster
    {
        public static void ConfigMapster()
        {
            //HTTPRecordUploadIdentifyRecordDetail 转 IdentifyRecord  
            TypeAdapterConfig<HTTPRecordUploadIdentifyRecordDetail, IdentifyRecord>
                .NewConfig()
                .Map(dest => dest.RecordDate,
                    src => TimestampUtility.ToLocalTimeDateBySeconds(src.RecordDate))
                ;

            //HTTPRecordUploadSystemRecordDetail 转 SystemRecord  
            TypeAdapterConfig<HTTPRecordUploadSystemRecordDetail, SystemRecord>
                .NewConfig()
                .Map(dest => dest.RecordDate,
                    src => TimestampUtility.ToLocalTimeDateBySeconds(src.RecordDate))
                ;



            //HTTPPushRecordDetail 转 IdentifyRecord  
            TypeAdapterConfig<HTTPPushRecordDetail, IdentifyRecord>
                .NewConfig()
                .Map(dest => dest.RecordID, src => TimestampUtility.ToUnixTimestampBySeconds(src.NoteTime))
                .Map(dest => dest.RecordType, src => src.EventType)
                .Map(dest => dest.RecordDate, src => src.NoteTime)
                .Map(dest => dest.SN, src => src.DeviceID)
                .Map(dest => dest.UserID, src => src.EmployeeID)
                .Map(dest => dest.Name, src => src.EmployeeName)
                .Map(dest => dest.Job, src => string.Empty)
                .Map(dest => dest.Department, src => string.Empty)
                .Map(dest => dest.IdentityCard, src => string.Empty)
                .Map(dest => dest.CardNum, src => src.CardNo)
                .Map(dest => dest.QRCode, src => src.QRCode)
                .Map(dest => dest.BodyTemp, src => src.HumTemp)
                .Map(dest => dest.PhotoLen, src => src.Pic_Len)
                .Map(dest => dest.Photo, src => src.ImgURL)
                ;

            //HTTPPushRecordDetail 转 SystemRecord  
            TypeAdapterConfig<HTTPPushRecordDetail, SystemRecord>
                .NewConfig()
                .Map(dest => dest.RecordID, src => TimestampUtility.ToUnixTimestampBySeconds(src.NoteTime))
                .Map(dest => dest.RecordType, src => src.EventType)
                .Map(dest => dest.RecordDate, src => src.NoteTime)
                .Map(dest => dest.SN, src => src.DeviceID)
                ;
        }
    }
}
