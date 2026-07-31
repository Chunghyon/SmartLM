using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DB.Table
{
    [Table("Holiday")]
    public class Holiday
    {
        /// <summary>
        /// 节假日的序号，人员权限绑定时使用
        /// </summary>
        [Key]
        [Required]
        public int Num { get; set; }


        /// <summary>
        /// 节假日日期 年-月-日 示例：2020-10-01
        /// </summary>
        public string Date { get; set; }


        /// <summary>
        /// 节假日管控范围，1--全天；2--上午 00:00-12:00;  3--下午(12:00-23:59),默认值为1；
        /// </summary>
        public int HolidayType { get; set; }


        /// <summary>
        /// 是否每年循环，1--每年循环；0--不循环；默认值为0；
        /// </summary>
        public int Cycle { get; set; }
    }
}
