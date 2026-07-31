using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaceWebServer.DB.Table
{
    /// <summary>
    /// 后台操作员
    /// </summary>
    [Table("User")]
    public class UserDetail
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Required]
        public string UserPassword { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Required]
        public string Phone { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        [Required]
        public string Role { get; set; }


        /// <summary>
        /// 最近登录时间
        /// </summary>
        [Required]
        public DateTime LogTime { get; set; }

        /// <summary>
        /// 最近在线时间
        /// </summary>
        [Required]
        public DateTime OnlineTime { get; set; }

    }
}
