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
    /// 开门时段
    /// </summary>
    [Table("TimeGroup")]
    public class TimeGroupDetail
    {

        public TimeGroupDetail()
        {
        }

        public TimeGroupDetail(int v)
        {
            GroupNum = v;
        }

        [Key]
        [Required]
        public int GroupNum { get; set; }
        /// <summary>
        /// 星期一
        /// </summary>
        public string Week1 { get; set; }
        /// <summary>
        /// 星期二
        /// </summary>
        public string Week2 { get; set; }
        /// <summary>
        /// 星期三
        /// </summary>
        public string Week3 { get; set; }
        /// <summary>
        /// 星期四
        /// </summary>
        public string Week4 { get; set; }
        /// <summary>
        /// 星期五
        /// </summary>
        public string Week5 { get; set; }
        /// <summary>
        /// 星期六
        /// </summary>
        public string Week6 { get; set; }
        /// <summary>
        /// 星期日
        /// </summary>
        public string Week7 { get; set; }
    }
}
