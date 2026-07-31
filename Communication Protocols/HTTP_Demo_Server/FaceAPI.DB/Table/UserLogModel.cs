using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaceWebServer.DB.Table
{
    [Table("UserLog")]
    public class UserLogModel
    {

        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogID { get; set; }


        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
        public int UserID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 日志时间
        /// </summary>
        [Required]
        public DateTime LogTime { get; set; }


        /// <summary>
        /// 日志类型
        /// </summary>
        [Required]
        public string LogType { get; set; }

        /// <summary>
        /// 设备信息
        /// </summary>
        [Required]
        public string LogDrive { get; set; }

        /// <summary>
        /// 人员信息
        /// </summary>
        [Required]
        public string LogPeople { get; set; }

        /// <summary>
        /// 详情
        /// </summary>
        [Required]
        public string LogDetail { get; set; }

        public UserLogModel() { }

        public UserLogModel(UserDetail user, string sType, string sDetail)
        {
            UserID = user.UserID;
            UserName = user.UserName;
            LogTime = DateTime.Now;
            LogType = sType;
            LogDetail = sDetail;

            LogPeople = string.Empty;
            LogDrive = string.Empty;
        }

    }
}
