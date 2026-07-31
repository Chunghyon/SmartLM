using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DB.Table
{
    /// <summary>
    /// 人员权限表
    /// </summary>
    [Table("PeopleAccessDetail")]
    public class PeopleAccessDetail
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccessID { get; set; }
        #region 人员信息
        /// <summary>
        /// 人员ID
        /// </summary>
        public int PeopleID { get; set; }

        /// <summary>
        /// 用户号
        /// </summary>
        public long UserID { get; set; }
        #endregion

        #region 设备信息
        /// <summary>
        /// 设备ID
        /// </summary>
        public int DeviceID { get; set; }
        #endregion




        #region 权限信息
        /// <summary>
        /// 人员角色 0,普通人员；1，管理员;2 黑名单
        /// </summary>
        public int AccessType { get; set; }

        /// <summary>
        /// 截止日期  unix 时间戳 秒级  
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// 开门次数 0-65535；  65535--表示无限制，0--表示禁止通行
        /// </summary>
        public int OpenTimes { get; set; }


        /// <summary>
        /// 是否为常开卡，1--是；0--否
        /// </summary>
        public int KeepOpen { get; set; }

        /// <summary>
        /// 开门时段组号
        /// </summary>
        public int Timegroup { get; set; }

        /// <summary>
        /// 节假日受限，逗号分隔：1,2,3,4,5
        /// </summary>
        public string Holidays { get; set; }

        /// <summary>
        /// 电梯权限，逗号分隔：1,2,3,4,5
        /// </summary>
        public string Elevators { get; set; }
        #endregion



        #region 状态信息
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime LastUpdatetime { get; set; }


        #endregion

        #region 上传状态
        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UploadTime { get; set; }

        /// <summary>
        /// 上传状态：0--未上传；1--已上传；2--待删除
        /// </summary>
        public int UploadStatus { get; set; }

        /// <summary>
        /// 上传结果： 0 --无操作；1--正常；
        /// HTTPv1 返回值为 100-999  HTTPv2 返回值为 1000-1999
        /// </summary>
        public int UploadResult { get; set; }

        /// <summary>
        /// 重复人员编号：如果是上传后，照片重复时，此ID记录跟谁重复
        /// </summary>
        public long RepeatID { get; set; }

        /// <summary>
        /// 上传发生异常时，设备返回的异常描述
        /// </summary>
        public string? UploadResultMsg { get; set; }
        #endregion


        public PeopleAccessDetail()
        {
            CreateTime = DateTime.Now;
            UploadTime = DateTime.Now;
            LastUpdatetime = DateTime.Now;
        }

    }
}
