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
    /// 闹铃表
    /// </summary>
    [Table("AlarmClock")]
    public class AlarmClock
    {
        //闹铃序号
        [Key]
        [Required]
        public int Num { get; set; }

        //闹铃时间
        public DateTime Date { get; set; }

        //闹铃响铃时长,单位分秒
        public int Times { get; set; }

    }
}
